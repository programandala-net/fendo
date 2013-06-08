.( fendo_markup_wiki.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

\ This file defines the wiki markup. 

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

\ 2013-05-18 Start. First HTML tags.
\ 2013-06-01 Paragraphs, lists, headings, delete.
\ 2013-06-02 New: also 'previous_space?'.
\   New: Counters for both types of elements (markups and
\   printable words); required in order to separate words.
\ 2013-06-04 New: punctuation words, HTML entity words. More
\   markups.
\ 2013-06-05 Change: '|' renamed to '_'; '|' will be needed for the
\   table markup.
\ 2013-06-05 New: Finished the code for entities; the common code for
\   entities and punctuation has been factored.
\ 2013-06-06 Change: HTML entities moved to <fendo_markup_html.fs>.
\ 2013-06-06 New: First version of table markup, based on Creole
\   and text2tags: data cells and header cells. Also caption.
\ 2013-06-06 New: several new markups.
\ 2013-06-06 Change: renamed from "fendo_markup.fs" to
\   "fendo_markup_wiki.fs"; it is included from the new file <fendo_markup.fs>.
\ 2013-06-06: New: Words for merging Forth code in the pages: '<:' and ':>'.

\ **************************************************************
\ Todo

\ 2013-06-04: Creole {{{...}}} markup.
\ 2013-06-04: Nested lists.
\ 2013-06-04: Also '*' for lists?
\ 2013-06-04: Flag the first markup of the current line, in
\ order to use '--' both forth nested lists and delete, or '**'
\ for list and for bold.
\ 2013-06-05: Comments. '{*...*}'?
\ 2013-06-05: divide files: fendo_markup.fs includes
\ fendo_markup_html.fs and fendo_markup_own.fs ?

\ **************************************************************
\ Generic tool words for markup and parsing

\ Counters
\ xxx used only by the parser; but will be required here too
variable #markups     \ consecutive markups parsed
variable #nonmarkups  \ consecutive nonmarkups parsed
variable #parsed      \ items already parsed in the current line (before the current item)

: exhausted?  ( -- ff )
  \ Is the current source line exhausted?
  [false] [if]
    \ First version, doesn't work when there are trailing spaces
    \ at the end of the line.
    >in @ source nip =
  [else]
    \ Second version, works fine when there are trailing spaces
    \ at the end of the line:
    save-input  parse-name nip 0= >r  restore-input throw  r>
  [then]
  ;

defer content  ( ca len -- )
  \ Manage a string of content: print it and update the counters.
  \ Defined in <fendo_parser.fs>.

defer close_pending  ( -- )
  \ Close the pending markups.
  \ Defined in <fendo_parser.fs>.

: markups  ( xt1 xt2 a -- )
  \ Open or close a HTML tag.
  \ This code is based on FML, a Forth-ish Markup Language for RetroWiki.
  \ xt1 = execution token of the opening HTML tag
  \ xt2 = execution token of the closing HTML tag
  \ a = markup flag variable: is the markup already open?
  dup >r @
  if    nip false
  else  drop true
  then  r> !  execute
  ;

variable #heading       \ level of the opened heading  \ xxx not used yet
variable opened_["""]?  \ is there an open block quote?
variable opened_[""]?   \ is there an open inline quote?
variable opened_[###]?  \ is there an open block code?
variable opened_[##]?   \ is there an open inline code?
variable opened_[**]?   \ is there an open '**'?
variable opened_[,,]?   \ is there an open ',,'?
variable opened_[--]?   \ is there an open '--'?
variable opened_[//]?   \ is there an open '//'?
variable opened_[=]?    \ is there an open heading?
variable opened_[=|=]?  \ is there an open table caption?
variable opened_[^^]?   \ is there an open '^^'?
variable opened_[_]?    \ is there an open '_'?
variable opened_[__]?   \ is there an open '__'?

: <pre><code>  ( -- )
  [markup>order] <pre> <code> [previous]
  ;
: </code></pre>  ( -- )
  [markup>order] </code> </pre> [previous]
  ;

\ **************************************************************
\ Tool words for lists

variable bullet_list_items    \ counter 
variable numbered_list_items  \ counter 

: ((-))  ( a -- )
  \ List element.
  \ a = counter variable
  [markup>order]
  dup @ if  </li>  then  <li>  1 swap +!  separate? off
  [previous]
  ;
: (-)  ( -- )
  \ Bullet list item.
  bullet_list_items ((-))
  ;
: (+)  ( -- )
  \ Numbered list item.
  numbered_list_items ((-))
  ;

\ **************************************************************
\ Tool words for tables

variable #rows   \ counter for the current table
variable #cells  \ counter for the current table

: (>tr<)  ( -- )
  \ New row in the current table.
  [markup>order] <tr> [previous]  1 #rows +!  #cells off 
  ;
: >tr<  ( -- )
  \ New row in the current table; close the previous row if needed.
  #rows @ if  [markup>order] </tr> [previous]  then  (>tr<)
  ;
: close_pending_cell  ( -- )
  \ Close a pending table cell.
  header_cell? @ 
  if    [markup>order] </th> [previous]
  else  [markup>order] </td> [previous]
  then
  ;
: ((|))  ( xt -- )
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)
  #cells @ if  close_pending_cell else  >tr<  then
  exhausted?
  if    drop #cells off
  else  execute  1 #cells +!
  then
  ;
: actual_cell?  ( -- ff )
  \ The parsed cell markup is the first one of a table
  \ but it's not at the start of the line?
  \ ff = not a real cell?
  table_started? @  #parsed @ 0=  or
  ;
: (|)  ( xt -- )
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)
  table_started? @ 0=
  if  [markup>order] <table> [previous]  then  ((|))
  ;
: <table><caption>  ( -- )
  [markup>order] <table> <caption> [previous]
  ;

\ **************************************************************
\ Tool words for merged Forth code

: :>?  ( ca1 len1 ca2 len2 -- ca1 len1 ff )
  \ Add a new name to the parsed merged Forth code.
  \ ca1 len1 = code already parsed in the code
  \ ca2 len2 = new name parsed in the code
  \ ff = is the name the end of the code?
  2dup s" :>" str= >r  s+ s"  " s+  r>
  ;
: slurp<:  ( "forthcode :>" -- ca len )
  \ Get the content of an merged Forth code. 
  \ ca len = Forth code
  s" "
  begin
    parse-name dup
    if    :>?               \ end of the code?
    else  2drop refill 0=   \ end of the parsing area?
    then
  until
  ;

\ **************************************************************
\ Tool words for punctuation

\
\ Punctuation markup is needed in order to print it properly
\ after another markup:
\
\   This // emphasis // does the right spacing.  But this //
\   emphasis // , well, needs to be followed by a markup comma.
\
\ The ',' markup prints the comma without a leading space.  If
\ ',' were not a markup but an ordinary printable content, a
\ leading space would be printed. 

: :punctuation   ( ca len -- )
  \ Create a punctuation word.
  \ ca len = punctuation --and name of its punctuation word
  :echo_name  separate? on
  ;
: punctuation:   ( "name" -- )
  \ Create a punctuation word.
  \ "name" = punctuation --and name of its punctuation word
  parse-name? abort" Parseable name expected in 'punctuation:'"
  :punctuation
  ;

\ **************************************************************
\ Actual markup

\ The Fendo markup was inspired by Creole (http://wikicreole.org),
\ text2tags (http://text2tags.org) and others.

only forth markup>order definitions also forth fendo>order 

\ Merged Forth code

: <:  ( "forthcode :>" -- )
  \ Start and interpret a Forth block.
  only fendo>order markup>order forth>order  forth_block? on
  slurp<: evaluate
  ;  immediate
: :>  ( -- )
  \ Finish a Forth block.
  forth_block? @ 0= abort" ':>' without '<:'"
  only markup>order  forth_block? off  separate? off
  ; immediate

\ Grouping

: _  ( -- )
  \ Open or close a <p> region.
  ['] <p> ['] </p> opened_[_]? markups  separate? off
  ;
: ----  ( -- )
  \ Create a horizontal line.
  <hr/> separate? off
  ;
: \\  ( -- )
  \ Line break.
  <br/> separate? off
  ;

\ Text

: //  ( -- )
  \ Open or close a <em> region.
  ['] <em> ['] </em> opened_[//]? markups 
  ;
: **  ( -- )
  \ Open or close a <strong> region.
  ['] <strong> ['] </strong> opened_[**]? markups
  ;
: --  ( -- )
  \ Open or close a <s> region.
  ['] <s> ['] </s> opened_[--]? markups
  ;
: __  ( -- )
  \ Start of finish a <u> region.
  ['] <u> ['] </u> opened_[__]? markups
  ;
: ^^  ( -- )
  \ Start of finish a <sup> region.
  ['] <sup> ['] </sup> opened_[^^]? markups
  ;
: ,,  ( -- )
  \ Start of finish a <sub> region.
  ['] <sub> ['] </sub> opened_[,,]? markups
  ;

\ Quotes

: ""  ( -- )
  \ Open or close a <q> region.
  ['] <q> ['] </q> opened_[""]? markups
  ;
: """  ( -- )
  \ Open or close a <blockquote> region.
  ['] <blockquote> ['] </blockquote> opened_["""]? markups
  ;

\ Code

: ##  ( )
  \ Open or close an inline <code> region.
  \ xxx todo activate a special parsing in the region.
  ['] <code> ['] </code> opened_[##]? markups
  ;
: ###  ( )
  \ Open or close an block <code> region.
  \ xxx todo activate a special parsing in the region.
  ['] <pre><code> ['] </code></pre> opened_[###]? markups
  ;

\ Headings

: =  ( -- )
  \ Open or close a <h1> region.
  ['] <h1> ['] </h1> opened_[=]? markups
  ;
: ==  ( -- )
  \ Open or close a <h2> region.
  ['] <h2> ['] </h2> opened_[=]? markups
  ;
: ===  ( -- )
  \ Open or close a <h3> region.
  ['] <h3> ['] </h3> opened_[=]? markups
  ;
: ====  ( -- )
  \ Open or close a <h4> region.
  ['] <h4> ['] </h4> opened_[=]? markups
  ;
: =====  ( -- )
  \ Open or close a <h5> region.
  ['] <h5> ['] </h5> opened_[=]? markups
  ;
: ======  ( -- )
  \ Open or close a <h6> region.
  ['] <h6> ['] </h6> opened_[=]? markups
  ;

\ Lists

: -  ( -- )
  \ Bullet list item.
  bullet_list_items @ 0= if  <ul>  then  (-)
  ;
: +  ( -- )
  \ Numbered list item.
  numbered_list_items @ 0= if  <ol>  then  (+)
  ;

\ Tables

: |  ( -- )
  \ Mark a table data cell.
  actual_cell? if  ['] <td> (|)  else  s" |" content  then
  ;
: |=  ( -- )
  \ Mark a table header cell.
  actual_cell? if  ['] <th> (|)  else  s" |=" content  then
  ;
: =|=  ( -- )
  \ Open or close a table caption; must be the first markup of a table.
  #rows @ abort" The '=|=' markup must be the first one in a table"
  ['] <table><caption> ['] </caption> opened_[=|=]? markups
  ;

\ Escape

: ~  ( "name" -- )
  \ Escape a name: Parse and echo it, even if it's a markup.
  parse-name? abort" Parseable name expected in '~'"  content
  ; 

\ Punctuation

punctuation: .
punctuation: ,
punctuation: ;
punctuation: :
punctuation: ...
punctuation: !
punctuation: ?
\ xxx needed?:
punctuation: )
punctuation: "
punctuation: '
punctuation: »
punctuation: ]
punctuation: }

only forth fendo>order definitions

.( fendo_markup_wiki.fs compiled) cr
