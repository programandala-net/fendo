.( fendo.markup.html.tags.fs ) cr

\ This file is part of Fendo.

\ This file defines the HTML tags.

\ Copyright (C) 2013,2014,2015 Marcos Cruz (programandala.net)

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
require galope/plus-slash-string.fs
fendo_definitions

\ **************************************************************
\ Printing

: "/>"  ( -- ca len )
  \ Return the closing of a void HTML tag,
  \ ">" in HTML syntax or "/>" in XHTML syntax.
  s" />" xhtml? 0= if  +/string  then
  ;
: (html{})  ( ca len -- )
  \ Start an empty HTML tag.
  \ ca len = HTML tag
  s" <" echo echo echo_attributes
  ;
: (html{)  ( ca len -- )
  \ Start an opening HTML tag.
  \ ca len = HTML tag
\  ." Parameter in '(html{)' = " 2dup type  \ XXX INFORMER
\  ." ; 'separate?' = " separate? ? cr  \ XXX INFORMER
  s" <" _echo echo echo_attributes
  ;
: (}html)  ( -- )
  \ Common tasks after an empty or opening HTML tag.
  separate? off  -attributes
  ;
: {html}  ( ca len -- )
  \ Print an empty HTML tag (e.g. <br/>, <hr/>),
  \ with all previously defined attributes.
  \ ca len = HTML tag
  (html{}) "/>" echo (}html)
  ;
: {html  ( ca len -- )
  \ Print an opening HTML tag (e.g. <p>, <a>),
  \ with all previously defined attributes.
  \ ca len = HTML tag
\  ." start of {html -- " 2dup type cr  \ xxx informer
\  ." href= " href=@ type cr  \ xxx informer
  (html{) s" >" echo (}html)
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

: <a>  ( -- )  s" a" {html  ;
: </a>  ( -- )  s" a" html}  ;
: <abbr>  ( -- )  s" abbr" {html  ;
: </abbr>  ( -- )  s" abbr" html}  ;
: <address>  ( -- )  s" address" {html  ;
: </address>  ( -- )  s" address" html}  ;
: <area>  ( -- )  s" area" {html  ;
: </area>  ( -- )  s" area" html}  ;
: <article>  ( -- )  s" article" {html  ;
: </article>  ( -- )  \n s" article" html}  ;
: <aside>  ( -- )  s" aside" {html  ;
: </aside>  ( -- )  \n s" aside" html}  ;
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
: <blockquote>  ( -- )  \n s" blockquote" {html  ;
: </blockquote>  ( -- )  \n s" blockquote" html}  ;
: <body>  ( -- )  \n s" body" {html  ;
: </body>  ( -- )  \n s" body" html}  ;
: <br/>  ( -- )  s" br" {html}  ;
: <button>  ( -- )  s" button" {html  ;
: </button>  ( -- )  s" button" html}  ;
: <canvas>  ( -- )  s" canvas" {html  ;
: </canvas>  ( -- )  s" canvas" html}  ;
: <caption>  ( -- )  \n s" caption" {html  ;
: </caption>  ( -- )  s" caption" html}  ;
: <cite>  ( -- )  s" cite" {html  ;
: </cite>  ( -- )  s" cite" html}  ;
: <code>  ( -- )  s" code" {html  ;
: </code>  ( -- )  s" code" html}  ;
: <col>  ( -- )  \n s" col" {html  ;
: </col>  ( -- )  \n s" col" html}  ;
: <colgroup>  ( -- )  \n s" colgroup" {html  ;
: </colgroup>  ( -- )  \n s" colgroup" html}  ;
: <dd>  ( -- )  \n s" dd" {html  ;
: </dd>  ( -- )  s" dd" html}  ;
: <del>  ( -- )  s" del" {html  ;
: </del>  ( -- )  s" del" html}  ;
: <dfn>  ( -- )  s" dfn" {html  ;
: </dfn>  ( -- )  s" dfn" html}  ;
: <div>  ( -- )  \n s" div" {html  ;
: </div>  ( -- )  \n s" div" html}  ;
: <dl>  ( -- )  \n s" dl" {html  ;
: </dl>  ( -- )  \n s" dl" html}  ;
: <dt>  ( -- )  \n s" dt" {html  ;
: </dt>  ( -- )  s" dt" html}  ;
: <em>  ( -- )  s" em" {html  ;
: </em>  ( -- )  s" em" html}  ;
: <embed>  ( -- )  \n s" embed" {html  ;
: </embed>  ( -- )  s" embed" html}  ;
: <figure>  ( -- )  s" figure" {html  ;
: </figure>  ( -- )  s" figure" html}  ;
: <figcaption>  ( -- )  s" figcaption" {html  ;
: </figcaption>  ( -- )  s" figcaption" html}  ;
: <fieldset>  ( -- )  \n s" fieldset" {html  ;
: </fieldset>  ( -- )  s" fieldset" html}  ;
: <form>  ( -- )  \n s" form" {html  ;
: </form>  ( -- )  s" form" html}  ;
: <footer>  ( -- )  \n s" footer" {html  ;
: </footer>  ( -- )  \n s" footer" html}  ;
: <h1>  ( -- )  \n s" h1" {html  ;
: </h1>  ( -- )  s" h1" html} \n  ;
: <h2>  ( -- )  \n s" h2" {html  ;
: </h2>  ( -- )  s" h2" html} \n  ;
: <h3>  ( -- )  \n s" h3" {html  ;
: </h3>  ( -- )  s" h3" html} \n  ;
: <h4>  ( -- )  \n s" h4" {html  ;
: </h4>  ( -- )  s" h4" html} \n  ;
: <h5>  ( -- )  \n s" h5" {html  ;
: </h5>  ( -- )  s" h5" html} \n  ;
: <h6>  ( -- )  \n s" h6" {html  ;
: </h6>  ( -- )  s" h6" html} \n  ;
: <head>  ( -- )  \n s" head" {html  ;
: </head>  ( -- )  \n s" head" html}  ;
: <header>  ( -- )  \n s" header" {html  ;
: </header>  ( -- )  \n s" header" html}  ;
: <hgroup>  ( -- )  \n s" hgroup" {html  ;
: </hgroup>  ( -- )  \n s" hgroup" html}  ;
: <hr/>  ( -- )  \n s" hr" {html}  ;
: <html>  ( -- )  \n s" html" {html  ;
: </html>  ( -- )  \n s" html" html}  ;
: <i>  ( -- )  s" i" {html  ;
: </i>  ( -- )  s" i" html}  ;
: <iframe>  ( -- )  \n s" iframe" {html  ;
: </iframe>  ( -- )  \n s" iframe" html}  ;
: <img/>  ( -- )  s" img" {html}  ;
: <input>  ( -- )  \n s" input" {html  ;
: </input>  ( -- )  s" input" html}  ;
: <ins>  ( -- )  s" ins" {html  ;
: </ins>  ( -- )  s" ins" html}  ;
: <kbd>  ( -- )  s" kbd" {html  ;
: </kbd>  ( -- )  s" kbd" html}  ;
: <label>  ( -- )  s" label" {html  ;
: </label>  ( -- )  s" label" html}  ;
: <legend>  ( -- )  s" legend" {html  ;
: </legend>  ( -- )  s" legend" html}  ;
: <li>  ( -- )  \n s" li" {html  ;
: </li>  ( -- )  s" li" html}  ;
: <link/>  ( -- )  \n s" link" {html}  ;
: <map>  ( -- )  \n s" map" {html  ;
: </map>  ( -- )  \n s" map" html}  ;
: <mark>  ( -- )  s" mark" {html  ;
: </mark>  ( -- )  s" mark" html}  ;
: <meta>  ( -- )  \n s" meta" {html  ;
: </meta>  ( -- )  s" meta" html}  ;
: <meta/>  ( -- )  \n s" meta" {html}  ;
: <nav>  ( -- )  \n s" nav" {html  ;
: </nav>  ( -- )  \n s" nav" html}  ;
: <noscript>  ( -- )  \n s" noscript" {html  ;
: </noscript>  ( -- )  \n s" noscript" html}  ;
: <object>  ( -- )  \n s" object" {html  ;
: </object>  ( -- )  \n s" object" html}  ;
: <ol>  ( -- )  \n s" ol" {html  ;
: </ol>  ( -- )  \n s" ol" html}  ;
: <p>  ( -- )  \n s" p" {html  ;
: </p>  ( -- )  \n s" p" html}  ;
: <param>  ( -- )  \n s" param" {html  ;
: </param>  ( -- )  s" param" html}  ;
: <pre>  ( -- )  \n s" pre" {html  ;
: </pre>  ( -- )  \n s" pre" html}  ;
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
: <script>  ( -- )  \n s" script" {html  ;
: </script>  ( -- )  \n s" script" html}  ;
: <section>  ( -- )  \n s" section" {html  ;
: </section>  ( -- )  \n s" section" html}  ;
: <select>  ( -- )  \n s" select" {html  ;  \ xxx latest tag copied from http://dev.w3.org/html5/markup/elements-by-function.html
: </select>  ( -- )  s" select" html}  ;
: <small>  ( -- )  s" small" {html  ;
: </small>  ( -- )  s" small" html}  ;
: <source>  ( -- )  \n s" source" {html  ;
: </source>  ( -- )  s" source" html}  ;
: <span>  ( -- )  s" span" {html  ;
: </span>  ( -- )  s" span" html}  ;
: <strong>  ( -- )  s" strong" {html  ;
: </strong>  ( -- )  s" strong" html}  ;
: <style>  ( -- )  \n s" style" {html  ;
: </style>  ( -- )  \n s" style" html}  ;
: <sub>  ( -- )  s" sub" {html  ;
: </sub>  ( -- )  s" sub" html}  ;
: <sup>  ( -- )  s" sup" {html  ;
: </sup>  ( -- )  s" sup" html}  ;
: <table>  ( -- )  \n s" table" {html table_started? on  ;
: </table>  ( -- )  \n s" table" html} table_started? off  ;
: <tbody>  ( -- )  \n s" tbody" {html  ;
: </tbody>  ( -- )  \n s" tbody" html}  ;
: <td>  ( -- )  \n s" td" {html  header_cell? off  ;
: </td>  ( -- )  s" td" html}  ;
: <tfoot>  ( -- )  \n s" tfoot" {html  ;
: </tfoot>  ( -- )  \n s" tfoot" html}  ;
: <th>  ( -- )  \n s" th" {html  header_cell? on  ;
: </th>  ( -- )  s" th" html}  ;
: <thead>  ( -- )  \n s" thead" {html  ;
: </thead>  ( -- )  \n s" thead" html}  ;
: <time>  ( -- )  s" time" {html  ;
: </time>  ( -- )  s" time" html}  ;
: <title>  ( -- )  \n s" title" {html  ;
: </title>  ( -- )  s" title" html}  ;
: <tr>  ( -- )  \n s" tr" {html  ;
: </tr>  ( -- )  s" tr" html} ;
: <track>  ( -- )  s" track" {html  ;
: </track>  ( -- )  s" track" html}  ;
: <u>  ( -- )  s" u" {html  ;
: </u>  ( -- )  s" u" html}  ;
: <ul>  ( -- )  \n s" ul" {html  ;
: </ul>  ( -- )  \n s" ul" html}  ;
: <var>  ( -- )  s" var" {html  ;
: </var>  ( -- )  s" var" html}  ;
: <video>  ( -- )  \n s" video" {html  ;
: </video>  ( -- )  \n s" video" html}  ;
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
: [-->]  ( -- )  postpone -->  ;  immediate
: [<!--]  ( -- )  postpone <!--  ;  immediate
: [</a>]  ( -- )  postpone </a>  ;  immediate
: [</abbr>]  ( -- )  postpone </abbr>  ;  immediate
: [</address>]  ( -- )  postpone </address>  ;  immediate
: [</area>]  ( -- )  postpone </area>  ;  immediate
: [</article>]  ( -- )  postpone </article>  ;  immediate
: [</aside>]  ( -- )  postpone </aside>  ;  immediate
: [</audio>]  ( -- )  postpone </audio>  ;  immediate
: [</b>]  ( -- )  postpone </b>  ;  immediate
: [</base>]  ( -- )  postpone </base>  ;  immediate
: [</bdi>]  ( -- )  postpone </bdi>  ;  immediate
: [</bdo>]  ( -- )  postpone </bdo>  ;  immediate
: [</blockquote>]  ( -- )  postpone </blockquote>  ;  immediate
: [</body>]  ( -- )  postpone </body>  ;  immediate
: [</button>]  ( -- )  postpone </button>  ;  immediate
: [</canvas>]  ( -- )  postpone </canvas>  ;  immediate
: [</caption>]  ( -- )  postpone </caption>  ;  immediate
: [</cite>]  ( -- )  postpone </cite>  ;  immediate
: [</code>]  ( -- )  postpone </code>  ;  immediate
: [</col>]  ( -- )  postpone </col>  ;  immediate
: [</colgroup>]  ( -- )  postpone </colgroup>  ;  immediate
: [</dd>]  ( -- )  postpone </dd>  ;  immediate
: [</del>]  ( -- )  postpone </del>  ;  immediate
: [</dfn>]  ( -- )  postpone </dfn>  ;  immediate
: [</div>]  ( -- )  postpone </div>  ;  immediate
: [</dl>]  ( -- )  postpone </dl>  ;  immediate
: [</dt>]  ( -- )  postpone </dt>  ;  immediate
: [</em>]  ( -- )  postpone </em>  ;  immediate
: [</embed>]  ( -- )  postpone </embed>  ;  immediate
: [</fieldset>]  ( -- )  postpone </fieldset>  ;  immediate
: [</figcaption>]  ( -- )  postpone </figcaption>  ;  immediate
: [</figure>]  ( -- )  postpone </figure>  ;  immediate
: [</footer>]  ( -- )  postpone </footer>  ;  immediate
: [</form>]  ( -- )  postpone </form>  ;  immediate
: [</h1>]  ( -- )  postpone </h1>  ;  immediate
: [</h2>]  ( -- )  postpone </h2>  ;  immediate
: [</h3>]  ( -- )  postpone </h3>  ;  immediate
: [</h4>]  ( -- )  postpone </h4>  ;  immediate
: [</h5>]  ( -- )  postpone </h5>  ;  immediate
: [</h6>]  ( -- )  postpone </h6>  ;  immediate
: [</head>]  ( -- )  postpone </head>  ;  immediate
: [</header>]  ( -- )  postpone </header>  ;  immediate
: [</hgroup>]  ( -- )  postpone </hgroup>  ;  immediate
: [</html>]  ( -- )  postpone </html>  ;  immediate
: [</i>]  ( -- )  postpone </i>  ;  immediate
: [</iframe>]  ( -- )  postpone </iframe>  ;  immediate
: [</input>]  ( -- )  postpone </input>  ;  immediate
: [</ins>]  ( -- )  postpone </ins>  ;  immediate
: [</kbd>]  ( -- )  postpone </kbd>  ;  immediate
: [</label>]  ( -- )  postpone </label>  ;  immediate
: [</legend>]  ( -- )  postpone </legend>  ;  immediate
: [</li>]  ( -- )  postpone </li>  ;  immediate
: [</map>]  ( -- )  postpone </map>  ;  immediate
: [</mark>]  ( -- )  postpone </mark>  ;  immediate
: [</meta>]  ( -- )  postpone </meta>  ;  immediate
: [</nav>]  ( -- )  postpone </nav>  ;  immediate
: [</noscript>]  ( -- )  postpone </noscript>  ;  immediate
: [</object>]  ( -- )  postpone </object>  ;  immediate
: [</ol>]  ( -- )  postpone </ol>  ;  immediate
: [</p>]  ( -- )  postpone </p>  ;  immediate
: [</param>]  ( -- )  postpone </param>  ;  immediate
: [</pre>]  ( -- )  postpone </pre>  ;  immediate
: [</q>]  ( -- )  postpone </q>  ;  immediate
: [</rp>]  ( -- )  postpone </rp>  ;  immediate
: [</rt>]  ( -- )  postpone </rt>  ;  immediate
: [</ruby>]  ( -- )  postpone </ruby>  ;  immediate
: [</s>]  ( -- )  postpone </s>  ;  immediate
: [</samp>]  ( -- )  postpone </samp>  ;  immediate
: [</script>]  ( -- )  postpone </script>  ;  immediate
: [</section>]  ( -- )  postpone </section>  ;  immediate
: [</select>]  ( -- )  postpone </select>  ;  immediate
: [</small>]  ( -- )  postpone </small>  ;  immediate
: [</source>]  ( -- )  postpone </source>  ;  immediate
: [</span>]  ( -- )  postpone </span>  ;  immediate
: [</strong>]  ( -- )  postpone </strong>  ;  immediate
: [</style>]  ( -- )  postpone </style>  ;  immediate
: [</sub>]  ( -- )  postpone </sub>  ;  immediate
: [</sup>]  ( -- )  postpone </sup>  ;  immediate
: [</table>]  ( -- )  postpone </table>  ;  immediate
: [</tbody>]  ( -- )  postpone </tbody>  ;  immediate
: [</td>]  ( -- )  postpone </td>  ;  immediate
: [</tfoot>]  ( -- )  postpone </tfoot>  ;  immediate
: [</th>]  ( -- )  postpone </th>  ;  immediate
: [</thead>]  ( -- )  postpone </thead>  ;  immediate
: [</time>]  ( -- )  postpone </time>  ;  immediate
: [</title>]  ( -- )  postpone </title>  ;  immediate
: [</tr>]  ( -- )  postpone </tr>  ;  immediate
: [</track>]  ( -- )  postpone </track>  ;  immediate
: [</u>]  ( -- )  postpone </u>  ;  immediate
: [</ul>]  ( -- )  postpone </ul>  ;  immediate
: [</var>]  ( -- )  postpone </var>  ;  immediate
: [</video>]  ( -- )  postpone </video>  ;  immediate
: [<a>]  ( -- )  postpone <a>  ;  immediate
: [<abbr>]  ( -- )  postpone <abbr>  ;  immediate
: [<address>]  ( -- )  postpone <address>  ;  immediate
: [<area>]  ( -- )  postpone <area>  ;  immediate
: [<article>]  ( -- )  postpone <article>  ;  immediate
: [<aside>]  ( -- )  postpone <aside>  ;  immediate
: [<audio>]  ( -- )  postpone <audio>  ;  immediate
: [<b>]  ( -- )  postpone <b>  ;  immediate
: [<base>]  ( -- )  postpone <base>  ;  immediate
: [<bdi>]  ( -- )  postpone <bdi>  ;  immediate
: [<bdo>]  ( -- )  postpone <bdo>  ;  immediate
: [<blockquote>]  ( -- )  postpone <blockquote>  ;  immediate
: [<body>]  ( -- )  postpone <body>  ;  immediate
: [<br/>]  ( -- )  postpone <br/>  ;  immediate
: [<br>]  ( -- )  postpone <br>  ;  immediate
: [<button>]  ( -- )  postpone <button>  ;  immediate
: [<canvas>]  ( -- )  postpone <canvas>  ;  immediate
: [<caption>]  ( -- )  postpone <caption>  ;  immediate
: [<cite>]  ( -- )  postpone <cite>  ;  immediate
: [<code>]  ( -- )  postpone <code>  ;  immediate
: [<col>]  ( -- )  postpone <col>  ;  immediate
: [<colgroup>]  ( -- )  postpone <colgroup>  ;  immediate
: [<dd>]  ( -- )  postpone <dd>  ;  immediate
: [<del>]  ( -- )  postpone <del>  ;  immediate
: [<dfn>]  ( -- )  postpone <dfn>  ;  immediate
: [<div>]  ( -- )  postpone <div>  ;  immediate
: [<dl>]  ( -- )  postpone <dl>  ;  immediate
: [<dt>]  ( -- )  postpone <dt>  ;  immediate
: [<em>]  ( -- )  postpone <em>  ;  immediate
: [<embed>]  ( -- )  postpone <embed>  ;  immediate
: [<fieldset>]  ( -- )  postpone <fieldset>  ;  immediate
: [<figcaption>]  ( -- )  postpone <figcaption>  ;  immediate
: [<figure>]  ( -- )  postpone <figure>  ;  immediate
: [<footer>]  ( -- )  postpone <footer>  ;  immediate
: [<form>]  ( -- )  postpone <form>  ;  immediate
: [<h1>]  ( -- )  postpone <h1>  ;  immediate
: [<h2>]  ( -- )  postpone <h2>  ;  immediate
: [<h3>]  ( -- )  postpone <h3>  ;  immediate
: [<h4>]  ( -- )  postpone <h4>  ;  immediate
: [<h5>]  ( -- )  postpone <h5>  ;  immediate
: [<h6>]  ( -- )  postpone <h6>  ;  immediate
: [<head>]  ( -- )  postpone <head>  ;  immediate
: [<header>]  ( -- )  postpone <header>  ;  immediate
: [<hgroup>]  ( -- )  postpone <hgroup>  ;  immediate
: [<hr/>]  ( -- )  postpone <hr/>  ;  immediate
: [<hr>]  ( -- )  postpone <hr>  ;  immediate
: [<html>]  ( -- )  postpone <html>  ;  immediate
: [<i>]  ( -- )  postpone <i>  ;  immediate
: [<iframe>]  ( -- )  postpone <iframe>  ;  immediate
: [<img/>]  ( -- )  postpone <img/>  ;  immediate
: [<img>]  ( -- )  postpone <img>  ;  immediate
: [<input>]  ( -- )  postpone <input>  ;  immediate
: [<ins>]  ( -- )  postpone <ins>  ;  immediate
: [<kbd>]  ( -- )  postpone <kbd>  ;  immediate
: [<label>]  ( -- )  postpone <label>  ;  immediate
: [<legend>]  ( -- )  postpone <legend>  ;  immediate
: [<li>]  ( -- )  postpone <li>  ;  immediate
: [<link/>]  ( -- )  postpone <link/>  ;  immediate
: [<map>]  ( -- )  postpone <map>  ;  immediate
: [<mark>]  ( -- )  postpone <mark>  ;  immediate
: [<meta/>]  ( -- )  postpone <meta/>  ;  immediate
: [<meta>]  ( -- )  postpone <meta>  ;  immediate
: [<nav>]  ( -- )  postpone <nav>  ;  immediate
: [<noscript>]  ( -- )  postpone <noscript>  ;  immediate
: [<object>]  ( -- )  postpone <object>  ;  immediate
: [<ol>]  ( -- )  postpone <ol>  ;  immediate
: [<p>]  ( -- )  postpone <p>  ;  immediate
: [<param>]  ( -- )  postpone <param>  ;  immediate
: [<pre>]  ( -- )  postpone <pre>  ;  immediate
: [<q>]  ( -- )  postpone <q>  ;  immediate
: [<rp>]  ( -- )  postpone <rp>  ;  immediate
: [<rt>]  ( -- )  postpone <rt>  ;  immediate
: [<ruby>]  ( -- )  postpone <ruby>  ;  immediate
: [<s>]  ( -- )  postpone <s>  ;  immediate
: [<samp>]  ( -- )  postpone <samp>  ;  immediate
: [<script>]  ( -- )  postpone <script>  ;  immediate
: [<section>]  ( -- )  postpone <section>  ;  immediate
: [<select>]  ( -- )  postpone <select>  ;  immediate
: [<small>]  ( -- )  postpone <small>  ;  immediate
: [<source>]  ( -- )  postpone <source>  ;  immediate
: [<span>]  ( -- )  postpone <span>  ;  immediate
: [<strong>]  ( -- )  postpone <strong>  ;  immediate
: [<style>]  ( -- )  postpone <style>  ;  immediate
: [<sub>]  ( -- )  postpone <sub>  ;  immediate
: [<sup>]  ( -- )  postpone <sup>  ;  immediate
: [<table>]  ( -- )  postpone <table>  ;  immediate
: [<tbody>]  ( -- )  postpone <tbody>  ;  immediate
: [<td>]  ( -- )  postpone <td>  ;  immediate
: [<tfoot>]  ( -- )  postpone <tfoot>  ;  immediate
: [<th>]  ( -- )  postpone <th>  ;  immediate
: [<thead>]  ( -- )  postpone <thead>  ;  immediate
: [<time>]  ( -- )  postpone <time>  ;  immediate
: [<title>]  ( -- )  postpone <title>  ;  immediate
: [<tr>]  ( -- )  postpone <tr>  ;  immediate
: [<track>]  ( -- )  postpone <track>  ;  immediate
: [<u>]  ( -- )  postpone <u>  ;  immediate
: [<ul>]  ( -- )  postpone <ul>  ;  immediate
: [<var>]  ( -- )  postpone <var>  ;  immediate
: [<video>]  ( -- )  postpone <video>  ;  immediate
markup<order

\ **************************************************************
\ Tag shortcuts used by the Fendo markup and some addons

: block_source_code{  ( -- )
  [<pre>] [<code>]
  ;
: }block_source_code  ( -- )
  [</code>] [</pre>]
  ;

\ **************************************************************
\ Change history of this file

\ 2013-06-10: Start. Factored from <fendo_markup_html.fs>.
\
\ 2013-06-15: Void tags are closed depending on HTML or XHTML
\ syntaxes. Tag alias in both syntaxes.
\
\ 2013-10-23: New: immediate version of some tags, for the user's
\ application.
\
\ 2013-10-26: New: immediate version of '<p>'.
\
\ 2013-10-27: New: '<link/>' and its immediate version '[<link/>]'.
\
\ 2013-10-30: New: More immediate versions of tags.
\
\ 2013-11-18: New: '[<br/>]', '[<hr/>]'.
\
\ 2013-11-26: New: Immediate version of definition lists tags.
\
\ 2013-11-30: New: Immediate version of <abbr> tags.
\
\ 2013-12-06: Change: '(html{)' and '(}html)' factored from '{html}'
\ and '{html'.
\
\ 2013-12-06: Change: carriage returns in tags have been corrected,
\ and removed from the immediate versions; this makes the final HTML
\ clearer.
\
\ 2014-02-15: New: '[<strong>]', '[</strong>]', '[</div>]'.
\
\ 2014-02-15: New: '(html{})' for empty tags (it does the same than
\ '(html{)' but without the separation).
\
\ 2014-07-13: New: '[<q>]', '[</q>]', '[<blockquote>]',
\ '[</blockquote>]'.
\
\ 2014-07-14: New: '[<title>]', '[</title>]', needed by the Atom
\ module; also '<meta/>'.
\
\ 2014-11-09: Fix: the '-attributes' in '(}html)' caused a problem
\ after the recent implementation of 'unmarkup': the 'href=' attribute
\ was deleted, because 'unmarkup' uses 'evaluate_content'. The
\ solution was to save and restore the attributes in
\ '(evaluate_content)' (defined in <fendo.parser.fs>).
\
\ 2014-11-18: Fix: 'separate? on' is added again at the end of
\ 'html}'.
\
\ 2014-12-13: New: All missing immediate versions of the HTML tags.
\ Now the list is complete.
\
\ 2015-02-01: Change: the 'xhtml?' variable is a value now.

.( fendo.markup.html.tags.fs compiled) cr
