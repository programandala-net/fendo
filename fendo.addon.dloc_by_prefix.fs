.( fendo.addon.dloc_by_prefix.fs) cr

\ This file is part of Fendo.

\ This file is the code common to several content lists addons.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ 2013-11-25: Start. Unfinished.
\ 2014-03-02: Rewritten with 'traverse_pids'.
\ 2014-03-03: Draft pages are not included.
\ 2014-03-06: Typo. Missing requirement.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.traverse_pids.fs

\ From Galope
require galope/module.fs

\ **************************************************************

module: fendo.addon.dloc_by_prefix

variable prefix
: (dloc_by_prefix)  ( ca len -- f )
  \ Create a description list of content
  \ if the given pid starts with the current prefix.
  \ ca len = pid
  \ f = continue with the next element?
  2dup pid$>data>pid# draft? 0= >r
  2dup prefix $@ string-prefix? r> and ?dtddoc true
  ;

export

: dloc_by_prefix  ( ca len -- )
  \ Create a description list of content
  \ with pages whose pid starts with the given prefix.
  prefix $!  [<dl>] ['] (dloc_by_prefix) traverse_pids [</dl>]
  ;

;module

.( fendo.addon.dloc_by_prefix.fs compiled) cr
