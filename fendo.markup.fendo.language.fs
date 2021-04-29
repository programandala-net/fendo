.( fendo.markup.fendo.language.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for languages.

\ Last modified  20210429T1659+0200.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018,2021 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Requirements {{{1

forth_definitions

require galope/s-comma.fs \ `s,`

\ ==============================================================
\ Custom language markup tools {{{1

fendo_definitions

\ XXX TODO nested, with depth counter?

0 [if]

\ In order to markup the language of the contents, the website
\ application must create the language specific markups for the
\ required languages, this way:

  language_markup: eo
  language_markup: es

For every language, the following markups are created:

- `lang((` for inline zones
- `lang((((` for block zones
- `lang''` for inline quotes
- `lang''''` for block quotes

Usage example:

  In English you say: Hello world!
  In Esperanto you say eo'' Saluton, mondo! ''
  In Spanish you say: es'' ¡Hola mundo! ''
  Please do it la(( ipso facto )) , es(( gracias )) .

[then]

: language_inline_markup ( ca len -- )
  2dup s" ((" s+ :create_markup s,
  does> ( -- ) ( dfa ) count (xml:)lang=! [<span>] ;
  \ Create a language inline markup.
  \ ca len = ISO code of a language

: language_inline_quote_markup ( ca len -- )
  2dup s" ''" s+ :create_markup s,
  does> ( -- ) ( dfa ) count (xml:)lang=! [markup>order] '' [markup<order] ;
  \ Create a language inline quote markup.
  \ ca len = ISO code of a language

: language_block_markup ( ca len -- )
  2dup s" ((((" s+ :create_markup s,
  does> ( -- ) ( dfa ) count (xml:)lang=! [<div>] ;
  \ Create a language block markup.
  \ ca len = ISO code of a language

: language_block_quote_markup ( ca len -- )
  2dup s" ''''" s+ :create_markup s,
  does> ( -- ) ( dfa ) count (xml:)lang=! [markup>order] '''' [markup<order] ;
  \ Create a language block quote markup.
  \ ca len = ISO code of a language

: language_markup: ( "name" -- )
  parse-name? abort" Expected language code"
  2dup language_inline_markup  2dup language_inline_quote_markup
  2dup language_block_markup  language_block_quote_markup ;
  \ Create inline and block language and quote markups.
  \ Used by the website application to create all
  \ language markups used in the contents.
  \ name = ISO code of a language

.( fendo.markup.fendo.language.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2014-06-06: `)))` changed to `))))`, after the last changes in
\ Fendo's markup.
\
\ 2014-07-13: New: inline and block quotes.
\
\ 2014-11-09: The usage text is updated.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-06: Improve documentation.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2021-04-29: Require `s,`, word removed from Gforth between versions
\ 0.7.9_20201231 and 0.7.9_20210422, and added to Galope.

\ vim: filetype=gforth
