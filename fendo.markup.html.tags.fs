.( fendo.markup.html.tags.fs ) cr

\ This file is part of Fendo.

\ This file defines the HTML tags.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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
  s" />" xhtml? @ 0= if  +/string  then
  ;
: (html{})  ( ca len -- )
  \ Start an empty HTML tag.
  \ ca len = HTML tag
  s" <" echo echo echo_attributes
  ;
: (html{)  ( ca len -- )
  \ Start an opening HTML tag.
  \ ca len = HTML tag
\  cr ." separate? in (html{) is " separate? ?  \ XXX INFORMER 2014-08-13
  s" <" _echo echo echo_attributes
  ;
: (}html)  ( -- )
  \ Common tasks after an empty or opening HTML tag.
  separate? off
\  s" About to clear the attributes in '(}html)'" type cr  \ XXX INFORMER
  -attributes  \ XXX FIXME this is the problem!
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
: <blockquote>  ( -- )  echo_cr s" blockquote" {html  ;
: </blockquote>  ( -- )  echo_cr s" blockquote" html}  ;
: <body>  ( -- )  echo_cr s" body" {html  ;
: </body>  ( -- )  echo_cr s" body" html}  ;
: <br/>  ( -- )  s" br" {html}  ;
: <button>  ( -- )  s" button" {html  ;
: </button>  ( -- )  s" button" html}  ;
: <canvas>  ( -- )  s" canvas" {html  ;
: </canvas>  ( -- )  s" canvas" html}  ;
: <caption>  ( -- )  echo_cr s" caption" {html  ;
: </caption>  ( -- )  s" caption" html}  ;
: <cite>  ( -- )  s" cite" {html  ;
: </cite>  ( -- )  s" cite" html}  ;
: <code>  ( -- )  s" code" {html  ;
: </code>  ( -- )  s" code" html}  ;
: <col>  ( -- )  echo_cr s" col" {html  ;
: </col>  ( -- )  echo_cr s" col" html}  ;
: <colgroup>  ( -- )  echo_cr s" colgroup" {html  ;
: </colgroup>  ( -- )  echo_cr s" colgroup" html}  ;
: <dd>  ( -- )  echo_cr s" dd" {html  ;
: </dd>  ( -- )  s" dd" html}  ;
: <del>  ( -- )  s" del" {html  ;
: </del>  ( -- )  s" del" html}  ;
: <dfn>  ( -- )  s" dfn" {html  ;
: </dfn>  ( -- )  s" dfn" html}  ;
: <div>  ( -- )  echo_cr s" div" {html  ;
: </div>  ( -- )  echo_cr s" div" html}  ;
: <dl>  ( -- )  echo_cr s" dl" {html  ;
: </dl>  ( -- )  echo_cr s" dl" html}  ;
: <dt>  ( -- )  echo_cr s" dt" {html  ;
: </dt>  ( -- )  s" dt" html}  ;
: <em>  ( -- )  s" em" {html  ;
: </em>  ( -- )  s" em" html}  ;
: <embed>  ( -- )  echo_cr s" embed" {html  ;
: </embed>  ( -- )  s" embed" html}  ;
: <figure>  ( -- )  s" figure" {html  ;
: </figure>  ( -- )  s" figure" html}  ;
: <figcaption>  ( -- )  s" figcaption" {html  ;
: </figcaption>  ( -- )  s" figcaption" html}  ;
: <fieldset>  ( -- )  echo_cr s" fieldset" {html  ;
: </fieldset>  ( -- )  s" fieldset" html}  ;
: <form>  ( -- )  echo_cr s" form" {html  ;
: </form>  ( -- )  s" form" html}  ;
: <footer>  ( -- )  echo_cr s" footer" {html  ;
: </footer>  ( -- )  echo_cr s" footer" html}  ;
: <h1>  ( -- )  echo_cr s" h1" {html  ;
: </h1>  ( -- )  s" h1" html} \n  ;
: <h2>  ( -- )  echo_cr s" h2" {html  ;
: </h2>  ( -- )  s" h2" html} \n  ;
: <h3>  ( -- )  echo_cr s" h3" {html  ;
: </h3>  ( -- )  s" h3" html} \n  ;
: <h4>  ( -- )  echo_cr s" h4" {html  ;
: </h4>  ( -- )  s" h4" html} \n  ;
: <h5>  ( -- )  echo_cr s" h5" {html  ;
: </h5>  ( -- )  s" h5" html} \n  ;
: <h6>  ( -- )  echo_cr s" h6" {html  ;
: </h6>  ( -- )  s" h6" html} \n  ;
: <head>  ( -- )  echo_cr s" head" {html  ;
: </head>  ( -- )  echo_cr s" head" html}  ;
: <header>  ( -- )  echo_cr s" header" {html  ;
: </header>  ( -- )  echo_cr s" header" html}  ;
: <hgroup>  ( -- )  echo_cr s" hgroup" {html  ;
: </hgroup>  ( -- )  echo_cr s" hgroup" html}  ;
: <hr/>  ( -- )  echo_cr s" hr" {html}  ;
: <html>  ( -- )  echo_cr s" html" {html  ;
: </html>  ( -- )  echo_cr s" html" html}  ;
: <i>  ( -- )  s" i" {html  ;
: </i>  ( -- )  s" i" html}  ;
: <iframe>  ( -- )  echo_cr s" iframe" {html  ;
: </iframe>  ( -- )  echo_cr s" iframe" html}  ;
: <img/>  ( -- )  s" img" {html}  ;
: <input>  ( -- )  echo_cr s" input" {html  ;
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
: <link/>  ( -- )  echo_cr s" link" {html}  ;
: <map>  ( -- )  echo_cr s" map" {html  ;
: </map>  ( -- )  echo_cr s" map" html}  ;
: <mark>  ( -- )  s" mark" {html  ;
: </mark>  ( -- )  s" mark" html}  ;
: <meta>  ( -- )  echo_cr s" meta" {html  ;
: </meta>  ( -- )  s" meta" html}  ;
: <meta/>  ( -- )  echo_cr s" meta" {html}  ;
: <nav>  ( -- )  echo_cr s" nav" {html  ;
: </nav>  ( -- )  echo_cr s" nav" html}  ;
: <noscript>  ( -- )  echo_cr s" noscript" {html  ;
: </noscript>  ( -- )  echo_cr s" noscript" html}  ;
: <object>  ( -- )  echo_cr s" object" {html  ;
: </object>  ( -- )  echo_cr s" object" html}  ;
: <ol>  ( -- )  echo_cr s" ol" {html  ;
: </ol>  ( -- )  echo_cr s" ol" html}  ;
: <p>  ( -- )  echo_cr s" p" {html  ;
: </p>  ( -- )  echo_cr s" p" html}  ;
: <param>  ( -- )  echo_cr s" param" {html  ;
: </param>  ( -- )  s" param" html}  ;
: <pre>  ( -- )  echo_cr s" pre" {html  ;
: </pre>  ( -- )  echo_cr s" pre" html}  ;
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
: <select>  ( -- )  echo_cr s" select" {html  ;  \ xxx latest tag copied from http://dev.w3.org/html5/markup/elements-by-function.html
: </select>  ( -- )  s" select" html}  ;
: <small>  ( -- )  s" small" {html  ;
: </small>  ( -- )  s" small" html}  ;
: <source>  ( -- )  echo_cr s" source" {html  ;
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
: <tbody>  ( -- )  echo_cr s" tbody" {html  ;
: </tbody>  ( -- )  echo_cr s" tbody" html}  ;
: <td>  ( -- )  echo_cr s" td" {html  header_cell? off  ;
: </td>  ( -- )  s" td" html}  ;
: <tfoot>  ( -- )  echo_cr s" tfoot" {html  ;
: </tfoot>  ( -- )  echo_cr s" tfoot" html}  ;
: <th>  ( -- )  echo_cr s" th" {html  header_cell? on  ;
: </th>  ( -- )  s" th" html}  ;
: <thead>  ( -- )  echo_cr s" thead" {html  ;
: </thead>  ( -- )  echo_cr s" thead" html}  ;
: <time>  ( -- )  s" time" {html  ;
: </time>  ( -- )  s" time" html}  ;
: <title>  ( -- )  echo_cr s" title" {html  ;
: </title>  ( -- )  s" title" html}  ;
: <tr>  ( -- )  echo_cr s" tr" {html  ;
: </tr>  ( -- )  s" tr" html} ;
: <track>  ( -- )  s" track" {html  ;
: </track>  ( -- )  s" track" html}  ;
: <u>  ( -- )  s" u" {html  ;
: </u>  ( -- )  s" u" html}  ;
: <ul>  ( -- )  echo_cr s" ul" {html  ;
: </ul>  ( -- )  echo_cr s" ul" html}  ;
: <var>  ( -- )  s" var" {html  ;
: </var>  ( -- )  s" var" html}  ;
: <video>  ( -- )  echo_cr s" video" {html  ;
: </video>  ( -- )  echo_cr s" video" html}  ;
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
: [<abbr>]  ( -- )  postpone <abbr>  ;  immediate
: [</abbr>]  ( -- )  postpone </abbr>  ;  immediate
: [<blockquote>]  ( -- )  postpone <blockquote>  ;  immediate
: [</blockquote>]  ( -- )  postpone </blockquote>  ;  immediate
: [<br/>]  ( -- )  postpone <br/>  ;  immediate
: [<caption>]  ( -- )  postpone <caption>  ;  immediate
: [<code>]  ( -- )  postpone <code>  ;  immediate
: [</code>]  ( -- )  postpone </code>  ;  immediate
: [<dd>]  ( -- )  postpone <dd>  ;  immediate
: [</dd>]  ( -- )  postpone </dd>  ;  immediate
: [<div>]  ( -- )  postpone <div>  ;  immediate
: [</div>]  ( -- )  postpone </div>  ;  immediate
: [<dl>]  ( -- )  postpone <dl>  ;  immediate
: [</dl>]  ( -- )  postpone </dl>  ;  immediate
: [<dt>]  ( -- )  postpone <dt>  ;  immediate
: [</dt>]  ( -- )  postpone </dt>  ;  immediate
: [<h1>]  ( -- )  postpone <h1>  ;  immediate
: [</h1>]  ( -- )  postpone </h1>  ;  immediate
: [<h2>]  ( -- )  postpone <h2>  ;  immediate
: [</h2>]  ( -- )  postpone </h2>  ;  immediate
: [<h3>]  ( -- )  postpone <h3>  ;  immediate
: [</h3>]  ( -- )  postpone </h3>  ;  immediate
: [<h4>]  ( -- )  postpone <h4>  ;  immediate
: [</h4>]  ( -- )  postpone </h4>  ;  immediate
: [<h5>]  ( -- )  postpone <h5>  ;  immediate
: [</h5>]  ( -- )  postpone </h5>  ;  immediate
: [<h6>]  ( -- )  postpone <h6>  ;  immediate
: [</h6>]  ( -- )  postpone </h6>  ;  immediate
: [<hr/>]  ( -- )  postpone <hr/>  ;  immediate
: [<img>]  ( -- )  postpone <img>  ;  immediate
: [<li>]  ( -- )  postpone <li>  ;  immediate
: [</li>]  ( -- )  postpone </li>  ;  immediate
: [<link/>]  ( -- )  postpone <link/>  ;  immediate
: [<ol>]  ( -- )  postpone <ol>  ;  immediate
: [</ol>]  ( -- )  postpone </ol>  ;  immediate
: [<p>]  ( -- )  postpone <p>  ;  immediate
: [</p>]  ( -- )  postpone </p>  ;  immediate
: [<pre>]  ( -- )  postpone <pre>  ;  immediate
: [</pre>]  ( -- )  postpone </pre>  ;  immediate
: [<q>]  ( -- )  postpone <q>  ;  immediate
: [</q>]  ( -- )  postpone </q>  ;  immediate
: [<span>]  ( -- )  postpone <span>  ;  immediate
: [</span>]  ( -- )  postpone </span>  ;  immediate
: [<strong>]  ( -- )  postpone <strong>  ;  immediate
: [</strong>]  ( -- )  postpone </strong>  ;  immediate
: [<table>]  ( -- )  postpone <table>  ;  immediate
: [</td>]  ( -- )  postpone </td>  ;  immediate
: [</th>]  ( -- )  postpone </th>  ;  immediate
: [<title>]  ( -- )  postpone <title>  ;  immediate
: [</title>]  ( -- )  postpone </title>  ;  immediate
: [<tr>]  ( -- )  postpone <tr>  ;  immediate
: [</tr>]  ( -- )  postpone </tr>  ;  immediate
: [<ul>]  ( -- )  postpone <ul>  ;  immediate
: [</ul>]  ( -- )  postpone </ul>  ;  immediate
markup<order

\ **************************************************************
\ Tag shortcuts used by the wiki markup and some addons

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

.( fendo.markup.html.tags.fs compiled) cr
