.( fendo.addon.asciidoctor.fs) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file is the Asciidoctor addon. It provides words to include
\ contents in Asciidoctor (or AsciiDoc) format, either inline or from
\ a file.

\ Last modified 201901271946.
\ See change log at the end of the file.

\ Copyright (C) 2015,2017,2018,2019 Marcos Cruz (programandala.net)

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

\ Asciidoctor must be installed in the system.  See:
\ <http://asciidoctor.org>.
\
\ The original AsciiDoc engine could be used instead, but this file
\ should be adapted.  Support for AsciidDoc is planned.  See:
\ <http://asciidoc.org>.

forth_definitions

require galope/package.fs \ `package`, `private`, `public`, `end-package`
require galope/trim.fs    \ `trim`

fendo_definitions

\ ==============================================================

package fendo.addon.asciidoctor

: input_file$ ( -- ca len )
  source_dir $@ current_page source_file s" .adoc" s+ s+ ;
  \ Return filename _ca len_ of the temporary file that
  \ contains the Asciidoctor code to be converted to HTML.

: output_file$ ( -- ca len )
  input_file$ s" .html" s+ ;
  \ Return filename _ca len_ of the temporary file that
  \ contains the HTML output of the Asciidoctor code, to be
  \ included in the page.

: >input_file ( ca len -- )
  input_file$ w/o create-file throw
  dup >r write-file throw  r> close-file throw ;
  \ Save the given contents to the file that Asciidoctor will load as input.
  \ ca len = text in Asciidoctor format

: <output_file ( -- ca len )
  output_file$ slurp-file ;
  \ Get the content of the file that Asciidoctor created as output.
  \ ca len = contents in HTML format

\ XXX TODO Use also AsciiDoc as alternative. Maybe `asciidoc_command`
\ and a defered `adoc_command`.

\ The Asciidoctor command includes all the required options, even when
\ they are the default; just in case the deafaults change in the
\ future, and also for the sake of clarity:
: asciidoctor_base_command$ ( -- ca len )
  s" asciidoctor "
  s" --backend html5 " s+     \ default
  s" --doctype article " s+   \ default
  s" --no-header-footer " s+  \ supress the document header and footer
                              \ in the output
  \ s" --compact " s+         \ remove blank lines in the output
  s" --out-file " s+ output_file$ s+ ;

\ XXX FIXME -- 2018-08-20: Asciidoctor 1.5.7.1 throws error
\ "--compact" is not accepted. But it's still in the documentation.

: asciidoctor_command$ ( -- ca len )
  asciidoctor_base_command$ s"  " s+ ;
  \ Return the Asciidoctor command without the input file.

: ((include_asciidoctor)) ( ca1 len1 -- ca2 len2 )
  asciidoctor_command$ s"  " s+ 2swap s+
  system $? abort" The system asciidoctor command failed"
  <output_file ;
  \ Return the contents _ca2 len2_ converted
  \ from the Asciidoctor file _ca1 len1_.
  \ The header and footer of the file will be ignored.

: (include_asciidoctor) ( ca1 len1 -- ca2 len2 )
  file>local ((include_asciidoctor)) ;
  \ Return the contents _ca2 len2_ converted
  \ from the Asciidoctor file _ca1 len1_, which is converted to local.
  \ The header and footer of the file will be ignored.

: parse_asciidoctor ( "ccc<}asciidoctor>" -- ca len )
  s" "
  begin
    0 parse dup
    if    2dup trim s" }asciidoctor" str=
          if 2drop exit else s+ true then
    else  2drop s\" \n" s+ refill
    then  0=
  until ;

public

: include_asciidoctor ( ca len -- )
  (include_asciidoctor) echo
  output_file$ delete-file throw ;

  \ doc{
  \
  \ include_asciidoctor ( ca len -- )
  \
  \ Include contents in Asciidoctor format from file _ca len_.  The
  \ header and footer of the file will be ignored.
  \
  \ See: `asciidoctor{`, `include_markdown`.
  \
  \ }doc

markup>current

: asciidoctor{ ( "ccc<}asciidoctor>" -- )
  \ ." in asciidoctor{ input_file$ = " input_file$ type key drop \ XXX INFORMER
  parse_asciidoctor >input_file
  input_file$ ((include_asciidoctor)) echo
  input_file$  delete-file throw
  output_file$ delete-file throw ;

  \ doc{
  \
  \ asciidoctor{ ( "ccc<}asciidoctor>" -- )
  \
  \ Parse contents in Asciidoctor format, until "}asciidoctor" is
  \ found (on its own line). Then save the contents to a temporary
  \ file, convert it to HTML and include the result into the current
  \ page.
  \
  \ See: `include_asciidoctor`, `markdown{`.
  \
  \ }doc

end-package

.( fendo.addon.asciidoctor.fs compiled) cr

\ ==============================================================
\ Change log

\ 2015-02-11: Start. `include_asciidoctor` works.
\
\ 2015-09-03: Added `file>local` to `include_asciidoctor`.
\
\ 2017-02-10: Factor `(include_asciidoctor)` from
\ `include_asciidoctor`, to reuse it for README.adoc files, which need
\ additional operations.
\
\ 2017-06-22: Update source style, layout and header.
\
\ 2017-06-24: Add `asciidoctor{`.
\
\ 2018-08-20: Deactivate the Asciidoctor option "--compact".
\
\ 2018-09-27: Use `package` instead of `module:`.
\
\ 2018-12-08: Update notation of Forth words in comments and strings.
\
\ 2019-01-19: Improve documentation. Use temporary files using the
\ source page filename with added extensions. This way the Asciidoctor
\ `include::` comand can be relative to the page source, instead of
\ </tmp> directory as before.
\
\ 2019-01-27: Fix: `include_asciidoctor` tried to delete
\ `input_file$`.

\ vim: filetype=gforth
