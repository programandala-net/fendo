.( fendo.markup.fendo.image.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for images.

\ Last modified 20220905T1134+0200.
\ See change log at the end of the file.

\ Copyright (C) 2013, 2014, 2017, 2018, 2020, 2021, 2022 Marcos Cruz
\ (programandala.net)

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

\ XXX TMP Move to Galope:
[undefined] bl+ [if]
: bl+ ( ca len -- ca' len' ) s"  " s+ ;
[then]

require galope/default-of.fs       \ `default-of`

fendo_definitions

require fendo.image.fs

\ ==============================================================
\ Complex images markup code {{{1

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

: ({{) ( "ccc" -- )
  parse_image [<img>] ;

: (complex_{{) ( "ccc" -- )
  ({{) ;

  \ doc{
  \
  \ (complex_{{) ( "ccc" -- )
  \
  \ The default action of `{{`.
  \
  \ Start the definition of an image. Parse the input stream for an
  \ image markup until an ending `}}` is found.
  \
  \ Usage examples:

  \ ----
  \ {{ mypicture.jpg }}
  \ {{ there/another.jpg | Optional alternative text here }}
  \ {{ over/there/this_one.jpg | Alt text | title="Fendo picture" }}
  \ {{ over/there/this_one.jpg | | title="Picture without alt text" }}
  \ ----

  \ Note:
  \ - Spaces are not allowed in the filename.
  \ - The markup can span on several text lines, provided ``|``
  \   doesn't lay at the start of a line.
  \ - The size attributes of the images are added automatically.
  \ - Only JPEG and PNG images are supported.
  \
  \ See also: `}}`.
  \
  \ }doc

: (complex_}}) ( -- )
  true abort" `}}` without `{{`" ;

  \ doc{
  \
  \ (complex_}}) ( -- )
  \
  \ The default action of `}}`.
  \
  \ End the definition of an image that was started by the
  \ `(complex_{{)` action of `{{`.
  \
  \ }doc

\ ==============================================================
\ Simple images markup code {{{1

: parse_simple_image ( "ccc" -- ca len )
  s" "
  begin  parse-name dup
    if   2dup s" }}" str= >r s+ bl+ r>
    else 2drop bl+ refill 0= then
  until ;
  \ Get the content of a `{{` image. Parse the input stream "ccc" until
  \ a "}}" markup is found and return the parsed text (including "}}")
  \ in string _ca len_.

variable {{-depth
  \ Store the stack depth at the start of the image markup, in order to
  \ calculate later the number of arguments left.

: (simple_{{) ( -- )
  depth {{-depth ! parse_simple_image evaluate_markup ;

  \ doc{
  \
  \ (simple_{{) ( -- )
  \
  \ Start an image markup (simple version). ``(simple_{{)`` is a
  \ possible action of the actual markup `{{`, selected by
  \ `simple_{{`.
  \
  \ Usage examples:

  \ ----
  \ simple_{{ \ select the simple image markup
  \ {{ "mypicture.jpg" }}
  \ {{ "there/another.jpg" "Optional alternative text here" }}
  \ {{ "over/there/this_one.jpg" "Alt text" title=" Fendo picture" }}
  \ {{ "over/there/this_one.jpg" title=" Picture without alt text" }}
  \ complex_{{ \ return to the default format
  \ ----

  \ The text in the markup is evaluated as Forth code. The first
  \ string must be a page identifier, an actual URL or a shortcut. The
  \ second, optional, string must be the image text. HTML parameters
  \ can be set by the corresponding parsing words like ``title="``,
  \ ``style="``, etc., in any order or position. Also their storage
  \ variants like ``title=!`` are valid.
  \
  \ WARNING: The text "}}", delimited by spaces or end of lines,
  \ cannot be part of the alternative text or any attribute. Otherwise
  \ it would be mistaken for the ending `}}`.
  \
  \ See also: `(simple_}})`, `(complex_{{)`, `img`, `shortcut:`.
  \
  \ }doc

: (simple_}}) ( ca1 len1 | ca1 len1 ca2 len2 -- )
  depth {{-depth @ - case
    2       of s" " img                                               endof
    4       of      img                                               endof
    default-of true abort" Wrong number of arguments in image markup" endof
  endcase ;

  \
  \ doc{
  \
  \ (simple_}}) ( ca1 len1 | ca1 len1 ca2 len2 -- )
  \
  \ End an image markup (simple version) by calling `img` with the
  \ given paramenters. ``(simple_}})`` is a possible action of the
  \ actual markup `}}`, selected by `simple_{{`.
  \
  \ The string _ca1 len1_ is the address (an actual URL, a page
  \ identifier or a shortcut). The optional string _ca2 len2_ is the
  \ image text (if _ca2 len2_ is missing, an empty string is used
  \ instead and passed to `img`). HTML attributes are set by the
  \ corresponding Fendo words. See `(simple_{{)` for usage examples.
  \
  \ See also: ``(complex_}})`, `img`.
  \
  \ }doc

\ ==============================================================
\ Markup {{{1

markup_definitions

defer {{

defer }}

\ ==============================================================
\ Markup selectors {{{1

fendo_definitions

: complex_{{ ( -- )
  [markup>order]
  ['] (complex_{{) is {{
  ['] (complex_}}) is }}
  [markup<order] ;

  \ doc{
  \
  \ complex_{{ ( -- )
  \
  \ Select the old complex version of the image markups `{{` and `}}`,
  \ provided by `(complex_{{)` and `(complex_{{)`.
  \
  \ See also: `simple_{{`.
  \
  \ }doc

: simple_{{ ( -- )
  [markup>order]
  ['] (simple_{{) is {{
  ['] (simple_}}) is }}
  [markup<order] ;

  \ doc{
  \
  \ simple_{{ ( -- )
  \
  \ Select the new simple version of the link markups `{{` and `}}`,
  \ provided by `(simple_{{)` and `(simple_{{)`.
  \
  \ See also: `complex_{{`.
  \
  \ }doc

complex_{{ \ set the default

fendo_definitions

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
\
\ 2020-11-16: Make `{{` and `}}` deferred in order to prepare an
\ alternative simpler version.
\
\ 2020-11-23: Update documentation of `(complex_{{)` and
\ `(complex_}})`.
\
\ 2021-10-23: Replace "See:" with "See also:" in the documentation.
\
\ 2022-09-05: Add an alternative simple syntax markup, after the link
\ markup.

\ vim: filetype=gforth
