.( fendo.addon.tagged_pages_by_prefix.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides lists of tagged pages.

\ Last modified  20220123T1353+0100.
\ See change log at the end of the file.

\ Copyright (C) 2014,2015,2017,2018 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute it and/or modify it
\ under the terms of the GNU General Public License as published by
\ the Free Software Foundation; either version 2 of the License, or
\ (at your option) any later version.

\ Fendo is distributed in the hope that it will be useful, but WITHOUT
\ ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
\ or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
\ License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program; if not, see <http://gnu.org/licenses>.

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================
\ Requirements {{{1

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.dtddoc.fs

\ ==============================================================
\ Code {{{1

package fendo.addon.tagged_pages_by_prefix

variable prefix$
: flags>or ( f1 ... fn n -- f )
  1- 0 ?do  or  loop ;

: ((tagged_pages_by_prefix)) { D: pid -- }

  \ Create a description list of content
  \ if the given page ID starts with the current prefix.

\  ." Parameter in `((tagged_pages_by_prefix))` = " pid type cr  \ XXX INFORMER

  \ Do nothing if length is 0.
  \ XXX FIXME
  \ This check seems to be needed, because temporary shortcuts created by the
  \ application could return an empty string, what would make `pid$>pid#` crash.
  \ But it's not clear yet.
\  pid nip 0= ?exit

  pid prefix$ $@ string-prefix? 0= ?exit
  pid pid$>pid#
\  ." pid# = " dup . cr  \ XXX INFORMER

  \ Do nothing if it's a draft page.
  dup draft?  if  drop exit  then

  tags
\  ." `tags` in `((tagged_pages_by_prefix))` = " 2dup type cr  \ XXX INFORMER
  evaluate_tags tag_presence @
\  ." `tag_presence` in `((tagged_pages_by_prefix))` = " dup . cr  \ XXX INFORMER
  if  pid dtddoc  tag_presence off  then ;

: (tagged_pages_by_prefix) ( ca len -- true )
  ((tagged_pages_by_prefix)) true ;
  \ ca len = page ID
  \ true = continue with the next element?

public

: tagged_pages_by_prefix ( ca1 len1 ca2 len2 -- )
\  ." `argc` in `tagged_pages_by_prefix`= " argc ? cr  \ XXX INFORMER
  prefix$ $!  tags_do_presence
  [<dl>] ['] (tagged_pages_by_prefix) traverse_pids [</dl>] ;
  \ ca1 len1 = tag
  \ ca2 len2 = prefix

end-package

.( fendo.addon.tagged_pages_by_prefix.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2014-03-07: Start.
\
\ 2014-03-12: Improvement: `((tagged_pages_by_prefix))` rearranged, faster.
\
\ 2015-02-01: Fix: Now `((tagged_pages_by_prefix))` ignores empty
\ strings.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-17: Update: replace `pid$>data>pid#` with `pid$>pid#`.

\ vim: filetype=gforth
