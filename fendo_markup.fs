.( fendo_markup.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

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
\ 2013-06-28 Change: 'forth_code?' changed to 'forth_code_depth', a counter.
\ 2014-02-03 New: ':echo_name+'.

\ **************************************************************
\ Generic tool words for markup and parsing

: :echo_name   ( ca len -- )
  \ Create a word that prints its own name.
  \ ca len = word name 
  2dup :create  s,
  does>  ( -- ) ( dfa )  count echo
  ;
: :echo_name_   ( ca len -- )
  \ Create a word that prints its own name
  \ and forces separation from the following text.
  \ ca len = word name 
  2dup :create  s,
  does>  ( -- ) ( dfa )  count echo  separate? on
  ;
: :echo_name+   ( ca len -- )
  \ Create a word that prints its own name
  \ and forces the following text will not be separated.
  \ ca len = word name 
  2dup :create  s,
  does>  ( -- ) ( dfa )  count _echo  separate? off
  ;
variable header_cell?  \ flag, is it a header cell the latest opened cell in the table?
variable table_started?  \ flag, is a table open?

variable execute_markup?  \ flag, execute the markup while parsing?
execute_markup? on  \ execute by default; otherwise print it
variable preserve_eol?  \ flag, preserve in the target the end of lines of the source?
variable forth_code_depth  \ depth level of the parsed Forth code block?

\ **************************************************************
\ Config

variable xhtml?  \ flag, XHTML syntax?

\ **************************************************************
\ Modules

include ./fendo_markup_html.fs
include ./fendo_shortcuts.fs
include ./fendo_markup_wiki.fs
include ./fendo_markup_macros.fs

.( fendo_markup.fs compiled) cr

