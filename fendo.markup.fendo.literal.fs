.( fendo.markup.fendo.literal.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for literal zones.

\ Last modified 201812080157.
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
\ Requirements

forth_definitions

require galope/trim.fs  \ `trim`

\ ==============================================================
\ Tools

fendo_definitions

: literal-line ( -- ca len )
  read_source_line 0= abort" Missing closing `....`"
  escaped_source_code ;
  \ Parse a new line from the current literal block.

: "...."? ( ca len -- f )
  trim s" ...." str= ;
  \ Does the given string contains only "...."?

: literal-line? ( -- ca len true | false )
  literal-line 2dup "...."? 0= ;
  \ Parse a new line from the current literal block.

: (....) ( "literal content ...." -- )
  begin  literal-line? dup >r ?echo_line r> 0=  until ;
  \ Parse and echo a literal block.

\ ==============================================================
\ Markup

markup_definitions

: .... ( "verbatim content ...." -- )
  [<pre>] (....) [</pre>] ;
  \ Open, parse and close a literal block.

fendo_definitions

.( fendo.markup.fendo.literal.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
