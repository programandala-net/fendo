.( addons/basin_source_code.fs) cr

\ This file is part of Fendo.

\ This file is the BASin source code addon.

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
\ 2013-12-11 New: 'basin_source_code_translated'.
\ 2013-12-11 Change: an xt is used, not a translation table; this
\   makes it possible to use different translation tools.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.

\ **************************************************************
\ Todo

\ 2013-07-26 Character set conversions.

\ **************************************************************
\ Requirements

\ From Galope
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'

\ Fendo addons
require ./source_code.fs
require ./basin_charset.fs

\ From Forth Foundation Library
require ffl/chr.fs  \ 'chr-digit'

\ **************************************************************
\ BASIC source code in BASin format

module: basin_source_code_fendo_addon_module

: not_basin_header?  ( ca len -- wf )
  \ ca len = source code line
  -leading drop c@ chr-digit?
  ;
: skip_basin_header  ( -- )
  \ Read the opened source file and skip the lines of the BASin header.
  0.  \ fake file position, for the first 2drop
  begin
    2drop  \ file position from the previous loop
    source_code_fid file-position throw 
    read_source_code_line >r  not_basin_header?  r> 0= or
  until  source_code_fid reposition-file throw
  ;

export
 
: basin_source_code_translated  ( ca len -- ca' len' )
  \ Convert the content of a BASin file to UTF-8.
  basin_charset translated
  ;
: basin_source_code  ( ca len -- )
  \ Read the content of a BASin file and echo it.
  \ ca len = file name
  s" basin" programming_language!
  ['] basin_source_code_translated is source_code_posttranslated
  open_source_code skip_basin_header (opened_source_code)
  no_source_code_translation  \ default
  ;

;module

.( addons/basin_source_code.fs compiled) cr
