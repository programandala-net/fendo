\ addons/abbr.fs 

\ This file is part of
\ Fendo-programandala

\ This file is the abbr addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

\ Fendo-programandala is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo-programandala is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ Fendo-programandala is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-06-19 Start.
\ 2013-06-29 All addons are defined, but they're temporary fakes.
\ 2013-07-03 Fix: '(abbr)' refills the input stream until
\   something is found. This makes it possible to separate the
\   addon words and the actual abbreviation in different lines.
\ 2013-07-20 New: '.abbr', 
\ 2013-07-28 Fix: old '[previous]' changed to '[markup<order]';
\   this was the reason the so many wordlists remained in the
\   search order.
\ 2013-11-30 Many changes.
\ 2013-12-05 Many changes. First working version.

\ **************************************************************

table constant fendo_abbr_wid  \ for user's abbrs

variable joined_abbr?  \ print the abbr joined to the word before?

0 [if]
\ xxx old first versions
begin-structure abbr%  \ xxx todo tmp draft
  field: abbr$
  field: abbr-meaning$
  field: abbr-lang#
  field: abbr-translation-xt
end-structure
: abbr: ( xt1 xt2 "name" -- )  \ xxx draft
  \ Create an user's abbr.
  \ xt1 = return a string with the actual abbr
  \ xt2 = return a string with the meaning of the actual abbr
  get-current >r  fendo_abbr_wid set-current
  create , ,  r> set-current
  does>  ( -- ca1 len1 ca2 len2 )
    \ ca1 len1 = actual abbr
    \ ca2 len2 = meaning of the actual abbr
    ( pfa ) dup perform rot perform
  ;
: sabbr: ( ca1 len1 ca2 len2 ca3 len3 "name" -- )
  \ Create an user's simple abbr.
  \ ca1 len1 = actual abbr
  \ ca2 len2 = meaning of the abbr
  \ ca3 len3 = ISO code of the language
  get-current >r  fendo_abbr_wid set-current
  create 2, 2, 2,  r> set-current
  does>  ( -- ca1 len1 ca2 len2 ca3 len3 )
    \ ca1 len1 = actual abbr
    \ ca2 len2 = meaning of the abbr
    \ ca3 len3 = ISO code of the language
    ( pfa )
    dup >r 2@ 4 cells + 2@
    r@ 2 cells + 2@ 
    r> 2@ 
  ;
: mlabbr: ( ca1 len1 xt ca3 len3 "name" -- )
  \ Create an user's multilingual abbr.
  \ ca1 len1 = actual abbr
  \ xt = return a string with the meaning of the abbr
  \ ca3 len3 = ISO code of the language
  get-current >r  fendo_abbr_wid set-current
  create 2, , 2,  r> set-current
  does>  ( -- ca1 len1 ca2 len2 ca3 len3 )
    \ ca1 len1 = actual abbr
    \ ca2 len2 = meaning of the abbr
    \ ca3 len3 = ISO code of the language
    ( pfa )
    dup >r 2@ 3 cells + 2@
    r@ 2 cells + perform
    r> 2@ 
  ;
[then]

: abbr: ( "name" -- )
  \ Create an user's abbr.
  get-current >r  fendo_abbr_wid set-current :
  r> set-current
  \ The abbr word must return the following strings:
  ( -- ca1 len1 ca2 len2 ca3 len3 )
    \ ca1 len1 = actual abbr
    \ ca2 len2 = meaning of the abbr
    \ ca3 len3 = translation of the meaning in the current language
    \   (or an empty string)
    \ ca4 len4 = ISO code of the language of the abbr
  ;

: parse_abbr  ( "name" -- ca len )
  \ Parse an abbreviation.
  \ ca len = abbreviaton id
  begin   parse-name dup 0=
  while   2drop refill 0= abort" Missing abbr"
  repeat
  ;
: abbr?  ( ca len -- xt true | false )
  \ ca len = abbreviaton id
  fendo_abbr_wid search-wordlist
  ;
: do_parse_abbr  ( "name" -- xt true | false )
  parse_abbr abbr? 0= abort" Undefined abbr"
  ;
: echo_abbr  ( ca len -- )
  joined_abbr? @ if  echo  else  _echo  then
  ;
: undefined_abbr  ( ca len -- )
  2dup echo_abbr
  ." WARNING: undefined abbr " type cr
\  key drop  \ xxx informer
  ;
: translation_in_parens  ( ca len -- ca' len' )
  s"  (" 2swap s+ s" )" s+
  ;
: meaning+translation  ( ca1 len1 ca2 len2 -- ca1' len1' )
  \ Complete the abbr meaning with its translation, if any.
  \ ca1 len1 = abbr meaning
  \ ca2 len2 = abbr meaning translation, or empty string
  dup if  translation_in_parens s+  else  2drop  then
  ;
: echo_abbr_tag  ( xt -- )
  execute (xml:)lang=!  meaning+translation title=!
  joined_abbr? @ 0= separate? !
  [<abbr>] echo [</abbr>]
  ;
: .abbr  ( ca len -- )
  \ xxx not used
  2dup abbr?
  if  echo_abbr_tag 2drop  else  undefined_abbr  then
  ;
: abbr  ( "name" -- )
  \ Parse and echo an abbr.
  parse_abbr 2dup abbr?
  if  echo_abbr_tag 2drop  else  undefined_abbr  then
  ;
: +abbr  ( "name" -- )
  \ Parse and echo an abbr
  \ that must be joined to the previous word.
  joined_abbr? on  abbr  joined_abbr? off
  ;
: abbr+  ( "name" -- )
  \ Parse and echo an abbr
  \ that must be joined to the next word.
  abbr  separate? off
  ;
: +abbr+  ( "name" -- )
  \ Parse and echo an abbr
  \ that must be joined to the previous word
  \ and to the next word.
  +abbr  separate? off
  ;
: execute_abbr  ( "name" -- ca1 len1 ca2 len2 ca3 len3 ca4 len4 )
  \ "name" = abbr id
  \ ca1 len1 = actual abbr
  \ ca2 len2 = meaning of the abbr
  \ ca3 len3 = translation of the meaning in the current language
  \   (or an empty string)
  \ ca4 len4 = ISO code of the language of the abbr
  do_parse_abbr execute 
  ;
: abbr_meaning  ( "name" -- ca len )
  execute_abbr 2drop 2drop 2nip echo
  ;
: abbr_translation  ( "name" -- )
  execute_abbr 2drop 2nip 2nip echo
  ;
: abbrs_list  ( u -- )
  \ xxx todo
  \ u = heading level
  drop \ xxx tmp
  ;

