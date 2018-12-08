.( fendo.addon.ql_forth_blocks_source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Forth blocks source code addon.

\ Last modified 201812081823.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017,2018 Marcos Cruz (programandala.net)

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
\ Requirements

require ./fendo.addon.forth_blocks_source_code.fs
require ./fendo.addon.ql_charset.fs

\ ==============================================================

: ql_forth_blocks_source_code ( ca len -- )
  forth_blocks_source_code ;
  \ Read the content of a QL Forth blocks file and echo it.
  \ ca len = file name
  \ XXX TODO -- set the character set for this file type

.( fendo.addon.ql_forth_blocks_source_code.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-12-10: Code extracted from <addons/forth_blocks_source_code.fs>.
\
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
