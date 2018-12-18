.( fendo.markup.html.entities.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the HTML entities.

\ Last modified 201812181312.
\ See change log at the end of the file.

\ Copyright (C) 2013,2017,2018 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Defining words

: :entity ( ca len -- )
  get-current >r entities>current
  :echo_name
  separate? off  \ XXX why?
  r> set-current ;
  \ Create a HTML entity word.
  \ ca len = entity --and name of its entity word

: entity: ( "name" -- )
  parse-name? abort" Parseable name expected in `entity:`"
  :entity ;
  \ Create a HTML entity word.
  \ "name" = entity --and name of its entity word

\ ==============================================================
\ HTML entities

\ entity comment: \ Character \ Unicode code point (decimal) \ Standard DTD \ Old ISO subset \ Description

entity: &quot; \ " \ U+0022 (34) \ HTML 2.0 \ HTMLspecial \ ISOnum \ quotation mark (= APL quote)
entity: &amp; \ & \ U+0026 (38) \ HTML 2.0 \ HTMLspecial \ ISOnum \ ampersand
entity: &apos; \ ' \ U+0027 (39) \ XHTML 1.0 \ HTMLspecial \ ISOnum \ apostrophe (= apostrophe-quote); see below
entity: &lt; \ < \ U+003C (60) \ HTML 2.0 \ HTMLspecial \ ISOnum \ less-than sign
entity: &gt; \ > \ U+003E (62) \ HTML 2.0 \ HTMLspecial \ ISOnum \ greater-than sign
entity: &nbsp; \   \ U+00A0 (160) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ no-break space (= non-breaking space)
entity: &iexcl; \ ¡ \ U+00A1 (161) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ inverted exclamation mark
entity: &cent; \ ¢ \ U+00A2 (162) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ cent sign
entity: &pound; \ £ \ U+00A3 (163) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ pound sign
entity: &curren; \ ¤ \ U+00A4 (164) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ currency sign
entity: &yen; \ ¥ \ U+00A5 (165) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ yen sign (= yuan sign)
entity: &brvbar; \ ¦ \ U+00A6 (166) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ broken bar (= broken vertical bar)
entity: &sect; \ § \ U+00A7 (167) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ section sign
entity: &uml; \ ¨ \ U+00A8 (168) \ HTML 3.2 \ HTMLlat1 \ ISOdia \ diaeresis (= spacing diaeresis); see Germanic umlaut
entity: &copy; \ © \ U+00A9 (169) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ copyright symbol
entity: &ordf; \ ª \ U+00AA (170) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ feminine ordinal indicator
entity: &laquo; \ « \ U+00AB (171) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ left-pointing double angle quotation mark (= left pointing guillemet)
entity: &not; \ ¬ \ U+00AC (172) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ not sign
entity: &shy; \  \ U+00AD (173) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ soft hyphen (= discretionary hyphen)
entity: &reg; \ ® \ U+00AE (174) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ registered sign ( = registered trademark symbol)
entity: &macr; \ ¯ \ U+00AF (175) \ HTML 3.2 \ HTMLlat1 \ ISOdia \ macron (= spacing macron = overline = APL overbar)
entity: &deg; \ ° \ U+00B0 (176) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ degree symbol
entity: &plusmn; \ ± \ U+00B1 (177) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ plus-minus sign (= plus-or-minus sign)
entity: &sup2; \ ² \ U+00B2 (178) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ superscript two (= superscript digit two = squared)
entity: &sup3; \ ³ \ U+00B3 (179) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ superscript three (= superscript digit three = cubed)
entity: &acute; \ ´ \ U+00B4 (180) \ HTML 3.2 \ HTMLlat1 \ ISOdia \ acute accent (= spacing acute)
entity: &micro; \ µ \ U+00B5 (181) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ micro sign
entity: &para; \ ¶ \ U+00B6 (182) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ pilcrow sign ( = paragraph sign)
entity: &middot; \ · \ U+00B7 (183) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ middle dot (= Georgian comma = Greek middle dot)
entity: &cedil; \ ¸ \ U+00B8 (184) \ HTML 3.2 \ HTMLlat1 \ ISOdia \ cedilla (= spacing cedilla)
entity: &sup1; \ ¹ \ U+00B9 (185) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ superscript one (= superscript digit one)
entity: &ordm; \ º \ U+00BA (186) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ masculine ordinal indicator
entity: &raquo; \ » \ U+00BB (187) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ right-pointing double angle quotation mark (= right pointing guillemet)
entity: &frac14; \ ¼ \ U+00BC (188) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ vulgar fraction one quarter (= fraction one quarter)
entity: &frac12; \ ½ \ U+00BD (189) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ vulgar fraction one half (= fraction one half)
entity: &frac34; \ ¾ \ U+00BE (190) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ vulgar fraction three quarters (= fraction three quarters)
entity: &iquest; \ ¿ \ U+00BF (191) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ inverted question mark (= turned question mark)
entity: &Agrave; \ À \ U+00C0 (192) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter A with grave accent (= Latin capital letter A grave)
entity: &Aacute; \ Á \ U+00C1 (193) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter A with acute accent
entity: &Acirc; \ Â \ U+00C2 (194) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter A with circumflex
entity: &Atilde; \ Ã \ U+00C3 (195) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter A with tilde
entity: &Auml; \ Ä \ U+00C4 (196) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter A with diaeresis
entity: &Aring; \ Å \ U+00C5 (197) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter A with ring above (= Latin capital letter A ring)
entity: &AElig; \ Æ \ U+00C6 (198) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter AE (= Latin capital ligature AE)
entity: &Ccedil; \ Ç \ U+00C7 (199) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter C with cedilla
entity: &Egrave; \ È \ U+00C8 (200) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter E with grave accent
entity: &Eacute; \ É \ U+00C9 (201) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter E with acute accent
entity: &Ecirc; \ Ê \ U+00CA (202) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter E with circumflex
entity: &Euml; \ Ë \ U+00CB (203) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter E with diaeresis
entity: &Igrave; \ Ì \ U+00CC (204) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter I with grave accent
entity: &Iacute; \ Í \ U+00CD (205) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter I with acute accent
entity: &Icirc; \ Î \ U+00CE (206) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter I with circumflex
entity: &Iuml; \ Ï \ U+00CF (207) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter I with diaeresis
entity: &ETH; \ Ð \ U+00D0 (208) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter Eth
entity: &Ntilde; \ Ñ \ U+00D1 (209) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter N with tilde
entity: &Ograve; \ Ò \ U+00D2 (210) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter O with grave accent
entity: &Oacute; \ Ó \ U+00D3 (211) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter O with acute accent
entity: &Ocirc; \ Ô \ U+00D4 (212) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter O with circumflex
entity: &Otilde; \ Õ \ U+00D5 (213) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter O with tilde
entity: &Ouml; \ Ö \ U+00D6 (214) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter O with diaeresis
entity: &times; \ × \ U+00D7 (215) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ multiplication sign
entity: &Oslash; \ Ø \ U+00D8 (216) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter O with stroke (= Latin capital letter O slash)
entity: &Ugrave; \ Ù \ U+00D9 (217) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter U with grave accent
entity: &Uacute; \ Ú \ U+00DA (218) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter U with acute accent
entity: &Ucirc; \ Û \ U+00DB (219) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter U with circumflex
entity: &Uuml; \ Ü \ U+00DC (220) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter U with diaeresis
entity: &Yacute; \ Ý \ U+00DD (221) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter Y with acute accent
entity: &THORN; \ Þ \ U+00DE (222) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin capital letter THORN
entity: &szlig; \ ß \ U+00DF (223) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter sharp s (= ess-zed); see German Eszett
entity: &agrave; \ à \ U+00E0 (224) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter a with grave accent
entity: &aacute; \ á \ U+00E1 (225) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter a with acute accent
entity: &acirc; \ â \ U+00E2 (226) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter a with circumflex
entity: &atilde; \ ã \ U+00E3 (227) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter a with tilde
entity: &auml; \ ä \ U+00E4 (228) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter a with diaeresis
entity: &aring; \ å \ U+00E5 (229) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter a with ring above
entity: &aelig; \ æ \ U+00E6 (230) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter ae (= Latin small ligature ae)
entity: &ccedil; \ ç \ U+00E7 (231) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter c with cedilla
entity: &egrave; \ è \ U+00E8 (232) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter e with grave accent
entity: &eacute; \ é \ U+00E9 (233) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter e with acute accent
entity: &ecirc; \ ê \ U+00EA (234) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter e with circumflex
entity: &euml; \ ë \ U+00EB (235) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter e with diaeresis
entity: &igrave; \ ì \ U+00EC (236) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter i with grave accent
entity: &iacute; \ í \ U+00ED (237) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter i with acute accent
entity: &icirc; \ î \ U+00EE (238) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter i with circumflex
entity: &iuml; \ ï \ U+00EF (239) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter i with diaeresis
entity: &eth; \ ð \ U+00F0 (240) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter eth
entity: &ntilde; \ ñ \ U+00F1 (241) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter n with tilde
entity: &ograve; \ ò \ U+00F2 (242) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter o with grave accent
entity: &oacute; \ ó \ U+00F3 (243) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter o with acute accent
entity: &ocirc; \ ô \ U+00F4 (244) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter o with circumflex
entity: &otilde; \ õ \ U+00F5 (245) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter o with tilde
entity: &ouml; \ ö \ U+00F6 (246) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter o with diaeresis
entity: &divide; \ ÷ \ U+00F7 (247) \ HTML 3.2 \ HTMLlat1 \ ISOnum \ division sign (= obelus)
entity: &oslash; \ ø \ U+00F8 (248) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter o with stroke (= Latin small letter o slash)
entity: &ugrave; \ ù \ U+00F9 (249) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter u with grave accent
entity: &uacute; \ ú \ U+00FA (250) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter u with acute accent
entity: &ucirc; \ û \ U+00FB (251) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter u with circumflex
entity: &uuml; \ ü \ U+00FC (252) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter u with diaeresis
entity: &yacute; \ ý \ U+00FD (253) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter y with acute accent
entity: &thorn; \ þ \ U+00FE (254) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter thorn
entity: &yuml; \ ÿ \ U+00FF (255) \ HTML 2.0 \ HTMLlat1 \ ISOlat1 \ Latin small letter y with diaeresis
entity: &OElig; \ Œ \ U+0152 (338) \ HTML 4.0 \ HTMLspecial \ ISOlat2 \ Latin capital ligature oe
entity: &oelig; \ œ \ U+0153 (339) \ HTML 4.0 \ HTMLspecial \ ISOlat2 \ Latin small ligature oe
entity: &Scaron; \ Š \ U+0160 (352) \ HTML 4.0 \ HTMLspecial \ ISOlat2 \ Latin capital letter s with caron
entity: &scaron; \ š \ U+0161 (353) \ HTML 4.0 \ HTMLspecial \ ISOlat2 \ Latin small letter s with caron
entity: &Yuml; \ Ÿ \ U+0178 (376) \ HTML 4.0 \ HTMLspecial \ ISOlat2 \ Latin capital letter y with diaeresis
entity: &fnof; \ ƒ \ U+0192 (402) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ Latin small letter f with hook (= function = florin)
entity: &circ; \ ˆ \ U+02C6 (710) \ HTML 4.0 \ HTMLspecial \ ISOpub \ modifier letter circumflex accent
entity: &tilde; \ ˜ \ U+02DC (732) \ HTML 4.0 \ HTMLspecial \ ISOdia \ small tilde
entity: &Alpha; \ Α \ U+0391 (913) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Alpha
entity: &Beta; \ Β \ U+0392 (914) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Beta
entity: &Gamma; \ Γ \ U+0393 (915) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Gamma
entity: &Delta; \ Δ \ U+0394 (916) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Delta
entity: &Epsilon; \ Ε \ U+0395 (917) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Epsilon
entity: &Zeta; \ Ζ \ U+0396 (918) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Zeta
entity: &Eta; \ Η \ U+0397 (919) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Eta
entity: &Theta; \ Θ \ U+0398 (920) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Theta
entity: &Iota; \ Ι \ U+0399 (921) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Iota
entity: &Kappa; \ Κ \ U+039A (922) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Kappa
entity: &Lambda; \ Λ \ U+039B (923) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Lambda
entity: &Mu; \ Μ \ U+039C (924) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Mu
entity: &Nu; \ Ν \ U+039D (925) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Nu
entity: &Xi; \ Ξ \ U+039E (926) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Xi
entity: &Omicron; \ Ο \ U+039F (927) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Omicron
entity: &Pi; \ Π \ U+03A0 (928) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Pi
entity: &Rho; \ Ρ \ U+03A1 (929) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Rho
entity: &Sigma; \ Σ \ U+03A3 (931) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Sigma
entity: &Tau; \ Τ \ U+03A4 (932) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Tau
entity: &Upsilon; \ Υ \ U+03A5 (933) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Upsilon
entity: &Phi; \ Φ \ U+03A6 (934) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Phi
entity: &Chi; \ Χ \ U+03A7 (935) \ HTML 4.0 \ HTMLsymbol \  \ Greek capital letter Chi
entity: &Psi; \ Ψ \ U+03A8 (936) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Psi
entity: &Omega; \ Ω \ U+03A9 (937) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek capital letter Omega
entity: &alpha; \ α \ U+03B1 (945) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter alpha
entity: &beta; \ β \ U+03B2 (946) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter beta
entity: &gamma; \ γ \ U+03B3 (947) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter gamma
entity: &delta; \ δ \ U+03B4 (948) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter delta
entity: &epsilon; \ ε \ U+03B5 (949) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter epsilon
entity: &zeta; \ ζ \ U+03B6 (950) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter zeta
entity: &eta; \ η \ U+03B7 (951) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter eta
entity: &theta; \ θ \ U+03B8 (952) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter theta
entity: &iota; \ ι \ U+03B9 (953) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter iota
entity: &kappa; \ κ \ U+03BA (954) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter kappa
entity: &lambda; \ λ \ U+03BB (955) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter lambda
entity: &mu; \ μ \ U+03BC (956) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter mu
entity: &nu; \ ν \ U+03BD (957) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter nu
entity: &xi; \ ξ \ U+03BE (958) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter xi
entity: &omicron; \ ο \ U+03BF (959) \ HTML 4.0 \ HTMLsymbol \ NEW \ Greek small letter omicron
entity: &pi; \ π \ U+03C0 (960) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter pi
entity: &rho; \ ρ \ U+03C1 (961) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter rho
entity: &sigmaf; \ ς \ U+03C2 (962) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter final sigma
entity: &sigma; \ σ \ U+03C3 (963) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter sigma
entity: &tau; \ τ \ U+03C4 (964) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter tau
entity: &upsilon; \ υ \ U+03C5 (965) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter upsilon
entity: &phi; \ φ \ U+03C6 (966) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter phi
entity: &chi; \ χ \ U+03C7 (967) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter chi
entity: &psi; \ ψ \ U+03C8 (968) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter psi
entity: &omega; \ ω \ U+03C9 (969) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek small letter omega
entity: &thetasym; \ ϑ \ U+03D1 (977) \ HTML 4.0 \ HTMLsymbol \ NEW \ Greek theta symbol
entity: &upsih; \ ϒ \ U+03D2 (978) \ HTML 4.0 \ HTMLsymbol \ NEW \ Greek Upsilon with hook symbol
entity: &piv; \ ϖ \ U+03D6 (982) \ HTML 4.0 \ HTMLsymbol \ ISOgrk3 \ Greek pi symbol
entity: &ensp; \   \ U+2002 (8194) \ HTML 4.0 \ HTMLspecial \ ISOpub \ en space
entity: &emsp; \   \ U+2003 (8195) \ HTML 4.0 \ HTMLspecial \ ISOpub \ em space
entity: &thinsp; \   \ U+2009 (8201) \ HTML 4.0 \ HTMLspecial \ ISOpub \ thin space
entity: &zwnj; \   \ U+200C (8204) \ HTML 4.0 \ HTMLspecial \ NEW RFC 2070 \ zero-width non-joiner
entity: &zwj; \   \ U+200D (8205) \ HTML 4.0 \ HTMLspecial \ NEW RFC 2070 \ zero-width joiner
entity: &lrm; \   \ U+200E (8206) \ HTML 4.0 \ HTMLspecial \ NEW RFC 2070 \ left-to-right mark
entity: &rlm; \   \ U+200F (8207) \ HTML 4.0 \ HTMLspecial \ NEW RFC 2070 \ right-to-left mark
entity: &ndash; \ – \ U+2013 (8211) \ HTML 4.0 \ HTMLspecial \ ISOpub \ en dash
entity: &mdash; \ — \ U+2014 (8212) \ HTML 4.0 \ HTMLspecial \ ISOpub \ em dash
entity: &lsquo; \ ‘ \ U+2018 (8216) \ HTML 4.0 \ HTMLspecial \ ISOnum \ left single quotation mark
entity: &rsquo; \ ’ \ U+2019 (8217) \ HTML 4.0 \ HTMLspecial \ ISOnum \ right single quotation mark
entity: &sbquo; \ ‚ \ U+201A (8218) \ HTML 4.0 \ HTMLspecial \ NEW \ single low-9 quotation mark
entity: &ldquo; \ “ \ U+201C (8220) \ HTML 4.0 \ HTMLspecial \ ISOnum \ left double quotation mark
entity: &rdquo; \ ” \ U+201D (8221) \ HTML 4.0 \ HTMLspecial \ ISOnum \ right double quotation mark
entity: &bdquo; \ „ \ U+201E (8222) \ HTML 4.0 \ HTMLspecial \ NEW \ double low-9 quotation mark
entity: &dagger; \ † \ U+2020 (8224) \ HTML 4.0 \ HTMLspecial \ ISOpub \ dagger, obelisk
entity: &Dagger; \ ‡ \ U+2021 (8225) \ HTML 4.0 \ HTMLspecial \ ISOpub \ double dagger, double obelisk
entity: &bull; \ • \ U+2022 (8226) \ HTML 4.0 \ HTMLspecial \ ISOpub \ bullet (= black small circle)
entity: &hellip; \ … \ U+2026 (8230) \ HTML 4.0 \ HTMLsymbol \ ISOpub \ horizontal ellipsis (= three dot leader)
entity: &permil; \ ‰ \ U+2030 (8240) \ HTML 4.0 \ HTMLspecial \ ISOtech \ per mille sign
entity: &prime; \ ′ \ U+2032 (8242) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ prime (= minutes = feet)
entity: &Prime; \ ″ \ U+2033 (8243) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ double prime (= seconds = inches)
entity: &lsaquo; \ ‹ \ U+2039 (8249) \ HTML 4.0 \ HTMLspecial \ ISO proposed \ single left-pointing angle quotation mark
entity: &rsaquo; \ › \ U+203A (8250) \ HTML 4.0 \ HTMLspecial \ ISO proposed \ single right-pointing angle quotation mark
entity: &oline; \ ‾ \ U+203E (8254) \ HTML 4.0 \ HTMLsymbol \ NEW \ overline (= spacing overscore)
entity: &frasl; \ ⁄ \ U+2044 (8260) \ HTML 4.0 \ HTMLsymbol \ NEW \ fraction slash (= solidus)
entity: &euro; \ € \ U+20AC (8364) \ HTML 4.0 \ HTMLspecial \ NEW \ euro sign
entity: &image; \ ℑ \ U+2111 (8465) \ HTML 4.0 \ HTMLsymbol \ ISOamso \ black-letter capital I (= imaginary part)
entity: &weierp; \ ℘ \ U+2118 (8472) \ HTML 4.0 \ HTMLsymbol \ ISOamso \ script capital P (= power set = Weierstrass p)
entity: &real; \ ℜ \ U+211C (8476) \ HTML 4.0 \ HTMLsymbol \ ISOamso \ black-letter capital R (= real part symbol)
entity: &trade; \ ™ \ U+2122 (8482) \ HTML 4.0 \ HTMLsymbol \ ISOnum \ trademark symbol
entity: &alefsym; \ ℵ \ U+2135 (8501) \ HTML 4.0 \ HTMLsymbol \ NEW \ alef symbol (= first transfinite cardinal)
entity: &larr; \ ← \ U+2190 (8592) \ HTML 4.0 \ HTMLsymbol \ ISOnum \ leftwards arrow
entity: &uarr; \ ↑ \ U+2191 (8593) \ HTML 4.0 \ HTMLsymbol \ ISOnum \ upwards arrow
entity: &rarr; \ → \ U+2192 (8594) \ HTML 4.0 \ HTMLsymbol \ ISOnum \ rightwards arrow
entity: &darr; \ ↓ \ U+2193 (8595) \ HTML 4.0 \ HTMLsymbol \ ISOnum \ downwards arrow
entity: &harr; \ ↔ \ U+2194 (8596) \ HTML 4.0 \ HTMLsymbol \ ISOamsa \ left right arrow
entity: &crarr; \ ↵ \ U+21B5 (8629) \ HTML 4.0 \ HTMLsymbol \ NEW \ downwards arrow with corner leftwards (= carriage return)
entity: &lArr; \ ⇐ \ U+21D0 (8656) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ leftwards double arrow
entity: &uArr; \ ⇑ \ U+21D1 (8657) \ HTML 4.0 \ HTMLsymbol \ ISOamsa \ upwards double arrow
entity: &rArr; \ ⇒ \ U+21D2 (8658) \ HTML 4.0 \ HTMLsymbol \ ISOnum \ rightwards double arrow
entity: &dArr; \ ⇓ \ U+21D3 (8659) \ HTML 4.0 \ HTMLsymbol \ ISOamsa \ downwards double arrow
entity: &hArr; \ ⇔ \ U+21D4 (8660) \ HTML 4.0 \ HTMLsymbol \ ISOamsa \ left right double arrow
entity: &forall; \ ∀ \ U+2200 (8704) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ for all
entity: &part; \ ∂ \ U+2202 (8706) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ partial differential
entity: &exist; \ ∃ \ U+2203 (8707) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ there exists
entity: &empty; \ ∅ \ U+2205 (8709) \ HTML 4.0 \ HTMLsymbol \ ISOamso \ empty set (= null set = diameter)
entity: &nabla; \ ∇ \ U+2207 (8711) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ nabla (= backward difference)
entity: &isin; \ ∈ \ U+2208 (8712) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ element of
entity: &notin; \ ∉ \ U+2209 (8713) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ not an element of
entity: &ni; \ ∋ \ U+220B (8715) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ contains as member
entity: &prod; \ ∏ \ U+220F (8719) \ HTML 4.0 \ HTMLsymbol \ ISOamsb \ n-ary product (= product sign)
entity: &sum; \ ∑ \ U+2211 (8721) \ HTML 4.0 \ HTMLsymbol \ ISOamsb \ n-ary summation
entity: &minus; \ − \ U+2212 (8722) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ minus sign
entity: &lowast; \ ∗ \ U+2217 (8727) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ asterisk operator
entity: &radic; \ √ \ U+221A (8730) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ square root (= radical sign)
entity: &prop; \ ∝ \ U+221D (8733) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ proportional to
entity: &infin; \ ∞ \ U+221E (8734) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ infinity
entity: &ang; \ ∠ \ U+2220 (8736) \ HTML 4.0 \ HTMLsymbol \ ISOamso \ angle
entity: &and; \ ∧ \ U+2227 (8743) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ logical and (= wedge)
entity: &or; \ ∨ \ U+2228 (8744) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ logical or (= vee)
entity: &cap; \ ∩ \ U+2229 (8745) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ intersection (= cap)
entity: &cup; \ ∪ \ U+222A (8746) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ union (= cup)
entity: &int; \ ∫ \ U+222B (8747) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ integral
entity: &there4; \ ∴ \ U+2234 (8756) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ therefore sign
entity: &sim; \ ∼ \ U+223C (8764) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ tilde operator (= varies with = similar to)
entity: &cong; \ ≅ \ U+2245 (8773) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ congruent to
entity: &asymp; \ ≈ \ U+2248 (8776) \ HTML 4.0 \ HTMLsymbol \ ISOamsr \ almost equal to (= asymptotic to)
entity: &ne; \ ≠ \ U+2260 (8800) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ not equal to
entity: &equiv; \ ≡ \ U+2261 (8801) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ identical to; sometimes used for 'equivalent to'
entity: &le; \ ≤ \ U+2264 (8804) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ less-than or equal to
entity: &ge; \ ≥ \ U+2265 (8805) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ greater-than or equal to
entity: &sub; \ ⊂ \ U+2282 (8834) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ subset of
entity: &sup; \ ⊃ \ U+2283 (8835) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ superset of
entity: &nsub; \ ⊄ \ U+2284 (8836) \ HTML 4.0 \ HTMLsymbol \ ISOamsn \ not a subset of
entity: &sube; \ ⊆ \ U+2286 (8838) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ subset of or equal to
entity: &supe; \ ⊇ \ U+2287 (8839) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ superset of or equal to
entity: &oplus; \ ⊕ \ U+2295 (8853) \ HTML 4.0 \ HTMLsymbol \ ISOamsb \ circled plus (= direct sum)
entity: &otimes; \ ⊗ \ U+2297 (8855) \ HTML 4.0 \ HTMLsymbol \ ISOamsb \ circled times (= vector product)
entity: &perp; \ ⊥ \ U+22A5 (8869) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ up tack (= orthogonal to = perpendicular)
entity: &sdot; \ ⋅ \ U+22C5 (8901) \ HTML 4.0 \ HTMLsymbol \ ISOamsb \ dot operator
entity: &vellip; \ ⋮ \ U+22EE (8942) \ HTML 5.0 \ <b>?</b> \ <b>?</b> \ vertical ellipsis
entity: &lceil; \ ⌈ \ U+2308 (8968) \ HTML 4.0 \ HTMLsymbol \ ISOamsc \ left ceiling (= APL upstile)
entity: &rceil; \ ⌉ \ U+2309 (8969) \ HTML 4.0 \ HTMLsymbol \ ISOamsc \ right ceiling
entity: &lfloor; \ ⌊ \ U+230A (8970) \ HTML 4.0 \ HTMLsymbol \ ISOamsc \ left floor (= APL downstile)
entity: &rfloor; \ ⌋ \ U+230B (8971) \ HTML 4.0 \ HTMLsymbol \ ISOamsc \ right floor
entity: &lang; \ 〈 \ U+2329 (9001) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ left-pointing angle bracket (= bra)
entity: &rang; \ 〉 \ U+232A (9002) \ HTML 4.0 \ HTMLsymbol \ ISOtech \ right-pointing angle bracket (= ket)
entity: &loz; \ ◊ \ U+25CA (9674) \ HTML 4.0 \ HTMLsymbol \ ISOpub \ lozenge
entity: &spades; \ ♠ \ U+2660 (9824) \ HTML 4.0 \ HTMLsymbol \ ISOpub \ black spade suit
entity: &clubs; \ ♣ \ U+2663 (9827) \ HTML 4.0 \ HTMLsymbol \ ISOpub \ black club suit (= shamrock)
entity: &hearts; \ ♥ \ U+2665 (9829) \ HTML 4.0 \ HTMLsymbol \ ISOpub \ black heart suit (= valentine)
entity: &diams; \ ♦ \ U+2666 (9830) \ HTML 4.0 \ HTMLsymbol \ ISOpub \ black diamond suit

.( fendo.markup.html.entities.fs compiled) cr

\ ==============================================================
\ Change log

\ 2013-06-10: Factored from <fendo_markup_html.fs> and completed.
\ Reference:
\ https://en.wikipedia.org/wiki/List_of_XML_and_HTML_character_entity_references
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2018-12-18: Fix the position of semicolon in several entities.

\ vim: filetype=gforth
