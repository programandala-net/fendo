.( fendo_markup.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the markup. 

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

\ 2013-05-18 Start. First HTML tags.
\ 2013-06-01 Paragraphs, lists, headings, delete.
\ 2013-06-02 New: also 'previous_space?'.
\   New: Counters for both types of elements (markups and
\   printable words); required in order to separate words.
\ 2013-06-04 New: punctuation words, HTML entity words. More
\   markups.
\ 2013-06-04: Words to de/activate the parsing voc, in order to
\   mix Forth code in the sources. Maybe '[forth]' and '[fendo]'.
\ 2013-06-05 Change: '|' renamed to '_'; '|' will be needed for the
\   table markup.
\ 2013-06-05 New: Finished the code for entities; the common code for
\   entities and punctuation has been factored.
\ 2013-06-06 Change: HTML entities moved to <fendo_markup_html.fs>.

\ **************************************************************
\ Todo

\ 2013-06-04: Creole table markup.
\ 2013-06-04: Creole {{{...}}} markup.
\ 2013-06-04: Rewrite 'markup': accept the xt of the HTML tags,
\ not their names. This will let to add attributes to the Fendo
\ markup.
\ 2013-06-04: Nested lists.
\ 2013-06-04: Also '*' for lists?
\ 2013-06-04: Flag the first markup of the current line, in
\ order to use '--' both forth nested lists and delete, or '**'
\ for list and for bold.
\ 2013-06-05: Comments. '{*...*}'?
\ 2013-06-05: divide files: fendo_markup.fs includes
\ fendo_markup_html.fs and fendo_markup_own.fs ?

\ **************************************************************
\ Generic tool words for markup

\ xxx used in the parser only, but will be needed here too:
variable #markups   \ counter of consecutive markups
variable #printable \ counter of consecutive printable elements 

defer content
  \ Manage a string of content: print it and update the counters.
  \ Defined in the parser module.

: markups  ( xt1 xt2 a -- )
  \ Open or close a HTML tag.
  \ This code is based on FML, a Forth-ish Markup Language for RetroWiki.
  \ xt1 = execution token of the opening HTML tag
  \ xt2 = execution token of the closing HTML tag
  \ a = markup flag variable: is the markup already open?
  dup >r @
  if    nip false
  else  drop true
  then  r> !  execute
  ;

variable opened_[//]?         \ is there an open '//'?
variable opened_[**]?         \ is there an open '**'?
variable opened_[--]?         \ is there an open '--'?
variable opened_[__]?         \ is there an open '__'?
variable opened_[=]?          \ is there an open heading?
variable #header              \ level of the opened heading
variable opened_[_]?          \ is there an open '_'?
variable bullet_list_items    \ counter 
variable numbered_list_items  \ counter 
variable opened_[##]?         \ is there an open inline code?
variable opened_[###]?        \ is there an open block code?
variable opened_[""]?         \ is there an open inline quote?
variable opened_["""]?        \ is there an open block quote?

\ **************************************************************
\ Tool words for lists

: ((-))  ( a -- )
  \ List element.
  \ a = counter variable
  [fendo_markup_voc]
  dup @ if  </li>  then  <li>  1 swap +!  separate? off
  [previous]
  ;
: (-)  ( -- )
  \ Bullet list item.
  bullet_list_items ((-))
  ;
: (+)  ( -- )
  \ Numbered list item.
  numbered_list_items ((-))
  ;

\ **************************************************************
\ Tool words for punctuation

: :punctuation   ( ca len -- )
  \ Create a punctuation word.
  \ ca len = punctuation --and name of its punctuation word
  :echo_name  separate? on
  ;
: punctuation:   ( "name" -- )
  \ Create a punctuation word.
  \ "name" = punctuation --and name of its punctuation word
  parse-name? abort" Parseable name expected in 'punctuation:'"
  :punctuation
  ;

\ **************************************************************
\ Actual markup

also fendo_markup_voc definitions

\ Tool markup

: [+forth]  ( -- )
  \ Set the vocabularies for inserting Forth code in the page content.
  only fendo_voc also fendo_markup_voc also forth
  ;  immediate
: [-forth]  ( -- )
  \ Set the vocabularies for parsing the page content.
  only fendo_markup_voc
  ;  immediate

\ Fendo markup, inspired by Creole (http://wikicreole.org),
\ text2tags (http://text2tags.org) and others.

: //  ( -- )
  \ Start or finish a <em> region.
  ['] <em> ['] </em> opened_[//]? markups 
  ;
: **  ( -- )
  \ Start or finish a <strong> region.
  ['] <strong> ['] </strong> opened_[**]? markups
  ;
: --  ( -- )
  \ Start or finish a <del> region.
  ['] <del> ['] </del> opened_[--]? markups
  ;
: ""  ( -- )
  \ Start or finish a <q> region.
  ['] <q> ['] </q> opened_[""]? markups
  ;
: """  ( -- )
  \ Start or finish a <blockquote> region.
  ['] <blockquote> ['] </blockquote> opened_["""]? markups
  ;
: ##  ( )
  \ Start or finish an inline <code> region.
  \ xxx todo special parsing required in the region.
  ['] <code> ['] </code> opened_[##]? markups
  ;
: _  ( -- )
  \ Start or finish a <p> region.
  ['] <p> ['] </p> opened_[_]? markups  separate? off
  ;
: =  ( -- )
  \ Start or finish a <h1> region.
  ['] <h1> ['] </h1> opened_[=]? markups
  ;
: ==  ( -- )
  \ Start or finish a <h2> region.
  ['] <h2> ['] </h2> opened_[=]? markups
  ;
: ===  ( -- )
  \ Start or finish a <h3> region.
  ['] <h3> ['] </h3> opened_[=]? markups
  ;
: ====  ( -- )
  \ Start or finish a <h4> region.
  ['] <h4> ['] </h4> opened_[=]? markups
  ;
: =====  ( -- )
  \ Start or finish a <h5> region.
  ['] <h5> ['] </h5> opened_[=]? markups
  ;
: ======  ( -- )
  \ Start or finish a <h6> region.
  ['] <h6> ['] </h6> opened_[=]? markups
  ;

: \\  ( -- )
  \ Line break.
  [fendo_markup_voc] <br/> [previous]
  separate? off
  ;
: -  ( -- )
  \ Bullet list item.
  bullet_list_items @ 0= if  <ul>  then  (-)
  ;
: +  ( -- )
  \ Numbered list item.
  numbered_list_items @ 0= if  <ol>  then  (+)
  ;
: }content  ( -- )
  \ Mark the end of the page content. 
  \ xxx todo 'do_content?' is useless when there are several content
  \ blocks in one page!
  do_content? on
  ;
: ----  ( -- )
  \ Create a horizontal line.
  [fendo_markup_voc] <hr/> [previous]
  separate? off
  ;

\ Escape

: ~  ( "name" -- )
  \ Escape a name: Parse and echo it, even if it's a markup.
  parse-name? abort" Parseable name expected in '~'"
  content  
  ; 

\ Punctuation

punctuation: .
punctuation: ,
punctuation: ;
punctuation: :
punctuation: ...
punctuation: !
punctuation: ?
\ xxx needed?:
punctuation: )
punctuation: "
punctuation: '
punctuation: »
punctuation: ]
punctuation: }

previous 
fendo_voc definitions

.( fendo_markup.fs compiled) cr
