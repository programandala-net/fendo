\ fendo.fs

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.
\ This file is the main one; it loads all the modules.

\ Copyright (C) 2012 Marcos Cruz (programandala.net)

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
\ with Gforth (<http://www.jwdt.com/~paysan/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2012-06-30 Start.

\ **************************************************************
\ Requirements

require ffl/str.f
require ffl/tos.f
require ffl/xos.f

require galope/anew.fs
require galope/csb.fs

\ **************************************************************
\ Modules

anew --fendo--


