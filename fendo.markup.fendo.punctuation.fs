.( fendo.markup.fendo.punctuation.fs ) cr

\ This file is part of Fendo.

\ This file defines the Fendo markup for punctuation.

\ Copyright (C) 2013,2014 Marcos Cruz (programandala.net)

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

\ See at the end of the file.

\ **************************************************************
\ Tools

\ Punctuation markup is needed in order to print it properly
\ after another markup. Example:

\   This // emphasis // does the right spacing.
\   But this // emphasis // , well
\   needs to be followed by a markup comma.

\ The ',' markup must print a comma without a leading space.
\ If',' were not a markup but an ordinary printable content,
\ a leading space would be printed.

\ The same happens with opening parens and other opening punctuaction
\ characters, e.g.:

\   In this ( « ** example ** »).

\ the characters "(" and "«" must be defined as opening punctuation
\ (one single word '(«' would work too), and '»).' should be a closing
\ punctuation word ('»', ')' and '.' apart would work too).

fendo_definitions

: }punctuation:   ( "name" -- )
  \ Create a closing punctuation word.
  \ "name" = punctuation --and name of its punctuation word
  parse-name? abort" Missing name in '}punctuation:'"
  :echo_name_
  ;
: punctuation{:   ( "name" -- )
  \ Create an opening punctuation word.
  \ "name" = punctuation --and name of its punctuation word
  parse-name? abort" Missing name in 'punctuation{:'"
  :echo_name+
  ;

\ **************************************************************
\ Markup

markup_definitions

\ xxx todo complete as required

\ }punctuation: "  \ xxx fixme, can not be closing and opening unless
\ the system is redisegned to track the used punctuations.
\ }punctuation: '  \ xxx fixme same case than "
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
}punctuation: ",
}punctuation: ".
}punctuation: ":
}punctuation: ";
}punctuation: )
}punctuation: ),
}punctuation: ).
}punctuation: ):
}punctuation: );
}punctuation: ,
}punctuation: .
}punctuation: ...
}punctuation: ...),
}punctuation: ...).
}punctuation: ...);
}punctuation: ...»
}punctuation: ...».
}punctuation: ...»;
}punctuation: :
}punctuation: ;
}punctuation: ?
}punctuation: ]
}punctuation: }
}punctuation: »
}punctuation: »),
}punctuation: »).
}punctuation: »,
}punctuation: ».
}punctuation: ’
}punctuation: ”


fendo_definitions

\ **************************************************************
\ Change history of this file

\ 2014-04-21: Code moved from <fendo.markup.fendo.fs>.

.( fendo.markup.fendo.punctuation.fs compiled ) cr
