.( fendo.addon.hierarchy_navigation_links.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the hierarchy navigation links addon.

\ Last modified 201812080157.
\ See change log at the end of the file.

\ Copyright (C) 2017,2018 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================

require fendo.addon.hierarchy_meta_links.fs

defer first$ ( -- ca len )
defer previous$ ( -- ca len )
defer next$ ( -- ca len )
defer up$ ( -- ca len )
defer last$ ( -- ca len )
  \ Strings that must be defined by the application.  `l10n-string` or
  \ `noname-l10n-string` can be used for multilingual sites.

: (hierarchy_navigation_link)  ( ca1 len1 ca2 len2 -- )
  2swap s" <span class=`br`>" 2swap s+
  s" :</span> " s+  \ a trailing space is needed...
  2over pid$>pid# title s+  \ ..because the title could start with markups
  link ;
  \ ca1 len1 = link text
  \ ca2 len2 = href
  \ XXX TODO deactivate the <br/> for text browsers

: hierarchy_navigation_link  ( ca1 len1 ca2 len2 -- )
  [<li>] proper_hierarchical_link?
  if (hierarchy_navigation_link) else 2drop 2drop then [</li>] ;
  \ ca1 len1 = link text
  \ ca2 len2 = href

: hierarchy_first_navigation_link  ( -- )
  first$ current_page first_page hierarchy_navigation_link ;

: hierarchy_upper_navigation_link  ( -- )
  up$ current_page ?upper_page hierarchy_navigation_link ;

: hierarchy_previous_navigation_link  ( -- )
  previous$ current_page ?previous_page hierarchy_navigation_link ;

: hierarchy_next_navigation_link  ( -- )
  next$ current_page ?next_page hierarchy_navigation_link ;

: hierarchy_last_navigation_link ( -- )
  last$ current_page last_page hierarchy_navigation_link ;

0 [if]

  \ Usage example

: (hierarchy_navigation_links) ( -- )
  s" hierarchy" class=! [<ul>]
  hierarchy_previous_navigation_link
  hierarchy_upper_navigation_link
  hierarchy_next_navigation_link
  [</ul>] ;

: hierarchy_navigation_links ( -- )
  current_page pid#>hierarchy if (hierarchy_navigation_links) then ;

[then]

.( fendo.addon.hierarchy_navigation_links.fs compiled) cr

\ ==============================================================
\ Change log

\ 2017-06-25: Start. Moved from Fendo-programandala, in order to share
\ it with Fendo-VEB.
\
\ 2018-09-28: Replace `previous_page` with `?previous_page`.  Replace
\ `next_page` with `?next_page`.  Replace `upper_page` with
\ `?upper_page`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
