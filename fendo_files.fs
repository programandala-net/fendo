.( fendo_files.fs) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the file tools.

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
\ 2013-05-18 Rewritten with '-suffix'.
\ 2013-06-01 Fix: typo in code.

\ **************************************************************
\ Requirements

require galope/minus-suffix.fs  \ '-suffix'

\ **************************************************************
\

defer forth_extension
: (forth_extension)  ( -- ca len )
  s" .fs"
  ;
' forth_extension is (forth_extension)
defer html_extension
: (html_extension)  ( -- ca len )
  s" .html"
  ;
' html_extension is (html_extension)

: source>target  ( ca1 len1 -- ca2 len2 )
  \ ca1 len1 = Forth source page file name
  \ ca2 len2 = target HTML page file name
  forth_extension -suffix  html_extension s+
  ;

\ **************************************************************
\ 

: echo  ( ca len -- )
  \ Print text string to the HTML file.
  type  \ xxx to do
  ;

.( fendo_files.fs compiled) cr
