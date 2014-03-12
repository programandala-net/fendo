.( fendo.addon.pages_by_regex.fs) cr

\ This file is part of Fendo.

\ This file provides a word that counts all pages whose pid matches a
\ regex.

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

\ 2014-03-09: Written, after <fendo.addon.pages_by_prefix.fs>.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.regex.fs

\ From Galope
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/rgx-wcmatch-question.fs  \ 'rgx-wcmatch?'

\ **************************************************************

module: fendo.addon.pages_by_regex

variable pages
: ((pages_by_regex))  { D: pid -- }
  \ Increase the number of pages whose pid matches the current regex.
  pid regex rgx-wcmatch? 0= ?exit
  pid pid$>data>pid# draft? ?exit  1 pages +!
  ;
: (pages_by_regex)  ( ca len -- f )
  \ Increase the number of pages whose pid matches the current regex.
  \ ca len = pid
  \ f = continue with the next element?
  ((pages_by_regex)) true
  ;

export

: pages_by_regex  ( ca len -- n )
  \ Number of pages whose pid starts with the given prefix.
  >regex pages off   ['] (pages_by_regex) traverse_pids  pages @
  ;

;module

.( fendo.addon.pages_by_regex.fs compiled) cr


