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
\ 2013-11-11 Improvement: 'lang' uses both the "language" metadatum
\   and the language prefix of the filename.
\ 2013-11-30 Change: 'mlsconstant' and 'langs' are deprecated; now
\   the application must define its own words
\   to store the number of languages and the multilingual strings. \
\   xxx tmp
\ 2013-12-01 Change: several renamings.

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
require galope/c-slash-string.fs  \ 'c/string'
require galope/paren-star.fs  \ '(*'

\ **************************************************************

true to multilingual?

0 value langs  \ number of language sections of the website  \ xxx old

: lang_prefix  ( ca1 len1 -- ca2 len2 )
  \ Get the ISO language code from the first part of a source page file.
  \ ca1 len1 = page id
  \ ca2 len2 = ISO language code
  [char] . c/string
  ;
: pid#>lang$  { page_id -- ca len }
  \ Return the ISO language code of the given page.
  \ The "language" metadatum has higher priority than the filename's
  \ prefix.
  page_id language dup ?exit  2drop
  page_id source_file lang_prefix
  ;
: current_lang$  ( -- ca len )
  \ Return the ISO language code of the current page.
  \ xxx todo remove tmp result?
  current_page ?dup if  pid#>lang$  else  s" en"  then
  ;
: pid#>lang#  ( a -- n )
  \ Return the language number of the given page.
  \ This number is used as an offset, e.g. for multilingual
  \ texts.
  \ a = page id
  pid#>lang$ s" _language" s+ evaluate
  ;
: current_lang#  ( -- n )
  \ Return the language number of the current page.
  current_page pid#>lang#
  ;
: +lang  ( a -- a' )
  \ Add the current language number as cells.
  current_lang# cells +
  ;
: l10n$,  ( ca-n len-n ... ca1 len1 -- )
  \ Compile the language strings.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language
  langs 0 ?do  $!,  loop 
  ;
: (l10n$)  ( -- )
  \ Define what language string do.
  does>   ( -- ca len )  ( pfa ) +lang $@
  ;
: l10n$  ( ca-n len-n ... ca1 len1 "name" -- )
  \ Create a localization string constant.
  \ It will return the string in the language of the current page.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language
  \ name = name of the constant
  (*
  Note:  \ xxx tmp
  Strings must be pushed on the stack in reverse alphabetical order
  of its ISO language code.
  *)
  langs 0= abort" 'langs' is not set; 'l10n$' can not work."
  create  l10n$,  (l10n$)
  ;
' l10n$ alias mlsconstant  \ old name
: noname_l10n$  ( ca-n len-n ... ca1 len1 -- xt )
  \ Unnamed version of 'l10n$'.
  \ Create a localization string constant.
  \ It will return the string in the language of the current page.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language
  (*
  Note:  \ xxx tmp
  Strings must be pushed on the stack in reverse alphabetical order
  of its ISO language code.
  *)
  langs 0= abort" 'langs' is not set; 'noname_l10n$' can not work."
  noname create  l10n$,  latestxt  (l10n$)
  ;

.( addons/multilingual.fs compiled) cr
