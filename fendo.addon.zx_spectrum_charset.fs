.( fendo.addon.zx_spectrum_charset.fs) cr

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

\ 2014-10-12: Start.

\ **************************************************************

forth_definitions
require galope/translated.fs
require galope/char-to-string.fs
fendo_definitions

false [if]  \ old first version

translations: zx_spectrum_charset

  \ These translations are useless before the syntax highlighting,
  \ because the syntax highlighting converts the HTML markups to
  \ explicit characters.

  144 c>str s" <span class='ZXSpectrumUDG'>A</span>"
  145 c>str s" <span class='ZXSpectrumUDG'>B</span>"
  146 c>str s" <span class='ZXSpectrumUDG'>C</span>"
  147 c>str s" <span class='ZXSpectrumUDG'>D</span>"
  148 c>str s" <span class='ZXSpectrumUDG'>E</span>"
  149 c>str s" <span class='ZXSpectrumUDG'>F</span>"
  150 c>str s" <span class='ZXSpectrumUDG'>G</span>"
  151 c>str s" <span class='ZXSpectrumUDG'>H</span>"
  152 c>str s" <span class='ZXSpectrumUDG'>I</span>"
  153 c>str s" <span class='ZXSpectrumUDG'>J</span>"
  154 c>str s" <span class='ZXSpectrumUDG'>K</span>"
  155 c>str s" <span class='ZXSpectrumUDG'>L</span>"
  156 c>str s" <span class='ZXSpectrumUDG'>M</span>"
  157 c>str s" <span class='ZXSpectrumUDG'>N</span>"
  158 c>str s" <span class='ZXSpectrumUDG'>O</span>"
  159 c>str s" <span class='ZXSpectrumUDG'>P</span>"
  160 c>str s" <span class='ZXSpectrumUDG'>Q</span>"
  161 c>str s" <span class='ZXSpectrumUDG'>R</span>"
  162 c>str s" <span class='ZXSpectrumUDG'>S</span>"
  163 c>str s" <span class='ZXSpectrumUDG'>T</span>"
  164 c>str s" <span class='ZXSpectrumUDG'>U</span>"
  165 c>str s" <span class='ZXSpectrumToken'>RND</span>"
  166 c>str s" <span class='ZXSpectrumToken'>INKEY$</span>"
  167 c>str s" <span class='ZXSpectrumToken'>PI</span>"
  168 c>str s" <span class='ZXSpectrumToken'>FN</span>"
  169 c>str s" <span class='ZXSpectrumToken'>POINT</span>"
  170 c>str s" <span class='ZXSpectrumToken'>SCREEN$</span>"
  171 c>str s" <span class='ZXSpectrumToken'>ATTR</span>"
  172 c>str s" <span class='ZXSpectrumToken'>AT</span>"
  173 c>str s" <span class='ZXSpectrumToken'>TAB</span>"
  174 c>str s" <span class='ZXSpectrumToken'>VAL$</span>"
  175 c>str s" <span class='ZXSpectrumToken'>CODE</span>"
  176 c>str s" <span class='ZXSpectrumToken'>VAL</span>"
  177 c>str s" <span class='ZXSpectrumToken'>LEN</span>"
  178 c>str s" <span class='ZXSpectrumToken'>SIN</span>"
  179 c>str s" <span class='ZXSpectrumToken'>COS</span>"
  180 c>str s" <span class='ZXSpectrumToken'>TAN</span>"
  181 c>str s" <span class='ZXSpectrumToken'>ASN</span>"
  182 c>str s" <span class='ZXSpectrumToken'>ACS</span>"
  183 c>str s" <span class='ZXSpectrumToken'>ATN</span>"
  184 c>str s" <span class='ZXSpectrumToken'>LN</span>"
  185 c>str s" <span class='ZXSpectrumToken'>EXP</span>"
  186 c>str s" <span class='ZXSpectrumToken'>INT</span>"
  187 c>str s" <span class='ZXSpectrumToken'>SQR</span>"
  188 c>str s" <span class='ZXSpectrumToken'>SGN</span>"
  189 c>str s" <span class='ZXSpectrumToken'>ABS</span>"
  190 c>str s" <span class='ZXSpectrumToken'>PEEK</span>"
  191 c>str s" <span class='ZXSpectrumToken'>IN</span>"
  192 c>str s" <span class='ZXSpectrumToken'>USR</span>"
  193 c>str s" <span class='ZXSpectrumToken'>STR$</span>"
  194 c>str s" <span class='ZXSpectrumToken'>CHR$</span>"
  195 c>str s" <span class='ZXSpectrumToken'>NOT</span>"
  196 c>str s" <span class='ZXSpectrumToken'>BIN</span>"
  197 c>str s" <span class='ZXSpectrumToken'>OR</span>"
  198 c>str s" <span class='ZXSpectrumToken'>AND</span>"
  199 c>str s" <span class='ZXSpectrumToken'>&lt;=</span>"
  200 c>str s" <span class='ZXSpectrumToken'>&gt;=</span>"
  201 c>str s" <span class='ZXSpectrumToken'>&lt;&gt;</span>"
  202 c>str s" <span class='ZXSpectrumToken'>LINE</span>"
  203 c>str s" <span class='ZXSpectrumToken'>THEN</span>"
  204 c>str s" <span class='ZXSpectrumToken'>TO</span>"
  205 c>str s" <span class='ZXSpectrumToken'>STEP</span>"
  206 c>str s" <span class='ZXSpectrumToken'>DEF FN</span>"
  207 c>str s" <span class='ZXSpectrumToken'>CAT</span>"
  208 c>str s" <span class='ZXSpectrumToken'>FORMAT</span>"
  209 c>str s" <span class='ZXSpectrumToken'>MOVE</span>"
  210 c>str s" <span class='ZXSpectrumToken'>ERASE</span>"
  211 c>str s" <span class='ZXSpectrumToken'>OPEN#</span>"
  212 c>str s" <span class='ZXSpectrumToken'>CLOSE#</span>"
  213 c>str s" <span class='ZXSpectrumToken'>MERGE</span>"
  214 c>str s" <span class='ZXSpectrumToken'>VERIFY</span>"
  215 c>str s" <span class='ZXSpectrumToken'>BEEP</span>"
  216 c>str s" <span class='ZXSpectrumToken'>CIRCLE</span>"
  217 c>str s" <span class='ZXSpectrumToken'>INK</span>"
  218 c>str s" <span class='ZXSpectrumToken'>PAPER</span>"
  219 c>str s" <span class='ZXSpectrumToken'>FLASH</span>"
  220 c>str s" <span class='ZXSpectrumToken'>BRIGHT</span>"
  221 c>str s" <span class='ZXSpectrumToken'>INVERSE</span>"
  222 c>str s" <span class='ZXSpectrumToken'>OVER</span>"
  223 c>str s" <span class='ZXSpectrumToken'>OUT</span>"
  224 c>str s" <span class='ZXSpectrumToken'>LPRINT</span>"
  225 c>str s" <span class='ZXSpectrumToken'>LLIST</span>"
  226 c>str s" <span class='ZXSpectrumToken'>STOP</span>"
  227 c>str s" <span class='ZXSpectrumToken'>READ</span>"
  228 c>str s" <span class='ZXSpectrumToken'>DATA</span>"
  229 c>str s" <span class='ZXSpectrumToken'>RESTORE</span>"
  230 c>str s" <span class='ZXSpectrumToken'>NEW</span>"
  231 c>str s" <span class='ZXSpectrumToken'>BORDER</span>"
  232 c>str s" <span class='ZXSpectrumToken'>CONTINUE</span>"
  233 c>str s" <span class='ZXSpectrumToken'>DIM</span>"
  234 c>str s" <span class='ZXSpectrumToken'>REM</span>"
  235 c>str s" <span class='ZXSpectrumToken'>FOR</span>"
  236 c>str s" <span class='ZXSpectrumToken'>GO TO</span>"
  237 c>str s" <span class='ZXSpectrumToken'>GO SUB</span>"
  238 c>str s" <span class='ZXSpectrumToken'>INPUT</span>"
  239 c>str s" <span class='ZXSpectrumToken'>LOAD</span>"
  240 c>str s" <span class='ZXSpectrumToken'>LIST</span>"
  241 c>str s" <span class='ZXSpectrumToken'>LET</span>"
  242 c>str s" <span class='ZXSpectrumToken'>PAUSE</span>"
  243 c>str s" <span class='ZXSpectrumToken'>NEXT</span>"
  244 c>str s" <span class='ZXSpectrumToken'>POKE</span>"
  245 c>str s" <span class='ZXSpectrumToken'>PRINT</span>"
  246 c>str s" <span class='ZXSpectrumToken'>PLOT</span>"
  247 c>str s" <span class='ZXSpectrumToken'>RUN</span>"
  248 c>str s" <span class='ZXSpectrumToken'>SAVE</span>"
  249 c>str s" <span class='ZXSpectrumToken'>RANDOMIZE</span>"
  250 c>str s" <span class='ZXSpectrumToken'>IF</span>"
  251 c>str s" <span class='ZXSpectrumToken'>CLS</span>"
  252 c>str s" <span class='ZXSpectrumToken'>DRAW</span>"
  253 c>str s" <span class='ZXSpectrumToken'>CLEAR</span>"
  254 c>str s" <span class='ZXSpectrumToken'>RETURN</span>"
  255 c>str s" <span class='ZXSpectrumToken'>COPY</span>"
;translations

[then]

translations: zx_spectrum_charset

  \ These translations work after the syntax highlighting.

  s" &lt;80&gt;" s" <span class='ZXSpectrumBlockGraph'>&nbsp;</span>"
  s" &lt;81&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x259D;</span>"
  s" &lt;82&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2598;</span>"
  s" &lt;83&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2580;</span>"
  s" &lt;84&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2597;</span>"
  s" &lt;85&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2590;</span>"
  s" &lt;86&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x259A;</span>"
  s" &lt;87&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x259C;</span>"
  s" &lt;88&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2596;</span>"
  s" &lt;89&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x259E;</span>"
  s" &lt;8a&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x258C;</span>"
  s" &lt;8b&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x259B;</span>"
  s" &lt;8c&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2584;</span>"
  s" &lt;8d&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x259F;</span>"
  s" &lt;8e&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2599;</span>"
  s" &lt;8f&gt;" s" <span class='ZXSpectrumBlockGraph'>&#x2588</span>"
  s" &lt;90&gt;" s" <span class='ZXSpectrumUDG'>A</span>"
  s" &lt;91&gt;" s" <span class='ZXSpectrumUDG'>B</span>"
  s" &lt;92&gt;" s" <span class='ZXSpectrumUDG'>C</span>"
  s" &lt;93&gt;" s" <span class='ZXSpectrumUDG'>D</span>"
  s" &lt;94&gt;" s" <span class='ZXSpectrumUDG'>E</span>"
  s" &lt;95&gt;" s" <span class='ZXSpectrumUDG'>F</span>"
  s" &lt;96&gt;" s" <span class='ZXSpectrumUDG'>G</span>"
  s" &lt;97&gt;" s" <span class='ZXSpectrumUDG'>H</span>"
  s" &lt;98&gt;" s" <span class='ZXSpectrumUDG'>I</span>"
  s" &lt;99&gt;" s" <span class='ZXSpectrumUDG'>J</span>"
  s" &lt;9a&gt;" s" <span class='ZXSpectrumUDG'>K</span>"
  s" &lt;9b&gt;" s" <span class='ZXSpectrumUDG'>L</span>"
  s" &lt;9c&gt;" s" <span class='ZXSpectrumUDG'>M</span>"
  s" &lt;9d&gt;" s" <span class='ZXSpectrumUDG'>N</span>"
  s" &lt;9e&gt;" s" <span class='ZXSpectrumUDG'>O</span>"
  s" &lt;9f&gt;" s" <span class='ZXSpectrumUDG'>P</span>"
  s" &lt;a0&gt;" s" <span class='ZXSpectrumUDG'>Q</span>"
  s" &lt;a1&gt;" s" <span class='ZXSpectrumUDG'>R</span>"
  s" &lt;a2&gt;" s" <span class='ZXSpectrumUDG'>S</span>"
  s" &lt;a3&gt;" s" <span class='ZXSpectrumUDG'>T</span>"
  s" &lt;a4&gt;" s" <span class='ZXSpectrumUDG'>U</span>"
  s" &lt;a5&gt;" s" <span class='ZXSpectrumToken'>RND</span>"
  s" &lt;a6&gt;" s" <span class='ZXSpectrumToken'>INKEY$</span>"
  s" &lt;a7&gt;" s" <span class='ZXSpectrumToken'>PI</span>"
  s" &lt;a8&gt;" s" <span class='ZXSpectrumToken'>FN</span>"
  s" &lt;a9&gt;" s" <span class='ZXSpectrumToken'>POINT</span>"
  s" &lt;aa&gt;" s" <span class='ZXSpectrumToken'>SCREEN$</span>"
  s" &lt;ab&gt;" s" <span class='ZXSpectrumToken'>ATTR</span>"
  s" &lt;ac&gt;" s" <span class='ZXSpectrumToken'>AT</span>"
  s" &lt;ad&gt;" s" <span class='ZXSpectrumToken'>TAB</span>"
  s" &lt;ae&gt;" s" <span class='ZXSpectrumToken'>VAL$</span>"
  s" &lt;af&gt;" s" <span class='ZXSpectrumToken'>CODE</span>"
  s" &lt;b0&gt;" s" <span class='ZXSpectrumToken'>VAL</span>"
  s" &lt;b1&gt;" s" <span class='ZXSpectrumToken'>LEN</span>"
  s" &lt;b2&gt;" s" <span class='ZXSpectrumToken'>SIN</span>"
  s" &lt;b3&gt;" s" <span class='ZXSpectrumToken'>COS</span>"
  s" &lt;b4&gt;" s" <span class='ZXSpectrumToken'>TAN</span>"
  s" &lt;b5&gt;" s" <span class='ZXSpectrumToken'>ASN</span>"
  s" &lt;b6&gt;" s" <span class='ZXSpectrumToken'>ACS</span>"
  s" &lt;b7&gt;" s" <span class='ZXSpectrumToken'>ATN</span>"
  s" &lt;b8&gt;" s" <span class='ZXSpectrumToken'>LN</span>"
  s" &lt;b9&gt;" s" <span class='ZXSpectrumToken'>EXP</span>"
  s" &lt;ba&gt;" s" <span class='ZXSpectrumToken'>INT</span>"
  s" &lt;bb&gt;" s" <span class='ZXSpectrumToken'>SQR</span>"
  s" &lt;bc&gt;" s" <span class='ZXSpectrumToken'>SGN</span>"
  s" &lt;bd&gt;" s" <span class='ZXSpectrumToken'>ABS</span>"
  s" &lt;be&gt;" s" <span class='ZXSpectrumToken'>PEEK</span>"
  s" &lt;bf&gt;" s" <span class='ZXSpectrumToken'>IN</span>"
  s" &lt;c0&gt;" s" <span class='ZXSpectrumToken'>USR</span>"
  s" &lt;c1&gt;" s" <span class='ZXSpectrumToken'>STR$</span>"
  s" &lt;c2&gt;" s" <span class='ZXSpectrumToken'>CHR$</span>"
  s" &lt;c3&gt;" s" <span class='ZXSpectrumToken'>NOT</span>"
  s" &lt;c4&gt;" s" <span class='ZXSpectrumToken'>BIN</span>"
  s" &lt;c5&gt;" s" <span class='ZXSpectrumToken'>OR</span>"
  s" &lt;c6&gt;" s" <span class='ZXSpectrumToken'>AND</span>"
  s" &lt;c7&gt;" s" <span class='ZXSpectrumToken'>&lt;=</span>"
  s" &lt;c8&gt;" s" <span class='ZXSpectrumToken'>&gt;=</span>"
  s" &lt;c9&gt;" s" <span class='ZXSpectrumToken'>&lt;&gt;</span>"
  s" &lt;ca&gt;" s" <span class='ZXSpectrumToken'>LINE</span>"
  s" &lt;cb&gt;" s" <span class='ZXSpectrumToken'>THEN</span>"
  s" &lt;cc&gt;" s" <span class='ZXSpectrumToken'>TO</span>"
  s" &lt;cd&gt;" s" <span class='ZXSpectrumToken'>STEP</span>"
  s" &lt;ce&gt;" s" <span class='ZXSpectrumToken'>DEF FN</span>"
  s" &lt;cf&gt;" s" <span class='ZXSpectrumToken'>CAT</span>"
  s" &lt;d0&gt;" s" <span class='ZXSpectrumToken'>FORMAT</span>"
  s" &lt;d1&gt;" s" <span class='ZXSpectrumToken'>MOVE</span>"
  s" &lt;d2&gt;" s" <span class='ZXSpectrumToken'>ERASE</span>"
  s" &lt;d3&gt;" s" <span class='ZXSpectrumToken'>OPEN#</span>"
  s" &lt;d4&gt;" s" <span class='ZXSpectrumToken'>CLOSE#</span>"
  s" &lt;d5&gt;" s" <span class='ZXSpectrumToken'>MERGE</span>"
  s" &lt;d6&gt;" s" <span class='ZXSpectrumToken'>VERIFY</span>"
  s" &lt;d7&gt;" s" <span class='ZXSpectrumToken'>BEEP</span>"
  s" &lt;d8&gt;" s" <span class='ZXSpectrumToken'>CIRCLE</span>"
  s" &lt;d9&gt;" s" <span class='ZXSpectrumToken'>INK</span>"
  s" &lt;da&gt;" s" <span class='ZXSpectrumToken'>PAPER</span>"
  s" &lt;db&gt;" s" <span class='ZXSpectrumToken'>FLASH</span>"
  s" &lt;dc&gt;" s" <span class='ZXSpectrumToken'>BRIGHT</span>"
  s" &lt;dd&gt;" s" <span class='ZXSpectrumToken'>INVERSE</span>"
  s" &lt;de&gt;" s" <span class='ZXSpectrumToken'>OVER</span>"
  s" &lt;df&gt;" s" <span class='ZXSpectrumToken'>OUT</span>"
  s" &lt;e0&gt;" s" <span class='ZXSpectrumToken'>LPRINT</span>"
  s" &lt;e1&gt;" s" <span class='ZXSpectrumToken'>LLIST</span>"
  s" &lt;e2&gt;" s" <span class='ZXSpectrumToken'>STOP</span>"
  s" &lt;e3&gt;" s" <span class='ZXSpectrumToken'>READ</span>"
  s" &lt;e4&gt;" s" <span class='ZXSpectrumToken'>DATA</span>"
  s" &lt;e5&gt;" s" <span class='ZXSpectrumToken'>RESTORE</span>"
  s" &lt;e6&gt;" s" <span class='ZXSpectrumToken'>NEW</span>"
  s" &lt;e7&gt;" s" <span class='ZXSpectrumToken'>BORDER</span>"
  s" &lt;e8&gt;" s" <span class='ZXSpectrumToken'>CONTINUE</span>"
  s" &lt;e9&gt;" s" <span class='ZXSpectrumToken'>DIM</span>"
  s" &lt;ea&gt;" s" <span class='ZXSpectrumToken'>REM</span>"
  s" &lt;eb&gt;" s" <span class='ZXSpectrumToken'>FOR</span>"
  s" &lt;ec&gt;" s" <span class='ZXSpectrumToken'>GO TO</span>"
  s" &lt;ed&gt;" s" <span class='ZXSpectrumToken'>GO SUB</span>"
  s" &lt;ee&gt;" s" <span class='ZXSpectrumToken'>INPUT</span>"
  s" &lt;ef&gt;" s" <span class='ZXSpectrumToken'>LOAD</span>"
  s" &lt;f0&gt;" s" <span class='ZXSpectrumToken'>LIST</span>"
  s" &lt;f1&gt;" s" <span class='ZXSpectrumToken'>LET</span>"
  s" &lt;f2&gt;" s" <span class='ZXSpectrumToken'>PAUSE</span>"
  s" &lt;f3&gt;" s" <span class='ZXSpectrumToken'>NEXT</span>"
  s" &lt;f4&gt;" s" <span class='ZXSpectrumToken'>POKE</span>"
  s" &lt;f5&gt;" s" <span class='ZXSpectrumToken'>PRINT</span>"
  s" &lt;f6&gt;" s" <span class='ZXSpectrumToken'>PLOT</span>"
  s" &lt;f7&gt;" s" <span class='ZXSpectrumToken'>RUN</span>"
  s" &lt;f8&gt;" s" <span class='ZXSpectrumToken'>SAVE</span>"
  s" &lt;f9&gt;" s" <span class='ZXSpectrumToken'>RANDOMIZE</span>"
  s" &lt;fa&gt;" s" <span class='ZXSpectrumToken'>IF</span>"
  s" &lt;fb&gt;" s" <span class='ZXSpectrumToken'>CLS</span>"
  s" &lt;fc&gt;" s" <span class='ZXSpectrumToken'>DRAW</span>"
  s" &lt;fd&gt;" s" <span class='ZXSpectrumToken'>CLEAR</span>"
  s" &lt;fe&gt;" s" <span class='ZXSpectrumToken'>RETURN</span>"
  s" &lt;ff&gt;" s" <span class='ZXSpectrumToken'>COPY</span>"
;translations

: zx_spectrum_source_code_translated  ( ca len -- ca' len' )
  \ Convert the content of a ZX Spectrum file to UTF-8.
  zx_spectrum_charset translated
  ;

.( fendo.addon.zx_spectrum_charset.fs compiled) cr

