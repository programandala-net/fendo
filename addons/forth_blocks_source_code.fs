.( addons/forth_blocks_source_code.fs) cr

\ This file is part of Fendo.

\ This file is the Forth blocks source code addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute it and/or modify it
\ under the terms of the GNU General Public License as published by
\ the Free Software Foundation; either version 2 of the License, or
\ (at your option) any later version.

\ Fendo is distributed in the hope that it will be useful, but WITHOUT
\ ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
\ or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
\ License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program; if not, see <http://gnu.org/licenses>.

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-11-09 Code extracted from <addons/source_code.fs>.
\ 2013-11-18 Change: 'programming_language' renamed to
\   'programming_language!', after the changes in the main code.
\ 2013-11-19 Fix: '(echo_forth_block)' and
\   'update_block_0_highlighting'  still used 'higlight?' as a
\   variable, but it was converted to a value.
\ 2013-11-30 Fix: now 'forth_block$' is defered; the application must
\   set it, depending on the languages used in the website.
\ 2013-12-10 Change: 'ql_forth_blocks_source_code' moved to its own file
\   <addons/ql_forth_blocks_source_code.fs>.
\ 2013-12-10 Change: All Abersoft Forth code is moved to its own file
\   <addons/abersoft_forth_blocks_source_code.fs>.

\ **************************************************************
\ Todo

\ 2013-12-12 let abersoft forth syntax? a vim syntax file will be
\ required.
\ 2013-07-26 Character set conversions.

\ **************************************************************
\ Requirements

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require fendo/addons/source_code.fs

\ **************************************************************
\ Forth source code in blocks format

module: forth_blocks_source_code_fendo_addon_module

64 value /forth_block_line  \ chars per line
variable forth_block  \ counter
variable forth_block_lenght 
export
16 value /forth_block  \ lines per block
variable forth_block_line  \ counter
variable highlight_forth_block_0?  \ flag
hide
defer forth_block$  \ "Block" in the current language
s" Block" 2constant (default_forth_block$)
' (default_forth_block$) is forth_block$
(*
\ 'forth_block$' must be set by the application.
\ Example with 'strings:' and ';strings',
\ from the Galope library:
strings: (forth_block$)
  s" Block"   \ English
  s" Bloko"   \ Esperanto
  s" Bloque"  \ Spanish
;strings  ' (forth_block$) is forth_block$
*)
: echo_forth_block_number  ( -- )
  [<p>] forth_block$ echo forth_block @ _echo. [</p>] 
  ;
: reset_forth_block  ( -- )
  0 forth_block_lenght !  new_source_code
  ;
: next_forth_block  ( -- )
  1 forth_block +!  reset_forth_block 
  ;
: update_forth_block_0_highlighting  ( -- )
  \ Turn off highlighting for Forth block 0, if needed.
  forth_block @ 0=
  if  highlight_forth_block_0? @ to highlight?  then
  ;
: (echo_forth_block)  ( -- )
  echo_forth_block_number
  highlight?  \ save
  update_forth_block_0_highlighting
  source_code@ echo_source_code 
  to highlight?  \ restore
  ;
: echo_forth_block  ( -- )
  forth_block_lenght @ if  (echo_forth_block)  then  next_forth_block
  ;
: forth_block_line++  ( -- n )
  \ Increment the counter of Forth block lines.
  \ n = new line number
  forth_block_line @  1+ dup /forth_block < abs *  dup forth_block_line !
  ;
: read_forth_block_line  ( -- ca len )
  \ Note: 'source_line' is a buffer defined in
  \ <fendo/fendo_markup_wiki.fs>.
  source_line dup /forth_block_line source_code_fid read-file throw
\  2dup ." «" type ." »" cr  \ xxx informer
  ;
export
: default_tidy_forth_block_line  ( ca len -- )
  2drop
  ;
defer tidy_forth_block_line  ( ca len -- )
hide
' default_tidy_forth_block_line is tidy_forth_block_line
: save_forth_block_line  ( ca len -- )
  2dup tidy_forth_block_line
  -trailing  dup forth_block_lenght +!
  append_source_code_line
  forth_block_line++ 0= if  echo_forth_block  then
  ;
export
: (forth_blocks_source_code)  ( -- )
  \ xxx todo let abersoft forth to be set
  s" forth" programming_language!
  0 forth_block !
  0 forth_block_line !
  0 forth_block_lenght !
  new_source_code
  begin   read_forth_block_line dup
  while   save_forth_block_line
  repeat  2drop 
  ;
: forth_blocks_source_code  ( ca len -- )
  \ Read the content of a Forth blocks file and echo it.
  \ ca len = file name
  highlight_forth_block_0? on  \ default
  open_source_code (forth_blocks_source_code) close_source_code
  ;

;module

.( addons/forth_blocks_source_code.fs compiled) cr
