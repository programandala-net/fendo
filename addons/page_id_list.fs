.( addons/page_id_list.fs) cr

\ This file is part of Fendo.

\ This file is the provides a list of page ids, required by other
\ addons.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ 2013-11-25 Code extracted from
\ <addons/list_of_content_by_prefix.fs>.
\ 2013-11-26 Change: several words renamed, after a new uniform
\   notation: "pid$" and "pid#" for both types of page ids.

\ **************************************************************

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/slash-counted-string.fs  \ '/counted-string'

module: page_id_list_fendo_addon_module

s" /tmp/list_of_content_common_fendo_addon.txt" 2constant pages_list_file$
: create_pages_list  ( -- )
  s" ls -1 " source_dir $@ s+ s" *" s+ forth_extension $@ s+
  s"  > " s+ pages_list_file$ s+ system
  ;
0 value pages_list_fid
/counted-string 2 + buffer: pages_list_element

export

: open_page_id_list  ( -- )
  create_pages_list
  pages_list_file$ r/o open-file throw
  to pages_list_fid
  ;
: close_page_id_list  ( -- )
  pages_list_fid close-file throw
  ;
: page_id_list@  ( -- ca len )
  \ Return the next page id from the pages list,
  \ or an empty string if the list is empty.
  pages_list_element dup /counted-string
  pages_list_fid 
  read-line throw and  source>pid$
  ;

;module

.( addons/list_of_content_by_prefix.fs compiled) cr

