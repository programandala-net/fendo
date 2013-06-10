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

\ **************************************************************
\ Change history of this file

\ 2013-05-17 Start.
\ 2013-05-18 Rewritten with '-suffix'.
\ 2013-06-01 Fix: typo in code.

\ **************************************************************
\ Requirements

require galope/minus-suffix.fs  \ '-suffix'

\ **************************************************************

: source>target_extension  ( ca1 len1 -- ca2 len2 )
  \ ca1 len1 = Forth source page file name
  \ ca2 len2 = target HTML page file name
  forth_extension $@ -suffix  html_extension $@ s+
  ;

variable target_fid  \ file id of the HTML target page

: /sourcefilename  ( -- ca len )
  \ Return the current source filename, without path.
  sourcefilename -path
  ;

.( fendo_files.fs compiled) cr
