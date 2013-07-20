.( fendo.fs )

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
\ 2013-07-09 Change: 'parse-name?' moved to Galope.

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
require galope/bracket-false.fs  \ '[false]'
require galope/bracket-previous.fs  \ '[previous]'
require galope/buffer-colon.fs  \ 'buffer:'
require galope/colon-alias.fs  \ ':alias'
require galope/colon-create.fs  \ ':create'
require galope/dollar-store-comma.fs  \ '$!,'
require galope/enum.fs  \ 'enum'
require galope/minus-bounds.fs  \ '-bounds'
require galope/minus-extension.fs  \ '-extension'
require galope/minus-leading.fs  \ '-leading'
require galope/minus-path.fs  \ '-path'
require galope/minus-suffix.fs  \ '-suffix'
require galope/parse-name-question.fs  \ 'parse-name?'
require galope/sconstant.fs  \ 'sconstant'
require galope/slash-sides.fs  \ '/sides'

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
