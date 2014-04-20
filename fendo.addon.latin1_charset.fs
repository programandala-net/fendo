.( fendo.addon.latin1_charset.fs) cr

\ This file is part of Fendo.

\ This file is the latin1 charset addon.

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

\ 2013-12-13: Written.

\ **************************************************************

forth_definitions
require galope/uncodepaged.fs
fendo_definitions

\ Reference:
\ http://www.unicode.org/Public/MAPPINGS/ISO8859/8859-1.TXT

uncodepage: latin1_charset_to_utf8

  \ Translation table from the latin1 charset
  \ (ISO/IEC 8859-1:1998) to UTF-8.

  0xA0 s"  "  \ No-Break Space (NBSP, char 0x00A0)
  0xA1 s" ¡"  \ Inverted Exclamation Mark
  0xA2 s" ¢"  \ Cent Sign
  0xA3 s" £"  \ Pound Sign
  0xA4 s" ¤"  \ Currency Sign
  0xA5 s" ¥"  \ Yen Sign
  0xA6 s" ¦"  \ Broken Bar
  0xA7 s" §"  \ Section Sign
  0xA8 s" ¨"  \ Diaeresis
  0xA9 s" ©"  \ Copyright Sign
  0xAA s" ª"  \ Feminine Ordinal Indicator
  0xAB s" «"  \ Left-Pointing Double Angle Quotation Mark
  0xAC s" ¬"  \ Not Sign
  0xAD s" ­"  \ Soft Hyphen (SHY, char 0x00AD)
  0xAE s" ®"  \ Registered Sign
  0xAF s" ¯"  \ Macron
  0xB0 s" °"  \ Degree Sign
  0xB1 s" ±"  \ Plus-Minus Sign
  0xB2 s" ²"  \ Superscript Two
  0xB3 s" ³"  \ Superscript Three
  0xB4 s" ´"  \ Acute Accent
  0xB5 s" µ"  \ Micro Sign
  0xB6 s" ¶"  \ Pilcrow Sign
  0xB7 s" ·"  \ Middle Dot
  0xB8 s" ¸"  \ Cedilla
  0xB9 s" ¹"  \ Superscript One
  0xBA s" º"  \ Masculine Ordinal Indicator
  0xBB s" »"  \ Right-Pointing Double Angle Quotation Mark
  0xBC s" ¼"  \ Vulgar Fraction One Quarter
  0xBD s" ½"  \ Vulgar Fraction One Half
  0xBE s" ¾"  \ Vulgar Fraction Three Quarters
  0xBF s" ¿"  \ Inverted Question Mark
  0xC0 s" À"  \ Latin Capital Letter A With Grave
  0xC1 s" Á"  \ Latin Capital Letter A With Acute
  0xC2 s" Â"  \ Latin Capital Letter A With Circumflex
  0xC3 s" Ã"  \ Latin Capital Letter A With Tilde
  0xC4 s" Ä"  \ Latin Capital Letter A With Diaeresis
  0xC5 s" Å"  \ Latin Capital Letter A With Ring Above
  0xC6 s" Æ"  \ Latin Capital Ligature Ae
  0xC7 s" Ç"  \ Latin Capital Letter C With Cedilla
  0xC8 s" È"  \ Latin Capital Letter E With Grave
  0xC9 s" É"  \ Latin Capital Letter E With Acute
  0xCA s" Ê"  \ Latin Capital Letter E With Circumflex
  0xCB s" Ë"  \ Latin Capital Letter E With Diaeresis
  0xCC s" Ì"  \ Latin Capital Letter I With Grave
  0xCD s" Í"  \ Latin Capital Letter I With Acute
  0xCE s" Î"  \ Latin Capital Letter I With Circumflex
  0xCF s" Ï"  \ Latin Capital Letter I With Diaeresis
  0xD0 s" Ð"  \ Latin Capital Letter Eth
  0xD1 s" Ñ"  \ Latin Capital Letter N With Tilde
  0xD2 s" Ò"  \ Latin Capital Letter O With Grave
  0xD3 s" Ó"  \ Latin Capital Letter O With Acute
  0xD4 s" Ô"  \ Latin Capital Letter O With Circumflex
  0xD5 s" Õ"  \ Latin Capital Letter O With Tilde
  0xD6 s" Ö"  \ Latin Capital Letter O With Diaeresis
  0xD7 s" ×"  \ Multiplication Sign
  0xD8 s" Ø"  \ Latin Capital Letter O With Stroke
  0xD9 s" Ù"  \ Latin Capital Letter U With Grave
  0xDA s" Ú"  \ Latin Capital Letter U With Acute
  0xDB s" Û"  \ Latin Capital Letter U With Circumflex
  0xDC s" Ü"  \ Latin Capital Letter U With Diaeresis
  0xDD s" Ý"  \ Latin Capital Letter Y With Acute
  0xDE s" Þ"  \ Latin Capital Letter Thorn
  0xDF s" ß"  \ Latin Small Letter Sharp S
  0xE0 s" à"  \ Latin Small Letter A With Grave
  0xE1 s" á"  \ Latin Small Letter A With Acute
  0xE2 s" â"  \ Latin Small Letter A With Circumflex
  0xE3 s" ã"  \ Latin Small Letter A With Tilde
  0xE4 s" ä"  \ Latin Small Letter A With Diaeresis
  0xE5 s" å"  \ Latin Small Letter A With Ring Above
  0xE6 s" æ"  \ Latin Small Ligature Ae
  0xE7 s" ç"  \ Latin Small Letter C With Cedilla
  0xE8 s" è"  \ Latin Small Letter E With Grave
  0xE9 s" é"  \ Latin Small Letter E With Acute
  0xEA s" ê"  \ Latin Small Letter E With Circumflex
  0xEB s" ë"  \ Latin Small Letter E With Diaeresis
  0xEC s" ì"  \ Latin Small Letter I With Grave
  0xED s" í"  \ Latin Small Letter I With Acute
  0xEE s" î"  \ Latin Small Letter I With Circumflex
  0xEF s" ï"  \ Latin Small Letter I With Diaeresis
  0xF0 s" ð"  \ Latin Small Letter Eth
  0xF1 s" ñ"  \ Latin Small Letter N With Tilde
  0xF2 s" ò"  \ Latin Small Letter O With Grave
  0xF3 s" ó"  \ Latin Small Letter O With Acute
  0xF4 s" ô"  \ Latin Small Letter O With Circumflex
  0xF5 s" õ"  \ Latin Small Letter O With Tilde
  0xF6 s" ö"  \ Latin Small Letter O With Diaeresis
  0xF7 s" ÷"  \ Division Sign
  0xF8 s" ø"  \ Latin Small Letter O With Stroke
  0xF9 s" ù"  \ Latin Small Letter U With Grave
  0xFA s" ú"  \ Latin Small Letter U With Acute
  0xFB s" û"  \ Latin Small Letter U With Circumflex
  0xFC s" ü"  \ Latin Small Letter U With Diaeresis
  0xFD s" ý"  \ Latin Small Letter Y With Acute
  0xFE s" þ"  \ Latin Small Letter Thorn
  0xFF s" ÿ"  \ Latin Small Letter Y With Diaeresis

;uncodepage

.( fendo.addon.latin1_charset.fs compiled) cr

