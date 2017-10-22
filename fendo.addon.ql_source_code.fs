.( fendo.addon.ql_source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Sinclair QL source code addon.

\ Last modified 201709182037.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017 Marcos Cruz (programandala.net)

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

forth_definitions

require galope/uncodepaged.fs

fendo_definitions

require ./fendo.addon.source_code.fs
require ./fendo.addon.ql_charset.fs

\ ==============================================================

: ql_source_code_translated ( ca len -- ca' len' )
  ql_charset_to_utf8 uncodepaged ;
  \ Convert the content of a QL file to UTF-8.

: ql_source_code ( ca len -- )
  ['] ql_source_code_translated is source_code_pretranslated
  source_code ;
  \ Read and echo the content of a Sinclair QL source code file _ca
  \ len_.  The Vim filetype is guessed from the filename.

.( fendo.addon.ql_source_code.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-12-10 Written with <galope/translated.fs>.
\ 2013-12-11 New: 'ql_source_code_translated'.
\ 2013-12-11 Change: an xt is used, not a translation table; this
\   makes it possible to use different translation tools.
\ 2013-12-12 Rewritten with <galope/uncodepaged.fs>.
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\ 2017-06-22: Update source style, layout and header.
\ 2017-09-18: Improve documentation.

\ vim: filetype=gforth
