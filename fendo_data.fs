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
\ 2013-05-17 Fix: There were two words with the name '>datum';
\   it caused no problem in practice, but was confusing.
\ 2013-05-17 Improvement: 'data{' gets the data only the first time.
\ 2013-05-17 New: 'require_data' is moved here from its own file,
\   and simplified. 

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
  \ ca len = parsed string
  \ a1 = definitive memory address of the string
  \ xxx todo non-parsing version, with empty check for freeing
  0 parse ( a ca len )  \ parse the rest of the current input line
  dup 1+ allocate throw  ( a ca len a1 )
  dup >r place r>  ( a a1 )
  'data @ rot >datum !
  ;
: datum@  ( a1 a2 -- ca len )
  \ Fetch a datum.
  \ a1 = page data address 
  \ a2 = address of the datum offset
  \ ca len = datum 
  >datum @ count
  ;
variable in_data_header?  \ flag to let the data fields to disguise the context
variable /datum  \ byte offset of the currently created datum
: datum:  ( "name" -- )
  \ Create a new page datum field.
  create  ( "name" -- )
    \ "name" = field name
    cell /datum dup @ , +!  \ store the offset and increment it
  does>
    in_data_header? @
    \ df = data field (parameter field address)
    if    ( df "text<nl>" ) datum!
    else  ( a df ) datum@
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

defer default_data
: (default_data)  ( -- )
  \ Set the default values of the page data.
  sourcefilename 
  ;
' (default_data) is default_data

variable get_data?  \ flag: get the page data? (otherwise, skip it) \ xxx old
get_data? on  \ default value
: }data  ( -- )
  \ Mark the end of the page data and finish the data header.
  in_data_header? off  \ end of header
  get_data? on  \ default value \ xxx old
  ;
: skip_data{  ( "text }data" -- )
  \ Skip the page data.
  begin   parse-word dup 0=
    if    2drop refill 0= dup abort" Missing '}data'"
    else  s" }data" str=
    then
  until   }data
  ;

false [if]  \ first version \ xxx old

\ The problem with this version is the variable 'get_data?' is
\ used to decide wheter the data has to be parsed or not.  It's
\ simpler and enough to check if the data name already exists.

: get_data{  ( "name" -- a )
  \ Get the page data.
  \ a = df of the page data id
  create  here 'data !  /datum @ allot
  in_data_header? on
  ;
: data{  ( "name" | "text }data" -- )
  \ Mark the start of the page data.
  get_data? @ if  get_data{  else  skip_data{  then
  ;

[then]
 
false [if]  \ second, new version, unfinished \ xxx old

\ This version parses the data id and searches for it.  But in
\ order to create a new word from a string, same low-level
\ operations are required, since Gforth provides no word to do
\ it directly.  It seemed simpler and safer to manipulate the
\ source pointer (third version).

: get_data{  ( a1 u1 -- a2 )
  \ Get the page data.
  \ a1 u1 = page data id name
  \ a2 = page data id address
  name-too-short? header,
  \ xxx todo finish, missing code here
  /datum @ allot
  in_data_header? on
  ;
: data{  ( "name<text><}data>" -- )
  \ Mark the start of the page data.
  parse-name 2dup find-name
  if    2drop skip_data{
  else  get_data{
  then
  ;

[then]

true [if]  \ third version

\ This version parses the data id and searches for it, and then
\ restores the input in order to be able to parse the name again
\ if needed.

: get_data{  ( "<name><text><spaces>}data" -- )
  \ Get the page data.
  create  here 'data !  /datum @ allot
  in_data_header? on
  ;
: data{  ( "<name><text><spaces>}data" -- )
  \ Mark the start of the page data.
  save-input parse-name find-name >r
  restore-input abort" 'restore-input' failed"
  r> if  skip_data{  else  get_data{  then
  ;

[then]

variable do_content?  \ flag: do the page content? (otherwise, skip it)
: require_data  ( "name" -- )
  \ Require a page file in order to get its data.
  \ "name" = file name
  do_content? dup @ swap off  require  get_content? !
  ;
