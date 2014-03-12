.( fendo.addon.source_code.common.fs) cr

\ This file is part of Fendo.

\ This file is the code common to several source code addons.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ 2013-11-18: Code extracted from <fendo_source_code.fs>.

\ 2013-12-11: New: '-b' parameter added to 'syntax+', then renamed to
\ 'parameters+'.

\ 2014-01-06: New: 'escaped_source_code'.

\ 2014-01-06: Fix: 'append_source_code_line' now escapes de the code
\ with 'escaped_source_code'.

\ 2014-02-06: Fix: 'highlighted' reseted 'programming_language$' befor
\ exiting, what turned off the highlighting of Forth blocks.

\ 2014-02-07: Fix: now 'append_source_code_line' calls
\ 'escaped_source_code' only if 'highlight?' is false, because Vim
\ will do the task while highlighting; if "<" were already converted
\ to "&lt;", Vim converted it to "&amp;lt;" and the code was ruined.

\ 2014-02-07: New: 'escaped_source_code' translates "&" too.

\ 2014-02-28: Change: 'replaced' is adapted to its new version in
\ Galope

\ 2014-03-02: Trivial fix.

\ 2014-03-09: Fix: 'escaped_source_code' converted ampersands, what
\ ruined the rest of HTML entities in source code blocks.

\ 2014-03-12: Change: filename and module renamed.

\ **************************************************************
\ Todo

\ **************************************************************
\ Requirements

\ From Galope
require galope/sourcepath.fs  \ 'sourcepath'
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'
require galope/replaced.fs  \ 'replaced'

\ **************************************************************

module: fendo.addon.source_code.common

export

false value highlight?  \ flag to switch the code highlighting on and off
$variable programming_language$  \ same values than Vim's 'filetype'
: programming_language!  ( ca len -- )
  \ Set the Vim's filetype for syntax highlighting.
  programming_language$ $!
  ;
: programming_language@  ( -- ca len )
  \ Fetch the Vim's filetype for syntax highlighting.
  programming_language$ $@
  ;
: programming_language?  ( -- wf )
  \ Has a programming language been set?
  programming_language@ nip 0<>
  ;

$variable source_code$
: escaped_source_code  ( ca len -- ca' len' )
  \ Escape special chars in source code.
  \ Used by the wiki markup module and the source code addon.
  \ s" &amp;" s" &" replaced  \ xxx fixme this ruins the others
  s" &lt;" s" <" replaced
  ;
: append_source_code_line  ( ca len -- )
  highlight? 0= if  escaped_source_code  then
  source_code$ $+!  s\" \n" source_code$ $+!
  ;
: new_source_code  ( -- )
  0 source_code$ $!len
  ;
: source_code@  ( -- ca len )
  source_code$ $@
  ;

\ **************************************************************
\ Syntax highlighting with Vim

\ xxx fixme remove spaces and use 's&' instead of 's+'.

hide

s" ./cache/addons/source_code/" 2constant cache_dir$  \ xxx todo unused
s" /tmp/fendo_addon.source_code.txt" 2dup 2constant input_file$
s" .xhtml" s+ 2constant output_file$

export  \ xxx tmp
true [if]
s" ex -f " 2constant base_highlight_command$
[else]  \ xxx tmp
$variable (base_highlight_command$)
s" vim -f " (base_highlight_command$) $!
: base_highlight_command$  ( -- ca len )
  (base_highlight_command$) $@
  ;
[then]
sourcepath s" source_code.vim " s+ 2constant vim_program$
hide

: program+  ( ca len -- ca' len' )
  \ Add the Vim program parameter to the Vim invocation command.
  s" -S " s+ vim_program$ s+
  ;
: parameters+  ( ca len -- ca' len' )
  \ Add the required parameters to the Vim invocation command.
  \ These parameters must be before the Vim program
  \ and the source file in the command.
  \ The binary option (-b) is required to make the
  \ charset translations to work fine.
\  ." programming_language$ in parameters+ is " programming_language$ $@ type cr  \ xxx informer
  s\" -b -c \"set filetype=" s+ programming_language$ $@ s+ s\" \" " s+
  ;
: file+  ( ca len -- ca' len' )
  \ Add the input file parameter to the Vim invocation command.
  input_file$ s+
  ;
: highlighting_command$  ( -- ca len )
  \ Return the complete highlighting command,
  \ ready to be executed by the shell.
  base_highlight_command$ parameters+ program+ file+
\  ." highlighting_command$ = " 2dup type cr  \ xxx informer
  ;
: >input_file  ( ca len -- )
  \ Save the given source code to the file that Vim will load as input.
  \ ca len = plain source code
  input_file$ w/o create-file throw
  dup >r write-file throw  r> close-file throw
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
  highlighting_command$ system
  $? abort" The highlighting command failed"
  <output_file
  ;
export
: highlighted  ( ca1 len1 -- ca1 len1 | ca2 len2 )
  \ Highlight the given source code, if needed.
  \ ca1 len1 = plain source code
  \ ca2 len2 = source code highlighted with <span> XHTML tags
\  ." programming_language$ in highlighted is " programming_language$ $@ type cr  \ xxx informer
\  ." plain source code in highlighted" cr 2dup type key drop  \ xxx informer
  highlight? if  (highlighted)  then
  ;

;module

.( fendo.addon.source_code.common.fs compiled) cr
