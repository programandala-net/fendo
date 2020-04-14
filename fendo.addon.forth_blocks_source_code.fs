.( fendo.addon.forth_blocks_source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Forth blocks source code addon.

\ Last modified 202004141707.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017,2018,2020 Marcos Cruz (programandala.net)

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

require galope/package.fs \ `package`, `private`, `public`, `end-package`
require galope/s-constant.fs \ `sconstant`

fendo_definitions

require ./fendo.addon.source_code.fs

\ ==============================================================
\ Forth source code in blocks format

package fendo.addon.forth_blocks_source_code

64 value /forth_block_line  \ chars per line
variable forth_block  \ counter
variable forth_block_lenght
public
16 value /forth_block  \ lines per block
variable forth_block_line  \ counter
variable highlight_forth_block_0?  \ flag
defer forth_block$  \ "Block" in the current language
private
s" Block" sconstant (default_forth_block$)
' (default_forth_block$) is forth_block$
(*
\ `forth_block$` can be vectored by the application
\ to a multilingual string, e.g.:
  s" Block"   \ English
  s" Bloko"   \ Esperanto
  s" Bloque"  \ Spanish
  l18n$ l18n_forth_block$
  ' l18n_forth_block$ is forth_block$
*)
: echo_forth_block_number ( -- )
  [<p>] forth_block$ echo forth_block @ _echo. [</p>] ;

: reset_forth_block ( -- )
  0 forth_block_lenght !  new_source_code ;

: next_forth_block ( -- )
  1 forth_block +!  reset_forth_block ;

: update_forth_block_0_highlighting ( -- )
  forth_block @ 0=
  if  highlight_forth_block_0? @ highlight? and to highlight?  then ;
  \ Turn off highlighting for Forth block 0, if needed.

: (echo_forth_block) ( -- )
  echo_forth_block_number
  highlight?  \ save
  update_forth_block_0_highlighting
  source_code@ echo_source_code
  to highlight? ; \ restore

: echo_forth_block ( -- )
  forth_block_lenght @ if  (echo_forth_block)  then  next_forth_block ;

: forth_block_line++ ( -- n )
  forth_block_line @  1+ dup /forth_block < abs *  dup forth_block_line ! ;
  \ Increment the counter of Forth block lines.
  \ n = new line number

: read_forth_block_line ( -- ca len )
  source_line dup /forth_block_line source_code_fid read-file throw
\  2dup ." «" type ." »" cr  \ XXX INFORMER
  ;
  \ Note: `source_line` is a buffer defined in
  \ <fendo/fendo_markup_wiki.fs>.

public

: default_tidy_forth_block_line ( ca len -- )
  2drop ;

defer tidy_forth_block_line ( ca len -- )

private

' default_tidy_forth_block_line is tidy_forth_block_line

: save_forth_block_line ( ca len -- )
  2dup tidy_forth_block_line
  -trailing  dup forth_block_lenght +!
  append_source_code_line
  forth_block_line++ 0= if  echo_forth_block  then ;

public
: (forth_blocks_source_code) ( -- )
  s" forth" programming_language?!  \ set it if unset
  0 forth_block !
  0 forth_block_line !
  0 forth_block_lenght !
  new_source_code
  begin   read_forth_block_line dup
  while   save_forth_block_line
  repeat  2drop ;

: forth_blocks_source_code ( ca len -- )
  highlight_forth_block_0? on  \ default
  open_source_code (forth_blocks_source_code) close_source_code ;
  \ Read the content of a Forth blocks file and echo it.
  \ ca len = file name

end-package

.( fendo.addon.forth_blocks_source_code.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-09: Code extracted from <addons/source_code.fs>.
\
\ 2013-11-18: Change: `programming_language` renamed to
\   `programming_language!`, after the changes in the main code.
\
\ 2013-11-19: Fix: `(echo_forth_block)` and
\   `update_block_0_highlighting`  still used `higlight?` as a
\   variable, but it was converted to a value.
\
\ 2013-11-30: Fix: now `forth_block$` is defered; the application must
\   set it, depending on the languages used in the website.
\
\ 2013-12-10: Change: `ql_forth_blocks_source_code` moved to its own file
\   <addons/ql_forth_blocks_source_code.fs>.
\
\ 2013-12-10: Change: All Abersoft Forth code is moved to its own file
\   <addons/abersoft_forth_blocks_source_code.fs>.
\
\ 2014-02-05: Fix: `update_forth_block_0_highlighting` set `highlight?`
\   when it was unset!
\
\ 2014-03-12: Change: module renamed after the filename.
\
\ 2014-10-19: Improvement: the programming language is not set to
\   Forth if it's not empty; this allows other flavours of Forth to be
\   used, e.g. ZX Spectrum`s Abersoft Forth or QL`s SuperForth.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2020-04-14: Define strings constants with `sconstant` instead of
\ `2constant`.

\ vim: filetype=gforth
