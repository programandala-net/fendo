.( fendo.markup.fendo.forth.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for inline Forth code.

\ Last modified 202011150119.
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
\ Requirements

forth_definitions

require galope/n-to-r.fs  \ `n>r`
require galope/n-r-from.fs  \ `nr>`
require galope/dollar-variable.fs  \ `$variable`

\ ==============================================================
\ Tools

fendo_definitions

: forth-start? ( ca len - f )
  s" <[" str= ; 

: forth-end? ( ca len - f )
  s" ]>" str= ; 

: update_forth_code_depth ( ca len -- )
  2dup forth-start? abs >r
       forth-end?       r> + forth_code_depth +! ;
  \ ca len = latest name parsed

: last_forth_end? ( ca len -- f )
  forth-end? forth_code_depth @ 0= and ;

: bl+ ( ca len -- ca' len' )
  s"  " s+ ;

: parse_forth_code ( "forthcode ]>" -- ca len )
  s" "
  begin  parse-name dup
    if   2dup update_forth_code_depth
         2dup last_forth_end?
         dup >r if 2drop else s+ bl+ then r>
    else 2drop bl+ refill 0= then
  until ;
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code

0 [if]  \ XXX TODO experimental version with a dynamic string

: forth_code_end? ( ca len -- f )
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +!
  forth_code_depth @ 0= and ;
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed

$variable forth_code$

: forth_code$+ ( ca len -- )
  forth_code$ $@  s"  " s+ 2swap s+  forth_code$ $! ;
  \ Append a string to the parsed Forth code.

: parse_forth_code ( "forthcode ]>" -- ca len )
  s" " forth_code$ $!
  begin   parse-name dup
    if
\          2dup ." { " type ." }"  \ XXX INFORMER
          2dup forth_code_end? dup >r if  2drop  else  forth_code$+  then r>
    else  2drop s"  " forth_code$+  refill 0=  then
  until   forth_code$ $@
\  cr ." <[ " 2dup type ."  ]>" cr  \ XXX INFORMER
  ;
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code

[then]

: evaluate_forth_code ( i*x ca len -- j*x )
  get-order n>r
  only fendo>order markup>order forth>order evaluate
  nr> set-order ;

\ ==============================================================
\ Markup

markup_definitions

: <[ ( "forthcode ]>" -- )
  1 forth_code_depth +!
  parse_forth_code evaluate_forth_code ; immediate
  \ Start, parse and interpret a Forth block.

: ]> ( -- )
  forth_code_depth @ 0= abort" `]>` without `<[`"
  -1 forth_code_depth +! ; immediate
  \ Finish a Forth block.

fendo_definitions

.( fendo.markup.fendo.forth.fs compiled ) cr

\ ==============================================================
\ Change log

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
\ 2020-11-15: Delete old unused code and check points.

\ vim: filetype=gforth
