.( fendo_files.fs ) 

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-01.

\ This file defines the file tools.

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

\ **************************************************************

\ Target file

: (open_target)  ( -- )
  \ Open the target HTML page file.
  current_page
\  cr ." current_page in (open_target) =  " dup .  \ xxx informer
  target_path/file
\  cr ." target file =  " 2dup type cr key drop  \ xxx informer
  w/o create-file throw target_fid !
\  ." target file just opened: "  \ xxx informer
\  target_fid @ . cr  \ xxx informer
\  s" <!-- xxx -->" target_fid @ write-line throw  \ xxx debugging
  ;
: open_target  ( -- )
  \ Open the target HTML page file, if needed.
  echo>file? if  (open_target)  then
  ;
: (close_target)  ( -- )
  \ Close the target HTML page file.
\  depth abort" stack not empty"  \ xxx informer
  target_fid @ close-file throw
\  ." target_fid just closed. " \ xxx informer
  target_fid off
  ;
: close_target  ( -- )
  \ Close the target HTML page file, if needed.
\  ." close_target" cr \ xxx informer
  target_fid @ if  (close_target)  then
  ;


0 [if]

: redirection{  ( -- )

	\ 2007-11-29 Improved with more detailed PHP code.
	\ 2009-01-01 Updated with domain vivirenbicicleta.info.
  \ 2013-05-07 Improved: the domain is not hardcoded anymore,
  \   but the config value. Renamed (formerly 'php-code{').

	S" <?php" >>html
	S" header( 'HTTP/1.1 301 Moved Permanently' );" >html
	S" header( 'Status: 301 Moved Permanently' );" >html
  \ old:
	\ S" header('Location: http://vivirenbicicleta.info/" >html
  \ 2012-07-31 new, localhost support:
	S" header('Location: http://'." >html
  S" ($_SERVER['HTTP_HOST']=='localhost'?'localhost/':'')" >>html
  S" .'" >>html "site-domain" >>html S" /" >>html

	;

: }redirection  ( -- )

	\ 2007-11-29 Improved with more detailed PHP code.
  \ 2013-05-07 Renamed (formerly '}php-code').

	S" ');" >>html
	S" exit(0);" >html
	S" ?>" >html

	;
[then]
