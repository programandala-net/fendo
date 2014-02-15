.( fendo.addon.ql_forth_blocks_source_code.fs) cr

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

\ 2013-12-10: Code extracted from <addons/forth_blocks_source_code.fs>.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.forth_blocks_source_code.fs
require ./fendo.addon.ql_charset.fs

\ **************************************************************

: ql_forth_blocks_source_code  ( ca len -- )
  \ Read the content of a QL Forth blocks file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  forth_blocks_source_code
  ;

.( fendo.addon.ql_forth_blocks_source_code.fs compiled) cr

