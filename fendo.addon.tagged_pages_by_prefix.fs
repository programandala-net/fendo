.( fendo.addon.tagged_pages_by_prefix.fs) cr

\ This file is part of Fendo.

\ This file provides lists of tagged pages.

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

\ 2014-03-07: Start.
\ 2014-03-12: Improvement: '((tagged_pages_by_prefix))' rearranged, faster.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.dtddoc.fs

\ From Galope
require galope/module.fs


\ **************************************************************

module: fendo.addon.tagged_pages_by_prefix

variable prefix$
: flags>or  ( f1 ... fn n -- f )
  1- 0 ?do  or  loop 
  ;
: ((tagged_pages_by_prefix))  { D: pid -- }
  \ Create a description list of content
  \ if the given pid starts with the current prefix.
  \ ca len = pid
  \ f = continue with the next element?
  pid prefix$ $@ string-prefix? 0= ?exit
  pid pid$>data>pid# dup draft?
  if    drop
  else  tags evaluate_tags tag_presence @
        if  pid dtddoc  tag_presence off  then
  then
  ;
: (tagged_pages_by_prefix)  ( ca len -- f )
  ((tagged_pages_by_prefix)) true
  ;

export

: tagged_pages_by_prefix  ( ca1 len1 ca2 len2 -- )
  \ ca1 len1 = tag
  \ ca2 len2 = prefix
  prefix$ $!  tags_do_presence
  [<dl>] ['] (tagged_pages_by_prefix) traverse_pids [</dl>]
  ;

;module

.( fendo.addon.tagged_pages_by_prefix.fs compiled) cr

