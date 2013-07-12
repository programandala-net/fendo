\ anchor.fs 

\ This file is part of
\ Fendo-programandala

\ This file is the anchor addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

\ Fendo-programandala is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo-programandala is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ Fendo-programandala is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-06-23 Start.

\ **************************************************************

: anchor ( "name" -- )
  [markup>order]
  id=!  <a> </a>
  [markup<order]
  ;
