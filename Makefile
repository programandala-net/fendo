# Makefile

# This file is part of Fendo
# http://programandala.net/en.program.fendo.html

# Last modified 202011282104

# ==============================================================
# Author

# Marcos Cruz (programandala.net), 2017, 2018, 2020.

# ==============================================================
# License

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and this
# license.  There is no warranty.

# ==============================================================
# Requirements

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

# ==============================================================
# History

# See at the end of the file.

# ==============================================================
# Notes about make

# $@ = the name of the target of the rule
# $< = the name of the first prerequisite
# $? = the names of all the prerequisites that are newer than the target
# $^ = the names of all the prerequisites

# `%` works only at the start of the filter pattern

# ==============================================================
# Config

VPATH = ./

MAKEFLAGS = --no-print-directory

#.ONESHELL:

# ==============================================================
# Main

.PHONY: all
all: doc

.PHONY: clean
clean: cleandoc

.PHONY: cleandoc
cleandoc:
	-rm -f doc/* tmp/*

# ==============================================================
# Documentation

.PHONY: doc
doc: \
	doc/fendo_manual.html \
	doc/fendo_manual.dbk \
	doc/fendo_manual.info \
	doc/fendo_manual.texi \
	doc/fendo_manual.pdf

# ----------------------------------------------
# Common rules

doc/%.pdf: tmp/%.adoc
	asciidoctor-pdf --out-file $@ $<

%.html: %.adoc
	asciidoctor --out-file=$@ $<

doc/%.dbk: tmp/%.adoc
	asciidoctor --backend=docbook --out-file=$@ $<

%.info: %.texi
	makeinfo -o $@ $<

%.texi: %.dbk
	pandoc -f docbook -o $@ $<

%.info: %.texi
	makeinfo -o $@ $<

# ----------------------------------------------
# Main

fendo_files=$(wildcard *.fs)
lib_files=$(wildcard doc_src/galope/*.fs)

tmp/glossary.adoc: tmp/files.txt
	glosara --level=3 --input=$< --output=$@

#glosara --level=3 -m "glossary{ }glossary" --input=$< --output=$@

doc/fendo_manual.html: tmp/fendo_manual.adoc README.adoc
	asciidoctor --out-file=$@ $<

tmp/manual_skeleton.adoc: doc_src/manual_skeleton.adoc VERSION.fs
	version=$$(gforth VERSION.fs -e "fendo_version type bye" )
	sed -e "s/%VERSION%/$${version}/" $< > $@

tmp/files.txt: $(fendo_files) $(lib_files)
	ls -1 $^ > $@

tmp/fendo_manual.adoc: tmp/manual_skeleton.adoc tmp/glossary.adoc
	cat $^ > $@

# ==============================================================
# Change log

# 2018-12-07: Start. Adapted from Galope
# (http://programandala.net/en.program.galope.html).
#
# 2018-12-19: Replace htmldoc with asciidoctor-pdf, the PDF is much better.
#
# 2020-11-28: Fix parameters to convert DocBook to Texinfo. Replace ".docbook"
# extension with ".dbk". Include the documentation of Galope library modules
# (at the moment only the `begin-translation` module) into the glossary.
