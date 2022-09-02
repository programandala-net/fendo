.( fendo.addon.multilingual.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file creates some low-level tools to manage multilingual
\ websites. See the manual for details.

\ Last modified 20220902T2140+0200.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017,2018,2019,2020,2021,2022 Marcos Cruz (programandala.net)

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
\ Requirements {{{1 {{{1

forth_definitions

require galope/begin-translation.fs \ `begin-translation`
require galope/dollar-comma.fs      \ `$,`
require galope/c-slash-string.fs    \ `c/string`

fendo_definitions

\ ==============================================================
\ Tools {{{1 {{{1

true to multilingual?

: lang_prefix ( ca1 len1 -- ca2 len2 )
  '.' c/string ;

  \ doc{
  \
  \ lang_prefix ( ca1 len1 -- ca2 len2 )
  \
  \ Get the language ID _ca2 len2_ from the first part of a source page file
  \ _ca1 len1_.
  \
  \ It is suggested to use ISO 639-1 or ISO 639-2 codes as language
  \ IDs.
  \
  \ See also: `pid$>lang#`.
  \
  \ }doc

: pid#>lang$ { page_id -- ca len }
  page_id language dup ?exit  2drop
  page_id source_file lang_prefix ;

  \ doc{
  \
  \ pid#>lang$ ( a -- ca len )
  \
  \ Convert page ID _a_ to its language ID _ca len_.
  \
  \ NOTE: The `language` metadatum has higher priority than the
  \ filename's prefix.
  \
  \ See also: `pid#>lang#`, `current_lang$`, `lang_prefix`.
  \
  \ }doc

: current_lang$ ( -- ca len )
  current_page ?dup if  pid#>lang$  else  s" en"  then ;
  \ XXX TODO configurable default language

  \ doc{
  \
  \ current_lang$ ( -- ca len )
  \
  \ Return the language ID _ca len_ of the current page.
  \
  \ See also: `current_lang#`, `current_page`, `pid#>lang$`.
  \
  \ }doc

: pid#>lang# ( a -- n )
  pid#>lang$ s" _language" s+ fendo>order evaluate fendo<order ;

  \ doc{
  \
  \ pid#>lang# ( a -- n )
  \
  \ Convert page ID _a_ to its language number _n_, which is used
  \ as an offset, e.g. for multilingual texts.
  \
  \ See also: `pid#>lang#`, `current_lang$`.
  \
  \ }doc

: pid$>lang# ( ca len -- n )
  pid$>pid# pid#>lang#  ;

  \ doc{
  \
  \ pid$>lang# ( ca len -- n )
  \
  \ Convert page ID _ca len_ to its language ID _n_, which is used as
  \ an offset, e.g. for multilingual texts.
  \
  \ See also: `pid#>lang#`, `pid#>lang$`, `current_lang$`.
  \
  \ }doc

: current_lang# ( -- n )
  current_page dup if  pid#>lang#  then  \ XXX TMP? for testing
  \ current_page pid#>lang#  \ XXX OLD, first version
  ;

  \ doc{
  \
  \ current_lang# ( -- n )
  \
  \ Return the language ID _n_ of the current page.  If no language
  \ has been defined, _n_ is zero, which corresponds to the first
  \ language.
  \
  \ See also: `current_lang$`, `current_page`, `pid#>lang#`.
  \
  \ }doc

\ ==============================================================
\ l10n strings {{{1 {{{1

\ XXX REMARK -- `l10n` words are deprecated. They are superseded by
\ `begin-translation`, which has been moved to Galope. In fact
\ now `langs` is provided by `begin-translation`.

: l10n-string, ( ca-n len-n ... ca1 len1 -- )
  langs 0 ?do $, loop ;
  \ Compile the language strings.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language

: (l10n-string) ( -- )
  does> ( -- ca len ) ( pfa ) current_lang# cells + $@ ;
  \ Define what localization strings do.

: l10n-string ( ca-n len-n ... ca1 len1 "name" -- )
  langs 0= abort" `langs` is not set."
  create l10n-string, (l10n-string) ;

  \ doc{
  \
  \ l10n-string ( ca-n len-n ... ca1 len1 "name" -- )
  \
  \ Create a localization string constant, with translations from _ca1
  \ len1_ (in the first language defined) to translation _ca-n len-n_
  \ (in the last language defined).
  \
  \ When executed, _name_ will return the string corresponding to the
  \ language of the current page.
  \
  \ Usage example:

  \ ----
  \ 0 constant en_language \ English
  \ 1 constant eo_language \ Esperanto
  \ 2 constant es_language \ Spanish
  \ 3 to langs
  \
  \ s" Hello" s" Saluton" s" Hola"
  \ l10n-string multilingual-salute$
  \ ----

  \ WARNING: the name of the language identifiers must have the format
  \ "XXX_language", where "XXX" is the ISO code used in the website
  \ (in word names and page prefixes). The reason is those identifiers
  \ need to be calculated by ``pid#>lang#``. This limitation will be
  \ solved in a future version.
  \
  \ NOTE: ``l10n-string`` is deprecated. It has been superseded by
  \ ``begin-translation`` (provided by the Galope library), which
  \ makes it easier to add new translations gradually and maintain
  \ them.
  \
  \ See also: `noname-l10n-string`, `langs`.
  \
  \ }doc

: noname-l10n-string ( ca-n len-n ... ca1 len1 -- xt )
  langs 0= abort" `langs` is not set; `noname-l10n-string` can not work."
  noname create  l10n-string,  latestxt  (l10n-string) ;

  \ doc{
  \
  \ noname-l10n-string ( ca-n len-n ... ca1 len1 -- xt )
  \
  \ Create an unnamed localization string constant, with translations
  \ from _ca1 len1_ (in the first language defined) to translation
  \ _ca-n len-n_ (in the last language defined).
  \
  \ When executed, _xt_ will return the string corresponding to the
  \ language of the current page.
  \
  \ NOTE: ``noname-l10n-string`` is deprecated. It has been superseded
  \ by ``begin-translation`` (provided by the Galope library), which
  \ makes it easier to add new translations gradually and maintain
  \ them.
  \
  \ See also: ``l10n-string`.
  \
  \ }doc

\ ==============================================================
\ Config of `begin-translation` {{{1 {{{1

' current_lang# is lang
  \ `lang` must return the current language.

\ ==============================================================
\ Development notes {{{1 {{{1

false [if]

\ 2019-03-11: Possible syntaxes considered and tried, from simple to
\ complex to implement:

\ The simplest one:

true
s" Hola"   spanish
s" Salute" interlingue
s" Hello"  english
l10n-constant$ multilingual-salute$

\ More legible, and only a `rot` has to be added to the code:

true
spanish     s" Hola"
interlingue s" Salute"
english     s" Hello"
l10n$ multilingual-salute$

\ Just syntactic sugar instead of `true`:

begin-l10n
  spanish     s" Hola"
  interlingue s" Salute"
  english     s" Hello"
end-l10n multilingual-salute$

\ No change, only clearer names:

begin-translation
  spanish     s" Hola"
  interlingue s" Salute"
  english     s" Hello"
end-translation multilingual-salute$

\ Final, a bit more complex to implement:

begin-translation multilingual-salute$
  spanish     s" Hola"
  interlingue s" Salute"
  english     s" Hello"
end-translation

[then]

.( fendo.addon.multilingual.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2013-10-14: Moved from the application Fendo-programandala.
\
\ 2013-10-15: Improvement: `mlsconstant` checks if `langs` is set.
\
\ 2013-11-11: Improvement: `lang` uses both the "language" metadatum
\ and the language prefix of the file.name.
\
\ 2013-11-30: Change: `mlsconstant` and `langs` are deprecated; now
\ the application must define its own words to store the number of
\ languages and the multilingual strings.
\
\ 2013-12-01: Change: several renamings.
\
\ 2014-02-04: Change: `current_lang#` returns 0 even if no page ID is
\ set; this is useful for testing the localization strings.
\
\ 2014-02-22: Change: all "l10n$" renamed to "l10n-str" in names.
\
\ 2014-03-02: Change: all "l10n-str" renamed to "l10n-string" in
\ names; "-str" is used by Forth Foundation Library's str module.
\
\ 2014-03-11: Fix: in certain cases, `pid#>lang#` needed to make sure
\ `fendo_wid` is in `order`.
\
\ 2014-03-24: New: `pid$>lang#`.
\
\ 2014-12-06: Change: the deprecated old names of `l10n-string` are
\ finally removed.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-10-30: Update Galope `$!,` to `$,`.
\
\ 2018-12-06: Improve documentation.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-17: Update: replace `pid$>data>pid#` with `pid$>pid#`.
\
\ 2018-12-20: Improve documentation.
\
\ 2019-03-11: Write `begin-translation` and
\ `begin-noname-translation`, more flexible alternatives to
\ `l10n-string` and `noname-l10n-string`. Update and improve
\ documentation.
\
\ 2019-03-14: Move `begin-translation`, `lang` and all related words
\ to the Galope library.
\
\ 2019-12-27: Fix typo in comment.
\
\ 2020-11-28: Remove the duplicated documentation of
\ `begin-translation`: now its Galope module is included in the
\ sources used to build the manual.
\
\ 2021-10-23: Replace "See:" with "See also:" in the documentation.a
\
\ 2022-09-02: Improve documentation.

\ vim: filetype=gforth
