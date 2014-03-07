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

module: fendo.addon.tag_cloud  \ xxx tmp

: count_tags  ( ca len -- )
  \ ca len = pid
\  ." count_tags " 2dup type cr  \ xxx informer
  pid$>data>pid# tags evaluate_tags
  ;
0 [if]  \ xxx todo
: count_tags_by_regex  ( ca len -- f )
  \ Increas the count of tags that are in pages whose pid
  \ matchs the current regex. 
  \ ca len = pid
  \ f = continue with the next element?
  2dup regex rgx-wcmatch? ?count_tags true
  ;
[then]
variable prefix$
: (count_tags_by_prefix)  { D: pid -- }
  \ Increase the count of tags that are in pages whose pid
  \ matchs the current regex. 
\  ." count_tags_by_prefix " 2dup type cr  \ xxx informer
  pid pid$>data>pid# draft? ?exit
  pid prefix$ $@ string-prefix? if  pid count_tags  then
  ;
: count_tags_by_prefix  ( ca len -- f )
  (count_tags_by_prefix) true
  ;

: init_tags  ( xt -- )
\  ." init_tags" cr  \ xxx informer
  tags_do_reset execute_all_tags  tags_do_increase  traverse_pids
  ;

export

variable numbers_in_tag_cloud
  \ flag: do include numbers?
variable numbers_linked_in_tag_cloud
  \ flag: is so, do include them in the link?

hide

: echo_tag_count  ( tag -- )
  s\" &nbsp;<span class=\"tagCount\">(" echo
  tag>count @ echo. s" )</span>" echo
  ;
: tag_cloud_numbers?  ( f1 -- f2 )
  numbers_in_tag_cloud @ and
  ;
: ?echo_tag_count  ( tag f -- )
  tag_cloud_numbers? if  echo_tag_count  else  drop  then
  ;
: inner_count  ( tag -- )
  numbers_linked_in_tag_cloud @ ?echo_tag_count
  ;
: outer_count  ( tag -- )
  numbers_linked_in_tag_cloud @ 0= ?echo_tag_count
  ;
: (tag_does_cloud)  { tag -- }
  \ Create a tag cloud link to the given tag.
  [<li>]  tag tag_link  tag inner_count  [</li>]  tag outer_count
  ;
: tag_does_cloud  ( tag -- )
\  ." tag_does_cloud" cr  \ xxx informer
  dup tag>count @ if  (tag_does_cloud)  else  drop  then
  ;
: tags_do_cloud  ( -- )
  ['] tag_does_cloud is (tag_does)
  ;
: do_tag_cloud  ( xt -- )
\  ." do_tag_cloud " cr  \ xxx informer
  init_tags
\  ." do_tag_cloud after init_tags" cr  \ xxx informer
  [<ul>]
  tags_do_cloud execute_all_tags
  [</ul>]
\  ." do_tag_cloud end" cr  \ xxx informer
  ;

export

0 [if]  \ xxx todo
: tag_cloud_by_regex  ( ca len -- )
  \ Create a tag cloud
  \ with pages whose pid matchs the given regex.
  >regex  ['] count_tags_by_regex do_tag_cloud
  ;
[then]
: tag_cloud_by_prefix  ( ca len -- )
  \ Create a tag cloud
  \ with pages whose pid matchs the given prefix.
\  ." tag_cloud_by_prefix " 2dup type cr  \ xxx informer
  prefix$ $!  ['] count_tags_by_prefix do_tag_cloud
  ;

;module

.( fendo.addon.tag_cloud.fs compiled) cr


