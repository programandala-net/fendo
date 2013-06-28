.( fendo_markup_html_tags.fs ) cr

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

\ 2013-06-10 Start. Factored from <fendo_markup_html.fs>.
\ 2013-06-15 Void tags are closed depending on HTML or XHTML
\   syntaxes. Tag alias in both syntaxes.

\ **************************************************************
\ Printing

: "/>"  ( -- ca len )
  \ Return the closing of a void HTML tag,
  \ ">" in HTML syntax or "/>" in XHTML syntax.
  s" />" xhtml? @ 0= if  +/string  then
  ;
: {html}  ( ca len -- )
  \ Print an empty HTML tag (e.g. <br/>, <hr/>),
  \ with all previously defined attributes.
  \ ca len = HTML tag
  s" <" echo echo echo_attributes "/>" echo
  separate? off  -attributes
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
\ HTML tags

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
: <br>  ( -- )  s" br" {html}  ;
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
: <hr>  ( -- )  s" hr" {html}  ;
: <html>  ( -- )  s" html" {html  ;
: </html>  ( -- )  echo_cr s" html" html}  ;
: <i>  ( -- )  s" i" {html  ;
: </i>  ( -- )  s" i" html}  ;
: <iframe>  ( -- )  s" iframe" {html  ;
: </iframe>  ( -- )  s" iframe" html}  ;
: <img>  ( -- )  s" img" {html}  ;
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

\ XHTML tags

markup>order
' <br> alias <br/>
' <hr> alias <hr/>
' <img> alias <img/>
markup<order

set-current

.( fendo_markup_html_tags.fs compiled) cr


