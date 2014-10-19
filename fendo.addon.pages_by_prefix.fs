.( fendo.addon.pages_by_prefix.fs) cr

\ This file is part of Fendo.

\ This file provides a word that counts all pages whose pid matches a
\ prefix.

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

\ 2014-03-07: Start. First working version.

\ **************************************************************
\ Requirements

forth_definitions

require galope/module.fs

fendo_definitions

require ./fendo.addon.traverse_pids.fs

\ **************************************************************

module: fendo.addon.pages_by_prefix

variable prefix$  \ module variable; in <fendo.addon.tag_cloud_by_prefix.fs> there's another one
variable pages
: ((pages_by_prefix))  { D: pid -- }
  \ Increase the number of pages whose pid starts with the given prefix.
  pid prefix$ $@ string-prefix? 0= ?exit
  pid pid$>data>pid# draft? ?exit  1 pages +!
  ;
: (pages_by_prefix)  ( ca len -- f )
  \ Increase the number of pages whose pid starts with the given prefix.
  \ ca len = pid
  \ f = continue with the next element?
  ((pages_by_prefix)) true
  ;

export

: pages_by_prefix  ( ca len -- n )
  \ Number of pages whose pid starts with the given prefix.
  \ Update 'prefix' and 'pages'.
\  cr ." In pages_by_prefix the prefix is " 2dup type  \ xxx informer
  prefix$ $!  pages off   ['] (pages_by_prefix) traverse_pids  pages @
\  ." and the pages count is " dup . \ xxx informer
  ;

;module

.( fendo.addon.pages_by_prefix.fs compiled) cr

