.( addons/numbered_list_of_content_by_regex.fs) cr

\ This file is part of Fendo.

\ This file is the addon that creates numbered content lists. 

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

\ 2013-11-26 Start.

\ **************************************************************

require ./list_of_content_by_regex.fs

: numbered_list_of_content_by_regex  ( ca len -- )
  \ Create an numbered list of content
  \ with pages whose page id matches the given prefix.
  [<ol>] list_of_content_by_regex [</ol>]
  ;

.( addons/numbered_list_of_content_by_regex.fs compiled) cr