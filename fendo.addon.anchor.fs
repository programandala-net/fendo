.( fendo.addon.anchor.fs) cr

\ This file is part of Fendo.

\ This file is the anchor addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ 2013-06-23 Start.
\ 2013-10-14 Moved from the application Fendo-programandala.

\ **************************************************************

: anchor ( "name" -- )
  [markup>order]
  id=!  <a> </a>
  [markup<order]
  ;

.( fendo.addon.anchor.fs compiled) cr
