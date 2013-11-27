.( fendo_markup_html_tags.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

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
\ 2013-10-23 New: immediate version of some tags,
\   for the user's application.
\ 2013-10-26 New: immediate version of '<p>'.
\ 2013-10-27 New: '<link/>' and its immediate version '[<link/>]'.
\ 2013-10-30 New: More immediate versions of tags.
\ 2013-11-18 New: '[<br/>]', '[<hr/>]'.
\ 2013-11-26 New: Immediate version of definition lists tags.

\ **************************************************************
\ Requirements

require galope/plus-slash-string.fs

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
  s" <" _echo echo echo_attributes "/>" echo
  separate? off  -attributes
  ;
: {html  ( ca len -- )
  \ Print an opening HTML tag (e.g. <p>, <a>),
  \ with all previously defined attributes.
  \ ca len = HTML tag
\  ." start of {html -- " 2dup type cr  \ xxx informer
\  ." href= " href=@ type cr  \ xxx informer
  s" <" _echo echo echo_attributes s" >" echo  separate? off
  -attributes
\  ." end of {html -- href= " href=@ type cr  \ xxx informer
  ;
: html}  ( ca len -- )
  \ Print a closing HTML tag (e.g. </p>, </a>).
  \ ca len = HTML tag
  s" </" echo echo s" >" echo  separate? on
  ;

\ **************************************************************
\ HTML tags

get-current markup>current

: <a>  ( -- )  
\  ." 6)" href=@ type space id=@ type cr  \ xxx informer
  s" a" {html  ;
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
: </h1>  ( -- )  s" h1" html} \n  ;
: <h2>  ( -- )  s" h2" {html  ;
: </h2>  ( -- )  s" h2" html} \n  ;
: <h3>  ( -- )  s" h3" {html  ;
: </h3>  ( -- )  s" h3" html} \n  ;
: <h4>  ( -- )  s" h4" {html  ;
: </h4>  ( -- )  s" h4" html} \n  ;
: <h5>  ( -- )  s" h5" {html  ;
: </h5>  ( -- )  s" h5" html} \n  ;
: <h6>  ( -- )  s" h6" {html  ;
: </h6>  ( -- )  s" h6" html} \n  ;
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
false [if]  \ xxx old
: <link>  ( -- )  s" link" {html  ;
: </link>  ( -- )  s" link" html}  ;
[then]
: <link/>  ( -- )  s" link" {html}  ;
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
' <br/> alias <br>
' <hr/> alias <hr>
' <img/> alias <img>
markup<order

set-current

\ **************************************************************
\ Immediate version of some usual tags, in the Fendo wordlist, in
\ order to save code when they are used in the user's application (no
\ '[markup>order]' and '[markup<order]' are required).

markup>order
: [<a>]  ( -- )  postpone <a>  ;  immediate
: [</a>]  ( -- )  postpone </a>  ;  immediate
: [<br/>]  ( -- )  postpone <br/> postpone \n  ;  immediate
: [<caption>]  ( -- )  postpone <caption> postpone \n  ;  immediate
: [<code>]  ( -- )  postpone <code>  ;  immediate
: [</code>]  ( -- )  postpone </code>  ;  immediate
: [<dd>]  ( -- )  postpone <dd>  ;  immediate
: [</dd>]  ( -- )  postpone </dd> postpone \n  ;  immediate
: [<div>]  ( -- )  postpone <div>  ;  immediate
: [<dl>]  ( -- )  postpone <dl> postpone \n  ;  immediate
: [</dl>]  ( -- )  postpone </dl> postpone \n  ;  immediate
: [<dt>]  ( -- )  postpone <dt>  ;  immediate
: [</dt>]  ( -- )  postpone </dt> postpone \n  ;  immediate
: [<h1>]  ( -- )  postpone <h1>  ;  immediate
: [</h1>]  ( -- )  postpone </h1> postpone \n  ;  immediate
: [<h2>]  ( -- )  postpone <h2>  ;  immediate
: [</h2>]  ( -- )  postpone </h2> postpone \n  ;  immediate
: [<h3>]  ( -- )  postpone <h3>  ;  immediate
: [</h3>]  ( -- )  postpone </h3> postpone \n  ;  immediate
: [<h4>]  ( -- )  postpone <h4>  ;  immediate
: [</h4>]  ( -- )  postpone </h4> postpone \n  ;  immediate
: [<h5>]  ( -- )  postpone <h5>  ;  immediate
: [</h5>]  ( -- )  postpone </h5> postpone \n  ;  immediate
: [<h6>]  ( -- )  postpone <h6>  ;  immediate
: [</h6>]  ( -- )  postpone </h6> postpone \n  ;  immediate
: [<hr/>]  ( -- )  postpone <hr/> postpone \n  ;  immediate
: [<img>]  ( -- )  postpone <img>  ;  immediate
: [<li>]  ( -- )  postpone <li>  ;  immediate
: [</li>]  ( -- )  postpone </li> postpone \n  ;  immediate
: [<link/>]  ( -- )  postpone <link/> postpone \n  ;  immediate
: [<ol>]  ( -- )  postpone <ol> postpone \n  ;  immediate
: [</ol>]  ( -- )  postpone </ol> postpone \n  ;  immediate
: [<p>]  ( -- )  postpone <p>  ;  immediate
: [</p>]  ( -- )  postpone </p> postpone \n  ;  immediate
: [<pre>]  ( -- )  postpone <pre>  ;  immediate
: [</pre>]  ( -- )  postpone </pre> postpone \n  ;  immediate
: [<span>]  ( -- )  postpone <span>  ;  immediate
: [</span>]  ( -- )  postpone </span>  ;  immediate
: [<table>]  ( -- )  postpone <table> postpone \n  ;  immediate
: [</td>]  ( -- )  postpone </td> postpone \n  ;  immediate
: [</th>]  ( -- )  postpone </th> postpone \n  ;  immediate
: [<tr>]  ( -- )  postpone <tr>  ;  immediate
: [</tr>]  ( -- )  postpone </tr> postpone \n  ;  immediate
: [<ul>]  ( -- )  postpone <ul> postpone \n  ;  immediate
: [</ul>]  ( -- )  postpone </ul> postpone \n  ;  immediate
markup<order

\ **************************************************************
\ Tag shortcuts used by the wiki markup and some addons

: block_source_code{  ( -- )
  [<pre>] [<code>]
  ;
: }block_source_code  ( -- )
  [</code>] [</pre>]
  ;

.( fendo_markup_html_tags.fs compiled) cr


