.( fendo.addon.tag_cloud.fs) cr

\ This file is part of Fendo.

\ This file provides tag clouds.

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

\ 2014-03-03: Start.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.regex.fs
require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs

\ From Galope
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/rgx-wcmatch-question.fs  \ 'rgx-wcmatch?'

\ **************************************************************

(*

First, the tags must be reseted and then counted.
This means two 'traverse_pids'.

Second, the tags with one or more instances must be executed
to create links...

We could write and use 'traverse-wordlist', but the tag links would be
unsorted.

We can create a file, say redirecting the output to a file and then:

   fendo_tags_wid words

the file can be sorted with sed and 'system', and finally interpreted.

*)

\ module: fendo.addon.tag_cloud  \ xxx tmp


: count_tags  ( ca len -- )
  \ ca len = pid
  pid$>data>pid# tags evaluate
  ;
: ?count_tags  ( ca len f -- )
  if  count_tags  else  2drop  then
  ;
: count_tags_by_regex  ( ca len -- f )
  \ Increas the count of tags that are in pages whose pid
  \ matchs the current regex. 
  \ ca len = pid
  \ f = continue with the next element?
  2dup regex rgx-wcmatch? ?count_tags true
  ;
variable tag_cloud_prefix$
: count_tags_by_prefix  ( ca len -- f )
  \ Increas the count of tags that are in pages whose pid
  \ matchs the current regex. 
  \ ca len = pid
  \ f = continue with the next element?
  2dup tag_cloud_prefix$ $@ string-prefix? ?count_tags true
  ;

: init_tags  ( -- )
  tags_do_reset execute_tags  tags_do_increase
  ;

\ export

: tag_cloud_by_regex  ( ca len -- )
  \ Create a tag cloud
  \ with pages whose pid matchs the given regex.
  >regex  init_tags ['] count_tags_by_regex traverse_pids 
  ;
: tag_cloud_by_prefix  ( ca len -- )
  \ Create a tag cloud
  \ with pages whose pid matchs the given prefix.
  tag_cloud_prefix$ $!  init_tags ['] count_tags_by_prefix traverse_pids 
  ;

\ ;module

.( fendo.addon.tag_cloud.fs compiled) cr


