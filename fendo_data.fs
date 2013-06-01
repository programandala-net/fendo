.( fendo_data.fs) cr

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
\ 2013-05-17 Fix: There were two words with the name '>datum';
\   it caused no problem in practice, but was confusing.
\ 2013-05-17 Improvement: 'data{' gets the data only the first time.
\ 2013-05-17 New: 'require_data' is moved here from its own file,
\   and simplified. 
\ 2013-05-18 Change: data fields return their offset, not their
\   content (neccessary to write 'datum!'; '>datum' removed
\   (now '+' can be used instead). 'datum!' is necessary in
\   order to set default values to certain fields.
\ 2013-05-18 New: 'parse_datum' is rewriten and factored out to
\   'datum!'.

\ **************************************************************
\ Requirements

require galope/bracket-true.fs  \ '[true]'
require galope/bracket-false.fs  \ '[false]'

\ **************************************************************
\ Page data engine

variable 'data  \ address of the latest created data
: datum!  ( ca len a -- )
  \ Store a datum.
  \ ca len = datum 
  \ a = datum address
  \ xxx todo free the memory
  >r  dup 1+ allocate throw  ( ca len a1 )
  dup >r place 
  r> r> !
  ;
: parse_datum  ( u "text<nl>" -- )
  \ Parse and store a datum.
  \ u = datum offset
  >r  0 parse \ parse the rest of the current input line
  'data @ r> + datum!
  ;
: datum@  ( a -- ca len )
  \ Fetch a datum.
  \ a = datum address 
  \ ca len = datum 
  @ count
  ;
variable in_data_header?  \ flag to let the data fields to disguise the context
variable /datum  \ byte offset of the currently created datum
: datum:  ( "name" -- )
  \ Create a new page datum field.
  create  ( "name" -- )
    \ "name" = field name
    cell /datum dup @ , +!  \ store the offset and increment it
  does>  ( a1 dfa | dfa "text<nl>" -- a2 | )
    \ dfa = data field address of the datum word
    \ u = datum offset
    \ a1 = page data address
    \ a2 = datum address
    @  in_data_header? @  ( u flag )
    if    ( u "datum<nl>" -- ) parse_datum
    else  ( a1 u -- a2 ) +
    then
  ;

\ **************************************************************
\ Page data fields

datum: source_file

datum: language  \ ISO code of the page's language
datum: title
datum: description

\ For special links (e.g. in breadcrumbs or menus):
datum: button  \ link text 
datum: button_tittle  \ link title
' button_tittle alias button-title

\ Dates in ISO format:
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
datum: tags  \ list of tags, separated by commas
datum: status  \ list of status ids, separated by commas

datum: related  \ list of page ids, xxx separated by commas?
datum: language_versions  \ list of page ids, xxx separated by commas?

\ **************************************************************
\ Page calculated data fields

: target_file  ( a -- ca len )
  \ a -- page id
  \ ca len = target HTML page file name
  source_file datum@ source>target
  ;

\ **************************************************************
\ Page data header

defer default_data
: (default_data)  ( -- )
  \ Set the default values of the page data.
  \ xxx todo finish
  sourcefilename 'data source_file datum!
  ;
' (default_data) is default_data

: }data  ( -- )
  \ Mark the end of the page data and finish the data header.
  in_data_header? off  \ end of header
  default_data
  ;
: skip_data{  ( "text }data" -- )
  \ Skip the page data.
  begin   parse-name dup 0=
    if    2drop refill 0= dup abort" Missing '}data'"
    else  s" }data" str=
    then
  until   }data
  ;

: create_name  ( ca len -- )
  \ Create a 'create' word with the given name.
  nextname create
  ;
: alias_alias  ( ca len -- )
  \ Create an 'alias' with the given name.
  nextname alias
  ;
: create_page_id  ( -- )
  \ Create the default page ids.
  sourcefilename create_name  \ the name is the source file name
  [false] [if]
    \ xxx todo
    latestxt
    dup create_alias
    dup -extension 2dup create_name  \ the source file name without extension
  [then]
  ;

: get_data{  ( "<text><spaces>}data" -- )
  \ Get the page data.
  create_page_id  here 'data !  /datum @ allot
  in_data_header? on
  ;
: data{  ( "<text><spaces>}data" -- )
  \ Mark the start of the page data.
  save-input parse-name find-name >r
  restore-input abort" 'restore-input' failed"
  r> if  skip_data{  else  get_data{  then
  ;

variable do_content?  \ flag: do the page content? (otherwise, skip it)
do_content? on
: require_data  ( "name" -- )
  \ Require a page file in order to get its data.
  \ "name" = file name
  do_content? dup @ swap off  require  do_content? !
  ;

.( fendo_data.fs compiled) cr
