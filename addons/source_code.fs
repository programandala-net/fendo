\ addons/source_code.fs 

\ This file is part of
\ Fendo-programandala.

\ This file is the source code addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

\ Fendo-programandala is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo-programandala is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ Fendo-programandala is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-07-21 Start, with noop definitions from
\   <fendo-programandala.fs>; only the basic 'source_code' works.
\ 2013-07-26 New: BASin and Forth blocks.
\ 2013-07-26 Fix: now source files are closed at the end.
\ 2013-07-28 Fix: 'basin_source_code' called 'echo_source_code'
\   instead of '(echo_source_code)'.
\ 2013-11-07 Change: Forth blocks are printed apart; empty blocks are
\   omited; no line number are printed.
\ 2013-11-08 Change: generic source code is not echoed by lines
\   anymore, as a first step towards syntax highlighting with Vim.

\ **************************************************************
\ Todo

\ 2013-07-26 Character set conversions.
\ 2013-11-07 Syntax highlighting, with Vim.

\ **************************************************************
\ Requirements

require string.fs  \ Gforth's dynamic strings

\ **************************************************************
\ Undefined source code

s" /counted-string" environment? 0=
[if]  255  [then]  dup constant /source_code_line
2 chars + buffer: source_code_line

variable source_code$

0 value source_code_fid

: open_source_code  ( ca len -- )
  \ ca len = file name
  2>r target_dir $@ files_subdir $@ s+ 2r> s+
\  2dup type cr  \ xxx informer
  r/o open-file throw  to source_code_fid
  ;
: close_source_code  ( -- )
  source_code_fid close-file throw
  ;
: read_source_code_line  ( -- ca len wf )
  source_code_line dup /source_code_line source_code_fid read-line throw
  ;
markup>order also forth
' <pre><code> alias source{
' </code></pre> alias }source 
previous markup<order
0 [if]  \ xxx old
: (read_and_echo_source_code)  ( -- )
  \ Read and echo the content of the opened source code file.
  begin  read_source_code_line  while  echo_line  repeat  2drop
  ;
: read_and_echo_source_code  ( -- )
  \ Read and echo the content of the opened source code file, surrounded by HTML markup.
  source{ (read_and_echo_source_code) }source
  ;
[then]

: echo_source_code  ( ca len -- )
  source{  echo  }source
  ;
: append_source_code_line  ( ca len -- )
  source_code$ $+!  s\" \n" source_code$ $+!
  ;
: slurp_source_code  ( -- ca len )
  \ Slurp the content of the opened source code file,
  \ from its current file position.
  0 source_code$ $!len
  begin   read_source_code_line
  while   append_source_code_line
  repeat  2drop
  source_code$ $@
  ;
: opened_source_code  ( -- )
  \ Read and echo the content of the opened source code file.
  slurp_source_code echo_source_code close_source_code 
  ;
: source_code  ( ca1 len1 ca2 len2 -- )
  \ Read and echo the content of a source code file.
  \ ca1 len1 = file name
  \ ca2 len2 = file type
  2drop  \ xxx todo
  open_source_code opened_source_code
  ;

\ **************************************************************
\ Forth source code in blocks

16 value /forth_block  \ lines
64 value /forth_block_line  \ characters
variable forth_block  \ counter

0 [if]

\ This first version shows all blocks together, with block titles and
\ line numbers, the way Forth blocks are usually printed.

variable forth_block_line  \ counter
: echo_forth_block  ( -- )
  s" <em>Screen # " echo forth_block @ echo. s" </em>" echo_line echo_cr
  1 forth_block +!
  ;
: forth_block_line++  ( -- )
  \ Increment the counter of Forth block lines.
  forth_block_line @  1+ dup /forth_block < abs *  forth_block_line !
  ;
: echo_forth_line_number  ( -- )
  s" <em>" echo forth_block_line @ s>d <# bl hold bl hold # #s #> echo s" </em>" echo
  ;
: (echo_forth_block_line)  ( ca len -- )
  echo_forth_line_number -trailing echo_line
  forth_block_line @ [ /forth_block 1- ] literal = if  echo_cr  then
  ;
: echo_forth_block_line  ( ca len -- )
  forth_block_line @ 0= if  echo_forth_block  then
  (echo_forth_block_line)  forth_block_line++
  ;
: read_forth_block_line  ( -- ca len )
  source_code_line dup /forth_block_line source_code_fid read-file throw
  ;
: (forth_blocks)  ( -- )
  source{
  0 forth_block !  0 forth_block_line ! 
  begin   read_forth_block_line dup
  while   echo_forth_block_line
  repeat  2drop 
  }source
  ;

[then]

0 [if]

\ This second version shows every block in a code zone; block titles
\ are outside the blocks (in the language of the current page); no
\ line numbers are added. This way Forth blocks will be easier to
\ higlight in the future.

\ 2013-11-07 

s" Bloque"
s" Bloko"
s" Block"
mlsconstant forth_block$
 
variable forth_block_line  \ counter
: echo_forth_block  ( -- )
  [<p>] forth_block$ echo forth_block @ _echo. [</p>] 
  1 forth_block +!
  ;
: forth_block_line++  ( -- )
  \ Increment the counter of Forth block lines.
  forth_block_line @  1+ dup /forth_block < abs *  forth_block_line !
  ;
: forth_block_line_0?  ( -- wf )
  forth_block_line @ 0= 
  ;
: echo_forth_block_line  ( ca len -- )
  forth_block_line_0? if  echo_forth_block source{  then
  -trailing echo_line  forth_block_line++
  forth_block_line_0? if  }source  then
  ;
: read_forth_block_line  ( -- ca len )
  source_code_line dup /forth_block_line source_code_fid read-file throw
  ;
: (forth_blocks)  ( -- )
  0 forth_block !  0 forth_block_line ! 
  begin   read_forth_block_line dup
  while   echo_forth_block_line
  repeat  2drop 
  ;

[then]

true [if]

\ This third version improves the second version:
\ Empty blocks are omited. 

\ 2013-11-07 

: >forth_block_line  ( n1 -- n2 )
  \ n1 = number of forth block line
  \ n2 = offset of forth block line in the buffer
  2 cells *
  ;
/forth_block >forth_block_line buffer: 'forth_block_line
: >'forth_block_line  ( n -- a )
  >forth_block_line 'forth_block_line +
  ;
: forth_block_line!  ( ca len n -- )
  >'forth_block_line 2!
  ;
: forth_block_line@  ( n -- ca len )
  >'forth_block_line 2@
  ;
s" Bloque"
s" Bloko"
s" Block"
mlsconstant forth_block$

variable forth_block_lenght 
variable forth_block_line  \ counter
: echo_forth_block_number  ( -- )
  [<p>] forth_block$ echo forth_block @ _echo. [</p>] 
  ;
: echo_forth_block_lines  ( -- )
  /forth_block 0 ?do
    i forth_block_line@ echo_line
  loop
  ;
: (echo_forth_block)  ( -- )
  echo_forth_block_number
  source{ echo_forth_block_lines }source
  ;
: echo_forth_block  ( -- )
  forth_block_lenght @ if  (echo_forth_block)  then
  1 forth_block +!
  0 forth_block_lenght !
  ;
: forth_block_line++  ( -- )
  \ Increment the counter of Forth block lines.
  forth_block_line @  1+ dup /forth_block < abs *  forth_block_line !
  ;
: save_forth_block_line  ( ca len -- )
  -trailing  dup forth_block_lenght +!
  >sb forth_block_line @ forth_block_line!
  forth_block_line++
  forth_block_line @ 0= if  echo_forth_block  then
  ;
: read_forth_block_line  ( -- ca len )
  source_code_line dup /forth_block_line source_code_fid read-file throw
  ;
: (forth_blocks)  ( -- )
  0 forth_block !
  0 forth_block_line !
  0 forth_block_lenght !
  begin   read_forth_block_line dup
  while   save_forth_block_line
  repeat  2drop 
  ;

[then]

: forth_blocks  ( ca1 len1 ca2 len2 -- )
  \ Read the content of a Forth blocks file and echo it.
  \ ca1 len1 = file name
  \ ca2 len2 = file type
  2drop  \ xxx todo
  open_source_code (forth_blocks) close_source_code
  ;
: skip_tap_header  ( -- )
  \ Code adapted from my tool "scr2txt" (2005-2012).
  24 s>d source_code_fid reposition-file throw
  ;
: abersoft_forth_blocks  ( ca len -- )
  \ Read the content of a ZX Spectrum's Abersoft Forth blocks TAP file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  open_source_code skip_tap_header (forth_blocks) close_source_code
  ;

\ **************************************************************
\ BASin source code

: not_basin_header?  ( ca len -- wf )
  -leading drop c@ chr-digit?
  ;
0 [if]  \ xxx old
2variable source_code_file_position
: save_source_code_position  ( -- )
  source_code_fid file-position throw source_code_file_position 2!
  ;
: restore_source_code_position  ( -- )
  source_code_file_position 2@ source_code_fid reposition-file throw
  ;
[then]
: skip_basin_header  ( -- )
  \ Read the opened source file and skip the lines of the BASin header.
  0.  \ fake file position, for the first 2drop
  begin
    2drop  \ old file position
    source_code_fid file-position throw 
    read_source_code_line >r
    not_basin_header? r> 0= or
  until  source_code_fid reposition-file throw
  ;
: basin_source_code  ( ca len -- )
  \ Read the content of a BASin file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  open_source_code skip_basin_header opened_source_code
  ;

