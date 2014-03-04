.( fendo.addon.tags.fs) cr

\ This file is part of Fendo.

\ This file creates the tools needed to use page tags.

\ Copyright (C) 2014 Marcos Cruz (programandala.net)

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

\ 2014-03-02: Start.
\ 2014-03-03: First draft.

\ **************************************************************
\ Tags wordlist

\ xxx todo finish

wordlist constant fendo_tags_wid

: tags>order  ( -- )
  fendo_tags_wid >order
  ;
: [tags>order]  ( -- )
  tags>order
  ;  immediate

\ **************************************************************
\ Tags data

\ The body of a tag word holds three data:
\ +0       = counter (how many times the tag is used in a set of pages)
\ +1 cell  = its own name token
\ +2 cells = counted string of the tag text

: tag>name  ( dfa -- ca len )
  cell+ @ name>string
  ;
: tag>text  ( dfa -- ca len )
  2 cells + count
  ;
: (tag>pid$)  ( ca1 len1 -- ca2 len2 )
  \ Default conversion from tag to pid.
  \ ca1 len1 = tag name
  \ ca2 len2 = corresponding page name
  \ xxx todo
  ;
defer tag>pid$  ( ca1 len1 -- ca2 len2 )
  \ Actual conversion from tag to pid,
  \ to be configured by the application.
' (tag>pid$) is tag>pid$

\ **************************************************************
\ Possible behaviours of the tags

: (tag_does_reset)  ( -- )
  ( dfa ) off
  ;
: (tag_does_increase)  ( -- )
  ( dfa ) 1 swap +!
  ;
: (tag_does_total)  ( -- +n )
  ( dfa ) @
  ;
: (tag_does_name)  ( -- ca len )
  ( dfa ) tag>name
  ;
: (tag_does_text)  ( -- ca len )
  ( dfa ) tag>text
  ;
: ((tag_does_link))  ( -- )
  \ Default creation of a tag link.
  \ xxx todo
  ( dfa ) dup (tag_does_text)
  ;
defer (tag_does_link)  ( -- ) ( dfa )
  \ Actual creation of a tag link,
  \ to be configured by the application.
' ((tag_does_link)) is (tag_does_link)

defer (tag_does)  \ current behaviour of the tags

\ **************************************************************
\ Choosing the behaviour of the tags

export

: tags_do_nothing  ( -- )
  \ Set the tags to do nothing (just return their dfa).
  ['] noop is (tag_does)
  ;
: tags_do_reset  ( -- )
  \ Set the tags to reset their counts.
  ['] (tag_does_reset) is (tag_does)
  ;
: tags_do_increase  ( -- )
  \ Set the tags to increase their counts.
  ['] (tag_does_increase) is (tag_does)
  ;
: tags_do_total  ( -- )
  \ Set the tags to return their counts.
  ['] (tag_does_total) is (tag_does)
  ;
: tags_do_text  ( -- )
  \ Set the tags to return their texts.
  ['] (tag_does_text) is (tag_does)
  ;
: tags_do_name  ( -- )
  \ Set the tags to return their names.
  ['] (tag_does_name) is (tag_does)
  ;
: tags_do_link  ( -- )
  \ Set the tags to create links.
  ['] (tag_does_link) is (tag_does)
  ;

\ **************************************************************
\ Create new tags

: tag  ( ca len "name" -- )
  \ Create a new tag.
  \ ca len = tag text name, to be used in links
  \ "name" = tag word name, to be used in URL
  \ Example usage:
  \   s" ZX Spectrum" tag zx_spectrum
  get-current >r  fendo_tags_wid set-current
  create
    \ The body holds a counter, the name token and the text
    0 , latestxt >name , s,
  r> set-current
  does>   ( -- ) ( dfa ) (tag_does)
  ;

\ **************************************************************
\ Traverse the tags

s" /tmp/fendo.tags.fs" 2constant tags_filename$
: tag_words  ( -- )
  \ List all tags.
  tags>order words previous
  ;
: create_tags_file  ( -- )
  \ Create a temporary file with the list of all tags.
  ['] tag_words 
  tags_filename$ w/o create-file throw dup >r outfile-execute
  r> close-file throw
  ;
: execute_tags  ( -- )
  \ Execute all tags with their current behaviour.
  create_tags_file  tags_filename$ slurp-file evaluate
  ;
: traverse_tags  ( xt -- )
  \ Execute all tags with the given behaviour.
  is (tag_does)  execute_tags
  ;

.( fendo.addon.tags.fs compiled) cr

