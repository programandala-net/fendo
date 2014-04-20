.( fendo.addon.cp1252_source_code.fs) cr

\ This file is part of Fendo.

\ This file is the CP1252 source code addon.

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

\ 2013-12-11: Written with <galope/translated.fs>.
\ 2013-12-11: Rewritten with <ftrac/ftrac.fs>.
\ 2013-12-12: Rewritten with <galope/uncodepaged.fs>.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.

\ **************************************************************
\ Requirements

forth_definitions

require galope/uncodepaged.fs

fendo_definitions

require ./fendo.addon.source_code.fs
require ./fendo.addon.cp1252_charset.fs

\ **************************************************************
\ Source code in CP1252 character encoding

: cp1252_source_code_translated  ( ca len -- ca' len' )
  \ Convert the content of a CP1252 file to UTF-8.
  cp1252_charset_to_utf8 uncodepaged
  ;
: cp1252_source_code  ( ca len -- )
  \ Read the content of a CP1252 file and echo it.
  \ ca len = file name
  ['] cp1252_source_code_translated is source_code_pretranslated
  source_code
  ;

.( fendo.addon.cp1252_source_code.fs compiled) cr


