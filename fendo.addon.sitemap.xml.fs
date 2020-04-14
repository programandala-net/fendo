.( fendo.addon.sitemap.xml.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Sitemap.xml addon.

\ XXX UNDER DEVELOPMENT

\ Last modified 202004141706.
\ See change log at the end of the file.

\ Copyright (C) 2015,2017,2018,2020 Marcos Cruz (programandala.net)

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
\ TODO

\ Everything.

\ ==============================================================
\ Requirements

forth_definitions

require galope/n-to-r.fs \ `n>r`
require galope/n-r-from.fs \ `nr>`
require galope/s-constant.fs \ `sconstant`

fendo_definitions

\ ==============================================================
\ Configurable texts

s" Content"
sconstant sitemap_default_content$
defer sitemap_content$
' sitemap_default_content$ is sitemap_content$

s" <strong>New page<strong>: "
sconstant sitemap_default_new_page$
defer sitemap_new_page$
' sitemap_default_new_page$ is sitemap_new_page$

s" <strong>Updated page</strong>: "
sconstant sitemap_default_updated_page$
defer sitemap_updated_page$
' sitemap_default_updated_page$ is sitemap_updated_page$

s" <strong>Edit summary</strong>: "
sconstant sitemap_default_edit_summary$
defer sitemap_edit_summary$
' sitemap_default_edit_summary$ is sitemap_edit_summary$

\ ==============================================================
\ Calculated data

: pid#>taguri ( a -- ca len )
  >r s" tag:" domain s+ s" ," s+
  r@ created 10 min s+ s" :" s+
  r> target_file s+ ;
  \ Return the tag URI of a page.
  \ Code converted from ForthCMS' `>page-taguri`, by the same author.
  \ a = page ID

\ ==============================================================
\ Tags

: <urlset> ( -- ) echo_cr s" urlset" {html  ;
: </urlset> ( -- ) echo_cr s" urlset" html}  ;
: <url> ( -- ) echo_cr s" url" {html  ;
: </url> ( -- ) echo_cr s" url" html}  ;
: <loc> ( -- ) echo_cr s" loc" {html  ;
: </loc> ( -- ) echo_cr s" loc" html}  ;
: <lastmod> ( -- ) echo_cr s" lastmod" {html  ;
: </lastmod> ( -- ) s" lastmod" html}  ;
: <changefreq> ( -- ) echo_cr s" changefreq" {html  ;
: </changefreq> ( -- ) s" changefreq" html}  ;
: <priority> ( -- ) echo_cr s" priority" {html  ;
: </priority> ( -- ) s" priority" html}  ;

\ ==============================================================
\ sitemap feed

: sitemap_link ( ca1 len1 ca2 len2 -- )
  rel=! href=! [<link/>] ;
  \ ca1 len1 = URL
  \ ca2 len2 = rel attribute

: sitemap_xhtml_summary{ ( -- )
  s" xhtml" type=! <summary> s" http://www.w3.org/1999/xhtml" xmlns=! [<div>] ;

: }sitemap_xhtml_summary ( -- )
  [</div>] </summary> ;

: sitemap_feed_author ( -- )
  <author> <name> site_author echo </name> </author> ;

: sitemap_feed_id ( -- )
  <id> current_lang$ pid$>url echo </id> ;
  \ The feed id is the website home page for the current language.

defer sitemap_site_title$ ( -- ca len )
' site_title is sitemap_site_title$
: sitemap_feed_title ( -- )
  [<title>] sitemap_site_title$ unmarkup echo [</title>] ;

: sitemap_feed_subtitle ( -- )
  <subtitle> site_subtitle unmarkup echo </subtitle> ;

: sitemap_feed_alternate_link ( -- )
  current_lang$ 2dup hreflang=! pid$>url s" alternate" sitemap_link ;

: sitemap_feed_selflink ( ca len -- )
  current_lang$ hreflang=! current_target_file_url s" self" sitemap_link ;

: sitemap_feed_links ( -- )
  sitemap_feed_alternate_link
  sitemap_feed_selflink ;

: time_zone ( -- ca len )
  s" date +%:z > /tmp/fendo.time_zone.txt" system
  s" /tmp/fendo.time_zone.txt" slurp-file
  1- ; \ remove the final line feed
  \ XXX not used

: sitemap_feed_updated ( -- )
  <updated> current_page modified echo </updated> ;

: sitemap_feed_generator ( -- )
  <generator> generator echo </generator> ;

: sitemap_feed_icon ( -- )
  <icon> site_icon +domain_url echo </icon> ;

: sitemap_feed_header ( -- )
  sitemap_feed_title
  sitemap_feed_subtitle
  sitemap_feed_links
  sitemap_feed_icon
  sitemap_feed_updated
  sitemap_feed_author
  sitemap_feed_id
  sitemap_feed_generator ;

: (sitemap{) ( -- f )
  xhtml?  true to xhtml?
  open_target
  s" <?xml version='1.0' encoding='utf-8'?>" echo
  current_lang$ xml:lang=!  domain_url xml:base=!
  s" http://www.w3.org/2005/sitemap" xmlns=!  <feed>
  sitemap_feed_header ;
  \ Create an sitemap file.
  \ f = saved content of `xhtml?`, to be restored by `}sitemap`

: sitemap{ ( -- )
  do_page? if  .sourcefilename (sitemap{)  else  skip_page  then ;
  \ Start the sitemap content, if needed.
  \ The end of the content is marked with the `}sitemap` markup.
  \ Only one 'sitemap{ ... }sitemap' block is allowed in the page.

: }sitemap ( f -- )
  </feed> close_target  to xhtml? ;
  \ Finish and close the sitemap file.
  \ f = saved `xhtml?`

\ ==============================================================
\ sitemap entries

: sitemap_entry_title ( a -- )
  s" xhtml" type=! [<title>] title evaluate_content [</title>] ;
  \ a = page ID

: sitemap_entry_id ( a -- )
  <id> pid#>taguri echo </id> ;
  \ a = page ID

: sitemap_entry_links ( a -- )
  dup pid#>lang$ hreflang=! pid#>url s" alternate" sitemap_link ;
  \ a = page ID

: sitemap_entry_updated ( a -- )
  <updated> modified echo </updated> ;
  \ a = page ID

: sitemap_entry_published ( a -- )
  <published> created echo </published> ;
  \ a = page ID

defer (sitemap_entry_summary)
: sitemap_entry_summary ( -- )
  sitemap_xhtml_summary{ (sitemap_entry_summary) }sitemap_xhtml_summary ;

: sitemap_entry_default_summary ( a -- )
  <summary> description unmarkup echo </summary> ;
  \ a = page ID

: .sitemap_entry_comment ( ca len a -- a )
  >r echo_line r> ;
  \ a = page ID

: sitemap_entry_updated_summary ( a -- )
  [ false ] [if]
    \ XXX OLD
    [<p>] sitemap_updated_page$ echo_line
    dup >r description evaluate_content
    r> edit_summary dup
    if  echo_space evaluate_content  else  2drop  then
    [</p>]
  [else]
    [<p>]  sitemap_updated_page$ echo_line
    dup description evaluate_content  [</p>]
    edit_summary dup if
      [<p>] sitemap_edit_summary$ echo_line
      echo_space evaluate_content  [</p>]
    else  2drop  then
  [then] ;
  \ a = page ID

: sitemap_entry_new_summary ( ca len -- )
  [<p>] sitemap_new_page$ echo_line
  description evaluate_content [</p>] ;
  \ ca len = page ID

: set_default_sitemap_entry_summary ( -- )
  ['] sitemap_entry_default_summary is (sitemap_entry_summary) ;

set_default_sitemap_entry_summary
: sitemap_entry ( ca len -- )
  <entry>  pid$>pid# >r
  r@ sitemap_entry_title
  r@ sitemap_entry_id
  r@ sitemap_entry_links
  r@ sitemap_entry_published
  r@ sitemap_entry_updated
  r> sitemap_entry_summary
  </entry> ;
  \ Create an sitemap entry in the sitemap file.
  \ ca len = page ID

: (sitemap_entry) ( ca len xt -- )
  is (sitemap_entry_summary)
  sitemap_entry set_default_sitemap_entry_summary ;
  \ Create an sitemap entry in the sitemap file, with non-default summary.
  \ ca len = page ID
  \ xt = type of sitemap entry summary, new or updated

: sitemap_updated_entry ( ca len -- )
  ['] sitemap_entry_updated_summary (sitemap_entry) ;
  \ Create an sitemap entry in the sitemap file,
  \ about an updated page of the site.
  \ ca len = page ID

: sitemap_new_entry ( ca len -- )
  ['] sitemap_entry_new_summary (sitemap_entry) ;
  \ Create an sitemap entry in the sitemap file, about a new page of the site.
  \ ca len = page ID

.( fendo.addon.sitemap.fs compiled) cr

\ ==============================================================
\ Change log

\ 2015-10-05: Start, using the code of the Atom module.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-08: Update notation of page IDs in comments and strings.
\
\ 2018-12-17: Update: replace `pid$>data>pid#` with `pid$>pid#`.
\
\ 2020-04-14: Define strings constants with `sconstant` instead of
\ `2constant`.

\ vim: filetype=gforth
