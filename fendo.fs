\ fendo.fs

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file is the main one; it loads all the modules.

\ Copyright (C) 2012,2013 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2012-06-30 Start.
\ 2013-04-28 New: <fendo_data.fs>, <fendo_content.fs>.
\ 2013-05-07 New: <fendo_require.fs>.

\ **************************************************************
\ Requirements

require ffl/str.fs
require ffl/tos.fs
require ffl/xos.fs

require galope/anew.fs
\ require galope/sb.fs

\ **************************************************************
\ Modules

anew --fendo--

vocabulary fendo_vocabulary
also fendo_vocabulary  definitions

include fendo_data.fs
include fendo_content.fs
include fendo_require.fs

