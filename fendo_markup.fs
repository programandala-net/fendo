.( fendo_html.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the markup.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-06-06 Start. This file is created with part of the old
\   <fendo_html_tags.fs>.

\ **************************************************************
\ Generic tool words for markup and parsing

: [fendo_markup_wid]  ( -- )
  fendo_markup_wid >order
  ;  immediate
variable header_cell?  \ flag, is it a header cell the latest opened cell in the table?
variable table_started?  \ flag, is a table open?

\ **************************************************************
\ Main

include fendo/fendo_markup_html.fs
include fendo/fendo_wiki_html.fs

.( fendo_markup.fs compiled) cr


