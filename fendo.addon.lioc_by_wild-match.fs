.( fendo.addon.lioc_by_wild-match.fs) cr

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

\ 2014-11-16 Start.

\ **************************************************************
\ Requirements

forth_definitions

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.lioc.fs
require ./fendo.addon.wild-match.fs

\ **************************************************************

module: fendo.addon.lioc_by_wild-match

: ((lioc_by_wild-match))  { D: pid -- }
  \ Create an element of a list of content,
  \ if the given pid (page id) matches the current wild-match string.
  pid wild-match$ $@ wild-match? 0= ?exit
  pid pid$>data>pid# draft? ?exit
  pid lioc
  ;
: (lioc_by_wild-match)  ( ca len -- true )
  \ ca len = pid
  \ true = continue with the next element?
  ((lioc_by_wild-match)) true
  ;

export

: lioc_by_wild-match  ( ca len -- )
  \ Create a list of content
  \ with pages whose pid matches the given wild-match string.
  wild-match$ $! ['] (lioc_by_wild-match) traverse_pids
  ;

;module

.( fendo.addon.lioc_by_wild-match.fs compiled) cr

