.( fendo.addon.source_code.common.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the code common to several source code addons.

\ Last modified 201812080157.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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
require galope/replaced.fs  \ `replaced`
require galope/sourcepath.fs  \ `sourcepath`

fendo_definitions

\ ==============================================================

package fendo.addon.source_code.common

public

false value highlight?
  \ flag to switch the code highlighting on and off

$variable programming_language$
  \ same values than Vim's `filetype`

$variable previous_programming_language$
  \ copy used by some addons

: programming_language! ( ca len -- )
  programming_language$ $! ;
  \ Set the Vim's filetype for syntax highlighting.

: programming_language@ ( -- ca len )
  programming_language$ $@ ;
  \ Fetch the Vim's filetype for syntax highlighting.

: programming_language? ( -- f )
  programming_language$ $@len 0<> ;
  \ Has a programming language been set?

: programming_language?! ( ca len -- )
  programming_language? if  2drop  else  programming_language! then ;
  \ Set the Vim's filetype for syntax highlighting, if not already set.

$variable source_code$

: escaped_source_code ( ca len -- ca' len' )
  highlight? 0= if s" &lt;" s" <" replaced then ;
  \ Escape special chars in source code.
  \ Used by the wiki markup module and the source code addon.

: append_source_code_line ( ca len -- )
  highlight? 0= if  escaped_source_code  then
  source_code$ $+!  s\" \n" source_code$ $+! ;

: new_source_code ( -- )
  0 source_code$ $!len ;

: source_code@ ( -- ca len )
  source_code$ $@ ;

\ ==============================================================
\ Syntax highlighting with Vim

\ XXX FIXME remove spaces and use `s&` instead of `s+`.

private

s" ./cache/addons/source_code/" 2constant cache_dir$  \ XXX TODO unused

s" /tmp/fendo_addon.source_code.txt" 2dup 2constant input_file$

s" .xhtml" s+ 2constant output_file$

public  \ XXX TMP

true [if]
  s" ex -f " 2constant base_highlight_command$
[else]  \ XXX TMP
  $variable (base_highlight_command$)
  s" vim -f " (base_highlight_command$) $!
  : base_highlight_command$ ( -- ca len )
    (base_highlight_command$) $@
    ;
[then]

sourcepath s" fendo.addon.source_code.vim " s+ 2constant vim_program$

private

: program+ ( ca len -- ca' len' )
  s" -S " s+ vim_program$ s+ ;
  \ Add the Vim program parameter to the Vim invocation command.

: parameters+ ( ca len -- ca' len' )
\  ." programming_language$ in parameters+ is " programming_language$ $@ type cr  \ XXX INFORMER
  s\" -b -c \"set filetype=" s+ programming_language$ $@ s+ s\" \" " s+ ;
  \ Add the required parameters to the Vim invocation command.
  \ These parameters must be before the Vim program
  \ and the source file in the command.
  \ The binary option (-b) is required to make the
  \ charset translations to work fine.

: highlighting_command$ ( -- ca len )
  base_highlight_command$ parameters+ program+ input_file$ s+
\  ." highlighting_command$ = " 2dup type cr  \ XXX INFORMER
  ;
  \ Return the complete highlighting command,
  \ ready to be executed by the shell.
  \ The command calls Vim in execution mode, this way:
  \   ex -f -b -c "set filetype=PROGRAMMING_LANGUAGE"
  \      -S ~/forth/fendo/fendo.addon.source_code.vim /tmp/fendo_addon.source_code.txt

\ XXX TODO -- There are similar words `>input_file` and `<output_file`
\ in <fendo.addon.source_code.common.fs>; maybe they can be shared.

: >input_file ( ca len -- )
  input_file$ w/o create-file throw
  dup >r write-file throw  r> close-file throw ;
  \ Save the given source code to the file that Vim will load as input.
  \ ca len = plain source code

: <output_file ( -- ca len )
  output_file$ slurp-file ;
  \ Get the content of the file. that Vim created as output.
  \ ca len = source code highlighted with <span> XHTML tags

: (highlighted) ( ca1 len1 -- ca2 len2 )
  >input_file
  highlighting_command$ system
  $? abort" The system highlighting command failed"
  <output_file ;
  \ Highlight the given source code.
  \ ca1 len1 = plain source code
  \ ca2 len2 = source code highlighted with <span> XHTML tags

public
: highlighted ( ca1 len1 -- ca1 len1 | ca2 len2 )
\  ." programming_language$ in highlighted is " programming_language$ $@ type cr cr cr  \ XXX INFORMER
\  ." plain source code in highlighted" cr 2dup type key drop  \ XXX INFORMER
  highlight? if  (highlighted)  then ;
  \ Highlight the given source code, if needed.
  \ ca1 len1 = plain source code
  \ ca2 len2 = source code highlighted with <span> XHTML tags

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

no_source_code_translation

: source_code_finished ( -- )
  s" " programming_language!  no_source_code_translation ;
  \ Reset default values about the source code.

end-package

.( fendo.addon.source_code.common.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-11-18: Code extracted from <fendo_source_code.fs>.
\
\ 2013-12-11: New: `-b` parameter added to `syntax+`, then renamed to
\ `parameters+`.
\
\ 2014-01-06: New: `escaped_source_code`.
\
\ 2014-01-06: Fix: `append_source_code_line` now escapes de the code
\ with `escaped_source_code`.
\
\ 2014-02-06: Fix: `highlighted` reseted `programming_language$`
\ before exiting, what turned off the highlighting of Forth blocks.
\
\ 2014-02-07: Fix: now `append_source_code_line` calls
\ `escaped_source_code` only if `highlight?` is false, because Vim
\ will do the task while highlighting; if "<" were already converted
\ to "&lt;", Vim converted it to "&amp;lt;" and the code was ruined.
\
\ 2014-02-07: New: `escaped_source_code` translates "&" too.
\
\ 2014-02-28: Change: `replaced` is adapted to its new version in
\ Galope
\
\ 2014-03-02: Trivial fix.
\
\ 2014-03-09: Fix: `escaped_source_code` converted ampersands, what
\ ruined the rest of HTML entities in source code blocks.
\
\ 2014-03-12: Change: filename and module renamed.
\
\ 2014-06-16: Fix: <source_code.vim> renamed to
\ <fendo.addon.source_code.vim>.
\
\ 2014-10-24: Change: `escaped_source_code` only works if `highlight?`
\ is off; otherwise Vim does its own substitutions.
\
\ 2014-11-01: Fix: `escaped_source_code` converted "&" to HTML
\ notation, what ruined the HTML entities.
\
\ 2014-12-07: Change: `source_code_finished` and related words have
\ been moved from <fendo.addon.source_code.fs>, in order to use that
\ word in <fendo.markup.fendo.code.fs>.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
