# Fendo

## Description

Fendo is a static website generator written in
[Forth](http://forth-standard.org) with
[Gforth](http://gnu.org/software/gforth).

Some features:

- Every source page is an independent Forth program that builds its HTML
  target.

- Actual Forth code can be embedded in source pages and design
  templates.

- Support for several markups (native, Asciidoctor, Markdown, HTML).

- Support for multilingual sites.

- Many addons for specific needs.

- Redirections.

- Nestable page shortcuts.

- Fully customizable.

Links:

- [Fendo home page](http://programandala.net/en.program.fendo.html)

- [Fendo repository in
  SourceHut](https://hg.sr.ht/~programandala_net/fendo)

- [Fendo repository in
  GitHub](http://github.com/programandala-net/fendo)

## Current status

Fendo is very stable and is regulary improved, after the requirements of
the author’s websites it already powers (three websites have been
converted to Fendo from previous engines and two have been created
directly with it).

The source code is well documented, but specific usage documentation is
still missing. A full manual with a complete glossary will be built
automatically from the sources.

The current version under development is 0.6.0, which will be considered
the first public version (releases A-05 and older are internal, not
usable).

## The name

Fendo stands for "Forth Engine for Net DOcuments". Also,
["fendo"](http://vortaro.net/#fendo) is the Esperanto word for "slot",
in the sense "a narrow depression, perforation, or aperture".

## History of the code and the repository

- 2012: The development of Fendo started in order to replace two
  previous website generators written by the author (one of them written
  in Forth, the other one in PHP) with a new one combining the best
  features of both of them.

- 2017-02-08: A local Git repository was created from all of the
  development backups, in order to resume the development and prepare
  the first public release.

- 2020-03-28: The Git repository was uploaded to
  [GitHub](https://github.com/programandala-net/fendo).

- 2020-11-29: The Git repository was converted to
  [Fossil](https://fossil-scm.org), keeping GitHub as an automatic
  read-only mirror.

- 2023-04-01: The Fossil repository was converted to
  [Mercurial](https://mercurial-scm.org), enabling bidirectional
  communication with GitHub.

- 2024-09-02: The Mercurial repository was uploaded to
  [SourceHut](https://hg.sr.ht/~programandala_net/fendo), keeping GitHub
  as a mirror.
