.( fendo.fs ) cr

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

only forth definitions

require ffl/str.fs
require ffl/tos.fs
require ffl/xos.fs

require galope/anew.fs
\ require galope/sb.fs
require galope/svariable.fs

\ Generic tool words

: [previous]  ( -- )
  previous
  ;  immediate
: parse-name?  ( "name" -- ca len f )
  \ Parse the next name in the source.
  \ ca len = parsed name
  \ f = empty name?
  parse-name dup 0=
  ;
: :svariable  ( ca len -- )
  \ Create a string variable with the name on the stack.
  \ ca len = name of the variable
  nextname svariable
  ;

anew --fendo--

\ **************************************************************
\ Wordlists

wordlist constant fendo_wid
wordlist constant fendo_markup_wid

fendo_wid >order definitions

\ **************************************************************
\ Modules

include fendo_files.fs
include fendo_data.fs
include fendo_echo.fs
include fendo_markup.fs
include fendo_parser.fs

.( fendo.fs compiled) cr
