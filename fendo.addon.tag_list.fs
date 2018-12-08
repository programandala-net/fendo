.( fendo.addon.tag_list.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides tag lists.

\ Last modified 201812080157.
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
\ Requirements

require ./fendo.addon.tags.fs

\ ==============================================================

: tag_list { pid -- }
  tags_do_count  pid tags evaluate_tags
  tags_do_list_link  #tag off pid tags evaluate_tags ;
  \ Create a tag list of links for the given page id.
  \ First, update the `#tags` variable.
  \ Then build the list (`#tag` must be reset, because it will be
  \ compared with `#tags` during the process).

: tag_ul ( pid -- )
  [<ul>] tag_list [</ul>] ;

: tag_ol ( pid -- )
  [<ol>] tag_list [</ol>] ;

.( fendo.addon.tag_list.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-03-04: Start.
\
\ 2014-11-05: Improvement: `tag_list` is modified after the
\ improvements in <fendo.addon.tags.fs> that let the last element of a
\ tag link list to be marked with a class.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
