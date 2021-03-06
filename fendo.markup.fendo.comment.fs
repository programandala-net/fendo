.( fendo.markup.fendo.comment.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for comments.

\ Last modified  202011160218.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Requirements {{{1

forth_definitions

require galope/paren-star.fs  \ `(*`

\ ==============================================================
\ Markup {{{1

markup_definitions

warnings @  warnings off
' (* alias (*   immediate
' *) alias *)   immediate
' \ alias \     immediate
warnings !

fendo_definitions

.( fendo.markup.fendo.comment.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
