.( addons/cp850_charset.fs) cr

\ This file is part of Fendo.

\ This file is the Sinclair cp850 source code addon.

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

require ftrac/ftrac.fs

\ References:
\ http://en.wikipedia.org/wiki/Code_page_850
\ http://en.wikipedia.org/wiki/Code_page_437#Interpretation_of_code_points_1.E2.80.9331_and_127

trac-array: cp850_charset_to_html

  \ Translation table from the CP850 charset to HTML entities.
 
  0x01 s" &#9786;" trac!
  0x02 s" &#9787;" trac!
  0x03 s" &#9829;" trac!
  0x04 s" &#9830;" trac!
  0x05 s" &#9827;" trac!
  0x06 s" &#9824;" trac!
  0x07 s" &#8226;" trac!
  0x08 s" &#9688;" trac!
\  0x09 s" &#9675;" trac!
\  0x0A s" &#9689;" trac!
  0x0B s" &#9794;" trac!
\  0x0C s" &#9792;" trac!
  0x0D s" &#9834;" trac!
  0x0E s" &#9835;" trac!
  0x0F s" &#9788;" trac!
  0x10 s" &#9658;" trac!
  0x11 s" &#9668;" trac!
  0x12 s" &#8597;" trac!
  0x13 s" &#8252;" trac!
  0x14 s" &#182;" trac!
  0x15 s" &#167;" trac!
  0x16 s" &#9644;" trac!
  0x17 s" &#8616;" trac!
  0x18 s" &#8593;" trac!
  0x19 s" &#8595;" trac!
  0x1A s" &#8594;" trac!
  0x1B s" &#8592;" trac!
  0x1C s" &#8735;" trac!
  0x1D s" &#8596;" trac!
  0x1E s" &#9650;" trac!
  0x1F s" &#9653;" trac!
  0x22 s" &quot;" trac!
  0x7F s" &#8962;" trac!
  0x80 s" &#199;" trac!
  0x81 s" &#252;" trac!
  0x82 s" &#233;" trac!
  0x83 s" &#226;" trac!
  0x84 s" &#228;" trac!
  0x85 s" &#224;" trac!
  0x86 s" &#229;" trac!
  0x87 s" &#231;" trac!
  0x88 s" &#234;" trac!
  0x89 s" &#235;" trac!
  0x8A s" &#232;" trac!
  0x8B s" &#239;" trac!
  0x8C s" &#238;" trac!
  0x8D s" &#236;" trac!
  0x8E s" &#196;" trac!
  0x8F s" &#197;" trac!
  0x90 s" &#201;" trac!
  0x91 s" &#230;" trac!
  0x92 s" &#198;" trac!
  0x93 s" &#244;" trac!
  0x94 s" &#246;" trac!
  0x95 s" &#242;" trac!
  0x96 s" &#251;" trac!
  0x97 s" &#249;" trac!
  0x98 s" &#255;" trac!
  0x99 s" &#214;" trac!
  0x9A s" &#220;" trac!
  0x9B s" &#248;" trac!
  0x9C s" &#163;" trac!
  0x9D s" &#216;" trac!
  0x9E s" &#215;" trac!
  0x9F s" &#402;" trac!
  0xA0 s" &#225;" trac!
  0xA1 s" &#237;" trac!
  0xA2 s" &#243;" trac!
  0xA3 s" &#250;" trac!
  0xA4 s" &#241;" trac!
  0xA5 s" &#209;" trac!
  0xA6 s" &#170;" trac!
  0xA7 s" &#186;" trac!
  0xA8 s" &#191;" trac!
  0xA9 s" &#174;" trac!
  0xAA s" &#172;" trac!
  0xAB s" &#189;" trac!
  0xAC s" &#188;" trac!
  0xAD s" &#161;" trac!
  0xAE s" &#171;" trac!
  0xAF s" &#187;" trac!
  0xB0 s" &#9617;" trac!
  0xB1 s" &#10642;" trac!
  0xB2 s" &#9619;" trac!
  0xB3 s" &#9474;" trac!
  0xB4 s" &#9508;" trac!
  0xB5 s" &#193;" trac!
  0xB6 s" &#194;" trac!
  0xB7 s" &#192;" trac!
  0xB8 s" &#169;" trac!
  0xB9 s" &#9571;" trac!
  0xBA s" &#9553;" trac!
  0xBB s" &#9559;" trac!
  0xBC s" &#9565;" trac!
  0xBD s" &#162;" trac!
  0xBE s" &#165;" trac!
  0xBF s" &#9488;" trac!
  0xC0 s" &#9492;" trac!
  0xC1 s" &#9524;" trac!
  0xC2 s" &#9516;" trac!
  0xC3 s" &#9500;" trac!
  0xC4 s" &#9472;" trac!
  0xC5 s" &#9532;" trac!
  0xC6 s" &#227;" trac!
  0xC7 s" &#195;" trac!
  0xC8 s" &#9562;" trac!
  0xC9 s" &#9556;" trac!
  0xCA s" &#9577;" trac!
  0xCB s" &#9574;" trac!
  0xCC s" &#9568;" trac!
  0xCD s" &#9552;" trac!
  0xCE s" &#9580;" trac!
  0xCF s" &#164;" trac!
  0xD0 s" &#240;" trac!
  0xD1 s" &#208;" trac!
  0xD2 s" &#202;" trac!
  0xD3 s" &#203;" trac!
  0xD4 s" &#200;" trac!
  0xD5 s" &#305;" trac!
  0xD6 s" &#205;" trac!
  0xD7 s" &#206;" trac!
  0xD8 s" &#207;" trac!
  0xD9 s" &#9496;" trac!
  0xDA s" &#9484;" trac!
  0xDB s" &#9608;" trac!
  0xDC s" &#9604;" trac!
  0xDD s" &#166;" trac!
  0xDE s" &#204;" trac!
  0xDF s" &#9600;" trac!
  0xE0 s" &#211;" trac!
  0xE1 s" &#223;" trac!
  0xE2 s" &#212;" trac!
  0xE3 s" &#210;" trac!
  0xE4 s" &#245;" trac!
  0xE5 s" &#213;" trac!
  0xE6 s" &#181;" trac!
  0xE7 s" &#254;" trac!
  0xE8 s" &#222;" trac!
  0xE9 s" &#218;" trac!
  0xEA s" &#219;" trac!
  0xEB s" &#217;" trac!
  0xEC s" &#253;" trac!
  0xED s" &#221;" trac!
  0xEE s" &#175;" trac!
  0xEF s" &#180;" trac!
  0xF0 s" &#173;" trac!
  0xF1 s" &#177;" trac!
  0xF2 s" &#8215;" trac!
  0xF3 s" &#190;" trac!
  0xF4 s" &#182;" trac!
  0xF5 s" &#167;" trac!
  0xF6 s" &#247;" trac!
  0xF7 s" &#184;" trac!
  0xF8 s" &#176;" trac!
  0xF9 s" &#168;" trac!
  0xFA s" &#183;" trac!
  0xFB s" &#185;" trac!
  0xFC s" &#179;" trac!
  0xFD s" &#178;" trac!
  0xFE s" &#9632;" trac!
  0xFF s" &#160;" trac!

trac-array: cp850_charset_to_utf8

  \ Translation table from the CP850 charset to UTF-8.

  0x01 s" ☺" trac!
  0x02 s" ☻" trac!
  0x03 s" ♥" trac!
  0x04 s" ♦" trac!
  0x05 s" ♣" trac!
  0x06 s" ♠" trac!
  0x07 s" •" trac!
  0x08 s" ◘" trac!
\  0x09 s" ○" trac!
\  0x0A s" ◙" trac!
  0x0B s" ♂" trac!
  0x0C s" ♀" trac!
\  0x0D s" ♪" trac!
  0x0E s" ♫" trac!
  0x0F s" ☼" trac!
  0x10 s" ►" trac!
  0x11 s" ◄" trac!
  0x12 s" ↕" trac!
  0x13 s" ‼" trac!
  0x14 s" ¶" trac!
  0x15 s" §" trac!
  0x16 s" ▬" trac!
  0x17 s" ↨" trac!
  0x18 s" ↑" trac!
  0x19 s" ↓" trac!
  0x1A s" →" trac!
  0x1B s" ←" trac!
  0x1C s" ∟" trac!
  0x1D s" ↔" trac!
  0x1E s" ▲" trac!
  0x1F s" ▼" trac!
  0x7F s" ⌂" trac!
  0x80 s" Ç" trac!
  0x81 s" ü" trac!
  0x82 s" é" trac!
  0x83 s" â" trac!
  0x84 s" ä" trac!
  0x85 s" à" trac!
  0x86 s" å" trac!
  0x87 s" ç" trac!
  0x88 s" ê" trac!
  0x89 s" ë" trac!
  0x8A s" è" trac!
  0x8B s" ï" trac!
  0x8C s" î" trac!
  0x8D s" ì" trac!
  0x8E s" Ä" trac!
  0x8F s" Å" trac!
  0x90 s" É" trac!
  0x91 s" æ" trac!
  0x92 s" Æ" trac!
  0x93 s" ô" trac!
  0x94 s" ö" trac!
  0x95 s" ò" trac!
  0x96 s" û" trac!
  0x97 s" ù" trac!
  0x98 s" ÿ" trac!
  0x99 s" Ö" trac!
  0x9A s" Ü" trac!
  0x9B s" ø" trac!
  0x9C s" £" trac!
  0x9D s" Ø" trac!
  0x9E s" ×" trac!
  0x9F s" ƒ" trac!
  0xA0 s" á" trac!
  0xA1 s" í" trac!
  0xA2 s" ó" trac!
  0xA3 s" ú" trac!
  0xA4 s" ñ" trac!
  0xA5 s" Ñ" trac!
  0xA6 s" ª" trac!
  0xA7 s" º" trac!
  0xA8 s" ¿" trac!
  0xA9 s" ®" trac!
  0xAA s" ¬" trac!
  0xAB s" ½" trac!
  0xAC s" ¼" trac!
  0xAD s" ¡" trac!
  0xAE s" «" trac!
  0xAF s" »" trac!
  0xB0 s" ░" trac!
  0xB1 s" ▒" trac!
  0xB2 s" ▓" trac!
  0xB3 s" │" trac!
  0xB4 s" ┤" trac!
  0xB5 s" Á" trac!
  0xB6 s" Â" trac!
  0xB7 s" À" trac!
  0xB8 s" ©" trac!
  0xB9 s" ╣" trac!
  0xBA s" ║" trac!
  0xBB s" ╗" trac!
  0xBC s" ╝" trac!
  0xBD s" ¢" trac!
  0xBE s" ¥" trac!
  0xBF s" ┐" trac!
  0xC0 s" └" trac!
  0xC1 s" ┴" trac!
  0xC2 s" ┬" trac!
  0xC3 s" ├" trac!
  0xC4 s" ─" trac!
  0xC5 s" ┼" trac!
  0xC6 s" ã" trac!
  0xC7 s" Ã" trac!
  0xC8 s" ╚" trac!
  0xC9 s" ╔" trac!
  0xCA s" ╩" trac!
  0xCB s" ╦" trac!
  0xCC s" ╠" trac!
  0xCD s" ═" trac!
  0xCE s" ╬" trac!
  0xCF s" ¤" trac!
  0xD0 s" ð" trac!
  0xD1 s" Ð" trac!
  0xD2 s" Ê" trac!
  0xD3 s" Ë" trac!
  0xD4 s" È" trac!
  0xD5 s" ı" trac!
  0xD6 s" Í" trac!
  0xD7 s" Î" trac!
  0xD8 s" Ï" trac!
  0xD9 s" ┘" trac!
  0xDA s" ┌" trac!
  0xDB s" █" trac!
  0xDC s" ▄" trac!
  0xDD s" ¦" trac!
  0xDE s" Ì" trac!
  0xDF s" ▀" trac!
  0xE0 s" Ó" trac!
  0xE1 s" ß" trac!
  0xE2 s" Ô" trac!
  0xE3 s" Ò" trac!
  0xE4 s" õ" trac!
  0xE5 s" Õ" trac!
  0xE6 s" µ" trac!
  0xE7 s" þ" trac!
  0xE8 s" Þ" trac!
  0xE9 s" Ú" trac!
  0xEA s" Û" trac!
  0xEB s" Ù" trac!
  0xEC s" ý" trac!
  0xED s" Ý" trac!
  0xEE s" ¯" trac!
  0xEF s" ´" trac!
  0xF0 s" ­" trac!  \ char 173
  0xF1 s" ±" trac!
  0xF2 s" ‗" trac!
  0xF3 s" ¾" trac!
  0xF4 s" ¶" trac!
  0xF5 s" §" trac!
  0xF6 s" ÷" trac!
  0xF7 s" ¸" trac!
  0xF8 s" °" trac!
  0xF9 s" ¨" trac!
  0xFA s" ·" trac!
  0xFB s" ¹" trac!
  0xFC s" ³" trac!
  0xFD s" ²" trac!
  0xFE s" ■" trac!
  0xFF s"  " trac!  \ char 160
