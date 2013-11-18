.( fendo_files.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

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

\ 2013-10-01 File created for the page redirection tools; 'open_target' and
\ 'close_target' are moved here from "fendo_parser.fs".
\ 2013-10-02 Page redirection tools.
\ 2013-11-18 New: 'file>local', factored from 'open_source_code',
\   (defined in <addons/source_code.fs>).

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

\ **************************************************************
\ Files

: file>local ( ca1 len1 -- ca2 len2 )
  \ ca1 len1 = file name
  \ ca2 len2 = file name with local path
  2>r target_dir $@ files_subdir $@ s+ 2r> s+
  ;

\ **************************************************************
\ Redirection

: (redirected) ( fid -- )
  \ Create the content of a file that redirects to the current page.
  >r  s" <?php" r@ write-line throw
	s" header('HTTP/1.1 301 Moved Permanently');" r@ write-line throw
	s" header('Status: 301 Moved Permanently');" r@ write-line throw
	S" header('Location: http://'." 
  s" ($_SERVER['HTTP_HOST']=='localhost'?'localhost/':'')" s+
  s" .'" s+
\	S" header('Location: http://localhost/"  \ xxx debugging
  domain $@ s" /" s+ current_target_file s+ s+
  s" ');" s+ r@ write-line throw
	s" exit(0);" r@ write-line throw
	s" ?>" r> write-line throw
	;
: redirected ( ca len -- )
  \ Create a file that redirects to the current page.
  \ ca len = old page, without domain
  \ 2013-10-02 Start, based on code from ForthCMS.
  +target_dir w/o create-file throw  dup (redirected)  close-file throw
  ;
: redirect ( "old_page" -- )
  \ Create a file that redirects to the current page.
  parse-name redirected
  ;

\ **************************************************************
\ Read source code

s" /counted-string" environment? 0=
[if]  255  [then]  dup constant /source_line
2 chars + buffer: source_line

: read_fid_line  { fid -- ca len wf }
  \ Get a line from the given file identifier.
  source_line dup /source_line fid read-line throw
  ;
: read_source_line  ( -- ca len wf )
  \ Get a line from the current source.
  source-id read_fid_line
  ;

.( fendo_files.fs compiled ) cr
