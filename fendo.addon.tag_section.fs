( fendo.addon.tag_section.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides an addon to create a section containing a heading
\ and a list of tagged pages in the current language.

\ Last modified 20220123T1353+0100.
\ See change log at the end of the file.

\ Copyright (C) 2020,2021 Marcos Cruz (programandala.net)

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

\ forth_definitions

\ require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.multilingual.fs
require ./fendo.addon.tag_section_by_prefix.fs

\ ==============================================================
\ Code {{{1

: tag_section_heading ( ca len -- )
  2dup id=! [<h2>] tags_do_text evaluate_tags evaluate_content [</h2>] ;

  \ doc{
  \
  \ tag_section_heading ( ca len -- )
  \
  \ Create a level-2 heading for tag _ca len_.
  \
  \ ``tag_section_heading`` is a factor of `tag_section`.
  \
  \ See also: `tags_do_text`, `evaluate_tags`, `evaluate_content`, `id=!`,
  \ `[<h2>]`.
  \
  \ }doc

: tag_section ( ca len -- )
  2dup tag_section_heading
  current_lang$ s" ." s+ tag_section_by_prefix ;

  \ doc{
  \
  \ tag_section ( ca len -- )
  \
  \ Create a tag section containing a heading and a definition list of
  \ pages, in the current language, containing tag _ca len_.
  \
  \ See also: `tag_section_heading`, `current_lang$`,
  \ `tag_section_by_prefix`.
  \
  \ }doc

.( fendo.addon.tag_section.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2020-04-25: Start.
\
\ 2020-07-06: Document the public words.
\
\ 2021-10-23: Replace "See:" with "See also:" in the documentation.

\ vim: filetype=gforth
