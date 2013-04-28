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

\ **************************************************************
\ Page data header

variable get_data?  \ flag: get the page data? (otherwise, skip it)
variable in_data_header?  \ flag to let the data fields to disguise the context
: skip_data{  ( "text }data" -- )
  \ Skip the page data.
  begin   parse-word dup 0=
    if    2drop refill 0= dup abort" Missing '}data'"
    else  s" }data" str=
    then
  until 
  ;
: get_data{  ( "name" -- )
  in_data_header? on
  ;
: data{  ( "name text }data" -- )
  \ Mark the start of the page data.
  \ "name" = page data id, usually the page name (its file name
  \   without the file name extension)
  \ xxx todo
  get_data? @ if  get_data{  else  skip_data{  then
  ;

: }data  ( -- )
  \ Mark the end of the page data.
  in_data_header? off
  ;

\ **************************************************************
\ Page data fields

: datum!  ( a "text<nl>" -- )
  \ Parse and store a datum.
  \ a = datum address
  \ ca u = parsed string
  \ a1 = definitive address of the string in memory
  0 parse ( a ca u )  \ parse the rest of the current input line
  dup allocate throw  ( a ca u a1 )
  dup >r place r> swap ! 
  ;
: datum@  ( a -- ca u )
  \ Fetch a datum.
  \ a = datum address 
  @ count
  ;
: datum:  ( "name" -- )
  \ Create a new page datum field.
  create  ( "name" -- )
  does>
    in_data_header? @
    if    ( pfa "text<nl>" ) datum!
    else  ( pfa ) datum@
    then
  ;

  \ xxx todo
  datum: language  \ ISO code of the page's language
  datum: title
  datum: description
  datum: keywords  \ list of keywords, separated by commas
  datum: button  \ link text used as botton (e.g. in 
  datum: button-tittle  \ page
  datum: created  \ creation date (in YYYY[MM[DD[HH[MM]]]] format)
  datum: modifed  \ modification date (in YYYY[MM[DD[HH[MM]]]] format)
  datum: access-key  \ access key (one char)
  \ hierarchy data, indicated with a page id (a page name):
  datum: up
  datum: prev
  datum: next
  datum: first
  datum: last

