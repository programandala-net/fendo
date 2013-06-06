.( fendo_parser.fs) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the page content tools.

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

\ **************************************************************
\ Requirements

require galope/backslash-end-of-file.fs  \ '\eof'

\ **************************************************************
\ Parser

: markup  ( nt -- )
  \ Manage a markup: execute it and update the counters.
  #printable off  name>int execute  1 #markups +!
  ;
: (content)  ( ca len -- )
  \ Manage a string of content: print it and update the counters.
  #markups off  _echo  1 #printable +!
  ;
' (content) is content
variable #nothings  \ counter of empty parsings
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed word
  #nothings off
  2dup find-name dup if  nip nip markup  else  drop content  then
  1 #parsed +!
  ;

: close_pending_bullet_list  ( -- )
  [fendo_markup_wid] </li> </ul> [previous]  bullet_list_items off
  ;
: close_pending_numbered_list  ( -- )
  [fendo_markup_wid] </li> </ol> [previous]  numbered_list_items off
  ;
: close_pending_list  ( -- )
  bullet_list_items @ if  close_pending_bullet_list  then
  numbered_list_items @ if  close_pending_numbered_list  then
  ;
: close_pending_header  ( -- )
  opened_[=]? @ if  [fendo_markup_wid] = [previous]  then
  ;
: close_pending_paragraph  ( -- )
  opened_[_]? @ if  [fendo_markup_wid] _ [previous]  then
  ;
: close_pending_table  ( -- )
  \ xxx todo
  #rows @ if
    [fendo_markup_wid] </tr> </table> [previous]
    #rows off  #cells off
  then
  ;
: (close_pending)  ( -- )
  \ Close the pending markups.
  \ Invoked when an empty line is parsed, and at the end of the
  \ parsing.
  close_pending_list close_pending_paragraph  close_pending_table echo_cr
  ;
' (close_pending) is close_pending
: nothing  ( -- )
  \ Manage a "nothing", a parsed empty name. 
  \ The first empty name means the current line is finished;
  \ the second consecutive empty name means the current line is empty.
  #nothings @
  if    close_pending  \ an empty line was parsed
  then  1 #nothings +!  #parsed off
  ;
: (parse_content)  ( "text" -- )
  \ Actually parse the page content.
  begin
    parse-name dup
    if    something  true
    else  nothing  2drop refill
    then  0=
  until
  ;
: parse_content  ( "text" -- )
  \ Parse the page content.
  separate? off
  only fendo_markup_wid >order
  (parse_content)
  only forth fendo_wid >order
  ;
: skip_content  ( "text }content" -- )
  \ Skip the page content.
  begin   parse-name dup 0=
    if    2drop refill 0= dup abort" Missing '}content'"
    else  s" }content" str=
    then
  until   do_content? on
  ;
: content{  ( "text }content" -- )
  \ Mark the start of the page content. 
  do_content? @ if  parse_content  else  skip_content  then
  ;

.( fendo_parser.fs compiled) cr
