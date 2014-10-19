.( fendo.config.fs ) cr

\ This file is part of Fendo.

\ This file setups the default configuration.

\ Copyright (C) 2013 Marcos Cruz (programandala.net)

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

\ **************************************************************
\ Change history of this file

\ 2013-06-07: Start.
\ 2013-06-23: Change: 'style_subdir' and 'designs_dir' removed;
\   one single subdir for files and images; design and template
\   variables are renamed. This way all is a bit simpler.
\ 2014-03-04: 'xhtml?' moved here from <fendo.markup.common.fs>.
\ 2014-07-08: New: Site variables, needed for the Atom module;
\   they are going to be used in the site template too.
\ 2014-07-14: Change: site variables, and 'domain' as well,
\   are converted to defered words, in order to make them
\   configurable as multilingual constants.

\ **************************************************************
\ Requirements

forth_definitions
require string.fs  \ Gforth's dynamic strings
fendo_definitions

\ **************************************************************
\ Configurable site data

\ Some site data are stored in ordinary variables, but as dynamic
\ strings (with  '$!', '$@', etc.). Other are defered words, in order
\ to make them configurable as multilingual constants.

\ All directories and subdirectories must have an ending slash.

defer domain

\ Filename extensions (with dot)

variable forth_extension  \ filename extension of Forth source pages
s" .fs" forth_extension $!

variable html_extension  \ default filename extension of target HTML files
s" .html" html_extension $!
\ Note: any page can have its own extension in its metadata, e.g.
\ ".xml" for Atom documents.

\ Local absolute directories (with final slash)

variable source_dir  \ Forth source pages
s" ~/forth/fendo-demo/pages/" source_dir $!

variable target_dir  \ target HTML pages
s" ~/forth/fendo-demo/html/" target_dir $!

\ Target relative subdirectories (with final slash)

variable files_subdir  \ target files
s" files/" files_subdir $!

variable website_design_subdir \ default design
s" designs/basic_minimal/" website_design_subdir $!

variable website_template  \ filename of the default HTML template
s" index.html" website_template $!

variable content_markup  \ markup that represents the page content in the template
s" {CONTENT}" content_markup $!

\ Target HTML

\ XXX TODO convert to value
variable xhtml?  \ flag, XHTML syntax?

\ Site constants and variables

\ They are defered words in order to convert them into multilingual
\ constants if needed.

defer site_title
defer site_plain_title  \ a copy without markups
defer site_subtitle
defer site_plain_subtitle  \ a copy without markups

defer site_icon
defer site_author

.( fendo.config.fs compiled) cr
