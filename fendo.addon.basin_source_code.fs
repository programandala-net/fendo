.( fendo.addon.basin_source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the BASin source code addon.

\ Last modified  20220123T1351+0100.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017 Marcos Cruz (programandala.net)

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

require ffl/chr.fs  \ `chr-digit`

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.source_code.fs
require ./fendo.addon.basin_charset.fs
require ./fendo.addon.latin1_source_code.fs

\ ==============================================================
\ Code {{{1

package fendo.addon.basin_source_code

: not_basin_header? ( ca len -- f )
  \ ca len = source code line
  -leading drop c@ chr-digit? ;

: skip_basin_header ( -- )
  0.  \ fake file position, for the first 2drop
  begin
    2drop  \ file position from the previous loop
    source_code_fid file-position throw
    read_source_code_line >r  not_basin_header?  r> 0= or
  until  source_code_fid reposition-file throw ;
  \ Read the opened source file and skip the lines of the BASin header.
  \
  \ XXX FIXME -- When the file does not have a header, the result is
  \ empty.

public

: basin_source_code_translated ( ca len -- ca' len' )
  basin_charset translated ;
  \ Convert the content of a BASin file to UTF-8.

: basin_source_code ( ca len -- )
  s" basin" programming_language!
  ['] basin_source_code_translated is source_code_posttranslated
  open_source_code skip_basin_header (opened_source_code)
  no_source_code_translation  ; \ default
  \ Read the contents of a BASin file and echo it.
  \ ca len = file name

: headerless_basin_latin1_source_code ( ca len -- )
  s" basin" programming_language!
  ['] latin1_source_code_translated is source_code_pretranslated
  ['] basin_source_code_translated is source_code_posttranslated
  open_source_code (opened_source_code)
  no_source_code_translation ; \ default
  \ Read the contents of a headerless BASin file,
  \ written with Latin1 encoding, and echo it.
  \ ca len = file name

end-package

.( fendo.addon.basin_source_code.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2013-11-09: Code extracted from <addons/source_code.fs>.
\
\ 2013-11-18: Change: `programming_language` renamed to
\ `programming_language!`, after the changes in the main code.
\
\ 2013-12-11: New: `basin_source_code_translated`.
\
\ 2013-12-11: Change: an xt is used, not a translation table; this
\ makes it possible to use different translation tools.
\
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\
\ 2014-03-12: Change: module renamed after the filename.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
