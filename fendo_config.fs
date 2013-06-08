.( fendo_config.fs) cr

\ This file is part of
\ Fendo ("Forth Engine for Net DOcuments") version A-00.

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

\ Fendo is written in Forth
\ with Gforth (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ 2013-06-07 Start.

\ **************************************************************
\ Requirements

require string.fs  \ Gforth's dynamic strings

require galope/svariable.fs

\ **************************************************************
\ Configurable variables

\ Config values are stored in ordinary variables, but as dynamic
\ strings (with  '$!', '$@', etc.).

\ Filenames

variable forth_extension   \ filename extension of Forth source pages
variable html_extension    \ filename extension of target HTML files

variable source_dir  \ directory of the Forth source pages (with final slash)
variable target_dir  \ directory of the target HTML pages (with final slash)
variable designs_dir \ directory of the HTML designs (with final slash)

\ Design and template

variable default_design_dir \ design directory in the designs directory (with final slash)
variable default_template   \ filename of the default HTML template
variable content_markup     \ markup that represents the page content in the template

\ **************************************************************
\ Default values

\ Filenames

s" .fs" forth_extension $!
s" .html" html_extension $!

s" ~/forth/fendo-demo/pages/" source_dir $!
s" ~/forth/fendo-demo/html/" target_dir $!
s" ~/forth/fendo-demo/designs/" designs_dir $!

\ Design and template

\ The actual design and template will be the default ones unless
\ the current page defines its own in its metadata. Thus the
\ 'design' and 'template' page metadata can be used to override
\ the website's default values.

s" basic_minimal/" default_design_dir $!
s" index.html" default_template $!
s" {CONTENT}" content_markup $!

.( fendo_config.fs compiled) cr

