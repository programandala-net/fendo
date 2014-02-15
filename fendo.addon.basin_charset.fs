.( fendo.addon.basin_charset.fs) cr

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

\ 2013-12-11: Start.

\ **************************************************************

require galope/translated.fs

translations: basin_charset
\  s" \`" s" &pound;"
  s" \`" s" £"
  s" \@" s" @"
\  s" \*" s" &copy;"
  s" \*" s" ©"
  s" \ " s" <span class='ZXSpectrumBlockGraph'>&nbsp;</span>"
  s" \ '" s" <span class='ZXSpectrumBlockGraph'>&#x259D;</span>"
  s" \' " s" <span class='ZXSpectrumBlockGraph'>&#x2598;</span>"
  s" \''" s" <span class='ZXSpectrumBlockGraph'>&#x2580;</span>"
  s" \ ." s" <span class='ZXSpectrumBlockGraph'>&#x2597;</span>"
  s" \ :" s" <span class='ZXSpectrumBlockGraph'>&#x2590;</span>"
  s" \'." s" <span class='ZXSpectrumBlockGraph'>&#x259A;</span>"
  s" \':" s" <span class='ZXSpectrumBlockGraph'>&#x259C;</span>"
  s" \. " s" <span class='ZXSpectrumBlockGraph'>&#x2596;</span>"
  s" \.'" s" <span class='ZXSpectrumBlockGraph'>&#x259E;</span>"
  s" \: " s" <span class='ZXSpectrumBlockGraph'>&#x258C;</span>"
  s" \:'" s" <span class='ZXSpectrumBlockGraph'>&#x259B;</span>"
  s" \.." s" <span class='ZXSpectrumBlockGraph'>&#x2584;</span>"
  s" \.:" s" <span class='ZXSpectrumBlockGraph'>&#x259F;</span>"
  s" \:." s" <span class='ZXSpectrumBlockGraph'>&#x2599;</span>"
  s" \::" s" <span class='ZXSpectrumBlockGraph'>&#x2588</span>"
  s" \a" s" <span class='ZXSpectrumUDG'>A</span>"
  s" \b" s" <span class='ZXSpectrumUDG'>B</span>"
  s" \c" s" <span class='ZXSpectrumUDG'>C</span>"
  s" \d" s" <span class='ZXSpectrumUDG'>D</span>"
  s" \e" s" <span class='ZXSpectrumUDG'>E</span>"
  s" \f" s" <span class='ZXSpectrumUDG'>F</span>"
  s" \g" s" <span class='ZXSpectrumUDG'>G</span>"
  s" \h" s" <span class='ZXSpectrumUDG'>H</span>"
  s" \i" s" <span class='ZXSpectrumUDG'>I</span>"
  s" \j" s" <span class='ZXSpectrumUDG'>J</span>"
  s" \k" s" <span class='ZXSpectrumUDG'>K</span>"
  s" \l" s" <span class='ZXSpectrumUDG'>L</span>"
  s" \m" s" <span class='ZXSpectrumUDG'>M</span>"
  s" \n" s" <span class='ZXSpectrumUDG'>N</span>"
  s" \o" s" <span class='ZXSpectrumUDG'>O</span>"
  s" \p" s" <span class='ZXSpectrumUDG'>P</span>"
  s" \q" s" <span class='ZXSpectrumUDG'>Q</span>"
  s" \r" s" <span class='ZXSpectrumUDG'>R</span>"
  s" \s" s" <span class='ZXSpectrumUDG'>S</span>"
  s" \t" s" <span class='ZXSpectrumUDG'>T</span>"
  s" \u" s" <span class='ZXSpectrumUDG'>U</span>"
  s" \#165" s" <span class='ZXSpectrumToken'>RND</span>"
  s" \#166" s" <span class='ZXSpectrumToken'>INKEY$</span>"
  s" \#167" s" <span class='ZXSpectrumToken'>PI</span>"
  s" \#168" s" <span class='ZXSpectrumToken'>FN</span>"
  s" \#169" s" <span class='ZXSpectrumToken'>POINT</span>"
  s" \#170" s" <span class='ZXSpectrumToken'>SCREEN$</span>"
  s" \#171" s" <span class='ZXSpectrumToken'>ATTR</span>"
  s" \#172" s" <span class='ZXSpectrumToken'>AT</span>"
  s" \#173" s" <span class='ZXSpectrumToken'>TAB</span>"
  s" \#174" s" <span class='ZXSpectrumToken'>VAL$</span>"
  s" \#175" s" <span class='ZXSpectrumToken'>CODE</span>"
  s" \#176" s" <span class='ZXSpectrumToken'>VAL</span>"
  s" \#177" s" <span class='ZXSpectrumToken'>LEN</span>"
  s" \#178" s" <span class='ZXSpectrumToken'>SIN</span>"
  s" \#179" s" <span class='ZXSpectrumToken'>COS</span>"
  s" \#180" s" <span class='ZXSpectrumToken'>TAN</span>"
  s" \#181" s" <span class='ZXSpectrumToken'>ASN</span>"
  s" \#182" s" <span class='ZXSpectrumToken'>ACS</span>"
  s" \#183" s" <span class='ZXSpectrumToken'>ATN</span>"
  s" \#184" s" <span class='ZXSpectrumToken'>LN</span>"
  s" \#185" s" <span class='ZXSpectrumToken'>EXP</span>"
  s" \#186" s" <span class='ZXSpectrumToken'>INT</span>"
  s" \#187" s" <span class='ZXSpectrumToken'>SQR</span>"
  s" \#188" s" <span class='ZXSpectrumToken'>SGN</span>"
  s" \#189" s" <span class='ZXSpectrumToken'>ABS</span>"
  s" \#190" s" <span class='ZXSpectrumToken'>PEEK</span>"
  s" \#191" s" <span class='ZXSpectrumToken'>IN</span>"
  s" \#192" s" <span class='ZXSpectrumToken'>USR</span>"
  s" \#193" s" <span class='ZXSpectrumToken'>STR$</span>"
  s" \#194" s" <span class='ZXSpectrumToken'>CHR$</span>"
  s" \#195" s" <span class='ZXSpectrumToken'>NOT</span>"
  s" \#196" s" <span class='ZXSpectrumToken'>BIN</span>"
  s" \#197" s" <span class='ZXSpectrumToken'>OR</span>"
  s" \#198" s" <span class='ZXSpectrumToken'>AND</span>"
  s" \#199" s" <span class='ZXSpectrumToken'>&lt;=</span>"
  s" \#200" s" <span class='ZXSpectrumToken'>&gt;=</span>"
  s" \#201" s" <span class='ZXSpectrumToken'>&lt;&gt;</span>"
  s" \#202" s" <span class='ZXSpectrumToken'>LINE</span>"
  s" \#203" s" <span class='ZXSpectrumToken'>THEN</span>"
  s" \#204" s" <span class='ZXSpectrumToken'>TO</span>"
  s" \#205" s" <span class='ZXSpectrumToken'>STEP</span>"
  s" \#206" s" <span class='ZXSpectrumToken'>DEF FN</span>"
  s" \#207" s" <span class='ZXSpectrumToken'>CAT</span>"
  s" \#208" s" <span class='ZXSpectrumToken'>FORMAT</span>"
  s" \#209" s" <span class='ZXSpectrumToken'>MOVE</span>"
  s" \#210" s" <span class='ZXSpectrumToken'>ERASE</span>"
  s" \#211" s" <span class='ZXSpectrumToken'>OPEN#</span>"
  s" \#212" s" <span class='ZXSpectrumToken'>CLOSE#</span>"
  s" \#213" s" <span class='ZXSpectrumToken'>MERGE</span>"
  s" \#214" s" <span class='ZXSpectrumToken'>VERIFY</span>"
  s" \#215" s" <span class='ZXSpectrumToken'>BEEP</span>"
  s" \#216" s" <span class='ZXSpectrumToken'>CIRCLE</span>"
  s" \#217" s" <span class='ZXSpectrumToken'>INK</span>"
  s" \#218" s" <span class='ZXSpectrumToken'>PAPER</span>"
  s" \#219" s" <span class='ZXSpectrumToken'>FLASH</span>"
  s" \#220" s" <span class='ZXSpectrumToken'>BRIGHT</span>"
  s" \#221" s" <span class='ZXSpectrumToken'>INVERSE</span>"
  s" \#222" s" <span class='ZXSpectrumToken'>OVER</span>"
  s" \#223" s" <span class='ZXSpectrumToken'>OUT</span>"
  s" \#224" s" <span class='ZXSpectrumToken'>LPRINT</span>"
  s" \#225" s" <span class='ZXSpectrumToken'>LLIST</span>"
  s" \#226" s" <span class='ZXSpectrumToken'>STOP</span>"
  s" \#227" s" <span class='ZXSpectrumToken'>READ</span>"
  s" \#228" s" <span class='ZXSpectrumToken'>DATA</span>"
  s" \#229" s" <span class='ZXSpectrumToken'>RESTORE</span>"
  s" \#230" s" <span class='ZXSpectrumToken'>NEW</span>"
  s" \#231" s" <span class='ZXSpectrumToken'>BORDER</span>"
  s" \#232" s" <span class='ZXSpectrumToken'>CONTINUE</span>"
  s" \#233" s" <span class='ZXSpectrumToken'>DIM</span>"
  s" \#234" s" <span class='ZXSpectrumToken'>REM</span>"
  s" \#235" s" <span class='ZXSpectrumToken'>FOR</span>"
  s" \#236" s" <span class='ZXSpectrumToken'>GO TO</span>"
  s" \#237" s" <span class='ZXSpectrumToken'>GO SUB</span>"
  s" \#238" s" <span class='ZXSpectrumToken'>INPUT</span>"
  s" \#239" s" <span class='ZXSpectrumToken'>LOAD</span>"
  s" \#240" s" <span class='ZXSpectrumToken'>LIST</span>"
  s" \#241" s" <span class='ZXSpectrumToken'>LET</span>"
  s" \#242" s" <span class='ZXSpectrumToken'>PAUSE</span>"
  s" \#243" s" <span class='ZXSpectrumToken'>NEXT</span>"
  s" \#244" s" <span class='ZXSpectrumToken'>POKE</span>"
  s" \#245" s" <span class='ZXSpectrumToken'>PRINT</span>"
  s" \#246" s" <span class='ZXSpectrumToken'>PLOT</span>"
  s" \#247" s" <span class='ZXSpectrumToken'>RUN</span>"
  s" \#248" s" <span class='ZXSpectrumToken'>SAVE</span>"
  s" \#249" s" <span class='ZXSpectrumToken'>RANDOMIZE</span>"
  s" \#250" s" <span class='ZXSpectrumToken'>IF</span>"
  s" \#251" s" <span class='ZXSpectrumToken'>CLS</span>"
  s" \#252" s" <span class='ZXSpectrumToken'>DRAW</span>"
  s" \#253" s" <span class='ZXSpectrumToken'>CLEAR</span>"
  s" \#254" s" <span class='ZXSpectrumToken'>RETURN</span>"
  s" \#255" s" <span class='ZXSpectrumToken'>COPY</span>"
;translations

.( fendo.addon.basin_charset.fs compiled) cr
