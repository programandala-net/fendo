.( fendo.markup.html.attributes.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the HTML attributes.

\ Last modified 201812080157.
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
\ TODO

\ 2013-08-10: Alternative to Gforth's strings: FFL's strings.

\ ==============================================================
\ Requirements

forth_definitions

require string.fs  \ Gforth's dynamic strings

false  dup constant [gforth_strings_for_attributes?]  immediate
0= [if]
  \ Forth Foundation Library's str module is used instead of Gforth's
  \ strings.
  \ XXX FIXME -- strange things happen in this case
  require ffl/str.fs
[then]

require galope/three-dup.fs
require galope/dollar-store-new.fs  \ `$!new`
require galope/xstack.fs
require galope/minus-cell-bounds.fs  \ `-cell-bounds`

fendo_definitions

\ ==============================================================
\ Fetch and store

: attribute@ ( a -- ca len )
  [gforth_strings_for_attributes?] [if]  $@  [else]  @ str-get  [then] ;
  \ a = attribute variable

: attribute! ( ca len a -- )
  [gforth_strings_for_attributes?] [if]  $!new  [else]  @ str-set  [then] ;
  \ a = attribute variable

\ ==============================================================
\ Defining words

: :attribute@ ( ca len a -- )
  rot rot s" @" s+ :create ,
  does> ( -- ca1 len1 ) ( dfa ) @ attribute@ ;
  \ Create a word that fetchs an attribute.
  \ ca len = name of the attribute variable
  \ a = attribute variable
  \ ca1 len1 = attribute value

: :attribute!  ( ca len a -- )
  rot rot s" !" s+ :create  ,
  does>  ( ca1 len1 -- )
\    ." Parameter in `does>` of `:attribute!` = " 2dup type cr  \ XXX INFORMER
    ( ca1 len1 dfa ) @ attribute! ;
  \ Create a word that stores an attribute.
  \ ca len = name of the attribute variable
  \ a = attribute variable
  \ ca1 len1 = attribute value

: :attribute?!  ( ca len a -- )
  rot rot s" ?!" s+ :create  ,
  does>  ( ca1 len1 -- )
    ( ca1 len1 dfa )
    @ dup attribute@ empty? if  attribute!  else  drop 2drop  then ;
  \ Create a word that stores an attribute, if it's empty.
  \ ca len = name of the attribute variable
  \ a = attribute variable
  \ ca1 len1 = attribute value

: :attribute+!  ( ca len a -- )
  rot rot s" +!" s+ :create  ,
  does>  ( ca1 len1 -- )
    ( ca1 len1 dfa )  @ dup attribute@ dup
    if    s"  " s+ 2>r rot rot 2r> 2swap s+ rot
    else  2drop  then  attribute! ;
  \ Create a word that adds a string to an attribute.
  \ ca len = name of the attribute variable
  \ a = attribute variable
  \ ca1 len1 = attribute value

: :attribute"  ( ca len a -- )
  rot rot s\" \"" s+ :create  ,
  does>  ( "text<quote>" -- )
    ( dfa ) [char] " parse  rot @ attribute! ;
  \ Create a word that parses and stores an attribute.
  \ ca len = name of the attribute variable
  \ a = attribute variable

: :attribute'  ( ca len a -- )
  rot rot s" '" s+ :create  ,
  does>  ( "text<quote>" -- )
    ( dfa ) [char] ' parse  rot @ attribute! ;
  \ Create a word that parses and stores an attribute.
  \ ca len = name of the attribute variable
  \ a = attribute variable

: ((attribute:))  ( -- )
  [gforth_strings_for_attributes?]
  [if]    s" " here 0 , $!
  [else]  str-new dup , str-init  [then] ;
  \ Compile and init the dynamic string of an attribute.

: (attribute:)  ( ca len -- a )
  :create   here >r ((attribute:)) r> ;
  \ Create an attribute variable.
  \ ca len = name of the attribute variable
  \ a = attribute variable

: :attribute  ( ca len a -- )
  3dup :attribute"
  3dup :attribute'
  3dup :attribute!
  3dup :attribute?!
  3dup :attribute+!
       :attribute@ ;
  \ Create the words to manage an attribute.
  \ ca len = attribute name
  \ a = attribute variable

: attribute:  ( "name" -- a )
  get-current fendo>current
  parse-name? abort" Missing name after `attribute:`"
  2dup (attribute:) ( ca len a )  dup >r
  :attribute  set-current  r> ;
  \ Create an attribute variable in the markup vocabulary,
  \ and all words needed to manage it.
  \ "name" = ca len = name of the attribute variable
  \ a = attribute variable

\ ==============================================================
\ Actual HTML attributes

depth [if]
  .( The stack must be empty before defining the attributes.)
  abort
[then]

link_text_as_attribute? [if]  \ XXX TMP
  attribute: link_text
[then]

\ References:
\   <http://dev.w3.org/html5/markup/>
\   <http://dev.w3.org/html5/markup/global-attributes.html>
\ XXX TODO complete
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
attribute: language=
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

\ ==============================================================
\ Virtual attributes

: (xml:)lang=  ( -- a )
  xhtml? if  xml:lang=  else  lang=  then ;
  \ Return the proper language attribute
  \ for the current syntax.

: (xml:)lang=!  ( ca len -- )
  (xml:)lang= attribute! ;
  \ Set the proper language attribute
  \ for the current syntax.

: (xml:)lang=@  ( ca len -- )
  (xml:)lang= attribute@ ;
  \ Fetch the proper language attribute
  \ for the current syntax.

\ ==============================================================
\ Table

create attributes  \ table for the attribute variables
#attributes 0 [?do]  ,  [loop]  \ fill the table

\ ==============================================================
\ Tools

: (attributes)  ( -- a len )
  attributes #attributes cells ;
  \ Return the start and length of the `'attribute_xt` table.
  \ attributes #attributes 1- cells  \ XXX OLD -- why `1-`?

: -attribute  ( a -- )
  [gforth_strings_for_attributes?]
  [if]  0 swap $!len  [else]  @ str-init  [then] ;
  \ Clear an attribute variable with an empty string.
  \ a = attribute variable

: -attributes  ( -- )
  (attributes) bounds ?do  i @ -attribute  cell +loop
  0 link_anchor $!len  ;
  \ Clear all HTML attributes, the link anchor and the link text.

: ?hreflang=!  ( a -- )
  language 2dup current_page language compare
  if  hreflang=!  else  2drop  then ;
  \ If the given page has a different language than the current one,
  \ then update the `hreflang` attribute.
  \ a = page id

\ ==============================================================
\ Echo

: ((+attribute))  ( ca1 len1 ca2 len2 -- )
\  2dup ." label<< " type ." >> " cr  \ XXX INFORMER
\  2over ." value<< " type ." >> " cr  \ XXX INFORMER
  echo_space echo echo_quote echo echo_quote ;
  \ Echo an attribute.
  \ ca1 len1 = attribute value
  \ ca2 len2 = attribute label
  \   (it includes the final "=", e.g. "alt=")

: (+attribute)  ( a ca len -- )
  rot body> >name name>string ((+attribute)) ;
  \ Echo an attribute.
  \ a = attribute variable
  \ ca len = attribute value

: echo_attribute  ( a -- )
\  ." {{{ " dup >name ?dup if  id.  else  ." ?"  then  ." }}}"  \ XXX INFORMER
  dup attribute@ dup if  (+attribute)  else  2drop drop  then ;
  \ Echo an attribute, if not empty.
  \ a = attribute variable

: echo_attributes  ( -- )
  \ Echo all non-empty attributes.
  (attributes) bounds ?do
    i @ echo_attribute
  cell +loop ;

\ ==============================================================
\ Saving and restoring the attributes

\ The URL anchor does not work like an attribute, but it must be saved
\ and restored with them, just in case.

#attributes 1+ 2* 4 * xstack attributes_stack
  \ Create a stack for the attributes plus the link anchor (2 cells per
  \ element, and 4 nesting levels).

: mem>x  ( ca len -- )
  save-mem 2>x ;

: save_attribute  ( a -- )
  attribute@ mem>x ;
  \ Save an attribute.
  \ a = attribute variable

: save_attributes  ( -- )
  attributes_stack
  link_anchor $@ mem>x
  (attributes) bounds ?do
    i @ save_attribute
  cell +loop ;
  \ Save the link anchor and all attributes.

: restore_attribute  ( a -- )
  2x> rot attribute! ;
  \ Restore an attribute.
  \ a = attribute variable

: restore_attributes  ( -- )
  attributes_stack
  (attributes) -cell-bounds ?do
    i @ restore_attribute
  [ cell negate ] literal +loop
  2x> link_anchor $!new ;
  \ Restore all attributes, and the link anchor.

.( fendo.markup.html.attributes.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-06-10: Start. Factored from <fendo_markup_html.fs>.
\
\ 2013-06-29: Fix: the `raw=` attribute, the last in the table, wasn't
\ cleared by `-attributes`.
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
\ 2013-08-12: New: `attribute!` and `attribute@` hide the conditional
\ compilation required to change the string system.
\
\ 2013-08-14: Change: The `raw=` attributte is removed; the word
\ `unraw_attributes` (defined in <fendo_markup_wiki.fs>) makes it
\ unnecessary.
\
\ 2013-08-15: New: `:attribute?!`, required by the bookmarked links.
\
\ 2013-10-27: New: `?hreflang=!`, factored out from
\ `hierarchy_meta_link` in <addons/hierarchy_meta_links.fs>.
\
\ 2013-10-30: Fix: `forth-wordlist` is set to current before requiring
\ the library files. The problem was <ffl/config.fs> created
\ `ffl.version` in the `fendo` vocabulary, but searched for it in
\ `forth-wordlist`.
\
\ 2013-12-05: Change: `(xml:)lang=` moved here from
\ <fendo_markup_wiki.fs>.
\
\ 2013-12-05: New: `(xml:)lang=!`, factored from
\ <fendo_markup_wiki.fs>; `(xml:)lang=@`.
\
\ 2014-07-13: New: `xmlnl=` attribute, needed by the Atom module.
\
\ 2014-11-09: Change: all code related to the old unused `raw=`
\ pseudo-attribute attribute is removed.
\
\ 2014-11-09: `-attribute` factored out from `-attributes`.
\
\ 2014-11-10: Change: Attribute variables don't contain an execution
\ token anymore. Now they behave as ordinary variables that contain a
\ dynamic string. The execution token was used to select one of the
\ two actual values of the attributes, because there were two sets
\ attributes. Now there's only one set of attributes, and all of them
\ are saved and restore when needed, using a specific stack. All
\ related words have been adapted.
\
\ 2014-11-16: Change: `-attributes` clears also `link_anchor`.
\
\ 2014-11-17: Fix: `save_attribute` uses `save-mem` in order to create
\ a copy of the string; `attribute!`, when using Gforth's dynamic
\ strings, removes the current string from memory before updating it.
\ These changes solve the problem caused by shortcuts created in the
\ same memory zone than the original href.
\
\ 2014-11-18: Changes: `mem>x` is factored out from `save_attribute`
\ and `save_attributes`; Galope's `$!new` is factored out from
\ `attribute!` and used also in `restore_attributes`.
\
\ 2014-11-30: New: `:attribute+!`.
\
\ 2014-12-13: New: `language=` attribute, required by `<script>`.
\
\ 2014-12-22: New: "attribute'" is added to support also single quotes
\ in HTML attributes. `unraw_attributes` is updated accordingly in
\ <fendo.markup.fendo.fs>.
\
\ 2014-12-22: Change `:attribute` factored from `attribute:`.
\
\ 2015-02-01: Change: the `xhtml?` variable is a value now.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
