.( addons/description_list_of_content_by_prefix.fs) cr

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

\ 2013-11-25 Start.

\ **************************************************************

require ./page_id_list.fs  \ Fendo addon

: (description_list_of_content_by_prefix)  ( ca len -- )
  \ Create a description list of content
  \ with pages whose filename start with the given prefix.
  \ xxx todo
  2>r open_page_id_list
  begin   page_id_list@ dup
  while
    2dup 2r@ string-prefix?
    if  [<li>] title_link [</li>]  else  2drop  then
  repeat  2drop close_page_id_list 2rdrop
  ;
: description_list_of_content_by_prefix  ( ca len -- )
  \ Create a description list of content
  \ with pages whose filename start with the given prefix.
  [<dl>] (description_list_of_content_by_prefix) [</dl>]
  ;

.( addons/description_list_of_content_by_prefix.fs compiled) cr

