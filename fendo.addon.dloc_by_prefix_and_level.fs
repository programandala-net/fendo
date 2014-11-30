.( fendo.addon.dloc_by_prefix_and_level.fs) cr

\ This file is part of Fendo.

\ This file is the code common to several content lists addons.

\ Copyright (C) 2014 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2014-11-26: Adapted from <fendo.addon.dloc_by_prefix.fs>.

\ **************************************************************
\ Requirements

forth_definitions

require galope/module.fs

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.dtddoc.fs

\ **************************************************************

module: fendo.addon.dloc_by_prefix_and_level

variable prefix
variable level
: ((dloc_by_prefix_and_level))  { D: pid -- }
  \ Create a description list of content
  \ if the given pid starts with the current prefix
  \ and has the current level.
  pid prefix $@ string-prefix? 0= ?exit
  pid pid$>level level @ <> ?exit
  pid pid$>data>pid# draft? ?exit
  pid dtddoc
  ;
: (dloc_by_prefix_and_level)  ( ca len -- f )
  \ ca len = pid
  \ f = continue with the next element?
  ((dloc_by_prefix_and_level)) true
  ;

export

: dloc_by_prefix_and_level  ( ca len n -- )
  \ Create a description list of content
  \ with pages whose pid has the given prefix and level.
  level ! prefix $!  [<dl>] ['] (dloc_by_prefix_and_level) traverse_pids [</dl>]
  ;

;module

.( fendo.addon.dloc_by_prefix_and_level.fs compiled) cr

