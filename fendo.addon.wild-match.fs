.( fendo.addon.wild-match.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides words required to use wildcard matches.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017 Marcos Cruz (programandala.net)

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

require string.fs  \ Gforth's dynamic strings
require galope/wild-match-question.fs  \ 'wild-match?'

fendo_definitions

\ ==============================================================

variable wild-match$

.( fendo.addon.wild-match.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-11-16: Start.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
