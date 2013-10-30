.( fendo_config.fs ) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-02.

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

\ 2013-06-07 Start.
\ 2013-06-23 Change: 'style_subdir' and 'designs_dir' removed;
\   one single subdir for files and images; design and template
\   variables are renamed. This way all is a bit simpler.

\ **************************************************************
\ Requirements

require string.fs  \ Gforth's dynamic strings

\ **************************************************************
\ Configurable variables

\ Config values are stored in ordinary variables, but as dynamic
\ strings (with  '$!', '$@', etc.).

\ All directories and subdirectories must have an ending slash.

variable domain
s" yourdomain.com" domain $!

\ Filename extensions

variable forth_extension  \ filename extension of Forth source pages
s" .fs" forth_extension $!

variable html_extension  \ filename extension of target HTML files
s" .html" html_extension $!

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

false [if] \ xxx bug thread
: $! place ;
: $@ count ;
: $@len c@ ;
[then]



.( fendo_config.fs compiled) cr

