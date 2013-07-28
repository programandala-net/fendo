
\ This program tries to reproduce the Fendo bug that arises when Forth
\ code (<: ... :>) is parsed at the end of a file.
\
\ 2013-07-28 Start. No error! :(

require string.fs

: content  ( ca len -- )
  \ Manage a parsed content.
  type space
  ;
: markup  ( ca len xt -- )
  \ Manage a parsed markup.
  nip nip execute
  ;
: something  ( ca len -- )
  \ Manage something found on the page content.
  \ ca len = parsed item (markup or printable content) 
  2dup forth-wordlist search-wordlist
  if  markup  else  content  then
  ;
: parse_content  ( "text" -- )
  \ Parse the current input source.
  \ The process is finished by the '}content' markup or the end
  \ of the source.
  begin
    parse-name dup
    if    something true
    else  2drop refill
    then  0=
  until
  ;
: content{  ( "text }content" -- )
  \ Start the page content.
  \ The end of the content is marked with the '}content' markup.
  parse_content
  ;
: }content  ( -- )
  \ Finish the page content. 
  ;

variable forth_code$
: forth_code$+  ( ca len -- )
  \ Append a string to the parsed Forth code.
  forth_code$ $@  s"  " s+ 2swap s+  forth_code$ $!
  ;
variable forth_code_depth  \ depth level of the parsed Forth code block?
: forth_code_end?  ( ca len -- ff )
  \ Is a name a valid end markup of the Forth code?
  \ ca len = latest name parsed 
  2dup s" <:" str= abs forth_code_depth +!
       s" :>" str= dup forth_code_depth +! 
  forth_code_depth @ 0= and
  ;
: parse_forth_code  ( "forthcode :>" -- ca len )
  \ Get the content of a merged Forth code. 
  \ Parse the input stream until a valid ":>" markup is found.
  \ ca len = Forth code
  s" " forth_code$ $!
  begin   parse-name dup
    if    \ 2dup ." { " type ." }"  \ xxx debug check
          2dup forth_code_end? dup >r if  2drop  else  forth_code$+  then r>
    else  2drop s"  " forth_code$+  refill 0=
    then
  until   forth_code$ $@
  \ cr ." <: " 2dup type ."  :>" cr  \ xxx debug check
  ;
: <:  ( "forthcode :>" -- )
  \ Start, parse and interpret a Forth block.
  1 forth_code_depth +!  parse_forth_code evaluate
  ;  immediate
: :>  ( -- )
  \ Finish a Forth block.
  forth_code_depth @ dup 0= abort" ':>' without '<:'"  1- forth_code_depth !
  ; immediate
\ \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
\ Example

content{

s" this is ordinary Forth code" type

<:
  s" but this is merged Forth code to be evaluated" type
  :>

and... this-is plain content!

<: s" more merged Forth code to be evaluated"
  type :>

}content

