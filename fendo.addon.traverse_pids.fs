.( fendo.addon.traverse_pids.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides a word that traverses all pids (page ids),
\ required by other addons.

\ Last modified 201809271801.
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

require galope/backslash-end-of-file.fs  \ '\eof'
require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

package fendo.addon.traverse_pids

s" /tmp/fendo.traverse_pids.fs" 2constant pids_file$
: ls$ ( -- ca len )
  s" ls -1 " source_dir $@ s+ s" *" s+ forth_extension $@ s+ ;

: sed$ ( -- ca len )
  s" | sed -e 's#.\+/\(.\+\)" forth_extension $@ s+ s" $" s+
  s\" #s\" \\1\" traversed_pid#'" s+ ;

: sort$ ( -- ca len )
  s" | sort --key 2,2" ;
  \ Sort only by the string content.

: command$ ( -- ca len )
  ls$ sed$ s+ sort$ s+ ;

: create_pids_file ( -- )
  command$ s"  > " s+ pids_file$ s+ system ;

defer (traversed_pid) ( ca len -- f )
  \ User action for every pid.

:noname ( ca len -- true )
  2drop true ;
  \ Default user action for every pid.
  \ ca len = pid
  \ true = continue with the next element?

is (traversed_pid)

public

variable last_traversed_pid

: traversed_pid ( ca len -- )
\  ." Parameter in 'traversed_pid' = " 2dup type cr  \ XXX INFORMER
  2dup last_traversed_pid $!
  (traversed_pid) 0= if  \eof  then ;
  \ ca len = pid

: traverse_pids ( xt -- )
  is (traversed_pid)  create_pids_file
  pids_file$ included ;

end-package

.( fendo.addon.traverse_pids.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-25: Code extracted from
\ <addons/list_of_content_by_prefix.fs>.
\
\ 2013-11-26: Change: several words renamed, after a new uniform
\ notation: "pid$" and "pid#" for both types of page ids.
\
\ 2013-11-27: Change: all words and the addon itself renamed.
\
\ 2013-11-27: New: the list doesn't include file paths.
\
\ 2013-11-27: Change: '(pid$_list@)' factored out from 'pid$_list@'.
\
\ 2013-11-27: New: 'pid$_list@' rewritten: now it skips draft pages.
\
\ 2014-03-02: Everything renamed. Rewritten. Simplified. The pid list
\ is Forth source, not a simple list anymore.
\
\ 2014-03-03: Fix: removed a redundant definition.
\
\ 2014-03-11: Fix: '--key=2,2' added to 'sort$'.
\
\ 2014-05-28: New: 'last_traversed_pid', required to improve the tag
\ cloud.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`. Simplify `traverse_pids`.

\ vim: filetype=gforth
