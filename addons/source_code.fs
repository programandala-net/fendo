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
\ 2013-11-08 Change: source code is not echoed by lines
\   anymore; this is a first step towards syntax highlighting.
\ 2013-11-08 Fix: The rubbish byte at the end of the last block of an
\   Abersoft Forth blocks file is removed.

\ **************************************************************
\ Todo

\ 2013-07-26 Character set conversions.
\ 2013-11-07 Syntax highlighting, with Vim.

\ **************************************************************
\ Requirements

require string.fs  \ Gforth's dynamic strings
require galope/module.fs
require galope/minus-leading.fs  \ '-leading'
require galope/sourcepath.fs  \ 'sourcepath'
require galope/string-suffix-question.fs  \ 'string-suffix?'
require ffl/chr.fs  \ 'chr-digit'

module: source_code_fendo-programandala_addon_module

\ **************************************************************
\ Syntax highlighting with Vim

\ xxx fixme remove spaces and use 's&' instead of 's+'.

s" /tmp/fendo-programandala.addon.source_code.txt" 2dup 2constant input_file$
s" .xhtml" s+ 2constant output_file$
s" vim -f " 2constant base_highlight_command$
sourcepath s" source_code.vim " s+ 2constant vim_program$

export
variable filetype$  \ same values than Vim's 'filetype' 
hide

: program+  ( ca len -- ca' len' )
  \ Add the Vim program parameter to the Vim invocation.
  s" -S " s+ vim_program$ s+
  ;
: syntax+  ( ca len -- ca' len' )
  \ Add the desired syntax parameter to the Vim invocation.
  \ This parameter must be the first one in the command line.
  s\" -c \"set filetype=basin\" " s+
  ;
: file+  ( ca len -- ca' len' )
  \ Add the input file parameter to the Vim invocation.
  input_file$ s+
  ;
: highlighting_command$  ( -- ca len )
  \ Return the complete highlighting command,
  \ ready to be executed by the shell.
  base_highlight_command$ syntax+ program+ file+
  ;
: >input_file  ( ca len -- )
  input_file$ w/o create-file throw
  dup >r write-file throw
  r> close-file throw
  ;
: <output_file  ( -- ca len )
  output_file$ slurp-file
  ;
: (highlighted)  ( ca1 len1 -- ca2 len2 )
  \ Highlight the given source code.
  >input_file
  highlighting_command$ system
  $? abort" The highlighting command failed"
  <output_file
  ;
variable highlighting?  highlighting? on
: highlighted  ( ca1 len1 -- ca1 len1 | ca2 len2 )
  \ Highlight the given source code, if needed.
  highlighting? @ if  (highlighted)  then
  ;
: filename>filetype  ( ca1 len1 -- ca2 len2 )
  \ Convert a filename to a Vim filetype.
	2dup s" .prg" string-suffix? if  s" clipper" exit  then
  2dup s" .asm" string-suffix? if s" z80" exit  then
  2dup s" .bac" string-suffix? if s" bacon" exit  then
  2dup s" .bb" string-suffix? if s" beta_basic" exit  then
  2dup s" .bbim" string-suffix? if s" bbim" exit  then
  2dup s" .fs" string-suffix? if s" forth" exit  then
  2dup s" .mb" string-suffix? if s" masterbasic" exit  then
  2dup s" .mbim" string-suffix? if s" mbim" exit  then
  2dup s" .opl" string-suffix? if s" oplplus" exit  then
  2dup s" .opl.txt" string-suffix? if s" oplplus" exit  then
  2dup s" .opp" string-suffix? if s" oplplus" exit  then
  2dup s" .sbim" string-suffix? if s" sbim" exit  then
  2dup s" .sdlbas" string-suffix? if s" sdlbasic" exit  then
  2dup s" .xbas" string-suffix? if s" x11basic" exit  then
  2dup s" .yab" string-suffix? if s" yabasic" exit  then
  2dup s" _bas" string-suffix? if  s" superbasic" exit  then
  2dup s" boot" str=  if  s" superbasic" exit  then
  2drop
  true abort" Unknown source code file type"
  ;

\ **************************************************************
\ File encodings

\ xxx todo

0 [if]

UTF-8 and Latin1 file encodings are managed by Vim during conversion
to XHTML, but special encodings (e.g. BASin markups, Sinclar BASIC
tokens and QL charset) require special conversions.

[then]

variable fileencoding  \ 0 if no special conversion is needed
1 enum basin_fileenconding
  enum sinclair_basic_fileencoding
  enum ql_fileencoding
drop

\ **************************************************************
\ Generic source code

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
: echo_source_code  ( ca len -- )
  source{  highlighted echo  }source
  ;
: append_source_code_line  ( ca len -- )
  source_code$ $+!  s\" \n" source_code$ $+!
  ;
: slurp_source_code  ( -- ca len )
  \ Slurp the content of the opened source code file,
  \ from its current file position.
  \ ca len = source code read from the current file
  0 source_code$ $!len
  begin   read_source_code_line
  while   append_source_code_line
  repeat  2drop  source_code$ $@
  ;
: (opened_source_code)  ( -- )
  \ Read and echo the content of the opened source code file.
  slurp_source_code echo_source_code close_source_code 
  ;
export

: (source_code)  ( ca len -- )
  \ Read and echo the content of a source code file.
  \ ca len = file name
  open_source_code (opened_source_code)
  ;
: source_code  ( ca len -- )
  \ Read and echo the content of a source code file.
  \ The Vim filetype is guessed from the filename.
  \ ca len = file name
  2dup filename>filetype filetype$ $!  (source_code)
  ;

hide

\ **************************************************************
\ Forth source code in blocks

16 value /forth_block  \ lines per block
64 value /forth_block_line  \ chars per line
variable forth_block  \ counter
variable forth_block_line  \ counter
variable forth_block_lenght 
variable abersoft_forth?  \ flag
variable highlight_block_0?  \ flag
s" Bloque" s" Bloko" s" Block" mlsconstant forth_block$
: echo_forth_block_number  ( -- )
  [<p>] forth_block$ echo forth_block @ _echo. [</p>] 
  ;
: reset_forth_block  ( -- )
  0 forth_block_lenght !  0 source_code$ $!len
  ;
: next_forth_block  ( -- )
  1 forth_block +!  reset_forth_block 
  ;
: update_block_highlighting  ( -- )
  forth_block @ 0=
  abersoft_forth? @
  highlight_block_0? @ 0=  or and
  if  highlighting? off  then
  ;
: (echo_forth_block)  ( -- )
  echo_forth_block_number
  highlighting? @  update_block_highlighting
  source_code$ $@ echo_source_code 
  highlighting? !
  ;
: echo_forth_block  ( -- )
  forth_block_lenght @ if  (echo_forth_block)  then  next_forth_block
  ;
: forth_block_line++  ( -- n )
  \ Increment the counter of Forth block lines.
  \ n = new line number
  forth_block_line @  1+ dup /forth_block < abs *  dup forth_block_line !
  ;
: only_one_final_char?  ( ca len -- wf )
  -leading nip 1 =
  ;
: (tidy_line)  ( ca len -- )
  \ Override the last byte of the string with a space, if needed.
  \ It's not clear if that byte is always corrupted,
  \ or, as supposed here, only when the rest of the line is blank.
  \ ca len = Forth block line, last one of the current block
  2dup only_one_final_char? if  + 1- bl swap c!  else  2drop  then
  ;
: tidy_line  ( ca len -- )
  \ Remove the last byte of the last line of the last Forth block.
  \ This is needed with Abersoft Forth's TAP files.
  \ It seems a bug of Abersoft Forth, that corrupts that memory
  \ position before saving the blocks to tape.
  \ ca len = Forth block line
  forth_block_line @ /forth_block 1- =  \ last of block?
  if  (tidy_line)  else  2drop  then
  ;
: read_forth_block_line  ( -- ca len )
  source_code_line dup /forth_block_line source_code_fid read-file throw
\  2dup ." «" type ." »" cr  \ xxx informer
  ;
: save_forth_block_line  ( ca len -- )
  abersoft_forth? if  2dup tidy_line  then
  -trailing  dup forth_block_lenght +!
  append_source_code_line
  forth_block_line++ 0= if  echo_forth_block  then
  ;
: skip_tap_header  ( -- )
  \ Code adapted from my tool "scr2txt" (2005-2012).
  24 s>d source_code_fid reposition-file throw
  ;
: (forth_blocks_source_code)  ( -- )
  s" forth" filetype$ $!
  0 forth_block !
  0 forth_block_line !
  0 forth_block_lenght !
  0 source_code$ $!len
  begin   read_forth_block_line dup
  while   save_forth_block_line
  repeat  2drop 
  ;

export

: forth_blocks_source_code  ( ca len -- )
  \ Read the content of a Forth blocks file and echo it.
  \ ca len = file name
  open_source_code (forth_blocks) close_source_code
  ;
: abersoft_forth_blocks_source_code  ( ca len -- )
  \ Read the content of a ZX Spectrum's Abersoft Forth blocks TAP file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  abersoft_forth? on
  open_source_code skip_tap_header (forth_blocks_source_code) close_source_code
  abersoft_forth? off
  ;
: ql_forth_blocks_source_code  ( ca len -- )
  \ Read the content of a QL Forth blocks file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  forth_blocks_source_code
  ;

hide

\ **************************************************************
\ BASin source code

: not_basin_header?  ( ca len -- wf )
  \ ca len = source code line
  -leading drop c@ chr-digit?
  ;
: skip_basin_header  ( -- )
  \ Read the opened source file and skip the lines of the BASin header.
  0.  \ fake file position, for the first 2drop
  begin
    2drop  \ file position from the previous loop
    source_code_fid file-position throw 
    read_source_code_line >r  not_basin_header?  r> 0= or
  until  source_code_fid reposition-file throw
  ;

export
 
: basin_source_code  ( ca len -- )
  \ Read the content of a BASin file and echo it.
  \ xxx todo set the character set for this file type
  \ ca len = file name
  s" basin" filetype$ $!
  open_source_code skip_basin_header (opened_source_code)
  ;

;module

