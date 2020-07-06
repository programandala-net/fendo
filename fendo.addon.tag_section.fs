( fendo.addon.tag_section.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides an addon to create a section containing a heading
\ and a list of tagged pages in the current language.

\ Last modified 202004252319.
\ See change log at the end of the file.

\ Copyright (C) 2020 Marcos Cruz (programandala.net)

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
\ Requirements

\ forth_definitions

\ require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.multilingual.fs
require ./fendo.addon.tag_section_by_prefix.fs

\ ==============================================================

: tag_section_heading ( ca len -- )
  2dup id=! [<h2>] tags_do_text evaluate_tags evaluate_content [</h2>] ;

: tag_section ( ca len -- )
  2dup tag_section_heading
  current_lang$ s" ." s+ tag_section_by_prefix ;
  \ Create a tags section containing a heading and a list of pages, in
  \ the current language, containing tag _ca len_.

.( fendo.addon.tag_section.fs compiled) cr

\ ==============================================================
\ Change log

\ 2020-04-25: Start.

\ vim: filetype=gforth
