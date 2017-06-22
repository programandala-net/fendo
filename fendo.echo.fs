.( fendo.echo.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the words that print to the target HTML file.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute
\ it and/or modify it under the terms of the GNU General
\ Public License as published by the Free Software
\ Foundation; either version 2 of the License, or (at your
\ option) any later version.

\ Fendo is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied
\ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
\ PURPOSE.  See the GNU General Public License for more
\ details.

\ You should have received a copy of the GNU General Public
\ License along with this program; if not, see
\ <http://gnu.org/licenses>.

\ ==============================================================
\ Requirements

forth_definitions
require galope/n-to-str.fs  \ 'n>str'
require galope/xstack.fs
fendo_definitions

\ ==============================================================
\ Output

variable echo>  \ destination of the output
\ Possible values of 'echo>':
0 constant >screen
1 constant >file
2 constant >string

variable echoed  \ used as dynamic string
s" " echoed $!

8 xstack echo_stack  \ create an extra stack

: save_echo ( -- )
  echo_stack  echo> @ >x  echoed $@ 2>x ;
  \ Save the echo status into the echo stack.

: restore_echo ( -- )
  echo_stack  2x> echoed $!  x> echo> ! ;
  \ Restore the echo status from the echo stack.

: echo>string ( -- )
  >string echo> !  0 echoed $!len ;
  \ Redirect the output to the dynamic string 'echoed'.

: echo>file ( -- )
  >file echo> ! ;
  \ Redirect the output to the target file.

: echo>screen ( -- )
  >screen echo> ! ;
  \ Redirect the output to the screen.

: echo>file? ( -- f )
  echo> @ >file = ;

: echo>screen? ( -- f )
  echo> @ >screen = ;

: echo>string? ( -- f )
  echo> @ >string = ;

echo>file
\ echo>screen  \ XXX for debugging

\ ==============================================================
\ Echo

variable target_fid  \ file id of the HTML target page

: (echo) ( xt | ca len xt -- )
  echo>screen?
  if  execute  else  target_fid @ outfile-execute  then ;

: (echo>string) ( ca len -- )
  echoed $+! ;
  \ Add a string to the 'echoed' string.

: echo ( ca len -- )
  echo>string?
  if  (echo>string)  else  ['] type (echo)  then ;
  \ Print a text string to the HTML file.

variable separate?  \ flag: separate the next item from the current one?

false value compact_html?  \ if true, no carriage return is created

\ XXX FIXME 'compact_html? is an experimental config flag for the
\ user's application. It makes the HTML a bit smaller, but more
\ difficult to read. The problem is it could ruin the contents in some
\ cases.

: (echo_cr) ( -- )
  echo>string?  if    s\" \n" (echo>string)
                else  ['] cr (echo)
                then  separate? off ;
  \ Do print a carriage return to the HTML file.

: echo_cr ( -- )
  compact_html? 0= if  (echo_cr)  then ;
  \ Print a carriage return to the HTML file.

' echo_cr alias \n

: echo_space ( -- )
  s"  " echo ;
  \ Print a space to the HTML file.

: echo_quote ( -- )
  s\" \"" echo ;
  \ Print a double quote to the HTML file.

: echo_period ( -- )
  s" ." echo ;
  \ Print a period to the HTML file.

: _separate ( -- )
  separate? @ if  echo_space  then  separate? on ;
  \ Separate the current tag or word from the previous one, if needed.

: _echo ( ca len -- )
  _separate echo ;
  \ Print a text string to the HTML file, with a previous space if needed.

: ?_echo ( ca len f -- )  \ XXX used?
  if  _echo  else  2drop  then ;

: echo_line ( ca len -- )
  echo echo_cr ;

: ?echo_line ( ca len f -- )
  if  echo_line  else  2drop  then ;

: echo. ( n -- )
  n>str echo ;

: _echo. ( n -- )
  n>str echo_space echo ;

: +echo ( ca len -- )
  separate? @ >r  separate? off echo  r> separate? ! ;

.( fendo.echo.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-06-04: Start.
\
\ 2013-06-08: New: First code for output redirection.
\
\ 2013-06-29: Change: 'target_fid' moved here from <fendo_files.fs>.
\
\ 2013-07-03: Change: 'dry?' renamed to 'echo>screen?'.
\
\ 2013-07-03: New: tools to redirect the output to a dynamic string.
\
\ 2013-07-12: New: '?_echo' moved here from <fendo_markup_wiki.fs>.
\
\ 2013-07-14: New: '?echo_line'.
\
\ 2013-07-21: New: 'echo.'. This somehow fixes the print corruption
\ caused by using 's>d <# #s #> echo' in the HTML template.
\
\ 2013-07-21: New: 'echo_line'.
\
\ 2013-11-07: New: '_echo.'.
\
\ 2013-11-26: Change: 'n>str' instead of '(echo.)'.
\
\ 2013-12-06: Fix: 'echo_cr' now does 'separate? off' in order to
\ remove unnecessary blank spaces.
\
\ 2014-02-15: New: 'echo_period'.
\
\ 2014-03-11: New: '+echo', experimental, factored from user macros.
\
\ 2014-11-01: Fix: now '+echo' preserves 'separate?'.
\
\ 2014-11-17: Fix: 'save_echo' and 'restore_echo' didn't activate
\ 'echo_stack'.
\
\ 2014-12-13: New: 'compact_html?' flag (experimental).
\
\ 2015-10-15: Updated the name of the Galope module <n-to-str.fs>.
\
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
