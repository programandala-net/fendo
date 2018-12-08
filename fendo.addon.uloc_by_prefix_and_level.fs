.( fendo.addon.uloc_by_prefix_and_level.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the addon that creates unnumbered content lists.

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

forth_definitions

require ./fendo.addon.lioc_by_prefix_and_level.fs

fendo_definitions

: uloc_by_prefix_and_level ( ca len n -- )
  [<ul>] lioc_by_prefix_and_level [</ul>] ;
  \ Create an unnumbered list of content
  \ with pages whose page ID starts with the given prefix and level.
  \ ca len = page ID prefix
  \ n = page hierarchical level (0 is the top)

.( fendo.addon.uloc_by_prefix_and_level.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-11-18: Created, based on <fendo.addon.uloc_by_prefix.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
