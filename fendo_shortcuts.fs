.( fendo_shortcuts.fs ) 

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

\ This file creates the tools for user's shortcuts.

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

\ 2013-10-22 Created with code extracted from <fendo_markup_wiki.fs>
\   and <fendo.fs>. New terminology: every "link" is renamed to
\   "shortcut".
\ 2013-10-23 Fix: stack comments.
\ 2013-10-25 New: debugging version of 'shortcut:'.

wordlist constant fendo_shortcuts_wid  \ for user's shortcuts

: shortcut:  ( "name" -- )
  \ Create an user's shortcut.
  [ true ] [if]  \ xxx normal version
  get-current >r  fendo_shortcuts_wid set-current :  r> set-current
  [else]  \ xxx debugging version
  get-current >r
  fendo_shortcuts_wid set-current
  parse-name
  2dup 2>r nextname
  :
  2r> 
  \ space 2dup type  \ xxx informer
  postpone sliteral postpone cr
  s" resolving shortcut " postpone sliteral
  postpone type
  postpone type
  r> set-current
  [then]
  ; 

0 [if]

\ Usage example of 'shortcut:'
\
\ User's shortcuts are used as recursive href parameters.
\ They can be used to create mnemonics, redirections or
\ "defered" links.

shortcut: gforth
  s" gforth_ext" href=!
  ;
shortcut: gforth_ext
  s" http://www.gnu.org/software/gforth/" href=!
  current_lang# case
    en_language of
      s" Gforth" link_text!
      s" Information and main links on Gforth" title=!
    endof
    eo_language of
      s" en(( Gforth ))" link_text!
      s" en" hreflang=!
      s" Informo kaj ĉefaj ligiloj pri Gforth" title=!
    endof
    es_language of
      s" en(( Gforth ))" link_text!
      s" en" hreflang=!
      s" Información y enlaces principales sobre Gforth" title=!
    endof
  endcase
  ;

[then]

: ((shortcut?))  ( xt1 xt2 1|-1  |  xt1 0  --  xt2 xt2 true  |  false )
  \ xt1 = old xt (former loop)
  \ xt2 = new xt
  if    2dup <> if  nip dup true  else  2drop false  then
  else  drop false  then
  ;
: (shortcut?)  ( xt ca len -- xt xt true  |  false )
  \ Is an href attribute a shortcut different from xt?
  \ ca len = href attribute (not empty)
  fendo_shortcuts_wid search-wordlist ((shortcut?))
  ;
: shortcut?  ( xt ca len -- xt xt true  |  false )
  \ Is an href attribute a shortcut different from xt?
  \ ca len = href attribute (or an empty string)
  dup  if  (shortcut?)  else  nip nip  then
  ;
:noname  ( ca len -- ca len | ca' len' )
  \ Unshortcut an href attribute recursively.
  \ ca len = href attribute 
  \ ca' len' = actual href attribute
  2dup href=!
  0 rot rot  \ fake xt
\  2dup cr ." about to unshortcut " type  \ xxx informer
  begin   ( xt ca len ) shortcut?  ( xt' xt' true  |  false )
  while   execute href=@
\  2dup ." --> " type  \ xxx informer
  repeat  href=@
\  cr  \ xxx informer
  ;  is unshortcut  \ defered in <fendo.fs>
