.( fendo.addon.lioc_by_prefix.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several content lists addons.

\ Last modified 201812081823.
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
\ Requirements

forth_definitions

require string.fs  \ Gforth's dynamic strings

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.lioc.fs

\ ==============================================================

package fendo.addon.lioc_by_prefix

variable prefix
: ((lioc_by_prefix))  { D: pid -- }
  pid prefix $@ string-prefix? 0= ?exit
  pid pid$>data>pid# draft? ?exit
  pid lioc  ;
  \ Create an element of a list of content
  \ if the given page ID starts with the current prefix.

: (lioc_by_prefix) ( ca len -- f )
  ((lioc_by_prefix)) true ;
  \ ca len = page ID
  \ f = continue with the next element?

public

: lioc_by_prefix ( ca len -- )
  prefix $!  ['] (lioc_by_prefix) traverse_pids ;
  \ Create a list of content
  \ with pages whose page ID starts with the given prefix.
  \ ca len = prefix

end-package

.( fendo.addon.lioc_by_prefix.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-25: Code extracted from the application Fendo-programandala.
\
\ 2013-11-27: Change: several words renamed, after a new uniform notation:
\   "pid$" and "pid#" for both types of page IDs.
\
\ 2014-03-02: Rewritten with `traverse_pids`. Renamed.
\
\ 2014-03-03: Draft pages are not included.
\
\ 2014-03-12: Improvement: faster, with `?exit` and rearranged
\ conditions.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
