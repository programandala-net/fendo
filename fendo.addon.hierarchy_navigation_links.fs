.( fendo.addon.hierarchy_navigation_links.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the hierarchy navigation links addon.

\ Last modified 201812242037.
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

  \ doc{
  \
  \ first$ ( -- ca len )
  \
  \ A word defined by ``defer``. It must be configured by the
  \ application in order to return a string _ca len_ containing
  \ "First" in the language of the current page.
  \
  \ Usage example in a monolingual, English-only website:

  \ ----
  \ :noname s" First" ; is first$
  \ ----

  \ Usage example in a multilingual website:

  \ ----
  \ s" Primera" \ Spanish
  \ s" Unua"    \ Esperanto
  \ s" First"   \ English
  \ noname-l10n-string is first$
  \ ----

  \ See: `previous$`, `next$`, `up$`, `last$`, `noname-l10n-string`.
  \
  \ }doc

  \ doc{
  \
  \ previous$ ( -- ca len )
  \
  \ A word defined by ``defer``. It must be configured by the
  \ application in order to return a string _ca len_ containing
  \ "Previous" in the language of the current page.
  \
  \ See `first$` for a usage example.
  \
  \ See: `next$`, `up$`, `last$`.
  \
  \ }doc

  \ doc{
  \
  \ next$ ( -- ca len )
  \
  \ A word defined by ``defer``. It must be configured by the
  \ application in order to return a string _ca len_ containing "Next"
  \ in the language of the current page.
  \
  \ See `first$` for a usage example and related words.
  \
  \ See: `previous$`, `up$`, `last$`.
  \
  \ }doc

  \ doc{
  \
  \ up$ ( -- ca len )
  \
  \ A word defined by ``defer``. It must be configured by the
  \ application in order to return a string _ca len_ containing
  \ "Up" in the language of the current page.
  \
  \ See `first$` for a usage example and related words.
  \
  \ See: `previous$`, `next$`, `last$`.
  \
  \ }doc

  \ doc{
  \
  \ last$ ( -- ca len )
  \
  \ A word defined by ``defer``. It must be configured by the
  \ application in order to return a string _ca len_ containing
  \ "Last" in the language of the current page.
  \
  \ See `first$` for a usage example and related words.
  \
  \ See: `previous$`, `next$`, `up$`.
  \
  \ }doc

: hierarchy_navigation_link  ( ca1 len1 ca2 len2 -- )
  2swap s" <span class=`br`>" 2swap s+
  s" :</span> " s+
  2over pid$>pid# title s+
  link ;
  \ XXX TODO deactivate the <br/> for text browsers
  \
  \ NOTE: a trailing space is needed after `</span>` because the title
  \ could start with markups.

  \ doc{
  \
  \ hierarchy_navigation_link  ( ca1 len1 ca2 len2 -- )
  \
  \ Create a hierarchy navigation link with link text _ca1 len1_ and
  \ target page ID _ca2 len2_.
  \
  \ ``hierarchy_navigation_link`` is a factor of
  \ `?hierarchy_navigation_link`.
  \
  \ See `hierarchy_previous_navigation_link?` for a usage example.
  \
  \ See: `hierarchy_first_navigation_link?`,
  \ `hierarchy_previous_navigation_link?`,
  \ `hierarchy_next_navigation_link?`,
  \ `hierarchy_upper_navigation_link?`.
  \
  \ }doc
  \ `hierarchy_last_navigation_link?`, \ XXX TODO --

: ?hierarchy_navigation_link  ( ca1 len1 ca2 len2 -- )
  proper_hierarchical_link?
  if hierarchy_navigation_link else 2drop 2drop then ;

  \ doc{
  \
  \ ?hierarchy_navigation_link  ( ca1 len1 ca2 len2 -- )
  \
  \ String _ca1 len1_ is the link text of a hierarchy navigation link.
  \ String _ca2 len2_ is the target page ID of the link.  If _ca2
  \ len2_ is a proper hierarchical link, create the link.  Otherwise
  \ discard the parameters and do nothing.
  \
  \ See: `proper_hierarchical_link?`, `hierarchy_navigation_link`.
  \
  \ }doc

: hierarchy_first_navigation_link  ( -- )
  first$ current_page first_page ?hierarchy_navigation_link ;

  \ doc{
  \
  \ hierarchy_first_navigation_link  ( -- )
  \
  \ Prepare the the link text and the target page ID of the hierarchy
  \ "first" navigation link, and execute `?hierarchy_navigation_link`.
  \
  \ See `hierarchy_previous_navigation_link` for a usage example.
  \
  \ See: `hierarchy_first_navigation_link?`,
  \ `first$`, `current_page`, `first_page`,
  \ `hierarchy_previous_navigation_link`,
  \ `hierarchy_next_navigation_link`,
  \ `hierarchy_last_navigation_link`,
  \ `hierarchy_upper_navigation_link`.
  \
  \ }doc

: hierarchy_first_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  first$ current_page ?first_page proper_hierarchical_link? ;

  \ doc{
  \
  \ hierarchy_first_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  \
  \ If the current page has a first page in the hierarchy, _f_ is
  \ _true_, _ca1 len1_ is the link text returned by `first$` and _ca2
  \ len2_ is the target page ID, as needed by
  \ `hierarchy_navigation_link`.  Otherwise _f_ is _false_, and _ca1
  \ len1_ and _ca2 len2_ are unimportant.
  \
  \ See: `hierarchy_first_navigation_link`,
  \ `hierarchy_previous_navigation_link?`,
  \ `hierarchy_next_navigation_link?`,
  \ `hierarchy_upper_navigation_link?`.
  \
  \ }doc
  \ `hierarchy_last_navigation_link?`, \ XXX TODO --

: hierarchy_upper_navigation_link  ( -- )
  up$ current_page ?upper_page ?hierarchy_navigation_link ;

  \ doc{
  \
  \ hierarchy_upper_navigation_link  ( -- )
  \
  \ Prepare the the link text and the target page ID of the hierarchy
  \ "upper" navigation link, and execute `?hierarchy_navigation_link`.
  \
  \ See `hierarchy_previous_navigation_link` for a usage example.
  \
  \ See: `hierarchy_upper_navigation_link?`,
  \ `up$`, `current_page`, `?upper_page`,
  \ `hierarchy_first_navigation_link`,
  \ `hierarchy_previous_navigation_link`,
  \ `hierarchy_next_navigation_link`,
  \ `hierarchy_last_navigation_link`.
  \
  \ }doc

: hierarchy_upper_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  up$ current_page ?upper_page proper_hierarchical_link? ;

  \ doc{
  \
  \ hierarchy_upper_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  \
  \ If the current page has an upper page in the hierarchy, _f_ is
  \ _true_, _ca1 len1_ is the link text returned by `up$` and _ca2
  \ len2_ is the target page ID, as needed by
  \ `hierarchy_navigation_link`.  Otherwise _f_ is _false_, and _ca1
  \ len1_ and _ca2 len2_ are unimportant.
  \
  \ See: `hierarchy_upper_navigation_link`,
  \ `hierarchy_first_navigation_link?`,
  \ `hierarchy_previous_navigation_link?`,
  \ `hierarchy_next_navigation_link?`,
  \
  \ }doc
  \ `hierarchy_last_navigation_link?`. \ XXX TODO --

: hierarchy_previous_navigation_link  ( -- )
  previous$ current_page ?previous_page ?hierarchy_navigation_link ;

  \ doc{
  \
  \ hierarchy_previous_navigation_link  ( -- )
  \
  \ Prepare the the link text and the target page ID of the hierarchy
  \ "previous" navigation link, and execute `?hierarchy_navigation_link`.
  \
  \ Usage example in the code:

  \ ----
  \ : (hierarchy_navigation_links) ( -- )
  \   s" hierarchy" class=! [<ul>]
  \   [<li>] hierarchy_previous_navigation_link [</li>]
  \   [<li>] hierarchy_upper_navigation_link [</li>]
  \   [<li>] hierarchy_next_navigation_link [</li>]
  \   [</ul>] ;
  \
  \ : hierarchy_navigation_links ( -- )
  \   current_page pid#>hierarchy if (hierarchy_navigation_links) then ;
  \ ----

  \ Usage example in the HTML template:

  \ ----
  \ <footer>
  \ <nav> <[ hierarchy_navigation_links ]> </nav>
  \ </footer>
  \ ----

  \ See: `hierarchy_previous_navigation_link?`,
  \ `previous$`, `current_page`, `?previous_page`,
  \ `hierarchy_first_navigation_link`,
  \ `hierarchy_next_navigation_link`,
  \ `hierarchy_last_navigation_link`,
  \ `hierarchy_upper_navigation_link`.
  \
  \ }doc

: hierarchy_previous_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  previous$ current_page ?previous_page proper_hierarchical_link? ;

  \ doc{
  \
  \ hierarchy_previous_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  \
  \ If the current page has a previous page in the hierarchy, _f_ is
  \ _true_, _ca1 len1_ is the link text returned by `previous$` and
  \ _ca2 len2_ is the target page ID, as needed by
  \ `hierarchy_navigation_link`.  Otherwise _f_ is _false_, and _ca1
  \ len1_ and _ca2 len2_ are unimportant.
  \
  \ Usage example in the code:

  \ ----
  \ : (hierarchy_navigation_links) ( -- )
  \   s" hierarchy" class=! [<ul>]
  \   hierarchy_previous_navigation_link?
  \   if [<li>] hierarchy_navigation_link [</li>] then
  \   hierarchy_upper_navigation_link?
  \   if [<li>] hierarchy_navigation_link [</li>] then
  \   hierarchy_next_navigation_link?
  \   if [<li>] hierarchy_navigation_link [</li>] then
  \   [</ul>] ;
  \
  \ : hierarchy_navigation_links ( -- )
  \   current_page pid#>hierarchy if (hierarchy_navigation_links) then ;
  \ ----

  \ Usage example in the HTML template:

  \ ----
  \ <footer>
  \ <nav> <[ hierarchy_navigation_links ]> </nav>
  \ </footer>
  \ ----

  \ See: `hierarchy_previous_navigation_link`,
  \ `hierarchy_first_navigation_link?`,
  \ `hierarchy_next_navigation_link?`,
  \ `hierarchy_upper_navigation_link?`.
  \
  \ }doc
  \ `hierarchy_last_navigation_link?`, \ XXX TODO --

: hierarchy_next_navigation_link  ( -- )
  next$ current_page ?next_page ?hierarchy_navigation_link ;

  \ doc{
  \
  \ hierarchy_next_navigation_link  ( -- )
  \
  \ Prepare the the link text and the target page ID of the hierarchy
  \ "next" navigation link, and execute `?hierarchy_navigation_link`.
  \
  \ See `hierarchy_previous_navigation_link` for a usage example.
  \
  \ See: `hierarchy_next_navigation_link?`,
  \ `next$`, `current_page`, `?next_page`,
  \ `hierarchy_first_navigation_link`,
  \ `hierarchy_previous_navigation_link`,
  \ `hierarchy_last_navigation_link`,
  \ `hierarchy_upper_navigation_link`.
  \
  \ }doc

: hierarchy_next_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  next$ current_page ?next_page proper_hierarchical_link? ;

  \ doc{
  \
  \ hierarchy_next_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  \
  \ If the current page has a next page in the hierarchy, _f_ is
  \ _true_, _ca1 len1_ is the link text returned by `next$` and _ca2
  \ len2_ is the target page ID, as needed by
  \ `hierarchy_navigation_link`.  Otherwise _f_ is _false_, and _ca1
  \ len1_ and _ca2 len2_ are unimportant.
  \
  \ See: `hierarchy_next_navigation_link`,
  \ `hierarchy_first_navigation_link?`,
  \ `hierarchy_previous_navigation_link?`,
  \ `hierarchy_upper_navigation_link?`.
  \
  \ }doc
  \ `hierarchy_last_navigation_link?`, \ XXX TODO --

: hierarchy_last_navigation_link ( -- )
  last$ current_page last_page ?hierarchy_navigation_link ;

  \ doc{
  \
  \ hierarchy_last_navigation_link  ( -- )
  \
  \ Prepare the the link text and the target page ID of the hierarchy
  \ "last" navigation link, and execute `?hierarchy_navigation_link`.
  \
  \ See `hierarchy_previous_navigation_link` for a usage example.
  \
  \ See:
  \ `last$`, `current_page`, `last_page`,
  \ `hierarchy_first_navigation_link`,
  \ `hierarchy_previous_navigation_link`,
  \ `hierarchy_next_navigation_link`,
  \ `hierarchy_upper_navigation_link`.
  \
  \ }doc
  \ See: `hierarchy_last_navigation_link?`, \ XXX TODO --

\ XXX TODO -- 2018-12-24: `?last_page` is not written yet.
\ : hierarchy_last_navigation_link?  ( -- ca1 len1 ca2 len2 f )
\   last$ current_page ?last_page proper_hierarchical_link? ;

  \
  \ hierarchy_last_navigation_link?  ( -- ca1 len1 ca2 len2 f )
  \
  \ If the current page has a last page in the hierarchy, _f_ is
  \ _true_, _ca1 len1_ is the link text returned by `last$` and _ca2
  \ len2_ is the target page ID, as needed by
  \ `hierarchy_navigation_link`.  Otherwise _f_ is _false_, and _ca1
  \ len1_ and _ca2 len2_ are unimportant.
  \
  \ See: `hierarchy_last_navigation_link`,
  \ `hierarchy_first_navigation_link?`,
  \ `hierarchy_previous_navigation_link?`,
  \ `hierarchy_next_navigation_link?`,
  \ `hierarchy_upper_navigation_link?`.
  \

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
\
\ 2018-12-20: Improve documentation. Remove the list item markups from
\ `hierarchy_navigation_link`; create only the link.
\
\ 2018-12-23: Rename `hierarchy_navigation_link` to
\ `?hierarchy_navigation_link`; rename `(hierarchy_navigation_link)`
\ to `hierarchy_navigation_link`.
\
\ 2018-12-24: Finish improving and documenting the hierarchy
\ navigation links.

\ vim: filetype=gforth
