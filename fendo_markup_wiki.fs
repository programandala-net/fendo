.( fendo_markup_wiki.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-01.

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
\ 2013-06-10: Change: the new '[markup<order]' substitutes '[previous]'.
\ 2013-06-18: New: some combined punctuation, e.g. "),".
\ 2013-06-28: Change: Forth code wiki markups can be nested.
\ 2013-06-28: New: Markups for comments, "{*" and "*}";
\   can not be nested.
\ 2013-06-29: New: First changes to fix and improve the source
\   code markups (the source region needs a special parsing).

\ **************************************************************
\ Todo

\ 2013-06-04: Creole {{{...}}} markup.
\ 2013-06-04: Nested lists.
\ 2013-06-04: Also '*' and '#' for lists?
\ 2013-06-04: Flag the first markup of the current line, in
\ order to use '--' both forth nested lists and delete, or '**'
\ for list and for bold.
\ 2013-06-05: Comments. '{*...*}'?
\ 2013-06-19: Compare Creole's markups with txt2tags' markups.

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
        execute_markup? on  preserve_eol? off  \ xxx tmp
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
  [markup>order] <pre> <code> [markup<order]
  ;
: </code></pre>  ( -- )
  [markup>order] </code> </pre> [markup<order]
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
  [markup<order]
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
  [markup>order] <tr> [markup<order]  1 #rows +!  #cells off 
  ;
: >tr<  ( -- )
  \ New row in the current table; close the previous row if needed.
  #rows @ if  [markup>order] </tr> [markup<order]  then  (>tr<)
  ;
: close_pending_cell  ( -- )
  \ Close a pending table cell.
  header_cell? @ 
  if    [markup>order] </th> [markup<order]
  else  [markup>order] </td> [markup<order]
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
  \ The parsed cell markup ("|" or "|=") is the first markup parsed
  \ on the current line or there's an opened table?
  \ This check lets those signs to be used as content in other contexts.
  \ ff = is it an actual cell?
  table_started? @  #parsed @ 0=  or
  ;
: (|)  ( xt -- )
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)
  table_started? @ 0=
  if  [markup>order] <table> [markup<order]  then  ((|))
  ;
: <table><caption>  ( -- )
  [markup>order] <table> <caption> [markup<order]
  ;

\ **************************************************************
\ Tool words for merged Forth code

: (forth_code_end?)  ( ca len -- ff )
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed 
\  ." «" 2dup type ." » "  \ xxx debug check
\  2dup type space \ xxx debug check
  2dup s" <:" str= abs forth_code_depth +!
       s" :>" str= dup forth_code_depth +! 
  forth_code_depth @
\  dup ." {" . ." }" \ xxx debug check
  0= and
\  dup  if ." END!" then  \ xxx debug check
\  key drop  \ xxx debug check
  ;
: forth_code_end?  ( ca1 len1 ca2 len2 -- ca1' len1' ff )
  \ Add a new name to the parsed merged Forth code
  \ and check if it's the end of the Forth code.
  \ ca1 len1 = code already parsed 
  \ ca1' len1' = code already parsed, with ca2 len2 added
  \ ca2 len2 = latest name parsed 
  \ ff = is ca2 len2 the right markup for the end of the code?
  2dup (forth_code_end?) dup >r
  0= and  \ empty the name if it's the end of the code
  s+ s"  " s+  r>
  ;
: forth_code  ( "forthcode :>" -- ca len )
  \ Get the content of a merged Forth code. 
  \ Parse the input stream until a valid ":>" markup is found.
  \ ca len = Forth code
  s" "
  begin   parse-name dup
    if    forth_code_end?
    else  2drop  s\" \n" s+ refill 0=
    then
  until
\  cr ." <<<" 2dup type ." >>>" cr key drop  \ xxx debug check
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
\ Tool words for code markup

: ?_echo  ( ca len -- )  \ xxx todo move
  if  _echo  else  2drop  then
  ;
: (##)  ( "source code ##" -- )
  \ Parse an inline source code region.
  \ xxx fixme preserve spaces; translate < and &
  begin   parse-name dup 
    if    2dup s" ##" str= dup >r 0= ?_echo r>
    else  2drop refill 0= dup abort" Missing closing '##'"
    then
  until
  ;
: (###)  ( "source code ###" -- )
  \ Parse a block source code region.
  \ xxx fixme preserve spaces; translate < and &
  begin   parse-name dup 
    if    2dup s" ###" str= dup >r 0= ?_echo r>
    else  2drop echo_cr refill 0= dup abort" Missing closing '###'"
    then
  until
  ;

\ **************************************************************
\ Tool words for images and links

\ xxx todo unfinished

variable #|  \ counter of "|" separators
variable rendering_image?   \ flag, currently rendering an image?
variable rendering_link?   \ flag, currently rendering an image?

: image|1  ( -- )
  \ First separator in image.
  [char] | parse alt=!
  ;
: image|2  ( -- )
  \ Second separator in image.
  [char] | parse raw=!
  ;
create 'image|  ' image|1 , ' image|2 ,  \ xt table
: image|  ( compile-time: a -- ) ( run-time: -- ) 
  \ Manage an image separator.
  \ Execute the word corresponding to the count of separators.
  \ ." image|"  \ xxx debug check
  #| @ cells 'image| + perform  1 #| +!
  ;

: link|1  ( -- )
  \ First separator in link.
  ;
: link|2  ( -- )
  \ Second separator in link.
  ;
create 'link|  ' link|1 , ' link|2 ,  \ xt table
: link|  ( compile-time: a -- ) ( run-time: -- ) 
  \ Manage a link separator.
  \ Execute the word corresponding to the count of separators.
  #| @ cells 'link| + perform  1 #| +!
  ;

\ **************************************************************
\ Actual markup

\ The Fendo markup was inspired by Creole (http://wikicreole.org),
\ text2tags (http://text2tags.org) and others.

only forth markup>order definitions fendo>order 

\ Comments

: {*  ( "text*}" -- )
  \ Start a comment.
  \ Parse the input stream until a "*}" markup is found.
  begin   parse-name dup
    if    s" *}" str=
    else  2drop  refill 0=
    then
  until
  ;
: *}  ( -- )
  abort" '*}' without '{*'"
  ;

\ Merged Forth code

: <:  ( "forthcode :>" -- )
  \ Start and interpret a Forth block.
  1 forth_code_depth +!
  only fendo>order markup>order forth>order 
  forth_code evaluate
  ;  immediate
: :>  ( -- )
  \ Finish a Forth block.
  forth_code_depth @
\  dup 
  0= abort" ':>' without '<:'"
\  1 = if
\    only markup>order
\    separate? off
\  then
  -1 forth_code_depth +!
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

: ##  ( -- )
  \ Open and close an inline <code> region.
  <code> (##) </code> 
  ;
: ###  ( -- )
  \ Open and close a block <code> region.
  <pre><code> (###) </code></pre>
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

\ Markup for different uses

: |  ( -- )
  \ Markup used as separator in tables, images and links.
  \ ." | rendered"  \ xxx debug check
  rendering_link? @ if  link| exit  then
  rendering_image? @ if  image| exit  then
  actual_cell? if  ['] <td> (|) exit  then
  content
  ;

\ Tables

: |=  ( -- )
  \ Mark a table header cell.
  actual_cell? if  ['] <th> (|)  else  s" |=" content  then
  ;
: =|=  ( -- )
  \ Open or close a table caption; must be the first markup of a table.
  #rows @ abort" The '=|=' markup must be the first one in a table"
  ['] <table><caption> ['] </caption> opened_[=|=]? markups
  ;

\ Images

: {{  ( -- )
  rendering_image? on  #| off
  parse-word src=!
  ;
: }}  ( -- )
  <img>  rendering_image? off
  ;

\ Links

: [[  ( -- )
  rendering_link? on  #| off
  s\" <img src=\""
  ;
: ]]  ( ca len -- )
  <a> echo </a>  rendering_link? off
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
punctuation: )
punctuation: ).
punctuation: );
punctuation: ):
punctuation: ),
punctuation: "
punctuation: ":
punctuation: ",
punctuation: ";
punctuation: ".
punctuation: '
punctuation: »
punctuation: ]
punctuation: }

only forth fendo>order definitions

.( fendo_markup_wiki.fs compiled) cr
