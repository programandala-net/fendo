.( fendo_markup_wiki.fs ) 

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

\ See at the end of the file.

\ **************************************************************
\ Todo

\ 2013-06-04: Creole {{{...}}} markup.
\ 2013-06-04: Nested lists.
\ 2013-06-04: Also '*' and '#' for lists?
\ 2013-06-04: Flag the first markup of the current line, in
\ order to use '--' both forth nested lists and delete, or '**'
\ for list and for bold.
\ 2013-06-19: Compare Creole's markups with txt2tags' markups.
\ 2013-06-29: xxx fixme rendering the whole site leavs >900
\ numbers on the stack, strings. it seems caused by links or
\ images.

\ **************************************************************
\ Debug tools

: xxxtype  ( ca len -- ca len )
  2dup ." «" type ." »"
  ;
: xxx.  ( x -- x )
  dup ." «" . ." »"
  ;

\ **************************************************************
\ Generic tool words for strings

\ xxx todo move?

: concatenate  ( ca1 len1 ca2 len2 -- ca1' len1' )
  2swap dup if  s"  " s+   then  2swap s+
  \ xxx faster alternative? benckmark
  \ 2 pick if  2swap s"  " s+ 2swap  then  s+
  ;
: ?concatenate  ( ca1 len1 ca2 len2 f -- ca1' len1' )
  if  concatenate  else  2drop  then
  ;
: otherwise_concatenate  ( ca1 len1 ca2 len2 f -- ca1' len1' f )
  \ Wrapper for '?concatenate'.
  \ If f is false, concatenate ca1 len1 and ca2 len2;
  \ if f is true, drop ca2 len2. 
  dup >r 0= ?concatenate r>
  ;

\ **************************************************************
\ Generic tool words for markup and parsing

\ Counters
\ xxx used only by the parser; but will be required here too
variable #markups     \ consecutive markups parsed
variable #nonmarkups  \ consecutive nonmarkups parsed
variable #parsed      \ items already parsed in the current line (before the current item)

: first_on_the_line?  ( -- ff )
  \ Is the last parsed name the first one on the current line?
  #parsed @ 0=
  ;
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
        \ execute_markup? on  preserve_eol? off  \ xxx tmp
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
\ Tools for lists

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
  bullet_list_items dup @ 0=
  if  [markup>order] <ul> [markup<order]  then  ((-))
  ;
: (+)  ( -- )
  \ Numbered list item.
  numbered_list_items dup @ 0=
  if  [markup>order] <ol> [markup<order]  then  ((-))
  ;

\ **************************************************************
\ Tools for tables

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
  table_started? @  first_on_the_line?  or
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
\ Tools for merged Forth code

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
\ Tools for punctuation

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
  :echo_name_
  ;
: punctuation:   ( "name" -- )
  \ Create a punctuation word.
  \ "name" = punctuation --and name of its punctuation word
  parse-name? abort" Missing name in 'punctuation:'"
  :punctuation
  ;

\ **************************************************************
\ Tools for code markup

: (##)  ( "source code ##" -- )
  \ Parse an inline source code region.
  \ xxx fixme preserve spaces; translate < and &
  begin   parse-name dup 
    if    2dup s" ##" str= dup >r 0= ?_echo r>
    else  2drop refill 0= dup abort" Missing closing '##'"
    then
  until
  ;
0 [if]  \ xxx old
: (###)  ( "source code ###" -- )
  \ Parse a block source code region.
  \ xxx todo preserve spaces (reading complete lines)
  \ xxx todo translate < and &
  begin   parse-name dup 
    if    2dup s" ###" str= dup >r 0= ?_echo r>
    else  2drop echo_cr refill 0= dup abort" Missing closing '###'"
    then
  until
  ;
[then]
s" /counted-string" environment? 0=
[if]  255  [then]  dup constant /###-line
2 chars + buffer: ###-line
: (###)  ( "source code ###" -- )
  \ Parse a block source code region.
  \ xxx todo preserve spaces (reading complete lines)
  \ xxx todo translate < and &
  \ cr ." (###) code = "  \ xxx debug check
  begin   
    ###-line dup /###-line source-id read-line throw 
    0= abort" Missing closing '###'"
    \ 2dup cr type  \ xxx debug check
    2dup s" ###" str= dup >r 0= ?echo_line r>
  until
  ." ### end!"  \ xxx debug check
  ;

\ **************************************************************
\ Tools for images and links

: or_end_of_section?  ( ca len ff1 -- ff2 )
  \ ca len = latest name parsed in the alt attribute section
  >r  s" |" str=  r> or 
  ;

\ **************************************************************
\ Tools for images 

: get_image_src_attribute  ( "filename<spc>" -- )
  \ Parse and store the image src attribute.
  files_subdir $@ parse-word s+ src=!
  ;
variable image_finished?  \ flag, no more image markup to parse?
: end_of_image?  ( ca len -- ff )
  \ ca len = latest name parsed 
  s" }}" str=  dup image_finished? !
  ;
: end_of_image_section?  ( ca len -- ff )
  \ ca len = latest name parsed 
  2dup end_of_image? or_end_of_section?
  ;
: more_image?  ( -- ff )
  \ Fill the input buffer or abort.
  refill 0= dup abort" Missing '}}'"
  ;
: get_image_alt_attribute  ( "...<space>|<space>" | "...<space>}}<space>"  -- )
  \ Parse and store the image alt attribute.
  s" "
  begin   parse-name dup
    if    2dup end_of_image_section?  otherwise_concatenate
    else  2drop  more_image?
    then  
  until   alt=!
  ;
: get_image_raw_attributes  ( "...<space>}}<space>"  -- )
  \ Parse and store the image raw attributes.
  s" "
  begin   parse-name dup 
    if    2dup end_of_image?  otherwise_concatenate
    else  2drop  more_image?
    then  
  until   raw=!
  ;
: parse_image  ( "imagemarkup}}" -- )
  \ Parse and store the image attributes.
  get_image_src_attribute
  [ false ] [if]  \ simple version
    parse-name end_of_image_section? 0=
      abort" Space not allowed in image filename"
  [else]  \ no abort
    begin  parse-name end_of_image_section? 0=
    while  s" <!-- xxx fixme space in image filename -->" echo
    repeat
  [then]
  image_finished? @ 0= if
    get_image_alt_attribute
    image_finished? @ 0= if  get_image_raw_attributes  then
  then
  ;
: ({{)  ( "imagemarkup}}" -- )
  parse_image [markup>order] <img> [markup<order]
  ;

\ **************************************************************
\ Tools for links

variable link_text  \ used as a dynamic string
: get_link_href_attribute  ( "filename<spc>" -- )
  \ Parse and store the link href attribute.
  parse-word href=!
  ;
variable link_finished?  \ flag, no more link markup to parse?
: end_of_link?  ( ca len -- ff )
  \ ca len = latest name parsed 
  s" ]]" str=  dup link_finished? !
  ;
: end_of_link_section?  ( ca len -- ff )
  \ ca len = latest name parsed 
  2dup end_of_link? or_end_of_section?
  ;
: more_link?  ( -- ff )
  \ Fill the input buffer or abort.
  refill 0= dup abort" Missing ']]'"
  ;
: parse_nested_image  ( -- ca len )
  \ Parse and render an image markup nested in other markup
  \ (currently only used in link texts).
  \ ca len = HTML of the image markup
  >attributes<
  echo> @ echo>string  ({{)  echo> !
  >attributes<  echoed $@ 
  ;
: parse_link_text  ( "...<space>|<space>" | "...<space>]]<space>"  -- )
  \ Parse and store the link text. 
  s" "
  begin
    parse-name dup
    if    2dup s" {{" str=
          if    2drop  parse_nested_image s+  false
          else  2dup end_of_link_section?
                otherwise_concatenate
          then
    else  2drop  more_link?
    then  
  until   link_text $!
  ;
: get_link_raw_attributes  ( "...<space>}}<space>"  -- )
  \ Parse and store the link raw attributes.
  s" "
  begin   parse-name dup 
    if    2dup end_of_link?  otherwise_concatenate
    else  2drop  more_link?
    then  
  until   raw=!
  ;
: parse_link  ( "linkmarkup]]" -- )
  \ Parse and store the link attributes.
  get_link_href_attribute
  [ false ] [if]  \ simple version
    parse-name end_of_link_section? 0=
      abort" Space not allowed in link filename or URL"
  [else]  \ no abort
    begin  parse-name end_of_link_section? 0=
    while  s" <!-- xxx fixme space in link filename or URL -->" echo
    repeat
  [then]
  link_finished? @ 0= if
    parse_link_text 
    link_finished? @ 0= if  get_link_raw_attributes  then
  then
  ;
: http://?  ( ca1 len1 -- ff )
  \ Does a string starts with "http://"?
  s" http://" string-prefix?
  ;
wordlist constant links_wid  \ for bookmarked links
: link:?  ( ca1 len1 --  xt -1 | 0 )
  \ Is a string the name of a bookmarked link?
  links_wid search-wordlist
  ;
: missing_link:_link_text  ( -- )
  \ Set the proper link text of a link: link when missing.
  \ xxx todo
  ;
: missing_http://_link_text  ( -- )
  \ Set the proper link text of a http:// link when missing.
  \ xxx todo
  ;
: missing_link_text  ( -- )
  \ Set the proper link text when missing.
  \ xxx todo
  href=@ link:?  if  missing_link:_link_text exit  then
  href=@ http://?  if  missing_http://_link_text exit  then
  href=@ link_text $!
  ;
: ([[)  ( "linkmarkup]]" -- )
  \ xxx todo
  \ Parse the link attributes and prepare them.
  s" " link_text $!  parse_link
  link_text $@len 0= if  missing_link_text  then
  \ xxx fixme use 'target_extension' instead, to get the
  \ extension of the destination file! there are links to Atom
  \ files, with their own extensions.
  href= $@ current_target_extension s+ href=!
  ;

\ **************************************************************
\ Tools for bookmarked links

: link:  ( ca1 len1 ca2 len2 ca3 len3 "name" -- )
  \ Create a bookmarked link.
  \ ca1 len1 = raw attributes
  \ ca2 len2 = link text
  \ ca3 len4 = URL or local page or bookmarked link
  \ "name" = link name 
  parse-name? abort" Missing link: name"
  get-current >r  links_wid set-current
  :create  $!, $!, $!,
  r> set-current
  does>  ( -- ca1 len1 ca2 len2 ca3 len3 ) 
    ( dfa ) dup >r $@ r@ cell + $@ r> 2 cells + $@
  ;

\ **************************************************************
\ Tools for languages

\ xxx todo nested, with depth counter

: (xml:)lang=  ( -- a )
  \ Return the proper language attribute.
  xhtml? @ if  xml:lang=  else  lang=  then
  ;
: ((:  ( "name" -- )
  \ Create a language inline markup.
  \ name = ISO code of a language
  parse-name? abort" Missing language code"
  2dup s" ((" s+ :create s,
  does>  ( -- ) ( dfa )
    count (xml:)lang= $! [markup>order] <span> [markup<order]
  ;
: (((:  ( "name" -- )
  \ Create a language block markup.
  \ name = ISO code of a language
  parse-name? abort" Missing language code"
  2dup s" (((" s+ :create s,
  does>  ( -- ) ( dfa )
    count (xml:)lang= $! [markup>order] <div> [markup<order]
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
  \ Start, parse and interpret a Forth block.
  1 forth_code_depth +!
  only fendo>order markup>order forth>order 
  forth_code evaluate
  ;  immediate
: :>  ( -- )
  \ Finish a Forth block.
  forth_code_depth @
\  dup   \ xxx
  0= abort" ':>' without '<:'"
\  1 = if  \ xxx
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
  \ Create a line break.
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
  \ Open or close a <h1> heading.
  ['] <h1> ['] </h1> opened_[=]? markups
  ;
: ==  ( -- )
  \ Open or close a <h2> heading.
\  cr ." opened_[=]? = " opened_[=]? ? key drop  \ xxx debug check
  ['] <h2> ['] </h2> opened_[=]? markups
  ;
: ===  ( -- )
  \ Open or close a <h3> heading.
  ['] <h3> ['] </h3> opened_[=]? markups
  ;
: ====  ( -- )
  \ Open or close a <h4> heading.
  ['] <h4> ['] </h4> opened_[=]? markups
  ;
: =====  ( -- )
  \ Open or close a <h5> heading.
  ['] <h5> ['] </h5> opened_[=]? markups
  ;
: ======  ( -- )
  \ Open or close a <h6> heading.
  ['] <h6> ['] </h6> opened_[=]? markups
  ;

\ Lists

: -  ( -- )
  \ Bullet list item.
  first_on_the_line? if  (-)  else  s" -" content  then
  ;
' - alias *
: +  ( -- )
  \ Numbered list item.
  first_on_the_line? if  (+)  else  s" +" content  then
  ;
' + alias #

\ Tables

: |  ( -- )
  \ Markup used as separator in tables, images and links.
  \ ." | rendered"  \ xxx debug check
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

\ Images

: {{  ( "imagemarkup}}" -- )
  ({{)
  ;
: }}  ( -- )
  true abort" '}}' without '{{'"
  ;

\ Links

: [[  ( "linkmarkup]]" -- )
  ([[) <a> link_text $@ echo </a> 
  ;
: ]]  ( -- )
  true abort" ']]' without '[['"
  ;

\ Language

((: en  (((: en  \ create 'en((' and 'en((('
((: eo  (((: eo  \ create 'eo((' and 'eo((('
((: es  (((: es  \ create 'es((' and 'es((('

: ))  ( -- )
  </span> separate? on
  ;
: )))  ( -- )
  </div> separate? on
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

.( fendo_markup_wiki.fs compiled ) cr

0 [if]

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
\ 2013-06-29: New: The image markups are rendered.
\ 2013-07-02: Change: Spaces found on filenames or URL don't abort
\   any more, but print a 'xxx fixme' warning in an HTML comment instead.
\   These undesired spaces are caused by a wrong rendering of "__" by
\   Simplil2Fendo, difficult to fix. Thus manual fix will be
\   required in the final .fs files.
\ 2013-07-04: New: language markup.
\ 2013-07-04: Fix: separation after punctuation markup.
\ 2013-07-04: Fix: now list markups work only at the start of
\   the line.
\ 2013-07-05: New: Creole's '*' and '#' are alias of '-' and
\   '+', for easier migration (so far converting the lists markups
\   in the original sources with Simplilo2Fendo seems difficult).
\ 2013-07-12: Finished 'link:'; changed 'link:?'.
\ 2013-07-12: Change: '?_echo' moved to <fendo_echo.fs>.
\ 2013-07-12: Change: '(###)' rewritten to parse whole lines.
\ 2013-07-14: New: '(###)' finished.

[then]
