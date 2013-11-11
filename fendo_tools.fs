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
  \ Create a link. All attributes are already set.
  \ ca len = text link, to be evaluated as content
  [<a>] evaluate_content [</a>]
  ;
: local_link  ( a ca len -- )
  \ Create a link to a local page.
  \ a = page id
  \ ca len = text link, to be evaluated as content
  \ xxx todo hreflang
  rot dup plain_description title=!
  dup target_file href=!
  access_key accesskey=!
  (link)
  ;
: link  ( a ca len -- )
  \ Create a link to a local page.
  \ a = page id
  \ ca len = text link, to be evaluated as content
  \ xxx todo hreflang
  rot dup plain_description title=!
  dup target_file href=!
  access_key accesskey=!
  (link)
  ;

