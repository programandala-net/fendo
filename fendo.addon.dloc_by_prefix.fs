.( fendo.addon.dloc_by_prefix.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several content lists addons.

\ Last modified 201809271539.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.dtddoc.fs

\ ==============================================================

package fendo.addon.dloc_by_prefix

variable prefix
: ((dloc_by_prefix)) { D: pid -- }
  pid prefix $@ string-prefix? 0= ?exit
  pid pid$>data>pid# draft? ?exit
  pid dtddoc  ;
  \ Create a description list of content
  \ if the given pid starts with the current prefix.

: (dloc_by_prefix) ( ca len -- f )
  ((dloc_by_prefix)) true ;
  \ ca len = pid
  \ f = continue with the next element?

public

: dloc_by_prefix ( ca len -- )
  prefix $!  [<dl>] ['] (dloc_by_prefix) traverse_pids [</dl>] ;
  \ Create a description list of content
  \ with pages whose pid starts with the given prefix.

end-package

.( fendo.addon.dloc_by_prefix.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-25: Start. Unfinished.
\ 2014-03-02: Rewritten with 'traverse_pids'.
\ 2014-03-03: Draft pages are not included.
\ 2014-03-06: Typo. Missing requirement.
\ 2014-03-10: Improvement: faster, with '?exit' and rearranged
\ conditions.
\ 2017-06-22: Update source style, layout and header.
\ 2018-09-27: Use `package` instead of `module:`.

\ vim: filetype=gforth
