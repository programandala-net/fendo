.( fendo.links.fs ) cr

\ This file is part of Fendo.

\ This file provides the words needed to create links,
\ by the markup words or by the user application.

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

\ See at the end of the file.

\ **************************************************************
\ Requirements

forth_definitions

require galope/minus-prefix.fs  \ '-prefix'

fendo_definitions

require fendo.markup.common.fs
require fendo.markup.html.fs


\ **************************************************************
\ Tools for links

: file://?  ( ca len -- wf )
  \ Does a string start with "file://"?
  s" file://" string-prefix?
  ;
' file://? alias file_href?
: http://?  ( ca len -- wf )
  \ Does a string start with "http://"?
  s" http://" string-prefix?
  ;
: https://?  ( ca len -- wf )
  \ Does a string start with "https://"?
  s" https://" string-prefix?
  ;
: ftp://?  ( ca len -- wf )
  \ Does a string start with "ftp://"?
  s" ftp://" string-prefix?
  ;
: external_href?  ( ca len -- wf )
  \ Is a href attribute external?
  2dup http://? >r  2dup https://? >r  ftp://? r> or r> or
  ;
link_text_as_attribute?  0= [if]  \ xxx tmp -- XXX FIXME unbalanced condition
$variable link_text
: link_text!  ( ca len -- )
  link_text $!
  ;
: link_text@  ( -- ca len )
  link_text $@
  ;
: link_text?!  ( ca len -- )
  \ If the the string variable 'link_text' is empty,
  \ store the given string into it.
  link_text@ empty? if  link_text!  else  2drop  then
  ;
[then]  \ XXX 2014-11-07 this was missing,
        \ but still no sure if this is the right position:
        \ it was placed here after comparation with
        \ <fendo.markup.fendo.link>.
variable link_text_already_evaluated?  \ flag
link_text_already_evaluated? off
: evaluate_link_text  ( -- )
  \ Note: 'link_text_already_evaluated?' is turned on by
  \ '(parse_link_text)' in <fendo.parser.fs>.
\  separate? @  \ XXX TMP 2014-08-13 try to fix the bug described in the to-do
  link_text@
\  cr ." In evaluate_link_text in fendo.links.fs link_text is " 2dup type  \ XXX INFORMER
  link_text_already_evaluated? @
\  dup cr ." ( in evaluate_link_text if fendo.links.fs link_text_already_evaluated? = ) " . key drop  \ XXX INFORMER
  if    echo  link_text_already_evaluated? off
  else  separate? @ >r evaluate_content r> separate? !  then
\  separate? !  \ XXX TMP 2014-08-13 try to fix the bug described in the to-do
  \ XXX TMP saving and restoring 'separate?' makes no difference
  ;

\ $variable link_anchor  \ XXX OLD -- moved to fendo.markup.html.attributes.fs>
: /anchor ( ca1 len1 -- ca2 len2 ca3 len3 )
  \ Divide a href attribute at its anchor.
  \ ca1 len1 = href attribute
  \ ca2 len2 = href attribute without the anchor
  \ ca3 len3 = anchor without the "#" character
  s" #" sides/ drop
  ; 
:noname ( ca len -- ca len | ca' len' )
  \ Remove the anchor from a href attribute.
  \ ca len = href attribute
  \ ca' len' = href attribute without anchor
  \ XXX TODO factor out
  /anchor 2drop
  ; is -anchor  \ defered in <fendo.fs>
:noname ( ca len -- ca len | ca' len' )
  \ Extract the anchor from a href attribute and store it.
  \ ca len = href attribute
  \ ca' len' = href attribute without anchor
  \ XXX TODO factor out
  /anchor link_anchor $!
  ; is -anchor!  \ defered in <fendo.fs>
: +anchor  ( ca1 len1 ca2 len2 -- ca1 len1 | ca3 len3 )
  \ Add a link anchor to a href attribute.
  \ ca1 len1 = href attribute
  \ ca2 len2 = anchor, without "#"
  ." Anchor parameter in '+anchor' = " 2dup type cr  \ XXX INFORMER
  dup if  2>r s" #" s+ 2r> s+  else  2drop  then
  ;
variable link_type
1 enum local_link
  enum external_link
  enum file_link  drop
: >link_type_id  ( ca len -- n )
  \ Convert an href attribute to its type id.
  2dup external_href? if  2drop external_link exit  then
  file_href? if  file_link exit  then
  local_link
  ;
: set_link_type  ( ca len -- )
  \ Get and store the type id of an href attribute.
  \ xxx todo no href means local, if there is/was an anchor label
  >link_type_id link_type !
  ;
: external_link?  ( -- wf )
  link_type @ external_link =
  ;
: local_link?  ( -- wf )
  link_type @ local_link =
  ;
: file_link?  ( -- wf )
  link_type @ file_link =
  ;
: missing_local_link_text  ( -- ca len )
\  ." missing_local_link_text" cr  \ xxx informer
  href=@ -extension 2dup required_data<pid$
  >sb  \ xxx tmp
  evaluate title
  save_echo echo>string
  save_attributes -attributes
  evaluate_content echoed $@
  save-mem  \ XXX TODO needed?
  restore_attributes
  restore_echo
  ;
: missing_external_link_text  ( -- ca len )
  href=@
  ;
: missing_file_link_text  ( -- ca len )
  href=@ -path
  ;
: missing_link_text  ( -- ca len )
  \ Set a proper link text if it's missing.
  \ xxx todo
  local_link?  if  missing_local_link_text exit  then
  external_link?  if  missing_external_link_text exit  then  \  xxx
  file_link?  if  missing_file_link_text exit  then  \ xxx
  true abort" Wrong link type"  \ xxx tmp
  ;
: external_class  ( -- )
  \ Add "external" to the class attribute.
  class=@ s" external" bs& class=!
  ;
:noname  ( ca len -- )
  \ Restore the link anchor of the local href attribute, if any.
  link_anchor $@ +anchor
  ; is link_anchor+
: convert_local_link_href  ( ca1 len1 -- ca2 len2 )
  \ Convert a raw local href to a finished href.
  ." Parameter in 'convert_local_link_href' = " 2dup type cr  \ XXX INFORMER
  dup if  pid$>data>pid# target_file  then  link_anchor+
  ;
: -file://  ( ca len -- ca' len' )
  s" file://" -prefix
  ;
: convert_file_link_href  ( ca len -- ca' len' )
  -file://  files_subdir $@ 2swap s+
  ;
: convert_link_href  ( ca len -- ca' len' )
  \ ca len = href attribute, without anchor
\  ." Parameter in 'convert_link_href' = " 2dup type cr  \ XXX INFORMER
  link_type @ case
    local_link      of  convert_local_link_href     endof
    file_link       of  convert_file_link_href      endof
  endcase
  ;
variable local_link_to_draft_page?
: (tune_local_hreflang)  ( a -- )
  \ Set the hreflang attribute of a local link, if needed.
  \ a = page id of the link destination
  s" pid#>lang$ 2dup current_lang$" evaluate str=
  if  2drop  else  hreflang=?!  then
  ;
: tune_local_hreflang  ( a -- )
  \ Set the hreflang attribute of a local link, if needed.
  \ a = page id of the link destination
  multilingual? if  (tune_local_hreflang)  else  drop  then
  ;
: tune_local_link  ( -- )
  \ xxx todo fetch alternative language title and description
\  ." tune_local_link" cr  \ xxx informer
  href=@
\  ." 'href=' in 'tune_local_link' (0) = " 2dup type cr  \ xxx informer
  pid$>(data>)pid#  >r
\  link_text@ ." link_text in tune_local_link (0) = " type cr  \ xxx informer
\  r@ title ." title in tune_local_link (1) = " type cr  \ xxx informer
  r@ draft? local_link_to_draft_page? !
  r@ description 
\  ." 'href=' in 'tune_local_link' (1) = " href=@ type cr  \ xxx informer
  unmarkup
\  ." 'href=' in 'tune_local_link' (2) = " href=@ type cr  \ xxx informer
  title=?!
\  link_text@ ." link_text in tune_local_link (1) = " type cr  \ xxx informer
  r@ title
\  ." title in tune_local_link (2) = " 2dup type cr  \ xxx informer
  link_text?!  \ xxx bug: this call corrupts 'link_text'
\  link_text@ ." link_text in tune_local_link (2) = " type cr  \ xxx informer
  r@ tune_local_hreflang
  r> access_key accesskey=?!
\  ." end of tune_local_link" cr  \ xxx informer
  ;
: tune_link  ( -- )  \ xxx todo
  \ Tune the attributes parsed from the link.
  local_link? if  tune_local_link  then
  href=@
  ." 'href=' in 'tune_link' before 'convert_link_href' = " 2dup type cr  \ xxx informer
  convert_link_href
  ." 'href=' in 'tune_link' after 'convert_link_href' = " 2dup type cr  \ xxx informer
  href=!
  link_text@ empty? if  missing_link_text link_text!  then
  external_link? if  external_class  then
  ;
: echo_link_text  ( -- )
  \ Echo just the link text.
  echo_space evaluate_link_text
  ;
\ Two hooks for the application,
\ e.g. to add the size of a linked file:
defer link_text_suffix
defer link_suffix
' noop  dup is link_text_suffix  is link_suffix
: (echo_link)  ( -- )
  \ Echo the final link.
\  cr ." In (echo_link) link_text$ is " link_text@ type  \ XXX INFORMER
  [<a>] evaluate_link_text link_text_suffix [</a>] link_suffix
  ;
: echo_link?  ( -- wf )
  \ Can the current link be echoed?
  href=@ nip  local_link_to_draft_page? @ 0=  and
  ;
: reset_link  ( -- )
  \ Reset the link attributes that are not actual HTML attributes,
  \ and are not reseted by the HTML tags layer.
  s" " link_text!  local_link_to_draft_page? off
  ;
: echo_link  ( -- )
  \ Echo a link, if possible.
  \ All link attributes have been set.
  \ XXX FIXME link_text@ here returns a string with macros already
  \ parsed! why?
\  ." In 'echo_link', 'link_text$' = " link_text@ type cr  \ XXX INFORMER
\  ." In 'echo_link', 'href=' = " href=@ 2dup type ." [".s 2drop ." ]" cr  \ XXX INFORMER
  tune_link  echo_link?
  if  (echo_link)  else  echo_link_text  then  reset_link
  ;

defer (get_link_href)  ( ca len -- )
  \ Defined in <fendo.markup.fendo.link.fs>.

\ **************************************************************
\ Links xxx note: original words of <fendo.tools.fs>

: (link)  ( ca len -- )
  \ Create a link.
  \ Its attributes and link text have to be set previously.
  \ ca len = page id, URL or shortcut
  (get_link_href) echo_link
  ;
: link  ( ca1 len1 ca2 len2 -- )
  \ Create a link of any type.
  \ Its attributes have to be set previously.
  \ ca1 len1 = page id, URL or shortcut
  \ ca2 len2 = link text
  ." In 'link' the link text is " 2dup type cr  \ XXX INFORMER
  ." In 'link' the page id is " 2over type cr  \ XXX INFORMER
  link_text! (link)
  ;
: link<pid$  ( ca len -- )
  \ Create a link to a local page.
  \ Its attributes have to be set previously.
  \ If 'link_text' is not set, the page title will be used.
  \ ca len = page id or shortcut to it
  \ xxx todo make it work with anchors!?
\  ." title_link " 2dup type cr  \ xxx informer
  2dup pid$>data>pid# title link_text?! (link)
  ;
: link<pid#  ( pid -- )
  \ Create a link to a local page.
  \ Its attributes have to be set previously.
  \ If 'link_text' is not set, the page title will be used.
  dup title link_text?! pid#>pid$ (link)
  ;

\ **************************************************************
\ Change history of this file

\ 2013-11-11: Code extracted from <fendo_markup_wiki.fs>: 'link'.
\
\ 2013-11-26: Change: several words renamed, after a new uniform
\ notation: "pid$" and "pid#" for both types of page ids.
\
\ 2014-03-03: New: 'link<pid#'.
\
\ 2014-03-03: Change: 'title_link' renamed to 'link<pid$'.
\
\ 2014-06-15: Fix: repeated evaluation of link texts is solved with
\ the new 'link_text_already_evaluated?' flag.
\
\ 2014-07-11: Change: 'pid$>url' moved to <fendo.data.fs>.
\
\ 2014-08-15: Fix: comment updated.
\
\ 2014-08-15: Fix: 'link_text_already_evaluated? off' was missing in
\ 'evaluate_link_text'.
\
\ 2014-10-12: Fix: 'evaluate_link_text' now preserves the content of
\ 'separate?' in the return stack; it ruined the stack before calling
\ 'evaluate_content'.
\
\ 2014-11-07: a '[then]' was missing (more details in a comment marked
\ with this date).
\
\ 2014-11-08: Change: 'unmarkup' (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.
\
\ 2014-11-09: Change: all 'true [if]' that compiled code that long ago
\ was moved from <fendo.markup.fendo.link.fs> have been removed. Those
\ conditions were needed just in case strange things happened.
\
\ 2014-11-11: Change: '+anchor' rewritten without locals.

.( fendo.links.fs ) cr
