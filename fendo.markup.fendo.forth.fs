.( fendo.markup.fendo.forth.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for inline Forth code.

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

require galope/n-to-r.fs  \ 'n>r'
require galope/n-r-from.fs  \ 'nr>'
require galope/dollar-variable.fs  \ '$variable'

\ **************************************************************
\ Tools

fendo_definitions

0 [if]  \ xxx first version

: (forth_code_end?)  ( ca len -- wf )
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed
\  ." «" 2dup type ." » "  \ xxx informer
\  2dup type space \ xxx informer
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +!
  forth_code_depth @
\  dup ." {" . ." }" \ xxx informer
  0= and
\  dup  if ." END!" then  \ xxx informer
\  key drop  \ xxx informer
  ;
: forth_code_end?  ( ca1 len1 ca2 len2 -- ca1' len1' wf )
  \ Add a new name to the parsed merged Forth code
  \ and check if it's the end of the Forth code.
  \ ca1 len1 = code already parsed
  \ ca1' len1' = code already parsed, with ca2 len2 added
  \ ca2 len2 = latest name parsed
  \ wf = is ca2 len2 the right markup for the end of the code?
  2dup (forth_code_end?) dup >r
  0= and  \ empty the name if it's the end of the code
  s+ s"  " s+  r>
  ;
: parse_forth_code  ( "forthcode ]>" -- ca len )
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code
  s" "
  begin   parse-name dup
    if
\          2dup ." { " type ." } "  \ xxx informer
          2dup forth_code_end?
          dup >r
          0= and  s+ s"  " s+ r>
    else  2drop
          \ s\" \n" s+
          s"  " s+
          refill 0=
    then
  until
\  cr ." <[ " 2dup type ." ]>" cr  \ xxx informer
  ;

[then]

true [if]  \ xxx 2013-08-10: second version, more legible

: "<["=  ( -- wf )
  s" <[" str=
  ;
: "]>"=  ( -- wf )
  s" ]>" str=
  ;
: update_forth_code_depth  ( ca len -- )
  \ ca len = latest name parsed
  2dup "<["= abs >r "]>"= r> + forth_code_depth +!
  ;
: forth_code_end?  ( ca len -- wf )
  "]>"= forth_code_depth @ 0= and
  ;
: bl+  ( ca len -- ca' len' )
  s"  " s+
  ;
: remaining   ( -- )
\ xxx informer
  >in @ source 2 pick - -rot + swap
  64 min
  cr ." ***> " type ."  <***" cr
  ;
: parse_forth_code  ( "forthcode ]>" -- ca len )
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code
  s" "
  begin   parse-name dup
    if
\           2dup ." { " type ." } "  \ xxx informer
\           ." { " input-lexeme 2@ type ." } "  \ xxx informer
\           remaining  key drop  \ xxx informer
          2dup update_forth_code_depth
          2dup forth_code_end?
          dup >r if  2drop  else
\          2dup ." {{ " type ." }}"  \ xxx informer
          s+ bl+  then  r>
    else  2drop bl+  refill 0=  then
  until
\  ." ]>" cr  \ xxx informer
\  cr ." <[ " 2dup type ." ]>" cr  \ xxx informer
  ;

[then]

0 [if]  \ experimental version with dynamic string, not finished

: forth_code_end?  ( ca len -- wf )
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed
\  ." «" 2dup type ." » "  \ xxx informer
\  2dup type space \ xxx informer
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +!
  forth_code_depth @
\  dup ." {" . ." }" \ xxx informer
  0= and
\  dup  if ." END!" then  \ xxx informer
\  key drop  \ xxx informer
  ;
$variable forth_code$
: forth_code$+  ( ca len -- )
  \ Append a string to the parsed Forth code.
  forth_code$ $@  s"  " s+ 2swap s+  forth_code$ $!
  ;
: parse_forth_code  ( "forthcode ]>" -- ca len )
  \ Get the content of a merged Forth code.
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code
  s" " forth_code$ $!
  begin   parse-name dup
    if
\          2dup ." { " type ." }"  \ xxx informer
          2dup forth_code_end? dup >r if  2drop  else  forth_code$+  then r>
    else  2drop s"  " forth_code$+  refill 0=  then
  until   forth_code$ $@
\  cr ." <[ " 2dup type ."  ]>" cr  \ xxx informer
  ;
[then]

: evaluate_forth_code  ( i*x ca len -- j*x )
  get-order n>r
  only fendo>order markup>order forth>order
  >sb  \ xxx tmp
  evaluate
  nr> set-order
\  cr ." <[..]> done!" key drop  \ xxx informer
  ;

\ **************************************************************
\ Markup

markup_definitions

true [if]  \ xxx first version

: <[  ( "forthcode ]>" -- )
  \ Start, parse and interpret a Forth block.
  1 forth_code_depth +!
  parse_forth_code
\  cr ." <[ " 2dup type ." ]>" cr  \ xxx informer
  evaluate_forth_code
  ;  immediate
: ]>  ( -- )
  \ Finish a Forth block.
  \ xxx todo
  forth_code_depth @
\  dup   \ xxx
  0= abort" ']>' without '<['"
\  1 = if  \ xxx
\    only markup>order
\    separate? off
\  then
  -1 forth_code_depth +!
  ;  immediate

fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.forth.fs compiled ) cr

