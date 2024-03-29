.( fendo.addon.lioc.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides a word that is needed by other addons.

\ Last modified 20211023T1637+0200.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017,2018,2019,2021 Marcos Cruz (programandala.net)

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

: (lioc) ( ca len -- )
  [<li>] link<pid$ [</li>] ;

  \ doc{
  \
  \ (lioc) ( ca len -- )
  \ 
  \ Create an element of a list of content for the page identified by
  \ page ID _ca len_. ``(lioc)`` is a factor of `(lioc)`.
  \
  \ }doc

: lioc ( ca len -- )
  2dup pid$>pid# unlistable? if 2drop else (lioc) then ;

  \ doc{
  \
  \ lioc ( ca len -- )
  \ 
  \ If page identified by page ID _ca len_ can be listed, i.e. its
  \ `properties` field does not contain "unlistable", create its
  \ corresponding element of a list of content.
  \
  \ See also: `unlistable?`, `(lioc)`.
  \
  \ }doc

: ?lioc ( ca len f -- )
  if  lioc  else  2drop  then ;
  \ Create an element of a list of content, if needed.
  \ ca len = page ID
  \
  \ XXX OLD Not used

.( fendo.addon.lioc.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2014-03-02: Factored out from <fendo.addon.lioc_by_regex.fs> and
\ <fendo.addon.lioc_by_prefix.fs>.
\
\ 2014-03-03: Change: `title_link` now is `link<pid$`, after the
\ changes in <fendo.tools.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2019-02-07: Improve `lioc`: check `unlistable?` and factor to
\ `(lioc)`. Improve documentation.
\
\ 2021-10-23: Replace "See:" with "See also:" in the documentation.

\ vim: filetype=gforth
