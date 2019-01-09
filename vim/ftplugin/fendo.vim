" ~/.vim/ftplugin/fendo.vim

" By Marcos Cruz (programandala.net), 2015, 2017

" Vim filetype plugin for Fendo

" See change log at the end of the file.

"if exists("b:did_ftplugin")
"  finish
"endif
"let b:did_ftplugin = 1

" ==============================================================
" Generic

setlocal textwidth=79

" ==============================================================
" Search

setlocal ignorecase
setlocal smartcase

" ==============================================================
" Tabs

setlocal tabstop=2
setlocal softtabstop=0
setlocal shiftwidth=2
setlocal expandtab

" ==============================================================
" Comments

setlocal comments=b:\\

" Used by the Vim-Commentary plugin:
setlocal commentstring=\\\ %s

" ==============================================================
" Format options

setlocal smartindent

"setlocal formatoptions-=t
"setlocal formatoptions+=cqor
"setlocal formatoptions-=r
setlocal formatoptions=cqorjn
" Note: The "j" flag removes a comment leader when joining lines.
" See ":help fo-table".

setlocal formatlistpat=^\\s*[#-*]\\s\\+
" XXX FIXME -- "-" does not work

" lists (not nested):
setlocal comments+=fb:*,fb:#

" ==============================================================
" Change log

" 2015-01-30: First version, based on <~/.vim/ftplugin/gforth.vim>
"
" 2015-02-11: New: 'formatlistpat'; 'formatoptions' updated; code reorganized
" into sections.
"
" 2015-02-24: Draft for lists, from AsciiDoc. Not working yet.
"
" 2017-10-04: Improve the pattern of `formatlistpat`. Update source style.

