.( fendo.addon.atom.fs) cr

\ This file is part of Fendo.

\ This file is the Atom addon.
\ XXX TODO --- unfinished

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
\   from 2009-10-21) of: ForthCMS ("Forth Calm Maker of Sites") version
\   B-00-201206 (http://programandala.net/en.program.forthcms.html).
\ 2014-07-06: First changes. 'echo' and 'echo_line' used instead of
\   the old ForthCMS words.
\ 2014-07-08: Site variables converted to Fendo.
\ 2014-07-10: More changes to make the code work with Fendo.

\ **************************************************************
\ TODO

\ add hreflang to <link>
\ add xml:lang to all human readable fields
\ add <logo>

\ **************************************************************
\ Requirements

\ XXX TODO

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
  >r s" tag:" domain $@ s+ s" ," s+ 
  r@ created s+ s" :" s+
  r> target_file s+
  ;

\ **************************************************************
\ Tags

: {atom_link}  ( ca1 len1 ca2 len2 ca3 len3 -- )
  \ XXX TODO convert to Fendo
  +rel_attribute 2swap {link}
  ;
: {atom_self_link}  ( ca len -- )
  s" " s" self" {atom_link}
  ;
: {atom_alternate_link}  ( ca len -- )
  \ XXX TODO convert to Fendo
  iso_lang >hreflang_attribute  s" alternate" {atom_link}
  ;
: +atom_lang_attributes}  ( ca1 len1 c_addr2 c2 -- )
  \ XXX TODO convert to Fendo
  \ ca1 len1 = xml tag
  \ ca2 len2 = content type (xhtml or html)
  +type_attribute
  iso_lang +xml:lang_attribute echo_line
  s" >" echo
  ;

: {atom_title}  ( -- )  s" <title" s" html" +atom_lang_attributes}  ;
' {/title} alias {/atom_title}
: {atom_title/}  ( -- )  {atom_title} echo {/atom_title}  ;
: {author}  ( -- )  s" <author>" echo  ;
: {/author}  ( -- )  s" </author>" echo_line  ;
: {entry}  ( -- )  s" <entry>" echo  ;
: {/entry}  ( -- )  s" </entry>" echo_line  ;
: {generator}  ( -- )  s" <generator>" echo  ;
: {/generator}  ( -- )  s" </generator>" echo_line  ;
: {icon}  ( -- )  s" <icon>" echo  ;
: {/icon}  ( -- )  s" </icon>" echo_line  ;
: {icon/}  ( ca len -- )  {icon} echo {/icon}  ;
: {id}  ( -- )  s" <id>" echo  ;
: {/id}  ( -- )  s" </id>" echo_line  ;
: {id/}  ( ca len -- )  {id} echo {/id}  ;
: {logo}  ( -- )  s" <logo>" echo  ;
: {/logo}  ( -- )  s" </logo>" echo_line  ;
: {logo/}  ( ca len -- )  {logo} echo {/logo}  ;
: {name}  ( -- )  s" <name>" echo  ;
: {/name}  ( -- )  s" </name>" echo_line  ;
: {name/}  ( ca len -- )  {name} echo {/name}  ;
: {published}  ( -- )  s" <published>" echo  ;
: {/published}  ( -- )  s" </published>" echo_line  ;
: {published/}  ( ca len -- )  {published} echo {/published}  ;
: {subtitle}  ( -- )  s" <subtitle" s" html" +atom_lang_attributes}  ;
: {/subtitle}  ( -- )  s" </subtitle>" echo  ;
: {subtitle/}  ( ca len -- )  {subtitle} echo {/subtitle}  ;
: {summary}  ( -- )  s" <summary" s" html" +atom_lang_attributes}  ;
: {/summary}  ( -- )  s" </summary>" echo_line  ;
: {summary/}  ( ca len -- )  {summary} echo {/summary}  ;
: {updated}  ( -- )  s" <updated>" echo_line  ;
: {/updated}  ( -- )  s" </updated>" echo  ;
: {updated/}  ( ca len -- )  {updated} echo {/updated}  ;

\ **************************************************************
\ Atom feed

: {atom_xhtml_summary}  ( -- )
  s" <summary" s" xhtml" +atom_lang_attributes}
  s" <div xmlns='http://www.w3.org/1999/xhtml'>" echo_line
  ;
: {/atom_xhtml_summary}  ( -- )
  {/div} {/summary}
  ;

: atom_feed_header_author  ( -- )
  {author} site_author $@ {name/} {/author}
  ;
: atom_feed_header_id  ( -- )
  domain $@ {id/}
  ;
: atom_feed_header_title  ( -- )
  site_plain_title $@ {atom_title/}
  ;
: atom_feed_header_subtitle  ( -- )
  site_plain_subtitle $@ {subtitle/}
  ;
: atom_feed_header_selflink  ( ca len -- )
  domain&current_target_file {atom_self_link}
  ;
: atom_feed_header_links  ( -- )
  domain $@ 2dup  {atom_alternate_link}  atom_feed_header_selflink
  ;
: atom_feed_header_updated  ( -- )
  {updated} time&date iso_date&time>html {/updated}
  ;
: atom_feed_header_generator  ( -- )
  {generator} s" Fendo " echo fendo_version echo {/generator}
  ;
: atom_feed_header_icon  ( -- )
  site_icon $@ {icon/}
  ;

: atom_feed_header  ( -- )
  atom_feed_header_title
  atom_feed_header_subtitle
  atom_feed_header_links
  atom_feed_header_icon
  atom_feed_header_updated
  atom_feed_header_author
  atom_feed_header_id
  atom_feed_header_generator
  ;

: atom_feed{  ( -- )
  s" <feed xmlns='http://www.w3.org/2005/Atom'>" echo_line
  atom_feed_header
  ;

: }atom_feed  ( -- )
  s" </feed>" echo_line
  ;

: (atom{)  ( ca1 len1 ca2 len2 -- )
  \ Create an Atom file.
  open_target
  s" <?xml version='1.0' encoding='utf-8'?>" echo_line
  atom_feed{
  ;

: atom{  ( "text }content" -- )
  \ Start the Atom content, if needed.
  \ The end of the content is marked with the '}atom' markup.
  \ Only one 'atom{ ... }atom' block is allowed in the page.
  do_page? if  .sourcefilename (atom{)  else  skip_page  then
  ;

: }atom  ( -- )
  \ Finish and close the Atom file.
  }atom_feed close_target
  ;

\ **************************************************************
\ Atom entries

: atom_entry_title  ( page_id -- )
  title {atom_title/}
  ;
: atom_entry_id  ( page_id -- )
  pid#>taguri {id/}
  ;
: atom_entry_links  ( page_id -- )
  pid#>url {atom_alternate_link}
  ;
: atom_entry_updated  ( page_id -- )
  modified {updated/}
  ;
: atom_entry_published  ( page_id -- )
  created {published/}
  ;
defer atom_entry_summary
: atom_entry_default_summary  ( page_id -- )
  description {summary/}
  ;
: .atom_entry_comment  ( ca len page_id -- page_id )
  >r echo_line r>
  ;
: atom_entry_custom_summary  ( page_id xt -- )
  \ Create a summary field with a custom content.
  \ xt = xt of the string constant with the custom header to be shown before the actual content
  {atom_xhtml_summary}
  {p}
  execute echo_line  \ custom header
  \ .atom_entry_comment
  \ atom_"content"  echo_line
  description echo_line
  {/p}
  {/atom_xhtml_summary}
  ;
: atom_entry_updated_summary  ( page_id -- )
  ['] atom_"updated_page"  atom_entry_custom_summary
  ;
: atom_entry_new_summary  ( ca len page_id -- )
  ['] atom_"new_page"  atom_entry_custom_summary
  ;
: default_atom_entry_summary  ( -- )
  ['] atom_entry_default_summary is atom_entry_summary
  ;
default_atom_entry_summary
: atom_entry  ( ca len page_id | page_id -- )
  \ Create an Atom entry in the Atom file.
  {entry}
  >r
  r@ atom_entry_title
  r@ atom_entry_id
  r@ atom_entry_links
  r@ atom_entry_published
  r@ atom_entry_updated
  r> atom_entry_summary
  {/entry}
  ;
: atom_updated_entry  ( ca len page_id -- )
  \ Create an Atom entry in the Atom file, about an updated page of the site.
  atom_entry
  default_atom_entry_summary
  ;
: atom_new_entry  ( ca len page_id -- )
  \ Create an Atom entry in the Atom file, about a new page of the site.
  ['] atom_entry_new_summary is atom_entry_summary
  atom_entry
  default_atom_entry_summary
  ;

.( fendo.addon.atom.fs compiled) cr
