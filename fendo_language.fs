.( fendo_language.fs) cr

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

\ 2013-11-11 Code extracted from <addons/multilingual.fs>,
\   because it's needed in <fendo_markup_wiki.fs>.

\ **************************************************************
\ Requirements

require galope/c-slash-string.fs  \ 'c/string'

\ **************************************************************

: lang_prefix  ( ca1 len1 -- ca2 len2 )
  \ Get the ISO language code from the first part of a source page file.
  \ ca1 len1 = page id
  \ ca2 len2 = ISO language code
  [char] . c/string
  ;
: lang  { page_id -- ca len }
  \ Return the ISO language code of the given page.
  \ The "language" metadatum has higher priority than the filename's
  \ prefix.
  page_id language dup ?exit  2drop
  page_id source_file lang_prefix
  ;
: current_lang  ( -- ca len )
  \ Return the ISO language code of the current page.
  current_page lang
  ;

.( fendo_language.fs compiled) cr

