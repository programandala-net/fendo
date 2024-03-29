.( fendo.addon.traverse_pids.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides a word that traverses all pids (page IDs),
\ required by other addons.

\ Last modified  20220228T2215+0100.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018,2020,2022 Marcos Cruz (programandala.net)

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

require galope/backslash-end-of-file.fs \ `\eof`
require galope/package.fs \ `package`, `private`, `public`, `end-package`
require galope/s-constant.fs \ `sconstant`

fendo_definitions

package fendo.addon.traverse_pids

s" /tmp/fendo.traverse_pids.fs" sconstant pids_file$

: ls$ ( -- ca len )
  s" ls -1 " source_dir $@ s+ s" *" s+ forth_extension $@ s+ ;
  \ Return the shell command _ca len_ used to list the page source files.

: sed$ ( -- ca len )
  s" sed -e 's#.\+/\(.\+\)" forth_extension $@ s+ s" $" s+
  s\" #s\" \\1\" traversed_pid#'" s+ ;
  \ Return the shell command _ca len_ used to convert the list of
  \ page source files into a list of Forth commands to traverse
  \ their corresponding page IDs.

create reverse
  \ Flag used by `sort$` to force the reverse order, and set by
  \ `reversed`.

: sort$ ( -- ca len )
  s" sort --key 2,2"
  s"  --reverse" reverse @ and s+ reverse off ;
  \ Return the shell command _ca len_ used to sort the list of page IDs.
  \
  \ Option `--key 2,2` makes the second field the only sorting key, which is
  \ required in order to ignore rest of the line.  By default, `sort` separates
  \ the fields by non-blank to blank transition, which is fine in this case.

: command$ ( -- ca len )
  ls$ s" |" s+ sed$ s+ s" |" s+ sort$ s+ ;
  \ Return the shell command _ca len_ used to create the Forth source
  \ that traverses all of the page IDs.

: create_pids_file ( -- )
  command$ s"  > " s+ pids_file$ s+
\  2dup type cr \ XXX INFORMER
  system ;

defer (traversed_pid) ( ca len -- f )
  \ User action for every page ID.

:noname ( ca len -- true )
  2drop true ;
  \ Default user action for every page ID.
  \ ca len = page ID
  \ true = continue with the next element?

is (traversed_pid)

public

variable last_traversed_pid

: traversed_pid ( ca len -- )
\  ." Parameter in `traversed_pid` = " 2dup type cr  \ XXX INFORMER
  2dup last_traversed_pid $!
  (traversed_pid) 0= if \eof then ;
  \ ca len = page ID

: traverse_pids ( xt -- )
  is (traversed_pid)  create_pids_file
  pids_file$ included ;

: reversed ( -- )
  reverse on ;
  \ Force `traverse_pids` to process the PIDs in reversed order.
  \ `reversed` is used before words like `dloc_by_regex`.

end-package

.( fendo.addon.traverse_pids.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2013-11-25: Code extracted from
\ <addons/list_of_content_by_prefix.fs>.
\
\ 2013-11-26: Change: several words renamed, after a new uniform
\ notation: "pid$" and "pid#" for both types of page IDs.
\
\ 2013-11-27: Change: all words and the addon itself renamed.
\
\ 2013-11-27: New: the list doesn't include file paths.
\
\ 2013-11-27: Change: `(pid$_list@)` factored out from `pid$_list@`.
\
\ 2013-11-27: New: `pid$_list@` rewritten: now it skips draft pages.
\
\ 2014-03-02: Everything renamed. Rewritten. Simplified. The page ID
\ list is Forth source, not a simple list anymore.
\
\ 2014-03-03: Fix: removed a redundant definition.
\
\ 2014-03-11: Fix: `--key=2,2` added to `sort$`.
\
\ 2014-05-28: New: `last_traversed_pid`, required to improve the tag
\ cloud.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`. Simplify
\ `traverse_pids`.
\
\ 2018-09-28: Update source style. Improve documentation.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2020-04-14: Define strings constants with `sconstant` instead of
\ `2constant`.
\
\ 2022-02-28: Add `reversed` to control the order of the PIDs handled
\ by `traverse_pids`.

\ vim: filetype=gforth
