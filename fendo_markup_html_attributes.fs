.( fendo_markup_html_attributes.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

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

\ 2013-06-10 Start. Factored from <fendo_markup_html.fs>.
\ 2013-06-29 Fix: the 'raw=' attribute, the last in the table,
\   wasn't cleared by '-attributes'.
\ 2013-07-03 New: secondary set of attributes, selectable
\ with a variable, in order to let nested markups.
\ 2013-07-03 Change: attributes are stored in dynamic strings.
\ 2013-07-21 Change: code rearranged a bit.
\ 2013-08-10 Change: FFL's strings as a compile option.
\ 2013-08-12 New: 'attribute!' and 'attribute@' hide the conditional
\   compilation required to change the string system.
\ 2013-08-14 Change: The 'raw=' attributte is removed; the word
\   'unraw_attributes' (defined in <fendo_markup_wiki.fs>)
\   makes it unnecessary.

\ **************************************************************
\ Todo

\ 2013-08-10 Alternative to Gforth's strings: FFL's strings.

\ **************************************************************
\ Requirements

require ../galope/3dup.fs

\ Dynamic strings system used for attributes
false  \ Gforth strings instead of FFL strings?
dup constant [gforth_strings_for_attributes?]  immediate
[if]    require string.fs
[else]  require ffl/str.fs
[then]

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
    ( ca1 len1 dfa ) perform attribute!
  ;
: :attribute"  ( ca len xt -- )
  \ Create a word that parses and stores an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  rot rot s\" \"" s+ :create  ,
  does>  ( "text<quote>" -- )
    ( dfa ) [char] " parse  rot perform attribute!
  ; 

variable attributes_set  \ 0 or 1
: >attributes<  ( -- )
  \ Exchange the attributes set (0->1, 1->0)
  attributes_set @ 0= abs  attributes_set !
  ;

: ((attribute:))  ( -- )
  \ Compile and init one of the two dynamic strings of an attribute.
  [gforth_strings_for_attributes?]
  [if]    s" " here 0 , $!
  [else]  str-new dup , str-init
  [then]
  ;
: (attribute:)  ( ca len -- xt )
  \ Create an attribute variable.
  \ It holds two values. The 'attributes_set' variable
  \ lets to choose which value is pointed to.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  :create   latestxt >r  ((attribute:)) ((attribute:))  r>
  does>     ( -- a )  ( dfa ) attributes_set @ cells + 
  ;
: attribute:  ( "name" -- xt )
  \ Create an attribute variable in the markup vocabulary,
  \ and three words to manage it.
  \ "name" = ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  get-current fendo>current 
  parse-name? abort" Missing name after 'attribute:'"
  2dup (attribute:) ( ca len xt )  dup >r
  3dup :attribute" 3dup :attribute! :attribute@
  set-current  r>
  ;

\ **************************************************************
\ Actual HTML attributes

depth [if]
  .( The stack must be empty before defining the attributes.)
  abort
[then]

\ The first attribute defined (thus the last one in the table) is a
\ special one that lets to include any raw content in the HTML
\ tag, without label or surrounding quotes:
\ attribute: raw=  \ xxx old

false dup constant link_text_as_attribute? [if]  \ xxx tmp
  attribute: link_text
[then]
\ xxx fixme -- the first attribute is not managed by the attributes loops

\ Real attributes
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

\ **************************************************************
\ Table

depth constant #attributes  \ count of defined attributes
create 'attributes_xt  \ table for the execution tokens of the attribute variables
#attributes 0 [?do]  ,  [loop]  \ fill the table

\ **************************************************************
\ Tools

: -attributes  ( -- )
  \ Clear all HTML attributes with empty strings.
  'attributes_xt #attributes cells bounds ?do
    [gforth_strings_for_attributes?]
    [if]    s" " i perform $!
    [else]  i perform @ str-init
    [then]
  cell +loop
  ;
: attributes_xt_zone  ( -- ca len )
  \ Return the start and length of the ''attribute_xt' table.
  'attributes_xt #attributes 1- cells
  ;

\ **************************************************************
\ Echo

: ((+attribute))  ( ca1 len1 ca2 len2 -- )
  \ Echo an attribute.
  \ ca1 len1 = attribute value
  \ ca2 len2 = attribute label
  \   (it includes the final "=", e.g. "alt=")
\  2dup ." label<< " type ." >> " cr  \ xxx debug check
\  2over ." value<< " type ." >> " cr  \ xxx debug check
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
\  ." {{{ " dup >name ?dup if  id.  else  ." ?"  then  ." }}}"  \ xxx debug check
  dup execute attribute@
  dup if  (+attribute)  else  2drop drop  then
  ;
: echo_real_attributes  ( -- )
  \ Echo all non-empty real attributes.
  attributes_xt_zone bounds ?do
    i @ echo_real_attribute
  cell +loop
  ;
[defined] raw= [if]
: echo_raw_attribute  ( -- )
  \ Echo the special raw attribute, if not empty.
  attributes_xt_zone + perform attribute@
  dup if  echo_space echo echo_space  else  2drop  then
  ;
[then]
: echo_attributes  ( -- )
  \ Echo all non-empty attributes.
  echo_real_attributes
  [defined] raw= [if]  echo_raw_attribute  [then]
  ;

.( fendo_markup_html_attributes.fs compiled) cr

