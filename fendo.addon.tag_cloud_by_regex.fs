.( fendo.addon.tag_cloud_by_regex.fs) cr

\ XXX TODO finish converting the code, copied from the prefix version

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides tag clouds by a page-id prefix.

\ Last modified 201812081823.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017,2018 Marcos Cruz (programandala.net)

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

require galope/max-n.fs  \ `max-n`
require galope/package.fs \ `package`, `private`, `public`, `end-package`
require galope/rgx-wcmatch-question.fs  \ `rgx-wcmatch?`

fendo_definitions

\ require ./fendo.addon.regex.fs  \ XXX TODO
require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.pages_by_regex.fs
\ require ./fendo.addon.tag_counts_by_prefix.fs  \ XXX TODO

\ ==============================================================

package fendo.addon.tag_cloud_by_prefix

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
\  tag_min_count @  tag_max_count @
  ;

: count_tags ( ca len -- )
\  ." count_tags " 2dup type cr  \ XXX INFORMER
  pid$>data>pid# tags evaluate_tags ;
  \ ca len = page ID

0 [if]  \ XXX TODO

: count_tags_by_regex ( ca len -- f )
  2dup regex rgx-wcmatch? ?count_tags true ;
  \ Increas the count of tags that are in pages whose page ID
  \ matches the current regex. 
  \ ca len = page ID
  \ f = continue with the next element?

[then]

variable prefix$
: (count_tags_by_prefix) { D: pid -- }
\  ." count_tags_by_prefix " 2dup type cr  \ XXX INFORMER
  pid pid$>data>pid# draft? ?exit
  pid prefix$ $@ string-prefix? if  pid count_tags  then ;
  \ Increase the count of tags that are in pages whose page ID
  \ matches the current regex. 

: count_tags_by_prefix ( ca len -- f )
  (count_tags_by_prefix) true ;
  \ ca len = page ID

: init_tags ( xt -- min max)
\  ." init_tags" cr  \ XXX INFORMER
  tags_do_reset execute_all_tags  tags_do_increase  traverse_pids
  tags_do_min_max ;
  \ xt = parameter for `traverse_pids`

public

variable tag_cloud_with_counts  \ flag
variable tag_cloud_with_sizes  \ flag
variable tag_cloud_counts_sized  \ flag  \ XXX TODO better name
variable tag_min_size  \ percentage
variable tag_max_size  \ percentage

\ Default config, to be changed by the application:
tag_cloud_with_counts on
tag_cloud_with_sizes on
tag_cloud_counts_sized off
090 tag_min_size !
400 tag_max_size !

private

: (tag_count) ( tag -- )
  s\" &nbsp;<span class=\"tagCount\">(" echo
  tag>count @ echo.
\  s" /" echo pages @ echo.  \ XXX TMP
\  ." pages " pages @ .  \ XXX XXX INFORMER
  s" )</span>" echo ;

: tag_count ( tag -- )
  tag_cloud_with_counts @ if  (tag_count)  else  drop  then ;

: tag_size_range ( -- n )
  tag_max_size @ tag_min_size @ - ;

: tag_count_range ( -- n )
  tag_max_count @ tag_min_count @ - ;

: tag_size ( +n1 -- +n2 )
  tag_min_count @ - 100 *  tag_count_range /
  tag_size_range *  100 /  tag_min_size @ +  ;
  \ +n1 = tag count
  \ +n2 = tag size (percentage)

: tag_cloud_sizes ( tag -- )
  tag>count @ tag_size n>str s" %" s+ s" font-size:" 2swap s+ style=! ;
  \ Set the font size style for the next HTML tag, actually <li> or <a>.

: ?tag_cloud_sizes ( tag f -- )
  tag_cloud_with_sizes @ and if  tag_cloud_sizes  else  drop  then ;
  \ Set the font size style for the next HTML tag, actually <li> or
  \ <a>, if needed.

: (tag_does_echo_cloud) { tag -- }
  tag tag_cloud_counts_sized @ ?tag_cloud_sizes  [<li>]
  tag tag_cloud_counts_sized @ 0= ?tag_cloud_sizes
  tag tag_link  tag tag_count  [</li>] ;
  \ Create a tag cloud link to the given tag.

: tag_does_echo_cloud ( tag -- )
  dup tag>count @ if  (tag_does_echo_cloud)  else  drop  then ;
  \ Create a tag cloud link to the given tag, if needed.

: tags_do_echo_cloud ( -- )
  ['] tag_does_echo_cloud is (tag_does) ;

: do_tag_cloud ( xt -- )
\  ." do_tag_cloud " cr  \ XXX INFORMER
  init_tags
\  ." do_tag_cloud after init_tags" cr  \ XXX INFORMER
  [<ul>] tags_do_echo_cloud execute_all_tags [</ul>]
\  ." do_tag_cloud end" cr  \ XXX INFORMER
  ;
  \ xt = parameter for `init_tags`

public

0 [if]  \ XXX TODO

: tag_cloud_by_regex ( ca len -- )
  >regex  ['] count_tags_by_regex do_tag_cloud ;
  \ Create a tag cloud
  \ with pages whose page ID matches the given regex.

[then]

: tag_cloud_by_prefix ( ca len -- )
\  ." tag_cloud_by_prefix " 2dup type cr  \ XXX INFORMER
  2dup pages_by_prefix
\  dup ." pages " . cr  \ XXX INFORMER
  pages !  prefix$ $!
  ['] count_tags_by_prefix do_tag_cloud ;
  \ Create a tag cloud
  \ with pages whose page ID matches the given prefix.

end-package

.( fendo.addon.tag_cloud_by_regex.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-03-09: Start, with the code of
\ <fendo.addon.tag_cloud_by_prefix.fs>
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
