.( fendo.addon.lioc.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides a word that is needed by other addons.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017 Marcos Cruz (programandala.net)

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

: lioc ( ca len -- )
  [<li>] link<pid$ [</li>] ;
  \ Create an element of a list of content.
  \ ca len = pid

: ?lioc ( ca len f -- )
  if  lioc  else  2drop  then ;
  \ Create an element of a list of content, if needed.
  \ ca len = pid

.( fendo.addon.lioc.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-03-02: Factored out from <fendo.addon.lioc_by_regex.fs> and
\ <fendo.addon.lioc_by_prefix.fs>.
\ 2014-03-03: Change: 'title_link' now is 'link<pid$', after the
\ changes in <fendo.tools.fs>.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
