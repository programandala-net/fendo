\ fendo_files.fs

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the files tools.

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

\ 2013-05-17 Start.

\ **************************************************************
\

defer fs>html

: (fs>html)  ( ca1 len1 -- ca2 len2 )
  \ Convert a filename from the original Forth source to the
  \ target HTML. Simply convert the "fs" extension to "html". 
  \ a1 len1 = filename of a page Forth source
  \ a2 len2 = filename of its target HTML file
  2 - ." html" s+
  ;

' (fs>html) is fs>html



