.( addons/source_code.fs) cr

\ This file is part of Fendo.

\ This file is the source code addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

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
\ 2013-11-09 First working version with syntax highlighting.
\   Addon moved from Fendo-programandala to Fendo, because part of the
\   code is required to implement optional syntax highlighting in the
\   '###' markup.
\ 2013-11-09 The BASin-specific code is moved to its own file.
\ 2013-11-09 The Forth-blocks-specific code is moved to its own file.

\ **************************************************************
\ Todo

\ 2013-07-26 Character set conversions.
\ 2013-11-09 Syntax highlighting cache!

\ **************************************************************
\ Requirements

require string.fs  \ Gforth's dynamic strings
require galope/dollar-variable.fs  \ '$variable'
require galope/module.fs
require galope/minus-leading.fs  \ '-leading'
require galope/sourcepath.fs  \ 'sourcepath'
require galope/string-suffix-question.fs  \ 'string-suffix?'

module: source_code_fendo_addon_module

\ **************************************************************
\ Syntax highlighting with Vim

\ xxx fixme remove spaces and use 's&' instead of 's+'.

s" /tmp/fendo-programandala.addon.source_code.txt" 2dup 2constant input_file$
s" .xhtml" s+ 2constant output_file$

export  \ xxx tmp
true [if]
s" vim -f " 2constant base_highlight_command$
[else]  \ xxx tmp
$variable (base_highlight_command$)
s" vim -f " (base_highlight_command$) $!
: base_highlight_command$  ( -- ca len )
  (base_highlight_command$) $@
  ;
[then]
sourcepath s" source_code.vim " s+ 2constant vim_program$
hide

export
$variable filetype$  \ same values than Vim's 'filetype' 
: programming_language  ( ca len -- )
  \ Set the Vim's filetype for syntax highlighting.
  filetype$ $!
  ;
hide

: program+  ( ca len -- ca' len' )
  \ Add the Vim program parameter to the Vim invocation.
  s" -S " s+ vim_program$ s+
  ;
: syntax+  ( ca len -- ca' len' )
  \ Add the desired syntax parameter to the Vim invocation.
  \ This parameter must be the first one in the command line.
  s\" -c \"set filetype=" s+ filetype$ $@ s+ s\" \" " s+
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
  \ Save the given source code to the file that Vim will load as input.
  \ ca len = plain source code
  input_file$ w/o create-file throw
  dup >r write-file throw
  r> close-file throw
  ;
: <output_file  ( -- ca len )
  \ Get the content of the file that Vim created as output.
  \ ca len = source code highlighted with <span> XHTML tags
  output_file$ slurp-file
  ;
: (highlighted)  ( ca1 len1 -- ca2 len2 )
  \ Highlight the given source code.
  \ ca1 len1 = plain source code
  \ ca2 len2 = source code highlighted with <span> XHTML tags
  >input_file
  highlighting_command$ 
\  2dup type cr  \ xxx informer
  system
  $? abort" The highlighting command failed"
  <output_file
  ;
export
variable highlight?  highlight? on
: highlighted  ( ca1 len1 -- ca1 len1 | ca2 len2 )
  \ Highlight the given source code, if needed.
  \ ca1 len1 = plain source code
  \ ca2 len2 = source code highlighted with <span> XHTML tags
  highlight? @ if  (highlighted)  then  s" " programming_language
  ;
: (filename>filetype)  { D: filename -- ca2 len2 }
  \ Convert a filename to a Vim's filetype.
  filename s" .4th" string-suffix? if s" forth" exit  then
  filename s" .asm" string-suffix? if s" z80" exit  then
  filename s" .bac" string-suffix? if s" bacon" exit  then
  filename s" .bas" string-suffix? if s" basic" exit  then
  filename s" .bb" string-suffix? if s" beta_basic" exit  then
  filename s" .bbim" string-suffix? if s" bbim" exit  then
  filename s" .fs" string-suffix? if s" forth" exit  then
  filename s" .ini" string-suffix? if s" dosini" exit  then
  filename s" .mb" string-suffix? if s" masterbasic" exit  then
  filename s" .mbim" string-suffix? if s" mbim" exit  then
  filename s" .opl" string-suffix? if s" oplplus" exit  then
  filename s" .opl.txt" string-suffix? if s" oplplus" exit  then
  filename s" .opp" string-suffix? if s" oplplus" exit  then
  filename s" .php" string-suffix? if s" php" exit  then
	filename s" .prg" string-suffix? if  s" clipper" exit  then
  filename s" .sbim" string-suffix? if s" sbim" exit  then
  filename s" .sdlbas" string-suffix? if s" sdlbasic" exit  then
  filename s" .seq" string-suffix? if s" forth" exit  then
  filename s" .sh" string-suffix? if s" sh" exit  then
  filename s" .vim" string-suffix? if s" vim" exit  then
  filename s" .xbas" string-suffix? if s" x11basic" exit  then
  filename s" .yab" string-suffix? if s" yabasic" exit  then
  filename s" .z80s" string-suffix? if s" z80" exit  then
  filename s" _bas" string-suffix? if  s" superbasic" exit  then
\  filename s" _scr" string-suffix? if  s" forth" exit  then  \ xxx  todo
\  filename s" _cmd" string-suffix? if  s" text" exit  then  \ xxx todo
  filename s" boot" str=  if  s" superbasic" exit  then
  filename s" ratpoisonrc" str=  if  s" ratpoison" exit  then
  s" text"
  ;
: filename>filetype  ( ca1 len1 -- ca2 len2 )
  \ Convert a filename to a Vim's filetype, if needed.
  \ If 'filetype$' has been set, use it; this let the application
  \ to override the default guessing based on the filename.
  filetype$ $@ dup if  2nip  else  2drop (filename>filetype)  then
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

export
$variable source_code$
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
  \ Note: 'read_fid_line' is defined in
  \ <fendo/fendo_wiki_markup_wiki.fs>.
  source_code_fid read_fid_line
  ;
: echo_source_code  ( ca len -- )
  block_source_code{  highlighted echo  }block_source_code
  ;
: append_source_code_line  ( ca len -- )
  source_code$ $+!  s\" \n" source_code$ $+!
  ;
: empty_source_code  ( -- )
  0 source_code$ $!len
  ;
: source_code@  ( -- ca len )
  source_code$ $@
  ;
: slurp_source_code  ( -- ca len )
  \ Slurp the content of the opened source code file,
  \ from its current file position.
  \ ca len = source code read from the current file
  begin   read_source_code_line
  while   append_source_code_line
  repeat  2drop source_code@
  ;
: (opened_source_code)  ( -- )
  \ Read and echo the content of the opened source code file.
  slurp_source_code echo_source_code close_source_code 
  ;
: (source_code)  ( ca len -- )
  \ Read and echo the content of a source code file.
  \ ca len = file name
  open_source_code (opened_source_code)
  ;
: source_code  ( ca len -- )
  \ Read and echo the content of a source code file.
  \ The Vim filetype is guessed from the filename.
  \ ca len = file name
  2dup filename>filetype programming_language  (source_code)
  ;

;module

.( addons/source_code.fs compiled) cr
