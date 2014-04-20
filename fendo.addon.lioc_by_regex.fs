.( fendo.addon.lioc_by_regex.fs) cr

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

\ 2013-11-25: Code extracted from the application Fendo-programandala.
\ 2013-11-27: Change: several words renamed, after a new uniform notation:
\   "pid$" and "pid#" for both types of page ids.
\ 2014-03-02: Rewritten with 'traverse_pids'. Renamed.
\ 2014-03-03: Draft pages are not included.
\ 2014-03-12: Improvement: faster, with '?exit' and rearranged
\ conditions.

\ **************************************************************
\ Requirements

forth_definitions

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/rgx-wcmatch-question.fs  \ 'rgx-wcmatch?'

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.regex.fs
require ./fendo.addon.lioc.fs

\ **************************************************************

module: fendo.addon.lioc_by_regex

: ((lioc_by_regex))  { D: pid -- }
  \ Create an element of a list of content,
  \ if the given pid matches the current regex.
  \ ca len = pid
  pid regex rgx-wcmatch? 0= ?exit
  pid pid$>data>pid# draft? ?exit
  pid lioc 
  ;
: (lioc_by_regex)  ( ca len -- f )
  \ ca len = pid
  \ f = continue with the next element?
  ((lioc_by_regex)) true
  ;

export

: lioc_by_regex  ( ca len -- )
  \ Create a list of content
  \ with pages whose pid matches the given regex.
  >regex ['] (lioc_by_regex) traverse_pids
  ;

;module

.( fendo.addon.lioc_by_regex.fs compiled) cr
