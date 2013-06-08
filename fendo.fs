.( fendo.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

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
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2012-06-30 Start.
\ 2013-04-28 New: <fendo_data.fs>, <fendo_content.fs>.
\ 2013-05-07 New: <fendo_require.fs>.

\ **************************************************************
\ Todo

\ 2013-06-08 Let line comments in data header.

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
require galope/svariable.fs  \ xxx used?
require galope/bracket-false.fs  \ '[false]'

anew --fendo--

\ Generic tool words (canditates for the Galope library)

: $@len? ( $addr -- u )
  \ Return the length of a dynamic string variable,
  \ even if it's not initialized.
  \ $addr = dynamic string variable
  \ u = length
  @ dup if  @  then
  ;

: [previous]  ( -- ) previous ;  immediate : parse-name?  (
"name" -- ca len f )
  \ Parse the next name in the source.  ca len = parsed name f =
  \ empty name?
  parse-name dup 0= ; : :svariable  ( ca len -- )
  \ Create a string variable with the given name.  ca len = name
  \ of the new variable
  nextname svariable ; : :create  ( ca len -- )
  \ Create a 'create' word with the given name.  ca len = name
  \ of the new word
  nextname create ; : :alias  ( xt ca len -- )
  \ Create an alias with the given name for the given xt.  ca
  \ len = name of the new alias xt = execution token of the
  \ original word
  nextname alias ; : -bounds  ( ca1 len1 -- ca2 ca1 )
  \ Convert an address and length to the parameters needed by a
  \ "do ... +loop" in order to examine that memory zone in
  \ reverse order.
  2dup + 1- nip 
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
: /sides  { ca1 len1 ca2 len2 -- ca1 len1' ca3 len3 }
  \ Search a string ca1 len1
  \ for the first occurence of a substring ca2 len2.
  \ Divide the string ca1 len1 in two parts: return both sides
  \ of the substring ca2 len2 (first occurence), excluding the
  \ substring ca2 len2 itself.
  \ 2013-06-07
  \ This word was inspired by Wil Banden's 'hunt'
  \ (Charscan library, 2003-02-17, public domain).
  \ ca1 len1  = string
  \ ca2 len2  = substring
  \ ca1 len1' = right side (or empty string if no match)
  \ ca3 len3  = left side (or whole string if no match)
  ca1 len1 ca2 len2 search if
    len2 - swap dup >r len2 + swap  \ second half
    ca1 r> ca1 - 1-                 \ first half
  else  over 0 
  then
  ;
: sides/  { ca1 len1 ca2 len2 -- ca1 len1' ca3 len3 }
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
  \ ca1 len1' = right side (or empty string if no match)
  \ ca3 len3  = left side (or whole string if no match)
  ca1 len1

  begin   2dup ca2 len2 search
  while   2nip
  repeat  2drop
    len2 - swap dup >r len2 + swap  \ second half
    ca1 r> ca1 - 1-                 \ first half

\    over 0 

  ;

\ **************************************************************
\ Wordlists

wordlist constant fendo_wid  \ for the program words except the markup
wordlist constant fendo_markup_wid  \ only for the markup

: markup>current  ( -- )  fendo_markup_wid set-current  ;
: markup>order    ( -- )  fendo_markup_wid >order  ;
: [markup>order]  ( -- )  markup>order ;  immediate
: fendo>current   ( -- )  fendo_wid set-current  ; 
: fendo>order     ( -- )  fendo_wid >order  ; 
: [fendo>order]   ( -- )  fendo>order  ;  immediate
: forth>order     ( -- )  forth-wordlist >order  ;
: [forth>order]   ( -- )  forth>order  ;  immediate

fendo>order definitions

\ **************************************************************
\ Modules

depth [if] abort [then]  \ xxx debugging
include fendo_config.fs
depth [if] abort [then]  \ xxx debugging
include fendo_files.fs
depth [if] abort [then]  \ xxx debugging
include fendo_data.fs
depth [if] abort [then]  \ xxx debugging
include fendo_echo.fs
depth [if] abort [then]  \ xxx debugging
include fendo_markup.fs
depth [if] abort [then]  \ xxx debugging
include fendo_parser.fs
depth [if] abort [then]  \ xxx debugging

.( fendo.fs compiled) cr
