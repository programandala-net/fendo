.( fendo_parser.fs ) 

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-01.

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

\ 2013-04-28 Start.
\ 2013-05-18 New: parser; 'skip_content{'.
\ 2013-06-01 New: parser rewritten from scratch. Management of
\   empty names and empty lines.
\ 2013-06-02 New: counters for both types of elements (markups and
\   printable words); required in order to separate words.
\ 2013-06-04 Fix: lists were not properly closed by an empty space.
\ 2013-06-05 Fix: 'markup' now uses a name token; this was
\   required in order to define '~', a markup that parses the
\   next name is the source.
\ 2013-06-05: New: '#parsed', required for implementing the
\   table markup.
\ 2013-06-05 Change: clearer code for closing the pending
\   markups.
\ 2013-06-06 Change: renamed from "fendo_content.fs" to
\   "fendo_parser.fs".
\ 2013-06-06 New: template implemented.
\ 2013-06-08 Improved: Template management.
\ 2013-06-08 New: first code for output redirection.
\ 2013-06-08 New: first implementation of target directories.
\ 2013-06-11 Fix: the target file is opened and closed depending
\   on the 'dry?' config variable.
\ 2013-06-11 Fix: typo in comment of 'template_halves'.
\ 2013-06-16 Change: The parser has been rewritten; now
\   'search-wordlist' is used instead of 'parse-name' and
\   'find-name'; this was needed to avoid matches in the Root wordlist.
\ 2013-06-16 Fix: Now '}content' toggles the parsing off and sets
\   the normal wordlist order.
\ 2013-06-23 Change: design and template variables are renamed
\   after the changes in the config module.
\ 2013-06-28 Change: '$@' is no longer required by metadata
\   fields, after Fendo A-01.
\ 2013-07-03 Change: 'dry?' renamed to 'echo>screen?', after the
\   changes in the echo module.
\ 2013-07-03 Change: words that check the current echo have been
\ renamed, after the  changes in the echo module.
\ 2013-07-28 New: 'parse_link_text' moved here from
\   <fendo_markup_wiki.fs>.
\
\ **************************************************************
\ Todo

\ 2013-07-20: redirect the parser output to a string. This will
\ let to parse the link texts that have markups!
\
\ 2013-06-04: Flag the first markup of the current line, in
\   order to use '--' both for nested lists and for delete.

\ **************************************************************
\ Pending markups

: close_pending_bullet_list  ( -- )
  \ Close a pending bullet list.
  [markup>order] </li> </ul> [previous]  bullet_list_items off
  ;
: close_pending_numbered_list  ( -- )
  \ Close a pending numbered list.
  [markup>order] </li> </ol> [previous]  numbered_list_items off
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
  opened_[=]? @ if  [markup>order] = [previous]  then
  ;
: close_pending_paragraph  ( -- )
  \ Close a pending paragraph, if needed.
  opened_[_]? @ if  [markup>order] _ [previous]  then
  ;
: close_pending_table  ( -- )
  \ Close a pending table, if needed.
  #rows @ if
    [markup>order] </tr> </table> [previous]
    #rows off  #cells off
  then
  ;
:noname  ( -- )
  \ Close the pending markups.
  \ Invoked when an empty line if parsed, and at the end of the
  \ parsing.
  close_pending_list close_pending_paragraph close_pending_table echo_cr
  ;
is close_pending

\ **************************************************************
\ Parser

0 [if]  \ xxx old first version, with wordlist order and parse-name
:noname  ( ca len -- )
  \ Manage a parsed string of content: print it and update the counters.
  #markups off  _echo  1 #nonmarkups +!
  ;
is content
: (markup)  ( nt -- )
  \ Manage a parsed markup: execute it and update the counters.
  \ nt = name token of the markup
  \ xxx bug thread ends here, but dissapeared without reason!
  #nonmarkups off name>int execute  1 #markups +!
  ;
: markup  ( ca len nt -- )
  \ Manage a parsed markup: execute it and update the counters,
  \ if required.
  \ nt = name token of the markup
  \ ca len = name of the markup
  execute_markup? @  \ xxx used?
  if    
    nip nip (markup)
    \ nip nip drop  \ xxx bug thread
  else
    drop content
  then
  ;
variable #nothings  \ counter of empty parsings
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content)
  #nothings off
  2dup find-name dup
  if
    markup \ xxx bug thread
    \ drop 2drop  \ markup \ xxx bug thread
  else
    drop content
  then
  1 #parsed +!
  ;
: nothing  ( -- )
  \ Manage a "nothing", a parsed empty name. 
  \ The first empty name means the current line is finished;
  \ the second consecutive empty name means the current line is empty.
  #nothings @  \ not the first consecutive time?
  if    close_pending  \ an empty line was parsed
  then  1 #nothings +!  #parsed off
  ;
: (parse_content)  ( "text" -- )
  \ Actually parse the page content.
  \ The process is finished by the '}content' markup.
  \ xxx fixme -- words in Root wordlist are executed!
  \   search-wordlist must be used instead of parse-name.
  begin
    parse-name dup
    if    something  true  \ xxx bug thread
    else  nothing  2drop refill
    then  0=
  until
  ;
: parse_content  ( "text" -- )
  \ Parse the current input source.
  \ The process is finished by the '}content' markup or the end
  \ of the source.
  separate? off
  only markup>order
  (parse_content)
  only forth fendo>order
  ;
[then]

\ xxx second version, with search-wordlist 

:noname  ( ca len -- )
  \ Manage a parsed string of content: print it and update the counters.
  #markups off  _echo  1 #nonmarkups +!
  ;
is content
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
  execute_markup? @
  if    nip nip (markup)
  else  drop content
  then
  ;
variable #nothings  \ counter of empty parsings
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content) 
  #nothings off
  2dup fendo_markup_wid search-wordlist
  if  markup
  else
    2dup fendo_markup_html_entities_wid search-wordlist
    if  markup  else  content  then
  then
  1 #parsed +!
  ;
: nothing  ( -- )
  \ Manage a "nothing", a parsed empty name. 
  \ The first empty name means the current line is finished;
  \ the second consecutive empty name means the current line is empty.
  preserve_eol? @ if  echo_cr  then  \ xxx tmp
  #nothings @  \ not the first consecutive time?
  if    close_pending  \ an empty line was parsed
  then  1 #nothings +!  #parsed off
  ;
variable more?  \ flag: keep on parsing more words?; changed by '}content'
: parse_content  ( "text" -- )
  \ Parse the current input source.
  \ The process is finished by the '}content' markup or the end
  \ of the source.
  separate? off  more? on
  begin
    parse-name ?dup
    if    something more? @
    else  drop nothing refill
    then  0=
  until
  ;
: parse_string  ( ca len -- )
  \ Parse a string. 
  \ ca len = content
  ['] parse_content execute-parsing \ xxx bug thread
  ;
: parse_file  ( ca len -- )
  \ xxx not used
  \ Parse a file.
  \ ca len = filename
  slurp-file parse_string
  ;
: skip_content  ( "text }content" -- )
  \ Skip the page content until the end of the content block.
  begin   parse-name ?dup 
    if    s" }content" str=
    else  drop refill 0= dup abort" Missing '}content'"
    then
  until   do_content? on
  ;
: parsed_link_text  ( "...<space>|<space>" | "...<space>]]<space>"  -- ca len )
  \ Parse and return the link text. 
  \ xxx todo 
  echo> @ echo>string  separate? off 
  begin   parse-name dup
    if    2dup end_of_link_section? if  2drop true  else  something false  then
    else  2drop more_link?
    then  0=
  until   echo> !  echoed $@ 
  ;
: (parse_link_text)  ( "...<space>|<space>" | "...<space>]]<space>"  -- )
  \ Parse the link text and store it into 'link_text'.
  parsed_link_text link_text $!
  ;
' (parse_link_text) is parse_link_text

\ Target file

: target_file  ( a -- ca len )
  \ Return a target HTML page filename. 
  \ a = page-id
  \ ca len = target HTML page file name
  source_file source>target_extension 
  ;
: target_path/file  ( a -- ca len )
  \ Return a target HTML page filename, with its local path.
  \ a = page-id
  \ ca len = target HTML page file name
  target_file target_dir $@ 2swap s+
\  2dup type cr  \ xxx debug check
  ;
: (open_target)  ( -- )
  \ Open the target HTML page file.
  current_page target_path/file
\  cr ." target file =  " 2dup type \ xxx debug check
  w/o create-file throw target_fid !
  \ ." target file just opened: " \ xxx debug check
  \ target_fid @ . cr key drop
  ;
: open_target  ( -- )
  \ Open the target HTML page file, if needed.
  echo>file? if  (open_target)  then
  ;
: (close_target)  ( -- )
  \ Close the target HTML page file.
  target_fid @ close-file throw
  \ ." target_fid just closed. " \ xxx debug check
  ;
: close_target  ( -- )
  \ Close the target HTML page file, if needed.
  target_fid @ if  (close_target)  then
  target_fid off
  ;

\ Design template

: template_file  ( -- ca len )
  \ Return the full path to the template file.
  target_dir $@
  [ true ] [if]  \ actual code
    current_page design_subdir dup 0=
    if  2drop website_design_subdir $@  then  
    current_page template dup 0=
    if  2drop website_template $@ then  s+ s+
  [else]  \ xxx bug thread
    website_design_subdir $@  \ xxx tmp
    website_template $@  \ xxx tmp
    s+ s+
  [then]
  \ ." template_file = " 2dup type cr  \ xxx debug check
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
\ xxx todo simplify, read the file twice?
variable template_content
\ svariable template_content  \ xxx tmp
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
  ;
: template{  ( -- )
  \ Echo the top half of the current template, above the page content.
  get_template
  template_top
  parse_string \ xxx bug thread
  ;
: }template  ( -- )
  \ Echo the bottom half of the current template, below the page content.
  get_template template_bottom parse_string
  ;

\ Markup

: content{  ( "text }content" -- )
  \ Start the page content.
  \ The end of the content is marked with the '}content' markup.
  \ Only one 'content{ ... }content' block is allowed in the page.
  do_content? @
  if    
    open_target
    template{  \ xxx bug thread
    parse_content
  else  skip_content
  then
  ;
get-current markup>current
: }content  ( -- )
  \ Finish the page content. 
\ cr .s cr ." start of }content " \ xxx debug check
  close_pending
  }template  \ xxx bug thread
  close_target \ xxx bug thread
  more? off  \ finish the current parsing
  do_content? on  \ default value for the next page
  only fendo>order forth>order
\ cr .s cr ." end of }content -- press any key..." key drop  \ xxx debug check
  ;
set-current

.( fendo_parser.fs compiled ) cr
