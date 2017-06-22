.( fendo.markup.html.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the HTML tags.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017 Marcos Cruz (programandala.net)

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
\ Modules

require ./fendo.markup.html.attributes.fs
require ./fendo.markup.html.tags.fs
require ./fendo.markup.html.entities.fs

.( fendo.markup.html.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-06-01: Start.
\ 2013-06-02: More tags. Start of the attribute management.
\ 2013-06-04: Fix: 'count' was missing for string variables.
\ 2013-06-06: Improvement: simpler attribute definition: one
\   defining word creates all required words, and the attributes'
\   xt are stored in a table in order to manage all of them in a
\   loop (e.g. for initialization or printing).
\ 2013-06-06: Change: HTML entities moved here from <fendo_markup.fs>.
\ 2013-06-06: New: many new HTML tags and attributes.
\ 2013-06-06: Change: renamed from "fendo_html_tags.fs" to
\   "fendo_markup_html.fs"; the generic words are moved to the
\   new file <fendo_markup.fs>.
\ 2013-06-10: Change: File divided in three:
\   attributes moved to <fendo_markup_html_attributes.fs>
\   tags moved to <fendo_markup_html_tags.fs>
\   entities moved to <fendo_markup_html_entities.fs>
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
