.( addons/ql_charset.fs) cr

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

\ 2013-12-10 Written.

\ **************************************************************

require galope/translated.fs

false [if]  \ first version

\ The HTML entities cause troubles with syntax highlighting

translations: ql_charset
  \ Translation table for the Sinclair QL charset.
  \ Reference:
  \ http://pages.zoom.co.uk/selsyn/ql/qlchar.html
  s\" \x22" s" &quot;"  \ double quotes
  s\" \x60" s" &#163;"  \ British pound sterling
  s\" \x7F" s" &#169;"  \ copyright
  s\" \x7E" s" &tilde;"  \ '~'
  s\" \x80" s" &#228;"  \ a umlaut
  s\" \x81" s" &#227;"  \ a tilde
  s\" \x82" s" &#229;"  \ a ring
  s\" \x83" s" &#233;"  \ e acute
  s\" \x84" s" &#246;"  \ o umlaut
  s\" \x85" s" &#245;"  \ o tilde 
  s\" \x86" s" &#248;"  \ o slash
  s\" \x87" s" &#252;"  \ u umlaut
  s\" \x88" s" &#231;"  \ c cedilla
  s\" \x89" s" &#241;"  \ n tilde
  s\" \x8A" s" &#230;"  \ ae ligature (ash)
  s\" \x8B" s" &#339;"  \ oe ligature (ethel)
  s\" \x8C" s" &#225;"  \ a acute
  s\" \x8D" s" &#224;"  \ a grave
  s\" \x8E" s" &#226;"  \ a circumflex
  s\" \x8F" s" &#235;"  \ e umlaut
  s\" \x90" s" &#232;"  \ e grave
  s\" \x91" s" &#234;"  \ e circumflex
  s\" \x92" s" &#239;"  \ i umlaut
  s\" \x93" s" &#237;"  \ i acute
  s\" \x94" s" &#236;"  \ i grave
  s\" \x95" s" &#238;"  \ i circumflex
  s\" \x96" s" &#243;"  \ o acute
  s\" \x97" s" &#242;"  \ o grave
  s\" \x98" s" &#244;"  \ o circumflex
  s\" \x99" s" &#250;"  \ u acute
  s\" \x9A" s" &#249;"  \ u grave
  s\" \x9B" s" &#251;"  \ u circumflex
  s\" \x9C" s" &#223;"  \ Greek beta / eszett
  s\" \x9D" s" &#162;"  \ cent
  s\" \x9E" s" &#165;"  \ yen
  s\" \x9F" s" `"  \ backtick
  s\" \xA0" s" &#196;"  \ A umlaut
  s\" \xA1" s" &#195;"  \ A tilde
  s\" \xA2" s" &#197;"  \ A ring
  s\" \xA3" s" &#201;"  \ E acute
  s\" \xA4" s" &#214;"  \ O umlaut
  s\" \xA5" s" &#213;"  \ O tilde
  s\" \xA6" s" &#216;"  \ O slash
  s\" \xA7" s" &#220;"  \ U umlaut
  s\" \xA8" s" &#199;"  \ C cedilla
  s\" \xA9" s" &#209;"  \ N tilde
  s\" \xAA" s" &#198;"  \ AE ligature
  s\" \xAB" s" &#338;"  \ OE ligature
  s\" \xAC" s" &#945;"  \ Greek alpha
  s\" \xAD" s" &#948;"  \ Greek delta
  s\" \xAE" s" &#952;"  \ Greek theta
  s\" \xAF" s" &#955;"  \ Greek lambda
  s\" \xB0" s" &#956;"  \ Greek mu
  s\" \xB1" s" &#960;"  \ Greek pi
  s\" \xB2" s" &#967;"  \ Greek phi
  s\" \xB3" s" &#161;"  \ inverted !
  s\" \xB4" s" &#191;"  \ inverted ?
  \ s\" \xB5" s" "  \ reverse S underdot ??? \ xxx todo
  s\" \xB6" s" &#167;"  \ section
  s\" \xB7" s" &#164;"  \ generic currency
  s\" \xB8" s" &#171;"  \ left angle quote
  s\" \xB9" s" &#187;"  \ right angle quote
  s\" \xBA" s" &#176;"  \ ring or deegre
  s\" \xBB" s" &#247;"  \ divide sign
  s\" \xBC" s" &#8592;"  \ arrow left
  s\" \xBD" s" &#8594;"  \ arrow right
  s\" \xBE" s" &#8593;"  \ arrow up
  s\" \xBF" s" &#8595;"  \ arrow down
  ;translations

[else]
 
translations: ql_charset
  \ Translation table for the Sinclair QL charset.
  \ Reference:
  \ http://pages.zoom.co.uk/selsyn/ql/qlchar.html
  s\" \x60" s" £"  \ British pound sterling
  s\" \x7F" s" ©"  \ copyright
  s\" \x7E" s" ~"  \ tilde
  s\" \x80" s" ä"  \ a umlaut
  s\" \x81" s" ã"  \ a tilde
  s\" \x82" s" å"  \ a ring
  s\" \x83" s" é"  \ e acute
  s\" \x84" s" ö"  \ o umlaut
  s\" \x85" s" õ"  \ o tilde 
  s\" \x86" s" ø"  \ o slash
  s\" \x87" s" ü"  \ u umlaut
  s\" \x88" s" ç"  \ c cedilla
  s\" \x89" s" ñ"  \ n tilde
  s\" \x8A" s" æ"  \ ae ligature (ash)
  s\" \x8B" s" œ"  \ oe ligature (ethel)
  s\" \x8C" s" á"  \ a acute
  s\" \x8D" s" à"  \ a grave
  s\" \x8E" s" â"  \ a circumflex
  s\" \x8F" s" ë"  \ e umlaut
  s\" \x90" s" è"  \ e grave
  s\" \x91" s" ê"  \ e circumflex
  s\" \x92" s" ï"  \ i umlaut
  s\" \x93" s" í"  \ i acute
  s\" \x94" s" ì"  \ i grave
  s\" \x95" s" î"  \ i circumflex
  s\" \x96" s" ó"  \ o acute
  s\" \x97" s" ò"  \ o grave
  s\" \x98" s" ô"  \ o circumflex
  s\" \x99" s" ú"  \ u acute
  s\" \x9A" s" ù"  \ u grave
  s\" \x9B" s" û"  \ u circumflex
  s\" \x9C" s" ß"  \ Greek beta / eszett
  s\" \x9D" s" ¢"  \ cent
  s\" \x9E" s" ¥"  \ yen
  s\" \x9F" s" `"  \ backtick
  s\" \xA0" s" Ä"  \ A umlaut
  s\" \xA1" s" Ã"  \ A tilde
  s\" \xA2" s" Å"  \ A ring
  s\" \xA3" s" É"  \ E acute
  s\" \xA4" s" Ö"  \ O umlaut
  s\" \xA5" s" Õ"  \ O tilde
  s\" \xA6" s" Ø"  \ O slash
  s\" \xA7" s" Ü"  \ U umlaut
  s\" \xA8" s" Ç"  \ C cedilla
  s\" \xA9" s" Ñ"  \ N tilde
  s\" \xAA" s" Æ"  \ AE ligature
  s\" \xAB" s" Œ"  \ OE ligature
  s\" \xAC" s" α"  \ Greek alpha
  s\" \xAD" s" δ"  \ Greek delta
  s\" \xAE" s" θ"  \ Greek theta
  s\" \xAF" s" λ"  \ Greek lambda
  s\" \xB0" s" μ"  \ Greek mu
  s\" \xB1" s" π"  \ Greek pi
  s\" \xB2" s" χ"  \ Greek phi
  s\" \xB3" s" ¡"  \ inverted !
  s\" \xB4" s" ¿"  \ inverted ?
  \ s\" \xB5" s" "  \ reverse S underdot ??? \ xxx todo
  s\" \xB6" s" §"  \ section
  s\" \xB7" s" ¤"  \ generic currency
  s\" \xB8" s" «"  \ left angle quote
  s\" \xB9" s" »"  \ right angle quote
  s\" \xBA" s" °"  \ ring or deegre
  s\" \xBB" s" ÷"  \ divide sign
  s\" \xBC" s" ←"  \ arrow left
  s\" \xBD" s" →"  \ arrow right
  s\" \xBE" s" ↑"  \ arrow up
  s\" \xBF" s" ↓"  \ arrow down
  ;translations

[then]
