.( fendo.markup.fendo.passthrough.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for passthroughs,
\ used to output content 'as is'.

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

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\ 2014-11-04: Simpler loop in '~~~~'.

\ **************************************************************
\ Todo

\ 2014-06-03: '~~' for inline passthrogh.

\ **************************************************************
\ Requirements

forth_definitions

require galope/trim.fs  \ 'trim'

\ **************************************************************
\ Tools

fendo_definitions

: passthrough-line  ( -- ca len )
  \ Parse a new line from the current passthrough block.
  read_source_line 0= abort" Missing closing '~~~~'"
  ;
: "~~~~"?  ( ca len -- wf )
  \ Does the given string contains only "~~~~"?
  trim s" ~~~~" str=
  ;
: passthrough-line?  ( -- ca len true | false )
  \ Parse a new line from the current passthrough block.
  passthrough-line 2dup "~~~~"? 0=
  ;

\ **************************************************************
\ Markup

markup_definitions

\ Block passthroughs

: ~~~~  ( "text<cr>~~~~" -- )
  \ Open, parse and close a passthrough block.
  \ The block is echoed 'as is', line by line,  ignoring any markups
  \ but the end of the block, that must be on its own line.
\  begin  passthrough-line? dup >r ?echo_line r> 0=  until  \ XXX OLD
  begin  passthrough-line?  while  echo_line  repeat  2drop
  ;

\ Escape

: ~  ( "name" -- )
  \ Escape a name: Parse and echo it, even if it's a markup.
  parse-name? abort" Parseable name expected by '~'"  content
  ;


fendo_definitions

.( fendo.markup.fendo.passthrough.fs compiled ) cr

