.( fendo.markup.fendo.punctuation.fs ) cr

\ This file is part of Fendo
\ (http://programandala.net/en.program.fendo.html).

\ This file defines the Fendo markup for punctuation.

\ Last modified 20170622.
\ See change log at the end of the file.

\ Copyright (C) 2013,2014,2017 Marcos Cruz (programandala.net)

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

\ ==============================================================
\ Tools

\ Punctuation markup is needed in order to print punctuation properly
\ after another markup. Example:

\   This // emphasis // does the right spacing.
\   But this // emphasis // , well, it
\   needs to be followed by a markup comma.

\ The ',' markup must print a comma without a leading space.  If ','
\ were not a markup but an ordinary printable content, a leading space
\ would be printed.

\ The same happens with opening parens and other opening punctuaction
\ characters, e.g.:

\   In this ( « ** example ** »).

\ the characters "(" and "«" must be defined as opening punctuation
\ (one single word '(«' would work too), and '»).' should be a closing
\ punctuation word ('»', ')' and '.' apart would work too).

fendo_definitions

: }punctuation:  ( "name" -- )
  parse-name? abort" Missing name in '}punctuation:'"
  :echo_name_ ;
  \ Create a closing punctuation word.
  \ "name" = punctuation --and name of its punctuation word

: punctuation{:  ( "name" -- )
  parse-name? abort" Missing name in 'punctuation{:'"
  :echo_name+ ;
  \ Create an opening punctuation word.
  \ "name" = punctuation --and name of its punctuation word

\ ==============================================================
\ Markup

markup_definitions

\ XXX TODO complete as required

\ }punctuation: "  \ XXX FIXME, the same punctuation can not be closing and opening 
\ unless the system is redisegned to track the used punctuations.
\ }punctuation: '  \ XXX FIXME same case than "
punctuation{: (  \ )
punctuation{: (¡
punctuation{: («
punctuation{: (¿
punctuation{: [  \ ]
punctuation{: {  \ }
punctuation{: ¡
punctuation{: «
punctuation{: ¿
punctuation{: ‘
punctuation{: “

}punctuation: !
}punctuation: "
}punctuation: ",
}punctuation: ".
}punctuation: ":
}punctuation: ";
}punctuation: &#39;s
}punctuation: 's
}punctuation: )
}punctuation: ),
}punctuation: ).
}punctuation: ):
}punctuation: );
}punctuation: ,
}punctuation: ,"
}punctuation: ,’
}punctuation: .
}punctuation: ."
}punctuation: ...
}punctuation: ...),
}punctuation: ...).
}punctuation: ...);
}punctuation: ...»
}punctuation: ...».
}punctuation: ...»;
}punctuation: .’
}punctuation: .”
}punctuation: :
}punctuation: ;
}punctuation: ;"
}punctuation: ?
}punctuation: ]
}punctuation: }
}punctuation: »
}punctuation: »)
}punctuation: »),
}punctuation: »).
}punctuation: »,
}punctuation: ».
}punctuation: »;
}punctuation: ’
}punctuation: ’,
}punctuation: ’”
}punctuation: ”
}punctuation: ”»
}punctuation: ”»,
}punctuation: ”».

fendo_definitions

.( fendo.markup.fendo.punctuation.fs compiled ) cr

\ ==============================================================
\ Change log

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.
\
\ 2014-07-14: New: double quote as ending punctuation. This removes
\ the ending space in calculated HTML attributes in the template.
\
\ 2014-10-12: New: "'s", though it's not punctuation.
\ 
\ 2014-11-04: New: '&#39;s' (HTML version of "'s"; useful in certain
\ cases).
\
\ 2014-12-10: Fix: duplicated punctuation removed.
\
\ 2017-06-22: Update source style, layout and header.

\ vim: filetype=gforth
