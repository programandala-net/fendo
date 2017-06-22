.( fendo.markup.macros.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides the tools to create the user macros.

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

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================

: macro: ( "name" -- )
  get-current >r  fendo_markup_macros_wid set-current  :
  r> set-current ;
  \ Create a user macro.

: macro_alias ( "name" -- )
  get-current  fendo_markup_macros_wid set-current  latestxt alias
  set-current ;
  \ Create a macro alias of the latest word,
  \ which is supposed to be a user macro.

.( fendo.markup.macros.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-02-05: Start, based on <addons/abbr.fs>.
\ 2014-11-18: Comment modified.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
