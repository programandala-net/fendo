.( fendo.addon.tag_counts_by_prefix.fs) cr

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

\ 2014-03-07: Start.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.traverse_pids.fs

\ From Galope
require galope/module.fs

\ **************************************************************

module: fendo.addon.tag_counts_by_prefix

variable prefix

: (((tag_counts_by_prefix)))  { D: pid -- }
  \ Increase the number of pages whose pid starts with the given prefix.
  pid prefix $@ string-prefix? 0= ?exit
  pid pid$>data>pid# dup draft? 
  if  drop  else  tags evaluate_tags  then
  ;
: ((tag_counts_by_prefix))  ( ca len -- f )
  \ Increase the number of pages whose pid starts with the given prefix.
  \ ca len = pid
  \ f = continue with the next element?
  (((tag_counts_by_prefix))) true
  ;

: (tag_counts_by_prefix)  ( -- +n_1 ... +n_n n )
  tags_do_total  ['] ((tag_counts_by_prefix)) traverse_pids 
  ;

export

: tag_counts_by_prefix  ( ca len -- +n_1 ... +n_n n )
  \ Return the total counts of all tags
  \ present in pages whose pids start with the given prefix.
  \ xxx todo reset tags; increase them by prefix
  prefix $!  depth >r  (tag_counts_by_prefix)  depth r> -
  ;

;module

.( fendo.addon.tag_counts_by_prefix.fs compiled) cr
