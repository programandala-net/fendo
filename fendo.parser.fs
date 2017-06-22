.( fendo.parser.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file creates the parser.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Requirements

forth_definitions

require galope/n-to-r.fs  \ 'n>r'
require galope/n-r-from.fs  \ 'nr>'
require galope/tilde-tilde.fs  \ '~~'

fendo_definitions

\ ==============================================================
\ Pending markups

: close_pending_bullet_list ( -- )
  [</li>] [</ul>] bullet_list_items off ;
  \ Close a pending bullet list.

: close_pending_numbered_list ( -- )
  [</li>] [</ol>] numbered_list_items off ;
  \ Close a pending numbered list.

: close_pending_list ( -- )
  bullet_list_items @ if  close_pending_bullet_list  then
  numbered_list_items @ if  close_pending_numbered_list  then ;
  \ Close a pending list, if needed.
  \ XXX TODO nested lists

\ : close_pending_heading ( -- )
\   opened_[=]? @ if  [markup>order] = [markup<order]  then ;
\   \ Close a pending heading, if needed.
\   \ XXX -- not used yet
\ XXX OLD

: close_pending_paragraph ( -- )
  opened_[_]? @ if  [markup>order] _
\  ." CLOSED PARAGRAPH " cr  \ XXX INFORMER
  [markup<order]  then ;
  \ Close a pending paragraph, if needed.

\ XXX OLD
\ : close_pending_table ( -- )
\   #rows @ if
\     [markup>order] </tr> </table> [markup<order]
\     #rows off  #cells off
\   then ;
\   \ Close a pending table, if needed.
\   \ XXX TODO this will be useless with the new format of tables

: close_pending ( -- )
\  ." close_pending because #nothings = " #nothings @ . cr  \ XXX INFORMER
  close_pending_paragraph
  \ close_pending_table \ XXX OLD
  close_pending_list
  echo_cr ;
  \ Close the pending markups.
  \ Invoked when an empty line if parsed, and at the end of the
  \ parsing.

\ ==============================================================
\ Parser

variable more?  \ flag: keep on parsing more words?; changed by '}content'

:noname ( ca len -- )
\  ." content! " order cr  \ XXX INFORMER
  #markups off  _echo  1 #nonmarkups +!
  ; is content
  \ Manage a parsed string of content: print it and update the counters.

: (markup) ( xt -- )
  #nonmarkups off  execute  1 #markups +! ;
  \ Manage a parsed markup: execute it and update the counters.
  \ xt = execution token of the markup

: markup ( ca len xt -- )
\  ." markup! " order cr  \ XXX INFORMER
  execute_markup? @
  if  nip nip (markup)  else  drop content  then ;
  \ Manage a parsed markup: execute it and update the counters,
  \ if required.
  \ ca len = name of the markup
  \ xt = execution token of the markup

: found? ( ca len -- xt | 0 )
  find-name dup if  name>int  then ;
  \ Is the given string found in the current wordlists?

: markup? ( ca len -- xt | 0 )
\  cr ." markup? >> " 2dup type key drop  \ XXX INFORMER
  get-order n>r
\  cr ." markup? 0 " .s key drop  \ XXX INFORMER
  set_markup_order found?
\  cr ." markup? 1 " .s key drop  \ XXX INFORMER
  nr> set-order
\  cr ." markup? 2 " .s key drop  \ XXX INFORMER
  ;
  \ Is the given string any kind of markup
  \ (wiki markup, HTML entity or user macro)?

\ variable evaluate_the_markup?  \ flag XXX OLD

\ evaluate_the_markup? on  \ XXX OLD

: something ( ca len -- )
\  ." Parameter in 'something' = " 2dup type cr  \ XXX INFORMER
\  depth 2 > abort" something wrong!"  \ XXX INFORMER
\  s" href=@" evaluate ." 'href=' in 'something' = " type cr  \ XXX INFORMER
  2dup parsed$ $!
  #nothings off
\  ." #nothings = " #nothings @ . cr  \ XXX INFORMER
  2dup markup? ?dup if  markup  else  content  then  1 #parsed +! ;
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content)

\ 0 [if]
\
\ \ 2013-08-10: experimental new version, with direct execution of
\ \ Forth code; unfinished
\
\ : something_in_code_zone ( ca len -- ) \ XXX TODO
\   2dup fendo_markup_wid search-wordlist
\   if  markup
\   else
\     2dup fendo_markup_html_entities_wid search-wordlist
\     if  markup  else  content  then
\   then
\   1 #parsed +! ;
\   \ Manage something found in a Forth code zone.
\   \ ca len = parsed item
\
\ : something_in_ordinary_zone ( ca len -- ) \ XXX TODO
\   2dup fendo_markup_wid search-wordlist
\   if  markup
\   else
\     2dup fendo_markup_html_entities_wid search-wordlist
\     if  markup  else  content  then
\   then
\   1 #parsed +! ;
\   \ Manage something found out of Forth code zones.
\   \ ca len = parsed item (markup or printable content)
\
\ : something ( ca len -- ) \ XXX TODO
\   #nothings off
\ \  ." #nothings = " #nothings @ . cr  \ XXX INFORMER
\ \  ~~  \ XXX INFORMER
\   forth_code_depth @
\   if    something_in_code_zone
\   else  something_in_ordinary_zone  then ;
\   \ Manage something found on the page content.
\   \ ca len = parsed item (markup, Forth code or printable content)
\
\ [then]

: nothing ( -- )
\  cr ." nothing"  \ XXX INFORMER
  preserve_eol? @ if  echo_cr  then  \ XXX TMP
  #nothings @  \ an empty line was parsed?
  if  close_pending  then  1 #nothings +!
\  ." #nothings = " #nothings @ . cr  \ XXX INFORMER
  #parsed off ;
  \ Manage a "nothing", a parsed empty name.
  \ The first empty name means the current line is finished;
  \ the second consecutive empty name means the current line is empty.

: parse_content ( "text" -- )
\  cr ." order in parse_content >> " order key drop  \ XXX INFORMER
\  ~~  \ XXX INFORMER
  separate? off  more? on
  begin
\    s" href=@" evaluate ." href (start of 'parse_content' loop) = " type cr  \ XXX INFORMER
    parse-name ?dup
    if    something more? @
    else  drop nothing refill
    then  0=
\    s" href=@" evaluate ." href (end of 'parse_content' loop) = " type cr  \ XXX INFORMER
  until ;
  \ Parse the current input source.
  \ The process is finished by the '}content' markup or the end
  \ of the source.

: (evaluate_content) ( ca len -- )
\  cr cr ." (evaluate_content) >> " 2dup type key drop  \ XXX INFORMER
  save_attributes -attributes  separate? @ >r
  ['] parse_content execute-parsing  #nothings off
  r> separate? !  restore_attributes ;
  \ Evaluate a string as page content.

' (evaluate_content) is evaluate_content

: more_link_text? ( ca len -- f )
  2dup end_of_link_section?
  if  2drop false  else  something true  then ;
  \ Manage a parsed name of a link text.

: parsed_link_text ( "text<spaces>|<spaces>" | "text<spaces>]]<spaces>"  -- ca len )

\  XXX OLD
\  evaluate_the_markup? off  \ deactivate the markup rendering in 'something'

  save_echo echo>string
  save_attributes -attributes
  separate? off

  [ false ] [if]

  \ XXX FIXME 2014-03-18 the parsing fails when the link text spans
  \ the next line?
  begin   parse-name dup
    if    more_link_text?
    else  2drop more_link?
    then  0=
  until

  [else]  \ XXX new version, being fixed

  begin   parse-name ?dup
    if    more_link_text?
    else  drop refill more_link?
    then  0=
  until

  [then]

  echoed $@
  save-mem   \ XXX needed?
  restore_attributes
  restore_echo
\  cr ." result of parsed_link_text = " 2dup type key drop  \ XXX INFORMER
\  evaluate_the_markup? on  \ XXX OLD
  ;
  \ Parse and return the link text.
  \ The possible markup in the link text is not evaluated here,
  \ but at a higher level, when the link is actually echoed. The
  \ reason is links can be created also by direct commands.

: (parse_link_text) ( "...<space>|<space>" | "...<space>]]<space>"  -- )
  s" " link_text!  \ XXX needed?
  parsed_link_text
  \ XXX FIXME link_text@ here returns a string with macros already
  \ parsed! why?
\  cr ." link_text$ in (parse_link_text) = " 2dup type key drop  \ XXX INFORMER
  link_text!
  link_text_already_evaluated? on  \ for 'evaluate_link_text' in <fendo.links.fs>
  ;
  \ Parse the link text and store it into 'link_text'.

' (parse_link_text) is parse_link_text

\ ==============================================================
\ Design template

\ XXX TODO An alternative method: instead of dividing the template by
\ the '{CONTENT}' markup, it would be simpler to parse it and create a
\ '{CONTENT}' word.

: template_file ( -- ca len )
  target_dir $@
  current_page design_subdir dup 0=  \ XXX useful?
  if  2drop website_design_subdir $@  then
  current_page template dup 0=  \ XXX individual page templates are useful
  if  2drop website_template $@ then  s+ s+
\  ." template_file = " 2dup type cr  \ XXX INFORMER
  ;
  \ Return the full path to the template file.

: template_halves ( ca1 len1 -- ca2 len2 ca3 len3 )
  content_markup $@ /sides 0=
  abort" The content markup is missing in the template" ;
  \ Divide the template in two parts, excluding the content holder.
  \ ca1 len1 = template content
  \ ca2 len2 = top half of the template content
  \ ca3 len3 = bottom half of the template content

: template_top ( ca1 len1 -- ca2 len2 )
  template_halves 2drop ;
  \ Extract the top half of a template, above the page content.
  \ ca1 len1 = template content
  \ ca2 len2 = top half of the template content

: template_bottom ( ca1 len1 -- ca2 len2 )
  template_halves 2nip ;
  \ Extract the bottom half of a template, above the page content.
  \ ca1 len1 = template content
  \ ca2 len2 = bottom half of the template content

: get_template ( -- ca len )
  template_file slurp-file ;
  \ Return the template content.

: template{ ( -- )
  get_template template_top evaluate_content ;
  \ Echo the top half of the current template,
  \ above the page content.

: }template ( -- )
  get_template template_bottom evaluate_content ;
  \ Echo the bottom half of the current template,
  \ below the page content.

\ ==============================================================
\ Markup

: .sourcefilename ( -- )
  sourcefilename type cr ;
  \ Print the name of the currently parsed file.

variable }content?  \ flag: was '}content' executed?

: (content{) ( -- )
  opened_markups_off
  open_target template{
  }content? off  parse_content
  }content? @ 0= abort" Missing '}content' at the end of the page" ;
  \ Create the top template part of the target page
  \ and parse the page content.

: update_page? ( -- f )
  current_page newer?  \ source newer than target?
  dup 0= if  current_target_file type ."  is up to date" cr  then ;
  \ Does the target of the current page have to be updated?

false value updating?  \ XXX TODO document

: do_page? ( -- f )
  false  \ don't do it, by default
  do_content? @ 0= ?exit
  current_page draft? ?exit
  updating? if  drop update_page?  else  0=  then ;

: skip_page ( -- )
  \eof do_content? on ;
  \ No target page must be created: Skip the current source page and
  \ restore the default value of `do_content?` for the next page.

: empty_stack ( -- )
\ ."  XXX stack check: " .s  \ XXX INFORMER
  depth abort" Stack not empty"
\  depth if  cr ." Stack not empty" cr .s quit  then  \ XXX INFORMER
  ;

: content{ ( "text }content" -- )
\  ." content{" cr  \ XXX INFORMER
\  ~~  \ XXX INFORMER
  do_page?
\  ~~  \ XXX INFORMER
  if
\  ." `argc` in `content{`= " argc ? cr  \ XXX INFORMER
    empty_stack .sourcefilename
\    ." content{" cr  \ XXX INFORMER
\    ~~  \ XXX INFORMER
    (content{)
  else
    skip_page
\    ~~  \ XXX INFORMER
\  ." `argc` in skipped `content{`= " argc ? cr  \ XXX INFORMER
  then ;
  \ Create the page content, if needed.
  \ The end of the content is marked with the '}content' markup.
  \ Only one 'content{ ... }content' block is allowed in the page.

: finish_the_target ( -- )
  close_pending }template close_target  more? off ;

get-current markup>current

: }content ( -- )
\ cr .s cr ." start of }content " \ XXX INFORMER
  finish_the_target
\  do_content? on  \ default value for the next page  \ XXX OLD
  only fendo>order forth>order
  }content? on
\  .s cr ." end of }content" cr  \ XXX INFORMER
\  ." `argc` in `}content`= " argc ? cr  \ XXX INFORMER
  ;
  \ Finish the page content.

set-current

.( fendo.parser.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2013-04-28: Start.
\
\ 2013-05-18: New: parser; 'skip_content{'.
\
\ 2013-06-01: New: parser rewritten from scratch. Management of empty
\ names and empty lines.
\
\ 2013-06-02: New: counters for both types of elements (markups and
\ printable words); required in order to separate words.
\
\ 2013-06-04: Fix: lists were not properly closed by an empty space.
\
\ 2013-06-05: Fix: 'markup' now uses a name token; this was required
\ in order to define '~', a markup that parses the next name is the
\ source.
\
\ 2013-06-05: New: '#parsed', required for implementing the table
\ markup.
\
\ 2013-06-05: Change: clearer code for closing the pending markups.
\
\ 2013-06-06: Change: renamed from "fendo_content.fs" to
\ "fendo_parser.fs".
\
\ 2013-06-06: New: template implemented.
\
\ 2013-06-08: Improved: Template management.
\
\ 2013-06-08: New: first code for output redirection.
\
\ 2013-06-08: New: first implementation of target directories.
\
\ 2013-06-11: Fix: the target file is opened and closed depending on
\ the 'dry?' config variable.
\
\ 2013-06-11: Fix: typo in comment of 'template_halves'.
\
\ 2013-06-16: Change: The parser has been rewritten; now
\ 'search-wordlist' is used instead of 'parse-name' and 'find-name';
\ this was needed to avoid matches in the Root wordlist.
\
\ 2013-06-16: Fix: Now '}content' toggles the parsing off and sets the
\ normal wordlist order.
\
\ 2013-06-23: Change: design and template variables are renamed after
\ the changes in the config module.
\
\ 2013-06-28: Change: '$@' is no longer required by metadata fields,
\ after Fendo A-01.
\
\ 2013-07-03: Change: 'dry?' renamed to 'echo>screen?', after the
\ changes in the echo module.
\
\ 2013-07-03: Change: words that check the current echo have been
\ renamed, after the  changes in the echo module.
\
\ 2013-07-28: New: 'parse_link_text' moved here from
\ <fendo_markup_wiki.fs>.
\
\ 2013-07-28: Fix: old '[previous]' changed to '[markup<order]'; this
\ was the reason the so many wordlists remained in the search order.
\
\ 2013-08-10: Fix: wrong exit flags in 'parsed_link_text' caused only
\ the first word was parsed.
\
\ 2013-08-10: Fix: the alternative attributes set was needed in
\ 'parsed_link_text', to preserve the current attributes already used
\ for the <a> tag.
\
\ 2013-08-10: Change: 'parse_string' renamed to 'evaluate_content'.
\
\ 2013-08-14: Fix: '#nothings off' was needed at the end of
\ '(evaluate_content)'. Otherwise '#nothings' activated
\ 'close_pending' before expected, e.g. this happened when a link was
\ at the end of a line.
\
\ 2013-09-06: New: 'do_page?'.
\
\ 2013-09-06: Fix: 'content{' doesn't call 'skip_content' anymore but
\ '\eof'; the reason is 'skip_content' parsed until "}content" was
\ found, what was wrong when this word was mentioned in the content
\ itself! That happened in one page and was hard to solve. It's
\ simpler to ignore the whole file. 'skip_content' has been removed.
\
\ 2013-09-29: New: '}content?' flag is used to check if '}content' was
\ executed; this way some markup errors can be detected.
\
\ 2013-12-06: New: 'opened_markups_off' in '(content{)'.
\
\ 2014-03-09: New: 'parsed$' keeps the string in 'something'; required
\ by the new way the heading wiki markups work.
\
\ 2014-06-04: Change: 'close_pending_list' restored. It was commented
\ out, but it's necessary.
\
\ 2014-06-15: Fix: The new flag 'evaluate_the_markup?' (temporarily
\ turned off in 'parsed_link_text) fixes the following problem: link
\ texts were rendered twice: during parsing and during echoing. For
\ example, this caused an abbr macro was recognized and executed
\ during parsing, and then another abbr macro inside the title
\ attribute of the first abbr was recognized and executed as well,
\ what ruined the HTML.  But the fix causes a new problem: images
\ inside link texts crash. The solution was to make the evaluation
\ optional at the higher level, in <fendo.links.fs>, as follows:
\
\ 2014-06-15: Fix: repeated evaluation of link texts is solved with
\ the new 'link_text_already_evaluated?' flag, defined and checked in
\ <fendo.links.fs>.
\
\ 2014-11-09: Old code that was not used or commented out has been
\ removed.
\
\ 2014-11-09: Fix: Now '(evaluate_content)' saves and restores the
\ HTML attributes.
\
\ 2014-11-17: Fix: Now '(evaluate_content)' deletes the HTML
\ attributes after saving them, and preserves 'separate?'.
\
\ 2014-12-06: Change: 'empty_stack' is factored out from
\ '.sourcefilename'.
\
\ 2015-02-01: Improvement: 'do_page?' uses 'newer?'.
\
\ 2015-02-02: New: 'update_page?'; improved 'do_page?'.
\
\ 2015-09-26: Commented out (not deleted yet, just in case)
\ `close_pending_table`, because the new table format (from
\ Asciidoctor) makes it unnecessary.
\
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
