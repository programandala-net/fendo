.( fendo.addon.markup.creole_verbatim_block.fs) cr

\ This file is part of Fendo.

\ This file provides the Creole markup for verbatim blocks, deprecated
\ from the set of markups used by default.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2014-04-20: Code deprecated, substituted by the Asciidoctor markup.
\ Extracted from <fendo.markup.wiki.fs> (then renamed to
\ <fendo.markup.fendo.fs>.

\ **************************************************************

fendo_definitions

: {{{-line  ( -- ca len )
  \ Parse a new line from the current verbatim block.
  read_source_line 0= abort" Missing closing '}}}'"
  escaped_source_code
  ;
: "}}}"?  ( ca len -- wf )
  \ Does the given string contains only "{{{"?
  trim s" }}}" str=
  ;
: {{{-line?  ( -- ca len true | false )
  \ Parse a new line from the current verbatim block.
  {{{-line 2dup "}}}"? 0=
  ;
: ({{{)  ( "verbatim content }}}" -- )
  \ Parse and echo a verbatim zone.
  begin  {{{-line? dup >r ?echo_line r> 0=  until
  ;

markup_definitions

: {{{ ( -- )
  \ Open, parse and close a verbatim block.
  [<pre>] ({{{) [</pre>]
  ;
: }}}  ( -- )
  \ Close a verbatim or pass-through block.
  true abort" '}}}' without '{{{'"
  ;  immediate

fendo_definitions
