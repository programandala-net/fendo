.( fendo.markup.fendo.image.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for images.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ **************************************************************
\ Change history of this file

\ See at the end of the file.

\ **************************************************************
\ Requirements

forth_definitions

require galope/jpeg.fs  \ JPEG tools
require galope/png.fs  \ PNG tools

\ **************************************************************
\ Tools

fendo_definitions

: get_image_src_attribute  ( "name" -- )
  \ Parse and store the image src attribute.
  files_subdir $@ parse-name s+ src=!
  ;
defer img-open
defer img-size
defer img-close
: set_jpeg_image  ( -- )
  ['] jpeg-open is img-open
  ['] jpeg-size is img-size
  ['] jpeg-close is img-close
  ;
: set_png_image  ( -- )
  ['] png-open is img-open
  ['] png-size is img-size
  ['] png-close is img-close
  ;
: jpeg-filename?  ( ca len -- wf )
  2dup s" .jpg" string-suffix? >r
  s" .jpeg" string-suffix? r> or
  ;
: png-filename?  ( ca len -- wf )
  s" .png" string-suffix?
  ;
: set_image_type  ( ca len -- )
  2dup jpeg-filename? if  2drop set_jpeg_image exit  then
  png-filename? if  set_png_image  exit  then 
  true abort" Unknown image file type."
  ;
: set_image_size_attributes  ( -- )
  target_dir $@ src=@ s+
  2dup set_image_type img-open img-size 
  n>str height=! n>str width=! img-close
  ;
variable image_finished?  \ flag, no more image markup to parse?
: end_of_image?  ( ca len -- wf )
  \ ca len = latest name parsed
  s" }}" str=  dup image_finished? !
  ;
: end_of_image_section?  ( ca len -- wf )
  \ ca len = latest name parsed
  2dup end_of_image? or_end_of_section?
  ;
: more_image?  ( -- wf )
  \ Fill the input buffer or abort.
  refill 0= dup abort" Missing '}}'"
  ;
: get_image_alt_attribute  ( "...<space>|<space>" | "...<space>}}<space>"  -- )
  \ Parse and store the image alt attribute.
  s" "
  begin   parse-name dup
    if    2dup end_of_image_section?  otherwise_concatenate
    else  2drop  more_image?  then
  until   alt=!
  ;
: get_image_raw_attributes  ( "...<space>}}<space>"  -- )
  \ Parse and store the image raw attributes.
  s" "
  begin   parse-name dup
    if    2dup end_of_image?  otherwise_concatenate
    else  2drop  more_image?  then
  until   ( ca len ) unraw_attributes
  ;
: parse_image  ( "imagemarkup}}" -- )
  \ Parse and store the image attributes.
\  cr ." parse_image " key drop  \ XXX INFORMER
  get_image_src_attribute
  set_image_size_attributes
  [ true ] [if]  \ simple version
    parse-name end_of_image_section? 0=
    abort" Space not allowed in image filename"
  [else]  \ xxx old, no abort
    begin  parse-name end_of_image_section? 0=
    while  s" <!-- xxx fixme space in image filename -->" echo
    repeat
  [then]
  image_finished? @ 0= if
    get_image_alt_attribute
    image_finished? @ 0= if  get_image_raw_attributes  then
  then
  ;
: ({{)  ( "imagemarkup}}" -- )
  parse_image [<img>]
  ;

\ **************************************************************
\ Markup

markup_definitions

: {{  ( "imagemarkup}}" -- )
  ({{)
  ;
: }}  ( -- )
  true abort" '}}' without '{{'"
  ;


fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.image.fs compiled ) cr

