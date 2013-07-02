.( fendo_echo.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-01.

\ This file defines the words that print to the target HTML file.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ **************************************************************
\ Change history of this file

\ 2013-06-04 Start.
\ 2013-06-08 New: First code for output redirection.
\ 2013-06-29 Change: 'target_fid' moved here from <fendo_files.fs>.
\ 2013-07-03 Change: 'dry?' renamed to 'echo>screen?'.
\ 2013-07-03 New: tools to redirect the output to a dynamic
\   string.

\ **************************************************************
\ Output

variable echo>screen?  \ flag, don't create the target files, echo to the screen instead
variable echo>string?  \ flag, temporarily echo to the 'echoed' string.
variable echoed  \ used as dynamic string

: echo>string  ( -- )
  \ Redirect the output to the dynamic string 'echoed'.
  echo>string? on
  0 echoed $!len
  ;
: echo>file  ( -- )
  \ Redirect the output to the target file.
  echo>string? off
  echo>screen? off
  ;
: echo>screen  ( -- )
  \ Redirect the output to the target file.
  echo>string? off
  echo>screen? on
  ;

echo>file

\ **************************************************************
\ Echo

variable target_fid  \ file id of the HTML target page

: (echo)  ( xt | ca len xt -- )
  echo>screen? @
  if    execute
  else  target_fid @ outfile-execute
  then
  ;
: (echo>string)  ( ca len -- )
  \ Add a string to to the 'echoed' string.
  echoed $+! 
  ;
: echo  ( ca len -- )
  \ Print a text string to the HTML file.
  echo>string? @
  if    (echo>string)
  else  ['] type (echo)
  then
  ;
: echo_cr  ( -- )
  \ Print a carriage return to the HTML file.
  echo>string? @
  if    s\" \n"  (echo>string)
  else  ['] cr (echo)
  then
  ;

: echo_space  ( -- )
  \ Print a space to the HTML file.
  s"  " echo
  ;
: echo_quote  ( -- )
  \ Print a double quote to the HTML file.
  s\" \"" echo 
  ;
variable separate?  \ flag: separate the next tag or word from the current one?
: _separate  ( -- )
  \ Separate the current tag or word from the previous one, if needed.
  separate? @ if  echo_space  then  separate? on
  ;
: _echo  ( ca len -- ) 
  \ Print a text string to the HTML file, with a previous space if needed.
  _separate echo 
  ;

.( fendo_echo.fs compiled) cr


