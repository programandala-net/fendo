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

\ **************************************************************
\ Todo

\ 2013-07-03 create a secondary set of attributes, selectable
\ with a variable, in order to let nested markups
\ 2013-06-29 use dynamic string for attributes

\ **************************************************************
\ Requirements

require galope/3dup.fs

\ **************************************************************
\ Defining words

: :attribute@  ( ca len xt -- )
  \ Create a word that fetchs an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  >r
  s" @" s+ :create  \ create a word with the name 'attribute@'.
  r> ,
  does>  ( -- ca1 len1 )
    ( dfa ) perform count
  ;
: :attribute!  ( ca len xt -- )
  \ Create a word that stores an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  >r
  s" !" s+ nextname create  \ create a word with the name 'attribute!'.
  r> ,
  does>  ( ca1 len1 -- )
    ( dfa ) perform place
  ;
: :attribute"  ( ca len xt -- )
  \ Create a word that parses and stores an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  >r
  s\" \"" s+ nextname create  \ create a word with the name 'attribute"'.
  r> ,
  does>  ( "text<quote>" -- )
    ( dfa ) [char] " parse  rot perform place
  ; 
: attribute:  ( "name" -- xt )
  \ Create (in the markup vocabulary)
  \ a string variable for an attribute, and two words to manage it.
  \ "name" = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca len = name of the attribute variable
  get-current fendo>current 
  parse-name? abort" Missing name after 'attribute:'"
  2dup :svariable latestxt  ( ca len xt )  dup >r
  3dup :attribute" 3dup :attribute! :attribute@
  set-current  r>
  ;

\ **************************************************************
\ Actual HTML attributes

depth [if]
  .( The stack must be empty before defining the attributes.)
  abort
[then]

\ The first attribute defined (thus the last in the table) is a
\ special one that lets to include any raw content in the HTML
\ tag, without label or surrounding quotes:
attribute: raw=

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
create 'attributes_xt  \ table for the xt of the attribute variables
#attributes 0 [?do]  ,  [loop]  \ fill the table

\ **************************************************************
\ Printing and manipulating 

: -attributes  ( -- )
  \ Clear all HTML attributes.
  'attributes_xt #attributes cells bounds ?do
    i perform off
  cell +loop
  ;

: ((+attribute))  ( ca1 len1 ca2 len2 -- )
  \ Print an attribute.
  \ ca1 len1 = attribute value
  \ ca2 len2 = attribute label
  \   (it includes the final "=", e.g. "alt=")
  echo_space echo echo_quote echo echo_quote
  ;
: (+attribute)  ( xt ca len -- )
  \ Print an attribute.
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  rot >name name>string ((+attribute))
  ;
: echo_real_attribute  ( xt -- )
  \ Print an attribute, if not empty.
  \ xt = execution token of the attribute variable
  dup execute count dup if  (+attribute)  else  2drop drop  then
  ;
: echo_raw_attribute  ( xt -- )
  \ Print the special raw attribute, if not empty.
  \ xt = execution token of the raw attribute variable.
  execute count dup if  echo_space echo echo_space  else  2drop  then
  ;
: echo_real_attributes  ( a u -- )
  \ Print all non-empty real attributes.
  \ a = address of the first attribute's xt
  \ u = count
  cells bounds ?do
    i @ echo_real_attribute
  cell +loop
  ;
: echo_attributes  ( -- )
  \ Print all non-empty attributes.
  'attributes_xt #attributes 1-
  2dup echo_real_attributes
  cells + @ echo_raw_attribute
  ;

.( fendo_markup_html_attributes.fs compiled) cr

