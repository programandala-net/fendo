.( fendo.addon.pid_list.fs) cr

\ This file is part of Fendo.

\ This file provides a list of page ids, required by other addons.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ 2013-11-25 Code extracted from
\ <addons/list_of_content_by_prefix.fs>.
\ 2013-11-26 Change: several words renamed, after a new uniform
\   notation: "pid$" and "pid#" for both types of page ids.
\ 2013-11-27 Change: all words and the addon itself renamed.
\ 2013-11-27 New: the list doesn't include file paths.
\ 2013-11-27 Change: '(pid$_list@)' factored out from 'pid$_list@'.
\ 2013-11-27 New: 'pid$_list@' rewritten: now it skips draft pages.

\ **************************************************************

require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/slash-counted-string.fs  \ '/counted-string'

module: pid_list_fendo_addon_module

s" /tmp/pid_list_fendo_addon.txt" 2constant pid_list_file$
: ls_command$  ( -- ca len )
  s" ls -1 " source_dir $@ s+ s" *" s+ forth_extension $@ s+
  ;
: +ls_filter  ( ca1 len1 -- ca2 len2 )
  s" |sed -e 's#.\+/\(.\+\)\" s+ forth_extension $@ s+ s" #\1#'" s+
  ;
: +sort_filter  ( ca1 len1 -- ca2 len2 )
  s" |sort" s+
  ;
: pid_list_command$  ( -- ca len )
  ls_command$ +ls_filter +sort_filter
  ;
: create_pid_list  ( -- )
  pid_list_command$
\  2dup type cr  \ xxx informer
  s"  > " s+ pid_list_file$ s+ system
  ;
0 value pid_list_fid
/counted-string 2 + buffer: pid_list_element
: (pid$_list@)  ( -- ca len wf )
  \ Return the next page id from the list.
  \ ca len = page id
  \ wf = is the page id valid? (=is the list not empty?)
  pid_list_element dup /counted-string
  pid_list_fid read-line throw
  ;

export

: open_pid_list  ( -- )
  create_pid_list
  pid_list_file$ r/o open-file throw
  to pid_list_fid
  ;
: close_pid_list  ( -- )
  pid_list_fid close-file throw
  ;
: pid$_list@  ( -- ca len )
  \ Return the next page id from the list,
  \ or an empty string if the list is empty.
  0.  \ for the first 2drop
  begin   2drop (pid$_list@)
    if    2dup (pid$>data>pid#) draft? 0=
    else  true  then
  until
  ;

;module

.( fendo.addon.pid_list.fs compiled) cr

