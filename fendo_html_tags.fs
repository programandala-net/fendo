.( fendo_html_tags.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the HTML tags.

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

\ Fendo is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-06-01 Start.
\ 2013-06-02 More tags. Start of the attribute management.
\ 2013-06-04 Fix: 'count' was missing for string variables.
\ 2013-06-06 Improvement: simpler attribute definition: one
\   defining word creates all required words, and the attributes'
\   xt are stored in a table in order to manage all of them in a
\   loop (e.g. for initialization or printing).
\ 2013-06-06 Change: HTML entities moved here from <fendo_markup.fs>.

\ **************************************************************
\ Todo

\ Is this file necessary? HTML tags can be used freely in the
\ contents. Are these words are useful only inside other words?

\ 2013-06-04 The word '&#' parses a number and echoes the
\ corresponding HTML entity.

\ **************************************************************
\ Requirements

require galope/3dup.fs
require galope/svariable.fs

\ **************************************************************
\ Generic tool words for markup

[undefined] fendo_markup_voc [if]
  vocabulary fendo_markup_voc 
  : [fendo_markup_voc]  ( -- )
    also fendo_markup_voc
    ;  immediate
[then]

\ **************************************************************
\ Tool words for HTML attributes

: :attribute@  ( ca len xt -- )
  \ Create a word that fetchs an attribute.
  \ ca len = name of the attribute variable
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  >r
  s" @" s+ nextname create  \ create a word with the name 'attribute@'.
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
  get-current also fendo_markup_voc definitions
  parse-name? abort" Parseable name expected in 'attribute:'"
  2dup :svariable latestxt  ( ca len xt )  dup >r
  3dup :attribute" 3dup :attribute! :attribute@
  previous set-current  r>
  ;

depth [if]
  .( The stack must be empty before defining the attributes.) abort
[then]

\ Note: the '=' ending is a compromise to avoid name clashes (e.g. 'align')

\ The first attribute defined (the last in the table) is a
\ special one that lets to include any raw content in the HTML
\ tag, without label or surrounding quotes:
attribute: raw=

\ Real attributes:
attribute: align=
attribute: alt=
attribute: class=
attribute: height=
attribute: href=
attribute: hreflang=
attribute: id=
attribute: src=
attribute: style=
attribute: width=

depth constant #attributes  \ count of defined attributes
create 'attributes_xt  \ table for the xt of the attribute variables
#attributes 0 [?do]  ,  [loop]  \ fill the table

: -attributes  ( -- )
  \ Clear all HTML attributes.
  'attributes_xt #attributes cells bounds do
    i perform off
  cell +loop
  ;

: ((+attribute))  ( ca1 len1 ca2 len2 -- )
  \ Print an attribute.
  \ ca1 len1 = attribute value
  \ ca2 len2 = attribute label
  echo_space s" =" s+ echo echo_quote echo echo_quote
  ;
: (+attribute)  ( xt ca len -- )
  \ Print an attribute.
  \ xt = execution token of the attribute variable
  \ ca1 len1 = attribute value
  rot >name name>string ((+attribute))
  ;
: +attribute  ( xt -- )
  \ Print an attribute, if not empty.
  \ xt = execution token of the attribute variable
  dup execute count ?dup if  (+attribute)  else  2drop  then
  ;
: echo_raw_attribute  ( xt -- )
  \ Print the special raw attribute, if not empty.
  \ xt = execution token of the raw attribute variable.
  execute count ?dup if  echo_space echo echo_space  else  2drop  then
  ;
: echo_real_attributes  ( a u -- )
  \ Print all non-empty real attributes.
  \ a = address of the first attribute's xt
  \ u = count
  cells bounds ?do
    i @ +attribute
  cell +loop
  ;
: echo_attributes  ( -- )
  \ Print all non-empty attributes.
  \ All but the last one are real attributes:
  'attributes_xt #attributes
  2dup -1 echo_real_attributes
  cells + @ echo_raw_attribute
  ;

\ **************************************************************
\ Tool words for HTML entities

: :echo_name   ( ca len -- )
  \ Create a word that prints its own name.
  \ ca len = word name 
  2dup nextname  create  s,
  does>  ( dfa )  count echo
  ;
: :entity   ( ca len -- )
  \ Create a HTML entity word. 
  \ ca len = entity --and name of its entity word
  :echo_name  separate? off
  ;
: entity:   ( "name" -- )
  \ Create a HTML entity word. 
  \ "name" = entity --and name of its entity word
  parse-name? abort" Parseable name expected in 'entity:'"
  :entity
  ;

\ **************************************************************
\ Tool words for HTML tags

: {html}  ( ca len -- )
  \ Print an empty HTML tag (e.g. <br/>, <hr/>),
  \ with all previously defined attributes.
  \ ca len = HTML tag
  s" <" echo echo echo_attributes s" />" echo  separate? off
  -attributes
  ;
: {html  ( ca len -- )
  \ Print an opening HTML tag (e.g. <p>, <a>),
  \ with all previously defined attributes.
  \ ca len = HTML tag
  s" <" _echo echo echo_attributes s" >" echo  separate? off
  -attributes
  ;
: html}  ( ca len -- )
  \ Print a closing HTML tag (e.g. </p>, </a>).
  \ ca len = HTML tag
  s" </" echo echo s" >" echo  separate? off
  ;

\ **************************************************************
\ Actual HTML markup

also fendo_markup_voc definitions

: <a>  ( -- )  s" a" {html  ;
: </a>  ( -- )  s" a" html}  ;
: <blockquote>  ( -- )  s" blockquote" {html  ;
: </blockquote>  ( -- )  s" blockquote" html}  ;
: <br/>  ( -- )  s" br" {html}  ;
: <code>  ( -- )  s" code" {html  ;
: </code>  ( -- )  s" code" html}  ;
: <dd>  ( -- )  s" dd" {html  ;
: </dd>  ( -- )  s" dd" html}  ;
: <dt>  ( -- )  s" dt" {html  ;
: </dt>  ( -- )  s" dt" html}  ;
: <del>  ( -- )  s" del" {html  ;
: </del>  ( -- )  s" del" html}  ;
: <dl>  ( -- )  s" dl" {html  ;
: </dl>  ( -- )  s" dl" html}  ;
: <div>  ( -- )  s" div" {html  ;
: </div>  ( -- )  s" div" html}  ;
: <em>  ( -- )  s" em" {html  ;
: </em>  ( -- )  s" em" html}  ;
: <h1>  ( -- )  s" h1" {html  ;
: </h1>  ( -- )  s" h1" html}  ;
: <h2>  ( -- )  s" h2" {html  ;
: </h2>  ( -- )  s" h2" html}  ;
: <h3>  ( -- )  s" h3" {html  ;
: </h3>  ( -- )  s" h3" html}  ;
: <h4>  ( -- )  s" h4" {html  ;
: </h4>  ( -- )  s" h4" html}  ;
: <h5>  ( -- )  s" h5" {html  ;
: </h5>  ( -- )  s" h5" html}  ;
: <h6>  ( -- )  s" h6" {html  ;
: </h6>  ( -- )  s" h6" html}  ;
: <hr/>  ( -- )  s" hr" {html}  ;
: <img/>  ( -- )  s" img" {html}  ;
: <li>  ( -- )  echo_cr s" li" {html  ;
: </li>  ( -- )  s" li" html}  ;
: <ol>  ( -- )  s" ol" {html  ;
: </ol>  ( -- )  echo_cr s" ol" html}  ;
: <p>  ( -- )  s" p" {html  ;
: </p>  ( -- )  s" p" html}  ;
: <pre>  ( -- )  s" pre" {html  ;
: </pre>  ( -- )  s" pre" html}  ;
: <q>  ( -- )  s" q" {html  ;
: </q>  ( -- )  s" q" html}  ;
: <span>  ( -- )  s" span" {html  ;
: </span>  ( -- )  s" span" html}  ;
: <strong>  ( -- )  s" strong" {html  ;
: </strong>  ( -- )  s" strong" html}  ;
: <table>  ( -- )  s" table" {html  ;
: </table>  ( -- )  s" table" html}  ;
: <td>  ( -- )  s" td" {html  ;
: </td>  ( -- )  s" td" html}  ;
: <th>  ( -- )  s" th" {html  ;
: </th>  ( -- )  s" th" html}  ;
: <tr>  ( -- )  s" tr" {html  ;
: </tr>  ( -- )  s" tr" html}  ;
: <ul>  ( -- )  s" ul" {html  ;
: </ul>  ( -- )  echo_cr s" ul" html}  ;

\ Entities

entity: &dquot;
entity: &gt;
entity: &lt;
entity: &nbsp;
entity: &squot;

previous 
fendo_voc definitions

.( fendo_html_tags.fs compiled) cr

