" Vim syntax file
" Language: Fendo markup
" Author:   Marcos Cruz (programandala.net)
" URL:      http://programandala.net/en.program.fendo_vim_syntax_file.html
" License:  GPL (http://www.gnu.org)
" Remarks:  Vim 6 or greater
" Updated:  2019-01-10

" This file is part of Fendo,
" a static website generator written in Forth
" (http://programandala.net/en.program.fendo.html)
" by Marcos Cruz (programandala.net).

" See change log at the end of the file.

" --------------------------------------------------------------
" XXX TODO

" Fix: list bullets clashes with bold marks at the start of line.
" Fix: required position of block delimiters.

" --------------------------------------------------------------

" For version 5.x: Clear all syntax items
" For version 6.x: Quit when a syntax file was already loaded
if version < 600
    syntax clear
elseif exists("b:current_syntax")
    finish
endif

" Synchronization method
syn sync fromstart
syn sync linebreaks=1
"syn sync ccomment
"syn sync maxlines=200

let s:cpo_save = &cpo
set cpo&vim

syn case ignore

" Characters allowed in keywords

if version >= 600
    setlocal iskeyword=33-255
else
    set iskeyword=33-255
endif

" --------------------------------------------------------------

" Run :help syn-priority to review syntax matching priority.
syn keyword fendoTodo TODO FIXME CHECK TEST XXX ZZZ DEPRECATED contained
"syn keyword fendoTodo todo fixme check test xxx zzz deprecated contained
"XXXsyn match fendoBackslash /\\/
syn region fendoIdMarker start=/^\$Id:\s/ end=/\s\$$/
"XXXsyn match fendoLineBreak /[ \t]+$/
syn match fendoLineBreak /\\\\$/
syn match fendoRuler /^-\{8}$/
"syn match fendoPagebreak /^<\{3,}$/
syn match fendoEntityRef /\\\@<!&[#a-zA-Z]\S\{-};/
" FIXME: The tricky part is not triggering on indented list items that are also
" preceeded by blank line, handles only bulleted items (see 'Limitations' above
" for workarounds).
"syn region fendoLiteralParagraph start="^\n[ \t]\+\(\([^-*. \t] \)\|\(\S\S\)\)" end="\(^+\?\s*$\)\@="
"syn region fendoLiteralParagraph start=/^\s\+\S\+/ end=/\(^+\?\s*$\)\@=/
"syn match fendoURL /\\\@<!\<\(http\|https\|ftp\|file\|irc\):\/\/[^| \t]*\(\w\|\/\)/
"syn match fendoEmail /[\\.:]\@<!\(\<\|<\)\w\(\w\|[.-]\)*@\(\w\|[.-]\)*\w>\?[0-9A-Za-z_]\@!/
"syn match fendoAttributeRef /\\\@<!{\w\(\w\|-\)*\([=!@#$%?:].*\)\?}/

"syn match fendoLink /\<\[\[[ \n\t]\+\_.\{-}[ \n\t]\+]]\>/
syn match fendoLink /\<\[\[\>.\{-}\<]]\>/ contains=fendoImage
syn match fendoImage /\<{{\>.\{-}\<}}\>/

" As a damage control measure quoted patterns always terminate at a blank
" line (see 'Limitations' above).
syn match fendoQuotedSubscript /\\\@<!\~\S\_.\{-}\(\~\|\n\s*\n\)/
syn match fendoQuotedSuperscript /\\\@<!\^\S\_.\{-}\(\^\|\n\s*\n\)/

"syn match fendoQuotedMonospaced /\(^\|[| \t([.,=\-\]]\)\@<=+\([ )\n\t]\)\@!\(.\|\n\(\s*\n\)\@!\)\{-}\S\(+\([| \t)[\],.?!;:=\-]\|$\)\@=\)/
"syn match fendoQuotedMonospaced /\(^\|[| \t([.,=\-\]]\)\@<=`\([ )\n\t]\)\@!\(.\|\n\(\s*\n\)\@!\)\{-}\S\(`\([| \t)[\],.?!;:=\-]\|$\)\@=\)/
"syn match fendoQuotedUnconstrainedMonospaced /[\\+]\@<!++\S\_.\{-}\(++\|\n\s*\n\)/
syn match fendoCode /\<##[ \n\t]\+\_.\{-}[ \n\t]\+##\>/

"syn match fendoQuotedEmphasized /\(^\|[| \t([.,=\-\]]\)\@<=_\([ )\n\t]\)\@!\(.\|\n\(\s*\n\)\@!\)\{-}\S\(_\([| \t)[\],.?!;:=\-]\|$\)\@=\)/
"syn match fendoQuotedEmphasized /\(^\|[| \t([.,=\-\]]\)\@<='\([ )\n\t]\)\@!\(.\|\n\(\s*\n\)\@!\)\{-}\S\('\([| \t)[\],.?!;:=\-]\|$\)\@=\)/
"syn match fendoQuotedUnconstrainedEmphasized /\\\@<!__\S\_.\{-}\(__\|\n\s*\n\)/
syn match fendoQuotedEmphasized /\<\/\/[ \n\t]\+\_.\{-}[ \n\t]\+\/\/\>/

"syn match fendoQuotedBold /\(^\|[| \t([.,=\-\]]\)\@<=\*\([ )\n\t]\)\@!\(.\|\n\(\s*\n\)\@!\)\{-}\S\(\*\([| \t)[\],.?!;:=\-]\|$\)\@=\)/
"syn match fendoQuotedUnconstrainedBold /\\\@<!\*\*\S\_.\{-}\(\*\*\|\n\s*\n\)/
syn match fendoQuotedBold /\<\*\*[ \n\t]\+\_.\{-}[ \n\t]\+\*\*\>/

" Don't allow ` in single quoted (a kludge to stop confusion with `monospaced`).
"syn match fendoQuotedSingleQuoted /\(^\|[| \t([.,=\-]\)\@<=`\([ )\n\t]\)\@!\([^`]\|\n\(\s*\n\)\@!\)\{-}[^` \t]\('\([| \t)[\],.?!;:=\-]\|$\)\@=\)/

"syn match fendoQuotedDoubleQuoted /\(^\|[| \t([.,=\-]\)\@<=``\([ )\n\t]\)\@!\(.\|\n\(\s*\n\)\@!\)\{-}\S\(''\([| \t)[\],.?!;:=\-]\|$\)\@=\)/

"syn match fendoDoubleDollarPassthrough /\\\@<!\(^\|[^0-9a-zA-Z$]\)\@<=\$\$..\{-}\(\$\$\([^0-9a-zA-Z$]\|$\)\@=\|^$\)/
"syn match fendoTriplePlusPassthrough /\\\@<!\(^\|[^0-9a-zA-Z$]\)\@<=+++..\{-}\(+++\([^0-9a-zA-Z$]\|$\)\@=\|^$\)/

syn match fendoOneLineTitle /^\s*\(=\{1,5}\)\s\+\S.*\s\+\1\s*$/ contains=fendoQuoted.*,,fendoAttributeRef,fendoEntityRef,fendoLink,fendoBackslash

syn match fendoAttributeList /^\[[^[ \t].*\]$/
syn match fendoQuoteBlockDelimiter /^'\{4}$/

syn match fendoAdmonitionNote /^\(NOTE\|TIP\):\(\s\+.*\)\@=/
syn match fendoAdmonitionNote /^\[\(NOTE\|TIP\)\]\s*$/
syn match fendoAdmonitionWarn /^\(CAUTION\|IMPORTANT\|WARNING\):\(\s\+.*\)\@=/
syn match fendoAdmonitionWarn /^\[\(CAUTION\|IMPORTANT\|WARNING\)\]\s*$/

" See http://vimdoc.sourceforge.net/htmldoc/usr_44.html for excluding region
" contents from highlighting.
"syn match fendoTablePrefix /\(\S\@<!\(\([0-9.]\+\)\([*+]\)\)\?\([<\^>.]\{,3}\)\?\([a-z]\)\?\)\?|/ containedin=fendoTableBlock contained
syn region fendoTableBlock matchgroup=fendoTableDelimiter start=/^|===$/ end=/^|===$/ keepend contains=ALL
"syn match fendoTablePrefix /\(\S\@<!\(\([0-9.]\+\)\([*+]\)\)\?\([<\^>.]\{,3}\)\?\([a-z]\)\?\)\?!/ containedin=fendoTableBlock contained
syn match fendoTableCell /\<|\>/ containedin=fendoTableBlock contained
syn match fendoTableHeaderCell /\<|=\>/ containedin=fendoTableBlock contained
syn match fendoTableCaption /\<=|=\>/ containedin=fendoTableBlock contained

syn region fendoLiteralBlock start=/^\.\{4}$/ end=/^\.\{4}$/ keepend
syn region fendoDataBlock matchgroup=fendoDataDelimiter start=/\<data{\>/ end=/\<}data\>/ contains=fendoDatumLabel,fendoDataCommentLine
syn match fendoDatumLabel /^\s*\S\+\>/ containedin=fendoDataBlock contained
syn match fendoContentBlock /\<content{\>\|\<}content\>/
"syn region fendoContentBlock matchgroup=fendoContentDelimiter start=/\<content{\>/ end=/\<}content\>/
syn region fendoCodeBlock start=/^#\{4}[a-zA-Z0-9_-]*$/ end=/^#\{4}$/ keepend
syn region fendoForthBlock start=/\<<\[\>/ end=/\<]>\>/ keepend
" XXX OLD:
" syn region fendoListingBlock start=/^-\{4}$/ end=/^-\{4}$/ keepend
syn region fendoPassthroughBlock start="^\~\{4}$" end="^\~\{4}$"

" --------------------------------------------------------------
" Comments

syn region fendoCommentBlock start='\<(\*\>' end='\<\*)\>' contains=fendoTodo
syn match fendoCommentLine '\<\\\>.*$' contains=fendoTodo
syn match fendoCommentLine '\<#!\>.*$'
syn match fendoDataCommentLine '^\s*\\\>.*$' containedin=fendoDataBlock contained

" XXX OLD
""syn region fendoAttributeEntry start=/^:\a/ end=/:\(\s\|$\)/ oneline
"syn region fendoAttributeEntry start=/^:\w/ end=/:\(\s\|$\)/ oneline

" --------------------------------------------------------------
" Lists

" So far Fendo does not allow nested lists.

syn match fendoListBullet /^\s*\*\s\+/
syn match fendoListBullet /^\s*-\s\+/
syn match fendoListNumber /^\s*#\s\+/
" Starts with any of the above.
" syn region fendoList start=/^\s*\(-\|\*\{1,5}\)\s/ start=/^\s*\(\(\d\+\.\)\|\.\{1,5}\|\(\a\.\)\|\([ivxIVX]\+)\)\)\s\+/ start=/.\+\(:\{2,4}\|;;\)$/ end=/\(^[=*]\{4,}$\)\@=/ end=/\(^+\?\s*$\)\@=/ contains=fendoList.\+,fendoQuoted.*,fendoMacroAttributes,fendoAttributeRef,fendoEntityRef,fendoLink,fendoBackslash,fendoCommentLine,fendoAttributeList

" --------------------------------------------------------------
" Styles

highlight fendoAdmonitionNote term=reverse ctermfg=white ctermbg=green guifg=white guibg=green
highlight fendoAdmonitionWarn term=reverse ctermfg=white ctermbg=red guifg=white guibg=red
highlight fendoBackslash ctermfg=darkmagenta guifg=darkmagenta
highlight fendoBiblio term=bold ctermfg=darkcyan cterm=bold guifg=darkcyan gui=bold
highlight fendoDoubleDollarPassthrough term=underline ctermfg=darkmagenta guifg=darkmagenta
highlight fendoGlossary term=underline ctermfg=darkgreen cterm=underline guifg=darkgreen gui=underline
highlight fendoQuestion term=underline ctermfg=darkgreen cterm=underline guifg=darkgreen gui=underline
highlight fendoQuotedDoubleQuoted term=bold ctermfg=darkyellow guifg=darkyellow
highlight fendoQuotedMonospaced term=standout ctermfg=darkyellow guifg=darkyellow
highlight fendoQuotedSingleQuoted term=bold ctermfg=darkyellow guifg=darkyellow
highlight fendoQuotedSubscript term=bold ctermfg=darkyellow guifg=darkyellow
highlight fendoQuotedSuperscript term=bold ctermfg=darkyellow guifg=darkyellow
highlight fendoReference term=underline ctermfg=darkmagenta guifg=darkmagenta
highlight fendoReplacements term=standout ctermfg=darkcyan guifg=darkcyan
highlight fendoRevisionInfo term=standout ctermfg=blue guifg=darkblue gui=bold
"highlight fendoSource term=standout ctermfg=darkyellow guifg=darkyellow
"highlight fendoTripplePlusPassthrough term=underline ctermfg=darkmagenta guifg=darkmagenta

highlight fendoQuotedBold term=bold cterm=bold gui=bold
highlight fendoQuotedEmphasized term=italic cterm=italic gui=italic
"Attributes
highlight link fendoAttributeEntry Constant
highlight link fendoAttributeList Constant
highlight link fendoAttributeMacro Macro
highlight link fendoAttributeRef Constant

"Lists
highlight link fendoListBullet Label
highlight link fendoListNumber Label

" Titles
hi def link fendoOneLineTitle Title

"Links
"highlight link fendoEmail Underlined
highlight link fendoLink Underlined
highlight link fendoImage PreProc


"Blocks
"highlight fendoExampleBlockDelimiter term=standout ctermfg=darkyellow guifg=darkyellow
highlight fendoFilterBlock term=standout ctermfg=darkyellow guifg=darkyellow
" XXX OLD:
"highlight fendoListingBlock term=standout ctermfg=darkyellow guifg=darkyellow
highlight fendoLiteralBlock term=standout ctermfg=darkyellow guifg=darkyellow
"highlight fendoCodeBlock term=standout ctermfg=darkyellow guifg=darkyellow
"highlight fendoLiteralParagraph term=standout ctermfg=darkyellow guifg=darkyellow
highlight fendoQuoteBlockDelimiter term=standout ctermfg=darkyellow guifg=darkyellow

hi def link fendoCode Statement
hi def link fendoCodeBlock Statement
hi def link fendoForthBlock Special

"Tables
" XXX OLD: highlight link fendoTableBlock2 NONE
highlight link fendoTableBlock NONE
"highlight fendoTableDelimiter2 term=standout ctermfg=darkcyan guifg=darkcyan
highlight fendoTableDelimiter term=standout ctermfg=darkcyan guifg=darkcyan
" XXX OLD: highlight fendoTable_OLD term=standout ctermfg=darkyellow guifg=darkyellow
"highlight fendoTablePrefix2 term=standout ctermfg=darkcyan guifg=darkcyan
"highlight fendoTablePrefix term=standout ctermfg=darkcyan guifg=darkcyan
" XXX NEW
highlight fendoTableCell term=standout ctermfg=darkcyan guifg=darkcyan
highlight fendoTableHeaderCell term=standout ctermfg=darkcyan guifg=darkcyan
highlight fendoTableCaption term=standout ctermfg=darkcyan guifg=darkcyan

"Comments
hi default link fendoCommentBlock Comment
hi default link fendoCommentLine Comment
hi default link fendoDataCommentLine Comment

" Sections of the document
hi def link fendoDataDelimiter Special
hi def link fendoDatumLabel Special
hi def link fendoContentDelimiter Special

"Other
highlight link fendoEntityRef Special
highlight link fendoIdMarker Special
highlight link fendoLineBreak Special
"highlight link fendoPagebreak Type
highlight link fendoPassthroughBlock Identifier
highlight link fendoRuler Type
highlight link fendoToDo Todo

let b:current_syntax = "fendo"

"Show tab and trailing characters
"set listchars=tab:»·,trail:·
"set list

"
"set textwidth=78 formatoptions=tcqn autoindent
set formatoptions=tcqn

if version >= 700
    "Prevent simple numbers at the start of lines to be confused with list items:
    set formatlistpat=^\\s*\\d\\+\\.\\s\\+
endif

" XXX original:
"set comments=s1:/*,ex:*/,://,b:#,:%,fb:-,fb:*,fb:.,fb:+,fb:>
" XXX 2013-12-30, modifed by programandala.net:
set comments=://,fb:-,fb:*,fb:.,fb:+,fb:>
" (copied to ~/.vim/ftplugin/fendo.vim)

" nnoremap Q gq}

" --------------------------------------------------------------
" Change log

" 2015-01-01: Start, based on the Vim Syntax File for AsciiDoc.
"
" 2015-01-13: Comments, titles, code, data block, content block, bold,
" italic...
"
" 2015-01-18: Links.
"
" 2015-01-19: Images.
"
" 2015-01-30: Fix: removed the AsciiDoc line comment.
"
" 2015-01-30: Fix: lists; old code is removed.
"
" 2015-02-06: Fix: now 'fendoForthBlock' start and end are isolated words.
"
" 2015-02-12: Removed the useless 'fendoListingBlock'.
"
" 2015-01-14: Fix: now the title markup can have spaces at the start and at
" the end of the line.
"
" 2015-02-15: Fix: several matchs of code block markups were not limited to 4
" characters.
"
" 2015-02-19: Fix: fendoCodeBlock's start regexp.
"
" 2015-02-26: Vim license.
"
" 2015-09-19: Commented out `set list`.
"
" 2015-09-26: Fixed typo in the table delimiter patterns. Activated the
" highlighting of table delimiters, that was commented out.
"
" 2017-10-04: Update bullet lists with "-".
"
" 2019-01-05: Highlight the metadata field labels.
"
" 2019-01-10: Highlight line comments in data block.

" ----------------------------------------------
