.( fendo.markup.fendo.heading.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for headings.

\ Last modified 201812081823.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018 Marcos Cruz (programandala.net)

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
\ Tools

fendo_definitions

markup>order

: close_heading ( xt a -- )
  dup @ if  off execute  else  2drop  parsed$ $@ echo  then ;

: (=) ( -- )
  first_on_the_line? if
    ['] <h1> ['] </h1> opened_[=]? markups
  else  ['] </h1> opened_[=]? close_heading  then ;
  \ Open or close a <h1> heading.

: (==) ( -- )
  first_on_the_line? if
    ['] <h2> ['] </h2> opened_[==]? markups
  else  ['] </h2> opened_[==]? close_heading  then ;
  \ Open or close a <h2> heading.

: (===) ( -- )
  first_on_the_line? if
    ['] <h3> ['] </h3> opened_[===]? markups
  else  ['] </h3> opened_[===]? close_heading  then ;
  \ Open or close a <h3> heading.

: (====) ( -- )
  first_on_the_line? if
    ['] <h4> ['] </h4> opened_[====]? markups
  else  ['] </h4> opened_[====]? close_heading  then ;
  \ Open or close a <h4> heading.

: (=====) ( -- )
  first_on_the_line? if
    ['] <h5> ['] </h5> opened_[=====]? markups
  else  ['] </h5> opened_[=====]? close_heading  then ;
  \ Open or close a <h5> heading.

: (======) ( -- )
  first_on_the_line? if
    ['] <h6> ['] </h6> opened_[======]? markups
  else  ['] </h6> opened_[======]? close_heading  then ;
  \ Open or close a <h6> heading.

markup<order

\ ==============================================================
\ Markup

markup_definitions

: = ( -- )
  (=) ;
  \ Open or close a <h1> heading.

: == ( -- )
  (==) ;
  \ Open or close a <h2> heading.

: === ( -- )
  (===) ;
  \ Open or close a <h3> heading.

: ==== ( -- )
  (====) ;
  \ Open or close a <h4> heading.

: ===== ( -- )
  (=====) ;
  \ Open or close a <h5> heading.

: ====== ( -- )
  (======) ;
  \ Open or close a <h6> heading.

fendo_definitions

.( fendo.markup.fendo.heading.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
