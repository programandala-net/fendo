.( fendo_markup.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the markup.

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

\ 2013-06-06 Start. This file is created with part of the old
\   <fendo_html_tags.fs>.
\ 2013-06-08 Change: 'forth_block?' renamed to 'forth_code?'.

\ **************************************************************
\ Generic tool words for markup and parsing

: :echo_name   ( ca len -- )
  \ Create a word that prints its own name.
  \ ca len = word name 
  2dup nextname  create  s,
  does>  ( dfa )  count echo
  ;
variable header_cell?  \ flag, is it a header cell the latest opened cell in the table?
variable table_started?  \ flag, is a table open?

variable execute_markup?  \ flag, execute the markup while parsing?
execute_markup? on  \ execute by default; otherwise print it
variable forth_code?  \ flag, parsing in a Forth code block?

false [if]  \ xxx todo finish
: :>?  ( ca1 len1 ca2 len2 -- ca1 len1 ff )
  \ Add a new name to the parsed merged Forth code.
  \ ca1 len1 = code already parsed in the code
  \ ca2 len2 = new name parsed in the code
  \ ff = is the name the end of the code?
  2dup s" :>" str= >r  s+ s"  " s+  r>
  ;
: slurp-parse  ( ca1 len1 "forthcode :>" -- ca2 len2 )
  \ Get the content of the input stream until a delimiter name is found.
  \ Include that name and return the parsed content.
  \ ca1 len1 = 
  \ ca2 len2 = 
  s" "
  begin
    parse-name dup
    if    slurp:>?               \ end of the code?
    else  2drop refill 0=   \ end of the parsing area?
    then
  until
  ;
[then]

\ **************************************************************
\ Modules

include ./fendo_markup_html.fs
include ./fendo_markup_wiki.fs

.( fendo_markup.fs compiled) cr

