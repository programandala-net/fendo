.( fendo.markup.common.fs ) cr

\ This file is part of Fendo.

\ This file defines some common words for the markup definitions, also
\ required by the links module.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ 2014-03-04: Start. Code extracted from <fendo.markup.fs>.
\ 2014-03-04: Change: 'xhtml?' moved to <fendo.config.fs>.
\ 2014-03-04: Change: parser vectors moved here from
\   <fendo.markup.wiki.fs>.

\ **************************************************************
\ Requirements

forth_definitions
require galope/unhtml.fs
fendo_definitions

\ **************************************************************
\ Parser vectors

\ Some words defined in <fendo.parser.fs> will be needed earlier.

defer content  ( ca len -- )
  \ Manage a string of content: print it and update the counters.
defer evaluate_content  ( ca len -- )
  \ Evaluate a string as page content.
\ defer close_pending  ( -- ) \ xxx tmp
  \ Close the pending markups.

\ **************************************************************
\ Common tools

: :echo_name   ( ca len -- )
  \ Create a word that prints its own name.
  \ ca len = word name
  2dup :create  s,
  does>  ( -- ) ( dfa )  count echo
  ;
: :echo_name_   ( ca len -- )
  \ Create a word that prints its own name
  \ and forces separation from the following text.
  \ ca len = word name
  2dup :create  s,
  does>  ( -- ) ( dfa )  count echo  separate? on
  ;
: :echo_name+   ( ca len -- )
  \ Create a word that prints its own name
  \ and forces the following text will not be separated.
  \ ca len = word name
  2dup :create  s,
  does>  ( -- ) ( dfa )  count _echo  separate? off
  ;
variable header_cell?  \ flag, is it a header cell the latest opened cell in the table?
variable table_started?  \ flag, is a table open?

variable execute_markup?  \ flag, execute the markup while parsing?
execute_markup? on  \ execute by default; otherwise print it
variable preserve_eol?  \ flag, preserve in the target the end of lines of the source?
variable forth_code_depth  \ depth level of the parsed Forth code block?

: (unmarkup)  ( ca len -- ca' len' )
  \ Remove the markup of a string.
  \ The method used is to convert it to HTML and then remove the HTML,
  \ easier than removing all possible original markups.
  \ Gforth's 'save-mem' copies a memory block into a newly allocated
  \ region in the heap. It is needed because the region used by 'echoed'
  \ will be restored at the end of 'unmarkup'.
  \ XXX TODO remove double spaces?
  \ XXX FIXME this sometimes blanks the 'href=' attribute, why?
  ." parameter in (unmarkup) = " 2dup type cr  \ xxx informer
  s" href=@" evaluate ." href (before 'evaluate_content') = " type cr  \ xxx informer
  \ XXX FIXME the problem happens when there are markups in the
  \ parameter string; something goes wrong in the complex 'evaluate_content'.
  echo>string evaluate_content
  s" href=@" evaluate ." href  (after 'evaluate_content') = " type ." <<< PROBLEM" cr  \ xxx informer
  \ XXX FIXME the problem is the set of attributes has been changed...
  \ ...and not restored:
  s" >attributes< href=@ >attributes<" evaluate ." alternative href = " type ."  <<< it is in the alternative set!" cr  \ xxx informer
  echoed $@
  save-mem  \ XXX TODO needed?
  unhtml
  ;
\ variable (echo>)
\ variable (echoed)
: unmarkup  ( ca len -- ca' len' )
  \ Remove the markup of a string; the current status of 'echo' is preserved.
  \ The method used is to convert it to HTML and then remove the HTML,
  \ easier than removing all possible original markups.
  save_echo (unmarkup) restore_echo
  ;

.( fendo.markup.common.fs compiled) cr


