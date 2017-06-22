.( fendo.addon.shared_datum.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the shared datum addon.  It provides words to create
\ and manage datum fields shared by several pages.
\
\ XXX NOT FINISHED -- Even not started! -- This could be a general
\ layer below <fendo.addon.project.fs>.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2015,2017 Marcos Cruz (programandala.net)

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

: shared_datum: ( "name" -- )
  ;

.( fendo.addon.shared_datum.fs compiled) cr

\ ==============================================================
\ Change log

\ 2015-02-02 Start.
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
