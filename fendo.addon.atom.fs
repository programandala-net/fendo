.( fendo.addon.atom.fs) cr

\ This file is part of Fendo.

\ This file is the Atom addon.

\ Copyright (C) 2009,2014 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2014-06-05: Start, using the code of the Atom module (last version,
\ from 2009-10-21) of: ForthCMS ("Forth Calm Maker of Sites") version
\ B-00-201206 (http://programandala.net/en.program.forthcms.html).
\
\ 2014-07-06: First changes. 'echo' and 'echo_line' used instead of
\ the old ForthCMS words.
\
\ 2014-07-08: Site variables converted to Fendo.
\
\ 2014-07-10: More changes to make the code compatible with Fendo.
\
\ 2014-07-11: More changes to make the code compatible with Fendo.
\
\ 2014-11-08: Change: 'unmarkup' (just implemented) is used instead of
\ hard-coded plain text versions of some data fields.

\ **************************************************************
\ TODO

\ add hreflang to <link>
\ add xml:lang to all human readable fields
\ add <logo> (larger image, twice as wide as it's tall)
\ hack the description, in order to let the application to provide a
\ custom string

\ **************************************************************
\ Requirements

\ include galope/time&date-to-iso.fs  \ XXX not used
\ include galope/yyyymmdd-to-iso.fs  \ XXX not used

\ **************************************************************
\ Configurable texts

s" Content"
2constant atom_default_"content"
defer atom_"content"
' atom_default_"content" is atom_"content"

s" <strong>New page<strong>: "
2constant atom_default_"new_page"
defer atom_"new_page"
' atom_default_"new_page" is atom_"new_page"

s" <strong>Updated page</strong>: "
2constant atom_default_"updated_page"
defer atom_"updated_page"
' atom_default_"updated_page" is atom_"updated_page"

\ **************************************************************
\ Calculated data

: pid#>taguri  ( a -- ca len )
  \ Return the tag URI of a page.
  \ Code converted from ForthCMS' '>page-taguri', by the same author.
  \ a = page id
  >r s" tag:" domain s+ s" ," s+ 
  r@ created 10 min s+ s" :" s+
  r> target_file s+
  ;

\ **************************************************************
\ Tags

: <author>  ( -- )  echo_cr s" author" {html  ;
: </author>  ( -- )  echo_cr s" author" html}  ;
: <entry>  ( -- )  echo_cr s" entry" {html  ;
: </entry>  ( -- )  echo_cr s" entry" html}  ;
: <feed>  ( -- )  echo_cr s" feed" {html  ;
: </feed>  ( -- )  echo_cr s" feed" html}  ;
: <generator>  ( -- )  echo_cr s" generator" {html  ;
: </generator>  ( -- )  s" generator" html}  ;
: <icon>  ( -- )  echo_cr s" icon" {html  ;
: </icon>  ( -- )  s" icon" html}  ;
: <id>  ( -- )  echo_cr s" id" {html  ;
: </id>  ( -- )  s" id" html}  ;
: <logo>  ( -- )  echo_cr s" logo" {html  ;
: </logo>  ( -- )  s" logo" html}  ;
: <name>  ( -- )  echo_cr s" name" {html  ;
: </name>  ( -- )  s" name" html}  ;
: <published>  ( -- )  echo_cr s" published" {html  ;
: </published>  ( -- )  s" published" html}  ;
: <subtitle>  ( -- )  echo_cr s" subtitle" {html  ;
: </subtitle>  ( -- )  s" subtitle" html}  ;
: <summary>  ( -- )  echo_cr s" summary" {html  ;
: </summary>  ( -- )  echo_cr s" summary" html}  ;
: <updated>  ( -- )  echo_cr s" updated" {html  ;
: </updated>  ( -- )  s" updated" html}  ;

\ **************************************************************
\ Atom feed

: atom_link  ( ca1 len1 ca2 len2 -- )
  \ ca1 len1 = URL
  \ ca2 len2 = rel attribute
  rel=! href=! [<link/>]
  ;
: atom_xhtml_summary{  ( -- )
  s" xhtml" type=! s" http://www.w3.org/1999/xhtml" xmlns=! <summary>  [<div>]
  ;
: }atom_xhtml_summary  ( -- )
  [</div>] </summary>
  ;
: atom_feed_author  ( -- )
  <author> <name> site_author echo </name> </author>
  ;
: atom_feed_id  ( -- )
  \ The feed id is the website home page for the current language.
  <id> current_lang$ pid$>url echo </id>
  ;
: atom_feed_title  ( -- )
  [<title>] site_title unmarkup echo [</title>]
  ;
: atom_feed_subtitle  ( -- )
  <subtitle> site_subtitle unmarkup echo </subtitle>
  ;
: atom_feed_alternate_link  ( -- )
  \ iso_lang hreflang=!  \ XXX TODO convert to Fendo
  domain_url s" alternate" atom_link
  ;
: atom_feed_selflink  ( ca len -- )
  current_target_file_url s" self" atom_link
  ;
: atom_feed_links  ( -- )
  atom_feed_alternate_link
  atom_feed_selflink
  ;
: time_zone  ( -- ca len )
  \ XXX not used
  s" date +%:z > /tmp/fendo.time_zone.txt" system
  s" /tmp/fendo.time_zone.txt" slurp-file
  1-  \ remove the final line feed
  ;
: atom_feed_updated  ( -- )
  <updated> current_page modified echo </updated>
  ;
: atom_feed_generator  ( -- )
  <generator> generator echo </generator>
  ;
: atom_feed_icon  ( -- )
  \ XXX TODO add URL
  <icon> site_icon echo </icon>
  ;
: atom_feed_header  ( -- )
  atom_feed_title
  atom_feed_subtitle
  atom_feed_links
  atom_feed_icon
  atom_feed_updated
  atom_feed_author
  atom_feed_id
  atom_feed_generator
  ;
: (atom{)  ( ca1 len1 ca2 len2 -- )
  \ Create an Atom file.
  open_target 
  s" <?xml version='1.0' encoding='utf-8'?>" echo
  s" http://www.w3.org/2005/Atom" xmlns=! <feed>
  atom_feed_header
  ;
: atom{  ( -- )
  \ Start the Atom content, if needed.
  \ The end of the content is marked with the '}atom' markup.
  \ Only one 'atom{ ... }atom' block is allowed in the page.
  do_page? if  .sourcefilename (atom{)  else  skip_page  then
  ;
: }atom  ( -- )
  \ Finish and close the Atom file.
  </feed> close_target
  ;

\ **************************************************************
\ Atom entries

: atom_entry_title  ( a -- )
  \ a = page id
  s" xhtml" type=! [<title>] title evaluate_content [</title>]
  ;
: atom_entry_id  ( a -- )
  \ a = page id
  <id> pid#>taguri echo </id>
  ;
: atom_entry_links  ( a -- )
  \ a = page id
  pid#>url s" alternate" atom_link
  ;
: atom_entry_updated  ( a -- )
  \ a = page id
  <updated> modified echo </updated>
  ;
: atom_entry_published  ( a -- )
  \ a = page id
  <published> created echo </published>
  ;
defer atom_entry_summary
: atom_entry_default_summary  ( a -- )
  \ a = page id
  <summary> description unmarkup echo </summary>
  ;
: .atom_entry_comment  ( ca len a -- a )
  \ a = page id
  >r echo_line r>
  ;
: atom_entry_custom_summary  ( ca1 len1 ca2 len2 -- )
  \ Create a summary field with a custom content.
  \ ca1 len1 = page id
  \ ca2 len2 = custom header to be shown before the actual content
  atom_xhtml_summary{
  [<p>] echo_line  description evaluate_content [</p>]
  }atom_xhtml_summary
  ;
: atom_entry_updated_summary  ( ca len -- )
  \ ca len = page id
  atom_"updated_page" atom_entry_custom_summary
  ;
: atom_entry_new_summary  ( ca len -- )
  \ ca len = page id
  atom_"new_page" atom_entry_custom_summary
  ;
: set_default_atom_entry_summary  ( -- )
  ['] atom_entry_default_summary is atom_entry_summary
  ;
set_default_atom_entry_summary
: atom_entry  ( ca len -- )
  \ Create an Atom entry in the Atom file.
  \ ca len = page id
  <entry>  pid$>data>pid# >r
  r@ atom_entry_title
  r@ atom_entry_id
  r@ atom_entry_links
  r@ atom_entry_published
  r@ atom_entry_updated
  r> atom_entry_summary
  </entry>
  ;
: (atom_entry)  ( ca len xt -- )
  \ Create an Atom entry in the Atom file, with non-default summary.
  \ ca len = page id
  \ xt = type of atom entry summary, new or updated
  is atom_entry_summary  atom_entry  set_default_atom_entry_summary
  ;
: atom_updated_entry  ( ca len -- )
  \ Create an Atom entry in the Atom file, about an updated page of the site.
  \ ca len = page id
  ['] atom_entry_updated_summary (atom_entry)
  ;
: atom_new_entry  ( ca len -- )
  \ Create an Atom entry in the Atom file, about a new page of the site.
  \ ca len = page id
  ['] atom_entry_new_summary (atom_entry)
  ;

.( fendo.addon.atom.fs compiled) cr
