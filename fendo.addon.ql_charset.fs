.( fendo.addon.ql_charset.fs) cr

\ This file is part of Fendo.

\ This file is the Sinclair QL source code addon.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ 2013-12-10: Written with <galope/translated.fs>.
\ 2013-12-12: Rewritten with <galope/uncodepaged.fs>.
\ 2014-10-24: Solution for the QL char 0xB5: Unicode chars U+01A8 and U+0323 combined.
\ 2014-10-24: Reference URL no longer exists; updated to a copy in web.archive.org.

\ **************************************************************

forth_definitions
require galope/uncodepaged.fs
fendo_definitions

\ Reference:
\ http://web.archive.org/web/20080907083614/http://pages.zoom.co.uk/selsyn/ql/qlchar.html

uncodepage: ql_charset_to_html
  \ Translation table from the Sinclair QL charset
  \ to HTML entities.
\  0x22 s" &quot;"  \ double quotes
  0x60 s" &#163;"  \ British pound sterling
  0x7F s" &copy;"  \ copyright
  0x7E s" &tilde;"  \ '~'
  0x80 s" &#228;"  \ a umlaut
  0x81 s" &#227;"  \ a tilde
  0x82 s" &#229;"  \ a ring
  0x83 s" &#233;"  \ e acute
  0x84 s" &#246;"  \ o umlaut
  0x85 s" &#245;"  \ o tilde
  0x86 s" &#248;"  \ o slash
  0x87 s" &#252;"  \ u umlaut
  0x88 s" &#231;"  \ c cedilla
  0x89 s" &#241;"  \ n tilde
  0x8A s" &#230;"  \ ae ligature (ash)
  0x8B s" &#339;"  \ oe ligature (ethel)
  0x8C s" &#225;"  \ a acute
  0x8D s" &#224;"  \ a grave
  0x8E s" &#226;"  \ a circumflex
  0x8F s" &#235;"  \ e umlaut
  0x90 s" &#232;"  \ e grave
  0x91 s" &#234;"  \ e circumflex
  0x92 s" &#239;"  \ i umlaut
  0x93 s" &#237;"  \ i acute
  0x94 s" &#236;"  \ i grave
  0x95 s" &#238;"  \ i circumflex
  0x96 s" &#243;"  \ o acute
  0x97 s" &#242;"  \ o grave
  0x98 s" &#244;"  \ o circumflex
  0x99 s" &#250;"  \ u acute
  0x9A s" &#249;"  \ u grave
  0x9B s" &#251;"  \ u circumflex
  0x9C s" &#223;"  \ Greek beta / eszett
  0x9D s" &#162;"  \ cent
  0x9E s" &#165;"  \ yen
  0x9F s" &#96;"  \ backtick
  0xA0 s" &#196;"  \ A umlaut
  0xA1 s" &#195;"  \ A tilde
  0xA2 s" &#197;"  \ A ring
  0xA3 s" &#201;"  \ E acute
  0xA4 s" &#214;"  \ O umlaut
  0xA5 s" &#213;"  \ O tilde
  0xA6 s" &#216;"  \ O slash
  0xA7 s" &#220;"  \ U umlaut
  0xA8 s" &#199;"  \ C cedilla
  0xA9 s" &#209;"  \ N tilde
  0xAA s" &#198;"  \ AE ligature
  0xAB s" &#338;"  \ OE ligature
  0xAC s" &#945;"  \ Greek alpha
  0xAD s" &#948;"  \ Greek delta
  0xAE s" &#952;"  \ Greek theta
  0xAF s" &#955;"  \ Greek lambda
  0xB0 s" &#956;"  \ Greek mu
  0xB1 s" &#960;"  \ Greek pi
  0xB2 s" &#967;"  \ Greek phi
  0xB3 s" &#161;"  \ inverted !
  0xB4 s" &#191;"  \ inverted ?
  0xB5 s" &#424;&#803;"  \ reversed S with dot below
  0xB6 s" &#167;"  \ section
  0xB7 s" &#164;"  \ generic currency
  0xB8 s" &#171;"  \ left angle quote
  0xB9 s" &#187;"  \ right angle quote
  0xBA s" &#176;"  \ ring or deegre
  0xBB s" &#247;"  \ divide sign
  0xBC s" &#8592;"  \ arrow left
  0xBD s" &#8594;"  \ arrow right
  0xBE s" &#8593;"  \ arrow up
  0xBF s" &#8595;"  \ arrow down
  ;uncodepage

uncodepage: ql_charset_to_utf8
  \ Translation table from the Sinclair QL charset
  \ to UTF-8.
  0x60 s" £"  \ British pound sterling
  0x7F s" ©"  \ copyright
  0x7E s" ~"  \ tilde
  0x80 s" ä"  \ a umlaut
  0x81 s" ã"  \ a tilde
  0x82 s" å"  \ a ring
  0x83 s" é"  \ e acute
  0x84 s" ö"  \ o umlaut
  0x85 s" õ"  \ o tilde
  0x86 s" ø"  \ o slash
  0x87 s" ü"  \ u umlaut
  0x88 s" ç"  \ c cedilla
  0x89 s" ñ"  \ n tilde
  0x8A s" æ"  \ ae ligature (ash)
  0x8B s" œ"  \ oe ligature (ethel)
  0x8C s" á"  \ a acute
  0x8D s" à"  \ a grave
  0x8E s" â"  \ a circumflex
  0x8F s" ë"  \ e umlaut
  0x90 s" è"  \ e grave
  0x91 s" ê"  \ e circumflex
  0x92 s" ï"  \ i umlaut
  0x93 s" í"  \ i acute
  0x94 s" ì"  \ i grave
  0x95 s" î"  \ i circumflex
  0x96 s" ó"  \ o acute
  0x97 s" ò"  \ o grave
  0x98 s" ô"  \ o circumflex
  0x99 s" ú"  \ u acute
  0x9A s" ù"  \ u grave
  0x9B s" û"  \ u circumflex
  0x9C s" ß"  \ Greek beta / eszett
  0x9D s" ¢"  \ cent
  0x9E s" ¥"  \ yen
  0x9F s" `"  \ backtick
  0xA0 s" Ä"  \ A umlaut
  0xA1 s" Ã"  \ A tilde
  0xA2 s" Å"  \ A ring
  0xA3 s" É"  \ E acute
  0xA4 s" Ö"  \ O umlaut
  0xA5 s" Õ"  \ O tilde
  0xA6 s" Ø"  \ O slash
  0xA7 s" Ü"  \ U umlaut
  0xA8 s" Ç"  \ C cedilla
  0xA9 s" Ñ"  \ N tilde
  0xAA s" Æ"  \ AE ligature
  0xAB s" Œ"  \ OE ligature
  0xAC s" α"  \ Greek alpha
  0xAD s" δ"  \ Greek delta
  0xAE s" θ"  \ Greek theta
  0xAF s" λ"  \ Greek lambda
  0xB0 s" μ"  \ Greek mu
  0xB1 s" π"  \ Greek pi
  0xB2 s" χ"  \ Greek phi
  0xB3 s" ¡"  \ inverted !
  0xB4 s" ¿"  \ inverted ?
  0xB5 s" ƨ̣"  \ reversed S with dot below (U+01A8 and U+0323 combined) \ XXX Note: this doesn't look right with Vimprobable, but yes with Midori.
  0xB6 s" §"  \ section
  0xB7 s" ¤"  \ generic currency
  0xB8 s" «"  \ left angle quote
  0xB9 s" »"  \ right angle quote
  0xBA s" °"  \ ring or deegre
  0xBB s" ÷"  \ divide sign
  0xBC s" ←"  \ arrow left
  0xBD s" →"  \ arrow right
  0xBE s" ↑"  \ arrow up
  0xBF s" ↓"  \ arrow down
  ;uncodepage

.( fendo.addon.ql_charset.fs compiled) cr
