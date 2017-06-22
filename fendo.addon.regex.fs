.( fendo.addon.regex.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides two words to compile a temporary regex.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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

forth_definitions

\ From Forth Foundation Library
require ffl/rgx.fs  \ regular expressions

\ From Galope
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'

fendo_definitions

\ ==============================================================

module: fendo.addon.regex

: regex_error ( ca len n -- )
  ." Bad regular expression at position " . ." :" cr type abort ;

export

rgx-create regex
: >regex ( ca len -- )
  2dup regex rgx-compile if  2drop  else  regex_error  then ;

;module

.( fendo.addon.regex.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-26: Start.
\ 2014-03-02: Simplified. Renamed. Generalized.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
