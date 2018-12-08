.( fendo.addon.dtddoc.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides a word that is needed by other addons.

\ Last modified 201812081823.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018 Marcos Cruz (programandala.net)

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

: dtddoc ( ca len -- )
\  ." >>>> Parameter in `dtddoc` = " 2dup type cr  \ XXX INFORMER
  [<dt>] 
\  ." `separate?` in `dtddoc` after `[<dt>]` = " separate? ? cr  \ XXX INFORMER
  2dup link<pid$ [</dt>]
\  ." href= (0) " href=@ type cr  \ XXX INFORMER
  [<dd>]
\  ." href= (1) " href=@ type cr  \ XXX INFORMER
  pid$>data>pid#
\  ." href= (2) " href=@ type cr  \ XXX INFORMER
  description
\  ." href= (3) " href=@ type cr  \ XXX INFORMER
\  ." content to evaluate = " 2dup type cr key drop  \ XXX INFORMER
  evaluate_content [</dd>] ;
  \ Create an element of a definition list for the given page ID.
  \ ca len = page ID

: ?dtddoc ( ca len f -- )
  if  dtddoc  else  2drop  then ;
  \ Create an element of a definition list for the given page ID, if needed.
  \ ca len = page ID

.( fendo.addon.dtddoc.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-26: Start.
\
\ 2014-03-02: Renamed.
\
\ 2014-03-03: Change: `title_link` now is `link<pid$`, after the
\ changes in <fendo.tools.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
