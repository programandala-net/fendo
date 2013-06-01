.( fendo_content.fs) cr

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

\ **************************************************************
\ Requirements

require galope/backslash-end-of-file.fs  \ '\eof'

\ **************************************************************
\ Parser

: do_markup  ( ca len -- )
  \ Evaluate a markup; if it's not a markup, print it.
  \ ca len = parsed word
  2dup find-name if  evaluate  else  echo  then
  ;
variable #empty_lines  \ counter
: close_pending_unordered_list  ( -- )
  #- @ if  [ also fendo_markup_voc ] </li> </ul> [ previous ]  then
  ;
: close_pending_ordered_list  ( -- )
  #+ @ if  [ also fendo_markup_voc ] </li> </ol> [ previous ]  then
  ;
: close_pending_lists  ( -- )
  close_pending_unordered_list  close_pending_ordered_list  
  ;
: close_pending_paragraphs  ( -- )
  |? @ if  [ also fendo_markup_voc ] | [ previous ]  then
  ;
: empty_line  ( -- )
  \ Manage an empty line. 
  \ xxx todo
  ." {EMPTY LINE}"  \ xxx debug check
  close_pending_lists close_pending_paragraphs
  ;
variable #empty_names  \ counter
: empty_name  ( -- )
  \ Manage an empty name. 
  \ The first empty name means the current line is finished;
  \ the second consecutive empty name means the current line is empty.
  #empty_names @ if  empty_line  then  1 #empty_names +!
  ;
: (parse_content)  ( "text" -- )
  cr order cr
  begin
    parse-name dup
    if    do_markup  #empty_names off  true
    else  empty_name 2drop refill
    then  0=
  until
  ;
: parse_content  ( "text" -- )
  ." parse_content" cr  \ xxx tmp
  only fendo_markup_voc
  (parse_content)
  only forth also fendo_voc
  ;

\ **************************************************************
\ Content marks

: }content  ( -- )
  \ Do mark the end of the page content. 
  [ also fendo_markup_voc ]  }content  [ previous ]
  ;
: skip_content  ( "text }content" -- )
  \ Skip the page content.
  begin   parse-name dup 0=
    if    2drop refill 0= dup abort" Missing '}content'"
    else  s" }content" str=
    then
  until   }content
  ;
: content{  ( "text }content" -- )
  \ Mark the start of the page content. 
  do_content? @ if  parse_content  else  skip_content  then
  ;

.( fendo_content.fs compiled) cr
