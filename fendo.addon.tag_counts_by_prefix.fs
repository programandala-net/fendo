.( fendo.addon.tag_counts_by_prefix.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several content lists addons.

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
\ Requirements

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.traverse_pids.fs

\ ==============================================================

package fendo.addon.tag_counts_by_prefix

variable prefix

: (((tag_counts_by_prefix))) { D: pid -- }
  pid prefix $@ string-prefix? 0= ?exit
  pid pid$>data>pid# dup draft? 
  if  drop  else  tags evaluate_tags  then ;
  \ Increase the number of pages whose page ID starts with the given prefix.

: ((tag_counts_by_prefix)) ( ca len -- f )
  (((tag_counts_by_prefix))) true ;
  \ Increase the number of pages whose page ID starts with the given prefix.
  \ ca len = page ID
  \ f = continue with the next element?

: (tag_counts_by_prefix) ( -- +n_1 ... +n_n n )
  tags_do_total  ['] ((tag_counts_by_prefix)) traverse_pids  ;

public

: tag_counts_by_prefix ( ca len -- +n_1 ... +n_n n )
  prefix $!  depth >r  (tag_counts_by_prefix)  depth r> - ;
  \ Return the total counts of all tags
  \ present in pages whose pids start with the given prefix.
  \ XXX TODO -- reset tags; increase them by prefix

end-package

.( fendo.addon.tag_counts_by_prefix.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-03-07: Start.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.

\ vim: filetype=gforth
