.( fendo.addon.lioc_by_prefix_and_level.fs) cr

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

require string.fs  \ Gforth's dynamic strings

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.lioc.fs

\ ==============================================================

package fendo.addon.lioc_by_prefix_and_level

variable prefix

variable level

: ((lioc_by_prefix_and_level))  { D: pid -- }
  pid prefix $@ string-prefix? 0= ?exit
  pid pid$>level level @ <> ?exit
  pid pid$>data>pid# draft? ?exit
  pid lioc ;
  \ Create an element of a list of content
  \ if the given pid starts with the current prefix
  \ and has the current level.

: (lioc_by_prefix_and_level) ( ca len -- true )
  ((lioc_by_prefix_and_level)) true ;
  \ ca len = pid
  \ true = continue with the next element?

public

: lioc_by_prefix_and_level ( ca len n -- )
  level ! prefix $!  ['] (lioc_by_prefix_and_level) traverse_pids ;
  \ Create a list of content
  \ with pages whose pid has the given prefix and level.
  \ ca len = prefix
  \ n = page hierarchical level (0 is the top)

end-package

.( fendo.addon.lioc_by_prefix_and_level.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-11-18: Created, based on <fendo.addon.lioc_by_prefix.fs>.
\ 2017-06-22: Update source style, layout and header.
\ 2018-09-27: Use `package` instead of `module:`.

\ vim: filetype=gforth
