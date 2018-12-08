.( fendo.addon.zx_spectrum_source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the ZX Spectrum source code addon.

\ Last modified 201812080157.
\ See change log at the end of the file.

\ Copyright (C) 2014,2015,2017 Marcos Cruz (programandala.net)

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

require ./fendo.addon.source_code.fs
require ./fendo.addon.zx_spectrum_charset.fs

\ ==============================================================

: zx_spectrum_source_code ( ca len -- )
  set_zx_spectrum_source_code_translation source_code ;

: zx_spectrum_unexpanded_llist ( ca len -- )
  set_zx_spectrum_unexpanded_llist_translation source_code ;

.( fendo.addon.zx_spectrum_source_code.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-10-17: Start, with part of the file. <fendo.addon.source_code.fs>.
\
\ 2015-01-31: New: `zx_spectrum_unexpanded_llist`.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
