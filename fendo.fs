.( fendo.fs ) cr

\ This file is part of Fendo.

\ This file is the main one; it loads all the modules.

\ Copyright (C) 2012,2013,2014 Marcos Cruz (programandala.net)

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
\ Version history of Fendo

\ 2012-06-30: Start of version A-00.
\
\ 2013-06-28: Start of version A-01.
\
\ 2013-10-22: Start of version A-02.
\
\ 2014-02-15: Start of version A-03. All files are renamed.
\
\ 2014-04-20: Start of version A-04. Improved, more homogenous, and
\ more regular markup.
\
\ 2014-11-08: Start of version A-05. Many fixes and improvements.  The
\ last one: 'unmarkup' is used instead of duplicated plain text
\ versions of the main data fields.

\ **************************************************************
\ Change history of this file

\ See at the end of the file.

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

false value [bug_thread] immediate  \ xxx tmp

\ **************************************************************
\ Requirements

only forth definitions

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
require galope/paren-star.fs  \ '(*'
require galope/sconstant.fs  \ 'sconstant'
require galope/slash-sides.fs  \ '/sides'
require galope/tilde-tilde.fs  \ improved '~~' for debugging
require galope/trim.fs  \ 'trim'

false [if]

  \ xxx fixme
  \ 2013-08-10: Without this string buffer, the input stream gets
  \ corrupted at the end of the template.  I didn't find the bug
  \ yet. It seems a problem with Gforth's 's+'.

  require galope/sb.fs  \ string buffer
  1024 100 * heap_sb
  \ warnings @  warnings off
  \ ' bs" alias s"  immediate
  \ ' bs+ alias s+
  \ ' bs& alias s&
  \ warnings !

[else]

  \ xxx tmp

  \ 2014-02-22: It seems the problem has vanished.  The circular
  \ string buffer is not necessary anymore.  Two words of it are still
  \ used in <fendo.markup.wiki.fs>; they are deactivated here:

  ' noop alias >sb
  ' s+ alias bs&

[then]

anew --fendo--

false [if]  \ xxx todo

false  \ Gforth's dynamic strings instead of FFL's?
dup     constant gforth-strings?
dup     constant [gforth-strings?]  immediate
0= dup  constant ffl-strings?
        constant [ffl-strings?]  immediate
[then]

s" gforth" environment? drop s" 0.8" str< [if]

\ Safer alternatives for words of Gforth's string.fs
\ Not necessary with Gforth 0.8.

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
table constant fendo_markup_macros_wid  \ user macros
wordlist constant fendo_markup_wid  \ markup, except HTML entities and user macros
wordlist constant fendo_wid  \ program, except markup and HTML entities
wordlist constant fendo_pid_wid  \ page ids

: forth>current  ( -- )
  forth-wordlist set-current
  ;
: markup>current  ( -- )
  fendo_markup_wid set-current
  ;
: entities>current  ( -- )
  fendo_markup_html_entities_wid set-current
  ;
\ XXX OLD
\ variable recognize_macros?  \ flag, turned off for parsing HTML parameters
\ recognize_macros? on
: markup_wids  ( -- wid'1 ... wid'n )
  \ Return the wordlists that contain markup to be recognized.
  \ wid'1 = highest priority wordlist
  \ wid'n = lowest priority wordlist
\ XXX OLD
\  recognize_macros? @ if
    fendo_markup_macros_wid
\  then
  fendo_markup_html_entities_wid
  fendo_markup_wid
  ;
: markup_order  ( -- wid'1 ... wid'n n )
  \ Return the wordlist order required for the markup parsing.
  \ wid'1 = highest priority wordlist
  \ wid'n = lowest priority wordlist
  depth >r markup_wids depth r> -
  ;
: markup>order  ( -- )
  markup_order 0 ?do  >order  loop
  ;
: set_markup_order  ( -- )
  markup_order set-order
  ;
: [markup>order]  ( -- )
  markup>order
  ;  immediate
: markup<order  ( -- )
  markup_order 0 ?do  drop previous  loop
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
: fendo<order  ( -- )
  previous
  ;
: [fendo>order]  ( -- )
  fendo>order
  ;  immediate
: [fendo<order]  ( -- )
  fendo<order
  ;  immediate
: forth>order  ( -- )
  forth-wordlist >order
  ;
: [forth>order]  ( -- )
  forth>order
  ;  immediate
: set_forth_order  ( -- )
  only forth>order
  ;
: set_fendo_order  ( -- )
  set_forth_order fendo>order
  ;
: markup_definitions  ( -- )
  set_forth_order markup>order fendo>order markup>current
  ;
: fendo_definitions  ( -- )
  set_fendo_order fendo>current
  ;
: forth_definitions  ( -- )
  set_forth_order forth>current
  ;

fendo_definitions

\ **************************************************************
\ Config

s" A-04-20140714" 2constant fendo_version
s" Fendo (Forth Engine for Net DOcuments) " fendo_version s+ 2constant generator

false constant link_text_as_attribute?  \ xxx tmp -- experimental

\ **************************************************************
\ Modules

false value multilingual?  \ to be changed by <addons/multilingual.fs>

depth [if] abort [then]  \ xxx debugging
include ./fendo.config.fs
depth [if] abort [then]  \ xxx debugging
defer unshortcut  \ defined in <fendo.shortcuts.fs>
defer just_unshortcut  \ defined in <fendo.shortcuts.fs>
include ./fendo.data.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo.echo.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo.files.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo.links.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo.markup.fs
depth [if] abort [then]  \ xxx debugging
include ./fendo.parser.fs
depth [if] abort [then]  \ xxx debugging

\ **************************************************************
\ Change history of this file

\ 2012-06-30: Start.
\ 
\ 2013-04-28: New: <fendo_data.fs>, <fendo_content.fs>.
\
\ 2013-05-07: New: <fendo_require.fs>.
\
\ 2013-06: New: Generic tool words; wordlists.
\
\ 2013-07-09: Change: 'parse-name?' moved to Galope.
\
\ 2014-02-05: New: 'fendo_markup_macros_wid'.
\
\ 2014-02-05: New: 'markup_wids', 'markup_order', 'set_markup_order';
\ they are written to make the parser kernel simpler and easier to
\ expand.
\
\ 2014-02-05: Change: 'markup>order' and 'markup<order' rewritten with
\ the new word 'markup_order'.
\
\ 2014-03-18: Fix checking "gforth" with 'environment?'.
\
\ 2014-04-20: New: 'markup_definitions', 'fendo_definitions',
\ 'forth_definitions', 'forth>current'...; this makes some thing
\ easier, e.g. modules that define markups and require libraries.  The
\ requirements section of all Fendo files is updated with
\ 'forth_definitions' and 'fendo_definitions'.
\
\ 2014-06-14: New: 'recognize_macros?'.
\
\ 2014-06-15: Change: 'recognize_macros?' is commented out. The
\ problem was solved with 'evaluate_the_markup?' in <fendo.parser.fs>.
.( fendo.fs compiled) cr
