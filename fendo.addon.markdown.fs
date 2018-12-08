.( fendo.addon.markdown.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Markdown addon. It provides words to include
\ contents in Markdown, either inline or from a file.

\ Last modified 201812080157.
\ See change log at the end of the file.

\ Copyright (C) 2015,2017 Marcos Cruz (programandala.net)

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
\ with Gforth (<http://gnu.org/software/gforth>).

\ ==============================================================
\ Requirements

\ Pandoc must be installed in the system.  See:
\ <http://johnmacfarlane.net/pandoc/>.

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`
require galope/trim.fs    \ `trim`

fendo_definitions

\ ==============================================================

package fendo.addon.markdown

s" /tmp/fendo_addon.markdown.adoc" 2dup 2constant input_file$
s" .html" s+ 2constant output_file$

\ XXX TODO -- There are similar words `>input_file` and `<output_file`
\ in <fendo.addon.source_code.common.fs>; maybe they can be shared.

: >input_file ( ca len -- )
  input_file$ w/o create-file throw
  dup >r write-file throw  r> close-file throw ;
  \ Save the given contents to the file that Markdown will load as input.
  \ ca len = text in Markdown format

: <output_file ( -- ca len )
  output_file$ slurp-file ;
  \ Get the content of the file that Markdown created as output.
  \ ca len = contents in HTML format

public

: (pandoc_markdown_base_command)$ ( -- ca len )
  s" pandoc -f markdown -t html -o " output_file$ s+ ;

: (markdown_base_command)$ ( -- ca len )
  s" markdown > " output_file$ s+ ;

defer markdown_base_command$ ( -- ca len )
  \ Base command to convert a Markdown file, not specified,
  \ and save the result in `output_file$`.
  \ The default action is the `markdown` program;
  \ `pandoc` is provided as an alternative.

' (markdown_base_command)$ is markdown_base_command$

private

: markdown_command$ ( -- ca len )
  markdown_base_command$ s"  " s+ ;
  \ Return the Markdown command without the input file.

: ((include_markdown)) ( ca1 len1 -- ca2 len2 )
  markdown_command$ s"  " s+ 2swap s+
  system $? abort" The system markdown command failed"
  <output_file ;
  \ Return the contents _ca2 len2_ converted
  \ from the Markdown file _ca1 len1_.
  \ The header and footer of the file will be ignored.

: (include_markdown) ( ca1 len1 -- ca2 len2 )
  file>local ((include_markdown)) ;
  \ Return the contents _ca2 len2_ converted
  \ from the Markdown file _ca1 len1_, which is converted to local.
  \ The header and footer of the file will be ignored.

: parse_markdown ( "ccc<}markdown>" -- ca len )
  s" "
  begin
    0 parse dup
    if    2dup trim s" }markdown" str=
          if 2drop exit else s+ true then
    else  2drop s\" \n" s+ refill
    then  0=
  until ;

public

: include_markdown ( ca1 len1 -- )
  (include_markdown) echo ;
  \ Include contents in Markdown format from the given file.
  \ The header and footer of the file will be ignored.

markup>current

: markdown{ ( "ccc<}markdown>" -- )
  parse_markdown >input_file
  input_file$ ((include_markdown)) echo ;
  \ Parse contents in Markdown format, until "}markdown" is
  \ found (on its own line). Then save them to a file, convert them to
  \ HTML and and include them into the current page.

end-package

.( fendo.addon.markdown.fs compiled) cr

\ ==============================================================
\ Change log

\ 2017-06-25: Start.
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.

\ vim: filetype=gforth
