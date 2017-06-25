.( fendo.files.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the file tools.

\ Last modified 201706260028.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2015,2017 Marcos Cruz (programandala.net)

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
require galope/string-suffix-question.fs
fendo_definitions

\ ==============================================================
\ Target file

: (open_target) ( -- )
  current_page
\  cr ." current_page in (open_target) =  " dup .  \ XXX INFORMER
  target_path/file
\  cr ." target file =  " 2dup type cr key drop  \ XXX INFORMER
  w/o create-file throw target_fid !
\  ." target file just opened: "  \ XXX INFORMER
\  target_fid @ . cr  \ XXX INFORMER
\  s" <!-- XXX -->" target_fid @ write-line throw  \ XXX debugging
  ;
  \ Open the target file (HTML or Atom).

: open_target ( -- )
  echo>file? if  (open_target)  then ;
  \ Open the target file (HTML or Atom), if needed.

: complete_iso_date ( ca1 len1 -- ca2 len2 )
  dup case
    10 of exit          endof
     7 of s" -01" s+    endof
     4 of s" -01-01" s+ endof
     abort" Wrong ISO date format"
  endcase ;

: (set_file_mtime) ( ca1 len1 ca2 len2 -- )
  complete_iso_date
  \ 2dup cr ." Net ISO date = " type \ XXX INFORMER
  s\" touch --date=\"" 2swap s+ s\" \" " s+ 2swap s+
  system $? abort" Error in set_modification_time" ;
  \ Set the modification time of file _ca1 len1_ to ISO date _ca2
  \ len2_.  The host operating system shell is used.

: set_file_mtime ( ca1 len1 ca2 len2 -- )
  \ 2dup cr ." Raw ISO date = " type \ XXX INFORMER
  dup if (set_file_mtime) else 2drop 2drop then ;
  \ If _ca2 len2_ is not empty,
  \ set the modification time of file _ca1 len1_ to ISO date _ca2
  \ len2_.

false [if]  \ XXX OLD

: set_modification_time ( ca len -- )
  2>r s" touch --date=" current_page file_mtime s+ s"  " s+ 2r> s+
  system $? abort" Error in set_modification_time" ;
  \ Set the modification time of the given file
  \ to the correspondent metadata of the current page.
  \ The host operating system shell is used.

[endif]

: set_modification_time ( ca len -- )
  current_page file_mtime set_file_mtime ;
  \ Set the modification time of the given file
  \ to the correspondent metadata of the current page.
  \ The host operating system shell is used.

: set_current_target_modification_time ( -- )
  current_page target_path/file set_modification_time ;
  \ Set the modification time of the current target file
  \ to the correspondent metadata of the source file.
  \ The host operating system shell is used.

: (close_target) ( -- )
\  depth abort" stack not empty"  \ XXX INFORMER
  target_fid @ close-file throw
\  ." target_fid just closed. " \ XXX INFORMER
  target_fid off  set_current_target_modification_time ;
  \ Close the target file (HTML or Atom).

: close_target ( -- )
\  ." close_target" cr \ XXX INFORMER
  target_fid @ if  (close_target)  then ;
  \ Close the target file (HTML or Atom), if needed.

\ ==============================================================
\ Files

: file>local ( ca1 len1 -- ca2 len2 )
  2>r target_dir $@ files_subdir $@ s+ 2r> s+ ;
  \ Add local path to filename _ca1 len1_, resulting
  \ _ca2 len2_.

\ ==============================================================
\ Redirection

: ((redirected)) ( fid -- )
  >r  s" <?php" r@ write-line throw
  s" header('HTTP/1.1 301 Moved Permanently');" r@ write-line throw
  s" header('Status: 301 Moved Permanently');" r@ write-line throw
  S" header('Location: http://'."
  s" ($_SERVER['HTTP_HOST']=='localhost'?'localhost/':'')" s+
  s" .'" s+
  domain&current_target_file s+
  s" ');" s+ r@ write-line throw
  s" exit(0);" r@ write-line throw
  s" ?>" r> write-line throw ;
  \ Create the content of a file that redirects to the current page.

: ?+redirected_extension ( ca1 len1 -- ca2 len2 )
  2dup s" .html" string-suffix? ?exit
  2dup s" .php" string-suffix? ?exit
  2dup s" .htm" string-suffix? ?exit
  html_extension $@ s+ ;
  \ Make sure _ca1 len1_, which is a page id (old page filename
  \ without path and extension) or a page filename (with html, htm or
  \ php extensions), have a filename extension. If not, add the
  \ default HTML extension.

: redirected>target ( ca1 len1 -- ca2 len2 )
  ?+redirected_extension +target_dir ;
  \ Convert an old page id (whose filename does not exist any more)
  \ to its correspondent target filename.
  \ The default target extension is assumed.
  \ ca1 len1 = page id (old page filename without path and extension)
  \            or page filename (with html, htm or php extensions)
  \ ca2 len2 = target filename with path

\ The file modification time of a redirected file can be
\ the modification or creation dates of the page they point to.

defer redirection_date ( a1 -- a2 )

: modified>redirection ( -- )
  ['] file_mtime is redirection_date ;
  \ The file modification time of the next redirected file will
  \ be the modification date of the current page.
  \ The calculated datum 'file_mtime' is used instead of 'modified',
  \ because it gives more control to the user.

: created>redirection ( -- )
  ['] created is redirection_date ;
  \ The file modification time of the next redirected file will
  \ be the creation date of the current page.

\ The default is the 'created' date. This way the redirected files
\ are not uploaded during server updates.

created>redirection

: (redirected) ( ca len -- )
  redirected>target 2dup 2>r w/o create-file throw  dup ((redirected))  close-file throw
  2r> current_page redirection_date set_file_mtime
  created>redirection ; \ restore the default
  \ Create a file that redirects to the current page.
  \ ca len = page id (old page filename without path and extension)
  \          or page filename (with html, htm or php extensions)
  \ 2013-10-02 Start, based on code from ForthCMS.

: redirected ( ca len -- )
  current_page draft? if  2drop  else  (redirected)  then ;
  \ Create a file that redirects to the current page, if possible.
  \ ca len = page id (old page filename without path and extension)
  \          or page filename (with html, htm or php extensions)

: redirect ( "name" -- )
  parse-name redirected ;
  \ Create a file that redirects to the current page, if possible.
  \ "name" = page id (old page filename without path and extension)
  \          or page filename (with html, htm or php extensions)

: new_redirected ( ca len -- )
  current_page draft? if  2drop  else  (redirected)  then ;
  \ Create a file that redirects to the current page, if possible.
  \ ca len = page id (old page filename without path and extension)
  \          or page filename (with html, htm or php extensions)
  \ The date of the redirection file will be the modification date
  \ of the current page.

: new_redirect ( "name" -- )
  modified>redirection redirect ;
  \ Create a file that redirects to the current page, if possible.
  \ The date of the redirection file will be the modification date
  \ of the current page.
  \ "name" = page id (old page filename without path and extension)
  \          or page filename (with html, htm or php extensions)

\ ==============================================================
\ Read source code

s" /counted-string" environment? 0=
[if]  255  [then]  dup constant /source_line
2 chars + buffer: source_line

: read_fid_line { fid -- ca len f }
  source_line dup /source_line fid read-line throw ;
  \ Get a line from the given file identifier.

: read_source_line ( -- ca len f )
  source-id read_fid_line ;
  \ Get a line from the current source.

.( fendo.files.fs compiled ) cr

\ ==============================================================
\ Change log

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
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-06-24: Improve `redirected>target`: add the HTML filename
\ extension only when the input string has no recognized extension.
\ Improve `set_file_mtime`: complete the ISO date if needed.  Improve
\ comment.
\
\ 2017-06-25: Improve `set_file_mtime`: do nothing if the date string
\ is empty.

\ vim: filetype=gforth
