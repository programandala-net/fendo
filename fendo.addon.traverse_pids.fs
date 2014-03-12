.( fendo.addon.traverse_pids.fs) cr

\ This file is part of Fendo.

\ This file provides a word that traverses all pids, required by other addons.

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

\ 2013-11-25: Code extracted from
\ <addons/list_of_content_by_prefix.fs>.
\ 2013-11-26: Change: several words renamed, after a new uniform
\   notation: "pid$" and "pid#" for both types of page ids.
\ 2013-11-27: Change: all words and the addon itself renamed.
\ 2013-11-27: New: the list doesn't include file paths.
\ 2013-11-27: Change: '(pid$_list@)' factored out from 'pid$_list@'.
\ 2013-11-27: New: 'pid$_list@' rewritten: now it skips draft pages.
\ 2014-03-02: Everything renamed. Rewritten. Simplified. The pid list
\   is Forth source, not a simple list anymore.
\ 2014-03-03: Fix: removed a redundant definition.
\ 2014-03-11: Fix: '--key=2,2' added to 'sort$'.

\ **************************************************************

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/backslash-end-of-file.fs  \ '\eof'

module: fendo.addon.traverse_pids

s" /tmp/fendo.traverse_pids.fs" 2constant pids_file$
: ls$  ( -- ca len )
  s" ls -1 " source_dir $@ s+ s" *" s+ forth_extension $@ s+
  ;
: sed$  ( -- ca len )
  s" | sed -e 's#.\+/\(.\+\)" forth_extension $@ s+ s" $" s+
  s\" #s\" \\1\" traversed_pid#'" s+
  ;
: sort$  ( -- ca len )
  s" | sort --key 2,2"  \ sort only by the string content
  ;
: command$  ( -- ca len )
  ls$ sed$ s+ sort$ s+
  ;
: create_pids_file  ( -- )
  command$ s"  > " s+ pids_file$ s+ system
  ;
defer (traversed_pid)  ( ca len -- f )  \ user action for every pid
:noname  ( ca len -- true )
  \ Default user action for every pid.
  \ ca len = pid
  \ true = continue with the next element?
  2drop true
  ;
is (traversed_pid)

export

: traversed_pid  ( ca len -- )
  \ ca len = pid
  (traversed_pid) 0= if  \eof  then
  ;
: traverse_pids  ( xt -- )
  is (traversed_pid)  create_pids_file  
  \ pid_file$ included  \ xxx fixme this causes problems:
  \   *** glibc detected *** gforth: free(): invalid pointer: 0xb69fb7f0 ***
  \   Aborted.
  pids_file$ slurp-file evaluate  \ xxx this works
  ;

;module

.( fendo.addon.traverse_pids.fs compiled) cr

