.( addons/page_id_list_regex_filter.fs) cr

\ This file is part of Fendo.

\ This file creates the regex tools required to filter a page id list.

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

require ffl/rgx.fs  \ regular expressions

rgx-create page_id_list_filter
: regex_error  ( ca len n -- )
  ." Bad regular expression at position " . ." :" cr type abort
  ;
: >page_id_list_filter  ( ca len -- )
  2dup page_id_list_filter rgx-compile if  2drop  else  regex_error  then
  ;

.( addons/page_id_list_regex_filter.fs compiled) cr
