.( fendo.addon.dloc_by_regex.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several content lists addons.

\ Last modified  20220123T1335+0100.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018 Marcos Cruz (programandala.net)

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
require galope/rgx-wcmatch-question.fs  \ `rgx-wcmatch?`

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.regex.fs
require ./fendo.addon.dtddoc.fs

\ ==============================================================
\ Code {{{1

package fendo.addon.dloc_by_regex

: ((dloc_by_regex)) { D: pid -- }
  pid regex rgx-wcmatch? 0= ?exit
  pid pid$>pid# draft? ?exit
  pid dtddoc  ;
  \ Create an element of a description list of content
  \ if the given page ID matchs the current regex. 

: (dloc_by_regex) ( ca len -- f )
  ((dloc_by_regex)) true ;
  \ ca len = page ID
  \ f = continue with the next element?

public

: dloc_by_regex ( ca len -- )
  >regex  [<dl>] ['] (dloc_by_regex) traverse_pids [</dl>] ;
  \ Create a description list of content
  \ with pages whose page ID matchs the given regex.

end-package

.( fendo.addon.dloc_by_regex.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2013-11-26: Start. First working version.
\
\ 2013-11-27: Change: several words renamed, after a new uniform
\   notation: "pid$" and "pid#" for both types of page IDs.
\
\ 2014-03-02: Rewritten with `traverse_pids`.
\
\ 2014-03-03: Draft pages are not included.
\
\ 2014-03-09: Improvement: faster, with `?exit` and rearranged
\ conditions.
\
\ 2014-03-10: Fix: double local.
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
