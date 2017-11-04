.( fendo.addon.multilingual.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file creates some low-level tools to manage multilingual
\ websites. The language of a page is indicated using the 2-letter or
\ 3-letter ISO language code as first part of the page's file name,
\ e.g.:

\   en.section.subsection.html
\   es.sección.subsección.html
\   eo.fako.subfako.html

\ Last modified 201710301630.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================
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

\ ==============================================================
\ Requirements

forth_definitions

require galope/dollar-comma.fs    \ '$,'
require galope/c-slash-string.fs  \ 'c/string'
require galope/paren-star.fs      \ '(*'

fendo_definitions

\ ==============================================================

true to multilingual?

0 value langs  \ number of language sections of the website  \ XXX OLD

: lang_prefix ( ca1 len1 -- ca2 len2 )
  [char] . c/string ;
  \ Get the ISO language code from the first part of a source page file.
  \ ca1 len1 = page id
  \ ca2 len2 = ISO language code

: pid#>lang$ { page_id -- ca len }
  page_id language dup ?exit  2drop
  page_id source_file lang_prefix ;
  \ Return the ISO language code of the given page.
  \ The "language" metadatum has higher priority than the filename's
  \ prefix.

: current_lang$ ( -- ca len )
  current_page ?dup if  pid#>lang$  else  s" en"  then ;
  \ Return the ISO language code of the current page.
  \ XXX TODO configurable default language

: pid#>lang# ( a -- n )
  pid#>lang$ s" _language" s+ fendo>order evaluate fendo<order ;
  \ Return the language number of the given page id.
  \ This number is used as an offset, e.g. for multilingual
  \ texts.
  \ a = page id

: pid$>lang# ( ca len -- n )
  pid$>data>pid# pid#>lang#  ;
  \ Return the language number of the given page id.
  \ This number is used as an offset, e.g. for multilingual
  \ texts.
  \ ca len = page id

: current_lang# ( -- n )
  current_page dup if  pid#>lang#  then  \ XXX TMP? for testing
  ;
  \ Return the language number of the current page
  \ (or zero, the first language, if there's no current page yet).
  \ current_page pid#>lang#  \ XXX OLD, first version

: +lang ( a -- a' )
  current_lang# cells + ;
  \ Add the current language number as cells.

: l10n-string, ( ca-n len-n ... ca1 len1 -- )
  langs 0 ?do $, loop ;
  \ Compile the language strings.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language

: (l10n-string) ( -- )
  does> ( -- ca len ) ( pfa ) +lang $@ ;
  \ Define what localization strings do.

: l10n-string ( ca-n len-n ... ca1 len1 "name" -- )
  langs 0= abort" 'langs' is not set; 'l10n-string' can not work."
  create  l10n-string,  (l10n-string) ;
  \ Create a localization string constant.
  \ It will return the string in the language of the current page.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language
  \ name = name of the constant
  (*
  Note:  \ XXX TMP
  Strings must be pushed on the stack in reverse alphabetical order
  of its ISO language code.
  *)

: noname-l10n-string ( ca-n len-n ... ca1 len1 -- xt )
  langs 0= abort" 'langs' is not set; 'noname-l10n-string' can not work."
  noname create  l10n-string,  latestxt  (l10n-string) ;
  \ Unnamed version of 'l10n-string'.
  \ Create a localization string constant.
  \ It will return the string in the language of the current page.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language
  \
  \ XXX TMP -- Note:
  \ Strings must be pushed on the stack in reverse alphabetical order
  \ of its ISO language code.

.( fendo.addon.multilingual.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-10-14: Moved from the application Fendo-programandala.
\
\ 2013-10-15: Improvement: 'mlsconstant' checks if 'langs' is set.
\
\ 2013-11-11: Improvement: 'lang' uses both the "language" metadatum
\ and the language prefix of the file.name.
\
\ 2013-11-30: Change: 'mlsconstant' and 'langs' are deprecated; now
\ the application must define its own words to store the number of
\ languages and the multilingual strings. \ XXX TMP
\
\ 2013-12-01: Change: several renamings.
\
\ 2014-02-04: Change: 'current_lang#' returns 0 even if no pid is set;
\ this is useful for testing the localization strings.
\
\ 2014-02-22: Change: all "l10n$" renamed to "l10n-str" in names.
\
\ 2014-03-02: Change: all "l10n-str" renamed to "l10n-string" in
\ names; "-str" is used by Forth Foundation Library's str module.
\
\ 2014-03-11: Fix: in certain cases, 'pid#>lang#' needed to make sure
\ 'fendo_wid' is in 'order'.
\
\ 2014-03-24: New: 'pid$>lang#'.
\
\ 2014-12-06: Change: the deprecated old names of 'l10n-string' are
\ finally removed.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-10-30: Update Galope `$!,` to `$,`.

\ vim: filetype=gforth
