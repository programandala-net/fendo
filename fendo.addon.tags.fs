.( fendo.addon.tags.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file creates the tools needed to use page tags.

\ Last modified 202004220005.
\ See change log at the end of the file.

\ Copyright (C) 2014,2017,2018,2020 Marcos Cruz (programandala.net)

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
\ Stack notation

\ In this file, the stack notation "tag" represents the body (the data
\ field address) of a tag word. Tag words are created by `create`.

\ ==============================================================
\ Requirements

forth_definitions
require galope/n-to-r.fs \ `n>r`
require galope/n-r-from.fs \ `nr<`
require galope/one-plus-store.fs \ `1+!`
require galope/s-constant.fs \ `sconstant`
fendo_definitions

\ ==============================================================
\ Tags wordlist

wordlist constant fendo_tags_wid

: tags>order ( -- )
  fendo_tags_wid >order ;

: tags<order ( -- )
  previous ;

: [tags>order] ( -- )
  tags>order ; immediate

: [tags<order] ( -- )
  tags<order ; immediate

: tags_order ( -- wid 1 )
  fendo_tags_wid 1 ;
  \ Return the wordlist order required to parse markup.

: set_tags_order ( -- )
  tags_order set-order ;

\ ==============================================================
\ Tag data

\ The body of a tag word holds three data:
\ +0       = counter (how many times the tag is used in a set of pages)
\ +1 cell  = its own name token
\ +2 cells = counted string of the tag text

: tag>count ( tag -- a )
  ; immediate
  \ Syntactic sugar, just in case the data are reorganized.

: tag>name ( tag -- ca len )
  cell+ @ name>string ;

: tag>text ( tag -- ca len )
  3 cells + count ;

: tag>own_page ( tag -- a )
  2 cells + ;

\ Counters used to mark the last element of a tag list, in order to
\ make it easier to manipulate the list with CSS:

variable #tags  \ number of evaluated tags (how many tags were evaluated)
variable #tag   \ number of the tag that is being evaluated

defer tags_url_section$  \ to be set by the application
s" tag." sconstant (tags_url_section$)
' (tags_url_section$) is tags_url_section$  \ default

: (tag>pid$) ( tag -- ca len )
  tags_url_section$ rot tag>name s+ ;
  \ Default conversion from tag to page ID.

defer tag>pid$ ( tag -- ca len )
' (tag>pid$) is tag>pid$
  \ Actual conversion from tag to page ID,
  \ to be configured by the application.

\ ==============================================================
\ Possible behaviours of the tags

: ((tag_link)) ( tag ca len -- )
\  2dup cr type  \ XXX INFORMER
  rot tag>text link ;
  \ ca len = page ID

: (tag_link) ( tag -- )
  dup tag>pid$ ((tag_link)) ;
  \ Create a link to the given tag.

: tag_link ( tag -- )
  >r get-order set_fendo_order  r> (tag_link)  set-order ;
  \ Create a link to the given tag.

: (tag_does_reset) ( tag -- )
  tag>count off ;
  \ Reset the given tag.

: (tag_does_increase) ( tag -- )
  tag>count 1+! ;
  \ Increase the count of the given tag.

: (tag_does_total) ( tag -- +n )
  tag>count @ ;
  \ Count of the given tag.

: (tag_does_name) ( tag -- ca len )
  tag>name ;
  \ Name of the given tag.

: (tag_does_text) ( tag -- ca len )
  tag>text ;
  \ Text of the given tag.

: (tag_does_link) ( tag -- )
  tag_link ;
  \ Create a link to the page of the given tag.

: last_listed_link? ( -- f )
  1 #tag +!  #tag @ #tags @ = ;

: (tag_does_list_link) ( tag -- )
  last_listed_link? if  s" last" class=!  then  [<li>] tag_link [</li>] ;
  \ Create a list element with a link to the page of the given tag.
  \ Note: Before creating the link list,
  \ the `#tag` variable must be set to zero, and
  \ the `#tags` variable must be calculated.
  \ See the definition of `tag_list` in <fendo.addon.tag_list.fs>
  \ as as example how a tag link list is built.

variable tag_searched_for$

variable tag_presence  \ counter

: (tag_does_presence) ( tag -- )
  tag>name tag_searched_for$ $@ str= abs tag_presence +! ;

: (tag_does_count) ( tag -- )
  drop  1 #tags +! ;
  \ Increase the count of evaluated tags.

defer (tag_does)  \ current behaviour of the tags

\ ==============================================================
\ Choosing the behaviour of the tags

: tags_do_nothing ( -- )
  ['] drop is (tag_does) ;
  \ Set the tags to do nothing.

: tags_do_body ( -- )
  ['] noop is (tag_does) ;
  \ Set the tags to return their body addresses.

: tags_do_reset ( -- )
  ['] (tag_does_reset) is (tag_does) ;
  \ Set the tags to reset their counts.

: tags_do_increase ( -- )
  ['] (tag_does_increase) is (tag_does) ;
  \ Set the tags to increase their counts.

: tags_do_total ( -- )
  ['] (tag_does_total) is (tag_does) ;
  \ Set the tags to return their counts.

: tags_do_text ( -- )
  ['] (tag_does_text) is (tag_does) ;
  \ Set the tags to return their texts.

: tags_do_name ( -- )
  ['] (tag_does_name) is (tag_does) ;
  \ Set the tags to return their names.

: tags_do_link ( -- )
  ['] (tag_does_link) is (tag_does) ;
  \ Set the tags to create links.

: tags_do_list_link ( -- )
  ['] (tag_does_list_link) is (tag_does) ;
  \ Set the tags to create listed links.

: tags_do_presence ( ca len -- )
  tag_searched_for$ $!   tag_presence off
  ['] (tag_does_presence) is (tag_does) ;
  \ Set the tags to check if their name is the given name.
  \ ca len = tag name

: tags_do_count ( -- )
  #tags off  ['] (tag_does_count) is (tag_does) ;
  \ Set the tags to count how many tags are executed.
  \ Output will be in the `#tags` variable.

\ ==============================================================
\ Create new tags

: tag ( ca len "name" -- )
  get-current >r  fendo_tags_wid set-current
  create
    \ The body holds a counter, the name token, the text,
    \ and the last own page ID ( XXX not used yet)
    0 ,  \ tag counter
    latestxt >name ,  \ name token
    0 , \ dynamic string variable, page ID of the (last) own page
    s,  \ text
  r> set-current
  does>  ( -- ) ( dfa ) (tag_does) ;
  \ Create a new tag.
  \ ca len = tag text name, to be used in link text
  \ "name" = tag word name, to be used in URL
  \ Example usage:
  \   s" ZX Spectrum" tag zx_spectrum

\ ==============================================================
\ Traverse the tags

s" /tmp/fendo.tags.fs" sconstant tags_filename$

: tag_words ( -- )
  tags>order words tags<order ;
  \ List all tags.

: create_tags_file ( -- )
  ['] tag_words
  tags_filename$ w/o create-file throw dup >r outfile-execute
  r> close-file throw ;
  \ Create a temporary file with the list of all tags.

: evaluate_tags ( ca len -- )
\  ." evaluate_tags " 2dup type cr ~~  \ XXX INFORMER
  get-order n>r set_tags_order  evaluate  nr> set-order ;

: check_tags ( ca len -- )
  ['] (tag_does) defer@ >r tags_do_nothing evaluate_tags
                        r> is (tag_does) ;

: (execute_all_tags) ( ca len -- )
  tags>order evaluate  previous ;

: execute_all_tags ( -- )
\  ." execute_all_tags" cr  \ XXX INFORMER
  create_tags_file  tags_filename$ slurp-file
  2dup check_tags (execute_all_tags)
\  ." execute_all_tags end" cr  \ XXX INFORMER
  ;
  \ Execute all tags with their current behaviour.

: traverse_tags ( xt -- )
  is (tag_does)  execute_all_tags ;
  \ Execute all tags with the given behaviour.

.( fendo.addon.tags.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-03-02: Start.
\
\ 2014-03-03: First draft.
\
\ 2014-03-04: New: `evaluate_tags` and wid order words; words for
\ listed links.
\
\ 2014-03-04: Change: `execute_tags` renamed to `execute_all_tags`.
\
\ 2014-03-07: Change: `tag_link` factored from `(does_tag_link)`.
\
\ 2014-03-11: Fix: now `tag_link` sets the needed wordlist order; this
\ is needed because the order was changed before evaluating the tags.
\
\ 2014-05-28: New: `tags_used_only_once_link_to_its_own_page` flag.
\
\ 2104-05-28: Change: `(tag_link)` modified in order to implement
\ `tags_used_only_once_link_to_its_own_page`.
\
\ 2014-06-03: Change: `tags_used_only_once_link_to_its_own_page`
\ renamed to `lonely_tags_link_to_content`.
\
\ 2014-11-05: Improvement: In order to add a class to the last element
\ of a tag link list (what makes some things easier for CSS), new
\ words are added: `#tags`, `#tag`, `(tag_does_count)` and
\ `tag_do_count`.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-11-15: Update `++` to `1+!`.
\
\ 2018-09-27: Remove useless forgotten `export`, part of Galope's
\ module `module`, which is not used anymore by Fendo.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2020-04-14: Define strings constants with `sconstant` instead of
\ `2constant`.
\
\ 2020-04-21: Remove the `lonely_tags_link_to_content` flag variable
\ and all its related code. Its effect can be achievied with a
\ shortcut.

\ vim: filetype=gforth
