.( fendo.addon.lioc.fs) cr

\ This file is part of Fendo.

\ This file provides a word that is needed by other addons.

\ Copyright (C) 2014 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2014-03-02: Factored out from <fendo.addon.lioc_by_regex.fs> and
\ <fendo.addon.lioc_by_prefix.fs>.
\ 2014-03-03: Change: 'title_link' now is 'link<pid$', after the
\ changes in <fendo.tools.fs>.

\ **************************************************************

: lioc  ( ca len -- )
  \ Create an element of a list of content.
  \ ca len = pid
  [<li>] link<pid$ [</li>]
  ;
: ?lioc  ( ca len f -- )
  \ Create an element of a list of content, if needed.
  \ ca len = pid
  if  lioc  else  2drop  then 
  ;

.( fendo.addon.lioc.fs compiled) cr

