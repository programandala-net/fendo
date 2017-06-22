.( fendo.addon.zx_spectrum_charset.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the ZX Spectrum source code addon.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2015,2017 Marcos Cruz (programandala.net)

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

forth_definitions
require galope/uncodepaged.fs
require galope/translated.fs
fendo_definitions

uncodepage: zx_spectrum_charset_below_128
  096 s" £"  \ British pound sterling
  127 s" ©"  \ copyright
;uncodepage
  \ These translations can be done with (and before) or without syntax
  \ highlighting.
  \ XXX FIXME -- see 'zx_spectrum_source_code_translated_without_highlighting'.

uncodepage: zx_spectrum_charset_for_source_code_without_highlighting

  \ XXX FIXME -- see 'zx_spectrum_source_code_translated_without_highlighting'.
  096 s" £"  \ British pound sterling
  127 s" ©"  \ copyright

  128 s" <span class='ZXSpectrumBlockGraph'>&nbsp;</span>"
  129 s" <span class='ZXSpectrumBlockGraph'>&#x259D;</span>"
  130 s" <span class='ZXSpectrumBlockGraph'>&#x2598;</span>"
  131 s" <span class='ZXSpectrumBlockGraph'>&#x2580;</span>"
  132 s" <span class='ZXSpectrumBlockGraph'>&#x2597;</span>"
  133 s" <span class='ZXSpectrumBlockGraph'>&#x2590;</span>"
  134 s" <span class='ZXSpectrumBlockGraph'>&#x259A;</span>"
  135 s" <span class='ZXSpectrumBlockGraph'>&#x259C;</span>"
  136 s" <span class='ZXSpectrumBlockGraph'>&#x2596;</span>"
  137 s" <span class='ZXSpectrumBlockGraph'>&#x259E;</span>"
  138 s" <span class='ZXSpectrumBlockGraph'>&#x258C;</span>"
  139 s" <span class='ZXSpectrumBlockGraph'>&#x259B;</span>"
  140 s" <span class='ZXSpectrumBlockGraph'>&#x2584;</span>"
  141 s" <span class='ZXSpectrumBlockGraph'>&#x259F;</span>"
  142 s" <span class='ZXSpectrumBlockGraph'>&#x2599;</span>"
  143 s" <span class='ZXSpectrumBlockGraph'>&#x2588</span>"
  144 s" <span class='ZXSpectrumUDG'>A</span>"
  145 s" <span class='ZXSpectrumUDG'>B</span>"
  146 s" <span class='ZXSpectrumUDG'>C</span>"
  147 s" <span class='ZXSpectrumUDG'>D</span>"
  148 s" <span class='ZXSpectrumUDG'>E</span>"
  149 s" <span class='ZXSpectrumUDG'>F</span>"
  150 s" <span class='ZXSpectrumUDG'>G</span>"
  151 s" <span class='ZXSpectrumUDG'>H</span>"
  152 s" <span class='ZXSpectrumUDG'>I</span>"
  153 s" <span class='ZXSpectrumUDG'>J</span>"
  154 s" <span class='ZXSpectrumUDG'>K</span>"
  155 s" <span class='ZXSpectrumUDG'>L</span>"
  156 s" <span class='ZXSpectrumUDG'>M</span>"
  157 s" <span class='ZXSpectrumUDG'>N</span>"
  158 s" <span class='ZXSpectrumUDG'>O</span>"
  159 s" <span class='ZXSpectrumUDG'>P</span>"
  160 s" <span class='ZXSpectrumUDG'>Q</span>"
  161 s" <span class='ZXSpectrumUDG'>R</span>"
  162 s" <span class='ZXSpectrumUDG'>S</span>"
  163 s" <span class='ZXSpectrumUDG'>T</span>"
  164 s" <span class='ZXSpectrumUDG'>U</span>"
  165 s" <span class='ZXSpectrumToken'>RND</span>"
  166 s" <span class='ZXSpectrumToken'>INKEY$</span>"
  167 s" <span class='ZXSpectrumToken'>PI</span>"
  168 s" <span class='ZXSpectrumToken'>FN</span>"
  169 s" <span class='ZXSpectrumToken'>POINT</span>"
  170 s" <span class='ZXSpectrumToken'>SCREEN$</span>"
  171 s" <span class='ZXSpectrumToken'>ATTR</span>"
  172 s" <span class='ZXSpectrumToken'>AT</span>"
  173 s" <span class='ZXSpectrumToken'>TAB</span>"
  174 s" <span class='ZXSpectrumToken'>VAL$</span>"
  175 s" <span class='ZXSpectrumToken'>CODE</span>"
  176 s" <span class='ZXSpectrumToken'>VAL</span>"
  177 s" <span class='ZXSpectrumToken'>LEN</span>"
  178 s" <span class='ZXSpectrumToken'>SIN</span>"
  179 s" <span class='ZXSpectrumToken'>COS</span>"
  180 s" <span class='ZXSpectrumToken'>TAN</span>"
  181 s" <span class='ZXSpectrumToken'>ASN</span>"
  182 s" <span class='ZXSpectrumToken'>ACS</span>"
  183 s" <span class='ZXSpectrumToken'>ATN</span>"
  184 s" <span class='ZXSpectrumToken'>LN</span>"
  185 s" <span class='ZXSpectrumToken'>EXP</span>"
  186 s" <span class='ZXSpectrumToken'>INT</span>"
  187 s" <span class='ZXSpectrumToken'>SQR</span>"
  188 s" <span class='ZXSpectrumToken'>SGN</span>"
  189 s" <span class='ZXSpectrumToken'>ABS</span>"
  190 s" <span class='ZXSpectrumToken'>PEEK</span>"
  191 s" <span class='ZXSpectrumToken'>IN</span>"
  192 s" <span class='ZXSpectrumToken'>USR</span>"
  193 s" <span class='ZXSpectrumToken'>STR$</span>"
  194 s" <span class='ZXSpectrumToken'>CHR$</span>"
  195 s" <span class='ZXSpectrumToken'>NOT</span>"
  196 s" <span class='ZXSpectrumToken'>BIN</span>"
  197 s" <span class='ZXSpectrumToken'>OR</span>"
  198 s" <span class='ZXSpectrumToken'>AND</span>"
  199 s" <span class='ZXSpectrumToken'>&lt;=</span>"
  200 s" <span class='ZXSpectrumToken'>&gt;=</span>"
  201 s" <span class='ZXSpectrumToken'>&lt;&gt;</span>"
  202 s" <span class='ZXSpectrumToken'>LINE</span>"
  203 s" <span class='ZXSpectrumToken'>THEN</span>"
  204 s" <span class='ZXSpectrumToken'>TO</span>"
  205 s" <span class='ZXSpectrumToken'>STEP</span>"
  206 s" <span class='ZXSpectrumToken'>DEF FN</span>"
  207 s" <span class='ZXSpectrumToken'>CAT</span>"
  208 s" <span class='ZXSpectrumToken'>FORMAT</span>"
  209 s" <span class='ZXSpectrumToken'>MOVE</span>"
  210 s" <span class='ZXSpectrumToken'>ERASE</span>"
  211 s" <span class='ZXSpectrumToken'>OPEN#</span>"
  212 s" <span class='ZXSpectrumToken'>CLOSE#</span>"
  213 s" <span class='ZXSpectrumToken'>MERGE</span>"
  214 s" <span class='ZXSpectrumToken'>VERIFY</span>"
  215 s" <span class='ZXSpectrumToken'>BEEP</span>"
  216 s" <span class='ZXSpectrumToken'>CIRCLE</span>"
  217 s" <span class='ZXSpectrumToken'>INK</span>"
  218 s" <span class='ZXSpectrumToken'>PAPER</span>"
  219 s" <span class='ZXSpectrumToken'>FLASH</span>"
  220 s" <span class='ZXSpectrumToken'>BRIGHT</span>"
  221 s" <span class='ZXSpectrumToken'>INVERSE</span>"
  222 s" <span class='ZXSpectrumToken'>OVER</span>"
  223 s" <span class='ZXSpectrumToken'>OUT</span>"
  224 s" <span class='ZXSpectrumToken'>LPRINT</span>"
  225 s" <span class='ZXSpectrumToken'>LLIST</span>"
  226 s" <span class='ZXSpectrumToken'>STOP</span>"
  227 s" <span class='ZXSpectrumToken'>READ</span>"
  228 s" <span class='ZXSpectrumToken'>DATA</span>"
  229 s" <span class='ZXSpectrumToken'>RESTORE</span>"
  230 s" <span class='ZXSpectrumToken'>NEW</span>"
  231 s" <span class='ZXSpectrumToken'>BORDER</span>"
  232 s" <span class='ZXSpectrumToken'>CONTINUE</span>"
  233 s" <span class='ZXSpectrumToken'>DIM</span>"
  234 s" <span class='ZXSpectrumToken'>REM</span>"
  235 s" <span class='ZXSpectrumToken'>FOR</span>"
  236 s" <span class='ZXSpectrumToken'>GO TO</span>"
  237 s" <span class='ZXSpectrumToken'>GO SUB</span>"
  238 s" <span class='ZXSpectrumToken'>INPUT</span>"
  239 s" <span class='ZXSpectrumToken'>LOAD</span>"
  240 s" <span class='ZXSpectrumToken'>LIST</span>"
  241 s" <span class='ZXSpectrumToken'>LET</span>"
  242 s" <span class='ZXSpectrumToken'>PAUSE</span>"
  243 s" <span class='ZXSpectrumToken'>NEXT</span>"
  244 s" <span class='ZXSpectrumToken'>POKE</span>"
  245 s" <span class='ZXSpectrumToken'>PRINT</span>"
  246 s" <span class='ZXSpectrumToken'>PLOT</span>"
  247 s" <span class='ZXSpectrumToken'>RUN</span>"
  248 s" <span class='ZXSpectrumToken'>SAVE</span>"
  249 s" <span class='ZXSpectrumToken'>RANDOMIZE</span>"
  250 s" <span class='ZXSpectrumToken'>IF</span>"
  251 s" <span class='ZXSpectrumToken'>CLS</span>"
  252 s" <span class='ZXSpectrumToken'>DRAW</span>"
  253 s" <span class='ZXSpectrumToken'>CLEAR</span>"
  254 s" <span class='ZXSpectrumToken'>RETURN</span>"
  255 s" <span class='ZXSpectrumToken'>COPY</span>"
;uncodepage
  \ These translations have to be used when highlighting is off.
  \
  \ Note: These translations would be useless before the syntax
  \ highlighting, because the syntax highlighting would convert the
  \ HTML markups to explicit characters.

translations: zx_spectrum_charset_for_source_code_with_highlighting

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
  \ These translations work after the syntax highlighting.

uncodepage: zx_spectrum_charset_for_unexpanded_llist_without_highlighting

  096 s" £"  \ British pound sterling
  127 s" ©"  \ copyright

  128 s" <span class='ZXSpectrumBlockGraph'>&nbsp;</span>"
  129 s" <span class='ZXSpectrumBlockGraph'>&#x259D;</span>"
  130 s" <span class='ZXSpectrumBlockGraph'>&#x2598;</span>"
  131 s" <span class='ZXSpectrumBlockGraph'>&#x2580;</span>"
  132 s" <span class='ZXSpectrumBlockGraph'>&#x2597;</span>"
  133 s" <span class='ZXSpectrumBlockGraph'>&#x2590;</span>"
  134 s" <span class='ZXSpectrumBlockGraph'>&#x259A;</span>"
  135 s" <span class='ZXSpectrumBlockGraph'>&#x259C;</span>"
  136 s" <span class='ZXSpectrumBlockGraph'>&#x2596;</span>"
  137 s" <span class='ZXSpectrumBlockGraph'>&#x259E;</span>"
  138 s" <span class='ZXSpectrumBlockGraph'>&#x258C;</span>"
  139 s" <span class='ZXSpectrumBlockGraph'>&#x259B;</span>"
  140 s" <span class='ZXSpectrumBlockGraph'>&#x2584;</span>"
  141 s" <span class='ZXSpectrumBlockGraph'>&#x259F;</span>"
  142 s" <span class='ZXSpectrumBlockGraph'>&#x2599;</span>"
  143 s" <span class='ZXSpectrumBlockGraph'>&#x2588</span>"
  144 s" <span class='ZXSpectrumUDG'>A</span>"
  145 s" <span class='ZXSpectrumUDG'>B</span>"
  146 s" <span class='ZXSpectrumUDG'>C</span>"
  147 s" <span class='ZXSpectrumUDG'>D</span>"
  148 s" <span class='ZXSpectrumUDG'>E</span>"
  149 s" <span class='ZXSpectrumUDG'>F</span>"
  150 s" <span class='ZXSpectrumUDG'>G</span>"
  151 s" <span class='ZXSpectrumUDG'>H</span>"
  152 s" <span class='ZXSpectrumUDG'>I</span>"
  153 s" <span class='ZXSpectrumUDG'>J</span>"
  154 s" <span class='ZXSpectrumUDG'>K</span>"
  155 s" <span class='ZXSpectrumUDG'>L</span>"
  156 s" <span class='ZXSpectrumUDG'>M</span>"
  157 s" <span class='ZXSpectrumUDG'>N</span>"
  158 s" <span class='ZXSpectrumUDG'>O</span>"
  159 s" <span class='ZXSpectrumUDG'>P</span>"
  160 s" <span class='ZXSpectrumUDG'>Q</span>"
  161 s" <span class='ZXSpectrumUDG'>R</span>"
  162 s" <span class='ZXSpectrumUDG'>S</span>"
  163 s" <span class='ZXSpectrumUDG'>T</span>"
  164 s" <span class='ZXSpectrumUDG'>U</span>"
  165 s"  RND "
  166 s"  INKEY$ "
  167 s"  PI "
  168 s"  FN "
  169 s"  POINT "
  170 s"  SCREEN$ "
  171 s"  ATTR "
  172 s"  AT "
  173 s"  TAB "
  174 s"  VAL$ "
  175 s"  CODE "
  176 s"  VAL "
  177 s"  LEN "
  178 s"  SIN "
  179 s"  COS "
  180 s"  TAN "
  181 s"  ASN "
  182 s"  ACS "
  183 s"  ATN "
  184 s"  LN "
  185 s"  EXP "
  186 s"  INT "
  187 s"  SQR "
  188 s"  SGN "
  189 s"  ABS "
  190 s"  PEEK "
  191 s"  IN "
  192 s"  USR "
  193 s"  STR$ "
  194 s"  CHR$ "
  195 s"  NOT "
  196 s"  BIN "
  197 s"  OR "
  198 s"  AND "
  199 s" &lt;="
  200 s" &gt;="
  201 s" &lt;&gt;"
  202 s"  LINE "
  203 s"  THEN"
  204 s"  TO "
  205 s"  STEP "
  206 s"  DEF FN "
  207 s"  CAT "
  208 s"  FORMAT "
  209 s"  MOVE "
  210 s"  ERASE "
  211 s"  OPEN # "
  212 s"  CLOSE # "
  213 s"  MERGE "
  214 s"  VERIFY "
  215 s"  BEEP "
  216 s"  CIRCLE "
  217 s"  INK "
  218 s"  PAPER "
  219 s"  FLASH "
  220 s"  BRIGHT "
  221 s"  INVERSE "
  222 s"  OVER "
  223 s"  OUT "
  224 s"  LPRINT "
  225 s"  LLIST "
  226 s"  STOP "
  227 s"  READ "
  228 s"  DATA "
  229 s"  RESTORE "
  230 s"  NEW "
  231 s"  BORDER "
  232 s"  CONTINUE "
  233 s"  DIM "
  234 s"  REM "
  235 s"  FOR "
  236 s"  GO TO "
  237 s"  GO SUB "
  238 s"  INPUT "
  239 s"  LOAD "
  240 s"  LIST "
  241 s"  LET "
  242 s"  PAUSE "
  243 s"  NEXT "
  244 s"  POKE "
  245 s"  PRINT "
  246 s"  PLOT "
  247 s"  RUN "
  248 s"  SAVE "
  249 s"  RANDOMIZE "
  250 s"  IF "
  251 s"  CLS "
  252 s"  DRAW "
  253 s"  CLEAR "
  254 s"  RETURN "
  255 s"  COPY "

;uncodepage
  \ These translations convert a raw unexpanded printer listing
  \ from ZX Spectrum +3 (using 'FORMAT LPRINT "U":LLIST').
  \
  \ XXX TODO -- 2015-01-31: not used yet

uncodepage: zx_spectrum_charset_for_unexpanded_llist_before_highlighting

\  013 s\" \n"   \ end of line: CR -> LF
  096 s" £"     \ British pound sterling
  127 s" ©"     \ copyright
  165 s"  RND "
  166 s"  INKEY$ "
  167 s"  PI "
  168 s"  FN "
  169 s"  POINT "
  170 s"  SCREEN$ "
  171 s"  ATTR "
  172 s"  AT "
  173 s"  TAB "
  174 s"  VAL$ "
  175 s"  CODE "
  176 s"  VAL "
  177 s"  LEN "
  178 s"  SIN "
  179 s"  COS "
  180 s"  TAN "
  181 s"  ASN "
  182 s"  ACS "
  183 s"  ATN "
  184 s"  LN "
  185 s"  EXP "
  186 s"  INT "
  187 s"  SQR "
  188 s"  SGN "
  189 s"  ABS "
  190 s"  PEEK "
  191 s"  IN "
  192 s"  USR "
  193 s"  STR$ "
  194 s"  CHR$ "
  195 s"  NOT "
  196 s"  BIN "
  197 s"  OR "
  198 s"  AND "
  199 s" <"
  200 s" >"
  201 s" <>"
  202 s"  LINE "
  203 s"  THEN"
  204 s"  TO "
  205 s"  STEP "
  206 s"  DEF FN "
  207 s"  CAT "
  208 s"  FORMAT "
  209 s"  MOVE "
  210 s"  ERASE "
  211 s"  OPEN # "
  212 s"  CLOSE # "
  213 s"  MERGE "
  214 s"  VERIFY "
  215 s"  BEEP "
  216 s"  CIRCLE "
  217 s"  INK "
  218 s"  PAPER "
  219 s"  FLASH "
  220 s"  BRIGHT "
  221 s"  INVERSE "
  222 s"  OVER "
  223 s"  OUT "
  224 s"  LPRINT "
  225 s"  LLIST "
  226 s"  STOP "
  227 s"  READ "
  228 s"  DATA "
  229 s"  RESTORE "
  230 s"  NEW "
  231 s"  BORDER "
  232 s"  CONTINUE "
  233 s"  DIM "
  234 s"  REM "
  235 s"  FOR "
  236 s"  GO TO "
  237 s"  GO SUB "
  238 s"  INPUT "
  239 s"  LOAD "
  240 s"  LIST "
  241 s"  LET "
  242 s"  PAUSE "
  243 s"  NEXT "
  244 s"  POKE "
  245 s"  PRINT "
  246 s"  PLOT "
  247 s"  RUN "
  248 s"  SAVE "
  249 s"  RANDOMIZE "
  250 s"  IF "
  251 s"  CLS "
  252 s"  DRAW "
  253 s"  CLEAR "
  254 s"  RETURN "
  255 s"  COPY "

;uncodepage

translations: zx_spectrum_charset_for_unexpanded_llist_after_highlighting

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

;translations

: zx_spectrum_source_code_translated_before_highlighting ( ca len -- ca' len' )
  zx_spectrum_charset_below_128 uncodepaged ;
  \ Convert the content of a ZX Spectrum file to UTF-8,
  \ before the syntax highlighting.

: zx_spectrum_source_code_translated_after_highlighting ( ca len -- ca' len' )
  zx_spectrum_charset_for_source_code_with_highlighting translated ;
  \ Convert the content of a ZX Spectrum file to UTF-8,
  \ after the syntax highlighting.

: zx_spectrum_source_code_translated_without_highlighting ( ca len -- ca' len' )
\  zx_spectrum_charset_below_128 uncodepaged  \ XXX FIXME -- old
  zx_spectrum_charset_for_source_code_without_highlighting uncodepaged ;
  \ Convert the content of a ZX Spectrum file to UTF-8.
  \
  \ XXX FIXME When 'zx_spectrum_charset_below_128' is used here,
  \ the translation done with
  \ 'zx_spectrum_charset_for_source_code_without_highlighting' is ruined
  \ (if the order is changed, it seems to work, but it is not a good
  \ solution, because one of the chars translated is 127).
  \ That's why all translations are done in
  \ 'zx_spectrum_charset_for_source_code_without_highlighting'.
  \ It seems the bug is in 'uncodepaged':
  \ when two translations are done on a chain way, something
  \ can go wrong.

: set_zx_spectrum_source_code_translation ( -- )
  highlight? if
    ['] zx_spectrum_source_code_translated_before_highlighting
    is source_code_pretranslated
    ['] zx_spectrum_source_code_translated_after_highlighting
    is source_code_posttranslated
  else
    ['] zx_spectrum_source_code_translated_without_highlighting
    is source_code_pretranslated
  then ;
  \ Set the proper translation for ZX Spectrum source code files.

: zx_spectrum_unexpanded_llist_translated_before_highlighting ( ca len -- ca' len' )
  zx_spectrum_charset_for_unexpanded_llist_before_highlighting uncodepaged ;
  \ Convert the content of a ZX Spectrum +3
  \ unexpanded llist file to UTF-8,
  \ before the syntax highlighting.

: zx_spectrum_unexpanded_llist_translated_after_highlighting ( ca len -- ca' len' )
  zx_spectrum_charset_for_unexpanded_llist_after_highlighting translated ;
  \ Convert the content of a ZX Spectrum +3
  \ unexpanded llist file to UTF-8,
  \ after the syntax highlighting.

: zx_spectrum_unexpanded_llist_translated_without_highlighting ( ca len -- ca' len' )
  zx_spectrum_charset_for_unexpanded_llist_without_highlighting uncodepaged ;
  \ Convert the content of a ZX Spectrum +3
  \ unexpanded llist file to UTF-8.

: set_zx_spectrum_unexpanded_llist_translation ( -- )
  highlight? if
    ['] zx_spectrum_unexpanded_llist_translated_before_highlighting
    is source_code_pretranslated
    ['] zx_spectrum_unexpanded_llist_translated_after_highlighting
    is source_code_posttranslated
  else
    ['] zx_spectrum_unexpanded_llist_translated_without_highlighting
    is source_code_pretranslated
   then ;
  \ Set the proper translation for ZX Spectrum unexpanded llist files,
  \ created by ZX Spectrum +3 this way: 'FORMAT LPRINT "U":LLIST".

.( fendo.addon.zx_spectrum_charset.fs compiled) cr

\ ==============================================================
\ Change log

\ 2014-10-12: Start.
\
\ 2014-11-05: Fix: charset translation worked only when highlighting
\ was on. Now, 'set_zx_spectrum_source_code_translation' (factored
\ from  'zx_spectrum_source_code', defined in
\ <fendo.addon.zx_spectrum_source_code.fs>) does the proper selection.
\
\ 2014-11-05: Change: <galope/uncodepaged.fs> is used instead of
\ <galope/translated.fs> for one of the translation tables. It finally
\ works, but something strange happened in certain cases. It seems a
\ strange bug in uncodepaged (the details are in the code).
\
\ 2015-01-31: New: new words for unexpanded llistings (created by ZX
\ Spectrum +3 with 'FORMAT LPRINT "U":LLIST').
\
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
