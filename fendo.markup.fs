.( fendo.markup.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the markup.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ ==============================================================

require ./fendo.markup.common.fs
require ./fendo.markup.html.fs
require ./fendo.shortcuts.fs
require ./fendo.markup.fendo.fs
require ./fendo.markup.macros.fs

.( fendo.markup.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-06-06: Start. This file is created with part of the old
\   <fendo_html_tags.fs>.
\ 2013-06-08: Change: 'forth_block?' renamed to 'forth_code?'.
\ 2013-06-28: Change: 'forth_code?' changed to 'forth_code_depth', a counter.
\ 2014-02-03: New: ':echo_name+'.
\ 2014-03-04: Change: all definitions are moved to
\ <fendo.markup.common.fs>.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
