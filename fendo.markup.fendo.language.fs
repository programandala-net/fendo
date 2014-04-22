.( fendo.markup.fendo.language.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for languages.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ **************************************************************
\ Change history of this file

\ See at the end of the file.

\ **************************************************************
\ Custom language markup tools

fendo_definitions

\ xxx todo nested, with depth counter?

0 [if]

\ In order to markup the language of the contents, the website
\ application must create the language specific markups for the
\ required languages, this way:

  language_markup: eo
  language_markup: es

For every language markups are created: 'lang((' for inline zones and
'lang((((' for block zones.

Usage example:

  In English you say: Hello world! .
  In Esperanto you say: eo(( Saluton, mondo! )) .
  In Spanish you say: es(( ¡Hola mundo! )) .

[then]

: language_inline_markup  ( ca len -- )
  \ Create a language inline markup.
  \ ca len = ISO code of a language
  2dup s" ((" s+ :create_markup s,
  does>  ( -- )
    ( dfa ) count (xml:)lang=! [<span>]
  ;
\ XXX OLD
\ : ((:  ( "name" -- )
\   \ Create a language inline markup.
\   \ name = ISO code of a language
\   parse-name? abort" Missing language code" (((:)
\   ;
: language_block_markup  ( ca len -- )
  \ Create a language block markup.
  \ ca len = ISO code of a language
  2dup s" (((" s+ :create_markup s,
  does>  ( -- )
    ( dfa ) count (xml:)lang=! [<div>]
  ;
\ XXX old
\ : (((:  ( "name" -- )
\   \ Create a language block markup.
\   \ name = ISO code of a language
\   parse-name? abort" Missing language code" ((((:)
\   ;
: language_markup:  ( "name" -- )
  \ Create inline and block language markups.
  \ Used by the website application to create all
  \ language markups used in the contents.
  \ name = ISO code of a language
  parse-name? abort" Expected language code"
  2dup language_inline_markup language_block_markup
  ;

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.language.fs compiled ) cr

