.( fendo.markup.fendo.list.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for lists.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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
\ To-do

\ Nested lists.

\ ==============================================================
\ Tools

fendo_definitions

variable bullet_list_items    \ counter
variable numbered_list_items  \ counter

: ((-)) ( a -- )
  dup @ if  [</li>]  then  [<li>]  1 swap +!  separate? off ;
  \ List element.
  \ a = counter variable

: (-) ( -- )
  bullet_list_items dup @ 0= if  [<ul>]  then  ((-)) ;
  \ Bullet list item.

: (#) ( -- )
  numbered_list_items dup @ 0= if  [<ol>]  then  ((-)) ;
  \ Numbered list item.

\ ==============================================================
\ Markup

markup_definitions

: - ( -- )
  first_on_the_line? if  (-)  else  s" -" content  then ;
  \ Bullet list item.

' - alias *  \ XXX TMP
: # ( -- )
  first_on_the_line? if  (#)  else  s" #" content  then ;
  \ Numbered list item.

fendo_definitions

.( fendo.markup.fendo.list.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
