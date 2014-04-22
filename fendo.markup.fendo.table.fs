.( fendo.markup.fendo.table.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for lists.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ **************************************************************
\ Change history of this file

\ See at the end of the file.

\ **************************************************************
\ Tools

fendo_definitions

variable #rows   \ counter for the current table
variable #cells  \ counter for the current table

: (>tr<)  ( -- )
  \ New row in the current table.
  [<tr>]  1 #rows +!  #cells off
  ;
: >tr<  ( -- )
  \ New row in the current table; close the previous row if needed.
  #rows @ if  [</tr>]  then  (>tr<)
  ;
: close_pending_cell  ( -- )
  \ Close a pending table cell.
  header_cell? @ if  [</th>]  else  [</td>]  then
  ;
: ((|))  ( xt -- )
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)
  #cells @ if  close_pending_cell else  >tr<  then
  exhausted?
  if    drop #cells off
  else  execute  1 #cells +!  then
  ;
: actual_cell?  ( -- wf )
  \ The parsed cell markup ("|" or "|=") is the first markup parsed
  \ on the current line or there's an opened table?
  \ This check lets those signs to be used as content in other contexts.
  \ wf = is it an actual cell?
  table_started? @  first_on_the_line?  or
  ;
: (|)  ( xt -- )
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)
  table_started? @ 0= if  [<table>]  then  ((|))
  ;
: <table><caption>  ( -- )
  [<table>] [<caption>]
  ;

\ **************************************************************
\ Markup

markup_definitions

: |  ( -- )
  \ Markup used as separator in tables, images and links.
\ ." | rendered"  \ xxx informer
  actual_cell? if  ['] <td> (|)  else  s" |" content  then
  ;
: |=  ( -- )
  \ Mark a table header cell.
  actual_cell? if  ['] <th> (|)  else  s" |=" content  then
  ;
: =|=  ( -- )
  \ Open or close a table caption; must be the first markup of a table.
  #rows @ abort" The '=|=' markup must be the first one in a table"
  ['] <table><caption> ['] </caption> opened_[=|=]? markups
  ;


fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.table.fs compiled ) cr
