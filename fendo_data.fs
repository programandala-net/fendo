.( fendo_data.fs ) 

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-01.

\ File changed 2013-06-28 18

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
\ 2013-06-08 Fix: '@' missing in 'default_data'; beside, renamed
\   to "set_default_data'.
\ 2013-06-08 Change: 'datum@' and 'datum!' are removed;
\   '$@' and '$!' are used instead (from Gforth's <string.fs>)).
\ 2013-06-08 Fix: name clash (old 'source_filename' >
\   '+source_path'; new 'source_filename' > '/sourcefilename').
\ 2013-06-23 Change: design and template fields are renamed
\   after the changes in the config module.
\ 2013-06-28 Change: hierarchy metadata fields are renamed with
\   the "_page" prefix, to avoid the clash with 'next' and make
\   the code clearer; 'up' is renamed to 'upper_page'.
\ 2013-06-28 Change: metadata fields return their values, not
\   their addresses; a parallel word is created to return the
\   address, only needed to set the default data;
\   this change makes the code nicer.
\ 2013-06-29 Change: '/sourcefilename' moved here from
\   <fendo_files.fs>.
\ 2013-06-29 Change: 'source>target_extension' moved here
\   from <fendo_files.fs>.
\ 2013-06-29 New: 'target_extension'; now target filename extension
\   depends on the corresponding optional metadatum too.

\ **************************************************************
\ Todo

\ 2013-06-08 Can 'current_page' be used instead of ''data'?
\ 2013-06-07 Calculated data: rendered_title plain_title
\   ... same with description. Better: filter words!

\ **************************************************************
\ Page data engine

variable current_data  \ address of the latest created data
: parse_datum  ( u "text<nl>" -- )
  \ Parse a datum, remove its leading spaces and store it. 
  \ u = datum offset
  >r  0 parse  \ parse the rest of the current input line
  -leading  
\  dup  if  ." Parsed datum: " 2dup type cr  then  \ xxx debug check
  current_data @ r> + $!
  ;
variable in_data_header?  \ flag to let the data fields to disguise the context
variable /datum  \ offset of the current datum; at the end, length of the data
: :datum>value  ( ca len -- )
  \ Create a page metadatum that returns its value.
  \ This is the normal version of the metadatum: if executed in
  \ the metada header (between 'data{' and '}data') it will
  \ parse its datum from the input stream; out of the header it
  \ will return the datum string.
  \ ca len = datum name
  nextname create
    cell /datum dup @ , +!  \ store the offset and increment it
  does>  ( a1 | "text<nl>" -- a2 | )
    \ a1 = page data address
    \ a2 = datum address
    \ dfa = data field address of the datum word
    \ u = datum offset
    ( a1 dfa | dfa "text<nl>" )
    @  in_data_header? @  ( u ff )
    if    ( u "datum<nl>" ) parse_datum
    else  ( a1 u ) + $@
    then
  ;
: :datum>address  ( ca len -- )
  \ Create a page metadatum that returns the address of its data.
  \ ca len = datum name
  s" '" 2swap s+ nextname
  latestxt  \ of the word previously created by ':datum>value'
  create  >body ,
  does>  ( a1 -- a2 )
    \ a1 = page data address
    \ a2 = datum address
    \ dfa = data field address of the datum word
    \ u = datum offset
    ( a1 dfa ) 
    @ @
\    dup ." datum offset = " .  \ xxx debug check
    ( a1 u ) +
  ;
: datum:  ( "name" -- )
  \ Create a page metadatum.
  parse-name 2dup :datum>value :datum>address 
  ;

\ **************************************************************
\ Page data fields

datum: source_file

datum: language  \ ISO code of the page's language
datum: title  \ page title; xxx can include markups?
datum: description  \ page description; xxx can include markups?

\ Dates in ISO format:
datum: created  \ creation date
datum: modifed  \ modification date

datum: access_key  \ access key (one char)

\ Hierarchy data, indicated with a page-id (a page name):
datum: upper_page
datum: previous_page
datum: next_page
datum: first_page
datum: last_page

datum: keywords  \ list of HTML meta keywords, separated by commas
datum: tags  \ list of public tags, separated by commas
datum: properties  \ list of properties, separated by commas
datum: edit_summary  \ description of the latest changes

datum: related  \ list of page-ids, xxx separated by commas?
datum: language_versions  \ list of page-ids, xxx separated by commas?

datum: filename_extension  \ alternative target filename extension (with dot)

\ Design and template

\ Note: The actual design and template will be the default ones
\ unless the current page defines its own in its metadata:

datum: design_subdir  \ target relative path to the design, with final slash
datum: template  \ HTML template filename in the design subdir

\ **************************************************************
\ Datum management

: /csv  ( ca len -- ca#1 len#1 ... ca#u len#u u )
  \ Divide a comma separated values in its values.
  \ ca len = string with comma separated values
  \ ca#1 len#1 ... ca#u len#u = one or more strings
  \ u = number of string returned
  depth >r
  begin  s" ," /sides 0=  until  2drop
  depth r> 2 - - 2/
  ;

\ **************************************************************
\ File names

0 value current_page  \ page-id of the current page

: target_extension  ( a -- ca len )
  \ Return the target filename extension.
  \ a = page id
  filename_extension dup 0=
  if  2drop html_extension $@   then 
  ;
: current_target_extension  ( -- ca len )
  current_page target_extension 
  ;
: source>target_extension  ( ca1 len1 -- ca2 len2 )
  \ ca1 len1 = Forth source page file name
  \ ca2 len2 = target HTML page file name
  forth_extension $@ -suffix  current_target_extension s+
  ;
: /sourcefilename  ( -- ca len )
  \ Return the current source filename, without path.
  sourcefilename -path
  ;

\ **************************************************************
\ Page-ids

\ The first time a page is interpreted, its data is parsed and
\ created, even if the content doesn't has to be parsered, in
\ case the data has been required by other page).  Then a
\ page-id and two aliases are created, based on the source
\ filename. The execution of the page-id returns the address of
\ the page data, in order to access the individual data fields.

: "page-id"  ( -- ca len )
  \ Return the name of the main page-id.
  /sourcefilename -extension
  ;
: :page_id  ( -- )
  \ Create the main page-id and init its data space.
  "page-id" :create  here current_data !  /datum @ allot
  ;
0 [if]  \ xxx old
: :page_id_aliases  { xt -- }
  \ Create aliases of the main page-id.
  \ xt = execution token of the main page-id
  xt /sourcefilename :alias  \ with the original source extension
  xt /sourcefilename source>target_extension :alias  \ with target extension
  ;
: :page_ids  ( -- )
  \ Create the default page-ids.
  :page_id  latestxt :page_id_aliases
  ;
[then]

\ **************************************************************
\ Page data header

defer set_default_data  ( -- )
  \ Set the default values of the page data.
: (set_default_data)  ( -- )
  \ Set the default values of the page data.
  \ xxx todo finish
  /sourcefilename 
\  2dup ." «" type ." »" \ xxx debug check
  current_data @ 'source_file $!
  ;
' (set_default_data) is set_default_data
: }data  ( -- )
  \ Mark the end of the page data header and complete it.
  in_data_header? @
  if    set_default_data
  then  in_data_header? off 
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
  :page_id
  current_data @ to current_page
  in_data_header? on
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
: +source_dir  ( ca1 len1 -- ca2 len2 )
  \ Complete a source page filename with its path.
  source_dir $@ 2swap s+
  ;
: require_data  ( "name" -- )
  \ Require a page file in order to get its data.
  \ "name" = file name
  do_content? @  do_content? off
  parse-name? abort" Filename expected in 'require_data'"
  +source_dir required
  do_content? !
  ;

\ **************************************************************
\ Calculated data

: (hierarchy)  ( ca len -- u )
  \ Return the hierarchy level of a page (0 is the top level).
  \ ca len = page id (source page filename without extension)
  \ (The method used works also with UTF-8 strings).
  0 rot rot  \ counter
  bounds ?do
    i c@ [char] . = abs +
  loop
  ;
: hierarchy  ( a -- u )
  \ Return the hierarchy level of a page (0 is the top level).
  \ a = page id (address of its data)
  source_file (hierarchy) 1-
  ;

.( fendo_data.fs compiled) cr
