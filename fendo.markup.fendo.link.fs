.( fendo.markup.fendo.link.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for links.

\ Copyright (C) 2013,2014,2015 Marcos Cruz (programandala.net)

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
  \ Defined in <fendo.parser.fs>.
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
  \ ca len = page id, URL or shortcut
  unshortcut 2dup set_link_type
  local_link? if  -anchor!  then  \ XXX OLD
  2dup last_href$ $! href=!
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

\ **************************************************************
\ Markup

markup_definitions

: [[  ( "linkmarkup ]]" -- )
  parse_link echo_link
  ;
: ]]  ( -- )
  true abort" ']]' without '[['"
  ;

fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2014-07-14: Change: 'domain' is updated; it's not a dynamic variable
\ anymore, after the changes in <fendo.config.fs>.
\
\ 2014-08-13: Fix: 'separate?' is saved and restored in 'parse_link',
\ because 'parse_link_text' changed it to true, what ruined previous
\ opening punctuation. For example, in the source "bla bla bla ( [[
\ url | link text ]] )" the bug caused the opening paren to remain
\ separated from the link.
\
\ 2014-11-08: Change: 'unmarkup' (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.
\
\ 2014-11-08: Removed some code that some time ago was moved to
\ <fendo.links.fs> and commented out.
\
\ 2015-01-17: Fix: typo in stack comment.

.( fendo.markup.fendo.link.fs compiled ) cr

