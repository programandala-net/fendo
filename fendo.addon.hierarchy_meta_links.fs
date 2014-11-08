.( fendo.addon.hierarchy_meta_links.fs) cr

\ This file is part of Fendo.

\ This file is the hierarchy meta links addon.

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

\ 2013-10-14 Moved from the application Fendo-programandala.
\
\ 2013-10-27 Change: 'hierarchy_meta_link' simplified.
\
\ 2013-11-26 Change: several words renamed, after a new uniform
\ notation: "pid$" and "pid#" for both types of page ids.
\
\ 2014-11-08: Change: 'unmarkup' (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.

\ **************************************************************

: hierarchy_meta_link  ( ca1 len1 ca2 len2 -- )
  \ Create a hierarchy meta link in the HTML header.
  \ ca1 len1 = page id
  \ ca2 len2 = rel
\  cr 2dup type ."  --> " 2over type  \ xxx informer
  rel=!
\  2dup ."  --> " type  \ xxx informer
  pid$>data>pid#
  dup target_file href=!
  dup title unmarkup title=!
  ?hreflang=!
  [<link/>]
  ;

: hierarchy_meta_links  ( -- )
  \ Create the required hierarchy meta links in the HTML header.
  current_page upper_page dup if
    s" up" hierarchy_meta_link  else  2drop  then
  current_page next_page dup if
    s" next" hierarchy_meta_link  else  2drop  then
  current_page previous_page dup if
    s" prev" hierarchy_meta_link  else  2drop  then
  current_page first_page dup if
    s" first" hierarchy_meta_link  else  2drop  then
  current_page last_page dup if
    s" last" hierarchy_meta_link  else  2drop  then
  ;

.( fendo.addon.hierarchy_meta_links.fs compiled) cr
