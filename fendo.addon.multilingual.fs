.( fendo.addon.multilingual.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file creates some low-level tools to manage multilingual
\ websites. See the manual for details.

\ Last modified 201903112319.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017,2018,2019 Marcos Cruz (programandala.net)

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
\ Requirements {{{1

forth_definitions

require galope/dollar-comma.fs   \ `$,`
require galope/c-slash-string.fs \ `c/string`
require galope/noname-create.fs  \ `noname-create`
require galope/paren-star.fs     \ `(*`

fendo_definitions

\ ==============================================================
\ Tools {{{1

true to multilingual?

0 value langs

  \ doc{
  \
  \ langs ( -- n )
  \
  \ A ``value`` that returns the number of language sections of a
  \ multilingual website. It must be configured by the application.
  \
  \ Usage example:

  \ ----
  \ 0 constant en_language \ English
  \ 1 constant eo_language \ Esperanto
  \ 2 constant es_language \ Spanish
  \ 3 to langs
  \ ----
  \
  \ See: `lang`, `l10n-string`, `default-lang`.
  \
  \ }doc

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
  \ See: `pid$>lang#`.
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
  \ See: `pid#>lang#`, `current_lang$`, `lang_prefix`.
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
  \ See: `current_lang#`, `current_page`, `pid#>lang$`.
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
  \ See: `pid#>lang#`, `current_lang$`.
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
  \ See: `pid#>lang#`, `pid#>lang$`, `current_lang$`.
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
  \ See: `current_lang$`, `current_page`, `pid#>lang#`.
  \
  \ }doc

: +lang ( a -- a' )
  current_lang# cells + ;
  \ Add the current language number as cells.

\ ==============================================================
\ l10n strings {{{1

: l10n-string, ( ca-n len-n ... ca1 len1 -- )
  langs 0 ?do $, loop ;
  \ Compile the language strings.
  \ ca1 len1 = text in the first language
  \ ca-n len-n = text in the last language

: (l10n-string) ( -- )
  does> ( -- ca len ) ( pfa ) +lang $@ ;
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
  \
  \ NOTE: ``l10n-string`` is deprecated. It has been superseded by
  \ ``begin-translation``, which makes it easier to add new
  \ translations gradually and maintain them.
  \
  \ See: `begin-translation`, `noname-l10n-string`, `langs`.
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
  \ by `begin-translation`, which makes it easier to add new
  \ translations gradually and maintain them.
  \
  \ See: `begin-translation`, `l10n-string`.
  \
  \ }doc

\ ==============================================================
\ l10n$ {{{1

false [if]

\ XXX OLD --  This code is previous to the definitive implementation:
\ `begin-translation`.

0 value default-lang

  \ doc{
  \
  \ default-lang ( -- n )
  \
  \ A ``value`` that returns the language that `l10n$` variables
  \ will use when the translation in the current language is not
  \ available, unless `default-l10n$` is set. Its default value is
  \ zero, i.e. the first language defined by the application.
  \
  \ See: `default-l10n$`, `lang`, `langs`.
  \
  \ }doc

$variable default-l10n$

  \ default-l10n$ ( -- a )
  \
  \ A dynamic string variable. _a_ is the address of the string, which
  \ can be retrieved by Gforth's ``$@`` and set by ``$!``. Its default
  \ value is an empty string.
  \
  \ When the dynamic string pointed by _a_ is not empty, it will be
  \ returned by the variables created by `l10n$`, whenever the
  \ translation in the current language is not available.
  \
  \ When the dynamic string pointed by _a_ is empty, which is the
  \ default, `default-lang` is used instead when the current
  \ translation of a `l10n$` variable is not available.
  \
  \ By storing an identificable string ``default-l10n$``, missing
  \ translations can be traced in the HTML.
  \
  \ See: `default-lang`, `langs`.

: l10n$, ( true n[n] ca[n] len[n] ... n[1] ca[1] len[1] -- )
  here >r langs 0 ?do s" " $, loop
  begin  dup true <>
  while  ( n ca len ) rot cells r@ + $!
  repeat drop rdrop ;
  \ Compile the localization strings received by `l10n$`.

: (l10n$) ( a -- ca len )
  dup +lang $@
  dup if   rot drop                      \ current lang
      else 2drop default-lang cells + $@ \ default lang
      then ;
  \ Behaviour of localization variables created by `l10n$`.
  \ _a_ is the pfa

: l10n$ ( true n[n] ca[n] len[n] ... n[1] ca[1] len[1] "name" -- )
  langs 0= abort" `langs` is not set."
  create l10n$,
  does> ( -- ca len ) ( pfa ) (l10n$) ;

  \ l10n$ ( true n[n] ca[n] len[n] ... n[1] ca[1] len[1] "name" -- )
  \
  \ Create a localization string constant, with translations from
  \ _ca[1] len[1]_ in language _n[n]_ to translation _ca[n] len[n]_ in
  \ language _n[1]_. Any number of translations can be provided.
  \ _true_ marks the end of data.
  \
  \ When executed, _name_ will return the string corresponding to the
  \ language of the current page. If the required translation is not
  \ available, `default-l10n$` is tried first, then `default-lang`.
  \
  \ Usage example:

  \ ----
  \ 0 constant english
  \ 1 constant esperanto
  \ 2 constant spanish
  \ 3 constant interlingue
  \ 4 to langs
  \
  \ true
  \ spanish     s" Hola"
  \ interlingue s" Salute"
  \ english     s" Hello"
  \ l10n$ multilingual-salute$
  \ ----

  \ See: `l10n$`, `default-lang`, `l10n-string`.

[then]

\ ==============================================================
\ begin-translation {{{1

: ?langs ( -- )
  langs 0= abort" `langs` is not set." ;
  \ Aborts if `langs` is zero, i.e. if no languages has been set yet.

0 value default-lang

  \ doc{
  \
  \ default-lang ( -- n )
  \
  \ A ``value`` that returns the language that `translation` variables
  \ will use when the translation in the current language is not
  \ available, unless `default-translation` is set. Its default value is
  \ zero, i.e. the first language defined by the application.
  \
  \ See: `default-translation`, `lang`, `langs`.
  \
  \ }doc

$variable default-translation

  \ doc{
  \
  \ default-translation ( -- a )
  \
  \ A dynamic string variable. _a_ is the address of the string, which
  \ can be retrieved by Gforth's ``$@`` and set by ``$!``. Its default
  \ value is an empty string.
  \
  \ When the dynamic string pointed by _a_ is not empty, it will be
  \ returned by the variables created by `translation`, whenever the
  \ translation in the current language is not available.
  \
  \ When the dynamic string pointed by _a_ is empty, which is the
  \ default, `default-lang` is used instead when the current
  \ translation of a `translation` variable is not available.
  \
  \ By storing an identificable string ``default-translation``, missing
  \ translations can be traced in the HTML.
  \
  \ See: `default-lang`, `langs`.
  \
  \ }doc

true constant translation-sys
  \ The value left by `begin-translation` and
  \ `begin-noname-transaltion`, to be consumed by `end-translation`.

: translation, ( translation-sys n[n] ca[n] len[n] ... n[1] ca[1] len[1] -- )
  here >r langs 0 ?do 0 , loop
  begin  dup translation-sys <>
  while  ( n ca len ) rot cells r@ + $!
  repeat drop rdrop ;
  \ Compile the localization strings received by `end-translation`.

: no-translation ( pfa -- ca len )
  default-translation $@ dup
  if   rot drop
  else 2drop default-lang cells + $@
  then ;
  \ Behaviour of words created by `begin-translation` or
  \ `begin-noname-translation` when no translation has been defined
  \ for the current language: If `default-translation` contains a
  \ non-empty string, return it. Otherwise return the translation in
  \ the language `default-lang`.

: (translation) ( pfa -- ca len )
  dup +lang @ if +lang $@ else no-translation then ;
  \ Behaviour of words created by `begin-translation` or
  \ `begin-noname-translation`.

: end-translation ( translation-sys n[n] ca[n] len[n] ... n[1] ca[1] len[1] -- )
  translation, does> ( -- ca len ) ( pfa ) (translation) ;

  \ doc{
  \
  \ end-translation ( translation-sys n[n] ca[n] len[n] ... n[1] ca[1] len[1] --)
  \
  \ End the definition of a translation started by
  \ `begin-translation`, by compiling all translations from string
  \ _ca[1] len[1]_ in language _n[n]_ to string _ca[n] len[n]_ in
  \ language _n[1]_. Any number of translations can be provided.
  \ _translation-sys_ is left by `begin-translation` and
  \ `begin-noname-translation` in order to mark marks the end of the
  \ parameters.
  \
  \ See `begin-translation` for details and a usage example.
  \
  \ }doc

: begin-translation ( "name" -- translation-sys )
  ?langs create translation-sys ;

  \ doc{
  \
  \ begin-translation ( "name" -- translation-sys )
  \
  \ Begin the definition of a translation, i.e. a string constant that
  \ will be calculated at run-time depending on the language of the
  \ current page.  _translation-sys_ is consumed by `end-translation`.
  \
  \ When executed, _name_ will return the string corresponding to the
  \ language of the current page. If the required translation is not
  \ available, `default-translation` is tried first, then
  \ `default-lang`.
  \
  \
  \ Usage example:

  \ ----
  \ 0 constant english
  \ 1 constant esperanto
  \ 2 constant spanish
  \ 3 constant interlingue
  \ 4 to langs
  \
  \ begin-translation multilingual-salute$
  \   spanish     s" Hola"
  \   interlingue s" Salute"
  \   english     s" Hello"
  \ end-translation
  \
  \ \ Since no Esperanto translation has been defined in this
  \ \ example, it will be calculated depending on
  \ \ `default-translation` and `default-lang`.
  \ ----

  \ See: `end-translation`, `default-translation`, `default-lang`, `l10n-string`.
  \
  \ }doc

: begin-noname-translation ( -- xt translation-sys )
  ?langs noname-create translation-sys ;

  \ doc{
  \
  \ begin-noname-translation ( -- xt translation-sys )
  \
  \ Begin the definition of an unnamed translation an return its _xt_.
  \ A transaltion is a string constant that will be calculated at
  \ run-time depending on the language of the current page.
  \ _translation-sys_ is consumed by `end-translation`.
  \
  \ When executed, _xt_ will return the string corresponding to the
  \ language of the current page. If the required translation is not
  \ available, `default-translation` is tried first, then
  \ `default-lang`.
  \
  \ Usage example:

  \ ----
  \ 0 constant english
  \ 1 constant esperanto
  \ 2 constant spanish
  \ 3 constant interlingue
  \ 4 to langs
  \
  \ defer my-salute
  \
  \ begin-noname-translation
  \   spanish     s" Hola"
  \   interlingue s" Salute"
  \   english     s" Hello"
  \ end-translation is my-salute
  \
  \ \ Since no Esperanto translation has been defined in this
  \ \ example, it will be calculated depending on
  \ \ `default-translation` and `default-lang`.
  \ ----

  \ See: `begin-translation`, `end-translation`,
  \ `default-translation`, `default-lang`, `l10n-string`.
  \
  \ }doc

\ ==============================================================
\ Development notes {{{1

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
\ Change log

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

\ vim: filetype=gforth
