.( fendo.markup.fendo.code.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for code.

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

: (##)  ( "source code ##" -- )
  \ Parse an inline source code region.
  \ xxx fixme preserve spaces
  begin   parse-name dup
    if    2dup s" ##" str=
          dup >r 0= if  escaped_source_code _echo  else  2drop  then  r>
    else  2drop refill 0= dup abort" Missing closing '##'"  then
  until
  ;
: ####-line  ( -- ca len )
  \ Parse a new line from the current source code block.
  read_source_line 0= abort" Missing closing '####'"
  escaped_source_code
  ;
: "####"?  ( ca len -- wf )
  \ Does the given string contains only "####"?
  trim s" ####" str=
  ;
: ####-line?  ( -- ca len wf )
  \ Parse a new line from the current source code block.
  \ ca len = source code line
  \ wf = is it "####"?
  ####-line 2dup "####"?
\  cr ." exit stack in '####-line?' " .s key drop  \ xxx informer
  ;
: plain_####-zone  ( "source code ####" -- )
  \ Parse and echo a source code zone "as is".
  begin
    ####-line? dup >r
    if  2drop  else  escaped_source_code echo_line  then  r>
  until
\  cr ." exit stack in 'plain_####-zone' " .s key drop  \ xxx informer
  ;
: highlighted_####-zone  ( "source code ####" -- )
  \ Parse a source code zone, highlight and echo it.
  new_source_code
  begin
    ####-line? dup >r
    if  2drop  else  append_source_code_line  then  r>
  until  source_code@ highlighted echo
  \ XXX FIXME here, 'source_code_finished' must be called, but it's
  \ defined in <fendo.addon.source.code.fs>
  ;
: highlight_####-zone?  ( -- wf )
  highlight? programming_language@ nip 0<> and
  ;
: (####)  ( "source code ####" -- )
  \ Parse and echo a source code zone.
  highlight_####-zone? if  highlighted_####-zone  else  plain_####-zone  then
  ;

\ **************************************************************
\ Markup

markup_definitions

: ##  ( -- )
  \ Open and close an inline <code> region.
  <code> (##) </code>
  ;
: ####  ( -- )
  \ Open and close a block <code> region.
  block_source_code{ (####) }block_source_code
  ;

\ **************************************************************
\ Custom code markup

0 [if]

The website application can create custom

The programming language of a source code block (actually, the name of
a valid Vim filetype in the host OS) can be set his way:

  <: s" gforth" programming_language! :>
  ####
    ... gforth code ...
  ####


But there's an easier alternative. First, the website application has
to define a custom markup this way:

  code_markup: gforth

Then, the following simpler markup can be used instead:

  ####gforth
    ... gforth code ...
  ####

[then]

fendo_definitions

: code_inline_markup  ( ca len -- )
  \ Create inline code markup for a specific Vim filetype.
  \ ca len = Vim filetype (for syntax highlighting)
  2dup s" ##" 2swap s+ :create_markup s,
  does>  ( -- )
    ( dfa )  count programming_language!
    [markup>order] ## [markup<order]
  ;
: code_block_markup  ( ca len -- )
  \ Create block code markup for a specific Vim filetype.
  \ ca len = Vim filetype (for syntax highlighting)
  2dup s" ####" 2swap s+ :create_markup s,
  does>  ( -- )
    ( dfa )  count programming_language!
    [markup>order] #### [markup<order]
  ;
: code_markup:  ( "name" -- )
  \ Create inline and block code markups.
  \ Used by the website application to create all
  \ specific code markups used in the contents.
  \ name = Vim filetype (for syntax highlighting)
  parse-name? abort" Expected Vim filetype name"
  2dup code_inline_markup code_block_markup
  ;

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\ 2014-04-22: Fix: silly bug.

.( fendo.markup.fendo.code.fs compiled ) cr

