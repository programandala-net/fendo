.( fendo.markup.fendo.forth.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for inline Forth code.

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

require galope/n-to-r.fs  \ `n>r`
require galope/n-r-from.fs  \ `nr>`
require galope/dollar-variable.fs  \ `$variable`

\ ==============================================================
\ Tools

fendo_definitions

0 [if]  \ XXX first version

: (forth_code_end?) ( ca len -- f )
\  ." «" 2dup type ." » "  \ XXX INFORMER
\  2dup type space \ XXX INFORMER
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +!
  forth_code_depth @
\  dup ." {" . ." }" \ XXX INFORMER
  0= and
\  dup  if ." END!" then  \ XXX INFORMER
\  key drop  \ XXX INFORMER
  ;
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed

: forth_code_end? ( ca1 len1 ca2 len2 -- ca1' len1' f )
  2dup (forth_code_end?) dup >r
  0= and  \ empty the name if it's the end of the code
  s+ s"  " s+  r> ;
  \ Add a new name to the parsed merged Forth code
  \ and check if it's the end of the Forth code.
  \ ca1 len1 = code already parsed
  \ ca1' len1' = code already parsed, with ca2 len2 added
  \ ca2 len2 = latest name parsed
  \ f = is ca2 len2 the right markup for the end of the code?

: parse_forth_code ( "forthcode ]>" -- ca len )
  s" "
  begin   parse-name dup
    if
\          2dup ." { " type ." } "  \ XXX INFORMER
          2dup forth_code_end?
          dup >r
          0= and  s+ s"  " s+ r>
    else  2drop
          \ s\" \n" s+
          s"  " s+
          refill 0=
    then
  until
\  cr ." <[ " 2dup type ." ]>" cr  \ XXX INFORMER
  ;
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code

[then]

true [if]  \ XXX 2013-08-10: second version, more legible

: "<["= ( -- f )
  s" <[" str= ;

: "]>"= ( -- f )
  s" ]>" str= ;

: update_forth_code_depth ( ca len -- )
  2dup "<["= abs >r "]>"= r> + forth_code_depth +! ;
  \ ca len = latest name parsed

: forth_code_end? ( ca len -- f )
  "]>"= forth_code_depth @ 0= and ;

: bl+ ( ca len -- ca' len' )
  s"  " s+ ;

: remaining  ( -- )
  >in @ source 2 pick - -rot + swap
  64 min
  cr ." ***> " type ."  <***" cr ;
  \ XXX INFORMER

: parse_forth_code ( "forthcode ]>" -- ca len )
  s" "
  begin   parse-name dup
    if
\           2dup ." { " type ." } "  \ XXX INFORMER
\           ." { " input-lexeme 2@ type ." } "  \ XXX INFORMER
\           remaining  key drop  \ XXX INFORMER
          2dup update_forth_code_depth
          2dup forth_code_end?
          dup >r if  2drop  else
\          2dup ." {{ " type ." }}"  \ XXX INFORMER
          s+ bl+  then  r>
    else  2drop bl+  refill 0=  then
  until
\  ." ]>" cr  \ XXX INFORMER
\  cr ." <[ " 2dup type ." ]>" cr  \ XXX INFORMER
  ;
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code

[then]

0 [if]  \ experimental version with dynamic string, not finished

: forth_code_end? ( ca len -- f )
\  ." «" 2dup type ." » "  \ XXX INFORMER
\  2dup type space \ XXX INFORMER
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +!
  forth_code_depth @
\  dup ." {" . ." }" \ XXX INFORMER
  0= and
\  dup  if ." END!" then  \ XXX INFORMER
\  key drop  \ XXX INFORMER
  ;
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
  only fendo>order markup>order forth>order
  >sb  \ XXX TMP
  evaluate
  nr> set-order
\  cr ." <[..]> done!" key drop  \ XXX INFORMER
  ;

\ ==============================================================
\ Markup

markup_definitions

true [if]  \ XXX first version

: <[ ( "forthcode ]>" -- )
  1 forth_code_depth +!
  parse_forth_code
\  cr ." <[ " 2dup type ." ]>" cr key drop \ XXX INFORMER
  evaluate_forth_code ; immediate
  \ Start, parse and interpret a Forth block.

: ]> ( -- )
  forth_code_depth @
\  dup   \ XXX
  0= abort" `]>` without `<[`"
\  1 = if  \ XXX
\    only markup>order
\    separate? off
\  then
  -1 forth_code_depth +! ; immediate
  \ Finish a Forth block.
  \ XXX TODO

fendo_definitions

.( fendo.markup.fendo.forth.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
