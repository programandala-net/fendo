.( addons/multilingual.fs) cr

\ This file is part of Fendo.

\ This file creates some low-level tools to manage multilingual
\ websites. The language of a page is indicated using the ISO language
\ code as first part of the page's file name, e.g.:
\   en.section.subsection.html
\   es.sección.subsección.html 
\   eo.fako.subfako.html

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ 2013-10-14 Moved from the application Fendo-programandala.
\ 2013-10-15 Improvement: 'mlsconstant' checks if 'langs' is set.

\ **************************************************************
\ Usage

0 [if]

\ **Before using** the word 'mlsconstant' defined by this addon, the
\ application must set the 'langs' value with the number of language
\ sections the website has.  Otherwise the program will stop with a
\ message.

\ The constant identifiers of the languages used in the website can be
\ defined later, but always in alphabetical order of its ISO language
\ code.

\ Example:

0 constant en_language
1 constant eo_language
2 constant es_language
3 to langs  \ counter

[then]

\ **************************************************************
\ Requirements

require galope/dollar-store-comma.fs  \ '$!,'

\ **************************************************************

0 value langs  \ number of language sections of the website

: lang  ( a -- ca len )
  \ Return the ISO language code of the given page.
  \ a = page id
  \ xxx todo use the left of the string until the first dot
  source_file drop 2
  ;
: current_lang  ( -- ca len )
  \ Return the ISO language code of the current page.
  current_page lang
  ;
: lang#  ( a -- n )
  \ Return the language number of the given page.
  \ This number is used as an offset, e.g. for multilingual
  \ texts.
  \ a = page id
  lang s" _language" s+ evaluate
  ;
: current_lang#  ( -- n )
  \ Return the language number of the current page.
  current_page lang#
  ;
: +lang  ( a -- a' )
  \ Add the current language number as cells.
  current_lang# cells +
  ;
: mlsconstant ( ca-n len-n ... ca1 len1 "name" -- )
  \ Create a multilingual string constant for three strings.
  \ It will return the string in the language of the current page.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language
  \ name = name of the constant
  \ (Strings must be pushed on the stack
  \ in reverse alphabetical order of its ISO language code).
  langs 0= abort" 'langs' is not set; 'mlsconstant' can not work."
  create  langs 0 ?do  $!,  loop 
  does>   ( -- ca len )  ( pfa ) +lang $@
  ;

.( addons/multilingual.fs compiled) cr
