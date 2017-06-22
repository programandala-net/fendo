.( fendo.addon.source_code.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the source code addon.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2015,2017 Marcos Cruz (programandala.net)

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
\ Todo

\ 2013-12-11: make '(filename>filetype)' configurable by the
\ application.
\ 2013-11-09: Syntax highlighting cache!

\ ==============================================================
\ Requirements

forth_definitions

require string.fs  \ Gforth's dynamic strings

require galope/dollar-variable.fs  \ '$variable'
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/minus-leading.fs  \ '-leading'
require galope/string-suffix-question.fs  \ 'string-suffix?'
require galope/translated.fs  \ 'translated'

fendo_definitions

require ./fendo.addon.source_code.common.fs  \ XXX TMP

\ ==============================================================

module: fendo.addon.source_code

: (filename>filetype) ( ca1 len1 -- ca2 len2 )
  basename  \ remove the path, because some comparations use the whole filename
  { D: filename }
  filename s" .4th" string-suffix? if  s" forth" exit  then
  filename s" .acef" string-suffix? if  s" aceforth" exit  then
  filename s" .acefs" string-suffix? if  s" aceforth" exit  then
  filename s" .asm" string-suffix? if  s" z80" exit  then
  filename s" .bac" string-suffix? if  s" bacon" exit  then
  filename s" .bas" string-suffix? if  s" basic" exit  then
  filename s" .bb" string-suffix? if  s" betabasic" exit  then
  filename s" .bbas" string-suffix? if  s" betabasic" exit  then
  filename s" .betabas" string-suffix? if  s" betabasic" exit  then
  filename s" .bbim" string-suffix? if  s" bbim" exit  then
  filename s" .fs" string-suffix? if  s" gforth" exit  then
  filename s" .ini" string-suffix? if  s" dosini" exit  then
  filename s" .mb" string-suffix? if  s" masterbasic" exit  then
  filename s" .mbas" string-suffix? if  s" masterbasic" exit  then
  filename s" .masterbas" string-suffix? if  s" masterbasic" exit  then
  filename s" .mbim" string-suffix? if  s" mbim" exit  then
  filename s" .opl" string-suffix? if  s" oplplus" exit  then
  filename s" .opl.txt" string-suffix? if  s" oplplus" exit  then
  filename s" .opp" string-suffix? if  s" oplplus" exit  then
  filename s" .php" string-suffix? if  s" php" exit  then
  filename s" .prg" string-suffix? if  s" clipper" exit  then
  filename s" .sbim" string-suffix? if  s" sbim" exit  then
  filename s" .sdlbas" string-suffix? if  s" sdlbasic" exit  then
  filename s" .seq" string-suffix? if  s" forth" exit  then
  filename s" .sinclairbas" string-suffix? if  s" sinclairbasic" exit  then
  filename s" .sh" string-suffix? if  s" sh" exit  then
  filename s" .unexpanded_llist" string-suffix? if  s" sinclairbasic" exit  then
  filename s" .vim" string-suffix? if  s" vim" exit  then
  filename s" .vimbas" string-suffix? if  s" vimclairbasic" exit  then
  filename s" .vimclairbas" string-suffix? if  s" vimclairbasic" exit  then
  filename s" .vbas" string-suffix? if  s" vimclairbasic" exit  then
  filename s" .xbas" string-suffix? if  s" x11basic" exit  then
  filename s" .yab" string-suffix? if  s" yabasic" exit  then
  filename s" .z80s" string-suffix? if  s" z80" exit  then
  filename s" .zxbas" string-suffix? if  s" zxbasic" exit  then
  filename s" _bas" string-suffix? if  s" superbasic" exit  then
  filename s" _sbim" string-suffix? if  s" sbim" exit  then
\  filename s" _scr" string-suffix? if  s" forth" exit  then  \ XXX TODO
\  filename s" _cmd" string-suffix? if  s" text" exit  then  \ XXX TODO
  filename s" boot" str=  if  s" superbasic" exit  then
  filename s" ratpoisonrc" str=  if  s" ratpoison" exit  then
  filename s" Makefile" str=  if  s" make" exit  then
  s" text" ;

: filename>filetype ( ca1 len1 -- ca2 len2 )
  \ Convert a filename to a Vim's filetype, if needed.
  \ If 'programming_language$' has been set, use it; this let the application
  \ to override the default guessing based on the filename.
\  cr ." In filename>filetype " 2dup type ."  --> "  \ XXX INFORMER
  programming_language$ $@ dup if  2nip  else  2drop (filename>filetype)  then
  2dup previous_programming_language$ $!
\  2dup type cr cr cr  \ XXX INFORMER
  ;
  \ Convert a filename to a Vim's filetype.
  \ XXX TODO make all this configurable by the application
  \ XXX TODO use a data structure instead of conditionals

\ ==============================================================
\ Generic source code

export
0 value source_code_fid
: read_source_code_line ( -- ca len f )
  source_code_fid read_fid_line ;
  \ Note: 'read_fid_line' is defined in
  \ <fendo/fendo_wiki_markup_wiki.fs>.

: slurp_source_code ( -- ca len )
  new_source_code
  begin   read_source_code_line
  while   append_source_code_line
  repeat  2drop source_code@ ;
  \ Slurp the content of the opened source code file,
  \ from its current file position.
  \ ca len = source code read from the current file

: noop_translation_table ( -- 0 0 )
  0 dup ;
  \ Fake translation table that does nothing.

0 [if]

\ XXX TMP -- moved from <fendo.addon.source_code.fs>:

defer source_code_pretranslated ( ca len -- ca' len' )
  \ Translate the source code before the highlighting.
  \ This must be vectored by a specific addon.
  \ This is used to translate 8-bit chars to UTF-8 chars;
  \ otherwise the code of those chars would be converted
  \ to text by the highlighting.

defer source_code_posttranslated ( ca len -- ca' len' )
  \ Translate the source code after the highlighting.
  \ This must be vectored by a specific addon.
  \ This is used to translate strings to HTML entities or tags;
  \ otherwise the highlighting would ruin them.

: no_source_code_translation ( -- )
  ['] noop dup
  is source_code_pretranslated
  is source_code_posttranslated ;
  \ Deactivate the source code translations.

[then]

no_source_code_translation

: open_source_code ( ca len -- )
  file>local r/o open-file throw  to source_code_fid ;
  \ ca len = file name

: close_source_code ( -- )
  source_code_fid close-file throw  source_code_finished
\  [<p>] s" At the end of 'close_source_code' 'programming_language' = " echo   \ XXX INFORMER
\  s" «" programming_language@ s" »" s+ s+ echo [</p>]  \ XXX INFORMER
  ;

: >source_code< ( ca len -- ca' len' )
\  cr ." In >source_code<"  \ XXX INFORMER
  source_code_pretranslated highlighted source_code_posttranslated ;
  \ Translate and highlight a source code.

: echo_source_code ( ca len -- )
  block_source_code{ >source_code< echo }block_source_code ;
  \ Echo a source code.

: (opened_source_code) ( -- )
\  slurp_source_code translate_source_code echo_source_code close_source_code  \ XXX TMP
  slurp_source_code echo_source_code close_source_code ;
  \ Read and echo the content of the opened source code file.

: (source_code) ( ca len -- )
  open_source_code (opened_source_code) ;
  \ Read and echo the content of a source code file.
  \ ca len = file name

: source_code ( ca len -- )
  2dup filename>filetype programming_language!  (source_code) ;
  \ Read and echo the content of a source code file.
  \ The Vim filetype is guessed from the filename, unless
  \ already set in the 'programming_language$' dynamic string.
  \ ca len = file name

: unsorted_source_code_files_by_dir_and_regex ( ca1 len1 ca2 len2 -- )
  programming_language$ $@ { D: filetype$ }  \ save the current value
  >regex 2dup { D: directory$ } file>local open-dir throw ( dirid )
  begin   dup pad 256 rot read-dir throw ( dirid len f )
  while   pad over ( dirid len ca len ) regex rgx-wcmatch?
          if    pad swap directory$ s" /" s+ 2swap s+ source_code
                filetype$ programming_language!  \ restore for the next file
          else  drop  then
  repeat  drop close-dir throw ;
  \ Read and echo the content of source code files
  \ in local target files directory ca1 len1 that match
  \ regex ca2 len2. The files will be unsorted.
  \ The Vim filetype is guessed from the filenames, unless
  \ already set in the 'programming_language$' dynamic string.
  \ ca1 len1 = path under the target <files> directory,
  \   with or without ending slash
  \ ca2 len2 = regex supported by the rgx module
  \   of the Forth Foundation Library

: (sorted_source_code_files_by_dir_and_regex) ( ca1 len1 ca2 len2 -- )
  programming_language$ $@ { D: filetype$ } \ save the current value
  >regex 2dup { D: directory$ } file>local open-dir throw ( dirid )
  begin   dup pad 256 rot read-dir throw ( dirid len f )
  while   pad over ( dirid len ca len ) regex rgx-wcmatch?
          if    \ Create a line of code.
                s\" s\" " type
                filetype$ type
                s\" \" programming_language! " type
                s\" s\" " type
                directory$ type s" /" type pad swap type [char] " emit
                s"  source_code" type cr
          else  drop  then
  repeat  drop close-dir throw ;
  \ Type Forth code that will read and echo the content of source code
  \ files in local target files directory ca1 len1 that match regex
  \ ca2 len2. The files will be unsorted. The output is already
  \ redirected to a temporary file, so this work actually creates a
  \ Forth source file that will be sorted and interpreted later.
  \
  \ The Vim filetype is guessed from the filenames, unless
  \ already set in the 'programming_language$' dynamic string.
  \ ca1 len1 = path under the target <files> directory,
  \   with or without ending slash
  \ ca2 len2 = regex supported by the rgx module
  \   of the Forth Foundation Library

s" /tmp/fendo.source_codes_by_dir_and_regex.txt" 2dup 2constant tmp_raw_file$

s" .fs" s+ 2constant tmp_sorted_file$

: sorted_source_code_files_by_dir_and_regex ( ca1 len1 ca2 len2 -- )
  ['] (sorted_source_code_files_by_dir_and_regex)
  tmp_raw_file$ w/o create-file throw  dup >r outfile-execute
  r> close-file throw
    \ First, create a file with the required commands,
    \ by executing '(sorted_source_codes_by_dir_and_regex)' with
    \ the output redirected to the file.

  s" sort " tmp_raw_file$ s+ s"  > " s+ tmp_sorted_file$ s+ system
    \ Second, sort the file, using the OS shell.

  tmp_sorted_file$ included ;
  \ Read and echo the content of source code files
  \ in local target files directory ca1 len1 that match
  \ regex ca2 len2. The files will be sorted.
  \
  \ The Vim filetype is guessed from the filenames, unless
  \ already set in the 'programming_language$' dynamic string.
  \
  \ ca1 len1 = path under the target <files> directory,
  \   with or without ending slash
  \ ca2 len2 = regex supported by the rgx module
  \   of the Forth Foundation Library
  \
  \ XXX TODO -- use previous_programming_language$

;module

.( fendo.addon.source_code.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-07-21: Start, with noop definitions from
\ <fendo-programandala.fs>; only the basic 'source_code' works.
\
\ 2013-07-26: New: BASin and Forth blocks.
\
\ 2013-07-26: Fix: now source files are closed at the end.
\
\ 2013-07-28: Fix: 'basin_source_code' called 'echo_source_code'
\ instead of '(echo_source_code)'.
\
\ 2013-11-07: Change: Forth blocks are printed apart; empty blocks are
\ omited; no line number are printed.
\
\ 2013-11-08: Change: source code is not echoed by lines anymore; this
\ is a first step towards syntax highlighting.
\
\ 2013-11-08: Fix: The rubbish byte at the end of the last block of an
\ Abersoft Forth blocks file is removed.
\
\ 2013-11-09: First working version with syntax highlighting.  Addon
\ moved from Fendo-programandala to Fendo, because part of the code is
\ required to implement optional syntax highlighting in the '###'
\ markup.
\
\ 2013-11-09: The BASin-specific code is moved to its own file.
\
\ 2013-11-09: The Forth-blocks-specific code is moved to its own file.
\
\ 2013-11-18: 'open_source_code' factored out to 'file>local' (defined
\ in <fendo_files.fs>.
\
\ 2013-11-18: Change: 'programming_language' renamed to
\ 'programming_language!'.
\
\ 2013-11-18: New: 'programming_language@'.
\
\ 2013-11-18: Change: All words related to syntax highlighting are
\ moved to <addons/source_code_common.fs>, because they are needed
\ also by the "###" markup.
\
\ 2013-12-10: Character set translation implemented with
\ <galope/translated.fs>: the default noop translation table must be
\ changed by the specific addons.
\
\ 2013-12-11: Character set translation improved: two translations can
\ be done: one before the highlighting and other after it; this
\ prevents the highlighting from ruining some translations. Besides,
\ an xt is used instead of a translation table created with
\ <galope/translated.fs>; this way any tool can be used for the task.
\
\ 2014-02-06: New: 'source_code_finished' now does all reseting final
\ task.  This fixes some obscure issues too.
\
\ 2014-02-15: Fix: path of the Fendo addons is converted to relative.
\
\ 2014-03-12: Change: module renamed after the filename;
\ "fendo.addon.source_code.common.fs" filename updated.
\
\ 2014-10-13: New: 'zx_spectrum_source_code'.
\
\ 2014-10-17: Change: 'zx_spectrum_source_code' is moved to its own
\ file <fendo.addon.zx_spectrum_source_code.fs>.
\
\ 2014-10-17: Improvement: '(filename>filetype)' is updated with
\ Vimclair BASIC filetype, and alternative extensions for MasterBASIC
\ and Beta BASIC.
\
\ 2014-10-19: New: 'programming_language?!'.
\
\ 2014-12-07: Change: 'source_code_finished' and related words have
\ been moved to <fendo.addon.source_code.common.fs>, in order to use
\ that word in <fendo.markup.fendo.code.fs>.
\
\ 2014-12-07: Change: removed old useless code about specific-platform
\ encodings.
\
\ 2015-01-30: New: ".sinclairbas" extension for Sinclair BASIC.
\
\ 2015-01-31: New: additional extensions: '.vimbas' and 'vimclairbas'
\ for Vimclair BASIC; '.unexpanded_llist' for Sinclair BASIC.
\
\ 2015-02-04: Fix: removed the definition of 'source_code_finished',
\ already moved to to <fendo.addon.source_code.common.fs>.
\
\ 2015-05-02: New: 'unsorted_source_codes_by_dir_and_regex',
\ "Makefile" support in '(filename>filetype)'.
\ 'sorted_source_codes_by_dir_and_regex'.
\
\ 2015-05-05: Fix: '(filename>filetype)' didn't recognize whole
\ filenames, only suffixes, because the path was not removed.
\
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
