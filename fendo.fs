.( fendo.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the main one; it loads all the modules.

\ Last modified 202001190149.
\ See change log at the end of the file.

\ Copyright (C) 2012,2013,2014,2015,2017,2018,2020 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute it and/or modify it
\ under the terms of the GNU General Public License as published by
\ the Free Software Foundation; either version 2 of the License, or
\ (at your option) any later version.
\
\ Fendo is distributed in the hope that it will be useful, but WITHOUT
\ ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
\ or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
\ License for more details.
\
\ You should have received a copy of the GNU General Public License
\ along with this program; if not, see <http://gnu.org/licenses>.

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://www.gnu.org/software/gforth).

\ ==============================================================
\ Debug

false value [bug_thread] immediate  \ XXX TMP

\ ==============================================================
\ Requirements

only forth definitions

\ ------------------------------
\ From Gforth

require string.fs  \ dynamic strings

\ ------------------------------
\ From Forth Foundation Library

\ require ffl/str.fs
\ require ffl/tos.fs
\ require ffl/xos.fs

\ ------------------------------
\ From Galope

\ First load the stringer and replace the original `s"` and `s+` with
\ the stringer variants, in order to make sure the rest of the modules
\ use the stringer:

require galope/stringer.fs \ `stringer`
require galope/s-s-plus.fs \ `ss+`
require galope/s-s-quote.fs \ `ss"`

1024 dup * 64 * allocate-stringer \ 64-MiB buffer by default

synonym s" ss" \ Replace `s"` with the `stringer` version `ss"`
synonym s+ ss+ \ Replace `s+` with the `stringer` version `ss+`

\ Then load the rest of the required modules:

require galope/three-dup.fs
require galope/anew.fs
require galope/backslash-end-of-file.fs  \ `\eof`
require galope/bracket-false.fs  \ `[false]`
require galope/buffer-colon.fs  \ `buffer:`
require galope/colon-alias.fs  \ `:alias`
require galope/colon-create.fs  \ `:create`
require galope/dollar-variable.fs  \ `$variable`
require galope/enum.fs  \ `enum`
require galope/minus-extension.fs  \ `-extension`
require galope/minus-leading.fs  \ `-leading`
require galope/minus-suffix.fs  \ `-suffix`
require galope/parse-name-question.fs  \ `parse-name?`
require galope/paren-star.fs  \ `(*`
require galope/sconstant.fs  \ `sconstant`
require galope/sides-slash.fs  \ `sides/`
require galope/slash-sides.fs  \ `/sides`
require galope/tilde-tilde.fs  \ improved `~~` for debugging
require galope/trim.fs  \ `trim`

\ Safer alternatives for words of Gforth's string.fs
\ (they will not be defined if Gforth >= 0.8):

require galope/dollar-fetch.fs  \ `$@`
require galope/dollar-fetch-len.fs  \ `$@len`

false [if]

  \ XXX FIXME -- 2013-08-10: Without this string buffer, the input
  \ stream gets corrupted at the end of the template.  I didn't find
  \ the bug yet.

  require galope/sb.fs  \ string buffer
  1024 100 * heap_sb
  \ warnings @  warnings off
  \ ' bs" alias s"  immediate
  \ ' bs+ alias s+
  \ ' bs& alias s&
  \ warnings !

[else]

  \ XXX TMP -- 2014-02-22: It seems the problem has vanished.  The
  \ circular string buffer is not necessary anymore.  Two words of it
  \ are still used in <fendo.markup.wiki.fs>; they are deactivated
  \ here:

  ' noop alias >sb
  ' s+ alias bs&

[then]

\ ------------------------------
\ Other

: empty? ( ca len -- f )
  nip 0= ;
  \ Is a string empty?

\ ==============================================================

anew -fendo

false [if]  \ XXX TODO

false  \ Gforth's dynamic strings instead of FFL's?
dup     constant gforth-strings?
dup     constant [gforth-strings?]  immediate
0= dup  constant ffl-strings?
        constant [ffl-strings?]  immediate
[then]

\ ==============================================================
\ Wordlists

table constant fendo_markup_html_entities_wid  \ HTML entities
table constant fendo_markup_macros_wid  \ user macros
wordlist constant fendo_markup_wid  \ markup, except HTML entities and user macros
wordlist constant fendo_wid  \ program, except markup and HTML entities
wordlist constant fendo_pid_wid  \ page IDs

: forth>current ( -- )
  forth-wordlist set-current ;

: markup>current ( -- )
  fendo_markup_wid set-current ;

: entities>current ( -- )
  fendo_markup_html_entities_wid set-current ;

\ XXX OLD
\ variable recognize_macros?  \ flag, turned off for parsing HTML parameters
\ recognize_macros? on
: markup_wids ( -- wid'1 ... wid'n )
  \ Return the wordlists that contain markup to be recognized.
  \ wid'1 = highest priority wordlist
  \ wid'n = lowest priority wordlist
\ XXX OLD
\  recognize_macros? @ if
    fendo_markup_macros_wid
\  then
  fendo_markup_html_entities_wid
  fendo_markup_wid ;

: markup_order ( -- wid'1 ... wid'n n )
  \ Return the wordlist order required for the markup parsing.
  \ wid'1 = highest priority wordlist
  \ wid'n = lowest priority wordlist
  depth >r markup_wids depth r> - ;

: markup>order ( -- )
  markup_order 0 ?do  >order  loop ;

: set_markup_order ( -- )
  markup_order set-order ;

: [markup>order] ( -- )
  markup>order ; immediate

: markup<order ( -- )
  markup_order 0 ?do  drop previous  loop ;

: [markup<order] ( -- )
  markup<order ; immediate

: fendo>current ( -- )
  fendo_wid set-current ;

: fendo>order ( -- )
  fendo_wid >order ;

: fendo<order ( -- )
  previous ;

: [fendo>order] ( -- )
  fendo>order ; immediate

: [fendo<order] ( -- )
  fendo<order ; immediate

: forth>order ( -- )
  forth-wordlist >order ;

: [forth>order] ( -- )
  forth>order ; immediate

: set_forth_order ( -- )
  only forth>order ;

: set_fendo_order ( -- )
  set_forth_order fendo>order ;

: markup_definitions ( -- )
  set_forth_order markup>order fendo>order markup>current ;

: fendo_definitions ( -- )
  set_fendo_order fendo>current ;

: forth_definitions ( -- )
  set_forth_order forth>current ;

fendo_definitions

\ ==============================================================
\ Config

require VERSION.fs

: generator ( -- ca len )
  s" Fendo (Forth Engine for Net DOcuments) " fendo_version s+ ;

false constant link_text_as_attribute?  \ XXX TMP -- experimental

\ ==============================================================
\ Modules

false value multilingual?  \ to be changed by <addons/multilingual.fs>

depth [if] abort [then]  \ XXX DEBUGGING
include ./fendo.config.fs
depth [if] abort [then]  \ XXX DEBUGGING
defer unshortcut        \ defined in <fendo.shortcuts.fs>
defer just_unshortcut   \ defined in <fendo.shortcuts.fs>
defer dry_unshortcut    \ defined in <fendo.shortcuts.fs>  \ XXX TMP
defer -anchor           \ defined in <fendo.links.fs>  \ XXX TMP
defer -anchor!          \ defined in <fendo.links.fs>  \ XXX TMP
defer -anchor?!         \ defined in <fendo.links.fs>  \ XXX TMP
defer link_anchor       \ dynamic string, defined in <fendo.links.fs>
defer link_text         \ dynamic string, defined in <fendo.links.fs>
defer link_anchor+      \ defined in <fendo.links.fs>  \ XXX TMP
include ./fendo.data.fs
depth [if] abort [then]  \ XXX DEBUGGING
include ./fendo.echo.fs
depth [if] abort [then]  \ XXX DEBUGGING
include ./fendo.files.fs
depth [if] abort [then]  \ XXX DEBUGGING
include ./fendo.links.fs
depth [if] abort [then]  \ XXX DEBUGGING
include ./fendo.markup.fs
depth [if] abort [then]  \ XXX DEBUGGING
include ./fendo.parser.fs
depth [if] abort [then]  \ XXX DEBUGGING

.( fendo.fs compiled) cr

\ ==============================================================
\ Change log

\ 2012-06-30: Start.
\
\ 2013-04-28: New: <fendo_data.fs>, <fendo_content.fs>.
\
\ 2013-05-07: New: <fendo_require.fs>.
\
\ 2013-06: New: Generic tool words; wordlists.
\
\ 2013-07-09: Change: `parse-name?` moved to Galope.
\
\ 2014-02-05: New: `fendo_markup_macros_wid`.
\
\ 2014-02-05: New: `markup_wids`, `markup_order`, `set_markup_order`;
\ they are written to make the parser kernel simpler and easier to
\ expand.
\
\ 2014-02-05: Change: `markup>order` and `markup<order` rewritten with
\ the new word `markup_order`.
\
\ 2014-03-18: Fix checking "gforth" with `environment?`.
\
\ 2014-04-20: New: `markup_definitions`, `fendo_definitions`,
\ `forth_definitions`, `forth>current`...; this makes some thing
\ easier, e.g. modules that define markups and require libraries.  The
\ requirements section of all Fendo files is updated with
\ `forth_definitions` and `fendo_definitions`.
\
\ 2014-06-14: New: `recognize_macros?`.
\
\ 2014-06-15: Change: `recognize_macros?` is commented out. The
\ problem was solved with `evaluate_the_markup?` in <fendo.parser.fs>.
\
\ 2015-02-11: Change: requirements reorganized and tidied.
\
\ 2015-02-12: Change: `link_anchor` and `link_text` are defered here
\ and defined in <fendo.fs>. Required because of a fix.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-11-04: Update to Galope 0.103.0: Remove <galope/minus-path.fs>
\ and use Gforth's `basename` instead.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2020-01-19: Use the stringer and replace `s"` and `s+` with their
\ stringer versions `ss"` and `ss+`. This partly avoids memory
\ allocation errors when hundreds of pages are built at once.

\ vim: filetype=gforth
