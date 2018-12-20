.( fendo.data.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the page data tools.

\ Last modified 201812201916.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2015,2017,2018 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Requirements

forth_definitions

require ./fendo.addon.traverse_pids.fs

\ From Galope
require galope/char-count.fs \ `char-count`
require galope/minus-common-prefix.fs \ `-common-prefix`
require galope/minus-extension.fs \ `-extension`
require galope/slash-ssv.fs \ `/ssv`
require galope/file-mtime.fs \ `file-mtime`
require galope/file-exists-question.fs \ `file-exists?`

\ From Fourth Foundation Library
require ffl/str.fs \ dynamic strings

fendo_definitions

\ ==============================================================
\ Page data engine

variable data_fields  \ counter

64 constant max_data_fields

max_data_fields cells buffer: fields_body_table

: erase_data ( -- )
  \ XXX TODO
  fields_body_table max_data_fields bounds ?do
    -1 i !
  cell +loop ;

variable current_data  \ address of the latest created data

: parse_datum ( u "text<nl>" -- )
  >r  0 parse  \ parse the rest of the current input line
  trim
\  dup  if  ." Parsed datum: " 2dup type cr  then  \ XXX INFORMER
  current_data @ r> + $! ;
  \ Parse a datum and store it.
  \ u = datum offset

variable in_data_header?
  \ flag to let the data fields to disguise the context

variable /datum
  \ offset of the current datum; at the end, length of the data

defer get_datum ( a u -- ca len )

: (get_datum) ( a u -- ca len )
  \ ." Input of `(get_datum)` :" .s cr \ XXX INFORMER
  + $@
  \ ." Output of `(get_datum)` :" 2dup type cr \ XXX INFORMER
  ;

' (get_datum) is get_datum

: :datum ( ca len -- )
  nextname create
    cell /datum dup @ , +!  \ store the offset and increment it
  does> ( a1 | "text<nl>" -- ca len | )
    \ a1 = page data address
    \ ca len = datum
    \ dfa = data field address of the datum word
    \ u = datum offset
    ( a1 dfa | dfa "text<nl>" )
    @  in_data_header? @ ( u f )
    if   ( u "datum<nl>" ) parse_datum
    else ( a1 u )          get_datum  then ;
  \ Create a page metadatum that parses or returns its value.
  \ This is the normal version of the metadatum: when executed in the
  \ metadata header (between `data{` and `}data`), it will get its
  \ datum from the input stream, until the end of the line, and will
  \ store it; when executed out of the metadata header, it will return
  \ the datum string.
  \ ca len = datum name

: :datum>address ( ca len -- )
  s" '" 2swap s+ nextname
  latestxt  \ of the word previously created by `:datum`
  create ( xt ) >body ,
  does> ( a1 -- a2 )
    ( a1 dfa ) @ @ ( a1 u ) + ;
  \ Create a page metadatum word that returns the address of its data.
  \ The new name will have a tick at the start.
  \ ca len = datum name
  \ a1 = page data address
  \ a2 = datum address (a dynamic string that can be updated by `$!`)
  \ dfa = data field address of the datum word
  \ u = datum offset

: datum: ( "name" -- )
  parse-name 2dup :datum :datum>address ;
  \ Create a page metadatum.

\ ==============================================================
\ Page data fields

datum: source_file

\ XXX TODO -- handling of markups in the data fields depend on the
\ application

datum: language  \ ISO code of the page's language
datum: title  \ page title; can include markups
datum: menu_title  \ short title used menus and navigation bars
datum: breadcrum_title \ short title used in breadcrumb navigation
datum: description  \ page description

\ Dates in ISO format:
datum: created    \ when the contents were created (written)
datum: published  \ when the contents were published (online)
datum: modified   \ when the contents were modified (updated)
' modified alias modifed
datum: file_modified  \ file modification date; if not specified, `modified` is used

datum: access_key  \ access key (one char)

\ Hierarchy data, indicated with a page ID (a page name):
datum: upper_page
datum: previous_page
datum: next_page
datum: first_page
datum: last_page

datum: keywords  \ list of HTML meta keywords, separated by commas
datum: tags  \ list of tag IDs, separated by spaces
datum: properties  \ list of properties, separated by commas
datum: edit_summary  \ description of the latest changes

datum: related  \ list of page IDs, separated by commas

datum: filename_extension  \ alternative target filename extension (with dot)

\ Design and template

\ Note: The actual design and template will be the default ones
\ unless the current page defines its own in its metadata:

datum: design_subdir  \ target relative path to the design, with final slash
datum: template  \ HTML template filename in the design subdir

\ .( /datum = ) /datum ? cr key drop  \ XXX INFORMER

\ ==============================================================
\ File names

0 value current_page  \ page ID of the current page

: target_extension ( a -- ca len )
  filename_extension dup 0=
  if  2drop html_extension $@   then ;
  \ Return the target filename extension of page ID _a_.

: current_target_extension ( -- ca len )
  current_page target_extension ;

: -forth_extension ( ca len -- ca' len' )
  forth_extension $@ -suffix ;
  \ Remove the Forth extension from a filename.

: +forth_extension ( ca len -- ca' len' )
  forth_extension $@ s+ ;
  \ Add the Forth extension to a filename.

: source>current_target_extension ( ca1 len1 -- ca2 len2 )
  -forth_extension current_target_extension s+ ;
  \ Change the Forth extension to the current target extension.
  \ ca1 len1 = Forth source page filename
  \ ca2 len2 = target HTML page filename

: /sourcefilename ( -- ca len )
  sourcefilename basename ;
  \ Return the current source filename, without path.

: pid#>pid$ ( a -- ca len )
  source_file -forth_extension ;
  \ Convert a numerical page ID to its string form.
  \ a = page ID
  \ ca len = page ID

: target_file ( a -- ca len )
\   ." `link_anchor` in `target_file` = " link_anchor $@ type cr  \ XXX INFORMER
\ XXX TODO -- `link_anchor+` should not be here
  dup >r pid#>pid$ r> target_extension s+ link_anchor+
\   ." Result in `target_file` = " 2dup type cr  \ XXX INFORMER
  ;
  \ Return a target HTML page filename.
  \ a = page ID
  \ ca len = target HTML page file name

: current_target_file ( -- ca len )
  current_page target_file ;
  \ Return the target HTML page filename of the current page.
  \ ca len = target HTML page file name

: domain&current_target_file ( -- ca len )
  domain s" /" s+ current_target_file s+ ;

: domain_url ( -- ca len )
  s" http://" domain s+ ;

: current_target_file_url ( -- ca len )
  s" http://" domain&current_target_file s+ ;

: +domain_url ( ca len -- ca' len' )
  domain_url s" /" s+ 2swap s+ ;

: pid#>url ( a -- ca len )
  target_file +domain_url ;

: +target_dir ( ca1 len1 -- ca2 len2 )
  target_dir $@ 2swap s+ ;
  \ Add the target path to a file name.
  \ ca1 len1 = file name
  \ ca2 len2 = file name, with its target local path

: target_path/file ( a -- ca len )
  target_file +target_dir
\  2dup type cr  \ XXX INFORMER
  ;
  \ Return a target HTML page filename, with its local path.
  \ a = page ID
  \ ca len = target HTML page file name, with its local path

\ ==============================================================
\ Page ID

\ The first time a page is interpreted, its data is parsed and
\ created (even if the content doesn't has to be parsered, e.g.
\ when the data has been required by other page).  Then a
\ page ID is created: it's the source filename without path
\ or extension. The execution of the page ID returns the address of
\ the page data.

: current_pid$ ( -- ca len )
  /sourcefilename -extension ;
  \ Return the name of the current page ID.
  \ XXX TODO -- combine with `current_page_pid$`?

: known_pid$? ( ca len -- 0 | xt +-1 )
  -anchor fendo_pid_wid search-wordlist ;

: new_page_data_space ( -- )
  here  dup current_data !  /datum @ dup allot  erase ;
  \ Create and init data space for a new page.

: (:pid) ( ca len -- )
  get-current >r  fendo_pid_wid set-current
  :create new_page_data_space
  r> set-current ;
  \ Create a new page ID and its data space.

: :pid ( -- )
  current_pid$ 2dup known_pid$?
  if  drop 2drop  else  (:pid)  then ;
  \ Create the current page ID and its data space, if needed.

: pid$>pid#? ( ca len -- a true | false )
  known_pid$? if execute true else 0 false then ;

  \ doc{
  \
  \ pid$>pid#? ( ca len -- a true | 0 false )
  \
  \ If string page ID _ca len_ is known, return its equivalent page ID
  \ _a_ and _true_. Otherwise return _0_ and _false_.
  \
  \ See: `pid$>pid#`.
  \
  \ }doc

: current_page_pid$ ( -- ca len )
  current_page pid#>pid$
\  current_page ?dup if  pid#>pid$  else  pad 0  then
  ;
  \ Return the string page ID of the current page,
  \ XXX TODO -- combine with `current_pid$`?
  \ XXX TODO `current_page` can be zero during debugging tasks,
  \ for example while using `echo>screen` to check the
  \ engine without files. But this alternative creates new
  \ problems because of the empty page ID:
  \ ca len = page ID or empty string

: descendant? ( ca1 len1 ca2 len2 -- f )
  s" ." s+ 2swap s" ." s+  \
  { D: descendant D: ancestor }
\  descendant ancestor str= ?dup if  0= exit  then  \ XXX OLD
  descendant ancestor string-prefix? ;
  \ Is the page whose ID is _ca2 len2_ a descendant of the page whose
  \ ID is _ca1 len1_?

: pid$>level ( ca len -- n )
  [char] . char-count ;
  \ Return the hierarchy level of the given page ID.
  \ The top level is 0.

: pid#>level ( a -- n )
  pid#>pid$ pid$>level ;
  \ Return the hierarchy level of the given page ID.
  \ The top level is 0.

: brother_pages? ( ca1 len1 ca2 len2 -- f )
  -common-prefix pid$>level -rot pid$>level or 0= ;
  \ Are the pages whose IDs are _ca1 len1_ and _ca2 len2_ brothers,
  \ i.e. do the last part (level) of their IDs is preceded by a common
  \ ancestor (hierarchy)?

\ ==============================================================
\ Debugging tools

: .data { pageID -- }
  ." data of pid# " pageID . cr
  ."   source_file = " pageID 'source_file $@ type cr
  ."   title = " pageID 'title $@ type cr
\  ."   project_status = " pageID project_status type cr
\  ."   project_start = " pageID project_start type cr
\  ."   project_end = " pageID project_end type cr
\  ."   project_completion = " pageID project_completion type cr
  ."   related = " pageID 'related $@ type cr ;

: .current_data ( -- )
  current_page .data  ;

\ ==============================================================
\ Page data header

defer set_default_data ( -- )
  \ Set the default values of the page data.

: (set_default_data) ( -- )
  /sourcefilename
\  2dup ." «" type ." »" \ XXX INFORMER
  current_data @ 'source_file $! ;
  \ Set the default values of the page data.
  \ XXX TODO finish

' (set_default_data) is set_default_data
: (}data) ( -- )
\  ." }data executed; data before defaults:" cr  \ XXX INFORMER
\  .current_data  \ XXX INFORMER
  set_default_data  in_data_header? off
\  ." data after defaults:" cr  \ XXX INFORMER
\  .current_data  \ XXX INFORMER
\  ." }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}" cr  \ XXX INFORMER
\  key drop  \ XXX INFORMER
  ;

: }data ( -- )
  in_data_header? @ if  (}data)  then
\  cr cr ." =========== }data" cr key drop  \ XXX INFORMER
\  ." `argc` in `}data`= " argc ? cr  \ XXX INFORMER
  ;
  \ Mark the end of the page data header and complete it.

: skip_data{ ( xt "<text><space>}data" -- )
  execute to current_page
\  ." skip_data{" cr  \ XXX INFORMER
  begin   parse-name dup 0=
    if    2drop refill 0= dup abort" Missing `}data`"
    else  s" }data" str=  then
  until   }data ;
  \ Skip the page data.
  \ xt = execution token of the current page ID

: get_data{ ( "<text><space>}data" -- )
\  ." `argc` in `get-data{` (start)= " argc ? cr  \ XXX INFORMER
\  ." {{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{" cr  \ XXX INFORMER
\  ." get_data{" cr  \ XXX INFORMER
  :pid current_data @
\  ." current_data copied to current_page =  " dup . cr  \ XXX INFORMER
  to current_page
\  .current_data  \ XXX INFORMER
  in_data_header? on
\  ." `argc` in `get-data{` (end)= " argc ? cr  \ XXX INFORMER
  ;
  \ Get the page data.

: data_already_got? ( -- 0 | xt +-1 )
  current_pid$ known_pid$? ;
  \ XXX FIXME This check means pids of draft can not be created...
  \ XXX ...but they are useful to do some checkings, e.g. in...
  \ XXX ...Fendo-programandala's related_pages.
  \ XXX Already solved?

: data{ ( "<text><spaces>}data" -- )
\  cr cr ." =========== data{" cr key drop  \ XXX INFORMER
\  cr cr ." ===========" current_target_file type s" data{" cr key drop  \ XXX INFORMER
\  ." `argc` in `data{` (start)= " argc ? cr  \ XXX INFORMER
  data_already_got? if  skip_data{  else  get_data{  then
\  ." `argc` in `data{` (end)= " argc ? cr  \ XXX INFORMER
  ;
  \ Mark the start of the page data.
  \ XXX TODO how to access the page IDs in the markup?...
  \ XXX ...INCLUDE them in the markup wordlist? create a wordlist?

variable do_content?  do_content? on
  \ flag: do the page content? (otherwise, skip it)

: +source_dir ( ca1 len1 -- ca2 len2 )
  source_dir $@ 2swap s+ ;
  \ Complete a source page filename with its path.

: +current_dir ( ca1 len1 -- ca2 len2 )  \ XXX TMP
  s" ./" 2swap s+ ;

: .required_data_error ( ca len -- )
\  order cr  \ XXX INFORMER
  cr ." Error requiring the data of the page <" type ." >" cr ;

: required_data_error ( ca len ior -- )
  >r .required_data_error r> throw ;

: (required_data) ( ca len -- )
\  ." parameter in `(required_data)` = " 2dup type cr key drop  \ XXX INFORMER
  do_content? off
\  .included key drop  \ XXX INFORMER
\  cr ." before catch " .s cr  key drop \ XXX INFORMER
  2dup  ['] required catch  ?dup
\  cr ." after catch " .s cr key drop \ XXX INFORMER
  if    nip nip required_data_error
  else  2drop  then
\  ." end of (required_data) " .s cr  \ XXX INFORMER
  ;
  \ Require a page file _ca len_ in order to get its data.

: required_data ( ca len -- )
\   ." parameter in `required_data` = " 2dup type cr  \ XXX INFORMER
\   ." related = " current_page related type cr  \ XXX INFORMER
  do_content? @ >r  current_page >r
  (required_data)
  r> to current_page  r> do_content? !
\   ." end of `required_data`" cr \ XXX INFORMER
\   ." related = " current_page related type cr  \ XXX INFORMER
\  ." >>>>>>>>" cr  \ XXX INFORMER
\  key drop  \ XXX INFORMER
  ;
  \ Require a page file _ca len_ in order to get its data.

: required_data<pid# ( a -- )
  source_file required_data ;
  \ Require a page file in order to get its data.
  \ a = page ID (address of its data)

: (required_data<pid$) ( ca len -- )
\   ." Stack at the start of `(required_data<pid$)` : " .s cr key drop \ XXX INFORMER
\   ." Parameter in `(required_data<pid$)` = " 2dup type cr key drop  \ XXX INFORMER
\   ." `link_anchor` in `(required_data<pid$)` = " link_anchor $@ type cr  \ XXX INFORMER
  -anchor?! +forth_extension
\   ." Stack before `required_data` in `(required_data<pid$)` : " .s cr key drop \ XXX INFORMER
  required_data
\   ." Stack after `required_data` in `(required_data<pid$)` : " .s cr key drop \ XXX INFORMER
\   ." Stack at the end of `(required_data<pid$)` : " .s cr key drop \ XXX INFORMER
  ;
  \ Require a page file in order to get its data.
  \ ca len = page ID

: required_data<pid$ ( ca len -- )
\  ." Parameter in `required_data<pid$` before `unshortcut` = " 2dup type cr  \ XXX INFORMER
  unshortcut
\  ." Parameter in `required_data<pid$` after `unshortcut` = " 2dup type cr  \ XXX INFORMER
  (required_data<pid$) ;
  \ Require a page file in order to get its data.
  \ ca len = page ID

: required_data<target ( ca len -- )
\  ." required_data<target " 2dup type cr  \ XXX INFORMER
  -extension required_data<pid$ ;
  \ Require a page file in order to get its data.
  \ ca len = target file, without path

: require_data ( "name" -- )
  parse-name? abort" File name expected in `require_data`"
  required_data ;
  \ Require a page file in order to get its data.
  \ "name" = filename

: (pid$>pid#) ( ca len -- a )
\  ." Parameter of `(pid$>pid#)` : " 2dup type cr key drop \ XXX INFORMER
\   ." Stack at the start of `(pid$>pid#)` : " .s cr key drop \ XXX INFORMER
\  -anchor \ XXX TMP
\  ." `link_anchor` in `(pid$>pid#)` = " link_anchor $@ type cr  \ XXX INFORMER
  2dup (required_data<pid$) pid$>pid#? drop
\   ." Stack at the end of `(pid$>pid#` : " .s cr key drop \ XXX INFORMER
  ;

  \ doc{
  \
  \ (pid$>pid#) ( ca len -- a )
  \
  \ Convert page ID _ca len_ into its equivalent _a_.
  \
  \ See: `pid$>pid#`
  \
  \ }doc

: pid$>pid# ( ca len -- a )
\   ." Parameter in `pid$>pid#`  before `dry_unshortcut` = " 2dup type cr  \ XXX INFORMER
\  key drop  \ XXX INFORMER
\  ."    `href=` in `pid$>pid#` before `dry_unshortcut` = " s" href=@" evaluate .s ." = " type cr  \ XXX INFORMER
\   ." `link_anchor` in `pid$>pid#` before `dry_unshortcut` = " link_anchor $@ type cr  \ XXX INFORMER
  dry_unshortcut  \ XXX TMP
\  ." >> `href=` in `pid$>pid#` after `dry_unshortcut`  = " s" href=@" evaluate .s ." = " type cr  \ XXX INFORMER
\   ." Parameter in `pid$>pid#` after `dry_unshortcut` = " 2dup type cr  \ XXX INFORMER
\   ." `link_anchor` in `pid$>pid#` after `dry_unshortcut` = " link_anchor $@ type cr  \ XXX INFORMER
  dup 0= abort" Empty page-id"  \ XXX TMP
  (pid$>pid#)
\  find-name name>int execute  \ XXX SECOND version; no difference, same corruption of the input stream
\  cr ." end of data<pid$>pid"  \ XXX INFORMER
  ;

  \ doc{
  \
  \ pid$>pid# ( ca len -- a )
  \
  \ If string page ID _ca len_ is unknown, get its data from the
  \ corresponding source page. Then return return the equivalent page
  \ ID _a_.
  \
  \ See: `pid$>pid#?`.
  \
  \ }doc

: pid$>(data>)pid# ( ca len -- a )
\   ." Parameter in `pid$>(data>)pid#`  = " 2dup type cr  \ XXX INFORMER
  dup if  pid$>pid#  else  2drop current_page  then ;
  \ Return a number page ID from a string page ID;
  \ if it's different from the current page, require its data.
  \ This word is needed to manage links to the current page
  \ (href attributes that contain just an anchor).

: pid$>url ( ca1 len1 -- ca2 len2 )
  pid$>pid# target_file +domain_url ;

: source>pid$ ( ca1 len1 -- ca2 len2 )
  basename -forth_extension ;
  \ Convert a source page to a page ID.
  \ ca1 len1 = Forth source page filename with path
  \ ca2 len2 = page ID

: source>pid# ( ca len -- a )
  source>pid$ pid$>pid# ;
  \ Convert a source page to a page ID.
  \ ca len = Forth source page filename with path
  \ a = page ID

: pid$>target ( ca1 len1 -- ca2 len2 )
  2dup pid$>pid# target_extension s+ +target_dir ;
  \ Convert a page ID to a target filename.

\ ==============================================================
\ Calculated data

: file_mtime ( a -- ca len )
  dup file_modified dup
  if  rot drop  else  2drop modified  then ;
  \ ISO time string used to set the mtime (modification time) of the
  \ target files. The `file_modified` datum is the first choice,
  \ then `modified`.

: newer? ( a -- f )
  dup target_path/file 2dup file-exists?
  if    file-mtime  rot file_mtime  str<
  else  2drop drop true  then
\  dup if  ." newer"  else  ." older"  then  cr  \ XXX INFORMER
  ;
  \ Is the given page newer than its target?

: description|title ( a -- ca len )
  dup >r description dup if  rdrop  else  2drop r> title  then ;
  \ Description or (if it's empty) title of the given page ID _a_.
  \ This is used as link title when no one has been specified.

: property? ( ca len a -- f )
  { D: property page_id }
  \ XXX TODO change the properties system: make it similar to tags:...
  \ XXX TODO ...make properties executable; they should trigger a flag.
  page_id properties  false { result }
  /ssv 0 ?do
    property str= result or to result
  loop  result ;
  \ ca len = property to check
  \ a = page ID (address of its data)
  \ f = is the property in the properties field of the page?

\ `ignore_draft_property?` is a flag for the application
\ that does what its name suggets:
\ When it's true, the "draft" status will be ignored,
\ so draft pages will be built as definitive pages.
false value ignore_draft_property?

: draft? ( a -- f )
  s" draft" rot property?  ignore_draft_property? 0= and ;
  \ Is page ID _a_ a draft? I.e., is "draft" in its properties field?

: pid$>draft? ( ca len -- f )
\   ." Stack at the start of `pid$>draft?` : " .s cr key drop \ XXX INFORMER
  pid$>pid# draft?
\   ." Stack at the end of `pid$>draft?` : " .s cr key drop \ XXX INFORMER
  ;
  \ Is page ID _ca len_ a draft page?

: pid$>hierarchy ( ca len -- u )
  0 rot rot  \ counter
  bounds ?do  i c@ [char] . = abs +  loop ;
  \ Return the hierarchy level of a page (0 is the top level).
  \ ca len = page ID (source page filename without extension)

: filename>hierarchy ( ca len -- u )
  pid$>hierarchy 1- ;
  \ Return the hierarchy level of a page (0 is the top level).
  \ ca len = filename (without path; with extension)

: pid#>hierarchy ( a -- u )
  pid#>pid$ pid$>hierarchy ;
  \ Return the hierarchy level of a page (0 is the top level).
  \ a = page ID (address of its data)

defer calculated_field? ( ca len -- f )
  \ Is _ca len_ the contents of a calculated field?
  \ A conventional mark is used to make same fields calculated.

: default_calculated_field? ( ca len -- f )
  s" [calculated]" str= ;
  \ Is _ca len_ the contents of a calculated field?
  \ A conventional mark is used to make same fields calculated.

' default_calculated_field? ' calculated_field? defer!

variable this_page \ dynamic string

variable a_previous_page \ dynamic string

: (pid$>previous) ( ca1 len1 -- true | ca2 len2 false )
  {: D: pid$ :}
  pid$ pid$>pid# draft?          ?dup ?exit
  pid$ this_page $@ brother_pages? 0= ?dup ?exit
  pid$ this_page $@ str=
  if        a_previous_page $@ false
  else pid$ a_previous_page $! true
  then ;
  \ Is page ID _ca1 len1_ (local _pid$_) contained in the dynamic string
  \ _this_page_?  If so, return the page ID _ca2 len2_ of its previous brother
  \ page in the hierarchy (page ID which was saved in the previous execution)
  \ and _false_ (to stop the traversing); otherwise return just _true_ (to
  \ continue the traversing).

: pid$>previous ( ca1 len1 -- ca2 len2 )
  a_previous_page off
  this_page $!
  ['] (pid$>previous) traverse_pids ;
  \ Return the previous brother page ID _ca2 len2_ of page ID _ca1 len1_.
  \ If no previous page exists, _ca2 len2_ is an empty string.

: ?previous_page ( a -- ca len )
  dup previous_page 2dup calculated_field?
  if   2drop pid#>pid$ pid$>previous
  else rot drop
  then ;
  \ If field `previous_page` of page ID _a_ is calculated, calculate it
  \ and return the result in string _ca len_; otherwise return the
  \ field contents.

variable a_next_page \ flag

: (pid$>next) ( ca1 len1 -- true | ca1 len1 false )
  {: D: pid$ :}
  pid$ pid$>pid# draft?          ?dup ?exit
  this_page $@ pid$ brother_pages? 0= ?dup ?exit
  this_page $@ pid$ -common-prefix str<
  if a_next_page on pid$ false else true then ;
  \ Is _ca1 len1_ (local _pid$_) the next brother page of the page
  \ whose page ID is contained in the dynamic string _this_page_?
  \ If so, return _ca1 len1_ and _false_ (to stop the traversing);
  \ otherwise return just _true_ (to continue the traversion).

: pid$>next ( ca1 len1 -- ca2 len2 )
  a_next_page off
  this_page $!
  ['] (pid$>next) traverse_pids
  a_next_page @ 0= if 0 0 then ;
  \ Return the next brother page ID _ca2 len2_ of page ID _ca1 len1_.
  \ If no next page exists, _ca2 len2_ is an empty string.

: ?next_page ( a -- ca len )
  dup next_page 2dup calculated_field?
  if   2drop pid#>pid$ pid$>next
  else rot drop
  then ;
  \ If field `next_page` of page ID _a_ is calculated, calculate it
  \ and return the result in string _ca len_; otherwise return the
  \ field contents.

: pid$>upper ( ca1 len1 -- ca2 len2 )
  -extension ;
  \ Return the upper page ID _ca2 len2_ of page ID _ca1 len1_.
  \ If no upper page exists, _ca2 len2_ is an empty string.

: ?upper_page ( a -- ca len )
  dup upper_page 2dup calculated_field?
  if   2drop pid#>pid$ pid$>upper
  else rot drop
  then ;
  \ If field `upper_page` of page ID _a_ is calculated, calculate it
  \ and return the result in string _ca len_; otherwise return the
  \ field contents.

: (pid$>first) ( ca1 len1 -- true | ca2 len2 false )
  {: D: pid$ :}
  pid$ pid$>pid# draft?          ?dup ?exit
  pid$ this_page $@ brother_pages? 0= ?dup ?exit
  pid$ false ;
  \ Is page ID _ca1 len1_ (local _pid$_) contained in the dynamic string
  \ _this_page_?  If so, return the page ID _ca2 len2_ of its first brother
  \ page in the hierarchy (page ID which was saved in the first execution)
  \ and _false_ (to stop the traversing); otherwise return just _true_ (to
  \ continue the traversing).

: pid$>first ( ca1 len1 -- ca2 len2 )
  this_page $!
  ['] (pid$>first) traverse_pids ;
  \ Return the first brother page ID _ca2 len2_ of page ID _ca1 len1_.
  \ If no first page exists, _ca2 len2_ is an empty string.

: ?first_page ( a -- ca len )
  dup first_page 2dup calculated_field?
  if   2drop pid#>pid$ pid$>first
  else rot drop
  then ;
  \ If field `first_page` of page ID _a_ is calculated, calculate it
  \ and return the result in string _ca len_; otherwise return the
  \ field contents.

\ ==============================================================
\ Data manipulation

: (file-mtime>modified) ( ca len -- )
  file-mtime 2dup current_page modified
  str< if  2drop  else  current_page 'modified $!  then ;
  \ If the modification time of the given file is more recent
  \ than the current page `modified` datum, update the page datum with
  \ the file modification time.

true value included_files_update_the_page_date?
  \ Config flag for the application.

: file-mtime>modified ( ca len -- )
  included_files_update_the_page_date?
  if (file-mtime>modified) else 2drop then ;
  \ If the modification time of the given file is more recent
  \ than the current page `modified` datum, update the page datum with
  \ the file modification time.
  \ This is used by addons that include contents file into the page,
  \ in order to update the page `modified` datum with the date of
  \ the most recent file used.

.( fendo.data.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-04-28: Start.
\
\ 2013-05-01: Fixed and finished the data system.
\
\ 2013-05-17: Fix: There were two words with the name `>datum`; it
\ caused no problem in practice, but was confusing.
\
\ 2013-05-17: Improvement: `data{` gets the data only the first time.
\
\ 2013-05-17: New: `require_data` is moved here from its own file, and
\ simplified.
\
\ 2013-05-18: Change: data fields return their offset, not their
\ content (neccessary to write `datum!`; `>datum` removed (now `+` can
\ be used instead). `datum!` is necessary in order to set default
\ values to certain fields.
\
\ 2013-05-18: New: `parse_datum` is rewriten and factored out to
\ `datum!`.
\
\ 2013-06-07: Fix: The check in `data{` was obsolete; it has been
\ rewritten.
\
\ 2013-06-08: Fix: The leading spaces of parsed data were not removed.
\
\ 2013-06-08: Fix: now `datum@` returns an empty string if the datum
\ was not set.
\
\ 2013-06-08: Fix: `@` missing in `default_data`; beside, renamed to
\ "set_default_data'.
\
\ 2013-06-08: Change: `datum@` and `datum!` are removed; `$@` and `$!`
\ are used instead (from Gforth's <string.fs>)).
\
\ 2013-06-08: Fix: name clash (old `source_filename` > `+source_path`;
\ new `source_filename` > `/sourcefilename`).
\
\ 2013-06-23: Change: design and template fields are renamed after the
\ changes in the config module.
\
\ 2013-06-28: Change: hierarchy metadata fields are renamed with the
\ "_page" prefix, to avoid the clash with `next` and make the code
\ clearer; `up` is renamed to `upper_page`.
\
\ 2013-06-28: Change: metadata fields return their values, not their
\ addresses; a parallel word is created to return the address, only
\ needed to set the default data; this change makes the code nicer.
\
\ 2013-06-29: Change: `/sourcefilename` moved here from
\ <fendo_files.fs>.
\
\ 2013-06-29: Change: `source>target_extension` moved here from
\ <fendo_files.fs>.
\
\ 2013-06-29: New: `target_extension`; now target filename extension
\ depends on the corresponding optional metadatum too.
\
\ 2013-07-28: New: `required_data`, `-forth_extension`,
\ `+forth_extension`.
\
\ 2013-09-06: Fix: `(required_data)` didn't save `current_page`.
\
\ 2013-09-06: New: `property?`, `draft?`.
\
\ 2013-09-29: Fix: `current_page` was not properly preserved when
\ `require_data` was used. This caused many pages were not created.
\ This bug was difficult to find out.
\
\ 2013-10-22: Change: the new word `trim`, defined in the Galope
\ library, is used instead of '-trailing -leading'.
\
\ 2013-10-22: Fix: `parse_datum` now uses `trim` instead of
\ `-leading`.
\
\ 2013-10-22: New: `data<id$>id`.
\
\ 2013-10-23: Improvement: `unshortcut` is used in `required_data<id$`
\ and `data<id$>id`.
\
\ 2013-11-06: New: `(data<)id$>id`.
\
\ 2013-11-07: New: `(required_data)` traps the errors and show an
\ additional error message that includes the filename. This is
\ important because the actual filename could be different from the
\ filename taken from the markup, because of the "unshortcuttting"
\ system.
\
\ 2013-11-11: Change: `/csv` moved to the Galope library.
\
\ 2013-11-24: New: `page_id$`, `descendant?`, `current_page_id$`.
\
\ 2013-11-25: New: `source>id$`, `source>id`.
\
\ 2013-11-26: Change: several words renamed, after a new uniform
\ notation: "pid$" and "pid#" for both types of page IDs.
\
\ 2013-11-26: New: `pid$>pid#`; page IDs are created in a specific
\ wordlist.
\
\ 2013-11-27: New: `(pid$>data>pid#)` factored out from
\ `pid$>data>pid#`, as required by `pid$_list@` (defined in
\ <addons/pid_list.fs>), where `unshortcut` is inconvenient.
\
\ 2013-11-28: New: `known_pid$?`, factored from `pid$>pid#` to be
\ reused in `:pid`.
\
\ 2013-11-28: New: `pid$>target`, to fix `redirected` (in
\ <fendo_files.fs>)
\
\ 2013-12-05: Change: `current_file_pid$` renamed to `current_pid$`.
\
\ 2013-12-06: New: `pid$>level` and `pid#>level`.
\
\ 2014-01-05: Typo: `modifed` corrected to `modified`; alias created
\ for the remaining mentions in the old pages.
\
\ 2014-01-06: New: `replaced`, used by the wiki markup module and the
\ common source code addon.
\
\ 2014-02-15: Fix: `forth-wordlist` is set to current before requiring
\ the library files. The problem was <ffl/config.fs> created
\ `ffl.version` in the `fendo` vocabulary, but searched for it in
\ `forth-wordlist`. This somehow arised after renaming all Fendo files
\ for version A-03. The same problem happened in <fendo.fs> on
\ 2013-10-30.
\
\ 2014-02-23: Fix: `(:pid)` now erases the data space.
\
\ 2014-02-23: Change: `(:pid)` is factored to `new_page_data_space`.
\
\ 2014-02-25: Change: `required_data<pid` renamed to
\ `required_data<pid#`.
\
\ 2014-02-25: Still debugging: some data is copied to a different
\ page.
\
\ 2014-02-28: `replaced` is moved to the Galope library.
\
\ 2014-02-28: Fix: stack notations and parameters in `pid:` and
\ `(pid:)`.
\
\ 2014-02-28: Fix: `data_already_got?` checked `fendo_wid`, not
\ `fendo_pid_wid`! That was the reason of the data corruption between
\ pages. Besides, `data_already_got?` can be factored with
\ `known_pid$?`.
\
\ 2014-03-02: New: `domain&current_target_file`, factored from
\ `(redirected)` (defined in <fendo.files.fs>).
\
\ 2014-03-03: New: `filename>hierarchy`.  Change: `(hierarchy)`
\ renamed to `pid$>hierarchy`; `hierarchy` updated.
\
\ 2014-03-03: Fix: `abort"` added to `pid$>data>pid#`, in order to
\ detect temporary shortcuts of missing pages that return an empty
\ string (these temporary shortcuts make links possible, but made the
\ program crash when trying to load the page metadata).
\
\ 2014-07-11: New: `pid#>url`, needed by the Atom module.  New:
\ `domain_url` factored out; needed by the Atom module.  New:
\ `+domain_url` factored out.  New: `pid$>url`, moved here from
\ <fendo.links.fs>.
\
\ 2014-07-14: Change: `domain` is updated; it's not a dynamic variable
\ anymore, after the changes in <fendo.config.fs>.  New:
\ `current_target_file_url`, used by the Atom module.  Fix: the
\ separator slash was missing in `+domain_url`.  Change:
\ `source>target_extension` renamed to
\ `source>current_target_extension`.  Fix: `target_file` added the
\ current target extension, not the target extension for the given
\ page ID.
\
\ 2014-11-08: Change: the `plain_title` and `plain_description` data
\ fields are removed because `unmarkup` (just implemented) makes them
\ unnecessary.
\
\ 2014-11-11: Change: `hierarchy` renamed to `pid#>hierarchy`; the old
\ alias is removed.
\
\ 2014-11-11: Change: `-anchor` was defered and added to
\ `(pid$>data>pid#)`.
\
\ 2014-11-11: Fix: `link_anchor+` was missing in `target_file`.
\
\ 2014-11-13: Fix: `pid$>level`.
\
\ 2014-11-14: Change: `-anchor!` instead of `-anchor` in
\ `(pid$>data>pid#)`; `-anchor!` called also in `known_pid$?` and
\ `(required_data<pid$)`. The new approach is to keep the anchors, and
\ remove them only to when the actual files are required. Shortcuts
\ need no modification; `pid$>source` was not used, it is removed.
\
\ 2014-11-16: Fix: now `link_anchor+` is called by `target_file`, not
\ at upper levels of the code, what caused problems.  Beside,
\ `(required_data<pid$)` now calls `-anchor?!` instead of `-anchor!`,
\ what ruined the anchor in some cases.
\
\ 2014-11-17: Change: now `pid$>data>pid#` calls `dry_unshortcut`
\ instead of `just_unshortcut`. This fixes the problem href was echoed
\ in "<dt>", in `tagged_pages_by_prefix`.
\
\ 2014-12-05: Change: `/ssv` is moved to the Galope library; an
\ useless `trim` is removed from `property?`.
\
\ 2014-12-06: New: calcutated datum `description|title`.
\
\ 2015-01-14: New: `file_modified` datum and `file_mtime` calculated
\ datum.
\
\ 2015-01-30: New: `ignore_draft_property?`.
\
\ 2015-02-11: Improvement: more detailed comments in `:datum>address`
\ and `:datum>value`. Change: `:datum>value` renamed to `:datum`. New:
\ `file-mtime>modified` and related words.
\
\ 2015-02-17: New: `get_datum` and `(get_datum)`, factored out from
\ `:datum` in order to let the application hack the default datum
\ fields.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-06-25: Add `pid$>draft?`, factored from
\ `proper_hierarchical_link?`.
\
\ 2017-11-04: Update to Galope 0.103.0: Replace `-path` with Gforth's
\ `basename`.
\
\ 2018-09-12: Fix the debugging code of `required_data`, which
\ corrupted the stack!  Besides, there is a long-time strange problem
\ in `(required_data)` when running of Gforth 0.7.9: after `[']
\ require catch`, the stack contains three -13 ior codes. No problem
\ on Gforth 0.7.3.  Maybe it has to do (but not only) with the paths
\ of required files.  The problem was temporarily dodged in the
\ Fendo-programandala app by ignoring <fendo-programandala.fs> when
\ `fendo-programandala_version` has been defined already, in order to
\ prevent the file from been included more than once by `require`.
\
\ 2018-09-27: Add `brother_pages?`, `pid$>previous`, `?previous_page`,
\ `pid$>next`, `?next_page`.
\
\ 2018-09-28: Fix `pid$>next` with the `a_next_page` flag. Fix
\ `?previous_page` and `?next_page`. Add `?upper_page`. Add
\ `calculated_field?` and `default_calculated_field?`. Add
\ `?first_page`. Improve documentation.
\
\ 2018-12-06: Add some debugging code.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-17: Rename `pid$>pid#` to `pid$>pid#` and make it return
\ also a true flag for consistency. Adapt the code to the new
\ behaviour. Then rename `pid$>data>pid#` to `pid$>pid#` and document
\ both words.
\
\ 2018-12-20: Fix typo in comment. Add metadata `published`,
\ `menu_title` and `breadcrumb_title`. Update stack notation of page
\ IDs.

\ vim: filetype=gforth
