.( fendo.addon.images_by_wildcard.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides a word that include images whose path matches a
\ shell wildcard.

\ Last modified  202011160218.
\ See change log at the end of the file.

\ Copyright (C) 2019 Marcos Cruz (programandala.net)

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

\ ==============================================================

package fendo.addon.images_by_wildcard

: list_file$ ( -- ca len ) s" /tmp/fendo.addon.images_by_wildcard.txt" ;

$variable alt_text

$variable attributes

80 constant /filename

/filename buffer: filename

target_dir $@len files_subdir $@len + constant /path

: make_list ( ca len -- )
  s" ls -1 "
  target_dir $@ s+
  files_subdir $@ s+ 2swap s+
  s"  > " s+ list_file$ s+ 2dup cr type system ;

: open_list ( ca len -- fid )
  make_list list_file$ r/o open-file throw  ;

: close_list ( fid -- ) close-file throw ;

: get_filename ( fid -- ca len f )
  filename dup /filename read-line throw >r /path /string r> ;

: alt+ ( ca len -- ca' len' )
  s"  | " s+ alt_text $@len if alt_text $@ s+ then ;

: attributes+ ( ca len -- ca' len' )
  s"  | " s+ attributes $@len if attributes $@ s+ then ;

: image_by_wildcard ( ca len -- )
  s" {{ " 2swap s+ alt+ attributes+ s"  }}" s+ evaluate_content ;

public

: images_by_wildcard ( ca1 len1 ca2 len2 ca3 len3 -- )
  attributes $!  alt_text $!  open_list ( fid )
  begin  dup get_filename ( fid ca len f )
  while  image_by_wildcard
  repeat 2drop close_list ;

  \ doc{
  \
  \ images_by_wildcard ( ca1 len1 ca2 len2 ca3 len3 -- )
  \
  \ Include images that match wildcard _ca2 len2_, using alt text
  \ _ca2 len2_ and atributtes _ca3 len3_ with all of them.
  \
  \ Usage example:

  \ ----
  \ <[ s" my-dir/here/2019*.png"
  \    s" This year"
  \    s\" title=\"Picture of this year\"" images_by_wildcard ]>
  \ ----
  \
  \ }doc

end-package

.( fendo.addon.images_by_wildcard.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2019-03-22: Start.

\ vim: filetype=gforth

