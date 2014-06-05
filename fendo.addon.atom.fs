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

\ **************************************************************
\ TODO

\ add hreflang to <link>
\ add xml:lang to all human readable fields
\ add <logo>

\ **************************************************************
\ Requirements

\ XXX TODO

\ **************************************************************

svariable atom-file

' create-html alias create-atom
' close-html alias close-atom
' >html alias >atom
' >>html alias >>atom

: {atom-link}  ( ca1 len1 ca2 len2 ca3 len3 -- )
  +rel-attribute 2swap {link}
  ;
: {atom-self-link}  ( ca len -- )
  s" " s" self" {atom-link}
  ;
: {atom-alternate-link}  ( ca len -- )
  iso-lang >hreflang-attribute  s" alternate" {atom-link}
  ;

: +atom-lang-attributes}  ( ca1 len1 c-addr2 c2 -- )
  \ ca1 len1 = xml tag
  \ ca2 len2 = content type (xhtml or html)
  +type-attribute
  iso-lang +xml:lang-attribute >atom
  s" >" >>atom
  ;

: {atom-title}  ( -- )  s" <title" s" html" +atom-lang-attributes}  ;
' {/title} alias {/atom-title}
: {atom-title/}  ( -- )  {atom-title} >>atom {/atom-title}  ;
: {author}  ( -- )  s" <author>" >atom  ;
: {/author}  ( -- )  s" </author>" >atom  ;
: {entry}  ( -- )  s" <entry>" >atom  ;
: {/entry}  ( -- )  s" </entry>" >atom  ;
: {icon}  ( -- )  s" <icon>" >atom  ;
: {/icon}  ( -- )  s" </icon>" >>atom  ;
: {icon/}  ( ca len -- )  {icon} >>atom {/icon}  ;
: {id}  ( -- )  s" <id>" >atom  ;
: {/id}  ( -- )  s" </id>" >>atom  ;
: {id/}  ( ca len -- )  {id} >>atom {/id}  ;
: {logo}  ( -- )  s" <logo>" >atom  ;
: {/logo}  ( -- )  s" </logo>" >>atom  ;
: {logo/}  ( ca len -- )  {logo} >>atom {/logo}  ;
: {name}  ( -- )  s" <name>" >atom  ;
: {/name}  ( -- )  s" </name>" >>atom  ;
: {name/}  ( ca len -- )  {name} >>atom {/name}  ;
: {published}  ( -- )  s" <published>" >atom  ;
: {/published}  ( -- )  s" </published>" >>atom  ;
: {published/}  ( ca len -- )  {published} >>atom {/published}  ;
: {subtitle}  ( -- )  s" <subtitle" s" html" +atom-lang-attributes}  ;
: {/subtitle}  ( -- )  s" </subtitle>" >>atom  ;
: {subtitle/}  ( ca len -- )  {subtitle} >>atom {/subtitle}  ;
: {summary}  ( -- )  s" <summary" s" html" +atom-lang-attributes}  ;
: {/summary}  ( -- )  s" </summary>" >>atom  ;
: {summary/}  ( ca len -- )  {summary} >>atom {/summary}  ;

: {atom-xhtml-summary}  ( -- )
  s" <summary" s" xhtml" +atom-lang-attributes}
  s" <div xmlns='http://www.w3.org/1999/xhtml'>" >atom
  ;

: {/atom-xhtml-summary}  ( -- )  {/div} {/summary}  ;

: {updated}  ( -- )  s" <updated>" >atom  ;
: {/updated}  ( -- )  s" </updated>" >>atom  ;
: {updated/}  ( ca len -- )  {updated} >>atom {/updated}  ;

: atom-feed-header-author  ( -- )  {author} "site-author" {name/} {/author}  ;
: atom-feed-header-id  ( -- )  "site-uri" {id/}  ;
: atom-feed-header-title  ( -- )  "site-title" untag {atom-title/}  ;
: atom-feed-header-subtitle  ( -- )  "site-subtitle" untag {subtitle/}  ;
: atom-feed-header-selflink  ( ca len -- )  atom-file count str+ {atom-self-link}  ;
: atom-feed-header-links  ( -- )  "site-uri" 2dup  {atom-alternate-link}  atom-feed-header-selflink  ;
: atom-feed-header-updated  ( -- )  {updated} time&date iso-date&time>html {/updated}  ;
: atom-feed-header-generator  ( -- )  s" <generator>forthCMS</generator>" >atom  ;
: atom-feed-header-icon  ( -- ) "site-icon" {icon/}  ;

: atom-feed-header
  atom-feed-header-title
  atom-feed-header-subtitle
  atom-feed-header-links
  atom-feed-header-icon
  atom-feed-header-updated
  atom-feed-header-author
  atom-feed-header-id
  atom-feed-header-generator
  ;

: atom-feed{  ( -- )
  s" <feed xmlns='http://www.w3.org/2005/Atom'>" >atom
  atom-feed-header
  ;

: }atom-feed  ( -- )
  s" </feed>" >atom
  ;

: atom{  ( ca1 len1 ca2 len2 -- )
  \ Create an Atom file.
  \ ca1 len1 = encoding
  \ ca2 len2 = file name
  2dup atom-file place  create-atom
  s" <?xml version='1.0' encoding='" >>atom >>atom s" '?>" >>atom
  atom-feed{
  ;

: }atom  ( -- )
  \ Close the Atom file.
  }atom-feed  close-atom
  ;

s" Content" sconstant atom-default-"content"
defer atom-"content"
' atom-default-"content" is atom-"content"

s" <strong>New page<strong>: " sconstant atom-default-"new_page"
defer atom-"new_page"
' atom-default-"new_page" is atom-"new_page"

s" <strong>Updated page</strong>: " sconstant atom-default-"updated_page"
defer atom-"updated_page"
' atom-default-"updated_page" is atom-"updated_page"

: atom-entry-title  ( page-id -- )  >page-title {atom-title/}  ;
: atom-entry-id  ( page-id -- )  >page-taguri {id/}  ;
: atom-entry-links  ( page-id -- )  >page-url {atom-alternate-link}  ;
: atom-entry-updated  ( page-id -- )  >page-change >iso-full-date {updated/}  ;
: atom-entry-published  ( page-id -- )  >page-created >iso-full-date {published/}  ;
defer atom-entry-summary
: atom-entry-default-summary  ( page-id -- )  >page-description {summary/}  ;

: .atom-entry-comment  ( ca len page-id -- page-id )  >r >atom r>  ;

: atom-entry-custom-summary  ( page-id xt -- )
  \ Create a summary field with a custom content.
  \ xt = xt of the string constant with the custom header to be shown before the actual content
  {atom-xhtml-summary}
  {p}
  execute >atom  \ custom header
  \ .atom-entry-comment
  \ atom-"content"  >atom
  >page-description >atom
  {/p}
  {/atom-xhtml-summary}
  ;

: atom-entry-updated-summary  ( page-id -- )
  ['] atom-"updated_page"  atom-entry-custom-summary
  ;

: atom-entry-new-summary  ( ca len page-id -- )
  ['] atom-"new_page"  atom-entry-custom-summary
  ;

: default-atom-entry-summary  ( -- )  ['] atom-entry-default-summary is atom-entry-summary ;
default-atom-entry-summary

: atom-entry  ( ca len page-id | page-id -- )
  \ Create an Atom entry in the Atom file.
  {entry}
  >r
  r@ atom-entry-title
  r@ atom-entry-id
  r@ atom-entry-links
  r@ atom-entry-published
  r@ atom-entry-updated
  r> atom-entry-summary
  {/entry}
  ;

: atom-updated-entry  ( ca len page-id -- )
  \ Create an Atom entry in the Atom file, about an updated page of the site.
  atom-entry
  default-atom-entry-summary
  ;

: atom-new-entry  ( ca len page-id -- )
  \ Create an Atom entry in the Atom file, about a new page of the site.
  ['] atom-entry-new-summary is atom-entry-summary
  atom-entry
  default-atom-entry-summary
  ;

.( fendo.addon.atom.fs compiled) cr
