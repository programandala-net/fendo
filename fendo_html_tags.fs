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

\ **************************************************************
\ Todo

\ Is this file necessary? HTML tags can be used freely in the
\ contents. Are these words are useful only inside other words?

\ All opening tags can have attributes and must reset all
\ attributes at the end.

\ **************************************************************
\ Requirements

require galope/svariable.fs

\ **************************************************************
\ Attributes

\ xxx todo complete
svariable alt
svariable class
svariable href
svariable hreflang
svariable id
svariable src
svariable style

: -attributes  ( -- )
  \ Clear all possible HTML attributes.
  \ xxx todo complete
  alt off
  class off
  href off
  hreflang off
  id off
  src off
  style off
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
  dup execute ?dup if  (+attribute)  else  2drop drop  then
  ;

  \ xxx todo complete
: +alt  ( -- )
  \ Print the "alt" attribute, if not empty.
  ['] alt +attribute
  ;
: +class  ( -- )
  \ Print the "class" attribute, if not empty.
  ['] class +attribute
  ;
: +href  ( -- )
  \ Print the "href" attribute, if not empty.
  ['] href +attribute
  ;
: +hreflang  ( -- )
  \ Print the "hreflang" attribute, if not empty.
  ['] hreflang +attribute
  ;
: +id  ( -- )
  \ Print the "id" attribute, if not empty.
  ['] id +attribute
  ;
: +src  ( -- )
  \ Print the "src" attribute, if not empty.
  ['] src +attribute
  ;
: +style  ( -- )
  \ Print the "style" attribute, if not empty.
  ['] style +attribute
  ;

: echo_attributes  ( -- )
  \ Print all non-empty attributes.
  +alt
  +class
  +href
  +hreflang
  +id
  +src
  +style
  ;

\ **************************************************************
\ HTML tags

: {html}  ( ca len -- )
  \ Print an empty HTML tag (eg. <br/>, <hr/>)
  \ ca len = HTML tag
  s" <" echo echo_attributes s" />" echo
  -attributes
  ;
: {html  ( ca len -- )
  \ Print an opening HTML tag.
  \ ca len = HTML tag
  s" <" echo echo_attributes s" >" echo
  -attributes
  ;
: html}  ( ca len -- )
  \ Print a closing HTML tag.
  \ ca len = HTML tag
  s" </" echo echo s" >" echo
  ;

\ **************************************************************
\ HTML actual markup

[undefined] fendo_markup_voc [if]
  vocabulary fendo_markup_voc 
[then]
also fendo_markup_voc definitions

\ xxx todo complete with attribute management
: <a>  ( -- )  s" a" {html  ;
: </a>  ( -- )  s" a" html}  ;
: <blockquote>  ( -- )  s" blockquote" {html  ;
: </blockquote>  ( -- )  s" blockquote" html}  ;
: <br/>  ( -- )  s" br />" {html}  ;
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
: <h1>  ( -- )  s" h1>" {html  ;
: </h1>  ( -- )  s" /h1>" html}  ;
: <h2>  ( -- )  s" h2>" {html  ;
: </h2>  ( -- )  s" /h2>" html}  ;
: <h3>  ( -- )  s" h3>" {html  ;
: </h3>  ( -- )  s" /h3>" html}  ;
: <h4>  ( -- )  s" h4>" {html  ;
: </h4>  ( -- )  s" /h4>" html}  ;
: <h5>  ( -- )  s" h5>" {html  ;
: </h5>  ( -- )  s" /h5>" html}  ;
: <h6>  ( -- )  s" h6>" {html  ;
: </h6>  ( -- )  s" /h6>" html}  ;
: <img>  ( -- )  s" img/>" {html}  ;
: <li>  ( -- )  s" li" {html  ;
: </li>  ( -- )  s" li" html}  ;
: <ol>  ( -- )  s" ol" {html  ;
: </ol>  ( -- )  s" ol" html}  ;
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
: <tr>  ( -- )  s" tr" {html  ;
: </tr>  ( -- )  s" tr" html}  ;
: <ul>  ( -- )  s" ul" {html  ;
: </ul>  ( -- )  s" ul" html}  ;

previous 
fendo_voc definitions

.( fendo_html_tags.fs compiled) cr

