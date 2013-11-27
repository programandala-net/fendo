.( addons/description_list_of_content_by_regex.fs) cr

\ This file is part of Fendo.

\ This file is the code common to several content lists addons.

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

\ 2013-11-26 Start. First working version.

\ **************************************************************
\ Requirements

\ Fendo addons
require ./page_id_list.fs
require ./page_id_list_regex_filter.fs
require ./definition_list_element.fs

\ **************************************************************

: (description_list_of_content_by_regex)  ( ca len -- )
  \ Create an element of a description list of content
  \ if the given page id matchs the current page id list filter.
  2dup type cr  \ xxx informer
  2dup page_id_list_filter rgx-cmatch?
  if  definition_list_element  else  2drop  then
  ;
: description_list_of_content_by_regex  ( ca len -- )
  \ Create a description list of content
  \ with pages whose page id matchs the given regex. 
  [<dl>]
  >page_id_list_filter open_page_id_list
  begin   page_id_list@ dup
  while   (description_list_of_content_by_regex)
  repeat  2drop close_page_id_list
  [</dl>]
  ;

.( addons/description_list_of_content_by_regex.fs compiled) cr

