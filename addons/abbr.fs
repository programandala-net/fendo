\ abbr.fs 

\ This file is part of
\ Fendo-programandala

\ This file is the abbr addon.

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

\ 2013-06-19 Start.
\ 2013-06-29 All addons are defined, but fakes.
\ 2013-07-03 Fix: '(abbr)' refills the input stream until
\   something is found. This makes it possible to separate the
\   addon words and the actual abbreviation in different lines.
\ 2013-07-20 New: '.abbr', 

\ **************************************************************

: abbr: ( "name" -- )
  \ xxx todo
  ;
: (abbr)  ( "name" -- ca len )
  \ Parse an abbreviation.
  begin   parse-name dup 0=
  while   2drop refill 0= abort" Missing abbr"
  repeat
  ;
: .abbr  ( ca len -- )
  \ xxx todo
  [markup>order] <abbr> [previous] echo [markup>order] </abbr> [previous]
  ;
: abbr ( "name" -- )
  \ xxx todo
  (abbr) .abbr  \ xxx tmp
  ;
: abbr_meaning ( "name" -- )
  \ xxx todo
  (abbr) .abbr  \ xxx tmp
  ;
: abbr_translation ( "name" -- )
  \ xxx todo
  (abbr) .abbr  \ xxx tmp
  ;
: abbrs_list ( u -- )
  \ xxx todo
  \ u = heading level
  drop \ xxx tmp
  ;

