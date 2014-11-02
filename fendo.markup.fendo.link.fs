.( fendo.markup.fendo.link.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for links.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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
\ Requirements

forth_definitions

require galope/dollar-variable.fs  \ '$variable'

\ **************************************************************
\ Tools

fendo_definitions

false [if]  \ xxx tmp
: file://?  ( ca len -- wf )
  \ Does a string start with "file://"?
  s" file://" string-prefix?
  ;
' file://? alias file_href?
: http://?  ( ca len -- wf )
  \ Does a string start with "http://"?
  s" http://" string-prefix?
  ;
: https://?  ( ca len -- wf )
  \ Does a string start with "https://"?
  s" https://" string-prefix?
  ;
: ftp://?  ( ca len -- wf )
  \ Does a string start with "ftp://"?
  s" ftp://" string-prefix?
  ;
: external_href?  ( ca len -- wf )
  \ Is a href attribute external?
  2dup http://? >r  2dup https://? >r  ftp://? r> or r> or
  ;
link_text_as_attribute? 0= [if]  \ xxx tmp
$variable link_text
: link_text!  ( ca len -- )
  link_text $!
  ;
: link_text@  ( -- ca len )
  link_text $@
  ;
: link_text?!  ( ca len -- )
  \ If the the string variable 'link_text' is empty,
  \ store the given string into it.
  link_text@ empty? if  link_text!  else  2drop  then
  ;
[then]
: evaluate_link_text  ( -- )
  link_text@ evaluate_content
  ;
[then]

false [if]  \ xxx tmp
$variable link_anchor
: -anchor  ( ca len -- ca len | ca' len' )
  \ Extract the anchor from a href attribute and store it.
  \ ca len = href attribute
  \ ca' len' = href attribute without anchor
  s" #" sides/ drop link_anchor $!
  ;
: +anchor  { ca1 len1 ca2 len2 -- ca3 len3 }
  \ Add a link anchor to a href attribute.
  \ ca1 len1 = href attribute
  \ ca2 len2 = anchor, without "#"
  ca1 len1 len2 if  s" #" s+ ca2 len2 s+  then
  ;
variable link_type
1 enum local_link
  enum external_link
  enum file_link  drop
: >link_type_id  ( ca len -- n )
  \ Convert an href attribute to its type id.
  2dup external_href? if  2drop external_link exit  then
  file_href? if  file_link exit  then
  local_link
  ;
: set_link_type  ( ca len -- )
  \ Get and store the type id of an href attribute.
  \ xxx todo no href means local, if there is/was an anchor label
  >link_type_id link_type !
  ;
: external_link?  ( -- wf )
  link_type @ external_link =
  ;
: local_link?  ( -- wf )
  link_type @ local_link =
  ;
: file_link?  ( -- wf )
  link_type @ file_link =
  ;
[then]

variable link_finished?  \ flag, no more link markup to parse?
: end_of_link?  ( ca len -- wf )
  \ ca len = latest name parsed
  s" ]]" str=  dup link_finished? !
\  cr ." end_of_link? >> " dup .  \ XXX INFORMER
  ;
: end_of_link_section?  ( ca len -- wf )
  \ ca len = latest name parsed
  2dup end_of_link? or_end_of_section?
  ;
: more_link?  ( -- wf )
  \ Fill the input buffer or abort.
  refill 0= dup abort" Missing ']]'"
  ;
defer parse_link_text  ( "...<spaces>|<spaces>" | "...<spaces>]]<spaces>"  -- )
  \ Parse the link text and store it into 'link_text'.
  \ Defined in <fendo_parser.fs>.
: get_link_raw_attributes  ( "...<space>]]<space>"  -- )
  \ Parse and store the link raw attributes.
  s" "
  begin   parse-name dup
    if    2dup end_of_link?  otherwise_concatenate
    else  2drop  more_link?  then
  until   ( ca len ) unraw_attributes
  ;
$variable last_href$  \ xxx new, experimental, to be used by the application
:noname  ( ca len -- )
\  ." (get_link_href) 0 " 2dup type cr  \ xxx informer
  unshortcut
\  ." (get_link_href) 1 " 2dup type cr  \ xxx informer
  2dup set_link_type
  local_link? if  -anchor  then  2dup last_href$ $! href=!
  ;  is (get_link_href)  \ defered in <fendo.links.fs>
: get_link_href  ( "href<spaces>" -- )
  \ Parse and store the link href attribute.
  parse-name (get_link_href)
\  ." ---> " href=@ type cr  \ xxx informer
\  external_link? if  ." EXTERNAL LINK: " href=@ type cr  then  \ xxx informer
  [ true ] [if]  \ simple version
    parse-name end_of_link_section? 0=
    abort" Space not allowed in link href"
  [else]  \ no abort  \ xxx tmp, this causes the parsing never ends
    begin  parse-name end_of_link_section? 0=
    while  s" <!-- xxx fixme space in link filename or URL -->" echo
    repeat
  [then]
  ;
: parse_link  ( "linkmarkup ]]" -- )
  \ Parse and store the link attributes.
\  ." entering parse_link -- order = " order cr \ xxx informer
\  cr ." separate? in parse_link (0) is " separate? ?  \ XXX INFORMER 2014-08-13
  get_link_href
\  cr ." separate? in parse_link (1) is " separate? ?  \ XXX INFORMER 2014-08-13
\  ." ---> " href=@ type cr  \ xxx informer
  link_finished? @ 0= if
\    ." link not finished; href= " href=@ type cr  \ xxx informer
\  cr ." separate? in parse_link (0) is " separate? ?  \ XXX INFORMER 2014-08-13
    separate? @  parse_link_text  separate? !
    link_finished? @ 0=
\  cr ." separate? in parse_link (1) is " separate? ?  \ XXX INFORMER 2014-08-13
    if
\      ." link not finished; link text= " link_text $@ type cr  \ xxx informer
      get_link_raw_attributes
      then
\  cr ." separate? in parse_link (2) is " separate? ?  \ XXX INFORMER 2014-08-13
  then
\  ." ---> " href=@ type cr  \ xxx informer
  ;
false [if]  \ xxx tmp
: missing_local_link_text  ( -- ca len )
\  ." missing_local_link_text" cr  \ xxx informer
  href=@ -extension 2dup required_data<pid$
  >sb  \ xxx tmp
  evaluate title
  echo> @ >r echo>string
  >attributes< -attributes  \ use the alternative set and init it
  evaluate_content
  r> echo> ! >attributes< echoed $@
  ;
: missing_external_link_text  ( -- ca len )
  href=@
  ;
: missing_file_link_text  ( -- ca len )
  href=@ -path
  ;
: missing_link_text  ( -- ca len )
  \ Set a proper link text if it's missing.
  \ xxx todo
  local_link?  if  missing_local_link_text exit  then
  external_link?  if  missing_external_link_text exit  then  \  xxx
  file_link?  if  missing_file_link_text exit  then  \ xxx
  true abort" Wrong link type"  \ xxx tmp
  ;
: external_class  ( -- )
  \ Add "external" to the class attribute.
  class=@ s" external" bs& class=!
  ;
: link_anchor+  ( ca len -- )
  \ Restore the link anchor of the local href attribute, if any.
  link_anchor $@ +anchor
  ;
: convert_local_link_href  ( ca1 len1 -- ca2 len2 )
  \ Convert a raw local href to a finished href.
  dup if  pid$>data>pid# target_file  then  link_anchor+
  ;
: pid$>url  ( ca1 len1 -- ca2 len2 )
  \ xxx not used?
  s" http://" domain s+ 2swap
  pid$>data>pid# target_file s+
  ;
: -file://  ( ca len -- ca' len' )
  s" file://" -prefix
  ;
: convert_file_link_href  ( ca len -- ca' len' )
  -file://  files_subdir $@ 2swap s+
  ;
: convert_link_href  ( ca len -- ca' len' )
  \ ca len = href attribute, without anchor
  link_type @ case
    local_link      of  convert_local_link_href     endof
    file_link       of  convert_file_link_href      endof
  endcase
  ;
variable local_link_to_draft_page?
: (tune_local_hreflang)  ( a -- )
  \ Set the hreflang attribute of a local link, if needed.
  \ a = page id of the link destination
  s" pid#>lang$ 2dup current_lang$" evaluate str=
  if  2drop  else  hreflang=?!  then
  ;
: tune_local_hreflang  ( a -- )
  \ Set the hreflang attribute of a local link, if needed.
  \ a = page id of the link destination
  multilingual? if  (tune_local_hreflang)  else  drop  then
  ;
: tune_local_link  ( -- )
  \ xxx todo fetch alternative language title and description
\  ." tune_local_link" cr  \ xxx informer
  href=@ pid$>(data>)pid#  >r
\  link_text@ ." link_text in tune_local_link (0) = " type cr  \ xxx informer
\  r@ title ." title in tune_local_link (1) = " type cr  \ xxx informer
  r@ draft? local_link_to_draft_page? !
  r@ plain_description title=?!
\  link_text@ ." link_text in tune_local_link (1) = " type cr  \ xxx informer
  r@ title
\  ." title in tune_local_link (2) = " 2dup type cr  \ xxx informer
  link_text?!  \ xxx bug: this call corrupts 'link_text'
\  link_text@ ." link_text in tune_local_link (2) = " type cr  \ xxx informer
  r@ tune_local_hreflang
  r> access_key accesskey=?!
\  ." end of tune_local_link" cr  \ xxx informer
  ;
: tune_link  ( -- )  \ xxx todo
  \ Tune the attributes parsed from the link.
  local_link? if  tune_local_link  then
  href=@ convert_link_href href=!
  link_text@ empty? if  missing_link_text link_text!  then
  external_link? if  external_class  then
  ;
: echo_link_text  ( -- )
  \ Echo just the link text.
  echo_space evaluate_link_text
  ;
\ Two hooks for the application,
\ e.g. to add the size of a linked file:
defer link_text_suffix
defer link_suffix
' noop  dup is link_text_suffix  is link_suffix
: (echo_link)  ( -- )
  \ Echo the final link.
\  cr ." separate? in (echo_link) is " separate? ?  \ XXX INFORMER 2014-08-13
  [<a>] evaluate_link_text link_text_suffix [</a>] link_suffix
  ;
: echo_link?  ( -- wf )
  \ Can the current link be echoed?
  href=@ nip  local_link_to_draft_page? @ 0=  and
  ;
: reset_link  ( -- )
  \ Reset the link attributes that are not actual HTML attributes,
  \ and are not reseted by the HTML tags layer.
  s" " link_text!  local_link_to_draft_page? off
  ;
: echo_link  ( -- )
  \ Echo a link, if possible.
  \ All link attributes have been set.
\  cr ." separate? in echo_link is " separate? ?  \ XXX INFORMER 2014-08-13
  tune_link  echo_link?
  if  (echo_link)  else  echo_link_text  then  reset_link
  ;
[then]


\ **************************************************************
\ Markup

markup_definitions

: [[  ( "linkmarkup]]" -- )
  parse_link echo_link
  ;
: ]]  ( -- )
  true abort" ']]' without '[['"
  ;

fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\ 2014-07-14: Change: 'domain' is updated; it's not a dinamyc variable
\   anymore, after the changes in <fendo.config.fs>.
\ 2014-08-13: Fix: 'separate?' is saved and restored in 'parse_link',
\ because 'parse_link_text' changed it to true, what ruined previous
\ opening punctuation. For example, in this source
\           bla bla bla ( [[ url | link text ]] )
\ the bug caused the opening paren to remain separated from the link.

.( fendo.markup.fendo.link.fs compiled ) cr

