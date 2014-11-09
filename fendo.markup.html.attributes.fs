.( fendo.markup.html.attributes.fs ) cr

\ This file is part of Fendo.

\ This file defines the HTML attributes.

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

\ See at the end of the file.

\ **************************************************************
\ Todo

\ 2013-08-10: Alternative to Gforth's strings: FFL's strings.

\ **************************************************************
\ Requirements

forth_definitions

require galope/3dup.fs
require galope/xstack.fs
require galope/minus-cell-bounds.fs  \ '-cell-bounds'

\ Dynamic strings system used for attributes
false  dup constant [gforth_strings_for_attributes?]  immediate
[if]    require string.fs  \ Gforth
[else]  require ffl/str.fs  \ Forth Foundation Library
[then]

fendo_definitions

\ **************************************************************
\ Fetch and store

: attribute@  ( a -- ca len )
  [gforth_strings_for_attributes?] [if]  $@  [else]  @ str-get  [then]
  ;
: attribute!  ( ca len a -- )
  [gforth_strings_for_attributes?] [if]  $!  [else]  @ str-set  [then]
  ;

\ **************************************************************
\ Defining words

: :attribute@  ( ca len xt -- )
  \ Create a word that fetchs an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  rot rot s" @" s+ :create ,
  does>  ( -- ca1 len1 )
    ( dfa ) perform attribute@
  ;
: :attribute!  ( ca len xt -- )
  \ Create a word that stores an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  rot rot s" !" s+ :create  ,
  does>  ( ca1 len1 -- )
\    ." Parameter in 'does>' of ':attribute!' = " 2dup type cr  \ XXX INFORMER
    ( ca1 len1 dfa ) perform attribute!
  ;
: :attribute?!  ( ca len xt -- )
  \ xxx not used
  \ Create a word that stores an attribute,
  \ if it's empty.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  rot rot s" ?!" s+ :create  ,
  does>  ( ca1 len1 -- )
    ( ca1 len1 dfa ) perform
    dup attribute@ empty? if  attribute!  else  drop 2drop  then
  ;
: :attribute"  ( ca len xt -- )
  \ Create a word that parses and stores an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  rot rot s\" \"" s+ :create  ,
  does>  ( "text<quote>" -- )
    ( dfa ) [char] " parse  rot perform attribute!
  ;
: ((attribute:))  ( -- )
  \ Compile and init the dynamic string of an attribute.
  [gforth_strings_for_attributes?]
  [if]    s" " here 0 , $!
  [else]  str-new dup , str-init  [then]
  ;
false [if]  \ XXX OLD first version
: (attribute:)  ( ca len -- xt )
  \ Create an attribute variable.
  \ It holds two values. The 'attributes_set' variable
  \ lets to choose which value is pointed to.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  :create   latestxt >r  ((attribute:)) ((attribute:))  r>
  does>     ( -- a )  ( dfa ) attributes_set @ cells +
  ;
[then]
: (attribute:)  ( ca len -- xt )
  \ Create an attribute variable.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  :create   latestxt >r  ((attribute:))  r>
  ;
: attribute:  ( "name" -- xt )
  \ Create an attribute variable in the markup vocabulary,
  \ and four words to manage it.
  \ "name" = ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ XXX TODO since the dual set system is removed, an address would be
  \ enough, not an xt...
  get-current fendo>current
  parse-name? abort" Missing name after 'attribute:'"
  2dup (attribute:) ( ca len xt )  dup >r
  3dup :attribute"
  3dup :attribute!
  3dup :attribute?!
  :attribute@
  set-current  r>
  ;

\ **************************************************************
\ Actual HTML attributes

depth [if]
  .( The stack must be empty before defining the attributes.)
  abort
[then]

link_text_as_attribute? [if]  \ xxx tmp
  attribute: link_text
[then]
\ xxx fixme -- the first attribute is not managed by the attributes loops

\ References:
\   <http://dev.w3.org/html5/markup/>
\   <http://dev.w3.org/html5/markup/global-attributes.html>
\ xxx todo complete
attribute: accesskey=
attribute: align=
attribute: alt=
attribute: autofocus=
attribute: border=
attribute: charset=
attribute: checked=
attribute: cite=
attribute: class=
attribute: content=
attribute: contenteditable=
attribute: contextmenu=
attribute: datetime=
attribute: dir=
attribute: disabled=
attribute: draggable=
attribute: dropzone=
attribute: form=
attribute: height=
attribute: hidden=
attribute: href=
attribute: hreflang=
attribute: http-equiv=
attribute: id=
attribute: ismap=
attribute: lang=
attribute: media=
attribute: name=
attribute: onabort=
attribute: onblur=
attribute: oncanplay=
attribute: oncanplaythrough=
attribute: onchange=
attribute: onclick=
attribute: oncontextmenu=
attribute: ondblclick=
attribute: ondrag=
attribute: ondragend=
attribute: ondragenter=
attribute: ondragleave=
attribute: ondragover=
attribute: ondragstart=
attribute: ondrop=
attribute: ondurationchange=
attribute: onemptied=
attribute: onended=
attribute: onerror=
attribute: onfocus=
attribute: oninput=
attribute: oninvalid=
attribute: onkeydown=
attribute: onkeypress=
attribute: onkeyup=
attribute: onload=
attribute: onloadeddata=
attribute: onloadedmetadata=
attribute: onloadstart=
attribute: onmousedown=
attribute: onmousemove=
attribute: onmouseout=
attribute: onmouseover=
attribute: onmouseup=
attribute: onmousewheel=
attribute: onpause=
attribute: onplay=
attribute: onplaying=
attribute: onprogress=
attribute: onratechange=
attribute: onreadystatechange=
attribute: onreset=
attribute: onscroll=
attribute: onseeked=
attribute: onseeking=
attribute: onselect=
attribute: onshow=
attribute: onstalled=
attribute: onsubmit=
attribute: onsuspend=
attribute: ontimeupdate=
attribute: onvolumechange=
attribute: onwaiting=
attribute: rel=
attribute: required=
attribute: spellcheck=
attribute: src=
attribute: style=
attribute: tabindex=
attribute: target=
attribute: title=
attribute: translate=
attribute: type=
attribute: usemap=
attribute: value=
attribute: width=
attribute: xml:base=
attribute: xml:lang=
attribute: xmlns=

depth constant #attributes  \ count of defined attributes

\ **************************************************************
\ Virtual attributes

: (xml:)lang=  ( -- a )
  \ Return the proper language attribute
  \ for the current syntax.
  xhtml? @ if  xml:lang=  else  lang=  then
  ;
: (xml:)lang=!  ( ca len -- )
  \ Set the proper language attribute
  \ for the current syntax.
  (xml:)lang= attribute!
  ;
: (xml:)lang=@  ( ca len -- )
  \ Fetch the proper language attribute
  \ for the current syntax.
  (xml:)lang= attribute@
  ;

\ **************************************************************
\ Table

create 'attributes_xt  \ table for the execution tokens of the attribute variables
#attributes 0 [?do]  ,  [loop]  \ fill the table

\ **************************************************************
\ Tools

: attributes_xt_zone  ( -- a len )
  \ Return the start and length of the ''attribute_xt' table.
  \ 'attributes_xt #attributes 1- cells  \ XXX OLD -- why '1-'?
  'attributes_xt #attributes cells
  ;
: -attribute  ( xt -- )
  \ Clear a HTML attribute with an empty string.
  [gforth_strings_for_attributes?]
  [if]    s" " rot perform $!
  [else]  perform @ str-init  [then]
  ;
: -attributes  ( -- )
  \ Clear all HTML attributes with empty strings.
  attributes_xt_zone bounds ?do  i -attribute  cell +loop
  ;
: ?hreflang=!  ( a -- )
  \ If the given page has a different language than the current one,
  \ then update the 'hreflang' attribute.
  \ a = page id
  language 2dup current_page language compare
  if  hreflang=!  else  2drop  then
  ;

\ **************************************************************
\ Echo

: ((+attribute))  ( ca1 len1 ca2 len2 -- )
  \ Echo an attribute.
  \ ca1 len1 = attribute value
  \ ca2 len2 = attribute label
  \   (it includes the final "=", e.g. "alt=")
\  2dup ." label<< " type ." >> " cr  \ xxx informer
\  2over ." value<< " type ." >> " cr  \ xxx informer
  echo_space echo echo_quote echo echo_quote
  ;
: (+attribute)  ( xt ca len -- )
  \ Echo an attribute.
  \ xt = execution token of the attribute variable
  \ ca len = attribute value
  rot >name name>string ((+attribute))
  ;
: echo_real_attribute  ( xt -- )
  \ Echo a real attribute, if not empty.
  \ xt = execution token of the attribute variable
\  ." {{{ " dup >name ?dup if  id.  else  ." ?"  then  ." }}}"  \ xxx informer
  dup execute attribute@
  dup if  (+attribute)  else  2drop drop  then
  ;
: echo_attributes  ( -- )
  \ Echo all non-empty attributes.
  attributes_xt_zone bounds ?do
    i @ echo_real_attribute
  cell +loop
  ;

\ **************************************************************
\ Saving and restoring the attributes

\ XXX TODO

\ Create a stack for the attributes (2 cells per attribute string,
\ and 4 nesting levels):
#attributes 2* 4 * xstack attributes_stack

: save_attribute  ( xt -- )
  \ Save an attribute.
  \ xt = execution token of the attribute variable
  execute attribute@ 2>x
  ;
: save_attributes  ( -- )
  \ Save all attributes.
  attributes_stack
  attributes_xt_zone bounds ?do
    i @ save_attribute
  cell +loop
  ;
: restore_attribute  ( xt -- )
  \ Restore an attribute.
  \ xt = execution token of the attribute variable
  2x> rot execute attribute!
  ;
: restore_attributes  ( -- )
  \ Restore all attributes.
  attributes_stack
  attributes_xt_zone -cell-bounds ?do
    i @ restore_attribute
  [ cell negate ] literal +loop
  ;

\ **************************************************************
\ Change history of this file

\ 2013-06-10: Start. Factored from <fendo_markup_html.fs>.
\
\ 2013-06-29: Fix: the 'raw=' attribute, the last in the table, wasn't
\ cleared by '-attributes'.
\
\ 2013-07-03: New: secondary set of attributes, selectable with a
\ variable, in order to let nested markups.
\
\ 2013-07-03: Change: attributes are stored in dynamic strings.
\
\ 2013-07-21: Change: code rearranged a bit.
\
\ 2013-08-10: Change: FFL's strings as a compile option.
\
\ 2013-08-12: New: 'attribute!' and 'attribute@' hide the conditional
\ compilation required to change the string system.
\
\ 2013-08-14: Change: The 'raw=' attributte is removed; the word
\ 'unraw_attributes' (defined in <fendo_markup_wiki.fs>) makes it
\ unnecessary.
\
\ 2013-08-15: New: ':attribute?!', required by the bookmarked links.
\
\ 2013-10-27: New: '?hreflang=!', factored out from
\ 'hierarchy_meta_link' in <addons/hierarchy_meta_links.fs>.
\
\ 2013-10-30: Fix: 'forth-wordlist' is set to current before requiring
\ the library files. The problem was <ffl/config.fs> created
\ 'ffl.version' in the 'fendo' vocabulary, but searched for it in
\ 'forth-wordlist'.
\
\ 2013-12-05: Change: '(xml:)lang=' moved here from
\ <fendo_markup_wiki.fs>.
\
\ 2013-12-05: New: '(xml:)lang=!', factored from
\ <fendo_markup_wiki.fs>; '(xml:)lang=@'.
\
\ 2014-07-13: New: 'xmlnl=' attribute, needed by the Atom module.
\
\ 2014-11-09: Change: all code related to the old unused 'raw='
\ pseudo-attribute attribute is removed.
\
\ 2014-11-09: '-attribute' factored out from '-attributes'.

.( fendo.markup.html.attributes.fs compiled) cr

