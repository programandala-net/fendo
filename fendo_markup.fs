.( fendo_markup.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the markup. 

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-05-18 Start. First HTML tags.
\ 2013-06-01 Paragraphs, lists, headings, delete.

\ **************************************************************
\ Requirements


\ **************************************************************
\ Tool words

: markup  ( ca1 len1 ca2 len2 a -- )
  \ Open or close a HTML tag.
  \ This code is based on FML, a Forth-ish Markup Language for RetroWiki.
  \ ca1 len1 = opening HTML tag
  \ ca2 len2 = closing HTML tag
  \ a = markup flag variable: is the markup already open?
  dup >r @
  if    echo 2drop r> off   \ close it
  else  2drop echo r> on    \ open it
  then 
  ;
variable //?  \ is there an open '//'?
variable **?  \ is there an open '**'?
variable --?  \ is there an open '--'?
variable __?  \ is there an open '__'?
variable |?  \ is there an open '|'?
variable =?  \ is there an open heading?
variable #-  \ counter of unordered list elements
variable #+  \ counter of ordered list elements

: ((-))  ( a -- )
  \ List element.
  \ a = counter variable
  dup @ if  s" </li>" echo  then  s" <li>" echo  1 swap +!
  ;
: (-)  ( -- )
  \ Unordered list element.
  \ #- @ if  s" </li>" echo  then   s" <li>" echo  1 #- +!  \ xxx old
  #- ((-))
  ;
: (+)  ( -- )
  \ Ordered list element.
  \ #+ @ if  s" </li>" echo  then   s" <li>" echo  1 #+ +!  \ xxx old
  #+ ((-))
  ;

: }unordered_list  ( -- )
  \ Closes an unordered list.
  </li> </ul>  #- off
  ;
: }ordered_list  ( -- )
  \ Closes an ordered list.
  </li> </ol>  #+ off
  ;
: }list  ( -- )
  \ Closes a list.
  #- @ if  }unordered_list  else  }ordered_list  then
  ;

\ **************************************************************
\ Actual markup

[undefined] fendo_markup_voc [if]
  vocabulary fendo_markup_voc 
[then]
also fendo_markup_voc definitions

: //  ( -- )
  \ Start of finish a <em> region.
  s" <em>" s" </em>" //? markup
  ;
: **  ( -- )
  \ Start of finish a <strong> region.
  s" <bold>" s" </bold>" **? markup
  ;
: --  ( -- )
  \ Start of finish a <del> region.
  s" <del>" s" </del>" --? markup
  ;
: |  ( -- )
  \ Start of finish a <p> region.
  s" <p>" s" </p>" |? markup
  ;
: \\  ( -- )
  \ Line break.
  s" <br />" echo
  ;
: -  ( -- )
  \ Element of unordered list.
  #- @ 0= if  s" <ul>" echo  then  (-)
  ;
: +  ( -- )
  \ Element of unordered list.
  #+ @ 0= if  s" <ol>" echo  then  (+)
  ;
: }content  ( -- )
  \ Mark the end of the page content. 
  \ xxx todo 'do_content?' is useless when there are several content
  \ blocks in one page!
  do_content? on
  ;
: =  ( -- )
  \ Start of finish a <h1> region.
  s" <h1>" s" </h1>" =? markup
  ;
: ==  ( -- )
  \ Start of finish a <h2> region.
  s" <h2>" s" </h2>" =? markup
  ;
: ===  ( -- )
  \ Start of finish a <h3> region.
  s" <h3>" s" </h3>" =? markup
  ;
: ====  ( -- )
  \ Start of finish a <h4> region.
  s" <h4>" s" </h4>" =? markup
  ;
: =====  ( -- )
  \ Start of finish a <h5> region.
  s" <h5>" s" </h5>" =? markup
  ;
: ======  ( -- )
  \ Start of finish a <h6> region.
  s" <h6>" s" </h6>" =? markup
  ;

previous 
fendo_voc definitions

.( fendo_markup.fs compiled) cr
