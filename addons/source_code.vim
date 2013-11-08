" addons/source_code.vim
" Vim program used by the source code addon.

" 2013-11-07 Start; variables.
" 2013-11-08 Actual conversion to XHTML; only the code zone is left in the file.

let g:html_number_lines = 0
let g:html_no_progress = 1
let g:html_use_xhtml = 1
let g:html_ignore_folding = 1
let g:html_pre_wrap = 0
let g:html_expand_tabs = 1
let g:html_use_encoding = "UTF-8"

TOhtml


" Delete lines from the top to "<pre>":
call search('<pre>','wc')
normal mm
call cursor(1,1) " Go to the top of the file.
normal d'm
 
" Delete lines from "</pre>" to the bottom:
call search('</pre>','wc')
normal dG

wqall 
