# Makefile

# This file is part of Fendo
# http://programandala.net/en.program.fendo.html

# Last modified 201812071625

# ==============================================================
# Author

# Marcos Cruz (programandala.net), 2017, 2018.

# ==============================================================
# License

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and this
# license.  There is no warranty.

# ==============================================================
# Requirements

# Asciidoctor (by Dan Allen)
# 	http://asciidoctor.org

# cat (from the GNU coreutils)

# Gforth (by Anton Erlt, Bernd Paysan et al.)
# 	http://gnu.org/software/gforth

# Glosara (by Marcos Cruz)
# 	http://programandala.net/en.program.glosara.html

# htmldoc (by Michael Sweet)
# 	http://www.htmldoc.org

# pandoc (by Johw Macfarlane)
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
	doc/fendo_manual.docbook \
	doc/fendo_manual.info \
	doc/fendo_manual.texi \
	doc/fendo_manual.html.htmldoc.pdf

# ----------------------------------------------
# Common rules

%.html.htmldoc.pdf: %.html
	htmldoc \
		--book \
		--no-toc \
		--linkcolor blue \
		--linkstyle plain \
		--header " t " \
		--footer "  1" \
		--format pdf14 \
		--webpage \
		$< > $@

%.html: %.adoc
	asciidoctor --out-file=$@ $<

doc/%.docbook: tmp/%.adoc
	asciidoctor --backend=docbook --out-file=$@ $<

%.texi: %.docbook
	pandoc -o $@ $<

%.info: %.texi
	makeinfo -o $@ $<

%.texi: %.docbook
	pandoc -o $@ $<

%.info: %.texi
	makeinfo -o $@ $<

# ----------------------------------------------
# Main

fendo_files=$(wildcard *.fs)

tmp/glossary.adoc: tmp/files.txt
	glosara --level=3 --input=$< --output=$@

#glosara --level=3 -m "glossary{ }glossary" --input=$< --output=$@

doc/fendo_manual.html: tmp/fendo_manual.adoc README.adoc
	asciidoctor --out-file=$@ $<

tmp/manual_skeleton.adoc: doc_src/manual_skeleton.adoc VERSION.fs
	version=$$(gforth VERSION.fs -e "fendo_version type bye" )
	sed -e "s/%VERSION%/$${version}/" $< > $@

tmp/files.txt: $(fendo_files)
	ls -1 $^ > $@

tmp/fendo_manual.adoc: tmp/manual_skeleton.adoc tmp/glossary.adoc
	cat $^ > $@

# ==============================================================
# Change log

# 2018-12-07: Start. Adapted from Galope
# (http://programandala.net/en.program.galope.html).
