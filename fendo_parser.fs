.( fendo_parser.fs) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

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

\ Fendo is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-04-28 Start.
\ 2013-05-18 New: Parser; 'skip_content{'.
\ 2013-06-01 New: Parser rewritten from scratch. Management of
\   empty names and empty lines.
\ 2013-06-02 New: Counters for both types of elements (markups and
\   printable words); required in order to separate words.
\ 2013-06-04 Fix: lists were not properly closed by an empty space.
\ 2013-06-05 Fix: 'markup' now uses a name token; this was
\   required in order to define '~', a markup that parses the
\   next name is the source.

\ **************************************************************
\ Todo

\ 2013-06-04: Flag the first markup of the current line, in
\   order to use '--' both forth nested lists and delete.
\ 2013-06-04: Additional vocabulary with Forth words allowed
\   during parsing? E.g. 's"', 'place'. Or define them in the same voc?
\   Or make them unnecesary? E.g. use 'class=' for parsing and
\   storing the class attribute, instead of 's" bla" class
\   place'.
\ 2013-06-05: New: '#parsed', required for implementing the
\   table markup.
\ 2013-06-05: Change: clearer code for closing the pending
\   markups.
\ 2013-06-06 Change: renamed from "fendo_content.fs" to
\   "fendo_parser.fs".
\ 2013-06-06 New: template implemented.
\ 2013-06-08 Improved: Template management.
\ 2013-06-08 New: First code for output redirection.


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

:noname  ( ca len -- )
  \ Manage a parsed string of content: print it and update the counters.
  #markups off  _echo  1 #nonmarkups +!
  ;
is content
: (markup)  ( nt -- )
  \ Manage a parsed markup: execute it and update the counters.
  \ nt = name token of the markup
  #nonmarkups off  name>int execute  1 #markups +!
  ;
: markup  ( ca len nt -- )
  \ Manage a parsed markup: execute it and update the counters,
  \ if required.
  \ nt = name token of the markup
  \ ca len = name of the markup
  execute_markup? @
  if    nip nip (markup)
  else  drop content
  then
  ;
variable #nothings  \ counter of empty parsings
: (something)  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content)
  #nothings off
  2dup find-name dup if  markup  else  drop content  then
  1 #parsed +!
  ;
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup, printable content or Forth code)
  forth_block? @ if  evaluate  else  (something)  then
  ;

\ Parser

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
  begin
    parse-name dup
    if    something  true
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
: parse_string  ( ca len -- )
  \ Parse a string. 
  \ ca len = content
  ['] parse_content execute-parsing
  ;
: parse_file  ( ca len -- )
  \ Parse a file.
  \ ca len = filename
  slurp-file parse_string
  ;
: skip_content  ( "text }content" -- )
  \ Skip the page content until the end of the content block.
  begin   parse-name dup 0=
    if    2drop refill 0= dup abort" Missing '}content'"
    else  s" }content" str=
    then
  until   do_content? on
  ;

\ Target file

: target_file  ( a -- ca len )
  \ a -- page-id
  \ ca len = target HTML page file name
  source_file datum@ source>target_extension
  ;
: open_target  ( -- )
  current_page target_file w/o create-file throw target_fid !
  ;

\ Design template

: template_file  ( -- ca len )
  \ Return the full path to the template file.
  designs_dir $@
  current_page design_dir datum@ dup 0=
  if  2drop default_design_dir $@  then  
  current_page template datum@ dup 0=
  if  2drop default_template $@ then  s+ s+
  ;
: template_halves  ( ca1 len1 -- ca2 len2 ca3 len3 )
  \ Divide the template in two parts, excluding the content holder.
  \ ca1 len1 = template content
  \ ca2 len2 = bottom half of the template content
  \ ca3 len3 = top half of the template content
  content_markup $@ /sides 
  ;
: template_top  ( ca1 len1 -- ca2 len2 )
  \ Extract the top half of a template, above the page content.
  \ ca1 len1 = template content
  \ ca2 len2 = top half of the template content
  template_halves 2nip
  ;
: template_bottom  ( ca1 len1 -- ca2 len2 )
  \ Extract the bottom half of a template, above the page content.
  \ ca1 len1 = template content
  \ ca2 len2 = bottom half of the template content
  template_halves 2drop
  ;
variable template_content
: get_template  ( -- ca len )
  template_content $@len?
  if  \ second time
    template_content $@  template_content $off
  else  \ first time
    template_file slurp-file 2dup template_content $!
  then
  ;
: template{  ( -- )
  \ Echo the top half of the current template, above the page content.
  get_template template_top parse_string
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
  if    open_target template{ parse_content
  else  skip_content
  then
  ;
get-current markup>current
: }content  ( -- )
  \ Finish the page content. 
  close_pending  }template  do_content? on
  ;
set-current

.( fendo_parser.fs compiled) cr
