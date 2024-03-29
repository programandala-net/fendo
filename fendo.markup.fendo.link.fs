.( fendo.markup.fendo.link.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for links.

\ Last modified  20220905T1209+0200.
\ See change log at the end of the file.

\ Copyright (C) 2013, 2014, 2015, 2017, 2018, 2020, 2021, 2022 Marcos
\ Cruz (programandala.net)

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
\ Requirements {{{1

forth_definitions

require fendo.markup.fendo.forth.fs \ `bl+`

require galope/default-of.fs       \ `default-of`
require galope/dollar-variable.fs  \ `$variable`
require galope/trim.fs             \ `trim`

\ ==============================================================
\ Complex links markup code {{{1

fendo_definitions

variable link_finished?  \ flag, no more link markup to parse?

: end_of_link? ( ca len -- f )
  s" ]]" str=  dup link_finished? !
\  cr ." end_of_link? >> " dup .  \ XXX INFORMER
  ;
  \ ca len = latest name parsed

: end_of_link_section? ( ca len -- f )
  2dup end_of_link? or_end_of_section? ;
  \ ca len = latest name parsed

: more_link? ( -- f )
  refill 0= dup abort" Missing `]]`" ;
  \ Fill the input buffer or abort.

defer parse_link_text ( "...<spaces>|<spaces>" | "...<spaces>]]<spaces>"  -- )
  \ Parse the link text and store it into `link_text`.
  \ Defined in <fendo.parser.fs>.

: get_link_raw_attributes ( "...<space>]]<space>"  -- )
  \ Parse and store the link raw attributes.
  s" "
  begin   parse-name dup
    if    2dup end_of_link?  otherwise_concatenate
    else  2drop  more_link?  then
  until   ( ca len ) unraw_attributes ;

$variable last_href$  \ XXX new, experimental, to be used by the application

:noname ( ca len -- )
  trim unshortcut 2dup set_link_type
  local_link? if  -anchor!  then  \ XXX OLD
  2dup last_href$ $! href=!
  ; is (get_link_href)  \ defered in <fendo.links.fs>

: get_link_href ( "href<spaces>" -- )
  parse-name (get_link_href)
\  ." ---> " href=@ type cr  \ XXX INFORMER
\  external_link? if  ." EXTERNAL LINK: " href=@ type cr  then  \ XXX INFORMER
  parse-name end_of_link_section? 0=
  abort" Space not allowed in link href" ;
  \ Parse and store the link href attribute.
  \ ca len = page ID, URL or shortcut

1 [if]

  \ XXX FIXME 2020-10-09: The parsing fails when the link markup spans
  \ on two or more lines.

: parse_link ( "linkmarkup ]]" -- )
\  ." entering parse_link -- order = " order cr \ XXX INFORMER
\  cr ." separate? in parse_link (0) is " separate? ?  \ XXX INFORMER 2014-08-13
  get_link_href
\  cr ." separate? in parse_link (1) is " separate? ?  \ XXX INFORMER 2014-08-13
\  ." ---> " href=@ type cr  \ XXX INFORMER
  link_finished? @ 0= if
\    ." link not finished; href= " href=@ type cr  \ XXX INFORMER
\  cr ." separate? in parse_link (0) is " separate? ?  \ XXX INFORMER 2014-08-13
    separate? @  parse_link_text  separate? !
    link_finished? @ 0=
\  cr ." separate? in parse_link (1) is " separate? ?  \ XXX INFORMER 2014-08-13
    if
\      ." link not finished; link text= " link_text $@ type cr  \ XXX INFORMER
      get_link_raw_attributes
      then
\  cr ." separate? in parse_link (2) is " separate? ?  \ XXX INFORMER 2014-08-13
  then
\  ." ---> " href=@ type cr  \ XXX INFORMER
  ;
  \ Parse and store the link attributes.

[else]

  \ XXX NEW
  \ XXX TODO
  \ XXX UNDER DEVELOPMENT

: parse_link ( "linkmarkup ]]" -- )
  cr ." parse_link #1 " .s \ XXX INFORMER
  get_link_href
  cr ." parse_link #2 " .s \ XXX INFORMER
  link_finished? @ 0= if
    cr ." link not finished " \ XXX INFORMER
    parse_link_text
    cr ." link text parsed " \ XXX INFORMER
    link_finished? @ 0= if get_link_raw_attributes then
  then
  cr ." parse_link #end " .s \ XXX INFORMER
  ;
  \ Parse and store the image attributes.

[then]

: (complex_[[) ( "linkmarkup ]]" -- )
  parse_link echo_link ;

  \ XXX TODO Indicate what happens when _ca2 len2_ is missing:

  \ doc{
  \
  \ (complex_[[) ( "ccc ]]" -- )
  \
  \ Start a link markup (complex version). ``(complex_[[)`` is a
  \ possible action of the actual markup `[[`, selected by
  \ `complex_[[`.
  \
  \ Usage examples:

  \ ----
  \ [[ http://programandala.net ]]
  \ [[ http://programandala.net/en.program.fendo.html | Fendo ]]
  \ [[ http://programandala.net/en.program.fendo.html | Fendo | title="Fendo home page" ]]
  \ ----

  \ The first part of the link can be a page identifier, an actual URL
  \ or a shortcut.

  \ WARNING: The link definition must be on one single line of text.
  \ This limitation may be removed from a future version of Fendo.
  \
  \ See also: `(complex_]])`, `(simple_[[)`, `link`, `shortcut:`.
  \
  \ }doc

: (complex_]]) ( -- )
  true abort" `]]` without `[[`" ;

  \ doc{
  \
  \ (complex_]]) ( -- )
  \
  \ End a link markup (complex version). ``(complex_]])`` is a
  \ possible action of the actual markup `]]`, selected by
  \ `complex_[[`.
  \
  \ See also: `(simple_]])`.
  \
  \ }doc

\ ==============================================================
\ Simple links markup code {{{1

: parse_link ( "ccc" -- ca len )
  s" "
  begin  parse-name dup
    if   2dup s" ]]" str= >r s+ bl+ r>
    else 2drop bl+ refill 0= then
  until ;
  \ Get the content of a `[[` link. Parse the input stream "ccc" until
  \ a "]]" markup is found and return the parsed text (including "]]")
  \ in string _ca len_.

variable [[-depth
  \ Store the stack depth at the start of the link markup, in order to
  \ calculate later the number of arguments left.

: (simple_[[) ( -- )
  depth [[-depth ! parse_link evaluate_markup ;

  \ doc{
  \
  \ (simple_[[) ( -- )
  \
  \ Start a link markup (simple version). ``(simple_[[)`` is a
  \ possible action of the actual markup `[[`, selected by
  \ `simple_[[`.
  \
  \ Usage examples:

  \ ----
  \ simple_[[ \ select the simple link markup
  \ [[ "http://programandala.net" ]]
  \ [[ "http://programandala.net/en.program.fendo.html"
  \    "Fendo homepage" ]]
  \ [[ "http://programandala.net/en.program.fendo.html" "Fendo"
  \    title=" Fendo home page" ]]
  \ [[ s" This is the title" title=!
  \    "http://programandala.net" ]]
  \ complex_[[ \ return to the default format
  \ ----

  \ The text in the markup is evaluated as Forth code. The first
  \ string must be a page identifier, an actual URL or a shortcut. The
  \ second, optional, string must be the link text. HTML parameters
  \ can be set by the corresponding parsing words like ``title="``,
  \ ``style="``, etc., in any order or position. Also their storage
  \ variants like ``title=!`` are valid.
  \
  \ WARNING: The text "]]", delimited by spaces or end of lines,
  \ cannot be part of the link text or any attribute. Otherwise it
  \ would be mistaken for the ending `]]`.
  \
  \ See also: `(simple_]])`, `(complex_[[)`, `link`, `shortcut:`.
  \
  \ }doc

: (simple_]]) ( ca1 len1 | ca1 len1 ca2 len2 -- )
  depth [[-depth @ - case
    2       of s" " link                                             endof
    4       of      link                                             endof
    default-of true abort" Wrong number of arguments in link markup" endof
  endcase ;

  \
  \ doc{
  \
  \ (simple_]]) ( ca1 len1 | ca1 len1 ca2 len2 -- )
  \
  \ End a link markup (simple version) by calling `link` with the
  \ given paramenters. ``(simple_]])`` is a possible action of the
  \ actual markup `]]`, selected by `simple_[[`.
  \
  \ The string _ca1 len1_ is the address (an actual URL, a page
  \ identifier or a shortcut). The optional string _ca2 len2_ is the
  \ link text (if _ca2 len2_ is missing, an empty string is used
  \ instead and passed to `link`). HTML attributes are set by the
  \ corresponding Fendo words. See `(simple_[[)` for usage examples.
  \
  \ See also: `(complex_]])`.
  \
  \ }doc

\ ==============================================================
\ Markup {{{1

markup_definitions

defer [[ ( "ccc" -- )

defer ]] ( -- ) \ complex version
         ( ca1 len1 ca2 len2 | ca1 len1 | -- ) \ simple version

\ ==============================================================
\ Markup selectors {{{1

fendo_definitions

: complex_[[ ( -- )
  [markup>order]
  ['] (complex_[[) is [[
  ['] (complex_]]) is ]]
  [markup<order] ;

  \ doc{
  \
  \ complex_[[ ( -- )
  \
  \ Select the old complex version of the link markups `[[` and `]]`,
  \ provided by `(complex_[[)` and `(complex_]])`.
  \
  \ See also: `simple_[[`, `link`.
  \
  \ }doc

: simple_[[ ( -- )
  [markup>order]
  ['] (simple_[[) is [[
  ['] (simple_]]) is ]]
  [markup<order] ;

  \ doc{
  \
  \ simple_[[ ( -- )
  \
  \ Select the new simple version of the link markups `[[` and `]]`,
  \ provided by `(simple_[[)` and `(simple_]])`.
  \
  \ See also: `complex_[[`, `link`.
  \
  \ }doc

complex_[[ \ set the default

.( fendo.markup.fendo.link.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2014-07-14: Change: `domain` is updated; it's not a dynamic variable
\ anymore, after the changes in <fendo.config.fs>.
\
\ 2014-08-13: Fix: `separate?` is saved and restored in `parse_link`,
\ because `parse_link_text` changed it to true, what ruined previous
\ opening punctuation. For example, in the source "bla bla bla ( [[
\ url | link text ]] )" the bug caused the opening paren to remain
\ separated from the link.
\
\ 2014-11-08: Change: `unmarkup` (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.
\
\ 2014-11-08: Removed some code that some time ago was moved to
\ <fendo.links.fs> and commented out.
\
\ 2015-01-17: Fix: typo in stack comment.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-13: Document `[[` and `]]`.
\
\ 2020-10-07: Improve documentation of `[[`.
\
\ 2020-10-09: Draft an improved definition of `parse_name`.
\
\ 2020-11-14: Add `trim` to tidy the argument of the action of
\ `(get_link_href)`. This makes it possible to use the alternative raw
\ syntax for links (`<[ s" HREF" s" TEXT" title=" TITLE" link ]>`), no
\ matter "HREF" is on a new line. Improve documentation of `[[`.
\
\ 2020-11-16: Make `[[` defered, write an alternative simpler version
\ of it and two words to select the old version or the new one. The
\ old version is still the default. Document the markups, their
\ actions and the selectors.
\
\ 2021-05-05: Fix two typos in documentation of `(simple_[[)`.
\
\ 2021-10-23: Improve documentation.
\
\ 2022-09-05: Fix and improve the documentation.

\ vim: filetype=gforth
