.( fendo.addon.ql_source_code.fs) cr

\ This file is part of Fendo.

\ This file is the Sinclair QL source code addon.

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

\ 2013-12-10 Written with <galope/translated.fs>.
\ 2013-12-11 New: 'ql_source_code_translated'.
\ 2013-12-11 Change: an xt is used, not a translation table; this
\   makes it possible to use different translation tools.
\ 2013-12-12 Rewritten with <galope/uncodepaged.fs>.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.source_code.fs
require ./fendo.addon.ql_charset.fs

\ From Galope
require galope/uncodepaged.fs

\ **************************************************************

: ql_source_code_translated  ( ca len -- ca' len' )
  \ Convert the content of a QL file to UTF-8.
  ql_charset_to_utf8 uncodepaged
  ;
: ql_source_code  ( ca len -- )
  \ Read and echo the content of a Sinclair QL source code file.
  \ The Vim filetype is guessed from the filename.
  \ ca len = file name
  ['] ql_source_code_translated is source_code_pretranslated
  source_code
  ;

.( fendo.addon.ql_source_code.fs compiled) cr
