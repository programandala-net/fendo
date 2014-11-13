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
\
\ 2014-11-08: Improved: 'hierarchy_meta_link' factored out from
\ 'hierarchy_meta_links'; 'unshortcut' added.
\
\ 2014-11-08: Fix: 'unshortcut' was missing.
\
\ 2014-11-08: Fix: second '2drop' was missing in
\ 'hierarchy_meta_link'.

\ **************************************************************

: (hierarchy_meta_link)  ( ca1 len1 ca2 len2 -- )
  \ Create a hierarchy meta link in the HTML header.
  \ ca1 len1 = page id
  \ ca2 len2 = rel
\  cr 2dup type ."  --> " 2over type  \ xxx informer
  rel=!
  ." Pid parameter in '(hierarchy_meta_link)' = " 2dup type cr  \ xxx informer
  pid$>data>pid#
  dup target_file href=!
  dup title unmarkup title=!
  ?hreflang=!
  [<link/>]
  ;
: hierarchy_meta_link  ( ca1 len1 ca2 len2 -- )
  \ Create a hierarchy meta link in the HTML header, if needed.
  \ ca1 len1 = rel
  \ ca2 len2 = page id
  ." Parameter in 'hierarchy_meta_link' = " 2dup type cr  \ xxx informer
  unshortcut dup if  2swap (hierarchy_meta_link)  else  2drop 2drop  then
  ;
: hierarchy_meta_links  ( -- )
  \ Create the required hierarchy meta links in the HTML header.
  s" up" current_page upper_page hierarchy_meta_link
  s" next" current_page next_page hierarchy_meta_link
  s" prev" current_page previous_page hierarchy_meta_link
  s" first" current_page first_page hierarchy_meta_link
  s" last" current_page last_page hierarchy_meta_link
  ;

.( fendo.addon.hierarchy_meta_links.fs compiled) cr
