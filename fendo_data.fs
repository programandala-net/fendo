.( fendo_data.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

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

\ See at the end of the file.

\ **************************************************************
\ Todo

\ 2013-06-08 Can 'current_page' be used instead of ''data'?
\ 2013-06-07 Calculated data: rendered_title plain_title
\   ... same with description. Better: filter words!

\ **************************************************************
\ Requirements

require galope/slash-csv.fs  \ '/csv'

\ **************************************************************
\ Page data engine

variable current_data  \ address of the latest created data
: parse_datum  ( u "text<nl>" -- )
  \ Parse a datum, remove its leading spaces and store it. 
  \ u = datum offset
  >r  0 parse  \ parse the rest of the current input line
  trim
\  dup  if  ." Parsed datum: " 2dup type cr  then  \ xxx informer
  current_data @ r> + $!
  ;
variable in_data_header?  \ flag to let the data fields to disguise the context
variable /datum  \ offset of the current datum; at the end, length of the data
: :datum>value  ( ca len -- )
  \ Create a page metadatum that returns its value.
  \ This is the normal version of the metadatum: if executed in
  \ the metadata header (between 'data{' and '}data') it will
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
    @  in_data_header? @  ( u wf )
    if    ( u "datum<nl>" ) parse_datum
    else  ( a1 u ) + $@  then
  ;
: :datum>address  ( ca len -- )
  \ Create a page metadatum that returns the address of its data.
  \ ca len = datum name
  s" '" 2swap s+ nextname
  latestxt  \ of the word previously created by ':datum>value'
  create  ( xt ) >body ,
  does>  ( a1 -- a2 )
    \ a1 = page data address
    \ a2 = datum address
    \ dfa = data field address of the datum word
    \ u = datum offset
    ( a1 dfa )  @ @
\    dup ." datum offset = " .  \ xxx informer
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
datum: title  \ page title; can include markups
datum: plain_title  \ the same without markups
datum: description  \ page description; can include markups?
datum: plain_description  \ the same without markups

\ Dates in ISO format:
datum: created  \ creation date
datum: modifed  \ modification date

datum: access_key  \ access key (one char)

\ Hierarchy data, indicated with a page id (a page name):
datum: upper_page
datum: previous_page
datum: next_page
datum: first_page
datum: last_page

datum: keywords  \ list of HTML meta keywords, separated by commas
datum: tags  \ list of public tags, separated by commas
datum: properties  \ list of properties, separated by commas
datum: edit_summary  \ description of the latest changes

datum: related  \ list of page ids, xxx separated by commas?
datum: language_versions  \ list of page ids, xxx separated by commas?  \ xxx deprecated

datum: filename_extension  \ alternative target filename extension (with dot)

\ Design and template

\ Note: The actual design and template will be the default ones
\ unless the current page defines its own in its metadata:

datum: design_subdir  \ target relative path to the design, with final slash
datum: template  \ HTML template filename in the design subdir

\ **************************************************************
\ File names

0 value current_page  \ page id of the current page

: target_extension  ( a -- ca len )
  \ Return the target filename extension.
  \ a = page id
  filename_extension dup 0=
  if  2drop html_extension $@   then 
  ;
: current_target_extension  ( -- ca len )
  current_page target_extension 
  ;
: -forth_extension  ( ca len -- ca' len' )
  \ Remove the Forth extension from a filename.
  forth_extension $@ -suffix
  ;
: +forth_extension  ( ca len -- ca' len' )
  \ Add the Forth extension to a filename.
  forth_extension $@ s+
  ;
: source>target_extension  ( ca1 len1 -- ca2 len2 )
  \ Change the Forth extension to the current target extension.
  \ ca1 len1 = Forth source page filename
  \ ca2 len2 = target HTML page filename
  \ xxx todo rename to current_source>target ?
  -forth_extension current_target_extension s+
  ;
: /sourcefilename  ( -- ca len )
  \ Return the current source filename, without path.
  sourcefilename -path
  ;
: target_file  ( a -- ca len )
  \ Return a target HTML page filename. 
  \ a = page id
  \ ca len = target HTML page file name
  source_file source>target_extension 
  ;
: current_target_file  ( -- ca len )
  \ Return the target HTML page filename of the current page.
  \ ca len = target HTML page file name
  current_page target_file
  ;
: +target_dir  ( ca1 len1 -- ca2 len2 )
  \ Add the target path to a file name.
  \ ca1 len1 = file name
  \ ca2 len2 = file name, with its target local path.
  target_dir $@ 2swap s+
  ;
: target_path/file  ( a -- ca len )
  \ Return a target HTML page filename, with its local path.
  \ a = page id
  \ ca len = target HTML page file name, with its local path.
  target_file +target_dir 
\  2dup type cr  \ xxx informer
  ;

\ **************************************************************
\ Page id

\ The first time a page is interpreted, its data is parsed and
\ created (even if the content doesn't has to be parsered, e.g.
\ when the data has been required by other page).  Then a
\ page id is created: its name is source filename without
\ extension. The execution of the page id returns the address of
\ the page data, in order to access the individual data fields.

: "page-id"  ( -- ca len )
  \ Return the name of the current page id.
  /sourcefilename -extension
  ;
: :page_id  ( -- )
  \ Create the main page id and init its data space.
  "page-id" :create  here current_data !  /datum @ allot
  ;

\ **************************************************************
\ Page data header

defer set_default_data  ( -- )
  \ Set the default values of the page data.
: (set_default_data)  ( -- )
  \ Set the default values of the page data.
  \ xxx todo finish
  /sourcefilename 
\  2dup ." «" type ." »" \ xxx informer
  current_data @ 'source_file $!
  ;
' (set_default_data) is set_default_data
: }data  ( -- )
  \ Mark the end of the page data header and complete it.
  in_data_header? @ if  set_default_data  then
  in_data_header? off 
  ;
: skip_data{  ( xt "<text><space>}data" -- )
  \ Skip the page data.
  \ xt = execution token of the current page id
  execute to current_page
\  ." skip_data{" cr  \ xxx informer
  begin   parse-name dup 0=
    if    2drop refill 0= dup abort" Missing '}data'"
    else  s" }data" str=  then
  until   }data
  ;
: get_data{  ( "<text><space>}data" -- )
  \ Get the page data.
\  ." get_data{" cr  \ xxx informer
  :page_id
  current_data @ 
\  ." current_data copied to current_page =  " dup . cr  \ xxx informer
  to current_page
  in_data_header? on
  ;
: data{  ( "<text><spaces>}data" -- )
  \ Mark the start of the page data.
  \ xxx todo how to access the page ids in the markup?...
  \ xxx ...include them in the markup wordlist? create a wordlist?
\  cr cr ." =========== data{" cr  \ xxx informer
  "page-id" fendo_wid search-wordlist
  if  skip_data{  else  get_data{  then
  ;

variable do_content?  \ flag: do the page content? (otherwise, skip it)
do_content? on
: +source_dir  ( ca1 len1 -- ca2 len2 )
  \ Complete a source page filename with its path.
  source_dir $@ 2swap s+
  ;
: +current_dir  ( ca1 len1 -- ca2 len2 )  \ xxx tmp
  s" ./" 2swap s+
  ;
: .required_data_error  ( ca len -- )
  cr ." Error requiring the data of the page <" type ." >" cr
  ;
: required_data_error  ( ca len ior -- )
  >r .required_data_error r> throw
  ;
: (required_data)  ( ca len -- )
  \ Require a page file in order to get its data.
  \ ca len = filename
  do_content? off
  2dup  ['] required catch  ?dup
  if    nip nip required_data_error
  else  2drop  then
  ;
: required_data  ( ca len -- )
  \ Require a page file in order to get its data.
  \ ca len = filename
\  cr ." required_data " 2dup type  \ xxx informer
  do_content? @ >r  current_page >r
  (required_data)
  r> to current_page  r> do_content? !
  ;
: required_data<id  ( a -- )
  \ Require a page file in order to get its data.
  \ a = page id (address of its data)
  source_file required_data
  ;
: (required_data<id$)  ( ca len -- )
  \ Require a page file in order to get its data.
  \ ca len = page id
\  2dup cr type ."  to require in (required_data<id$)"  \ xxx informer
  +forth_extension required_data
  ;
: required_data<id$  ( ca len -- )
  \ Require a page file in order to get its data.
  \ ca len = page id
\  cr ." required_data<id$ " 2dup type cr  \ xxx informer
  unshortcut
\  cr ." required_data<id$ " 2dup type cr  \ xxx informer
  (required_data<id$)
  ;
: required_data<target  ( ca len -- )
  \ Require a page file in order to get its data.
  \ ca len = target file
  -extension required_data<id$
  ;
: require_data  ( "name" -- )
  \ Require a page file in order to get its data.
  \ "name" = filename
  parse-name? abort" File name expected in 'require_data'"
  required_data
  ;
: data<id$>id  ( ca len -- a )
  \ Require a page file in order to get its data
  \ and convert its string page id to number page id.
  \ ca len = page id
  \ a = page id
\  2drop s" es"  \ xxx tmp, works, no shortcut
\  2drop s" es.programa.sideras"  \ xxx tmp, works, one shortcut level
\  2drop s" samforth"  \ xxx tmp, works, one shortcut level
\  2drop s" local2"  \ xxx tmp, works, two shortcuts levels
\  2drop s" local3"  \ xxx tmp, works, three shortcuts levels
\  2drop s" en.program.samforth"  \ xxx tmp, works, no shortcut
\  2dup cr type ."  to unshortcut in data<id$>id"  \ xxx informer
\  key drop  \ xxx informer
  unshortcut  \ xxx tmp
  2dup (required_data<id$)
  evaluate  \ xxx first version
\  find-name name>int execute  \ xxx second version; no difference, same corruption of the input stream
\  cr ." end of data<id$>id"  \ xxx informer
  ;
: (data<)id$>id  ( ca len -- a )
  \ Return a number page id from a string page id;
  \ if it's different from the current page, require its data.
  \ This word is needed to manage links to the current page
  \ (href attributes that contain just an anchor).
  dup if  data<id$>id  else  2drop current_page  then
  ;

\ **************************************************************
\ Calculated data

: property?  ( ca len a -- wf )
             { D: property page_id }
  \ ca len = property to check
  \ a = page id (address of its data)
  \ wf = is the property in the properties field?
  page_id properties  false { result }
  /csv 0 ?do
    trim property str= result or to result
  loop  result
  ;
: draft?  ( a -- wf )
  \ a = page id (address of its data)
  \ wf = is "draft" in the properties field?
  s" draft" rot property?
  ;

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

0 [if]

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
\ 2013-07-28 New: 'required_data', '-forth_extension',
\   '+forth_extension'.
\ 2013-09-06 Fix: '(required_data)' didn't save 'current_page'.
\ 2013-09-06 New: 'property?', 'draft?'.
\ 2013-09-29 Fix: 'current_page' was not properly preserved
\   when 'require_data' was used. This caused many pages were
\   not created. This bug was difficult to find out.
\ 2013-10-22 Change: the new word 'trim', defined in the Galope
\   library, is used instead of '-trailing -leading'.
\ 2013-10-22 Fix: 'parse_datum' now uses 'trim' instead of
\   '-leading'.
\ 2013-10-22 New: 'data<id$>id'.
\ 2013-10-23 Improvement: 'unshortcut' is used in
\   'required_data<id$' and 'data<id$>id'.
\ 2013-11-06 New: '(data<)id$>id'.
\ 2013-11-07 New: '(required_data)' traps the errors and show an
\   additional error message that includes the filename. This is
\   important because the actual filename could be different from
\   the filename taken from the markup, because of the
\   "unshortcuttting" system.
\ 2013-11-11 Change: '/csv' moved to the Galope library.

[then]
