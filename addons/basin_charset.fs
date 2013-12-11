.( addons/basin_charset.fs) cr

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

\ 2013-12-11 Start. 

\ **************************************************************

require galope/translated.fs

translations: basin_charset
\  s" \`" s" &pound;"
  s" \`" s" £"
  s" \@" s" @"
\  s" \*" s" &copy;"
  s" \*" s" ©"
  s" \ " s" <strong class='ZXSpectrumBlockGraph'>&nbsp;</strong>"
  s" \ '" s" <strong class='ZXSpectrumBlockGraph'>&#x259D;</strong>"
  s" \' " s" <strong class='ZXSpectrumBlockGraph'>&#x2598;</strong>"
  s" \''" s" <strong class='ZXSpectrumBlockGraph'>&#x2580;</strong>"
  s" \ ." s" <strong class='ZXSpectrumBlockGraph'>&#x2597;</strong>"
  s" \ :" s" <strong class='ZXSpectrumBlockGraph'>&#x2590;</strong>"
  s" \'." s" <strong class='ZXSpectrumBlockGraph'>&#x259A;</strong>"
  s" \':" s" <strong class='ZXSpectrumBlockGraph'>&#x259C;</strong>"
  s" \. " s" <strong class='ZXSpectrumBlockGraph'>&#x2596;</strong>"
  s" \.'" s" <strong class='ZXSpectrumBlockGraph'>&#x259E;</strong>"
  s" \: " s" <strong class='ZXSpectrumBlockGraph'>&#x258C;</strong>"
  s" \:'" s" <strong class='ZXSpectrumBlockGraph'>&#x259B;</strong>"
  s" \.." s" <strong class='ZXSpectrumBlockGraph'>&#x2584;</strong>"
  s" \.:" s" <strong class='ZXSpectrumBlockGraph'>&#x259F;</strong>"
  s" \:." s" <strong class='ZXSpectrumBlockGraph'>&#x2599;</strong>"
  s" \::" s" <strong class='ZXSpectrumBlockGraph'>&#x2588</strong>"
  s" \a" s" <strong class='ZXSpectrumUDG'>A</strong>"
  s" \b" s" <strong class='ZXSpectrumUDG'>B</strong>"
  s" \c" s" <strong class='ZXSpectrumUDG'>C</strong>"
  s" \d" s" <strong class='ZXSpectrumUDG'>D</strong>"
  s" \e" s" <strong class='ZXSpectrumUDG'>E</strong>"
  s" \f" s" <strong class='ZXSpectrumUDG'>F</strong>"
  s" \g" s" <strong class='ZXSpectrumUDG'>G</strong>"
  s" \h" s" <strong class='ZXSpectrumUDG'>H</strong>"
  s" \i" s" <strong class='ZXSpectrumUDG'>I</strong>"
  s" \j" s" <strong class='ZXSpectrumUDG'>J</strong>"
  s" \k" s" <strong class='ZXSpectrumUDG'>K</strong>"
  s" \l" s" <strong class='ZXSpectrumUDG'>L</strong>"
  s" \m" s" <strong class='ZXSpectrumUDG'>M</strong>"
  s" \n" s" <strong class='ZXSpectrumUDG'>N</strong>"
  s" \o" s" <strong class='ZXSpectrumUDG'>O</strong>"
  s" \p" s" <strong class='ZXSpectrumUDG'>P</strong>"
  s" \q" s" <strong class='ZXSpectrumUDG'>Q</strong>"
  s" \r" s" <strong class='ZXSpectrumUDG'>R</strong>"
  s" \s" s" <strong class='ZXSpectrumUDG'>S</strong>"
  s" \t" s" <strong class='ZXSpectrumUDG'>T</strong>"
  s" \u" s" <strong class='ZXSpectrumUDG'>U</strong>"
  s" \#165" s" <strong class='ZXSpectrumToken'>RND</strong>"
  s" \#166" s" <strong class='ZXSpectrumToken'>INKEY$</strong>"
  s" \#167" s" <strong class='ZXSpectrumToken'>PI</strong>"
  s" \#168" s" <strong class='ZXSpectrumToken'>FN</strong>"
  s" \#169" s" <strong class='ZXSpectrumToken'>POINT</strong>"
  s" \#170" s" <strong class='ZXSpectrumToken'>SCREEN$</strong>"
  s" \#171" s" <strong class='ZXSpectrumToken'>ATTR</strong>"
  s" \#172" s" <strong class='ZXSpectrumToken'>AT</strong>"
  s" \#173" s" <strong class='ZXSpectrumToken'>TAB</strong>"
  s" \#174" s" <strong class='ZXSpectrumToken'>VAL$</strong>"
  s" \#175" s" <strong class='ZXSpectrumToken'>CODE</strong>"
  s" \#176" s" <strong class='ZXSpectrumToken'>VAL</strong>"
  s" \#177" s" <strong class='ZXSpectrumToken'>LEN</strong>"
  s" \#178" s" <strong class='ZXSpectrumToken'>SIN</strong>"
  s" \#179" s" <strong class='ZXSpectrumToken'>COS</strong>"
  s" \#180" s" <strong class='ZXSpectrumToken'>TAN</strong>"
  s" \#181" s" <strong class='ZXSpectrumToken'>ASN</strong>"
  s" \#182" s" <strong class='ZXSpectrumToken'>ACS</strong>"
  s" \#183" s" <strong class='ZXSpectrumToken'>ATN</strong>"
  s" \#184" s" <strong class='ZXSpectrumToken'>LN</strong>"
  s" \#185" s" <strong class='ZXSpectrumToken'>EXP</strong>"
  s" \#186" s" <strong class='ZXSpectrumToken'>INT</strong>"
  s" \#187" s" <strong class='ZXSpectrumToken'>SQR</strong>"
  s" \#188" s" <strong class='ZXSpectrumToken'>SGN</strong>"
  s" \#189" s" <strong class='ZXSpectrumToken'>ABS</strong>"
  s" \#190" s" <strong class='ZXSpectrumToken'>PEEK</strong>"
  s" \#191" s" <strong class='ZXSpectrumToken'>IN</strong>"
  s" \#192" s" <strong class='ZXSpectrumToken'>USR</strong>"
  s" \#193" s" <strong class='ZXSpectrumToken'>STR$</strong>"
  s" \#194" s" <strong class='ZXSpectrumToken'>CHR$</strong>"
  s" \#195" s" <strong class='ZXSpectrumToken'>NOT</strong>"
  s" \#196" s" <strong class='ZXSpectrumToken'>BIN</strong>"
  s" \#197" s" <strong class='ZXSpectrumToken'>OR</strong>"
  s" \#198" s" <strong class='ZXSpectrumToken'>AND</strong>"
  s" \#199" s" <strong class='ZXSpectrumToken'>&lt;=</strong>"
  s" \#200" s" <strong class='ZXSpectrumToken'>&gt;=</strong>"
  s" \#201" s" <strong class='ZXSpectrumToken'>&lt;&gt;</strong>"
  s" \#202" s" <strong class='ZXSpectrumToken'>LINE</strong>"
  s" \#203" s" <strong class='ZXSpectrumToken'>THEN</strong>"
  s" \#204" s" <strong class='ZXSpectrumToken'>TO</strong>"
  s" \#205" s" <strong class='ZXSpectrumToken'>STEP</strong>"
  s" \#206" s" <strong class='ZXSpectrumToken'>DEF FN</strong>"
  s" \#207" s" <strong class='ZXSpectrumToken'>CAT</strong>"
  s" \#208" s" <strong class='ZXSpectrumToken'>FORMAT</strong>"
  s" \#209" s" <strong class='ZXSpectrumToken'>MOVE</strong>"
  s" \#210" s" <strong class='ZXSpectrumToken'>ERASE</strong>"
  s" \#211" s" <strong class='ZXSpectrumToken'>OPEN#</strong>"
  s" \#212" s" <strong class='ZXSpectrumToken'>CLOSE#</strong>"
  s" \#213" s" <strong class='ZXSpectrumToken'>MERGE</strong>"
  s" \#214" s" <strong class='ZXSpectrumToken'>VERIFY</strong>"
  s" \#215" s" <strong class='ZXSpectrumToken'>BEEP</strong>"
  s" \#216" s" <strong class='ZXSpectrumToken'>CIRCLE</strong>"
  s" \#217" s" <strong class='ZXSpectrumToken'>INK</strong>"
  s" \#218" s" <strong class='ZXSpectrumToken'>PAPER</strong>"
  s" \#219" s" <strong class='ZXSpectrumToken'>FLASH</strong>"
  s" \#220" s" <strong class='ZXSpectrumToken'>BRIGHT</strong>"
  s" \#221" s" <strong class='ZXSpectrumToken'>INVERSE</strong>"
  s" \#222" s" <strong class='ZXSpectrumToken'>OVER</strong>"
  s" \#223" s" <strong class='ZXSpectrumToken'>OUT</strong>"
  s" \#224" s" <strong class='ZXSpectrumToken'>LPRINT</strong>"
  s" \#225" s" <strong class='ZXSpectrumToken'>LLIST</strong>"
  s" \#226" s" <strong class='ZXSpectrumToken'>STOP</strong>"
  s" \#227" s" <strong class='ZXSpectrumToken'>READ</strong>"
  s" \#228" s" <strong class='ZXSpectrumToken'>DATA</strong>"
  s" \#229" s" <strong class='ZXSpectrumToken'>RESTORE</strong>"
  s" \#230" s" <strong class='ZXSpectrumToken'>NEW</strong>"
  s" \#231" s" <strong class='ZXSpectrumToken'>BORDER</strong>"
  s" \#232" s" <strong class='ZXSpectrumToken'>CONTINUE</strong>"
  s" \#233" s" <strong class='ZXSpectrumToken'>DIM</strong>"
  s" \#234" s" <strong class='ZXSpectrumToken'>REM</strong>"
  s" \#235" s" <strong class='ZXSpectrumToken'>FOR</strong>"
  s" \#236" s" <strong class='ZXSpectrumToken'>GO TO</strong>"
  s" \#237" s" <strong class='ZXSpectrumToken'>GO SUB</strong>"
  s" \#238" s" <strong class='ZXSpectrumToken'>INPUT</strong>"
  s" \#239" s" <strong class='ZXSpectrumToken'>LOAD</strong>"
  s" \#240" s" <strong class='ZXSpectrumToken'>LIST</strong>"
  s" \#241" s" <strong class='ZXSpectrumToken'>LET</strong>"
  s" \#242" s" <strong class='ZXSpectrumToken'>PAUSE</strong>"
  s" \#243" s" <strong class='ZXSpectrumToken'>NEXT</strong>"
  s" \#244" s" <strong class='ZXSpectrumToken'>POKE</strong>"
  s" \#245" s" <strong class='ZXSpectrumToken'>PRINT</strong>"
  s" \#246" s" <strong class='ZXSpectrumToken'>PLOT</strong>"
  s" \#247" s" <strong class='ZXSpectrumToken'>RUN</strong>"
  s" \#248" s" <strong class='ZXSpectrumToken'>SAVE</strong>"
  s" \#249" s" <strong class='ZXSpectrumToken'>RANDOMIZE</strong>"
  s" \#250" s" <strong class='ZXSpectrumToken'>IF</strong>"
  s" \#251" s" <strong class='ZXSpectrumToken'>CLS</strong>"
  s" \#252" s" <strong class='ZXSpectrumToken'>DRAW</strong>"
  s" \#253" s" <strong class='ZXSpectrumToken'>CLEAR</strong>"
  s" \#254" s" <strong class='ZXSpectrumToken'>RETURN</strong>"
  s" \#255" s" <strong class='ZXSpectrumToken'>COPY</strong>"
;translations
