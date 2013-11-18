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

\ **************************************************************
\ Todo

\ 2013-07-26 Character set conversions.

\ **************************************************************
\ Requirements

require fendo/addons/source_code.fs

\ **************************************************************
\ Forth source code in blocks format

module: forth_blocks_source_code_fendo_addon_module

16 value /forth_block  \ lines per block
64 value /forth_block_line  \ chars per line
variable forth_block  \ counter
variable forth_block_line  \ counter
variable forth_block_lenght 
variable abersoft_forth?  \ flag
variable highlight_block_0?  \ flag  xxx todo no interface yet
s" Bloque" s" Bloko" s" Block" mlsconstant forth_block$
: echo_forth_block_number  ( -- )
  [<p>] forth_block$ echo forth_block @ _echo. [</p>] 
  ;
: reset_forth_block  ( -- )
  0 forth_block_lenght !  new_source_code
  ;
: next_forth_block  ( -- )
  1 forth_block +!  reset_forth_block 
  ;
: update_block_0_highlighting  ( -- )
  \ Turn off highlighting for Forth block 0, if needed.
  forth_block @ 0=
  abersoft_forth? @  highlight_block_0? @ 0=  or and
  if  highlight? off  then
  ;
: (echo_forth_block)  ( -- )
  echo_forth_block_number
  highlight? @  update_block_0_highlighting
  source_code@ echo_source_code 
  highlight? !
  ;
: echo_forth_block  ( -- )
  forth_block_lenght @ if  (echo_forth_block)  then  next_forth_block
  ;
: forth_block_line++  ( -- n )
  \ Increment the counter of Forth block lines.
  \ n = new line number
  forth_block_line @  1+ dup /forth_block < abs *  dup forth_block_line !
  ;
: only_one_final_char?  ( ca len -- wf )
  -leading nip 1 =
  ;
: (tidy_line)  ( ca len -- )
  \ Override the last byte of the string with a space, if needed.
  \ It's not clear if that byte is always corrupted,
  \ or, as supposed here, only when the rest of the line is blank.
  \ ca len = Forth block line, last one of the current block
  2dup only_one_final_char? if  + 1- bl swap c!  else  2drop  then
  ;
: tidy_line  ( ca len -- )
  \ Remove the last byte of the last line of the last Forth block.
  \ This is needed with Abersoft Forth's TAP files.
  \ It seems a bug of Abersoft Forth, that corrupts that memory
  \ position before saving the blocks to tape.
  \ ca len = Forth block line
  forth_block_line @ /forth_block 1- =  \ last of block?
  if  (tidy_line)  else  2drop  then
  ;
: read_forth_block_line  ( -- ca len )
  \ Note: 'source_line' is a buffer defined in
  \ <fendo/fendo_markup_wiki.fs>.
  source_line dup /forth_block_line source_code_fid read-file throw
\  2dup ." «" type ." »" cr  \ xxx informer
  ;
: save_forth_block_line  ( ca len -- )
  abersoft_forth? if  2dup tidy_line  then
  -trailing  dup forth_block_lenght +!
  append_source_code_line
  forth_block_line++ 0= if  echo_forth_block  then
  ;
: skip_tap_header  ( -- )
  \ Code adapted from my tool "scr2txt" (2005-2012).
  24 s>d source_code_fid reposition-file throw
  ;
: (forth_blocks_source_code)  ( -- )
  s" forth" programming_language!
  0 forth_block !
  0 forth_block_line !
  0 forth_block_lenght !
  new_source_code
  begin   read_forth_block_line dup
  while   save_forth_block_line
  repeat  2drop 
  ;

export

: forth_blocks_source_code  ( ca len -- )
  \ Read the content of a Forth blocks file and echo it.
  \ ca len = file name
  open_source_code (forth_blocks_source_code) close_source_code
  ;
: abersoft_forth_blocks_source_code  ( ca len -- )
  \ Read the content of a ZX Spectrum's Abersoft Forth blocks TAP file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  abersoft_forth? on
  open_source_code skip_tap_header (forth_blocks_source_code) close_source_code
  abersoft_forth? off
  ;
: ql_forth_blocks_source_code  ( ca len -- )
  \ Read the content of a QL Forth blocks file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  forth_blocks_source_code
  ;

;module

.( addons/forth_blocks_source_code.fs compiled) cr
