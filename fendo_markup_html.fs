.( fendo_markup_html.fs ) cr

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
\ 2013-06-06 New: many new HTML tags and attributes.
\ 2013-06-06 Change: renamed from "fendo_html_tags.fs" to
\   "fendo_markup_html.fs"; the generic words are moved to the
\   new file <fendo_markup.fs>.

\ **************************************************************
\ Todo

\ 2013-06-04 The word '&#' parses a number and echoes the
\ corresponding HTML entity.

\ **************************************************************
\ Requirements

require galope/3dup.fs

\ **************************************************************
\ Tool words for HTML attributes

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
  parse-name? abort" Parseable name expected in 'attribute:'"
  2dup :svariable latestxt  ( ca len xt )  dup >r
  3dup :attribute" 3dup :attribute! :attribute@
  set-current  r>
  ;

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

depth constant #attributes  \ count of defined attributes
create 'attributes_xt  \ table for the xt of the attribute variables
#attributes 0 [?do]  ,  [loop]  \ fill the table

: -attributes  ( -- )
  \ Clear all HTML attributes.
  'attributes_xt #attributes 1- cells bounds ?do
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

\ **************************************************************
\ Tool words for HTML entities

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

get-current markup>current

: <a>  ( -- )  s" a" {html  ;
: </a>  ( -- )  s" a" html}  ;
: <abbr>  ( -- )  s" abbr" {html  ;
: </abbr>  ( -- )  s" abbr" html}  ;
: <address>  ( -- )  s" address" {html  ;
: </address>  ( -- )  s" address" html}  ;
: <area>  ( -- )  s" area" {html  ;
: </area>  ( -- )  s" area" html}  ;
: <article>  ( -- )  s" article" {html  ;
: </article>  ( -- )  echo_cr s" article" html}  ;
: <aside>  ( -- )  s" aside" {html  ;
: </aside>  ( -- )  echo_cr s" aside" html}  ;
: <audio>  ( -- )  s" audio" {html  ;
: </audio>  ( -- )  s" audio" html}  ;
: <b>  ( -- )  s" b" {html  ;
: </b>  ( -- )  s" b" html}  ;
: <base>  ( -- )  s" base" {html  ;
: </base>  ( -- )  s" base" html}  ;
: <bdi>  ( -- )  s" bdi" {html  ;
: </bdi>  ( -- )  s" bdi" html}  ;
: <bdo>  ( -- )  s" bdo" {html  ;
: </bdo>  ( -- )  s" bdo" html}  ;
: <blockquote>  ( -- )  s" blockquote" {html  ;
: </blockquote>  ( -- )  echo_cr s" blockquote" html}  ;
: <body>  ( -- )  s" body" {html  ;
: </body>  ( -- )  echo_cr s" body" html}  ;
: <br/>  ( -- )  s" br" {html}  ;
: <button>  ( -- )  s" button" {html  ;
: </button>  ( -- )  s" button" html}  ;
: <canvas>  ( -- )  s" canvas" {html  ;
: </canvas>  ( -- )  s" canvas" html}  ;
: <caption>  ( -- )  s" caption" {html  ;
: </caption>  ( -- )  s" caption" html}  ;
: <cite>  ( -- )  s" cite" {html  ;
: </cite>  ( -- )  s" cite" html}  ;
: <code>  ( -- )  s" code" {html  ;
: </code>  ( -- )  s" code" html}  ;
: <col>  ( -- )  s" col" {html  ;
: </col>  ( -- )  s" col" html}  ;
: <colgroup>  ( -- )  s" colgroup" {html  ;
: </colgroup>  ( -- )  s" colgroup" html}  ;
: <dd>  ( -- )  s" dd" {html  ;
: </dd>  ( -- )  s" dd" html}  ;
: <del>  ( -- )  s" del" {html  ;
: </del>  ( -- )  s" del" html}  ;
: <dfn>  ( -- )  s" dfn" {html  ;
: </dfn>  ( -- )  s" dfn" html}  ;
: <div>  ( -- )  s" div" {html  ;
: </div>  ( -- )  s" div" html}  ;
: <dl>  ( -- )  s" dl" {html  ;
: </dl>  ( -- )  s" dl" html}  ;
: <dt>  ( -- )  s" dt" {html  ;
: </dt>  ( -- )  s" dt" html}  ;
: <em>  ( -- )  s" em" {html  ;
: </em>  ( -- )  s" em" html}  ;
: <embed>  ( -- )  s" embed" {html  ;
: </embed>  ( -- )  s" embed" html}  ;
: <figure>  ( -- )  s" figure" {html  ;
: </figure>  ( -- )  s" figure" html}  ;
: <figcaption>  ( -- )  s" figcaption" {html  ;
: </figcaption>  ( -- )  s" figcaption" html}  ;
: <fieldset>  ( -- )  s" fieldset" {html  ;
: </fieldset>  ( -- )  s" fieldset" html}  ;
: <form>  ( -- )  s" form" {html  ;
: </form>  ( -- )  s" form" html}  ;
: <footer>  ( -- )  s" footer" {html  ;
: </footer>  ( -- )  s" footer" html}  ;
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
: <head>  ( -- )  s" head" {html  ;
: </head>  ( -- )  echo_cr s" head" html}  ;
: <header>  ( -- )  s" header" {html  ;
: </header>  ( -- )  s" header" html}  ;
: <hgroup>  ( -- )  echo_cr s" hgroup" {html  ;
: </hgroup>  ( -- )  echo_cr s" hgroup" html}  ;
: <hr/>  ( -- )  s" hr" {html}  ;
: <html>  ( -- )  s" html" {html  ;
: </html>  ( -- )  echo_cr s" html" html}  ;
: <i>  ( -- )  s" i" {html  ;
: </i>  ( -- )  s" i" html}  ;
: <iframe>  ( -- )  s" iframe" {html  ;
: </iframe>  ( -- )  s" iframe" html}  ;
: <img/>  ( -- )  s" img" {html}  ;
: <input>  ( -- )  s" input" {html  ;
: </input>  ( -- )  s" input" html}  ;
: <ins>  ( -- )  s" ins" {html  ;
: </ins>  ( -- )  s" ins" html}  ;
: <kbd>  ( -- )  s" kbd" {html  ;
: </kbd>  ( -- )  s" kbd" html}  ;
: <label>  ( -- )  s" label" {html  ;
: </label>  ( -- )  s" label" html}  ;
: <legend>  ( -- )  s" legend" {html  ;
: </legend>  ( -- )  s" legend" html}  ;
: <li>  ( -- )  echo_cr s" li" {html  ;
: </li>  ( -- )  s" li" html}  ;
: <link>  ( -- )  s" link" {html  ;
: </link>  ( -- )  s" link" html}  ;
: <map>  ( -- )  s" map" {html  ;
: </map>  ( -- )  s" map" html}  ;
: <mark>  ( -- )  s" mark" {html  ;
: </mark>  ( -- )  s" mark" html}  ;
: <meta>  ( -- )  s" meta" {html  ;
: </meta>  ( -- )  s" meta" html}  ;
: <nav>  ( -- )  echo_cr s" nav" {html  ;
: </nav>  ( -- )  echo_cr s" nav" html}  ;
: <noscript>  ( -- )  echo_cr s" noscript" {html  ;
: </noscript>  ( -- )  echo_cr s" noscript" html}  ;
: <object>  ( -- )  s" object" {html  ;
: </object>  ( -- )  s" object" html}  ;
: <ol>  ( -- )  s" ol" {html  ;
: </ol>  ( -- )  echo_cr s" ol" html}  ;
: <p>  ( -- )  s" p" {html  ;
: </p>  ( -- )  s" p" html}  ;
: <param>  ( -- )  s" param" {html  ;
: </param>  ( -- )  s" param" html}  ;
: <pre>  ( -- )  s" pre" {html  ;
: </pre>  ( -- )  s" pre" html}  ;
: <q>  ( -- )  s" q" {html  ;
: </q>  ( -- )  s" q" html}  ;
: <rp>  ( -- )  s" rp" {html  ;
: </rp>  ( -- )  s" rp" html}  ;
: <rt>  ( -- )  s" rt" {html  ;
: </rt>  ( -- )  s" rt" html}  ;
: <ruby>  ( -- )  s" ruby" {html  ;
: </ruby>  ( -- )  s" ruby" html}  ;
: <s>  ( -- )  s" s" {html  ;
: </s>  ( -- )  s" s" html}  ;
: <samp>  ( -- )  s" samp" {html  ;
: </samp>  ( -- )  s" samp" html}  ;
: <script>  ( -- )  echo_cr s" script" {html  ;
: </script>  ( -- )  echo_cr s" script" html}  ;
: <section>  ( -- )  echo_cr s" section" {html  ;
: </section>  ( -- )  echo_cr s" section" html}  ;
: <select>  ( -- )  s" select" {html  ;  \ xxx latest tag copied from http://dev.w3.org/html5/markup/elements-by-function.html
: </select>  ( -- )  s" select" html}  ;
: <small>  ( -- )  s" small" {html  ;
: </small>  ( -- )  s" small" html}  ;
: <source>  ( -- )  s" source" {html  ;
: </source>  ( -- )  s" source" html}  ;
: <span>  ( -- )  s" span" {html  ;
: </span>  ( -- )  s" span" html}  ;
: <strong>  ( -- )  s" strong" {html  ;
: </strong>  ( -- )  s" strong" html}  ;
: <style>  ( -- )  echo_cr s" style" {html  ;
: </style>  ( -- )  echo_cr s" style" html}  ;
: <sub>  ( -- )  s" sub" {html  ;
: </sub>  ( -- )  s" sub" html}  ;
: <sup>  ( -- )  s" sup" {html  ;
: </sup>  ( -- )  s" sup" html}  ;
: <table>  ( -- )  echo_cr s" table" {html table_started? on  ;
: </table>  ( -- )  echo_cr s" table" html} table_started? off  ;
: <tbody>  ( -- )  s" tbody" {html  ;
: </tbody>  ( -- )  s" tbody" html}  ;
: <td>  ( -- )  s" td" {html  header_cell? off  ;
: </td>  ( -- )  s" td" html}  ;
: <tfoot>  ( -- )  s" tfoot" {html  ;
: </tfoot>  ( -- )  s" tfoot" html}  ;
: <th>  ( -- )  s" th" {html  header_cell? on  ;
: </th>  ( -- )  s" th" html}  ;
: <thead>  ( -- )  s" thead" {html  ;
: </thead>  ( -- )  s" thead" html}  ;
: <time>  ( -- )  s" time" {html  ;
: </time>  ( -- )  s" time" html}  ;
: <title>  ( -- )  s" title" {html  ;
: </title>  ( -- )  s" title" html}  ;
: <tr>  ( -- )  echo_cr s" tr" {html  ;
: </tr>  ( -- )  s" tr" html} ;
: <track>  ( -- )  s" track" {html  ;
: </track>  ( -- )  s" track" html}  ;
: <u>  ( -- )  s" u" {html  ;
: </u>  ( -- )  s" u" html}  ;
: <ul>  ( -- )  s" ul" {html  ;
: </ul>  ( -- )  echo_cr s" ul" html}  ;
: <var>  ( -- )  s" var" {html  ;
: </var>  ( -- )  s" var" html}  ;
: <video>  ( -- )  s" video" {html  ;
: </video>  ( -- )  s" video" html}  ;
: <!--  ( -- )  s" <!--" echo  ;  \ xxx todo parse the comment, don't evaulate the markups
: -->  ( -- )  s" -->" echo  ;

\ Entities

entity: &dquot;
entity: &gt;
entity: &lt;
entity: &nbsp;
entity: &squot;

set-current

.( fendo_markup_html.fs compiled) cr

