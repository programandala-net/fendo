.( fendo.markup.fendo.image.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for images.

\ Last modified  202011160218.
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

fendo_definitions

require fendo.image.fs

\ ==============================================================
\ Tools to parse the image markup {{{1

: get_image_src_attribute ( "name" -- )
  files_subdir $@ parse-name s+ src=! ;
  \ Parse and store the image src attribute.

variable image_finished?  \ flag, no more image markup to parse?

: end_of_image? ( ca len -- f )
  s" }}" str=  dup image_finished? ! ;
  \ ca len = latest name parsed

: end_of_image_section? ( ca len -- f )
  2dup end_of_image? or_end_of_section? ;
  \ ca len = latest name parsed

: more_image? ( -- f )
  refill 0= dup abort" Missing `}}`" ;
  \ Fill the input buffer or abort.

: get_image_alt_attribute ( "...<space>|<space>" | "...<space>}}<space>"  -- )
  s" "
  begin   parse-name dup
    if    2dup end_of_image_section?  otherwise_concatenate
    else  2drop  more_image?  then
  until   alt=! ;
  \ Parse and store the image alt attribute.

: get_image_raw_attributes ( "...<space>}}<space>"  -- )
  s" "
  begin   parse-name dup
    if    2dup end_of_image?  otherwise_concatenate
    else  2drop  more_image?  then
  until  ( ca len ) unraw_attributes ;
  \ Parse and store the image raw attributes.

: parse_image ( "imagemarkup }}" -- )
\  cr ." parse_image " key drop  \ XXX INFORMER
  get_image_src_attribute
  set_image_size_attributes
  parse-name end_of_image_section? 0=
  abort" Space not allowed in image filename"
  image_finished? @ 0= if
    get_image_alt_attribute
    image_finished? @ 0= if  get_image_raw_attributes  then
  then ;
  \ Parse and store the image attributes.

: ({{) ( "imagemarkup}}" -- )
  parse_image [<img>] ;

\ ==============================================================
\ Markup {{{1

markup_definitions

: {{ ( "imagemarkup}}" -- )
  ({{) ;

  \ doc{
  \
  \ {{ ( "ccc }}" -- )
  \
  \ Start the definition of an image.
  \
  \ Usage examples:

  \ ----
  \ {{ mypicture.jpg }}
  \ {{ there/another.jpg | Optional alternative text here }}
  \ {{ over/there/this_one.jpg | Alt text | title="Fendo picture" }}
  \ {{ over/there/this_one.jpg | | title="Picture without alt text" }}
  \ ----

  \ - Spaces are not allowed in the filename.
  \ - The size attributes of the images are added automatically.
  \ - Only JPEG and PNG images are supported.
  \
  \ See: `}}`.
  \
  \ }doc

: }} ( -- )
  true abort" `}}` without `{{`" ;

  \ doc{
  \
  \ }} ( -- )
  \
  \ End the definition of an image that was started by `{{`.
  \
  \ }doc

.( fendo.markup.fendo.image.fs compiled ) cr

\ ==============================================================
\ Change log {{{1

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-13: Document `{{` and `}}`.
\
\ 2018-12-20: Fix typo in documentation.
\
\ 2020-10-09: Fix typo in stack comment of `parse_image`.
\
\ 2020-11-14: Remove old unused code from `parse_image`. Add `img`.
\ Factor `set_image_size_attributes`. Move the general code to a new
\ file <fendo.image.fs>, keep here the markup-specific code.

\ vim: filetype=gforth
