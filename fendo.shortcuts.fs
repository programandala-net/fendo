.( fendo.shortcuts.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file creates the tools for user's shortcuts.

\ Last modified 201812080157.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth).

\ ==============================================================

wordlist constant fendo_shortcuts_wid  \ for user's shortcuts

: shortcut: ( "name" -- )
  [ true ] [if]
    \ XXX normal version; it works fine
    get-current >r  fendo_shortcuts_wid set-current :
    r> set-current
  [else]
    \ XXX TMP -- debugging version
    get-current >r
    fendo_shortcuts_wid set-current
    parse-name
    2dup 2>r nextname :
    2r>
\    space 2dup type  \ XXX INFORMER
    postpone sliteral postpone cr
    s" resolving shortcut " postpone sliteral
    postpone type
    postpone type
    r> set-current
  [then] ;
  \ Create an user's shortcut.

false [if]

: :shortcut ( ca len -- )
  nextname shortcut: ;
  \ Create an user's shortcut.

\ XXX OLD
\ 2014-11-27: Useless try. The approach is wrong.

s" " :shortcut ( -- )
  current_pid$ href=! ;
  \ Create the default shortcut to the current page.
  \ This way links to anchors in the current page are possible.

[then]

0 [if]

\ Usage example of `shortcut:`
\
\ User's shortcuts are used as recursive href parameters.
\ They can be used to create mnemonics, redirections or
\ "defered" links.

shortcut: gforth
  s" gforth_ext" href=! ;

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
  endcase ;

[then]

: ((shortcut?)) ( xt1 xt2 1|-1  |  xt1 0  --  xt2 xt2 true  |  false )
\  if    2dup <> dup >r if  nip dup  else  2drop  then  r>  \ XXX OLD -- alternative
  if    2dup <> if  nip dup true  else  2drop false  then
  else  drop false  then ;
  \ xt1 = old xt (former loop)
  \ xt2 = new xt

: (shortcut?) ( xt ca len -- xt xt true  |  false )
\  ." Parameter in `(shortcut?)` = " 2dup type .s cr  \ XXX INFORMER
  fendo_shortcuts_wid search-wordlist ((shortcut?)) ;
  \ Is an href attribute a shortcut different from xt?
  \ ca len = href attribute (not empty)

: shortcut? ( xt ca len -- xt xt true  |  false )
\  ." Parameter in `shortcut?` = " 2dup type .s cr  \ XXX INFORMER
  dup  if  (shortcut?)  else  nip nip  then ;
  \ Is an href attribute a shortcut different from xt?
  \ ca len = href attribute (or an empty string)

:noname ( ca len -- ca len | ca' len' )
\  cr ." order = " order cr ." entering unshortcut " 2dup type  \ XXX INFORMER
\  ." Parameter in `unshortcut` = " 2dup type .s cr  \ XXX INFORMER
  2dup href=!
  0 rot rot  \ fake xt
\  2dup cr ." order = " order cr ." about to unshortcut " type  \ XXX INFORMER
  begin  ( xt ca len ) shortcut? ( xt' xt' true  |  false )
  while   execute href=@
\  ." --> " 2dup type  \ XXX INFORMER
  repeat  href=@
\  ." Result of `unshortcut` = " 2dup type .s cr  \ XXX INFORMER
\ save-mem  \ XXX TMP -- needed to preserve the actual zone of `href=`? No.
\  .s cr  \ XXX INFORMER
\  cr  \ XXX INFORMER
  ; is unshortcut  \ defered in <fendo.fs>
  \ Unshortcut an href attribute recursively.
  \ ca len = href attribute
  \ ca' len' = actual href attribute

:noname ( ca len -- ca len | ca' len' )
\  ." Parameter in `just_unshortcut` = " 2dup type .s cr  \ XXX INFORMER
  save_attributes
  unshortcut save-mem 2>r
\  ." `href=` in `just_unshortcut` before `restore_attributes` = " s" href=@" evaluate .s cr type cr  \ XXX INFORMER
  restore_attributes
  2r> 2dup href=!
\  ." `href=` in `just_unshortcut` after `restore_attributes` = " s" href=@" evaluate .s cr type cr  \ XXX INFORMER
  ; is just_unshortcut  \ defered in <fendo.fs>
  \ Unshortcut an href attribute recursively,
  \ but preserving all other attributes.
  \ ca len = href attribute
  \ ca' len' = actual href attribute

:noname ( ca len -- ca len | ca' len' ) \ XXX TMP -- for debugging
\  ." `href=` in `dry_unshortcut` before `save_attributes`    = " s" href=@" evaluate .s space type cr  \ XXX INFORMER
  save_attributes
\  ." `href=` in `dry_unshortcut` after `save_attributes`     = " s" href=@" evaluate .s space type cr  \ XXX INFORMER
  unshortcut 
\  ." TOS in `dry_unshortcut` after `unshortcut`              = " .s space 2dup type cr  \ XXX INFORMER
  save-mem 2>r
\  ." `href=` in `dry_unshortcut` before `restore_attributes` = " s" href=@" evaluate .s space type cr  \ XXX INFORMER
  restore_attributes
\  ." `href=` in `dry_unshortcut` after `restore_attributes`  = " s" href=@" evaluate .s space type cr  \ XXX INFORMER
  2r> 
  ; is dry_unshortcut  \ defered in <fendo.fs>
  \ Unshortcut an href attribute recursively,
  \ without modifing any attribute.
  \ ca len = href attribute
  \ ca' len' = actual href attribute

.( fendo.shortcuts.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2013-10-22: Created with code extracted from <fendo_markup_wiki.fs>
\ and <fendo.fs>. New terminology: every "link" is renamed to
\ "shortcut".
\
\ 2013-10-23: Fix: stack comments.
\
\ 2013-10-25: New: debugging version of `shortcut:`.
\
\ 2014-11-11: Fix: `just_unshortcut` now preserves the `href=`
\ attribute with `save-mem`. The problem started because of the use of
\ `save_attributes` and `restore_attributes`.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
