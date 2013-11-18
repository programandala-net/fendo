.( fendo_tools.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

\ This file provides some application-level tools.

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

\ 2013-11-11 Code extracted from <fendo_markup_wiki.fs>: 'link'.

\ **************************************************************
\ Links

: (link)  ( ca len -- )
  \ Create a link.
  \ Its attributes and link text have to be set previously.
  \ ca len = page id, URL or shortcut
  (get_link_href) tune_link echo_link
  ;
: link  ( ca1 len1 ca2 len2 -- )
  \ Create a link of any type.
  \ Its attributes have to be set previously.
  \ ca1 len1 = page id, URL or shortcut
  \ ca2 len2 = link text
  link_text! (link) 
  ;
: title_link  ( ca len -- )
  \ Create a link to a local page, using the page title as text link
  \ (unless the link text has been explicitily set).
  \ Its attributes have to be set previously.
  \ If 'link_text' is not set, the page title will be used.
  \ ca len = page id or shortcut to it
  \ xxx todo make it work with anchors!?
  2dup data<id$>id title link_text?! (link)
  ;