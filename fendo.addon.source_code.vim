" fendo.addon.source_code.vim

" This file is part of Fendo.
" This Vim program is used by the source code addon.

" Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

" Fendo is free software; you can redistribute it and/or modify it
" under the terms of the GNU General Public License as published by
" the Free Software Foundation; either version 2 of the License, or
" (at your option) any later version.

" Fendo is distributed in the hope that it will be useful, but WITHOUT
" ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
" or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
" License for more details.

" You should have received a copy of the GNU General Public License
" along with this program; if not, see <http://gnu.org/licenses>.

" Fendo is written in Forth with Gforth
" (<http://www.gnu.org/software/gforth/>),
" excepting some little tools written in Vim
" (<http://www.vim.org>) like this one.

" **************************************************************
" Change history of this file

" 2013-11-07: Start; variables.
" 2013-11-08: Actual conversion to XHTML; only the code zone is left in the file.
" 2014-02-15: File header completed. 

" **************************************************************

let g:html_number_lines = 0
let g:html_no_progress = 1
let g:html_use_xhtml = 1
let g:html_ignore_folding = 1
let g:html_pre_wrap = 0
let g:html_expand_tabs = 1
let g:html_use_encoding = "UTF-8"

silent! TOhtml

" Temporary code to collect all CSS classes:
"let s:style_filename="/tmp/source_code_style_".strftime("%Y%m%d%H%M%S").".css"
"execute "silent! /<style /,/style>/write ".s:style_filename

" Delete lines from the top to "<pre>":
call search('<pre>','wc')
normal mm
call cursor(1,1) " Go to the top of the file.
silent! normal d'm
 
" Delete lines from "</pre>" to the bottom:
call search('</pre>','wc')
silent! normal dG

silent! wqall 
