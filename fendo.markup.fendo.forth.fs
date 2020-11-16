.( fendo.markup.fendo.forth.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for inline Forth code.

\ Last modified  202011160218.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018,2020 Marcos Cruz (programandala.net)

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
\ Requirements {{{1

forth_definitions

require galope/n-to-r.fs  \ `n>r`
require galope/n-r-from.fs  \ `nr>`
require galope/dollar-variable.fs  \ `$variable`

\ ==============================================================
\ Tools {{{1

fendo_definitions

: forth_start? ( ca len - f )
  s" <[" str= ; 

: forth_end? ( ca len - f )
  s" ]>" str= ; 

: forth_code_end? ( ca len -- )
  2dup forth_start? abs >r
       forth_end?   dup r> + forth_code_depth +!
  forth_code_depth @ 0= and ;
  \ ca len = latest name parsed

: bl+ ( ca len -- ca' len' )
  s"  " s+ ;

: parse_forth_code ( "ccc" -- ca len )
  s" "
  begin  parse-name dup
    if   2dup forth_code_end?
         dup >r if 2drop else s+ bl+ then r>
    else 2drop bl+ refill 0= then
  until ;
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.

: evaluate_forth_code ( i*x ca len -- j*x )
  get-order n>r
  only fendo>order markup>order forth>order evaluate
  nr> set-order ;

\ ==============================================================
\ Markup {{{1

markup_definitions

: <[ ( "ccc" -- )
  1 forth_code_depth +!
  parse_forth_code evaluate_forth_code ; immediate

  \ doc{
  \
  \ <[ ( "ccc" -- )
  \
  \ Start a Forth code block, finished by `]>`. Parse the input stream
  \ until a valid (unnested) `]>` is found, then evaluate the parsed
  \ string as Forth code.
  \
  \ For convenience, Forth blocks delimited by the marks ``<[`` and
  \ `]>` can be nested. Nesting doesn't make any difference in the
  \ execution of the code: The whole code delimited by the outer marks
  \ is evaluated as if the inner ones didn't exist.
  \
  \ Usage example:

  \ ----
  \ _ This is a paragraph that contains
  \ <[ s" Forth code!" echo ]> .
  \ ----

  \ }doc

: ]> ( -- )
  forth_code_depth @ 0= abort" `]>` without `<[`"
  -1 forth_code_depth +! ; immediate

  \ doc{
  \
  \ ]> ( -- )
  \
  \ Mark the end of a Forth code block, started by `<[`.
  \
  \ See `<[` for a usage example.
  \
  \ }doc

fendo_definitions

.( fendo.markup.fendo.forth.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2020-04-14: Remove old `>sb`.
\
\ 2020-11-14: Delete old useless compilation condition.
\
\ 2020-11-15: Delete old unused code and check points. Simplify
\ `parse_forth_code`. Document `<[` and `]>`.

\ vim: filetype=gforth
