.( fendo.addon.dloc_by_prefix_and_level.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several content lists addons.

\ Last modified  20220228T2157+0100.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017,2018,2022 Marcos Cruz (programandala.net)

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
\ Requirements {{{1

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.traverse_pids.fs
require ./fendo.addon.dtddoc.fs

\ ==============================================================
\ Code {{{1

package fendo.addon.dloc_by_prefix_and_level

variable prefix

variable level

: ((dloc_by_prefix_and_level))  { D: pid$ -- }
  pid$ prefix $@ string-prefix? 0= ?exit
  pid$ pid$>level level @ <>       ?exit
  pid$ pid$>pid# draft?            ?exit
  pid$ dtddoc ;
  \ Create a description list of content if the given page ID starts with
  \ the current prefix and has the current level.

: (dloc_by_prefix_and_level) ( ca len -- f )
  ((dloc_by_prefix_and_level)) true ;
  \ ca len = page ID
  \ f = continue with the next element?

public

: dloc_by_prefix_and_level ( ca len n -- )
  level ! prefix $!
  [<dl>] ['] (dloc_by_prefix_and_level) traverse_pids [</dl>] ;
  \ Create a description list of content with pages whose page ID has
  \ prefix _ca len_ and level _n_

end-package

.( fendo.addon.dloc_by_prefix_and_level.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2014-11-26: Adapted from <fendo.addon.dloc_by_prefix.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`. Improve local
\ variable name and documentation.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-17: Update: replace `pid$>data>pid#` with `pid$>pid#`.
\
\ 2022-02-28: Fix layout of `((dloc_by_prefix_and_level))`.

\ vim: filetype=gforth
