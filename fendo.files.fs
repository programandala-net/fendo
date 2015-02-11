.( fendo.files.fs ) cr

\ This file is part of Fendo.

\ This file defines the file tools.

\ Copyright (C) 2013,2014,2015 Marcos Cruz (programandala.net)

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
\ History of this file

\ See at the end of the file.

\ **************************************************************
\ Target file

: (open_target)  ( -- )
  \ Open the target file (HTML or Atom).
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
  \ Open the target file (HTML or Atom), if needed.
  echo>file? if  (open_target)  then
  ;
: set_file_mtime  ( ca1 len1 ca2 len2 -- )
  \ Set the modification time of the given file.
  \ ca1 len1 = filename
  \ ca2 len2 = ISO date
  \ The host operating system shell is used.
  s\" touch --date=\"" 2swap s+ s\" \" " s+ 2swap s+
  system $? abort" Error in set_modification_time"
  ;
false [if]  \ XXX OLD
: set_modification_time  ( ca len -- )
  \ Set the modification time of the given file
  \ to the correspondent metadata of the current page.
  \ The host operating system shell is used.
  2>r s" touch --date=" current_page file_mtime s+ s"  " s+ 2r> s+
  system $? abort" Error in set_modification_time"
  ;
[endif]
: set_modification_time  ( ca len -- )
  \ Set the modification time of the given file
  \ to the correspondent metadata of the current page.
  \ The host operating system shell is used.
  current_page file_mtime set_file_mtime
  ;
: set_current_target_modification_time  ( -- )
  \ Set the modification time of the current target file
  \ to the correspondent metadata of the source file.
  \ The host operating system shell is used.
  current_page target_path/file set_modification_time
  ;
: (close_target)  ( -- )
  \ Close the target file (HTML or Atom).
\  depth abort" stack not empty"  \ xxx informer
  target_fid @ close-file throw
\  ." target_fid just closed. " \ xxx informer
  target_fid off  set_current_target_modification_time
  ;
: close_target  ( -- )
  \ Close the target file (HTML or Atom), if needed.
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

: ((redirected)) ( fid -- )
  \ Create the content of a file that redirects to the current page.
  >r  s" <?php" r@ write-line throw
  s" header('HTTP/1.1 301 Moved Permanently');" r@ write-line throw
  s" header('Status: 301 Moved Permanently');" r@ write-line throw
  S" header('Location: http://'."
  s" ($_SERVER['HTTP_HOST']=='localhost'?'localhost/':'')" s+
  s" .'" s+
  domain&current_target_file s+
  s" ');" s+ r@ write-line throw
  s" exit(0);" r@ write-line throw
  s" ?>" r> write-line throw
  ;
: redirected>target  ( ca1 len1 -- ca2 len2 )
  \ Convert an old page id (whose filename does not exist any more)
  \ to its correspondent target filename.
  \ The default target extension is assumed.
  \ ca1 len1 = page id (old page filename without path and extension)
  \ ca2 len2 = target filename with path
  html_extension $@ s+ +target_dir
  ;

\ The file modification time of a redirected file can be
\ the modification or creation dates of the page they point to.
defer redirection_date  ( a1 -- a2 )
: modified>redirection  ( -- )
  \ The file modification time of the next redirected file will
  \ be the modification date of the current page.
  \ The calculated datum 'file_mtime' is used instead of 'modified',
  \ because it gives more control to the user.
  ['] file_mtime is redirection_date
  ;
: created>redirection  ( -- )
  \ The file modification time of the next redirected file will
  \ be the creation date of the current page.
  ['] created is redirection_date
  ;

\ The default is the 'created' date. This way the redirected files
\ are not uploaded during server updates.
created>redirection

: (redirected) ( ca len -- )
  \ Create a file that redirects to the current page.
  \ ca len = page id (old page filename without path and extension)
  \ 2013-10-02 Start, based on code from ForthCMS.
  redirected>target 2dup 2>r w/o create-file throw  dup ((redirected))  close-file throw
  2r> current_page redirection_date set_file_mtime
  created>redirection  \ restore the default
  ;
: redirected ( ca len -- )
  \ Create a file that redirects to the current page, if possible.
  \ ca len = page id (old page filename without path and extension)
  current_page draft? if  2drop  else  (redirected)  then
  ;
: redirect ( "name" -- )
  \ Create a file that redirects to the current page, if possible.
  \ "name" = page id (old page filename without path and extension)
  parse-name redirected
  ;
: new_redirected ( ca len -- )
  \ Create a file that redirects to the current page, if possible.
  \ ca len = page id (old page filename without path and extension)
  \ The date of the redirection file will be the modification date
  \ of the current page.
  current_page draft? if  2drop  else  (redirected)  then
  ;
: new_redirect ( "name" -- )
  \ Create a file that redirects to the current page, if possible.
  \ The date of the redirection file will be the modification date
  \ of the current page.
  \ "name" = page id (old page filename without path and extension)
  modified>redirection redirect
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

\ **************************************************************
\ History of this file

\ 2013-10-01: File created for the page redirection tools;
\ 'open_target' and 'close_target' are moved here from
\ "fendo_parser.fs".
\
\ 2013-10-02: Page redirection tools.
\
\ 2013-11-18: New: 'file>local', factored from 'open_source_code',
\ (defined in <addons/source_code.fs>).
\
\ 2013-11-28: Fix: 'redirected' didn't add the target extension, only
\ the path; fixed with the new word 'pid$>target'.
\
\ 2014-03-02: Change: 'domain&current_target_file', factored from
\ '(redirected)' to <fendo.data.fs>.
\
\ 2014-07-13: New: 'set_current_target_modification_time'.
\
\ 2014-11-04: Improvement: 'redirected' set the file modification time
\ of the redirected file with the correspondent metadadatum of its
\ goal page. This way server updates will be easier.
\ 'set_current_target_modification_time ' was factored out.
\
\ 2015-01-14: Improvement: 'set_modification_time' uses the new
\ calculated datum 'file_mtime'.
\
\ 2015-01-14: Fix: 'redirected' created the redirected file even when
\ the current page is a draft.
\
\ 2015-01-14: Improvement: 'set_modification_time' is factored out to
\ 'set_file_mtime', that receives also the desired date. This lets the
\ redirection files to be set to the date the original was created.
\ The words for redirection are updated accordingly.
\
\ 2015-01-31: Change: there were two related words called
\ '(redirected)'. Although this caused no problem but a warning
\ message during compilation, for clarity the deeper one has been
\ renamed to '((redirected))'.
\
\ 2015-02-11: Typos.

.( fendo.files.fs compiled ) cr
