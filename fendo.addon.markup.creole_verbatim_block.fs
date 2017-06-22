.( fendo.addon.markup.creole_verbatim_block.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides the Creole markup for verbatim blocks, deprecated
\ from the set of markups used by default.

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

fendo_definitions

: {{{-line ( -- ca len )
  read_source_line 0= abort" Missing closing '}}}'"
  escaped_source_code ;
  \ Parse a new line from the current verbatim block.

: "}}}"? ( ca len -- f )
  trim s" }}}" str= ;
  \ Does the given string contains only "{{{"?

: {{{-line? ( -- ca len true | false )
  {{{-line 2dup "}}}"? 0= ;
  \ Parse a new line from the current verbatim block.

: ({{{) ( "verbatim content }}}" -- )
  begin  {{{-line? dup >r ?echo_line r> 0=  until ;
  \ Parse and echo a verbatim zone.

markup_definitions

: {{{ ( -- )
  [<pre>] ({{{) [</pre>] ;
  \ Open, parse and close a verbatim block.

: }}} ( -- )
  true abort" '}}}' without '{{{'" ; immediate
  \ Close a verbatim or pass-through block.

fendo_definitions

\ ==============================================================
\ Change log

\ 2014-04-20: Code deprecated, substituted by the Asciidoctor markup.
\ Extracted from <fendo.markup.wiki.fs> (then renamed to
\ <fendo.markup.fendo.fs>.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
