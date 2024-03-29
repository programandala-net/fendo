.( fendo.addon.atom.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Atom addon.

\ Last modified  20220902T2032+0200.
\ See change log at the end of the file.

\ Copyright (C) 2009,2014,2015,2017,2018,2020,2022 Marcos Cruz (programandala.net)

\ Fendo is free software; you can redistribute it and/or modify it
\ under the terms of the GNU General Public License as published by
\ the Free Software Foundation; either version 2 of the License, or
\ (at your option) any later version.

\ Fendo is distributed in the hope that it will be useful, but WITHOUT
\ ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
\ or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
\ License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program; if not, see <http://gnu.org/licenses>.

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================
\ TODO {{{1

\ Add xml:lang to all human readable fields.
\
\ Add <logo> (larger image, twice as wide as it's tall).
\
\ Hack the description, in order to let the application to provide a
\ custom string.

\ ==============================================================
\ Requirements {{{1

forth_definitions

require galope/n-to-r.fs \ `n>r`
require galope/n-r-from.fs \ `nr>`
require galope/s-constant.fs \ `sconstant`

fendo_definitions

\ ==============================================================
\ Configurable texts {{{1

s" Content"
sconstant atom_default_content$

defer atom_content$
' atom_default_content$ is atom_content$

s" <strong>New page<strong>: "
sconstant atom_default_new_page$

defer atom_new_page$
' atom_default_new_page$ is atom_new_page$

s" <strong>Updated page</strong>: "
sconstant atom_default_updated_page$

defer atom_updated_page$
' atom_default_updated_page$ is atom_updated_page$

s" <strong>Edit summary</strong>: "
sconstant atom_default_edit_summary$

defer atom_edit_summary$
' atom_default_edit_summary$ is atom_edit_summary$

\ ==============================================================
\ Calculated data {{{1

: pid#>taguri ( a -- ca len )
  >r s" tag:" domain s+ s" ," s+ 
  r@ created 10 min s+ s" :" s+
  r> target_file s+ ;
  \ Return the tag URI of a page.
  \ Code converted from ForthCMS's `>page-taguri`, by the same author.
  \ a = page ID

\ ==============================================================
\ Tags {{{1

: <author> ( -- ) echo_cr s" author" {html  ;
: </author> ( -- ) echo_cr s" author" html}  ;
: <content> ( -- ) echo_cr s" content" {html  ;
: </content> ( -- ) echo_cr s" content" html}  ;
: <entry> ( -- ) echo_cr s" entry" {html  ;
: </entry> ( -- ) echo_cr s" entry" html}  ;
: <feed> ( -- ) echo_cr s" feed" {html  ;
: </feed> ( -- ) echo_cr s" feed" html}  ;
: <generator> ( -- ) echo_cr s" generator" {html  ;
: </generator> ( -- ) s" generator" html}  ;
: <icon> ( -- ) echo_cr s" icon" {html  ;
: </icon> ( -- ) s" icon" html}  ;
: <id> ( -- ) echo_cr s" id" {html  ;
: </id> ( -- ) s" id" html}  ;
: <logo> ( -- ) echo_cr s" logo" {html  ;
: </logo> ( -- ) s" logo" html}  ;
: <name> ( -- ) echo_cr s" name" {html  ;
: </name> ( -- ) s" name" html}  ;
: <published> ( -- ) echo_cr s" published" {html  ;
: </published> ( -- ) s" published" html}  ;
: <subtitle> ( -- ) echo_cr s" subtitle" {html  ;
: </subtitle> ( -- ) s" subtitle" html}  ;
: <summary> ( -- ) echo_cr s" summary" {html  ;
: </summary> ( -- ) echo_cr s" summary" html}  ;
: <updated> ( -- ) echo_cr s" updated" {html  ;
: </updated> ( -- ) s" updated" html}  ;

\ ==============================================================
\ Atom feed {{{1

: atom_link ( ca1 len1 ca2 len2 -- )
  rel=! href=! [<link/>] ;
  \ ca1 len1 = URL
  \ ca2 len2 = rel attribute

: atom_xhtml_summary{ ( -- )
  s" xhtml" type=! <summary> s" http://www.w3.org/1999/xhtml" xmlns=! [<div>] ;

: }atom_xhtml_summary ( -- )
  [</div>] </summary> ;

: atom_feed_author ( -- )
  <author> <name> site_author echo </name> </author> ;

: atom_feed_id ( -- )
  <id> current_lang$ pid$>url echo </id> ;
  \ The feed id is the website home page for the current language.

defer atom_site_title$ ( -- ca len )

' site_title is atom_site_title$

: atom_feed_title ( -- )
  [<title>] atom_site_title$ unmarkup echo [</title>] ;

defer atom_site_subtitle$ ( -- ca len )

' site_subtitle is atom_site_subtitle$

: atom_feed_subtitle ( -- )
  atom_site_subtitle$ unmarkup dup
  if    <subtitle> unmarkup echo </subtitle>
  else  2drop  then ;

: atom_feed_alternate_link ( -- )
  current_lang$ 2dup hreflang=! pid$>url s" alternate" atom_link ;

: atom_feed_selflink ( ca len -- )
  current_lang$ hreflang=! current_target_file_url s" self" atom_link ;

: atom_feed_links ( -- )
  atom_feed_alternate_link
  atom_feed_selflink ;

: time_zone ( -- ca len )
  s" date +%:z > /tmp/fendo.time_zone.txt" system
  s" /tmp/fendo.time_zone.txt" slurp-file
  1- ; \ remove the final line feed
  \ XXX not used

: atom_feed_updated ( -- )
  <updated> current_page modified echo </updated> ;

: atom_feed_generator ( -- )
  <generator> generator echo </generator> ;

: atom_feed_icon ( -- )
  <icon> site_icon +domain_url echo </icon> ;

: atom_feed_header ( -- )
  atom_feed_title
  atom_feed_subtitle
  atom_feed_links
  atom_feed_icon
  atom_feed_updated
  atom_feed_author
  atom_feed_id
  atom_feed_generator ;

: (atom{) ( -- f )
  xhtml?  true to xhtml?
  open_target
  s" <?xml version='1.0' encoding='utf-8'?>" echo
  current_lang$ xml:lang=!  domain_url xml:base=!
  s" http://www.w3.org/2005/Atom" xmlns=!  <feed>
  atom_feed_header ;
  \ Create an Atom file.
  \ f = saved content of `xhtml?`, to be restored by `}atom`

: atom{ ( -- )
  do_page? if  .sourcefilename (atom{)  else  skip_page  then ;
  \ Start the Atom content, if needed.
  \ The end of the content is marked with the `}atom` markup.
  \ Only one `atom{ ... }atom` block is allowed in the page.

: }atom ( f -- )
  \ Finish and close the Atom file.
  \ f = saved `xhtml?`
  </feed> close_target  to xhtml? ;

\ ==============================================================
\ Atom entries {{{1

: atom_entry_title ( a -- )
  s" xhtml" type=! [<title>] title evaluate_content [</title>] ;
  \ a = page ID

: atom_entry_id ( a -- )
  <id> pid#>taguri echo </id> ;
  \ a = page ID

: atom_entry_links ( a -- )
  dup pid#>lang$ hreflang=! pid#>url s" alternate" atom_link ;
  \ a = page ID

: atom_entry_updated ( a -- )
  <updated> modified echo </updated> ;
  \ a = page ID

: atom_entry_published ( a -- )
  <published> created echo </published> ;
  \ a = page ID

defer (atom_entry_summary)
: atom_entry_summary ( -- )
  atom_xhtml_summary{ (atom_entry_summary) }atom_xhtml_summary ;

: atom_entry_default_summary ( a -- )
  <summary> description unmarkup echo </summary> ;
  \ a = page ID

: .atom_entry_comment ( ca len a -- a )
  >r echo_line r> ;
  \ a = page ID

: pid#>content ( a -- ca len )
  save_echo echo>string pid#>pid$ pid$>source included 
  echoed $@ type restore_echo ;
  \ a = page ID
  \ XXX UNDER DEVELOPMENT
  \ XXX TODO -- test

: atom_entry_content ( a -- )
  <content> 
  \ pid#>content echo  \ method 1
  contents \ method 2, after the template
  </content> ;
  \ a = page ID
  \ XXX UNDER DEVELOPMENT
  \ XXX TODO -- test

: atom_entry_updated_summary ( a -- )
  [ false ] [if]
    \ XXX OLD
    [<p>] atom_updated_page$ echo_line
    dup >r description evaluate_content
    r> edit_summary dup
    if  echo_space evaluate_content  else  2drop  then
    [</p>]
  [else]
    [<p>]  atom_updated_page$ echo_line
    dup description evaluate_content  [</p>]
    edit_summary dup if
      [<p>] atom_edit_summary$ echo_line
      echo_space evaluate_content  [</p>]
    else  2drop  then
  [then] ;
  \ a = page ID

: atom_entry_new_summary ( ca len -- )
  \ ca len = page ID
  [<p>] atom_new_page$ echo_line
  description evaluate_content [</p>] ;

: set_default_atom_entry_summary ( -- )
  ['] atom_entry_default_summary is (atom_entry_summary) ;

set_default_atom_entry_summary
: atom_entry ( ca len -- )
  <entry>  pid$>pid# >r
  r@ atom_entry_title
  r@ atom_entry_id
  r@ atom_entry_links
  r@ atom_entry_published
  r@ atom_entry_updated
  \ r@ atom_entry_content \ XXX UNDER DEVELOPMENT
  r> atom_entry_summary
  </entry> ;
  \ Create an Atom entry in the Atom file.
  \ ca len = page ID

: (atom_entry) ( ca len xt -- )
  is (atom_entry_summary)  atom_entry  set_default_atom_entry_summary ;
  \ Create an Atom entry in the Atom file, with non-default summary.
  \ ca len = page ID
  \ xt = type of atom entry summary, new or updated

: atom_updated_entry ( ca len -- )
  ['] atom_entry_updated_summary (atom_entry) ;
  \ Create an Atom entry in the Atom file, about an updated page of the site.
  \ ca len = page ID

: atom_new_entry ( ca len -- )
  ['] atom_entry_new_summary (atom_entry) ;
  \ Create an Atom entry in the Atom file, about a new page of the site.
  \ ca len = page ID

.( fendo.addon.atom.fs compiled) cr

\ ==============================================================
\ Change log {{{1

\ 2014-06-05: Start, using the code of the Atom module (last version,
\ from 2009-10-21) of: ForthCMS ("Forth Calm Maker of Sites") version
\ B-00-201206 (http://programandala.net/en.program.forthcms.html).
\
\ 2014-07-06: First changes. `echo` and `echo_line` used instead of
\ the old ForthCMS words.
\
\ 2014-07-08: Site variables converted to Fendo.
\
\ 2014-07-10: More changes to make the code compatible with Fendo.
\
\ 2014-07-11: More changes to make the code compatible with Fendo.
\
\ 2014-11-08: Change: `unmarkup` (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.
\
\ 2014-12-02: Fix: `xmlns=` was wrongly set in "<summary>", not in the
\ inner "<div>", what made the news reader to ignore the summary.
\
\ 2014-12-05: Fix: now `atom_feed_icon` creates a complete URL, not
\ just the file name.
\
\ 2014-12-05: Improvement: `(atom{)` now uses `xml:base=!` and
\ `xml:lang=!` with `<feed>`; `atom_feed_selflink` and
\ `atom_feed_alternate_link` use `hreflang=!`.
\
\ 2014-12-05: Improvement: `(atom{)` saves and sets `xhtml?`, and
\ `}atom` restores it. This forces some HTML tags and attributes to
\ get the proper XML flavour, without affecting the content pages.
\
\ 2014-12-05: Improvement: `atom_entry_links` now uses `hreflang=!`.
\
\ 2014-12-06: Change: new convention for the string constants and
\ variables; quotes ruined the syntax highlighting.
\
\ 2014-12-06: Improvement: `atom_entry_updated_summary` rewritten; it
\ shows the edit summary metadatum, if not empty.
\
\ 2014-12-06: Improvement: now there's Atom-specific site title
\ (`atom_site_title$`) configurable by the application; it defaults to
\ `site_title`.
\
\ 2014-12-07: Fix: added `forth_definitions` and `fendo_definitions`
\ for requirements, though no problem was detected.
\
\ 2015-02-01: Change: the `xhtml?` variable is a value now.
\
\ 2015-12-18: Confirmed the `<update>` and `<published>` entry tags
\ are right. The problem was the order used by the feed
\ viewer. Fix: the subtitle tag is not created when the subtitle is
\ empty.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2020-04-14: Define strings constants with `sconstant` instead of
\ `2constant`.
\
\ 2020-11-18: Draft the implementation of the `<content>` element.
\
\ 2022-09-02: Improvement: now there's Atom-specific site subwtitle
\ (`atom_site_subtitle$`) configurable by the application; it defaults
\ to `site_subtitle`. Fix `atom_feed_subtitle`.

\ vim: filetype=gforth
