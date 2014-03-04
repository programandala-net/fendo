.( fendo.addon.dtddoc.fs) cr

\ This file is part of Fendo.

\ This file provides a word that is needed by other addons.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ 2013-11-26: Start.
\ 2014-03-02: Renamed.
\ 2014-03-03: Change: 'title_link' now is 'link<pid$', after the
\ changes in <fendo.tools.fs>.

\ **************************************************************

: dtddoc  ( ca len -- )
  \ Create an element of a definition list for the given pid.
  \ ca len = pid
\  ." dtddoc " 2dup type cr  \ xxx informer
  2dup [<dt>] link<pid$ [</dt>]
\  ." href= (0) " href=@ type cr  \ xxx informer
  [<dd>]
\  ." href= (1) " href=@ type cr  \ xxx informer
  pid$>data>pid#
\  ." href= (2) " href=@ type cr  \ xxx informer
  description
\  ." href= (3) " href=@ type cr  \ xxx informer
\  ." content to evaluate = " 2dup type cr key drop  \ xxx informer
  evaluate_content [</dd>]
  ;
: ?dtddoc  ( ca len f -- )
  \ Create an element of a definition list for the given pid, if needed.
  \ ca len = pid
  if  dtddoc  else  2drop  then
  ;

.( fendo.addon.dtddoc.fs compiled) cr
