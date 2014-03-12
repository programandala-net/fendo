.( fendo.addon.source_code.fs) cr

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

\ See at the end of the file.

\ **************************************************************
\ Todo

\ 2013-12-11: make '(filename>filetype)' configurable by the
\ application.
\ 2013-11-09: Syntax highlighting cache!
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\ 2014-03-12: Change: "fendo.addon.source_code.common.fs" filename updated.

\ **************************************************************
\ Requirements

\ From Gforth
require string.fs  \ Gforth's dynamic strings

\ From Galope
require galope/dollar-variable.fs  \ '$variable'
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/minus-leading.fs  \ '-leading'
require galope/string-suffix-question.fs  \ 'string-suffix?'
require galope/translated.fs  \ 'translated'

\ From Fendo
require ./fendo.addon.source_code.common.fs  \ xxx tmp

module: fendo.addon.source_code

: (filename>filetype)  { D: filename -- ca2 len2 }
  \ Convert a filename to a Vim's filetype.
  \ xxx todo make this configurable by the application
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
  programming_language$ $@ dup if  2nip  else  2drop (filename>filetype)  then
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
0 value source_code_fid
: read_source_code_line  ( -- ca len wf )
  \ Note: 'read_fid_line' is defined in
  \ <fendo/fendo_wiki_markup_wiki.fs>.
  source_code_fid read_fid_line
  ;
: slurp_source_code  ( -- ca len )
  \ Slurp the content of the opened source code file,
  \ from its current file position.
  \ ca len = source code read from the current file
  new_source_code
  begin   read_source_code_line
  while   append_source_code_line
  repeat  2drop source_code@
  ;
: noop_translation_table  ( -- 0 0 )
  \ Fake translation table that does nothing.
  0 dup
  ;
defer source_code_pretranslated  ( ca len -- ca' len' )
  \ Translate the source code before the highlighting.
  \ This must be vectored by a specific addon.
  \ This is used to translate 8-bit chars to UTF-8 chars;
  \ otherwise the code of those chars would be converted
  \ to text by the highlighting.
defer source_code_posttranslated  ( ca len -- ca' len' )
  \ Translate the source code after the highlighting.
  \ This must be vectored by a specific addon.
  \ This is used to translate strings to HTML entities or tags;
  \ otherwise the highlighting would ruin them.
: no_source_code_translation  ( -- )
  \ Deactivates the source code translations.
  ['] noop dup
  is source_code_pretranslated
  is source_code_posttranslated
  ;
no_source_code_translation
: source_code_finished  ( -- )
  \ Reset default values about the source code.
  s" " programming_language!  no_source_code_translation
  ;
: open_source_code  ( ca len -- )
  \ ca len = file name
  file>local r/o open-file throw  to source_code_fid
  ;
: close_source_code  ( -- )
  source_code_fid close-file throw  source_code_finished
  ;
: >source_code<  ( ca len -- ca' len' )
  \ Translate and highlight a source code.
  source_code_pretranslated highlighted source_code_posttranslated
  ;
: echo_source_code  ( ca len -- )
  \ Echo a source code.
  block_source_code{ >source_code< echo }block_source_code
  ;
: (opened_source_code)  ( -- )
  \ Read and echo the content of the opened source code file.
\  slurp_source_code translate_source_code echo_source_code close_source_code  \ xxx tmp
  slurp_source_code echo_source_code close_source_code
  ;
: (source_code)  ( ca len -- )
  \ Read and echo the content of a source code file.
  \ ca len = file name
  open_source_code (opened_source_code)
  ;
: source_code  ( ca len -- )
  \ Read and echo the content of a source code file.
  \ The Vim filetype is guessed from the filename, unless
  \ already set in the 'programming_language$' dinamyc string.
  \ ca len = file name
  2dup filename>filetype programming_language!  (source_code)
  ;

' source_code alias zx_spectrum_source_code  \ xxx tmp

;module

\ **************************************************************
\ Change history of this file

\ 2013-07-21: Start, with noop definitions from
\   <fendo-programandala.fs>; only the basic 'source_code' works.
\ 2013-07-26: New: BASin and Forth blocks.
\ 2013-07-26: Fix: now source files are closed at the end.
\ 2013-07-28: Fix: 'basin_source_code' called 'echo_source_code'
\   instead of '(echo_source_code)'.
\ 2013-11-07: Change: Forth blocks are printed apart; empty blocks are
\   omited; no line number are printed.
\ 2013-11-08: Change: source code is not echoed by lines
\   anymore; this is a first step towards syntax highlighting.
\ 2013-11-08: Fix: The rubbish byte at the end of the last block of an
\   Abersoft Forth blocks file is removed.
\ 2013-11-09: First working version with syntax highlighting.
\   Addon moved from Fendo-programandala to Fendo, because part of the
\   code is required to implement optional syntax highlighting in the
\   '###' markup.
\ 2013-11-09: The BASin-specific code is moved to its own file.
\ 2013-11-09: The Forth-blocks-specific code is moved to its own file.
\ 2013-11-18: 'open_source_code' factored out to 'file>local' (defined
\   in <fendo_files.fs>.
\ 2013-11-18: Change: 'programming_language' renamed to
\   'programming_language!'.
\ 2013-11-18: New: 'programming_language@'.
\ 2013-11-18: Change: All words related to syntax highlighting
\   are moved to <addons/source_code_common.fs>, because they are needed
\   also by the "###" markup.
\ 2013-12-10: Character set translation implemented with
\ <galope/translated.fs>: the default noop translation table
\   must be changed by the specific addons.
\ 2013-12-11: Character set translation improved: two
\   translations can be done: one before the highlighting and other after
\   it; this prevents the highlighting from ruining some
\   translations. Besides, an xt is used instead of a translation
\   table created with <galope/translated.fs>;
\   this way any tool can be used for the task.
\ 2014-02-06: New: 'source_code_finished' now does all reseting final task.
\   This fixes some obscure issues too.
\ 2014-03-12: Change: module renamed after the filename.

.( fendo.addon.source_code.fs compiled) cr
