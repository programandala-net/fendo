.( fendo.addon.lioc_by_match.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several content lists addons.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017 Marcos Cruz (programandala.net)

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

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.lioc.fs
require ./fendo.addon.match.fs

\ ==============================================================

module: fendo.addon.lioc_by_match

: ((lioc_by_match))  { D: pid -- }
  pid match$ $@ match? 0= ?exit
  pid pid$>data>pid# draft? ?exit
  pid lioc ;
  \ Create an element of a list of content,
  \ if the given pid (page id) matches the current match string.

: (lioc_by_match) ( ca len -- true )
  ((lioc_by_match)) true ;
  \ ca len = pid
  \ true = continue with the next element?

export

: lioc_by_match ( ca len -- )
  match$ $! ['] (lioc_by_match) traverse_pids ;
  \ Create a list of content
  \ with pages whose pid matches the given match string.

;module

.( fendo.addon.lioc_by_match.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-11-16 Start.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
