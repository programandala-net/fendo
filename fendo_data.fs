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
\ 2013-06-07 Fix: The check in 'data{' was obsolete; it has been
\   rewritten.
\ 2013-06-08 Fix: The leading spaces of parsed data were not
\   removed.
\ 2013-06-08 Fix: now 'datum@' returns an empty string if the
\   datum was not set.
\ 2013-06-08 Fix: '@' missing in 'default_data'.

\ **************************************************************
\ Todo

\ 2013-06-07 Calculated data: rendered_title plain_title
\   ... same with description. Better: filter words!

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
  >r  0 parse  \ parse the rest of the current input line
  -leading  \ no leading spaces
  'data @ r> + datum!
  ;
: datum@  ( a -- ca len )
  \ Fetch a datum.
  \ a = datum address 
  \ ca len = datum 
  @ ?dup if  count  else  pad 0  then
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
    @  in_data_header? @  ( u ff )
    if    ( u "datum<nl>" -- ) parse_datum
    else  ( a1 u -- a2 ) + 
    then
  ;

\ **************************************************************
\ Page data fields

datum: source_file

datum: language     \ ISO code of the page's language
datum: title        \ page title; xxx can include markups?
datum: description  \ page description; xxx can include markups?

\ For special links (e.g. in breadcrumbs or menus):
datum: button         \ link text 
datum: button_tittle  \ link title
' button_tittle alias button-title

\ Dates in ISO format:
datum: created  \ creation date
datum: modifed  \ modification date

datum: access_key  \ access key (one char)
' access_key alias access-key

\ Hierarchy data, indicated with a page-id (a page name):
datum: up
datum: prev
datum: next
datum: first
datum: last

datum: keywords   \ list of keywords, separated by commas
datum: tags       \ list of tags, separated by commas
datum: status     \ list of status ids, separated by commas

datum: related            \ list of page-ids, xxx separated by commas?
datum: language_versions  \ list of page-ids, xxx separated by commas?

datum: design_dir   \ design directory in the designs directory, with final slash
datum: template     \ HTML template file

\ **************************************************************
\ Page-ids

\ The first time a page is interpreted, its data is parsed and
\ created, also when the content doesn't has to be parsered.
\ Then a page-id and two aliases are created, based on the
\ source filename. The execution of the page-id returns the
\ address of the page data, in order to access the individual
\ data fields.

: "page-id"  ( -- ca len )
  \ Return the name of the main page-id.
  \ ca len = name of the main page-id
  sourcefilename -extension
  \ 2dup cr ." page-id = " type cr  \ xxx debugging
  ;
: :page_id  ( -- )
  \ Create the main page-id and init its data space.
  "page-id" :create  here 'data !  /datum @ allot
  ;
: :page_id_aliases  { xt -- }
  \ Create aliases of the main page-id.
  \ xt = execution token of the main page-id
  xt sourcefilename :alias
  xt sourcefilename source>target_extension :alias
  ;
: :page_ids  ( -- )
  \ Create the default page-ids.
  :page_id  latestxt :page_id_aliases
  ;

\ **************************************************************
\ Page data header

0 value current_page  \ page-id of the current page

defer default_data  ( -- )
  \ Set the default values of the page data.
:noname  ( -- )
  \ Set the default values of the page data.
  \ xxx todo finish
  sourcefilename -path 'data @ source_file datum!
  ;
is default_data
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
: get_data{  ( "<text><spaces>}data" -- )
  \ Get the page data.
  :page_ids  'data @ to current_page  in_data_header? on
  ;
: data{  ( "<text><spaces>}data" -- )
  \ Mark the start of the page data.
  [false] [if]  \ xxx old and buggy
    save-input parse-name find-name >r
    restore-input abort" 'restore-input' failed in 'data{'"
    r> if  skip_data{  else  get_data{  then
  [else]  \ xxx new
    \ xxx how to access the page-ids in the markup?
    \ xxx include them in the markup wordlist? create a wordlist?
    "page-id" fendo_wid search-wordlist
    if  drop skip_data{  else  get_data{  then
  [then]
  ;

variable do_content?  \ flag: do the page content? (otherwise, skip it)
do_content? on
: source_filename  ( ca1 len1 -- ca2 len2 )
  \ Complete a source page filename with its path.
  source_dir count 2swap s+
  ;
: require_data  ( "name" -- )
  \ Require a page file in order to get its data.
  \ "name" = file name
  do_content? @  do_content? off
  parse-name? abort" Filename expected in 'require_data'"
  source_filename required
  do_content? !
  ;

.( fendo_data.fs compiled) cr
