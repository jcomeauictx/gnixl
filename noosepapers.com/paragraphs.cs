#!/usr/local/casperscript/bin/ccs --
(lorem_ipsum.cs) run
/paragraphs {  % source -
  % 2 consecutive endlines (LF, \012) we will interpret as paragraph
  % mark them with vertical tab (VT, \013) as paragraph marker
  % replace *single* endline with space (SP, \040)
  % `readline` returns substring true in normal case;
  % substring false at EOF;
  % rangecheck error if string filled before newline seen
  /source exch def
  /linebuffer 8192 string def
  /VT 8#13 chr def
  /SP ( ) def
  {[
    {source linebuffer readline not /eof exch def  % false means end-of-file
      dup strlen 0 eq  % empty string found
        {pop counttomark 0 eq  % only thing found so far?
          {(ignoring empty line preceding actual content) =}
          {VT exit}  % mark end of paragraph
        }
        {SP}  % append space
        ifelse
    }  % end of paragraph read
    loop
    ]  % create an array of the strings found
    /paragraph 1024 1024 mul string def  % megabyte string to hold paragraph
    {paragraph append} forall exit
  }
  loop
  <</EODCount 0 /EODString VT>>
  /SubFileDecode
} bind def
scriptname (paragraphs) eq {
  sys.argv dup length 1 gt
    {1 get (r) file}
    {pop loremipsum}
    ifelse /datasource exch def
  (testing paragraphs filter: ) =
  datasource paragraphs filter {
    dup 8192 string readstring pop dup length cvbool (paragraph: ) print =stack
    {=}
    {pop (exiting paragraphs loop) = exit}
    ifelse
  } loop
  (stack at end of paragraphs test: ) print =stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
