.( fendo_markup_wiki.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

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

\ 2013-11-07: make closing heading optional.
\ 2013-10-30: Optional file size in file links.
\ 2013-07-20: Idea for nested lists: prefix words to increase
\ and decrease the depth: >> << .
\ 2013-06-04: Nested lists.
\ 2013-06-04: Flag the first markup of the current line, in
\ order to use '--' both forth nested lists and delete, or '**'
\ for list and for bold.
\ 2013-06-19: Compare Creole's markups with txt2tags' markups.

\ **************************************************************
\ Requirements

\ 'bs&' is provided by <galope/sb.fs>, included in <fendo.fs>.

require galope/dollar-variable.fs  \ '$variable'
require galope/trim.fs  \ 'trim'
require fendo/addons/source_code.fs  \ xxx needed by '###'

\ **************************************************************
\ Debug tools

: xxxtype  ( ca len -- ca len )
  2dup ." «" type ." »"
  ;
: xxx.  ( x -- x )
  dup ." «" . ." »"
  ;

\ **************************************************************

get-current  forth-wordlist set-current

require galope/n-to-r.fs  \ 'n>r'
require galope/n-r-from.fs  \ 'nr>'
require galope/minus-prefix.fs  \ '-prefix'

require ffl/str.fs  \ FFL's dynamic strings

set-current

\ **************************************************************
\ Generic tool words for strings

\ xxx todo move?

: concatenate  ( ca1 len1 ca2 len2 -- ca1' len1' )
  \ Concatenates two string with a joining space.
  2swap dup if  s"  " s+ 2swap s+ exit  then  2drop  
  ;
: ?concatenate  ( ca1 len1 ca2 len2 f -- ca1' len1' )
  \ Concatenates two string with a joining space.
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

: first_on_the_line?  ( -- wf )
  \ Is the last parsed name the first one on the current line?
  #parsed @ 0=
  ;
: exhausted?  ( -- wf )
  \ Is the current source line exhausted?
  [false] [if]
    \ First version, doesn't work when there are trailing spaces
    \ at the end of the line.
    >in @ source nip =
  [else]
    \ Second version, works fine when there are trailing spaces
    \ at the end of the line:
    save-input  parse-name empty? >r  restore-input throw  r>
  [then]
  ;

defer content  ( ca len -- )
  \ Manage a string of content: print it and update the counters.
  \ Defined in <fendo_parser.fs>.
defer evaluate_content  ( ca len -- )
  \ Evaluate a string as page content.
  \ Defined in <fendo_parser.fs>.

\ defer close_pending  ( -- ) \ xxx tmp
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

\ **************************************************************
\ Tools for lists

variable bullet_list_items    \ counter 
variable numbered_list_items  \ counter 

: ((-))  ( a -- )
  \ List element.
  \ a = counter variable
  dup @ if  [</li>]  then  [<li>]  1 swap +!  separate? off
  ;
: (-)  ( -- )
  \ Bullet list item.
  bullet_list_items dup @ 0=
  if  [<ul>]  then  ((-))
  ;
: (+)  ( -- )
  \ Numbered list item.
  numbered_list_items dup @ 0=
  if  [<ol>]  then  ((-))
  ;

\ **************************************************************
\ Tools for tables

variable #rows   \ counter for the current table
variable #cells  \ counter for the current table

: (>tr<)  ( -- )
  \ New row in the current table.
  [<tr>]  1 #rows +!  #cells off 
  ;
: >tr<  ( -- )
  \ New row in the current table; close the previous row if needed.
  #rows @ if  [</tr>]  then  (>tr<)
  ;
: close_pending_cell  ( -- )
  \ Close a pending table cell.
  header_cell? @ if  [</th>]  else  [</td>]  then
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
: actual_cell?  ( -- wf )
  \ The parsed cell markup ("|" or "|=") is the first markup parsed
  \ on the current line or there's an opened table?
  \ This check lets those signs to be used as content in other contexts.
  \ wf = is it an actual cell?
  table_started? @  first_on_the_line?  or
  ;
: (|)  ( xt -- )
  \ New data cell in the current table.
  \ xt = execution cell of the HTML tag (<td> or <th>)
  table_started? @ 0= if  [<table>]  then  ((|))
  ;
: <table><caption>  ( -- )
  [<table>] [<caption>]
  ;

\ **************************************************************
\ Tools for merged Forth code

0 [if]  \ xxx first version

: (forth_code_end?)  ( ca len -- wf )
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed 
\  ." «" 2dup type ." » "  \ xxx informer
\  2dup type space \ xxx informer
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +! 
  forth_code_depth @
\  dup ." {" . ." }" \ xxx informer
  0= and
\  dup  if ." END!" then  \ xxx informer
\  key drop  \ xxx informer
  ;
: forth_code_end?  ( ca1 len1 ca2 len2 -- ca1' len1' wf )
  \ Add a new name to the parsed merged Forth code
  \ and check if it's the end of the Forth code.
  \ ca1 len1 = code already parsed 
  \ ca1' len1' = code already parsed, with ca2 len2 added
  \ ca2 len2 = latest name parsed 
  \ wf = is ca2 len2 the right markup for the end of the code?
  2dup (forth_code_end?) dup >r
  0= and  \ empty the name if it's the end of the code
  s+ s"  " s+  r>
  ;
: parse_forth_code  ( "forthcode ]>" -- ca len )
  \ Get the content of a merged Forth code. 
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code
  s" "
  begin   parse-name dup
    if    
\          2dup ." { " type ." } "  \ xxx informer
          2dup forth_code_end?
          dup >r
          0= and  s+ s"  " s+ r>
    else  2drop  
          \ s\" \n" s+ 
          s"  " s+ 
          refill 0=
    then
  until
\  cr ." <[ " 2dup type ." ]>" cr  \ xxx informer
  ;

[then]

true [if]  \ xxx 2013-08-10 second version, more legible

: "<["=  ( -- wf )
  s" <[" str= 
  ;
: "]>"=  ( -- wf )
  s" ]>" str= 
  ;
: update_forth_code_depth  ( ca len -- )
  \ ca len = latest name parsed 
  2dup "<["= abs >r "]>"= r> + forth_code_depth +! 
  ;
: forth_code_end?  ( ca len -- wf )
  "]>"= forth_code_depth @ 0= and
  ;
: bl+  ( ca len -- ca' len' )
  s"  " s+
  ;
: remaining   ( -- )
\ xxx informer
  >in @ source 2 pick - -rot + swap
  64 min
  cr ." ***> " type ."  <***" cr
  ;
: parse_forth_code  ( "forthcode ]>" -- ca len )
  \ Get the content of a merged Forth code. 
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code
  s" "
  begin   parse-name dup
    if    
\           2dup ." { " type ." } "  \ xxx informer
\           ." { " input-lexeme 2@ type ." } "  \ xxx informer
\           remaining  key drop  \ xxx informer
          2dup update_forth_code_depth
          2dup forth_code_end?
          dup >r if  2drop  else  
\          2dup ." {{ " type ." }}"  \ xxx informer
          s+ bl+  then  r>
    else  2drop bl+  refill 0=
    then
  until
\  ." ]>" cr  \ xxx informer
\  cr ." <[ " 2dup type ." ]>" cr  \ xxx informer
  ;

[then]

0 [if]  \ experimental version with dynamic string, not finished

: forth_code_end?  ( ca len -- wf )
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed 
\  ." «" 2dup type ." » "  \ xxx informer
\  2dup type space \ xxx informer
  2dup s" <[" str= abs forth_code_depth +!
       s" ]>" str= dup forth_code_depth +! 
  forth_code_depth @
\  dup ." {" . ." }" \ xxx informer
  0= and
\  dup  if ." END!" then  \ xxx informer
\  key drop  \ xxx informer
  ;
$variable forth_code$
: forth_code$+  ( ca len -- )
  \ Append a string to the parsed Forth code.
  forth_code$ $@  s"  " s+ 2swap s+  forth_code$ $!
  ;
: parse_forth_code  ( "forthcode ]>" -- ca len )
  \ Get the content of a merged Forth code. 
  \ Parse the input stream until a valid "]>" markup is found.
  \ ca len = Forth code
  s" " forth_code$ $!
  begin   parse-name dup
    if    
\          2dup ." { " type ." }"  \ xxx informer
          2dup forth_code_end? dup >r if  2drop  else  forth_code$+  then r>
    else  2drop s"  " forth_code$+  refill 0=
    then
  until   forth_code$ $@
\  cr ." <[ " 2dup type ."  ]>" cr  \ xxx informer
  ;
[then]

\ **************************************************************
\ Tools for punctuation

\
\ Punctuation markup is needed in order to print it properly
\ after another markup. Example:
\
\   This // emphasis // does the right spacing.  But this //
\   emphasis // , well, needs to be followed by a markup comma.
\
\ The ',' markup must print a comma without a leading space.  If
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
: ###-line  ( -- ca len )
  \ Parse a new line from the current source code block.
  read_source_line 0= abort" Missing closing '###'"
  ;
: "###"?  ( ca len -- wf )
  \ Does the given string contains only "###"?
  trim s" ###" str=
  ;
: ###-line?  ( -- ca len true | false )
  \ Parse a new line from the current source code block.
  ###-line 2dup "###"? dup if  nip nip  then  0=
  ;
: plain_###-zone  ( "source code ###" -- )
  \ Parse and echo a source code zone "as is".
  \ xxx todo translate "<" and "&" ?
  begin  ###-line? dup >r ?echo_line r> 0=  until
  ;
: highlighted_###-zone  ( "source code ###" -- )
  \ Parse a source code zone, highlight and echo it.
  new_source_code 
  begin   
    ###-line? dup >r 
    if  append_source_code_line  then  r> 0=
  until  source_code@ highlighted echo
  ;
: (###)  ( "source code ###" -- )
  \ Parse and echo a source code zone.
  highlight? if  highlighted_###-zone  else  plain_###-zone  then
  ;

\ **************************************************************
\ Tools for images and links

: or_end_of_section?  ( ca len wf1 -- wf2 )
  \ ca len = latest name parsed in the alt attribute section
  >r  s" |" str=  r> or 
  ;
str-create tmp-str
: unraw_attributes  ( ca len -- )
  \ Extract and store the individual attributes from
  \ a string of raw verbatim attributes.
  tmp-str str-set
  s\" =\" " s\" =\"" tmp-str str-replace
  tmp-str str-get
  >sb  \ xxx tmp
  evaluate
  ;

\ **************************************************************
\ Tools for images 

: get_image_src_attribute  ( "filename<spc>" -- )
  \ Parse and store the image src attribute.
  files_subdir $@ parse-word s+ src=!
  ;
variable image_finished?  \ flag, no more image markup to parse?
: end_of_image?  ( ca len -- wf )
  \ ca len = latest name parsed 
  s" }}" str=  dup image_finished? !
  ;
: end_of_image_section?  ( ca len -- wf )
  \ ca len = latest name parsed 
  2dup end_of_image? or_end_of_section?
  ;
: more_image?  ( -- wf )
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
  until   ( ca len ) unraw_attributes
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
  parse_image [<img>]
  ;

\ **************************************************************
\ Tools for links

: file://?  ( ca len -- wf )
  \ Does a string starts with "file://"?
  s" file://" string-prefix?
  ;
' file://? alias file_href?
: http://?  ( ca len -- wf )
  \ Does a string starts with "http://"?
  s" http://" string-prefix?
  ;
: https://?  ( ca len -- wf )
  \ Does a string starts with "https://"?
  s" https://" string-prefix?
  ;
: ftp://?  ( ca len -- wf )
  \ Does a string starts with "ftp://"?
  s" ftp://" string-prefix?
  ;
: external_href?  ( ca len -- wf )
  \ Is a href attribute external?
  2dup http://? >r
  2dup https://? >r
  ftp://?  r> or r> or
  ;
0 [if]  \ xxx old
wordlist constant links_wid  \ for bookmarked links
: link:?  ( ca len --  xt -1 | 0 )
  \ Is a string the name of a bookmarked link?
  links_wid search-wordlist
  ;
[then]
link_text_as_attribute? 0= [if]  \ xxx tmp
$variable link_text
: link_text!  ( ca len -- )
  link_text $!
  ;
: link_text@  ( -- ca len )
  link_text $@
  ;
: link_text?!  ( ca len -- )
  \ If the the string variable 'link_text' is empty,
  \ store the given string into it.
  link_text@ empty? if  link_text!  else  2drop  then
  ;
[then]
: evaluate_link_text  ( -- )
  link_text@ evaluate_content
  ;
  
$variable link_anchor
: -anchor  ( ca len -- ca len | ca' len' )
  \ Extract the anchor from a href attribute and store it.
  \ ca len = href attribute
  \ ca' len' = href attribute without anchor
  s" #" sides/ drop link_anchor $!
  ;
: +anchor  { ca1 len1 ca2 len2 -- ca3 len3 }
  \ Add a link anchor to a href attribute.
  \ ca1 len1 = href attribute
  \ ca2 len2 = anchor, without "#"
  ca1 len1 len2 if  s" #" s+ ca2 len2 s+  then
  ;
variable link_type
1 enum local_link
  enum external_link
  enum file_link  drop
: >link_type_id  ( ca len -- n )
  \ Convert an href attribute to its type id.
  2dup external_href? if  2drop external_link exit  then
  file_href? if  file_link exit  then
  local_link 
  ;
\ xxx old  
\ : >link_type_id  ( ca len -- n | 0 )
\  \ Convert an href attribute to its type id, if not empty.
\  dup if  (>link_type_id)  else  nip  then
\  ;
: set_link_type  ( ca len -- )
  \ Get and store the type id of an href attribute.
  \ xxx todo no href means local, if there is/was an anchor label
  >link_type_id link_type !
  ;
: external_link?  ( -- wf )
  link_type @ external_link =
  ;
: local_link?  ( -- wf )
  link_type @ local_link =
  ;
: file_link?  ( -- wf )
  link_type @ file_link =
  ;
0 [if]  \ xxx old, 2013-10-22 moved to its own file <fendo_shortcuts.fs>
: unlink?  ( xt1 xt2 1|-1  |  xt1 0  --  xt2 xt2 true  |  0 )
  \ Execute xt2 if it's different from xt1.
  \ xt1 = old xt (former loop)
  \ xt2 = new xt
  if    2dup <> if  nip dup true  else  2drop false  then
  else  drop false
  then
  ;
: (unlinked?)  ( xt ca len -- wf )
  \ Is an href attribute a link different from xt?
  \ ca len = href attribute (not empty)
  fendo_links_wid search-wordlist unlink?
  ;
: unlinked?  ( xt ca len -- wf )
  \ Is an href attribute a link different from xt?
  \ ca len = href attribute (or an empty string)
  dup  if  (unlinked?)  else  nip nip  then
  ;
: unlink  ( ca len -- ca len | ca' len' )
  \ xxx choose better name?: unalias, unfake...
  \ Unlink an href attribute recursively.
  \ ca len = href attribute 
  \ ca' len' = actual href attribute
  2dup href=!
  0 rot rot  \ fake xt
\  2dup ." unlink " type  \ xxx informer
  begin   ( xt ca len ) unlinked?
  while   execute href=@
\  2dup ." --> " type  \ xxx informer
  repeat  href=@
\  cr  \ xxx informer
  ;
[then]
variable link_finished?  \ flag, no more link markup to parse?
: end_of_link?  ( ca len -- wf )
  \ ca len = latest name parsed 
  s" ]]" str=  dup link_finished? !
  ;
: end_of_link_section?  ( ca len -- wf )
  \ ca len = latest name parsed 
  2dup end_of_link? or_end_of_section?
  ;
: more_link?  ( -- wf )
  \ Fill the input buffer or abort.
  refill 0= dup abort" Missing ']]'"
  ;
defer parse_link_text  ( "...<spaces>|<spaces>" | "...<spaces>]]<spaces>"  -- )
  \ Parse the link text and store it into 'link_text'.
  \ Defined in <fendo_parser.fs>.
: get_link_raw_attributes  ( "...<space>]]<space>"  -- )
  \ Parse and store the link raw attributes.
  s" "
  begin   parse-name dup 
    if    2dup end_of_link?  otherwise_concatenate
    else  2drop  more_link?
    then  
  until   ( ca len ) unraw_attributes
  ;
0 [if]  \ xxx old tmp
: (href_checked)  ( ca len -- ca len )
  \ Check the given empty href attribute.
  \ If there's no anchor, the href is not valid. 
  link_anchor $@len 0= abort" Empty href link" 
  ;
: href_checked  ( ca len -- ca len )
  \ Check the given href attribute, if it's empty.
  dup 0= if  (href_checked)  then
  ;
[then]
: (get_link_href)  ( ca len -- )
  unshortcut 
\  href_checked  \ xxx old
  2dup set_link_type
  local_link? if  -anchor  then  href=!
  ;
: get_link_href  ( "href<spaces>" -- )
  \ Parse and store the link href attribute.
  parse-word (get_link_href)
\  ." ---> " href=@ type cr  \ xxx informer
\  external_link? if  ." EXTERNAL LINK: " href=@ type cr  then  \ xxx informer
  [ false ] [if]  \ simple version
    parse-name end_of_link_section? 0=
    abort" Space not allowed in link href"
  [else]  \ no abort  \ xxx tmp
    \ This code is required until the migration from Simplilo is finished
    \ because some URL have "__",
    \ what simplilo2fendo converts to " __ "
    \ (e.g. page <es.diario.2010.08.29.txt>)
    begin  parse-name end_of_link_section? 0=
    while  s" <!-- xxx fixme space in link filename or URL -->" echo
    repeat
  [then]
  ;
: parse_link  ( "linkmarkup ]]" -- )
  \ Parse and store the link attributes.
  get_link_href
\  ." ---> " href=@ type cr  \ xxx informer
  link_finished? @ 0= if
\    ." link not finished; href= " href=@ type cr  \ xxx informer
    parse_link_text link_finished? @ 0=
    if  
\      ." link not finished; link text= " link_text $@ type cr  \ xxx informer
      get_link_raw_attributes 
      then
  then
\  ." ---> " href=@ type cr  \ xxx informer
  ;
: missing_local_link_text  ( -- ca len )
  href=@ -extension 2dup required_data<id$
  >sb  \ xxx tmp
  evaluate title
  echo> @ >r echo>string
  >attributes< -attributes  \ use the alternative set and init it
  evaluate_content
  r> echo> ! >attributes< echoed $@
  ;
: missing_external_link_text  ( -- ca len )
  href=@ 
  ;
: missing_file_link_text  ( -- ca len )
  href=@ -path
  ;
: missing_link_text  ( -- ca len )
  \ Set a proper link text if it's missing.
  \ xxx todo
  local_link?  if  missing_local_link_text exit  then
  external_link?  if  missing_external_link_text exit  then  \  xxx 
  file_link?  if  missing_file_link_text exit  then  \ xxx
  true abort" Wrong link type"  \ xxx tmp
  ;
: external_class  ( -- )
  \ Add "external" to the class attribute.
  class=@ s" external" bs& class=!
  ;
: link_anchor+  ( ca len -- )
  \ Restore the link anchor of the local href attribute, if any.
  link_anchor $@ +anchor
  ;
: convert_local_link_href  ( ca1 len1 -- ca2 len2 )
  \ Convert a raw local href to a finished href.
  dup if  data<id$>id target_file  then  link_anchor+
  ;
: url  ( ca1 len1 -- ca2 len2 )
  \ ca1 len1 = page id
  \ ca2 len2 = URL
  s" http://" domain $@ s+ 2swap
  data<id$>id target_file s+
  ;
: convert_file_link_href  ( ca len -- ca' len' )
  s" file://" -prefix  files_subdir $@ 2swap s+  
  ;
: convert_link_href  ( ca len -- ca' len' )
  \ ca len = href attribute, without anchor
  link_type @ case
    local_link      of  convert_local_link_href     endof
    file_link       of  convert_file_link_href      endof
  endcase 
  ;
variable local_link_to_draft_page?
: (tune_local_hreflang)  ( a -- )
  \ Set the hreflang attribute of a local link, if needed.
  \ a = page id of the link destination
  s" lang 2dup current_lang" evaluate str=
  if  2drop  else  hreflang=?!  then
  ;
: tune_local_hreflang  ( a -- )
  \ Set the hreflang attribute of a local link, if needed.
  \ a = page id of the link destination
  multilingual? if  (tune_local_hreflang)  else  drop  then
  ;
: tune_local_link  ( -- )
  \ xxx todo fetch alternative language title and description
\  ." tune_local_link" cr  \ xxx informer
  href=@ (data<)id$>id  >r
\  link_text@ ." link_text in tune_local_link (0) = " type cr  \ xxx informer
\  r@ title ." title in tune_local_link (1) = " type cr  \ xxx informer
  r@ draft? local_link_to_draft_page? !
  r@ plain_description title=?!
\  link_text@ ." link_text in tune_local_link (1) = " type cr  \ xxx informer
  r@ title 
\  ." title in tune_local_link (2) = " 2dup type cr  \ xxx informer
  link_text?!  \ xxx bug: this call corrupts 'link_text'
\  link_text@ ." link_text in tune_local_link (2) = " type cr  \ xxx informer
  r@ tune_local_hreflang
  r> access_key accesskey=?!
\  ." end of tune_local_link" cr  \ xxx informer
  ;
: tune_link  ( -- )  \ xxx todo
  \ Tune the attributes parsed from the link.
  local_link? if  tune_local_link  then
  href=@ convert_link_href href=!
  link_text@ empty? if  missing_link_text link_text!  then
  external_link? if  external_class  then
  ;
: echo_link_text  ( -- )
  \ Echo just the link text.
  echo_space evaluate_link_text
  ;
: (echo_link)  ( -- )
  \ Echo the final link.
  [<a>] evaluate_link_text [</a>]
  ;
: echo_link?  ( -- wf )
  \ Can the current link be echoed?
  href=@ nip  local_link_to_draft_page? @ 0=  and
  ;
: reset_link  ( -- )
  \ Reset the link attributes that are not actual HTML attributes,
  \ and are not reseted by the HTML tags layer.
  s" " link_text!  local_link_to_draft_page? off
  ;
: echo_link  ( -- )
  \ Echo the final link, if possible.
  echo_link? if  (echo_link)  else  echo_link_text  then  reset_link
  ;

\ **************************************************************
\ Tools for languages

\ xxx todo nested, with depth counter

0 [if]

\ The website application must create the language specific
\ markups for the languages used in the content, this way:

language_markups: en
language_markups: eo
language_markups: es

[then]

: (xml:)lang=  ( -- a )
  \ Return the proper language attribute.
  xhtml? @ if  xml:lang=  else  lang=  then
  ;
: :create_markup  ( ca len -- )
  \ Create a 'create' word with the given name in the markup
  \ wordlist.
  \ This is used by definining words that may be invoked by the website
  \ application to create specific markups.
  get-current >r  markup>current :create  r> set-current
  ;
: (((:)  ( ca len -- )
  \ Create a language inline markup.
  \ ca len = ISO code of a language
  2dup s" ((" s+ :create_markup s,
  does>  ( -- ) ( dfa )
    count (xml:)lang= attribute! [<span>]
  ;
: ((:  ( "name" -- )
  \ Create a language inline markup.
  \ name = ISO code of a language
  parse-name? abort" Missing language code" (((:)
  ;
: ((((:)  ( ca len -- )
  \ Create a language block markup.
  \ ca len = ISO code of a language
  2dup s" (((" s+ :create_markup s,
  does>  ( -- ) ( dfa )
    count (xml:)lang= attribute! [<div>]
  ;
: (((:  ( "name" -- )
  \ Create a language block markup.
  \ name = ISO code of a language
  parse-name? abort" Missing language code" ((((:)
  ;
: language_markups:  ( "name" -- )
  \ Create inline and block language markups.
  \ Used by the website application to create all
  \ language markups used in the contents.
  \ name = ISO code of a language
  parse-name? abort" Missing language code"
  2dup (((:) ((((:)
  ;

variable #nothings  \ counter of empty parsings \ xxx tmp moved from <fendo_parser.fs>

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
  ;  immediate
: *}  ( -- )
  true abort" '*}' without '{*'"
  ;  immediate

\ Merged Forth code

true [if]  \ xxx first version

: evaluate_forth_code  ( i*x ca len -- j*x )
  get-order n>r
  only fendo>order markup>order forth>order 
  >sb  \ xxx tmp
  evaluate
  nr> set-order
\  cr ." <[..]> done!" key drop  \ xxx informer
  ;
: <[  ( "forthcode ]>" -- )
  \ Start, parse and interpret a Forth block.
  1 forth_code_depth +!
  parse_forth_code 
\  cr ." <[ " 2dup type ." ]>" cr  \ xxx informer
  evaluate_forth_code
  ;  immediate
: ]>  ( -- )
  \ Finish a Forth block.
  \ xxx todo
  forth_code_depth @
\  dup   \ xxx
  0= abort" ']>' without '<['"
\  1 = if  \ xxx
\    only markup>order
\    separate? off
\  then
  -1 forth_code_depth +!
  ;  immediate

[then]

0 [if]  \ experimental version

\ quite different approach
\ xxx todo interpret numbers

: <[  ( "forthcode ]>" -- )
  \ Start a Forth code block.
  1 forth_code_depth +!
  forth>order 
  ; 
: ]>  ( -- )
  \ Finish a Forth block.
  forth_code_depth @
  0= abort" ']>' without '<['"
  previous
  -1 forth_code_depth +!
  ; 

[then]


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
' echo_cr alias \n

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
  block_source_code{ (###) }block_source_code
  ;

\ Headings

: =  ( -- )
  \ Open or close a <h1> heading.
  ['] <h1> ['] </h1> opened_[=]? markups
  ;
: ==  ( -- )
  \ Open or close a <h2> heading.
\  cr ." opened_[=]? = " opened_[=]? ? key drop  \ xxx informer
  ['] <h2> ['] </h2> opened_[=]? markups
\  depth abort" stack not empty"  \ xxx informer
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
\ ." | rendered"  \ xxx informer
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
  parse_link tune_link echo_link
  ;
: ]]  ( -- )
  true abort" ']]' without '[['"
  ;

\ Language

: ))  ( -- )
  \ End an inline language region.
  </span> separate? on
  ;
: )))  ( -- )
  \ End a block language region.
  </div> separate? on
  ;

\ Verbatim or pass-through blocks

: {{{  ( "text<space>}}}<space>" -- )
  \ Open a verbatim or pass-through block.
  \ Its content will be copied "as-is" to the target file.
  \ xxx todo combine with '(###)'?
  \ xxx todo preserve spaces (reading complete lines)
  begin
    read_source_line 0= abort" Missing closing '}}}'"
    2dup trim s" }}}" str= dup >r 0= ?echo_line r>
  until
  ;
: }}}  ( -- )
  \ Close a verbatim or pass-through block.
  true abort" '}}}' without '{{{'"
  ;  immediate

\ Escape

: ~  ( "name" -- )
  \ Escape a name: Parse and echo it, even if it's a markup.
  parse-name? abort" Parseable name expected by '~'"  content
  ; 

\ Punctuation
\ xxx todo complete as required

punctuation: !
punctuation: "
punctuation: ",
punctuation: ".
punctuation: ":
punctuation: ";
punctuation: '
punctuation: )
punctuation: ),
punctuation: ).
punctuation: ):
punctuation: );
punctuation: ,
punctuation: .
punctuation: ...
punctuation: ...),
punctuation: ...).
punctuation: ...);
punctuation: ...»
punctuation: ...».
punctuation: ...»;
punctuation: :
punctuation: ;
punctuation: ?
punctuation: ]
punctuation: }
punctuation: »
punctuation: »,
punctuation: ».

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
\ 2013-07-20: New: support for link anchors.
\ 2013-07-20: Fix: target extensions is added only to local links.
\ 2013-07-26: New: '\n' as an alias for 'echo_cr'; this lets to
\   make the final HTML cleaner, especially in the template.
\ 2013-07-26: '»,' and '.»' punctuations.
\ 2013-07-28: simpler and more legible 'parse_forth_code'.
\ 2013-08-10: Fix: 'evaluate_forth_code' factored from '<:', and
\   fixed with 'get-order' and 'set-order'.
\ 2013-08-10: Fix: the Forth code parsed by '<:' got
\   corrupted at the end of the template. It seemed a Gforth
\   issue. The Galope's circular string buffer has been used as
\   as layer for 's"' and 's+' and the problem dissapeared.
\ 2013-08-10: Change: 'parse_forth_code' rewritten, more
\   legible.
\ 2013-08-10: Bug: sometimes the content of 'href='
\ gets corrupted at the end of '([[)'. Gforth issue again?
\ Todo: Try FFL's dynamic strings for HTML attributes.
\ 2013-08-12: Fix: '(xml:)lang=' was modifed with '$!', even in
\   when FFL-strings were chosen in the configuration.
\ 2013-08-13: New: ':create_markup'.
\ 2013-08-13: New: 'language_markups:'.
\ 2013-08-14: New: 'ftp://?', 'external_link?', 'unlink';
\   new version of 'link:'.
\ 2013-08-14: Fix: 'abort"' in '*}' lacked a true flag.
\ 2013-08-14: New: 'link_text!', 'link_text@'.
\ 2013-08-14: New: 'unraw_attributes'.
\ 2013-08-15: Fix: now '[[' empties 'link_text' at the end.
\ 2013-08-15: New: 'external_class' to mark the external links.
\ 2013-09-05: Fix: 'tune_link'.
\ 2013-09-29: 'unlink' is factored with 'unlinked?'.
\ 2013-09-29: '>link_type_id' now checks if the link is empty;
\   and is factored with '(>link_type_id)'.
\ 2013-10-01: Change: '<:' and ':>' renamed to '<[' and ']>'.
\ 2013-10-22: Change: all code about user's bookmark links and
\   their "unlinking" is moved to its own file and the words
\   are renamed: "(un)shortcut" is used instead of "(un)link".
\ 2013-10-22: New: 'link' creates links to local pages, at the
\   application level.
\ 2013-10-25: Change: '>sb' added before 'evaluate', just to get
\   some clue about the string corruptions.
\ 2013-10-30: Change: '([[)' removed; the final code of '[[' has
\   been factored out as 'echo_link', '(echo_link)' and
\   'echo_link_text'.
\ 2013-10-30 Change: More immediate versions of tags used.
\ 2013-11-05: Fix: 'tune_local_link' evaluated the title and
\   consumed it.
\ 2013-11-05: Fix: local links with only the page id (no text, no raw
\   attrs), lacked the "html" extension.
\ 2013-11-06: New: 'href_checked'.
\ 2013-11-06: Improvement: 'get_link_href', '+anchor'.
\ 2013-11-07: Fix: local links with anchors work fine in
\   all cases.
\ 2013-11-07: New: '{{{' and '}}}', after Creole markup.
\ 2013-11-07: Change: '###-line' and '/###-line' renamed to
\   'source-line' and '/source-line'.
\ 2013-11-07: New: "https" links are recognized.
\ 2013-11-07: New: links to draft local pages are recognized.
\ 2013-11-09: Change: alias 's&' changed to the original 'bs&',
\   provided by <galope/sb.fs>, because also alias 's+' for 'bs+' has
\   been removed, in order to use the original Gforth's 's+' in several
\   cases.
\ 2013-11-09: New: 'read_source_line'.
\ 2013-11-09: New: The "###" markup highlights the code.
\ 2013-11-11: New: '(get_link_href)' factored out from 'get_link_href'
\   in order to use it in <fendo_tools.fs>.
\ 2013-11-11: New: 'tune_local_hreflang' sets the hreflang of local
\   links when needed.
\ 2013-11-11: Fix: anchors of external links were removed from the
\   URL.
\ 2013-11-18: Fix: 'convert_local_link_href' worked only for the
\   current page, and didn't used 'target_file', but only added
\   the target extension.
\ 2013-11-18: New: 'url'.

[then]
