( fendo.addon.tag_section_by_prefix.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file provides an addon to create a section containing a heading
\ and a list of tagged pages in the current language.

\ Last modified 202007062136.
\ See change log at the end of the file.

\ Copyright (C) 2020 Marcos Cruz (programandala.net)

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
\ Requirements

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`

fendo_definitions

require ./fendo.addon.dtddoc.fs
require ./fendo.addon.tags.fs
require ./fendo.addon.traverse_pids.fs

\ ==============================================================

package fendo.addon.tagged_pages_by_prefix

variable prefix$

: flags>or ( f1 ... fn n -- f )
  1- 0 ?do or loop ;

: ((tagged_pages_by_prefix)) {: D: pid -- :}

  \ Do nothing if length is 0.
  \ XXX FIXME
  \ This check seems to be needed, because temporary shortcuts created by the
  \ application could return an empty string, what would make `pid$>pid#` crash.
  \ But it's not clear yet.
\  pid nip 0= ?exit

  pid prefix$ $@ string-prefix? 0= ?exit
  pid pid$>pid#
  dup draft? if drop exit then
  tags evaluate_tags tag_presence @
  if pid dtddoc tag_presence off then ;
  \ Create a description list of content
  \ if the given page ID starts with the current prefix.

: (tagged_pages_by_prefix) ( ca len -- true )
  ((tagged_pages_by_prefix)) true ;
  \ ca len = page ID
  \ true = continue with the next element?

public

: tag_section_by_prefix ( ca1 len1 ca2 len2 -- )
  prefix$ $!  tags_do_presence
  [<dl>] ['] (tagged_pages_by_prefix) traverse_pids [</dl>] ;

  \ doc{
  \
  \ tag_section_by_prefix ( ca1 len1 ca2 len2 -- )
  \
  \ Create a tag section (without a heading) containing a definition
  \ list of pages that contain tag _ca1 len1_ and having prefix _ca2
  \ len2_.
  \
  \ See: `tag_section`, `tag_section_heading`, `tags_do_presence`,
  \ `[<dl>]`, `(tagged_pages_by_prefix)`, `traverse_pids`.
  \
  \ }doc

end-package

.( fendo.addon.tag_section.fs compiled) cr

\ ==============================================================
\ Change log

\ 2020-04-25: Start.
\
\ 2020-07-06: Document the public words.

\ vim: filetype=gforth

