\ galope/begin-translation.fs

\ This file is part of Galope
\ http://programandala.net/en.program.galope.html

\ Author: Marcos Cruz (programandala.net), 2019.

\ Last modified 201912271508
\ See change log at the end of the file

\ ==============================================================
\ Requirements {{{1

require ./noname-create.fs  \ `noname-create`

\ ==============================================================
\ begin-translation {{{1

defer lang ( -- )

  \ doc{
  \
  \ lang ( -- n )
  \
  \ A defered word that must be configured by the application.
  \ It returns the number _n_ (0 index) of the current language, which
  \ will be used to return the proprer translation defined by
  \ `begin-translation`.
  \
  \ See: `langs`, `default-lang`.
  \
  \ }doc

: +lang ( a1 -- a2 ) lang cells + ;

0 value langs

  \ doc{
  \
  \ langs ( -- n )
  \
  \ A ``value`` that returns the number of languages used by the
  \ application. It must be configured by the application.
  \
  \ Usage example:

  \ ----
  \ 0 constant en_language \ English
  \ 1 constant eo_language \ Esperanto
  \ 2 constant es_language \ Spanish
  \ 3 to langs
  \ ----
  \
  \ See: `lang`, `default-lang`.
  \
  \ }doc

: ?langs ( -- )
  langs 0= abort" `langs` is not set." ;
  \ Aborts if `langs` is zero, i.e. if no languages has been set yet.

0 value default-lang

  \ doc{
  \
  \ default-lang ( -- n )
  \
  \ A ``value`` that returns the language of the translation returned
  \ by constants created by `begin-translation` or
  \ `begin-noname-transalation` when the translation in the current
  \ language is not available, unless `default-translation` is set.
  \ Its default value is zero, i.e. the first language defined by the
  \ application.
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
  \ returned by the variables created by `begin-translation` or
  \ `begin-noname-translation`, whenever the translation in the
  \ current language is not available.
  \
  \ When the dynamic string pointed by _a_ is empty, which is the
  \ default, `default-lang` is used instead when the current
  \ translation of a `translation` variable is not available.
  \
  \ By storing an identificable string in ``default-translation``, missing
  \ translations can be traced application output.
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

  \ See: `end-translation`, `default-translation`, `default-lang`.
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
  \ `default-translation`, `default-lang`.
  \
  \ }doc

\ ==============================================================
\ Draft {{{1

false [if]

\ XXX OLD --  This code is a draft of the definitive implementation.

require ./dollar-comma.fs   \ `$,`

0 value default-lang

  \ default-lang ( -- n )
  \
  \ A ``value`` that returns the language that `l10n$` variables
  \ will use when the translation in the current language is not
  \ available, unless `default-l10n$` is set. Its default value is
  \ zero, i.e. the first language defined by the application.
  \
  \ See: `default-l10n$`, `lang`, `langs`.

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
\ Development notes {{{1

false [if]

\ 2019-03-11: Possible syntaxes considered and tried, from simple to
\ complex to implement:

\ The simplest one:

true
s" Hola"    spanish
s" Saluton" esperanto
s" Salute"  interlingue
s" Hello"   english
l10n-constant$ multilingual-salute$

\ More legible, and only a `rot` has to be added to the code:

true
spanish     s" Hola"
esperanto   s" Saluton"
interlingue s" Salute"
english     s" Hello"
l10n$ multilingual-salute$

\ Just syntactic sugar instead of `true`:

begin-l10n
  spanish     s" Hola"
  esperanto   s" Saluton"
  interlingue s" Salute"
  english     s" Hello"
end-l10n multilingual-salute$

\ No change, only clearer names:

begin-translation
  spanish     s" Hola"
  esperanto   s" Saluton"
  interlingue s" Salute"
  english     s" Hello"
end-translation multilingual-salute$

\ Final, a bit more complex to implement:

begin-translation multilingual-salute$
  spanish     s" Hola"
  esperanto   s" Saluton"
  interlingue s" Salute"
  english     s" Hello"
end-translation

[then]

\ ==============================================================
\ Change log {{{1

\ 2019-03-11: Write `begin-translation` and `begin-noname-translation`
\ as part of Fendo (http://programandala.net/en.program.fendo.html) to
\ replace its words `l10n-string` and `noname-l10n-string`.
\
\ 2019-03-14: Move the code from Fendo to Galope. It is usable by
\ other applications.
\
\ 2019-12-27: Fix documentation, which still mentioned its
\ original usage as part of Fendo.
