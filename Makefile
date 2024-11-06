# Makefile

# This file is part of Fendo
# http://programandala.net/en.program.fendo.html

# Last modified: 20241106T1604+0100.
# See change log at the end of the file.

# Author {{{1
# ==============================================================

# Marcos Cruz (programandala.net), 2017, 2018, 2020.

# License
# ==============================================================

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and this
# license.  There is no warranty.

# Requirements {{{1
# ==============================================================

# Asciidoctor (by Dan Allen and Sara White)
# 	http://asciidoctor.org

# asciidoctor-pdf (by Dan Allen and Sara White)
# 	http://asciidoctor.org

# cat (from the GNU coreutils)

# Gforth (by Anton Erlt, Bernd Paysan et al.)
# 	http://gnu.org/software/gforth

# Glosara (by Marcos Cruz)
# 	http://programandala.net/en.program.glosara.html

# pandoc (by John Macfarlane)
# 	http://john-macfarlane.net/pandoc

# History {{{1
# ==============================================================

# See at the end of the file.

# Notes about make {{{1
# ==============================================================

# $@ = the name of the target of the rule
# $< = the name of the first prerequisite
# $? = the names of all the prerequisites that are newer than the target
# $^ = the names of all the prerequisites

# `%` works only at the start of the filter pattern

# Config {{{1
# ==============================================================

VPATH = ./

MAKEFLAGS = --no-print-directory

#.ONESHELL:

# Interface {{{1
# ==============================================================

.PHONY: all
all: doc

.PHONY: doc
doc: epub html pdf

.PHONY: epub
epub: \
	doc/fendo_manual.epub \

.PHONY: html
html: \
	doc/fendo_manual.html \

.PHONY: pdf
pdf: \
	doc/fendo_manual.pdf

.PHONY: clean
clean: cleandoc cleanreadme

.PHONY: cleandoc
cleandoc:
	-rm -f doc/* tmp/*

# Documentation {{{1
# ==============================================================

# Common rules {{{2
# ----------------------------------------------

doc/%.pdf: tmp/%.adoc
	asciidoctor-pdf --out-file $@ $<

doc/%.html: tmp/%.adoc
	asciidoctor --out-file=$@ $<

doc/%.epub: tmp/%.adoc
	asciidoctor-epub3 --out-file=$@ $<

doc/%.dbk: tmp/%.adoc
	asciidoctor --backend=docbook --out-file=$@ $<

%.info: %.texi
	makeinfo -o $@ $<

%.texi: %.dbk
	pandoc -f docbook -o $@ $<

# Main {{{2
# ----------------------------------------------

fendo_files=$(wildcard *.fs)
lib_files=$(wildcard doc_src/galope/*.fs)

tmp/glossary.adoc: tmp/files.txt
	glosara --level=3 --sections --input=$< --output=$@

doc/fendo_manual.html: tmp/fendo_manual.adoc README.adoc
	asciidoctor --out-file=$@ $<

tmp/manual_skeleton.adoc: doc_src/manual_skeleton.adoc VERSION.fs
	version=$$(gforth VERSION.fs -e "fendo_version type bye" )
	sed -e "s/%VERSION%/$${version}/" $< > $@

tmp/files.txt: $(fendo_files) $(lib_files)
	ls -1 $^ > $@

tmp/fendo_manual.adoc: \
	tmp/manual_skeleton.adoc \
	doc_src/markup.adoc \
	README.adoc \
	tmp/glossary.adoc
	cat tmp/manual_skeleton.adoc tmp/glossary.adoc > $@

# README.md {{{2
# ----------------------------------------------

readme_title=Fendo

include Makefile.readme

# Change log {{{1
# ==============================================================

# 2018-12-07: Start. Adapted from Galope
# (http://programandala.net/en.program.galope.html).
#
# 2018-12-19: Replace htmldoc with asciidoctor-pdf, the PDF is much better.
#
# 2020-11-28: Fix parameters to convert DocBook to Texinfo. Replace ".docbook"
# extension with ".dbk". Include the documentation of Galope library modules
# (at the moment only the `begin-translation` module) into the glossary. Build
# also an EPUB manual. Don't build Info and Texinfo by default, the conversions
# have problems. Add sections to the glossary. Fix: add <README.adoc> and
# <doc_src/markup.adoc> to the prerequisites of the Asciidoctor manual.
#
# 2020-12-24: Build an online version of the README file for the Fossil
# repository.
#
# 2024-09-03: Remove the rules to build the <README.html> required by Fossil;
# add rules to build a <README.md> for SourceHut.
#
# 2024-11-06: Move the rules that build <README.md> to <Makefile.readme> in
# order to share them with another projects.
