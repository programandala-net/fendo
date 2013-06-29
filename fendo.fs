.( fendo.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-01.

\ This file is the main one; it loads all the modules.

\ Copyright (C) 2012,2013 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth
\   <http://forth.org>
\ with Gforth
\   <http://www.gnu.org/software/gforth/>
\   <http://www.bernd-paysan.de/gforth.html>
\   <http://www.complang.tuwien.ac.at/forth/gforth/>

\ **************************************************************
\ Change history of this file

\ 2012-06-30 Start.
\ 2013-04-28 New: <fendo_data.fs>, <fendo_content.fs>.
\ 2013-05-07 New: <fendo_require.fs>.
\ 2013-06 New: Generic tool words; wordlists.

\ **************************************************************
\ Version history 

\ 2012-06-30 Start of version A-00.
\ 2013-06-28 Start of version A-01.

\ **************************************************************
\ Todo

\ 2013-06-28 Make "plain_" data fields optional; they can be
\ filled as default data, if empty.
\
\ 2013-06-08 Let line comments in data header.

\ **************************************************************
\ Debug

false value [bug_thread] immediate

\ **************************************************************
\ Requirements

only forth definitions

\ From Gforth

require string.fs  \ dynamic strings

\ From Forth Foundation Library

\ require ffl/str.fs
\ require ffl/tos.fs
\ require ffl/xos.fs

\ From Galope

require galope/anew.fs
require galope/minus-leading.fs  \ '-leading'
require galope/sconstant.fs  \ xxx used?
require galope/svariable.fs  \ xxx used?
require galope/bracket-false.fs  \ '[false]'

anew --fendo--

\ Safer alternatives for words of Gforth's string.fs

warnings @  warnings off
: $@len ( $addr -- u )
  \ Return the length of a dynamic string variable,
  \ even if it's not initialized.
  \ $addr = dynamic string variable
  \ u = length
  @ dup if  @  then
  ;
: $@ ( $addr1 -- addr2 u )
  \ Return the content of a dynamic string variable,
  \ even if it's not initialized.
  \ $addr1 = string variable
  \ addr2 u = string
  @ dup if  dup cell+ swap @  else  pad swap  then 
  ;
warnings !

\ Generic tool words (candidates for the Galope library)

: [previous]  ( -- )
  previous
  ;  immediate
: parse-name?  ( "name" -- ca len f )
  \ Parse the next name in the source.
  \ ca len = parsed name 
  \ f = empty name?
  parse-name dup 0=
  ;
: :svariable  ( ca len -- )
  \ Create a string variable with the given name.
  \ ca len = name of the new variable
  nextname svariable
  ;
: :create  ( ca len -- )
  \ Create a 'create' word with the given name.
  \ ca len = name of the new word
  nextname create
  ;
: :alias  ( xt ca len -- )
  \ Create an alias with the given name for the given xt.
  \ ca len = name of the new alias
  \ xt = execution token of the original word
  nextname alias
  ;
: -bounds  ( ca1 len1 -- ca2 ca1 )
  \ Convert an address and length to the parameters needed by a
  \ "do ... +loop" in order to examine that memory zone in
  \ reverse order.
  2dup + 1- nip 
  ;
: csides/  ( ca1 len1 c -- ca2 len2 ca3 len3 )
  \ 2013-06-11 Start, based on '-extension'. Unfinished.
  \ Search a string ca1 len1
  \ for the last occurence of a character c.
  \ Divide the string ca1 len1 in two parts: return both sides
  \ of the character c (last occurence), excluding the
  \ character itself.
  \ ca1 len1 = string to search
  \ c = bound character to search for
  \ ca2 len2 = left part of ca1 len1, until and excluding the last c
  \ ca3 len3 = right part of ca1 len1, from and excluding the last c
  { character }
  2dup -bounds 1+ 2swap  \ default raw return values
  -bounds ?do
    i c@ character = if  drop i  leave  then
  -1 +loop  ( ca1 ca1' )  \ final raw return values
  over -
  ;
: -extension  ( ca1 len1 -- ca1 len1' | ca1 len1 )
  \ Remove the file extension of a filename.
  2dup -bounds 1+ 2swap  \ default raw return values
  -bounds ?do
    i c@ '.' = if  drop i  leave  then
  -1 +loop  ( ca1 ca1' )  \ final raw return values
  over -
  ;
: hunt  ( ca1 len1 ca2 len2 -- ca3 len3 )  \ xxx not used
  \ Search a string ca2 len1 for a substring ca2 len2.
  \ Return the part of ca1 len1 that starts with the first
  \ occurence of ca2 len2.
  \ From Wil Banden's Charscan library (2003-02-17), public domain.
  \ ca1 len1 = string
  \ ca2 len2 = substring
  \ ca3 len3 = ca1+i len1-i 
  search 0= if  chars + 0  then
  ;
[false] [if]  \ first version
: sides  ( ca0 len0 ca1 len2 -- ca3 len3 ca4 len4 )
  \ ca0 len0 = string searched;
  \   starts with the first substring found (ca2 len2)
  \ len2 = length of the substring searched for
  \ ca1 = old ca0, original starting address before the search
  { ca1 len2 }
  len2 - swap dup >r len2 + swap  \ left side
  ca1 r> ca1 -                    \ right side
  ;
[else]  \ second version; does the same, but it's clearer
: sides  { ca1' len1' ca1 len1 len2 -- ca3 len3 ca4 len4 }
  \ ca1' len1' = string searched, starting with the first (ca2 len2)
  \ ca1 len1 = original string, before the search
  \ len2 = length of the substring searched for
  \ ca1' len2 = substring found in ca1 len1
  \ ca3 len3 = left side of ca1 len1, until and excluding ca1' len2
  \ ca4 len4 = right side of ca1 len1, after ca1' len2
  ca1  len1 len1' -          \ left side
  ca1' len2 +  len1' len2 -  \ right side
  ;
[then]

: /sides  { ca1 len1 ca2 len2 -- ca1 len1' ca3 len3 ff }
  \ Search a string ca1 len1
  \ for the first occurence of a substring ca2 len2.
  \ Divide the string ca1 len1 in two parts: return both sides
  \ of the substring ca2 len2 (first occurence), excluding the
  \ substring ca2 len2 itself.
  \ 2013-06-07
  \ This word was inspired by Wil Baden's 'hunt'
  \ (Charscan library, 2003-02-17, public domain).
  \ ca1 len1  = string
  \ ca2 len2  = substring
  \ ca1 len1' = left side (or whole string if not found)
  \ ca3 len3  = right side (or empty string if not found)
  \ ff = found?
  \ Note: ca3 len3 can be empty also when ff is true.
  ca1 len1 ca2 len2 search dup >r
  if    ca1 len1 len2 sides
  else  over 0  \ fake right side
  then  r>
  ;
: +/string  ( ca1 len1 -- ca2 len2 )
  \ xxx not used
  \ Step forward by one char in a buffer.
  \ Inspired by Gforth's '+x/string'.
  \ ca1 len1 = buffer or string
  \ ca2 len2 = remaining buffer or string 
  char- swap char+ swap
  ;
: str<>  ( ca1 len1 ca2 len2 -- ff )
  \ Are two strings different?
  compare 0<>
  ;
: sides/  { ca1 len1 ca2 len2 -- ca1 len1' ca3 len3 f }
  \ Search a string ca1 len1
  \ for the last occurence of a substring ca2 len2.
  \ Divide the string ca1 len1 in two parts: return both sides
  \ of the substring ca2 len2 (last occurence), excluding the
  \ substring ca2 len2 itself.
  \ 2013-06-07
  \ This word was inspired by Wil Banden's 'hunt'
  \ (Charscan library, 2003-02-17, public domain).
  \ ca1 len1  = string
  \ ca2 len2  = substring
  \ ca1 len1' = left side (or whole string if not found) 
  \ ca3 len3  = right side (or empty string if not found)
  \ f = found?
  \ Note: ca3 len3 can be empty also when ff is true.
  ca1 len1 2dup
  begin   ca2 len2 search
  while   2nip 2dup +x/string   \ update the result and step forward
  repeat  2drop
  2dup ca1 len1 str<> dup >r  \ something found?
  if    ca1 len1 len2 sides
  else  over 0  \ fake right side
  then  r>
  ;
: -path  ( ca len -- ca' len' )
  \ Remove the file path and leave the filename.
  \ ca len = filename with path
  \ ca' len' = filename
  s" /" sides/ if  2nip  else  2drop  then
  ;
: -filename  ( ca len -- ca' len' )
  \ Remove the filename and leave the path (with ending slash).
  \ ca len = filename with path
  \ ca' len' = path (with ending slash)
  s" /" sides/ if  2drop s" /" s+  else  2nip  then 
  ;

\ **************************************************************
\ Wordlists

table constant fendo_markup_html_entities_wid  \ HTML entities
wordlist constant fendo_markup_wid  \ markup, except HTML entities
wordlist constant fendo_wid  \ program, except markup and HTML entities

: markup>current  ( -- )
  fendo_markup_wid set-current
  ;
: entities>current  ( -- )
  fendo_markup_html_entities_wid set-current
  ;
: markup>order  ( -- )  
  fendo_markup_html_entities_wid >order
  fendo_markup_wid >order 
  ;
: [markup>order]  ( -- )
  markup>order
  ;  immediate
: markup<order  ( -- )  
  previous previous
  ;
: [markup<order]  ( -- )
  markup<order
  ;  immediate
: fendo>current  ( -- )
  fendo_wid set-current
  ; 
: fendo>order  ( -- )
  fendo_wid >order
  ; 
: [fendo>order]  ( -- )
  fendo>order
  ;  immediate
: forth>order  ( -- )
  forth-wordlist >order
  ;
: [forth>order]  ( -- )
  forth>order
  ;  immediate

fendo>order definitions

s" A-00-20130608" sconstant version

\ **************************************************************
\ Modules

depth [if] abort [then]  \ xxx debugging
include ./fendo_config.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_data.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_echo.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_markup.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_parser.fs
depth [if] abort [then]  \ xxx debugging

.( fendo.fs compiled) cr
