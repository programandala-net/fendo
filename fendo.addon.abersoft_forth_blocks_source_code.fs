.( fendo.addon.abersoft_forth_blocks_source_code.fs) cr

\ This file is part of Fendo.

\ This file is the Forth blocks source code addon.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ 2013-12-10: Code extracted from <addons/forth_blocks_source_code.fs>.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\ 2014-03-12: Change: module renamed after the filename.
\ 2014-10-13: Improvement: UDG chars are translated.
\ 2014-10-19: Improvement: The code is highlighted with a specific Vim
\   syntax file, created for Abersoft Forth.

\ **************************************************************
\ Requirements

forth_definitions

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'

fendo_definitions

require ./fendo.addon.forth_blocks_source_code.fs
require ./fendo.addon.zx_spectrum_charset.fs

\ **************************************************************

module: fendo.addon.abersoft_forth_blocks_source_code

: skip_tap_header  ( -- )
  \ Code adapted from my tool "scr2txt" (2005-2012).
  24 s>d source_code_fid reposition-file throw
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

export

: abersoft_forth_blocks_source_code  ( ca len -- )
  \ Read the content of a ZX Spectrum's Abersoft Forth blocks TAP file and echo it.
  \ ca len = file name
  s" abersoft_forth" programming_language!  set_zx_spectrum_source_code_translation
  ['] tidy_line is tidy_forth_block_line
  highlight_forth_block_0? off
  open_source_code skip_tap_header (forth_blocks_source_code) close_source_code
  ['] default_tidy_forth_block_line is tidy_forth_block_line
  ;

;module

.( fendo.addon.abersoft_forth_blocks_source_code.fs compiled) cr
