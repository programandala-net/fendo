.( fendo.addon.projects.fs) cr

\ This file is part of
\ Fendo

\ This file is the projects addon.  It provides tools to add shared
\ metadata about software projects to the website pages.
\
\ XXX NOT FINISHED

\ Copyright (C) 2015 Marcos Cruz (programandala.net)

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

\ Fendo is written in Forth with Gforth
\ (<http://www.bernd-paysan.de/gforth.html>).

\ **************************************************************
\ Change history of this file

\ See at the end of the file.

\ **************************************************************
\ Requirements

forth_definitions

\ From Galope
require galope/module.fs  \ 'module:', ';module', 'hide', 'export'

fendo_definitions

\ **************************************************************
\

module: fendo.addon.projects

\ ------------------------------
\ First approach, abandoned

\ A data structure holds the data of named projects,
\ and any page can be associated with a project.

false [if]  \ XXX OLD -- not finished

\ Project structure
0
  2 cells +field >project_status     \ active, paused, halted or abandoned
  2 cells +field >project_start      \ date in ISO format
  2 cells +field >project_end        \ date in ISO format
  2 cells +field >project_completion \ percentage (as a string)
constant /project 

public

datum: project  \ name of the project the page is associated to

: (project_datum)  ( +n project -- ca len )
  ;
: pid>project?  ( pid -- a true | false )
  
  ;

: project_datum:  ( +n "name"  -- )
  \ Create a new project datum.
  \ +n = datum offset from the start of the project data.
  create ,
  does>  ( pid -- ca len )
  ( pid pfa )
  project dup if  (project_datum)  else  then
  ;

0 >project_status       project_datum: project_status
0 >project_start        project_datum: project_start
0 >project_end          project_datum: project_end
0 >project_completion   project_datum: project_completion

variable projects  \ counter

: project:  ( "name" -- )
  \ Create a new project.
  /project allocate throw constant
  ;

[then]


\ ------------------------------
\ Second approach

\ An ordinary metadatum holds the page id of the page that has the
\ data of the project (or nothing if the current page has the data).
\ Project metadata are hold in special fields.

datum: project_page

: :project_datum>value  ( ca len -- )
  \ Create a project page metadatum that returns its value.
  \ This is the normal version of the metadatum: if executed in
  \ the metadata header (between 'data{' and '}data') it will
  \ parse its datum from the input stream; out of the header it
  \ will return the datum string.
  \ ca len = datum name
  nextname create
    cell /datum dup @ , +!  \ store the offset and increment it
  does>  ( a1 | "text<nl>" -- ca len | )
    \ a1 = page data address
    \ ca len = datum 
    \ dfa = data field address of the datum word
    \ u = datum offset
    ( a1 dfa | dfa "text<nl>" )
    @  in_data_header? @  ( u wf )
    if    ( u "datum<nl>" ) parse_datum
    else  ( a1 u ) + $@  then
  ;
: :project_datum>address  ( ca len -- )
  \ Create a project page metadatum word that returns the address of its data.
  \ The new name will have a tick at the start.
  \ ca len = datum name
  s" '" 2swap s+ nextname
  latestxt  \ of the word previously created by ':project_datum>value'
  create  ( xt ) >body ,
  does>  ( a1 -- a2 )
    \ a1 = page data address
    \ a2 = datum address
    \ dfa = data field address of the datum word
    \ u = datum offset
    ( a1 dfa )  @ @
\    dup ." datum offset = " .  \ xxx informer
    ( a1 u ) +
  ;
: project_datum:  ( "name" -- )
  \ Create a project page metadatum.
  parse-name 2dup :datum>value :datum>address
  ;

project_datum: project_status
project_datum: project_start
project_datum: project_end
project_datum: project_completion


\ **************************************************************
\ Calculated data

: completed_project?  ( pid -- wf )
  \ Is the project completed?
  dup project_completion s" 100%" str=
  swap project_status s" completed" str= or
  ;
: ceased_project?  ( pid -- wf )
  \ Is the project completed or abandoned?
  project_end nip 0<>
  ;
: abandoned_project?  ( pid -- wf )
  \ Is the project abandoned?
  project_status s" abandoned" str=
  ;
: project?  ( pid -- wf )
  \ Is the given page a project?
  dup >r project_status nip
  r@ project_start nip
  r> project_completion nip  or or
  ;

;module

\ **************************************************************
\ Change history of this file

\ 2015-02-02: Start, partly based on the code of
\ <fendo-programandala.addon.projects.fs>.
\
\ 2015-02-06: Start of a new approach.
\
\ 2015-02-11: Fix: proper 'definitions' in the requirements.

.( fendo-programandala.addon.projects.fs compiled) cr
