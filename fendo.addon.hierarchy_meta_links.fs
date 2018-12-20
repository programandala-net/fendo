.( fendo.addon.hierarchy_meta_links.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the hierarchy meta links addon.

\ Last modified 201812201724.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2015,2017,2018 Marcos Cruz (programandala.net)

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

: proper_hierarchical_link? ( ca len -- ca' len' f )
\   ." Stack before `unshortcut` in `proper_hierarchical_link? : " .s cr \ XXX INFORMER
\   ." Parameter of `proper_hierarchical_link? : " 2dup type cr \ XXX INFORMER
\   key drop \ XXX INFORMER
  unshortcut
\   ." Stack after `unshortcut` in `proper_hierarchical_link? : " .s cr key drop \ XXX INFORMER
  dup
  if 2dup pid$>draft?
\   ." Stack after `pid$>draft?` in `proper_hierarchical_link? : " .s cr \ XXX INFORMER

    0= else false then
\   ." Stack at the end of `proper_hierarchical_link? : " .s cr \ XXX INFORMER
\   key drop \ XXX INFORMER
    ;

  \ doc{
  \
  \ proper_hierarchical_link? ( ca len -- ca' len' f )
  \
  \ If page ID _ca len_ is a proper hierarchical link (i.e., not a
  \ draft page), _f_ is true and _ca' len'_ is its equivalent
  \ `unshortcut` page ID; otherwise _f_ is false and _ca' len'_ is
  \ unimportant.
  \
  \ This check is required in order to bypass the default behaviour of
  \ links: a link to a draft local page simply prints the link text,
  \ but that is not convenient for the hierarchy meta links.  This
  \ check can be used also by the application, as part of the user's
  \ hierarchy navigation bar.
  \
  \ ``proper_hierarchical_link?`` is used by `hierarchical_meta_link`.
  \
  \ See: `hierarchical_meta_links`, `pid$>draft?`.
  \
  \ }doc

: (hierarchy_meta_link) ( ca1 len1 ca2 len2 -- )
\  cr 2dup type ."  --> " 2over type  \ XXX INFORMER
  rel=!
\   ." Pid parameter in `(hierarchy_meta_link)` before `pid$>pid#` = " 2dup type cr  \ XXX INFORMER
\   ." `link_anchor` in `(hierarchy_meta_link)` before `pid$>pid#` = " link_anchor $@ type cr  \ XXX INFORMER
  pid$>pid#
\   ." `href=` in `(hierarchy_meta_link)` after `pid$>pid#` = " href=@ type cr  \ XXX INFORMER
\   ." `link_anchor` in `(hierarchy_meta_link)` after `pid$>pid#` = " link_anchor $@ type cr  \ XXX INFORMER
  dup target_file href=!
  dup title unmarkup title=!
  ?hreflang=!  s" text/html" type=!
  [<link/>] ;

  \ doc{
  \
  \ (hierarchy_meta_link) ( ca1 len1 ca2 len2 -- )
  \
  \ Create a hierarchy meta link to page ID _ca2 len2_,
  \ with ``rel`` attribute _ca1 len2_.
  \
  \ ``(hierarchy_meta_link)`` is a factor of `hierarchy_meta_link`.
  \
  \ See: `[<link/>]`.
  \
  \ }doc

: hierarchy_meta_link ( ca1 len1 ca2 len2 -- )
\   ." Stack in `hierarchy_meta_link` : " .s cr  \ XXX INFORMER
\   ." Parameter in `hierarchy_meta_link` = " 2dup type cr  \ XXX INFORMER
\   key drop \ XXX INFORMER
  proper_hierarchical_link?
\   ." Stack in `hierarchy_meta_link` after `proper_hierarchical_link?` : " .s cr  \ XXX INFORMER
\   key drop \ XXX INFORMER
  if  2swap (hierarchy_meta_link)  else  2drop 2drop  then ;

  \ doc{
  \
  \ hierarchy_meta_link ( ca1 len1 ca2 len2 -- )
  \
  \ If needed, create a hierarchy meta link to page ID _ca2 len2_,
  \ with ``rel`` attribute _ca1 len2_.
  \
  \ Usage example in the HTML template:

  \ ----
  \ <head>
  \ <title>Page 1</title>
  \ <[
  \   s" prev" s" page_0" hierarchy_meta_link
  \   s" next" s" page_2" hierarchy_meta_link
  \ ]>
  \ </head>
  \ ----

  \ ``hierarchy_meta_link`` is needed only in special cases.
  \ `hierarchy_meta_links` can be used instead to create all
  \ hierarchical links automatically.
  \
  \ See: `(hierarchy_meta_link)`.
  \
  \ }doc

: hierarchy_meta_links ( -- )
\  s" up" current_page upper_page hierarchy_meta_link
  s" next" current_page ?next_page hierarchy_meta_link
  s" prev" current_page ?previous_page hierarchy_meta_link
\  s" first" current_page first_page hierarchy_meta_link
\  s" last" current_page last_page hierarchy_meta_link
  ;

  \ XXX TMP 2015-02-26: it seems "up", "first" and "last" are not allowed
  \ in <link>; I comment them out.

  \ doc{
  \
  \ hierarchy_meta_links ( -- )
  \
  \ Create the hierarchy meta links in the HTML header.
  \
  \ NOTE: Only "next" and "prev" are created. "up", "first" and "last"
  \ are not allowed in HTML ``<link>`` tag.
  \
  \ Usage example in the HTML template:
  \
  \ ----
  \ <head>
  \ <[ hierarchy_meta_links ]>
  \ </head>
  \ ----
  \
  \ See: `hierarchy_meta_link`.
  \
  \ }doc

.( fendo.addon.hierarchy_meta_links.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-10-14 Moved from the application Fendo-programandala.
\
\ 2013-10-27 Change: `hierarchy_meta_link` simplified.
\
\ 2013-11-26 Change: several words renamed, after a new uniform
\ notation: "pid$" and "pid#" for both types of page IDs.
\
\ 2014-11-08: Change: `unmarkup` (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.
\
\ 2014-11-08: Improved: `hierarchy_meta_link` factored out from
\ `hierarchy_meta_links`; `unshortcut` added.
\
\ 2014-11-08: Fix: `unshortcut` was missing.
\
\ 2014-11-08: Fix: second `2drop` was missing in
\ `hierarchy_meta_link`.
\
\ 2015-01-17: New: `proper_hierarchical_link?`.
\
\ 2015-01-17: Fix: `hierarchy_meta_link` uses the new
\ `proper_hierarchical_link?` and does not create links to local draft
\ pages anymore.
\
\ 2015-02-26: Fix: It seems "up", "first" and "last" are not allowed
\ in <link>; I comment them out in the hierarchical meta links.
\
\ 2015-02-26: Fix: 's" text/html" type=!' was missing in the
\ hierarchical meta links.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-06-25: Factor `proper_hierarchical_link?` to `pid$>draft?`.
\
\ 2018-09-12: Add debugging code to explore the problem detected in
\ `(required_data)` (see change log of <fendo.data.fs>).
\
\ 2018-09-28: Replace `previous_page` with `?previous_page`.  Replace
\ `next_page` with `?next_page`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-17: Update: replace `pid$>data>pid#` with `pid$>pid#`.
\
\ 2018-12-20: Improve documentation.

\ vim: filetype=gforth
