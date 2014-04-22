.( fendo.markup.fendo.literal.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for literal zones.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ See at the end of the file.

\ **************************************************************
\ Requirements

forth_definitions

require galope/trim.fs  \ 'trim'

\ **************************************************************
\ Tools

fendo_definitions

: literal-line  ( -- ca len )
  \ Parse a new line from the current literal block.
  read_source_line 0= abort" Missing closing '....'"
  escaped_source_code
  ;
: "...."?  ( ca len -- wf )
  \ Does the given string contains only "...."?
  trim s" ...." str=
  ;
: literal-line?  ( -- ca len true | false )
  \ Parse a new line from the current literal block.
  literal-line 2dup "...."? 0=
  ;
: (....)  ( "literal content ...." -- )
  \ Parse and echo a literal block.
  begin  literal-line? dup >r ?echo_line r> 0=  until
  ;

\ **************************************************************
\ Markup

markup_definitions

: .... ( "verbatim content ...." -- )
  \ Open, parse and close a literal block.
  [<pre>] (....) [</pre>]
  ;

fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.literal.fs compiled ) cr

