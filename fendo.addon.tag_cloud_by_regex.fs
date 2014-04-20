.( fendo.addon.tag_cloud_by_regex.fs) cr

\ xxx todo finish converting the code, copied from the prefix version

\ This file is part of Fendo.

\ This file provides tag clouds by a page-id prefix.

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

\ 2014-03-09: Start, with the code of
\ <fendo.addon.tag_cloud_by_prefix.fs>

\ **************************************************************
\ Requirements

forth_definitions

require galope/max-n.fs  \ 'max-n'
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/rgx-wcmatch-question.fs  \ 'rgx-wcmatch?'

fendo_definitions

\ require ./fendo.addon.regex.fs  \ xxx todo
require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.pages_by_regex.fs
\ require ./fendo.addon.tag_counts_by_prefix.fs  \ xxx todo

\ **************************************************************
\ To-do

\ xxx todo
\ move tag_cloud_by_regex to <fendo.addon.tag_cloud_by_regex.fs>

\ move the common code to <fendo.addon.tag_cloud.common.fs>

\ code the font sizes depending on the tag counts

\ **************************************************************

module: fendo.addon.tag_cloud_by_prefix

variable tag_min_count
variable tag_max_count
variable pages

: (tag_does_min_max)  ( tag -- )
  \ Update the min and max count with the count of the given tag.
  tag>count  @ dup
  tag_min_count @ min tag_min_count !
  tag_max_count @ max tag_max_count !
  ;
: tags_do_min_max  ( -- )
  max-n tag_min_count !  0 tag_max_count !
  ['] (tag_does_min_max) is (tag_does)  execute_all_tags
\  tag_min_count @  tag_max_count @
  ;

: count_tags  ( ca len -- )
  \ ca len = pid
\  ." count_tags " 2dup type cr  \ xxx informer
  pid$>data>pid# tags evaluate_tags
  ;
0 [if]  \ xxx todo
: count_tags_by_regex  ( ca len -- f )
  \ Increas the count of tags that are in pages whose pid
  \ matches the current regex. 
  \ ca len = pid
  \ f = continue with the next element?
  2dup regex rgx-wcmatch? ?count_tags true
  ;
[then]
variable prefix$
: (count_tags_by_prefix)  { D: pid -- }
  \ Increase the count of tags that are in pages whose pid
  \ matches the current regex. 
\  ." count_tags_by_prefix " 2dup type cr  \ xxx informer
  pid pid$>data>pid# draft? ?exit
  pid prefix$ $@ string-prefix? if  pid count_tags  then
  ;
: count_tags_by_prefix  ( ca len -- f )
  \ ca len = pid
  (count_tags_by_prefix) true
  ;
: init_tags  ( xt -- min max)
  \ xt = parameter for 'traverse_pids'
\  ." init_tags" cr  \ xxx informer
  tags_do_reset execute_all_tags  tags_do_increase  traverse_pids
  tags_do_min_max
  ;

export

variable tag_cloud_with_counts  \ flag
variable tag_cloud_with_sizes  \ flag
variable tag_cloud_counts_sized  \ flag  \ xxx todo better name
variable tag_min_size  \ percentage
variable tag_max_size  \ percentage

\ Default config, to be changed by the application:
tag_cloud_with_counts on
tag_cloud_with_sizes on
tag_cloud_counts_sized off
090 tag_min_size !
400 tag_max_size !

hide

: (tag_count)  ( tag -- )
  s\" &nbsp;<span class=\"tagCount\">(" echo
  tag>count @ echo.
\  s" /" echo pages @ echo.  \ xxx tmp
\  ." pages " pages @ .  \ xxx xxx informer
  s" )</span>" echo
  ;
: tag_count  ( tag -- )
  tag_cloud_with_counts @ if  (tag_count)  else  drop  then
  ;
: tag_size_range  ( -- n )
  tag_max_size @ tag_min_size @ -
  ;
: tag_count_range  ( -- n )
  tag_max_count @ tag_min_count @ - 
  ;
: tag_size  ( +n1 -- +n2 )
  \ +n1 = tag count
  \ +n2 = tag size (percentage)
  tag_min_count @ - 100 *  tag_count_range /
  tag_size_range *  100 /  tag_min_size @ + 
  ;
: tag_cloud_sizes  ( tag -- )
  \ Set the font size style for the next HTML tag, actually <li> or <a>.
  tag>count @ tag_size n>str s" %" s+ s" font-size:" 2swap s+ style=!
  ;
: ?tag_cloud_sizes  ( tag f -- )
  \ Set the font size style for the next HTML tag, actually <li> or
  \ <a>, if needed.
  tag_cloud_with_sizes @ and if  tag_cloud_sizes  else  drop  then
  ;
: (tag_does_echo_cloud)  { tag -- }
  \ Create a tag cloud link to the given tag.
  tag tag_cloud_counts_sized @ ?tag_cloud_sizes  [<li>]
  tag tag_cloud_counts_sized @ 0= ?tag_cloud_sizes
  tag tag_link  tag tag_count  [</li>]
  ;
: tag_does_echo_cloud  ( tag -- )
  \ Create a tag cloud link to the given tag, if needed.
  dup tag>count @ if  (tag_does_echo_cloud)  else  drop  then
  ;
: tags_do_echo_cloud  ( -- )
  ['] tag_does_echo_cloud is (tag_does)
  ;
: do_tag_cloud  ( xt -- )
\  ." do_tag_cloud " cr  \ xxx informer
  \ xt = parameter for 'init_tags'
  init_tags
\  ." do_tag_cloud after init_tags" cr  \ xxx informer
  [<ul>] tags_do_echo_cloud execute_all_tags [</ul>]
\  ." do_tag_cloud end" cr  \ xxx informer
  ;

export

0 [if]  \ xxx todo
: tag_cloud_by_regex  ( ca len -- )
  \ Create a tag cloud
  \ with pages whose pid matches the given regex.
  >regex  ['] count_tags_by_regex do_tag_cloud
  ;
[then]
: tag_cloud_by_prefix  ( ca len -- )
  \ Create a tag cloud
  \ with pages whose pid matches the given prefix.
\  ." tag_cloud_by_prefix " 2dup type cr  \ xxx informer
  2dup pages_by_prefix
\  dup ." pages " . cr  \ xxx informer
  pages !  prefix$ $!
  ['] count_tags_by_prefix do_tag_cloud
  ;

;module

.( fendo.addon.tag_cloud_by_regex.fs compiled) cr

