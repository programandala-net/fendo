\ fendo_data.fs

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the page data tools.

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

\ 2013-04-28 Start.
\ 2013-05-01 Fixed and finished the data system.

\ **************************************************************
\ Page data engine

variable 'data  \ address of the latest created data
: >datum  ( a1 a2 -- a3 )
  \ Convert data and datum addresses to the actual datum
  \ address.
  \ a1 = page data address
  \ a2 = datum offset address
  \ a3 = actual datum address
  @ +
  ;
: datum!  ( a "text<nl>" -- )
  \ Parse and store a datum.
  \ a = datum address (it contains the datum offset)
  \ ca u = parsed string
  \ a1 = definitive memory address of the string
  \ xxx todo non-parsing version, with empty check for freeing
  0 parse ( a ca u )  \ parse the rest of the current input line
  dup 1+ allocate throw  ( a ca u a1 )
  dup >r place r>  ( a a1 )
  'data @ rot >datum !
  ;
: datum@  ( a1 a2 -- ca u )
  \ Fetch a datum.
  \ a1 = page data address 
  \ a2 = address of the datum offset
  \ ca u = datum 
  >datum @ count
  ;
variable in_data_header?  \ flag to let the data fields to disguise the context
variable >datum  \ byte offset of the currently created datum
: datum:  ( "name" -- )
  \ Create a new page datum field.
  create  ( "name" -- )
    \ "name" = field name
    cell >datum dup @ , +!  \ store the offset and increment it
  does>
    in_data_header? @
    if    ( pfa "text<nl>" ) datum!
    else  ( a pfa ) datum@
    then
  ;

\ **************************************************************
\ Page data fields

datum: language  \ ISO code of the page's language
datum: title
datum: description

\ For special links (e.g. in breadcrumbs or menus):
datum: button  \ link text 
datum: button_tittle  \ link title
' button_tittle alias button-title

\ Dates in YYYY[MM[DD[HH[MM]]]] format:
datum: created  \ creation date
datum: modifed  \ modification date

datum: access_key  \ access key (one char)
' access_key alias access-key

\ Hierarchy data, indicated with a page id (a page name):
datum: up
datum: prev
datum: next
datum: first
datum: last

datum: keywords  \ list of keywords, separated by commas

datum: related  \ list of page ids, xxx separated by commas?
datum: language_versions  \ list of page ids, xxx separated by commas?

\ **************************************************************
\ Page data header

variable get_data?  \ flag: get the page data? (otherwise, skip it)
get_data? on  \ default value
: }data  ( -- )
  \ Mark the end of the page data and finish the data header.
  in_data_header? off  \ end of header
  get_data? on  \ default value
  ;
: skip_data{  ( "text }data" -- )
  \ Skip the page data.
  begin   parse-word dup 0=
    if    2drop refill 0= dup abort" Missing '}data'"
    else  s" }data" str=
    then
  until   }data
  ;
: get_data{  ( "name" -- a )
  \ Get the page data.
  \ a = pfa of the page data id
  in_data_header? on
  create  here 'data !  >datum @ allot
  ;
: data{  ( -- )
  \ Mark the start of the page data.
  get_data? @ if  get_data{  else  skip_data{  then
  ;

