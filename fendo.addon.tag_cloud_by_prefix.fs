.( fendo.addon.tag_cloud_by_prefix.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides tag clouds by a page-id prefix.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================
\ Requirements

forth_definitions

require galope/max-n.fs  \ 'max-n'
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/rgx-wcmatch-question.fs  \ 'rgx-wcmatch?'

fendo_definitions

\ require ./fendo.addon.regex.fs  \ XXX TODO
require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.pages_by_prefix.fs
\ require ./fendo.addon.tag_counts_by_prefix.fs  \ XXX TODO

\ ==============================================================
\ To-do

\ XXX TODO

\ move the common code to <fendo.addon.tag_cloud.common.fs>

\ ==============================================================

module: fendo.addon.tag_cloud_by_prefix

variable tag_min_count
variable tag_max_count
variable pages

: (tag_does_min_max) ( tag -- )
  tag>count  @ dup
  tag_min_count @ min tag_min_count !
  tag_max_count @ max tag_max_count ! ;
  \ Update the min and max count with the count of the given tag.

: tags_do_min_max ( -- )
  max-n tag_min_count !  0 tag_max_count !
  ['] (tag_does_min_max) is (tag_does)  execute_all_tags
  \ tag_min_count @  tag_max_count @ \ XXX OLD XXX TMP --
  ;

: count_tags ( ca len -- )
\  cr ." pid$ = " 2dup type  \ XXX INFORMER
  pid$>data>pid# tags
\  cr ." tags = " 2dup type  \ XXX INFORMER
  evaluate_tags ;
  \ Count the tags in the given pid
  \ ca len = pid

variable prefix$  \ module variable; in <fendo.addon.pages_by_prefix.fs> there's another one
: (count_tags_by_prefix) { D: pid -- }
\  cr ." In count_tags_by_prefix pid is " pid type  \ XXX INFORMER
\  ."  and prefix is <" prefix$ $@ type ." >" cr  \ XXX INFORMER
  pid prefix$ $@ string-prefix? 0= ?exit
  pid pid$>data>pid# draft? ?exit
  pid count_tags ;
  \ Increase the count of tags that are in pages whose pid
  \ matches the current regex.

: count_tags_by_prefix ( ca len -- f )
  (count_tags_by_prefix) true ;
  \ ca len = pid
  \ f = continue with the next element?

: init_tags ( xt -- min max)
\  ." init_tags" cr  \ XXX INFORMER
  tags_do_reset execute_all_tags  tags_do_increase  traverse_pids
  tags_do_min_max ;
  \ xt = parameter for 'traverse_pids'

export

variable tag_cloud_with_counts  \ flag
variable tag_cloud_with_sizes  \ flag
variable tag_cloud_counts_sized  \ flag  \ XXX TODO better name
variable tag_min_size  \ percentage
variable tag_max_size  \ percentage

\ Default config, to be changed by the application:
tag_cloud_with_counts on
tag_cloud_with_sizes on
tag_cloud_counts_sized off
lonely_tags_link_to_content on
090 tag_min_size !
400 tag_max_size !

hide

: (tag_count) ( tag -- )
  s\" &nbsp;<span class=\"tagCount\">(" echo
  tag>count @ 
\  dup ."  count=" . key drop  \ XXX INFORMER
  echo.
\  s" /" echo pages @ echo.  \ XXX TMP
\  ." pages " pages @ .  \ XXX INFORMER
  s" )</span>" echo ;

: tag_count ( tag -- )
  tag_cloud_with_counts @ if  (tag_count)  else  drop  then ;

: tag_size_range ( -- n )
  tag_max_size @ tag_min_size @ - ;

: tag_count_range ( -- n )
  tag_max_count @ tag_min_count @ - ;

: tag_size ( +n1 -- +n2 )
  \ +n1 = tag count
  \ +n2 = tag size (percentage)
  tag_min_count @ - 100 *  tag_count_range /
  tag_size_range *  100 /  tag_min_size @ + ;

: tag_cloud_sizes ( tag -- )
  \ Set the font size style for the next HTML tag, actually <li> or <a>.
  tag>count @ tag_size n>str s" %" s+ s" font-size:" 2swap s+ style=! ;

: ?tag_cloud_sizes ( tag f -- )
  \ Set the font size style for the next HTML tag, actually <li> or <a>,
  \ if needed.
  tag_cloud_with_sizes @ and if  tag_cloud_sizes  else  drop  then ;

: (tag_does_echo_cloud) { tag -- }
\  tag tag>name cr ." In (tag_does_echo_cloud) the tag name is " type  \ XXX INFORMER
  tag tag_cloud_counts_sized @ ?tag_cloud_sizes  [<li>]
  tag tag_cloud_counts_sized @ 0= ?tag_cloud_sizes
  tag tag_link  tag tag_count  [</li>] ;
  \ Create a tag cloud link to the given tag.

: tag_does_echo_cloud ( tag -- )
  dup tag>count @
\  dup cr ." In tag_does_echo_cloud the tag count is " .  \ XXX INFORMER
  if  (tag_does_echo_cloud)  else  drop  then ;
  \ Create a tag cloud link to the given tag, if needed.

: tags_do_echo_cloud ( -- )
  ['] tag_does_echo_cloud is (tag_does) ;

: do_tag_cloud ( xt -- )
\  ." do_tag_cloud " cr  \ XXX INFORMER
  \ xt = parameter for 'init_tags'
  init_tags
\  ." do_tag_cloud after init_tags" cr  \ XXX INFORMER
  [<ul>] tags_do_echo_cloud execute_all_tags [</ul>]
\  ." do_tag_cloud end" cr  \ XXX INFORMER
  ;

export

: tag_cloud_by_prefix ( ca len -- )
\  cr ." In tag_cloud_by_prefix the prefix is " 2dup type key drop  \ XXX INFORMER
  2dup prefix$ $!  pages_by_prefix drop
  ['] count_tags_by_prefix do_tag_cloud ;
  \ Create a tag cloud
  \ with pages whose pid matches the given prefix.

;module

.( fendo.addon.tag_cloud_by_prefix.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-03-03: Start.
\ 2014-03-07: First working version.
\ 2014-03-08: Improved. First draft to change the size of tags.
\ 2014-03-09: Fix: calculation for the tag font size.
\ 2014-03-12: Improvement: faster, with additional '?exit' and rearranged
\   conditions.
\ 2014-05-28: Change: '++' used.
\ 2014-05-28: New: 'tags_used_only_once_link_to_its_own_page' flag.
\ 2014-06-03: Change: 'tags_used_only_once_link_to_its_own_page'
\   renamed to 'lonely_tags_link_to_content'.
\ 2014-10-12: Fix: now 'tag_cloud_by_prefix' also sets the 'prefix$'
\   variable, because 'pages_by_prefix' sets a homonymous module variable.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
