.( fendo.markup.fendo.code.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for code.

\ Last modified 201709182041.
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

require galope/trim.fs  \ 'trim'

fendo_definitions

require fendo/fendo.addon.source_code.common.fs

\ ==============================================================
\ Tools

fendo_definitions

: (##) ( "source code ##" -- )
  begin   parse-name dup
    if    2dup s" ##" str=
          dup >r 0= if  escaped_source_code _echo  else  2drop  then  r>
    else  2drop refill 0= dup abort" Missing closing '##'"  then
  until ;
  \ Parse an inline source code region.
  \ XXX FIXME preserve spaces
  \ XXX TODO highlight

: ####-line ( -- ca len )
  \ Parse a new line from the current source code block.
  read_source_line 0= abort" Missing closing '####'"
  escaped_source_code ;

: "####"? ( ca len -- f )
  \ Does the given string contains only "####"?
  trim s" ####" str= ;

: ####-line? ( -- ca len f )
  \ Parse a new line from the current source code block.
  \ ca len = source code line
  \ f = is it "####"?
  ####-line 2dup "####"?
\  cr ." exit stack in '####-line?' " .s key drop  \ XXX INFORMER
  ;

: plain_####-zone ( "source code ####" -- )
  begin
    ####-line? dup >r
    if  2drop  else  escaped_source_code echo_line  then  r>
  until
\  cr ." exit stack in 'plain_####-zone' " .s key drop  \ XXX INFORMER
  ;
  \ Parse and echo a source code zone "as is".

: highlighted_####-zone ( "source code ####" -- )
  new_source_code
  begin
    ####-line? dup >r
    if  2drop  else  append_source_code_line  then  r>
  until  source_code@ highlighted echo source_code_finished ;
  \ Parse a source code zone, highlight and echo it.

: highlight_####-zone? ( -- f )
  highlight? programming_language@ nip 0<> and ;

: (####) ( "source code ####" -- )
  highlight_####-zone? if   highlighted_####-zone
                       else plain_####-zone then ;
  \ Parse and echo a source code zone.

\ ==============================================================
\ Markup

markup_definitions

: ## ( -- )
  <code> (##) </code> ;
  \ Open and close an inline <code> region.

: #### ( -- )
  block_source_code{ (####) }block_source_code ;
  \ Open and close a block <code> region.

\ ==============================================================
\ Custom code markup

0 [if]

The website application can create custom

The programming language of a source code block (actually, the name of
a valid Vim filetype in the host OS) can be set his way:

  <[ s" gforth" programming_language! ]>
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

: code_inline_markup ( ca len -- )
  2dup s" ##" 2swap s+ :create_markup s,
  does> ( -- )
    ( dfa ) count programming_language!
    [markup>order] ## [markup<order] ;
  \ Create inline code markup for a specific Vim filetype.
  \ ca len = Vim filetype (for syntax highlighting)

: code_block_markup ( ca len -- )
  2dup s" ####" 2swap s+ :create_markup s,
  does> ( -- )
   ( dfa )  count programming_language!
    [markup>order] #### [markup<order] ;
  \ Create block code markup for a specific Vim filetype.
  \ ca len = Vim filetype (for syntax highlighting)

: code_markup: ( "name" -- )
  parse-name? abort" Expected Vim filetype name"
  2dup code_inline_markup code_block_markup ;
  \ Create inline and block code markups.
  \ Used by the website application to create all
  \ specific code markups used in the contents.
  \ name = Vim filetype (for syntax highlighting)

.( fendo.markup.fendo.code.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2014-04-22: Fix: silly bug.
\
\ 2014-12-12: Fix: 'source_code_finished' has been added at the end of
\ 'highlighted_####-zone'. This obscure bug made caused
\ 'programming_language$' to be preserved after source code blocks, so
\ overwritting the automatic detection done by the next 'source_code'
\ addon, in the current or next page. In order to call
\ 'source_code_finished', <fendo/fendo.addon.source_code.common.fs>
\ has been modifed and included.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-09-18: Fix documentation.

\ vim: filetype=gforth
