.( fendo.markup.fendo.table.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for lists.

\ Last modified  202011282114.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018,2019,2020 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Tools {{{1

fendo_definitions

variable #rows   \ counter for the current table
variable #cells  \ counter for the current table

: (new_table_row) ( -- )
  [<tr>]  1 #rows +!  #cells off ;
  \ New row in the current table.

: new_table_row ( -- )
  #rows @ if  [</tr>]  then  (new_table_row) ;
  \ New row in the current table; close the previous row if needed.

: close_pending_cell ( -- )
  header_cell? @ if  [</th>]  else  [</td>]  then ;
  \ Close a pending table cell.

: actual_cell? ( -- f )
  table_started? @  first_on_the_line?  or ;
  \ The parsed cell markup ("|" or "|=") is the first markup parsed
  \ on the current line or there's an opened table?
  \ This check lets those signs to be used as content in other contexts.
  \ f = is it an actual cell?

: (|) ( xt -- )
  #cells @            if    close_pending_cell    then
  first_on_the_line?  if    new_table_row         then
  exhausted?          if    drop #cells off  \ discard the last of the line
                      else  execute  1 #cells +!  then ;
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)

\ ==============================================================
\ Markup {{{1

markup_definitions

: | ( -- )
  table_started? @ if  ['] <td> (|)  else  s" |" content  then ;

  \ doc{
  \
  \ | ( -- )
  \
  \ Mark a table cell.
  \
  \ See `|===` far a usage example.
  \
  \ }doc

: |= ( -- )
  table_started? @ if  ['] <th> (|)  else  s" |=" content  then ;

  \ doc{
  \
  \ |= ( -- )
  \
  \ Mark a table header cell.
  \
  \ See `|===` far a usage example.
  \
  \ }doc

: =|= ( -- )
  table_started? @ if
    #rows @ abort" The `=|=` markup must be the first one in a table"
    ['] <caption> ['] </caption> opened_[=|=]? markups
  else  s" =|=" content  then ;

  \ doc{
  \
  \ =|= ( -- )
  \
  \ Open or close a table caption; it must be the first markup inside a table.
  \
  \ See `|===` far an usage example.
  \
  \ }doc

: |=== ( -- )
  table_started? @ if  [</table>]  else  [<table>]  then
  #rows off  #cells off ;

  \ doc{
  \
  \ |=== ( -- )
  \
  \ ``|===`` marks the start and end of a table.
  \
  \ The Fendo tables markup is similar to AsciiDoctor markup, but
  \ simpler.
  \
  \ `|` starts a cell, `|=` starts a header cell. `=|=` surrounds the
  \ caption and must be before any row.  Optional closing `|` is allowed
  \ and ignored at the end of the row.
  \
  \ Example:

  \ ....
  \ |===
  \ =|= Code: ## frames0 require decode frames@ d. ## =|=
  \ |= 512-byte buffers |= system frames
  \ | 1                 | 431
  \ | 2                 | 492
  \ | 3                 | 473
  \ | 4                 | 476
  \ | 10                | 491
  \ |===
  \ ....

  \ }doc

fendo_definitions

.( fendo.markup.fendo.table.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2015-08-15: Added `|===` after AsciiDoctor. Modified all the code
\ accordingly. Added the markup description and example.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-07: Fix typo.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2019-01-03: Fix typo in comment.
\
\ 2020-10-07: Improve documentation.
\
\ 2020-11-28: Fix markup in documentation of tables.

\ vim: filetype=gforth
