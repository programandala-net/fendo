.( fendo.markup.fendo.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ XXX TODO --

\ 2014-04-21: factor every markup to its own file.
\ 2014-04-21: Idea: flag to make the headers numbered.
\ 2014-02-04: write the AsciiDoc's <<< markup for CSS page break.
\ 2013-11-07: make closing heading optional.
\ 2013-10-30: Optional file size in file links.
\ 2013-07-20: Idea for nested lists: prefix words to increase
\ and decrease the depth: >> << .
\ 2013-06-04: Nested lists.
\ 2013-06-04: Flag the first markup of the current line, in
\ order to use '--' both forth nested lists and delete, or '**'
\ for list and for bold.
\ 2013-06-19: Compare Creole's markups with txt2tags' markups.
\ 2014-03-04: Change: parser vectors moved to <fendo.markup.common.fs>.

\ ==============================================================
\ Requirements

forth_definitions

require galope/replaced.fs  \ 'replaced'

fendo_definitions

require ./fendo.addon.source_code.common.fs  \ XXX TMP
require ./fendo.links.fs  \ XXX FIXME already loaded by the main file

\ ==============================================================
\ Debug tools

: xxxtype ( ca len -- ca len )
  2dup ." «" type ." »" ;

: XXX. ( x -- x )
  dup ." «" . ." »" ;

\ ==============================================================
\ Generic tool words for strings

\ XXX TODO -- move?

: concatenate ( ca1 len1 ca2 len2 -- ca1' len1' )
  2swap dup if  s"  " s+ 2swap s+ exit  then  2drop ;
  \ Concatenates two string with a joining space.

: ?concatenate ( ca1 len1 ca2 len2 f -- ca1' len1' )
  if  concatenate  else  2drop  then ;
  \ Concatenates two strings with a joining space.

: otherwise_concatenate ( ca1 len1 ca2 len2 f -- ca1' len1' f )
  dup >r 0= ?concatenate r> ;
  \ Wrapper for '?concatenate'.
  \ If f is false, concatenate ca1 len1 and ca2 len2;
  \ if f is true, drop ca2 len2.

\ ==============================================================
\ Generic tool words for markup and parsing

\ Counters
\ XXX used only by the parser; but will be required here too
\ XXX TODO -- somehow move to the parser
variable #markups     \ consecutive markups parsed
variable #nonmarkups  \ consecutive nonmarkups parsed
variable #parsed      \ items already parsed in the current line (before the current item)
variable parsed$      \ latest parsed name

variable #nothings  \ counter of empty parsings
                    \ XXX TMP -- moved from <fendo_parser.fs>

: first_on_the_line? ( -- f )
  #parsed @ 0= ;
  \ Is the last parsed name the first one on the current line?

: exhausted? ( -- f )
  [false] [if]
    \ First version, doesn't work when there are trailing spaces
    \ at the end of the line.
    >in @ source nip =
  [else]
    \ Second version, works fine when there are trailing spaces
    \ at the end of the line:
    save-input  parse-name empty? >r  restore-input throw  r>
  [then] ;
  \ Is the current source line exhausted?

: markups ( xt1 xt2 a -- )
  dup >r @
  if    nip false
        \ execute_markup? on  preserve_eol? off  \ XXX TMP
  else  drop true
  then  r> !  execute ;
  \ Open or close a HTML tag.
  \ This code is based on FML, a Forth-ish Markup Language for RetroWiki.
  \ xt1 = execution token of the opening HTML tag
  \ xt2 = execution token of the closing HTML tag
  \ a = markup flag variable: is the markup already open?
  \ XXX TODO -- simplify with 'inverted'

variable opened_[####]?   \ is there an open block code?
variable opened_[##]?     \ is there an open inline code?
variable opened_['''']?   \ is there an open block quote?
variable opened_['']?     \ is there an open inline quote?
variable opened_[((((]?   \ is there an open '(((('?  \ XXX not used yet
variable opened_[((]?     \ is there an open '(('?  \ XXX not used yet
variable opened_[**]?     \ is there an open '**'?
variable opened_[++++]?   \ is there an open '++++'?
variable opened_[++]?     \ is there an open '++'?
variable opened_[,,]?     \ is there an open ',,'?
variable opened_[----]?   \ is there an open '----'?
variable opened_[--]?     \ is there an open '--'?
variable opened_[//]?     \ is there an open '//'?
variable opened_[=]?      \ is there an open h1 heading?
variable opened_[=|=]?    \ is there an open table caption?
variable opened_[^^]?     \ is there an open '^^'?
variable opened_[_]?      \ is there an open '_'?
variable opened_[__]?     \ is there an open '__'?

variable #heading       \ level of the opened heading  \ XXX not used yet

false [if]

  \ First version, one flag shared by all headings.  This causes the
  \ opening and the closing tags become reversed in some unknown
  \ conditions, when several pages are parsed.  Maybe the reason is
  \ some flag remains set because a hidden markup error in certain
  \ page.  The new word 'opened_markups_off' solves the problem.

  ' opened_[=]?
  dup alias opened_[==]?    \ is there an open h2 heading?
  dup alias opened_[===]?    \ is there an open h3 heading?
  dup alias opened_[====]?    \ is there an open h4 heading?
  dup alias opened_[=====]?    \ is there an open h5 heading?
  alias opened_[======]?    \ is there an open h6 heading?

[else]

  \ Second version, one flag for every heading; in theory one common
  \ flag would be enough, because headings are not nested.  Beside,
  \ somehow this seems to fix the problem of the first version.  The
  \ new word 'opened_markups_off' solves the problem, anyway.

  variable opened_[==]?    \ is there an open h2 heading?
  variable opened_[===]?    \ is there an open h3 heading?
  variable opened_[====]?    \ is there an open h4 heading?
  variable opened_[=====]?    \ is there an open h5 heading?
  variable opened_[======]?    \ is there an open h6 heading?

[then]

: opened_markups_off ( -- )
  opened_[####]? off
  opened_[##]? off
  opened_['''']? off
  opened_['']? off
  opened_[((((]? off  \ XXX not used yet
  opened_[((]? off  \ XXX not used yet
  opened_[**]? off
  opened_[++++]? off
  opened_[++]? off
  opened_[,,]? off
  opened_[----]? off
  opened_[--]? off
  opened_[//]? off
  opened_[======]? off
  opened_[=====]? off
  opened_[====]? off
  opened_[===]? off
  opened_[==]? off
  opened_[=]? off
  opened_[=|=]? off
  opened_[^^]? off
  opened_[_]? off
  opened_[__]? off ;
  \ Unset all markup flags.
  \ This is used in '(content{)' (defined in <fendo_parser.fs>),
  \ in order to make sure all flags are unset before rendering a new
  \ page.

: or_end_of_section? ( ca len wf1 -- wf2 )
  >r  s" |" str=  r> or ;
  \ ca len = latest name parsed in the alt attribute section
  \ Used by links and images markup.

: unraw_attributes ( ca len -- )
  s\" =\" " s\" =\"" replaced
  s" =' " s" ='" replaced
  >sb  \ XXX TMP
  evaluate ;
  \ Extract and store the individual attributes from
  \ a string of raw verbatim attributes.
  \ Used by links and images markup.

: :create_markup ( ca len -- )
  get-current >r  markup>current :create  r> set-current ;
  \ Create a 'create' word with the given name in the markup
  \ wordlist.
  \ This is used by defining words that may be invoked by the website
  \ application to create custom markups.

\ ==============================================================
\ Actual markup

\ The Fendo markup was inspired by Creole (http://wikicreole.org),
\ txt2tags (http://txt2tags.org), Asciidoctor (http://ascidoctor.org)
\ and others.

markup_definitions

\ Grouping

: _ ( -- )
  ['] <p> ['] </p> opened_[_]? markups  separate? off ;
  \ Open or close a <p> region.

: -------- ( -- )
  <hr/> ;
  \ Create a horizontal rule.

: \\ ( -- )
  <br/> ;
  \ Create a line break.

' echo_cr alias \n

\ Text

: // ( -- )
  ['] <em> ['] </em> opened_[//]? markups ;
  \ Open or close a <em> region.

: ** ( -- )
  ['] <strong> ['] </strong> opened_[**]? markups ;
  \ Open or close a <strong> region.

: __ ( -- )
  ['] <u> ['] </u> opened_[__]? markups ;
  \ Start or finish a <u> region.

: ^^ ( -- )
  ['] <sup> ['] </sup> opened_[^^]? markups ;
  \ Start or finish a <sup> region.

: ,, ( -- )
  ['] <sub> ['] </sub> opened_[,,]? markups ;
  \ Start or finish a <sub> region.

\ Quotes

: '' ( -- )
  ['] <q> ['] </q> opened_['']? markups ;
  \ Open or close a <q> region.

: '''' ( -- )
  ['] <blockquote> ['] </blockquote> opened_['''']? markups ;
  \ Open or close a <blockquote> region.

\ Insertions

: ++ ( -- )
  ['] <ins> ['] </ins> opened_[++]? markups ;
  \ Open or close an <ins> region.

: ++++ ( -- )
  ['] <ins> ['] </ins> opened_[++++]? markups ;
  \ Open or close an <ins> block.
  \ XXX TODO add <div>?

\ Removals

: -- ( -- )
  ['] <del> ['] </del> opened_[--]? markups ;
  \ Open or close a <del> region.

: ---- ( -- )
  ['] <del> ['] </del> opened_[----]? markups ;
  \ Open or close a <del> block.
  \ XXX TODO add <div>?

\ Generic

: (( ( -- )
  [<span>] ;
  \ Start a generic inline zone.

: )) ( -- )
  [</span>] ;
  \ End an generic inline zone.

: (((( ( -- )
  [<div>] ;
  \ Start a generic block zone.

: )))) ( -- )
  [</div>] ;
  \ End a generic block zone.

require ./fendo.markup.fendo.code.fs
require ./fendo.markup.fendo.comment.fs
require ./fendo.markup.fendo.forth.fs
require ./fendo.markup.fendo.heading.fs
require ./fendo.markup.fendo.image.fs
require ./fendo.markup.fendo.language.fs
require ./fendo.markup.fendo.link.fs
require ./fendo.markup.fendo.list.fs
require ./fendo.markup.fendo.literal.fs
require ./fendo.markup.fendo.passthrough.fs
require ./fendo.markup.fendo.table.fs

\ XXX WARNING: Punctuation must be the last markup file required, and
\ no markup must be defined after the punctuation, because it creates
\ markup words with the name of Forth words (e.g. '!').
\ XXX FIXME Solvi this by changing the _order_ set by 'markup_definitions'?

require ./fendo.markup.fendo.punctuation.fs

fendo_definitions

.( fendo.markup.fendo.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2013-05-18: Start. First HTML tags.
\
\ 2013-06-01: Paragraphs, lists, headings, delete.
\
\ 2013-06-02: New: also 'previous_space?'.  New: Counters for both
\ types of elements (markups and printable words); required in order
\ to separate words.
\
\ 2013-06-04: New: punctuation words, HTML entity words. More markups.
\
\ 2013-06-05: Change: '|' renamed to '_'; '|' will be needed for the
\ table markup.
\
\ 2013-06-05: New: Finished the code for entities; the common code for
\ entities and punctuation has been factored.
\
\ 2013-06-06: Change: HTML entities moved to <fendo_markup_html.fs>.
\
\ 2013-06-06: New: First version of table markup, based on Creole and
\ text2tags: data cells and header cells. Also caption.
\
\ 2013-06-06: New: several new markups.
\
\ 2013-06-06: Change: renamed from "fendo_markup.fs" to
\ "fendo_markup_wiki.fs"; it is included from the new file
\ <fendo_markup.fs>.
\
\ 2013-06-06: New: Words for merging Forth code in the pages: '<:' and
\ ':>'.
\
\ 2013-06-10: Change: the new '[markup<order]' substitutes
\ '[previous]'.
\
\ 2013-06-18: New: some combined punctuation, e.g. "),".
\
\ 2013-06-28: Change: Forth code wiki markups can be nested.
\
\ 2013-06-28: New: Markups for comments, "{*" and "*}"; can not be
\ nested.
\
\ 2013-06-29: New: First changes to fix and improve the source code
\ markups (the source region needs a special parsing).
\
\ 2013-06-29: New: The image markups are rendered.
\
\ 2013-07-02: Change: Spaces found on filenames or URL don't abort any
\ more, but print a 'XXX FIXME' warning in an HTML comment instead.
\ These undesired spaces are caused by a wrong rendering of "__" by
\ Simplil2Fendo, difficult to fix. Thus manual fix will be required in
\ the final .fs files.
\
\ 2013-07-04: New: language markup.
\
\ 2013-07-04: Fix: separation after punctuation markup.
\
\ 2013-07-04: Fix: now list markups work only at the start of the
\ line.
\
\ 2013-07-05: New: Creole's '*' and '#' are alias of '-' and '+', for
\ easier migration (so far converting the lists markups in the
\ original sources with Simplilo2Fendo seems difficult).
\
\ 2013-07-12: Finished 'link:'; changed 'link:?'.
\
\ 2013-07-12: Change: '?_echo' moved to <fendo_echo.fs>.
\
\ 2013-07-12: Change: '(###)' rewritten to parse whole lines.
\
\ 2013-07-14: New: '(###)' finished.
\
\ 2013-07-20: New: support for link anchors.
\
\ 2013-07-20: Fix: target extensions is added only to local links.
\
\ 2013-07-26: New: '\n' as an alias for 'echo_cr'; this lets to make
\ the final HTML cleaner, especially in the template.
\
\ 2013-07-26: '»,' and '.»' punctuations.
\
\ 2013-07-28: simpler and more legible 'parse_forth_code'.
\
\ 2013-08-10: Fix: 'evaluate_forth_code' factored from '<:', and fixed
\ with 'get-order' and 'set-order'.
\
\ 2013-08-10: Fix: the Forth code parsed by '<:' got corrupted at the
\ end of the template. It seemed a Gforth issue. The Galope's circular
\ string buffer has been used as as layer for 's"' and 's+' and the
\ problem dissapeared.
\
\ 2013-08-10: Change: 'parse_forth_code' rewritten, more legible.
\
\ 2013-08-10: Bug: sometimes the content of 'href=' gets corrupted at
\ the end of '([[)'. Gforth issue again?  To do: Try FFL's dynamic
\ strings for HTML attributes.
\
\ 2013-08-12: Fix: '(xml:)lang=' was modifed with '$!', even in when
\ FFL-strings were chosen in the configuration.
\
\ 2013-08-13: New: ':create_markup'.
\
\ 2013-08-13: New: 'language_markups:'.
\
\ 2013-08-14: New: 'ftp://?', 'external_link?', 'unlink'; new version
\ of 'link:'.
\
\ 2013-08-14: Fix: 'abort"' in '*}' lacked a true flag.
\
\ 2013-08-14: New: 'link_text!', 'link_text@'.
\
\ 2013-08-14: New: 'unraw_attributes'.
\
\ 2013-08-15: Fix: now '[[' empties 'link_text' at the end.
\
\ 2013-08-15: New: 'external_class' to mark the external links.
\
\ 2013-09-05: Fix: 'tune_link'.
\
\ 2013-09-29: 'unlink' is factored with 'unlinked?'.
\
\ 2013-09-29: '>link_type_id' now checks if the link is empty; and is
\ factored with '(>link_type_id)'.
\
\ 2013-10-01: Change: '<:' and ':>' renamed to '<[' and ']>'.
\
\ 2013-10-22: Change: all code about user's bookmark links and their
\ "unlinking" is moved to its own file and the words are renamed:
\ "(un)shortcut" is used instead of "(un)link".
\
\ 2013-10-22: New: 'link' creates links to local pages, at the
\ application level.
\
\ 2013-10-25: Change: '>sb' added before 'evaluate', just to get some
\ clue about the string corruptions.
\
\ 2013-10-30: Change: '([[)' removed; the final code of '[[' has been
\ factored out as 'echo_link', '(echo_link)' and 'echo_link_text'.
\
\ 2013-10-30: Change: More immediate versions of tags used.
\
\ 2013-11-05: Fix: 'tune_local_link' evaluated the title and consumed
\ it.
\
\ 2013-11-05: Fix: local links with only the page id (no text, no raw
\ attrs), lacked the "html" extension.
\
\ 2013-11-06: New: 'href_checked'.
\
\ 2013-11-06: Improvement: 'get_link_href', '+anchor'.
\
\ 2013-11-07: Fix: local links with anchors work fine in all cases.
\
\ 2013-11-07: New: '{{{' and '}}}', after Creole markup.
\
\ 2013-11-07: Change: '###-line' and '/###-line' renamed to
\ 'source-line' and '/source-line'.
\
\ 2013-11-07: New: "https" links are recognized.
\
\ 2013-11-07: New: links to draft local pages are recognized.
\
\ 2013-11-09: Change: alias 's&' changed to the original 'bs&',
\ provided by <galope/sb.fs>, because also alias 's+' for 'bs+' has
\ been removed, in order to use the original Gforth's 's+' in several
\ cases.
\
\ 2013-11-09: New: 'read_source_line'.
\
\ 2013-11-09: New: The "###" markup highlights the code.
\
\ 2013-11-11: New: '(get_link_href)' factored out from 'get_link_href'
\ in order to use it in <fendo_tools.fs>.
\
\ 2013-11-11: New: 'tune_local_hreflang' sets the hreflang of local
\ links when needed.
\
\ 2013-11-11: Fix: anchors of external links were removed from the
\ URL.
\
\ 2013-11-18: Fix: 'convert_local_link_href' worked only for the
\ current page, and didn't used 'target_file', but only added the
\ target extension.
\
\ 2013-11-18: New: 'url', 'link_text_suffix'.
\
\ 2013-11-18: Change: '-file://' factored from
\ 'convert_file_link_href'.
\
\ 2013-11-18: Fix: 'highlight_###-zone?' instead of simply
\ 'highlight?', in '(###)'.
\
\ 2013-11-18:  Now all words related to syntax highlighting are in
\ <addons/source_code_common.fs>, not in <addons/source_code.fs>.
\
\ 2013-11-19: Change: '###-line?' returns a fake text with the false
\ flag; this fixes 'plain_###-zone' and requires a change in
\ 'highlighted_###-zone'.
\
\ 2013-11-27: Change: '{* ... *}' changed to '(* ... *)', just
\ implemented in the Galope library.
\
\ 2013-12-05: Change: '(xml:)lang=' moved to
\ <fendo_markup_html_attributes.fs>; '(xml:)lang= attribute!' factored
\ to '(xml:)lang=!' and  moved to <fendo_markup_html_attributes.fs>
\ too.
\
\ 2013-12-06: New: 'opened_markups_off'.
\
\ 2014-01-06: Fix: '###-line' and '(##)' now escape the "<" char with
\ the new word 'escaped_source_code' (defined in
\ <fendo/addons/source_code_common.fs>).
\
\ 2014-01-06: New: "---" markup for em dash.
\
\ 2014-02-03: Fix: '(##)' left the final parsed "##" markup on the
\ stack!
\
\ 2014-02-03: Change: 'punctuation:' renamed to '}punctuation:';
\ ':punctuation' removed.
\
\ 2014-02-03: New: 'punctuation{:' for opening punctuation characters.
\
\ 2014-02-03: New: '{{{' rewritten, based on '###'.
\
\ 2014-02-14: Fix: '###-line?' returned the string also when the
\ output flag was false, and it remained on the stack. It has be
\ rewritten.  Now it always returns   the string. 'plain_###-zone' and
\ 'highlighted_###-zone' have been modified accordingly, and now all
\ is simpler.
\
\ 2014-01-06: Change: the "---" markup for em dash has been removed,
\ because sometimes it must be separated from the previous word,
\ sometimes from the next word, and sometimes from both, depending on
\ the language and the function. So now this character will be up to
\ the writer.  Simplilo2Fendo has been updated to: now it simply
\ converts the '---' markup into the corresponding UTF-8 char.
\
\ 2014-02-24: New: 'set_image_size_attributes'. Support por JPEG
\ images.
\
\ 2014-02-25: Change: 'url' renamed to 'pid$>url'.
\
\ 2014-02-28: Change: 'replaced' is adapted to its new version in
\ Galope.
\
\ 2014-02-28: New: support for PNG images.
\
\ 2014-03-04: Change: now 'tune_link' is invoked in 'echo_link'; both
\ words were always invoqued together.
\
\ 2014-03-08: New: Punctuation: Unicodes quotes, single and double.
\
\ 2014-03-09: Fix: now open headings work only at the start of the
\ line.
\
\ 2014-03-12: Change: "fendo.addon.source_code.common.fs" filename
\ updated.
\
\ 2014-04-20:
\
\ Fix: 'fendo>order definitions' was wrong old code, because fendo
\ consists of several wordlists. Now: 'fendo>order fendo>current'.
\
\ New: '\' line comment markup.
\
\ Markup change: '###' > '####'.
\
\ 2014-04-21:
\
\ Markup change: '----' > '--------';
\
\ New markups: insertions and removals.
\
\ Change: '####:' moved from <fendo-programandala.fs', renamed to
\ 'code_markup:', simplified with ':create_markup' and improved: it
\ creates also inline markup.
\
\ Change: 'language_markups:' renamed to 'language_markup:'.
\
\ Change: file renamed to <fendo.markup.fendo.fs>.
\
\ New markups: '((' and '(((('.
\
\ Parts of the the code are moved to their own files:
\ <fendo.markup.fendo.code.fs>, <fendo.markup.fendo.comment.fs>,
\ <fendo.markup.fendo.forth.fs>, <fendo.markup.fendo.heading.fs>,
\ <fendo.markup.fendo.image.fs>, <fendo.markup.fendo.link.fs>,
\ <fendo.markup.fendo.list.fs>, <fendo.markup.fendo.literal.fs>,
\ <fendo.markup.fendo.passthrough.fs>,
\ <fendo.markup.fendo.punctuation.fs>, <fendo.markup.fendo.table.fs>,
\
\ 2014-11-17: Change: all unnecessary changes of 'separate?' are
\ removed.
\
\ 2014-12-22: Improvement: 'unraw_attributes' supports also single
\ quotes.
\
\ 2017-02-04: Fix typo.
\
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
