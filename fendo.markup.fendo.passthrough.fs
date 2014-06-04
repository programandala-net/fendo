.( fendo.markup.fendo.passthrough.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for passthroughs.

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

: ~~~~  ( "passthrough content ~~~~" -- )
  \ Open, parse and close a passthrough block.
  begin  passthrough-line? dup >r ?echo_line r> 0=  until
  ;

\ Escape

: ~  ( "name" -- )
  \ Escape a name: Parse and echo it, even if it's a markup.
  parse-name? abort" Parseable name expected by '~'"  content
  ;


fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.passthrough.fs compiled ) cr

