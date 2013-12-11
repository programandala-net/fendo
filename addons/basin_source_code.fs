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

\ **************************************************************
\ Todo

\ 2013-07-26 Character set conversions.

\ **************************************************************
\ Requirements

require fendo/addons/source_code.fs
require fendo/addons/basin_charset.fs
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
 
: basin_source_code  ( ca len -- )
  \ Read the content of a BASin file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  s" basin" programming_language!
  ['] basin_charset is source_code_posttranslation_table
  open_source_code skip_basin_header (opened_source_code)
  ;

;module

.( addons/basin_source_code.fs compiled) cr
