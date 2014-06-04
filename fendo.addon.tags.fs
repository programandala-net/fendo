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
\ 2014-03-04: New: 'evaluate_tags' and wid order words; words for
\   listed links.
\ 2014-03-04: Change: 'execute_tags' renamed to 'execute_all_tags'.
\ 2014-03-07: Change: 'tag_link' factored from '(does_tag_link)'.
\ 2014-03-11: Fix: now 'tag_link' sets the needed wordlist order;
\   this is needed because the order was changed before evaluating
\   the tags.
\ 2014-05-28: New: 'tags_used_only_once_link_to_its_own_page' flag.
\ 2104-05-28: Change: '(tag_link)' modified in order to implement
\   'tags_used_only_once_link_to_its_own_page'.
\ 2014-06-03: Change: 'tags_used_only_once_link_to_its_own_page'
\   renamed to 'lonely_tags_link_to_content'.

\ **************************************************************
\ Stack notation

\ In this file, the stack notation "tag" represents the body (the data
\ field address) of a tag word. Tag words are created by 'create'.

\ **************************************************************
\ Requirements

forth_definitions
require galope/n-to-r.fs  \ 'n>r'
require galope/n-r-from.fs  \ 'nr<'
require galope/plus-plus.fs  \ '++'
fendo_definitions

\ **************************************************************
\ Tags wordlist

wordlist constant fendo_tags_wid

: tags>order  ( -- )
  fendo_tags_wid >order
  ;
: tags<order  ( -- )
  previous
  ;
: [tags>order]  ( -- )
  tags>order
  ;  immediate
: [tags<order]  ( -- )
  tags<order
  ;  immediate
: tags_order  ( -- wid 1 )
  \ Return the wordlist order required to parse markup.
  fendo_tags_wid 1
  ;
: set_tags_order  ( -- )
  tags_order set-order
  ;

\ **************************************************************
\ Tag data

\ The body of a tag word holds three data:
\ +0       = counter (how many times the tag is used in a set of pages)
\ +1 cell  = its own name token
\ +2 cells = counted string of the tag text

: tag>count  ( tag -- a )
  \ Syntactic sugar, just in case the data are reorganized.
  ;  immediate
: tag>name  ( tag -- ca len )
  cell+ @ name>string
  ;
: tag>text  ( tag -- ca len )
  3 cells + count
  ;
: tag>own_page  ( tag -- a )
  2 cells +
  ;

defer tags_url_section$  \ to be set by the application
s" tag." 2constant (tags_url_section$)
' (tags_url_section$) is tags_url_section$  \ default
: (tag>pid$)  ( tag -- ca len )
  \ Default conversion from tag to pid.
  tags_url_section$ rot tag>name s+
  ;
defer tag>pid$  ( tag -- ca len )
  \ Actual conversion from tag to pid,
  \ to be configured by the application.
' (tag>pid$) is tag>pid$


\ **************************************************************
\ Possible behaviours of the tags

variable lonely_tags_link_to_content  \ flag

\ When 'lonely_tags_link_to_content' is on, the tag cloud
\ links of tags used only once are changed to its actual tagged page.
\ But this does not work for tag lists. The code required for tag list
\ would be much more complex. In order to achive the same effect in
\ tag lists, the user application can create a normal shorcut, e.g.:

\   shortcut: en.tag.dbase  s" en.program.my_database" href!  ;

\ In fact such a shortcut would have effect also in tag clouds, so the
\ current code triggered by 'lonely_tags_link_to_content'
\ is unnecessary.

: ((tag_link))  ( tag ca len -- )
  \ ca len = pid
  rot tag>text link
  ;
: (tag_link_to_tag_page)  ( tag -- )
  dup tag>pid$ ((tag_link))
  ;
: (tag_link_to_own_page)  ( tag -- )
  dup tag>own_page $@ 
\  type cr key drop  \ XXX INFORMER
  ((tag_link))
  ;
: tag_link_to_own_page?  ( tag -- wf )
  tag>count @
\  dup cr ." tag>count" . \ XXX INFORMER
  1 =
  lonely_tags_link_to_content @
\  dup cr ." lonely_tags_link_to_content" . \ XXX INFORMER
  and
  ;
: (tag_link)  ( tag -- )
  \ Create a link to the given tag.
  dup tag_link_to_own_page?
  if    (tag_link_to_own_page)
  else  (tag_link_to_tag_page)  then
  ;
: tag_link  ( tag -- )
  \ Create a link to the given tag.
  >r get-order set_fendo_order  r> (tag_link)  set-order
  ;

: (tag_does_reset)  ( tag -- )
  tag>count off
  ;
: (tag_does_increase)  ( tag -- )
  tag>count ++
  ;
: (tag_does_increase_and_save_own_page)  ( tag -- )
  dup (tag_does_increase)  last_traversed_pid $@ rot tag>own_page $!
  ;
: (tag_does_total)  ( tag -- +n )
  tag>count @
  ;
: (tag_does_name)  ( tag -- ca len )
  tag>name
  ;
: (tag_does_text)  ( tag -- ca len )
  tag>text
  ;
: (tag_does_link)  ( tag -- )
  tag_link
  ;
: (tag_does_list_link)  ( tag -- )
  [<li>] tag_link [</li>]
  ;
variable tag_searched_for$
variable tag_presence  \ counter
: (tag_does_presence)  ( tag -- )
  tag>name tag_searched_for$ $@ str=  abs tag_presence +!
  ;

defer (tag_does)  \ current behaviour of the tags

\ **************************************************************
\ Choosing the behaviour of the tags

export

: tags_do_nothing  ( -- )
  \ Set the tags to do nothing.
  ['] drop is (tag_does)
  ;
: tags_do_body  ( -- )
  \ Set the tags to return their body addresses.
  ['] noop is (tag_does)
  ;
: tags_do_reset  ( -- )
  \ Set the tags to reset their counts.
  ['] (tag_does_reset) is (tag_does)
  ;
: tags_do_increase  ( -- )
  \ Set the tags to increase their counts.
  lonely_tags_link_to_content @
  if    ['] (tag_does_increase_and_save_own_page)
  else  ['] (tag_does_increase)
  then  is (tag_does)
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
: tags_do_list_link  ( -- )
  \ Set the tags to create listed links.
  ['] (tag_does_list_link) is (tag_does)
  ;
: tags_do_presence  ( ca len -- )
  \ Set the tags to check if their name is the given name.
  \ ca len = tag name
  tag_searched_for$ $!   tag_presence off
  ['] (tag_does_presence) is (tag_does)
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
    \ The body holds a counter, the name token, the text,
    \ and the last own page id ( XXX not used yet)
    0 ,  \ tag counter
    latestxt >name ,  \ name token
    0 , \ dynamic string variable, pid of the (last) own page
    s,  \ text
  r> set-current
  does>   ( -- ) ( dfa ) (tag_does)
  ;

\ **************************************************************
\ Traverse the tags

s" /tmp/fendo.tags.fs" 2constant tags_filename$
: tag_words  ( -- )
  \ List all tags.
  tags>order words tags<order
  ;
: create_tags_file  ( -- )
  \ Create a temporary file with the list of all tags.
  ['] tag_words
  tags_filename$ w/o create-file throw dup >r outfile-execute
  r> close-file throw
  ;
: evaluate_tags  ( ca len -- )
  get-order n>r set_tags_order  evaluate  nr> set-order
  ;
: check_tags  ( ca len -- )
  ['] (tag_does) defer@ >r  tags_do_nothing evaluate_tags  r> is (tag_does)
  ;
: (execute_all_tags)  ( ca len -- )
  tags>order evaluate  previous
  ;
: execute_all_tags  ( -- )
  \ Execute all tags with their current behaviour.
\  ." execute_all_tags" cr  \ xxx informer
  create_tags_file  tags_filename$ slurp-file
  2dup check_tags (execute_all_tags)
\  ." execute_all_tags end" cr  \ xxx informer
  ;
: traverse_tags  ( xt -- )
  \ Execute all tags with the given behaviour.
  is (tag_does)  execute_all_tags
  ;

\ **************************************************************
\ Other

: #tags  ( -- n )
  \ Number of tags
  \ xxx todo
  ;


.( fendo.addon.tags.fs compiled) cr
