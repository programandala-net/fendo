.( fendo.image.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo tools required to include images.

\ Last modified  202011282119.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018,2020 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Requirements {{{1

forth_definitions

require galope/jpeg.fs  \ JPEG tools
require galope/png.fs  \ PNG tools

\ ==============================================================
\ Tools to set the image size attributes {{{1

fendo_definitions

defer img-open
defer img-size
defer img-close

: set_jpeg_image ( -- )
  ['] jpeg-open is img-open
  ['] jpeg-size is img-size
  ['] jpeg-close is img-close ;

: set_png_image ( -- )
  ['] png-open is img-open
  ['] png-size is img-size
  ['] png-close is img-close ;

: jpeg-filename? ( ca len -- f )
  2dup s" .jpg" string-suffix? >r
  s" .jpeg" string-suffix? r> or ;

: png-filename? ( ca len -- f )
  s" .png" string-suffix? ;

: set_image_type ( ca len -- )
  2dup jpeg-filename? if  2drop set_jpeg_image exit  then
  png-filename? if  set_png_image  exit  then
  true abort" Unknown image file type." ;

: (set_image_size_attributes) ( ca len -- )
  2dup set_image_type img-open img-size
  n>str height=! n>str width=! img-close ;

: set_image_size_attributes ( -- )
  target_dir $@ src=@ s+ (set_image_size_attributes) ;


\ ==============================================================
\ img {{{1

: img ( ca1 len1 ca2 len2 -- )
  alt=!
  files_subdir $@ 2swap s+ 2dup src=!
  target_dir $@ 2swap s+ (set_image_size_attributes)
  [<img>] ;

  \ doc{
  \
  \ img ( ca1 len1 ca2 len2 -- )
  \
  \ Define an image with source file _ca1 len1_ and alt text _ca2
  \ len2_. ``img`` is a direct Fendo word, not a Fendo markup word.
  \ Therefore it must be used between `<[` and `]>`. See `{{` for an
  \ image markup.
  \
  \ Usage examples:

  \ ----
  \ <[ s" mypicture.jpg" img ]>
  \ <[ "there/another.jpg" "Optional alternative text here" img ]>
  \ <[ "over/there/this_one.jpg" "Alt text" title=" Fendo picture" img ]>
  \ <[ "over/there/this_one.jpg" "" title=" Fendo picture" img ]>
  \ ----

  \ - The size attributes of the images are added automatically.
  \ - Only JPEG and PNG images are supported.
  \
  \ See also: `{{`.
  \
  \ }doc

.( fendo.image.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2020-11-15: Start. Code extracted from
\ <fendo.markup.fendo.image.fs> (2013..2020).
\
\ 2020-11-28: Fix typo in documentation of `img`.

\ vim: filetype=gforth
