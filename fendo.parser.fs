.( fendo.parser.fs ) cr

\ This file is part of Fendo.

\ This file creates the parser.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ **************************************************************
\ Change history of this file

\ See at the end of the file.

\ **************************************************************
\ Todo

\ 2013-07-20: redirect the parser output to a string. This will
\ let to parse the link texts that have markups!
\
\ 2013-06-04: Flag the first markup of the current line, in
\   order to use '--' both for nested lists and for delete.

\ **************************************************************
\ Requirements

get-current  forth-wordlist set-current

require galope/n-to-r.fs  \ 'n>r'
require galope/n-r-from.fs  \ 'nr>'
require galope/tilde-tilde.fs  \ '~~'

set-current

\ **************************************************************
\ Pending markups

: close_pending_bullet_list  ( -- )
  \ Close a pending bullet list.
  [markup>order] </li> </ul> [markup<order]  bullet_list_items off
  ;
: close_pending_numbered_list  ( -- )
  \ Close a pending numbered list.
  [markup>order] </li> </ol> [markup<order]  numbered_list_items off
  ;
: close_pending_list  ( -- )
  \ Close a pending list, if needed.
  \ xxx todo maybe an improvement will be required for nested lists
  bullet_list_items @ if  close_pending_bullet_list  then
  numbered_list_items @ if  close_pending_numbered_list  then
  ;
: close_pending_heading  ( -- )
  \ Close a pending heading, if needed.
  \ xxx not used yet
  opened_[=]? @ if  [markup>order] = [markup<order]  then
  ;
: close_pending_paragraph  ( -- )
  \ Close a pending paragraph, if needed.
  opened_[_]? @ if  [markup>order] _
\  ." CLOSED PARAGRAPH " cr  \ xxx informer
  [markup<order]  then
  ;
: close_pending_table  ( -- )
  \ Close a pending table, if needed.
  #rows @ if
    [markup>order] </tr> </table> [markup<order]
    #rows off  #cells off
  then
  ;
: close_pending  ( -- )
  \ Close the pending markups.
  \ Invoked when an empty line if parsed, and at the end of the
  \ parsing.
\  ." close_pending because #nothings = " #nothings @ . cr  \ xxx informer
  close_pending_list close_pending_paragraph close_pending_table echo_cr
  ;

\ **************************************************************
\ Parser

variable more?  \ flag: keep on parsing more words?; changed by '}content'

:noname  ( ca len -- )
  \ Manage a parsed string of content: print it and update the counters.
\  ." content! " order cr  \ xxx informer
  #markups off  _echo  1 #nonmarkups +!
  ;  is content
: (markup)  ( xt -- )
  \ Manage a parsed markup: execute it and update the counters.
  \ xt = execution token of the markup
  #nonmarkups off  execute  1 #markups +!
  ;
: markup  ( ca len xt -- )
  \ Manage a parsed markup: execute it and update the counters,
  \ if required.
  \ ca len = name of the markup
  \ xt = execution token of the markup
\  ." markup! " order cr  \ xxx informer
  execute_markup? @
  if  nip nip (markup)  else  drop content  then
  ;
: found?  ( ca len -- xt | 0 )
  \ Is the given string found in the current wordlists?
  find-name dup if  name>int  then
  ;
: markup?  ( ca len -- xt | 0 )
  \ Is the given string any kind of markup
  \ (wiki markup, HTML entity or user macro)?
\  cr ." markup? " .s key drop  \ xxx informer
  get-order n>r
\  cr ." markup? 0 " .s key drop  \ xxx informer
  set_markup_order found?
\  cr ." markup? 1 " .s key drop  \ xxx informer
  nr> set-order
\  cr ." markup? 2 " .s key drop  \ xxx informer
  ;
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content)
\  2dup ." something= " type space order cr  \ xxx informer
\  depth 2 > abort" something wrong!"  \ xxx informer
  2dup parsed$ $!
  #nothings off
\  ." #nothings = " #nothings @ . cr  \ xxx informer
  2dup markup? ?dup if  markup  else  content  then  1 #parsed +!
  ;

0 [if]
\ xxx old, abandoned on 2014-02-05, in order to implement the user macros
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content)
\  2dup cr type space  \ xxx informer
\  depth 2 > abort" something wrong!"  \ xxx informer
  #nothings off
\  ." #nothings = " #nothings @ . cr  \ xxx informer
  2dup fendo_markup_wid search-wordlist
  if  markup
  else
    2dup fendo_markup_html_entities_wid search-wordlist
    if  markup  else  content  then
  then
  1 #parsed +!
  ;
[then]
0 [if]

\ 2013-08-10: experimental new version, with direct execution of
\ Forth code; unfinished

: something_in_code_zone  ( ca len -- )  \ xxx todo
  \ Manage something found in a Forth code zone.
  \ ca len = parsed item
  2dup fendo_markup_wid search-wordlist
  if  markup
  else
    2dup fendo_markup_html_entities_wid search-wordlist
    if  markup  else  content  then
  then
  1 #parsed +!
  ;
: something_in_ordinary_zone  ( ca len -- )  \ xxx todo
  \ Manage something found out of Forth code zones.
  \ ca len = parsed item (markup or printable content)
  2dup fendo_markup_wid search-wordlist
  if  markup
  else
    2dup fendo_markup_html_entities_wid search-wordlist
    if  markup  else  content  then
  then
  1 #parsed +!
  ;
: something  ( ca len -- )  \ xxx todo
  \ Manage something found on the page content.
  \ ca len = parsed item (markup, Forth code or printable content)
  #nothings off
\  ." #nothings = " #nothings @ . cr  \ xxx informer
  forth_code_depth @
  if    something_in_code_zone
  else  something_in_ordinary_zone  then
  ;
[then]

: nothing  ( -- )
  \ Manage a "nothing", a parsed empty name.
  \ The first empty name means the current line is finished;
  \ the second consecutive empty name means the current line is empty.
\  cr ." nothing"  \ xxx informer
  preserve_eol? @ if  echo_cr  then  \ xxx tmp
  #nothings @  \ an empty line was parsed?
  if    close_pending
  then  1 #nothings +!
\  ." #nothings = " #nothings @ . cr  \ xxx informer
  #parsed off
  ;
: parse_content  ( "text" -- )
  \ Parse the current input source.
  \ The process is finished by the '}content' markup or the end
  \ of the source.
\  order key drop  \ xxx informer
  separate? off  more? on
  begin
    parse-name ?dup
    if    something more? @
    else  drop nothing refill
    then  0=
  until
  ;

: (evaluate_content)  ( ca len -- )
  \ Evaluate a string as page content.
  ['] parse_content execute-parsing  #nothings off
  ;
' (evaluate_content) is evaluate_content
: more_link_text?  ( ca len -- wf )
  \ Manage a parsed name of a link text.
  2dup end_of_link_section?
  if  2drop false  else  something true  then
  ;
: parsed_link_text  ( "text<spaces>|<spaces>" | "text<spaces>]]<spaces>"  -- ca len )
  \ Parse and return the link text.
  echo> @  echo>string
  >attributes< -attributes  \ use the alternative set and init it
  separate? off
  begin   parse-name dup
    if    more_link_text?
    else  2drop more_link?
    then  0=
  until   echo> ! >attributes<  echoed $@
\  2dup ." result of parsed_link_text = " type cr  \ xxx informer
  ;
: (parse_link_text)  ( "...<space>|<space>" | "...<space>]]<space>"  -- )
  \ Parse the link text and store it into 'link_text'.
  s" " link_text!  \ xxx needed?
  parsed_link_text
\  ." link_text in (parse_link_text) = " 2dup type cr  \ xxx informer  \ xxx informer
  link_text!
  ;
' (parse_link_text) is parse_link_text

0 [if]  \ xxx tmp moved to fendo_files.fs

\ Target file

: (open_target)  ( -- )
  \ Open the target HTML page file.
  current_page
\  cr ." current_page in (open_target) =  " dup .  \ xxx informer
  target_path/file
\  cr ." target file =  " 2dup type cr key drop  \ xxx informer
  w/o create-file throw target_fid !
\  ." target file just opened: "  \ xxx informer
\  target_fid @ . cr  \ xxx informer
\  s" <!-- xxx -->" target_fid @ write-line throw  \ xxx debugging
  ;
: open_target  ( -- )
  \ Open the target HTML page file, if needed.
  echo>file? if  (open_target)  then
  ;
: (close_target)  ( -- )
  \ Close the target HTML page file.
\  depth abort" stack not empty"  \ xxx informer
  target_fid @ close-file throw
\  ." target_fid just closed. " \ xxx informer
  target_fid off
  ;
: close_target  ( -- )
  \ Close the target HTML page file, if needed.
\  ." close_target" cr \ xxx informer
  target_fid @ if  (close_target)  then
  ;

[then]

\ Design template

: template_file  ( -- ca len )
  \ Return the full path to the template file.
  target_dir $@
  [ true ] [if]  \ xxx new version
    current_page design_subdir dup 0=  \ xxx useful?
    if  2drop website_design_subdir $@  then
    current_page template dup 0=  \ xxx individual page templates are useful
    if  2drop website_template $@ then  s+ s+
  [else]  \ xxx old version, without page fields
    website_design_subdir $@ website_template $@ s+ s+
  [then]
\  ." template_file = " 2dup type cr  \ xxx informer
  ;
: template_halves  ( ca1 len1 -- ca2 len2 ca3 len3 )
  \ Divide the template in two parts, excluding the content holder.
  \ ca1 len1 = template content
  \ ca2 len2 = top half of the template content
  \ ca3 len3 = bottom half of the template content
  content_markup $@ /sides 0=
  abort" The content markup is missing in the template"
  ;
: template_top  ( ca1 len1 -- ca2 len2 )
  \ Extract the top half of a template, above the page content.
  \ ca1 len1 = template content
  \ ca2 len2 = top half of the template content
  template_halves 2drop
  ;
: template_bottom  ( ca1 len1 -- ca2 len2 )
  \ Extract the bottom half of a template, above the page content.
  \ ca1 len1 = template content
  \ ca2 len2 = bottom half of the template content
  template_halves 2nip
  ;
false [if]  \ xxx first method; the template file is read only once
variable template_content
: get_template_first  ( -- ca len )
  \ Return the template content, the first time.
  template_file slurp-file 2dup template_content $!
  ;
: get_template_again  ( -- ca len )
  \ Return the template content, the second time.
  template_content $@  template_content $off
  ;
: get_template  ( -- ca len )
  \ Return the template content.
  template_content $@len
  if  get_template_again  else  get_template_first  then
\  .s  \ xxx informer
  ;
[else]  \ xxx second method; the template file is read twice
\ xxx 2013-10-27: this alternative is tried in order to see
\ its effect on the strange corruption of the input stream.
: get_template  ( -- ca len )
  \ Return the template content.
  template_file slurp-file
  ;
[then]
: template{  ( -- )
  \ Echo the top half of the current template,
  \ above the page content.
  get_template template_top evaluate_content
  ;
: }template  ( -- )
  \ Echo the bottom half of the current template,
  \ below the page content.
  get_template template_bottom evaluate_content
  ;

\ Markup

: .sourcefilename  ( -- )
  \ Print the name of the currently parsed file.
  sourcefilename type cr
\ ."  XXX stack check: " .s  \ xxx informer
  depth abort" Stack not empty"
\  depth if  cr ." Stack not empty" cr .s quit  then  \ xxx informer
  ;
variable }content?  \ flag: was '}content' executed?
: (content{)  ( -- )
  \ Create the top template part of the target page
  \ and parse the page content.
\  ~~  \ xxx informer
  opened_markups_off
  open_target template{
  }content? off  parse_content
  }content? @ 0= abort" Missing '}content' at the end of the page"
\  ~~  \ xxx informer
  ;
: do_page?  ( -- wf )
\  current_page draft? if  ." DRAFT!" cr  then  \ xxx informer
  do_content? @  current_page draft? 0=  and
\  do_content? @   \ xxx tmp
  ;
: skip_page  ( -- )
  \ No target page must be created; skip the current source page.
\  ." skip_page" cr  \ xxx informer
  \eof  \ skip the rest of the file
  do_content? on  \ set default for the next page
  ;
: content{  ( "text }content" -- )
  \ Create the page content, if needed.
  \ The end of the content is marked with the '}content' markup.
  \ Only one 'content{ ... }content' block is allowed in the page.
\  ." content{" cr  \ xxx informer
\  ~~  \ xxx informer
  do_page?
\  ~~  \ xxx informer
  if  .sourcefilename
\    ." content{" cr  \ xxx informer
\    ~~  \ xxx informer
    (content{)
  else
    skip_page
\    ~~  \ xxx informer
  then
  ;
: finish_the_target  ( -- )
  close_pending }template close_target
  more? off  \ finish the current parsing
  ;
get-current markup>current
: }content  ( -- )
  \ Finish the page content.
\ cr .s cr ." start of }content " \ xxx informer
  finish_the_target
\  do_content? on  \ default value for the next page  \ xxx old
  only fendo>order forth>order
  }content? on
\ cr .s cr ." end of }content"  \ xxx informer
  ;
set-current

\ **************************************************************
\ Change history of this file

\ 2013-04-28: Start.

\ 2013-05-18: New: parser; 'skip_content{'.

\ 2013-06-01: New: parser rewritten from scratch. Management of empty
\ names and empty lines.

\ 2013-06-02: New: counters for both types of elements (markups and
\ printable words); required in order to separate words.

\ 2013-06-04: Fix: lists were not properly closed by an empty space.

\ 2013-06-05: Fix: 'markup' now uses a name token; this was required
\ in order to define '~', a markup that parses the next name is the
\ source.

\ 2013-06-05: New: '#parsed', required for implementing the table
\ markup.

\ 2013-06-05: Change: clearer code for closing the pending markups.

\ 2013-06-06: Change: renamed from "fendo_content.fs" to
\ "fendo_parser.fs".

\ 2013-06-06: New: template implemented.

\ 2013-06-08: Improved: Template management.

\ 2013-06-08: New: first code for output redirection.

\ 2013-06-08: New: first implementation of target directories.

\ 2013-06-11: Fix: the target file is opened and closed depending on
\ the 'dry?' config variable.

\ 2013-06-11: Fix: typo in comment of 'template_halves'.

\ 2013-06-16: Change: The parser has been rewritten; now
\ 'search-wordlist' is used instead of 'parse-name' and 'find-name';
\ this was needed to avoid matches in the Root wordlist.

\ 2013-06-16: Fix: Now '}content' toggles the parsing off and sets the
\ normal wordlist order.

\ 2013-06-23: Change: design and template variables are renamed after
\ the changes in the config module.

\ 2013-06-28: Change: '$@' is no longer required by metadata fields,
\ after Fendo A-01.

\ 2013-07-03: Change: 'dry?' renamed to 'echo>screen?', after the
\ changes in the echo module.

\ 2013-07-03: Change: words that check the current echo have been
\ renamed, after the  changes in the echo module.

\ 2013-07-28: New: 'parse_link_text' moved here from
\ <fendo_markup_wiki.fs>.

\ 2013-07-28: Fix: old '[previous]' changed to '[markup<order]'; this
\ was the reason the so many wordlists remained in the search order.

\ 2013-08-10: Fix: wrong exit flags in 'parsed_link_text' caused only
\ the first word was parsed.

\ 2013-08-10: Fix: the alternative attributes set was needed in
\ 'parsed_link_text', to preserve the current attributes already used
\ for the <a> tag.

\ 2013-08-10: Change: 'parse_string' renamed to 'evaluate_content'.

\ 2013-08-14: Fix: '#nothings off' was needed at the end of
\ '(evaluate_content)'. Otherwise '#nothings' activated
\ 'close_pending' before expected, e.g. this happened when a link was
\ at the end of a line.

\ 2013-09-06: New: 'do_page?'.

\ 2013-09-06: Fix: 'content{' doesn't call 'skip_content' anymore but
\ '\eof'; the reason is 'skip_content' parsed until "}content" was
\ found, what was wrong when this word was mentioned in the content
\ itself! That happened in one page and was hard to solve. It's
\ simpler to ignore the whole file. 'skip_content' has been removed.

\ 2013-09-29: New: '}content?' flag is used to check if '}content' was
\ executed; this way some markup errors can be detected.

\ 2013-12-06: New: 'opened_markups_off' in '(content{)'.

\ 2014-03-09: New: 'parsed$' keeps the string in 'something'; required
\ by the new way the heading wiki markups work.

.( fendo.parser.fs compiled ) cr
