.( fendo.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

\ This file is the main one; it loads all the modules.

\ Copyright (C) 2012,2013 Marcos Cruz (programandala.net)

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
\   <http://forth.org>
\ with Gforth
\   <http://www.gnu.org/software/gforth/>
\   <http://www.bernd-paysan.de/gforth.html>
\   <http://www.complang.tuwien.ac.at/forth/gforth/>

\ **************************************************************
\ Change history of this file

\ 2012-06-30 Start.
\ 2013-04-28 New: <fendo_data.fs>, <fendo_content.fs>.
\ 2013-05-07 New: <fendo_require.fs>.
\ 2013-06 New: Generic tool words; wordlists.
\ 2013-07-09 Change: 'parse-name?' moved to Galope.

\ **************************************************************
\ Version history 

\ 2012-06-30 Start of version A-00.
\ 2013-06-28 Start of version A-01.
\ 2013-10-22 Start of version A-02.

\ **************************************************************
\ Todo

\ 2013-11-09 make '###' to highlight the code, if 'filetype$' is set;
\ rename 'filetype$' to 'programming_language$'.

\ 2013-11-07 bug: local links to draft pages don't wor.
\ this featured is not implemented.

\ 2013-11-05 bug: when a link text is evaluated with
\ 'evaluate_content', and it has markup, the attributes set for the
\ link are used by the those markups? not sure yet. the attributes set
\ must be an array pointed by a counter.
\ (2013-11-07 this seems solved after the implementation of anchors?)

\ 2013-10-28 distinction: 'language' (metadatum) and 'lang'
\ (filename prefix).

\ 2013-10-26 hardcoded '.html'?; use it in the page id?

\ 2013-10-26 fix local links to draft pages!

\ 2013-08-15 defered words at the start (and end?) of tags,
\ to let the website application insert hooks.

\ 2013-08-14 'raw=' is already converted by 'unraw_attributes';
\ maybe the 'raw=' fake attribute can be removed.

\ 2013-08-14 Fix: <p> is closed when ]] is at the end of line!

\ 2013-08-14 Choose better names for different meanings of "content":
\ 1) raw content of the page (printable content and markups)
\ 2) printable content only, not executable markups

\ 2013-06-28 Make "plain_" data fields optional; they can be
\ filled as default data, if empty.
\
\ 2013-06-08 line comments in data header.

\ 2013-10-30 Fix: 'forth-wordlist' is set to current before
\   requiring the library files. The problem was <ffl/config.fs>
\   created 'ffl.version' in the 'fendo' vocabulary, but searched for it
\   in 'forth-wordlist'.
 
\ **************************************************************
\ Stack notation

0 [if]

Some stack notations used in this program are different from the
common usage (shown in brackets):

a         [addr]  address 
ca        [c-addr]  character-aligned address 
ca len    [c-addr u]  character string 
f         [flag]  flag (0=false; other=true)
wf        [flag]  well-formed flag (0=false; -1=true)

[then]

\ **************************************************************
\ Debug

\ cr .( LOADING fendo.fs ) key drop  \ xxx informer

false value [bug_thread] immediate

\ **************************************************************
\ Requirements

forth-wordlist set-current

\ From Gforth

require string.fs  \ dynamic strings

\ From Forth Foundation Library

\ require ffl/str.fs
\ require ffl/tos.fs
\ require ffl/xos.fs

\ From Galope

require galope/3dup.fs
require galope/anew.fs
require galope/backslash-end-of-file.fs  \ '\eof'
require galope/bracket-false.fs  \ '[false]'
require galope/buffer-colon.fs  \ 'buffer:'
require galope/colon-alias.fs  \ ':alias'
require galope/colon-create.fs  \ ':create'
require galope/dollar-variable.fs  \ '$variable'
require galope/enum.fs  \ 'enum'
require galope/minus-bounds.fs  \ '-bounds'
require galope/minus-extension.fs  \ '-extension'
require galope/minus-leading.fs  \ '-leading'
require galope/minus-path.fs  \ '-path'
require galope/minus-suffix.fs  \ '-suffix'
require galope/parse-name-question.fs  \ 'parse-name?'
require galope/sconstant.fs  \ 'sconstant'
require galope/slash-sides.fs  \ '/sides'
require galope/trim.fs  \ 'trim'

true [if]

  \ 2013-08-10 Without this string buffer, the input stream gets
  \ corrupted at the end of the template.  I didn't find the bug
  \ yet. It seems a problem with Gforth's 's+'.

  require galope/sb.fs  \ string buffer
  1024 100 * heap_sb
  warnings @  warnings off
  \ ' bs" alias s"  immediate
  \ ' bs+ alias s+
  \ ' bs& alias s&
  warnings !

[then]

anew --fendo--

false [if]  \ xxx todo
false  \ Gforth's dynamic strings instead of FFL's?
dup     constant gforth-strings?
dup     constant [gforth-strings?]  immediate
0= dup  constant ffl-strings?
        constant [ffl-strings?]  immediate
[then]

true [if]

\ Safer alternatives for words of Gforth's string.fs
warnings @  warnings off
: $@len  ( a -- len )
  \ Return the length of a dynamic string variable,
  \ even if it's not initialized.
  @ dup if  @  then
  ;
: $@  ( a -- ca len )
  \ Return the content of a dynamic string variable,
  \ even if it's not initialized.
  @ dup if  dup cell+ swap @  else  pad swap  then 
  ;
warnings !

[then]

: empty?  ( ca len -- wf )
  \ Is a string empty?
  nip 0=
  ;

\ **************************************************************
\ Wordlists

table constant fendo_markup_html_entities_wid  \ HTML entities
wordlist constant fendo_markup_wid  \ markup, except HTML entities
wordlist constant fendo_wid  \ program, except markup and HTML entities
false [if]  \ xxx old, 2013-10-22 moved to its own file <fendo_shortcuts.fs>
wordlist constant fendo_links_wid  \ user links
[then]

: markup>current  ( -- )
  fendo_markup_wid set-current
  ;
: entities>current  ( -- )
  fendo_markup_html_entities_wid set-current
  ;
: markup>order  ( -- )  
  fendo_markup_html_entities_wid >order  fendo_markup_wid >order 
  ;
: [markup>order]  ( -- )
  markup>order
  ;  immediate
: markup<order  ( -- )  
  previous previous
  ;
: [markup<order]  ( -- )
  markup<order
  ;  immediate
: fendo>current  ( -- )
  fendo_wid set-current
  ; 
: fendo>order  ( -- )
  fendo_wid >order
  ; 
: [fendo>order]  ( -- )
  fendo>order
  ;  immediate
: forth>order  ( -- )
  forth-wordlist >order
  ;
: [forth>order]  ( -- )
  forth>order
  ;  immediate

fendo>order definitions

s" A-02-20131026" 2constant fendo_version

\ **************************************************************
\ Modules

depth [if] abort [then]  \ xxx debugging
include ./fendo_config.fs
depth [if] abort [then]  \ xxx debugging
defer unshortcut  \ defined in <fendo_shortcuts.fs>
include ./fendo_data.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_echo.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_files.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_markup.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_tools.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo_parser.fs
depth [if] abort [then]  \ xxx debugging

.( fendo.fs compiled) cr
