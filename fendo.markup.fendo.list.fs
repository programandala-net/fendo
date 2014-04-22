.( fendo.markup.fendo.list.fs ) cr

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

variable bullet_list_items    \ counter
variable numbered_list_items  \ counter

: ((-))  ( a -- )
  \ List element.
  \ a = counter variable
  dup @ if  [</li>]  then  [<li>]  1 swap +!  separate? off
  ;
: (-)  ( -- )
  \ Bullet list item.
  bullet_list_items dup @ 0=
  if  [<ul>]  then  ((-))
  ;
: (#)  ( -- )
  \ Numbered list item.
  numbered_list_items dup @ 0=
  if  [<ol>]  then  ((-))
  ;

\ **************************************************************
\ Markup

markup_definitions

: -  ( -- )
  \ Bullet list item.
  first_on_the_line? if  (-)  else  s" -" content  then
  ;
' - alias *  \ XXX TMP
: #  ( -- )
  \ Numbered list item.
  first_on_the_line? if  (#)  else  s" #" content  then
  ;

fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.list.fs compiled ) cr
