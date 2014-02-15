.( addons/latin1_source_code.fs) cr

\ This file is part of Fendo.

\ This file is the latin1 source code addon.

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

\ 2013-12-13: Written.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.

\ **************************************************************
\ Requirements

\ Fendo addons
require ./source_code.fs
require ./latin1_charset.fs

\ From Galope
require galope/uncodepaged.fs

\ **************************************************************
\ Source code in latin1 character encoding

: latin1_source_code_translated  ( ca len -- ca' len' )
  \ Convert the content of a latin1 file to UTF-8.
  latin1_charset_to_utf8 uncodepaged
  ;
: latin1_source_code  ( ca len -- )
  \ Read the content of a latin1 file and echo it.
  \ ca len = file name
  ['] latin1_source_code_translated is source_code_pretranslated
  source_code
  ;

.( addons/latin1_source_code.fs compiled) cr



