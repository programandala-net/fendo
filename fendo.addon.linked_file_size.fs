.( fendo.addon.linked_file_size.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file adds the file size to all file links.

\ Last modified 201903212107.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017,2019 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================

1024 constant KiB

KiB dup * constant MiB

: >linked_file_size  ( n -- ca len )
  s>d <# # # '.' hold #s #> ;

: echo_linked_file_size_MiB  ( n -- )
  100 * MiB / >linked_file_size echo s"  MiB" _echo ;

: echo_linked_file_size_KiB  ( n -- )
  100 * KiB / >linked_file_size echo s"  KiB" _echo ;

: echo_linked_file_size_B  ( n -- )
  echo. s" B" _echo ;

: echo_linked_file_size  ( n -- )
  dup MiB >= if  echo_linked_file_size_MiB exit  then
  dup KiB >= if  echo_linked_file_size_KiB exit  then
  echo_linked_file_size_B ;

: (linked_file_size)  ( -- )
  s"  (" _echo
  last_href$ $@ -file:// file>local
  slurp-file nip echo_linked_file_size
  s" )" echo ;

: linked_file_size  ( -- )
  file_link? if (linked_file_size) then ;

' linked_file_size is link_text_suffix
  \ Set the hook.


  \ doc{
  \
  \ linked_file_size  ( -- )
  \
  \ An alternative action for `link_text_suffix`. It adds the file
  \ size to file links.
  \
  \ Usage example (note this is already done when the addon of
  \ ``linked_file_size`` is loaded):

  \ ----
  \ ' linked_file_size is link_text_suffix
  \ ----

  \ }doc

.( fendo.addon.linked_file_size.fs compiled) cr

\ ==============================================================
\ Change log

\ 2019-03-21: Move the code from the application Fendo-programandala
\ (which builds http://programandala.net), in order to reuse it in
\ other sites. Update source style. Document.

\ vim: filetype=gforth

