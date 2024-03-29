.( fendo.addon.abersoft_forth_blocks_source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Forth blocks source code addon.

\ Last modified  20220123T1339+0100.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================
\ Requirements {{{1

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.forth_blocks_source_code.fs
require ./fendo.addon.zx_spectrum_charset.fs

\ ==============================================================
\ Code {{{1

package fendo.addon.abersoft_forth_blocks_source_code

: skip_tap_header ( -- )
  24 s>d source_code_fid reposition-file throw ;

: only_one_final_char? ( ca len -- f )
  -leading nip 1 = ;

: (tidy_line) ( ca len -- )
  2dup only_one_final_char? if  + 1- bl swap c!  else  2drop  then ;
  \ Override the last byte of the string with a space, if needed.
  \ It's not clear if that byte is always corrupted,
  \ or, as supposed here, only when the rest of the line is blank.
  \ ca len = Forth block line, last one of the current block

: tidy_line ( ca len -- )
  forth_block_line @ /forth_block 1- =  \ last of block?
  if  (tidy_line)  else  2drop  then ;
  \ Remove the last byte of the last line of the last Forth block.
  \ This is needed with Abersoft Forth's TAP files.
  \ It seems a bug of Abersoft Forth, that corrupts that memory
  \ position before saving the blocks to tape.
  \ ca len = Forth block line

public

: abersoft_forth_blocks_source_code ( ca len -- )
  s" abersoft_forth" programming_language!
  set_zx_spectrum_source_code_translation
  ['] tidy_line is tidy_forth_block_line
  highlight_forth_block_0? off
  open_source_code skip_tap_header
  (forth_blocks_source_code)
  close_source_code
  ['] default_tidy_forth_block_line is tidy_forth_block_line ;
  \ Read the contents of a ZX Spectrum's
  \ Abersoft Forth blocks TAP file and echo it.
  \ ca len = file name

end-package

.( fendo.addon.abersoft_forth_blocks_source_code.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2013-12-10: Code extracted from
\ <addons/forth_blocks_source_code.fs>.
\
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\
\ 2014-03-12: Change: module renamed after the filename.
\
\ 2014-10-13: Improvement: UDG chars are translated.
\
\ 2014-10-19: Improvement: The code is highlighted with a specific Vim
\ syntax file, created for Abersoft Forth.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
