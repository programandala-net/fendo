.( fendo.addon.tag_cloud.fs) cr

\ This file is part of Fendo.

\ This file provides tag lists.

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

\ 2014-03-04: Start.

\ **************************************************************
\ Requirements

\ From Fendo
require ./fendo.addon.tags.fs

\ **************************************************************

: tag_list  ( pid -- )
  \ Create a tag list of links for the given page id.
  tags_do_listed_link tags
  2dup type cr key drop  \ xxx informer
  evaluate_tags
  ;
: tag_ul  ( pid -- )
  [<ul>] tag_list [</ul>]
  ;
: tag_ol  ( pid -- )
  [<ol>] tag_list [</ol>]
  ;

.( fendo.addon.tag_cloud.fs compiled) cr



