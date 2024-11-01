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
  /VT 8#13 chr def
  /SP ( ) def
  /EOF (D) ord 64 not and chr def  % control-D marks end of file
  {[
    {source 8192 string (before readline: ) print =stack
      readline (after readline: ) print =stack
      not /eof exch def  % false means end-of-file
      dup strlen 0 eq  % empty string found
        {pop counttomark 0 eq  % only thing found so far?
          {(ignoring empty line preceding actual content) =}
          {pop  % remove space from end of previous line
            VT (found end of paragraph) = exit  % mark end of paragraph
          }
          ifelse
        }
        {
          % pdftotext replaces end-of-line hyphens with chr(0xac)
          % perhaps it meant chr(0xad), soft hyphen?
          dup dup strlen 1 sub 2 copy get 16#ac eq
            {0 exch getinterval}  % remove final character
            {pop pop SP}  % remove string and length, and append space
            ifelse
        }
        ifelse
      eof {(marking EOF) = EOF} if
    }  % end of paragraph read
    loop
    ]  % create an array of the strings found
    /paragraph 1024 1024 mul string def  % megabyte string to hold paragraph
    {paragraph exch (before append: ) print =stack append} forall
    paragraph
    (after append complete: ) print =stack
    exit
  }
  loop
  <</EODCount 0 /EODString VT>>
  /SubFileDecode
} bind def
scriptname (paragraphs) eq {
  sys.argv dup length 1 gt
    {1 get (r) file}
    {pop LoremIpsum}
    ifelse /datasource exch def
  (testing paragraphs filter: ) =
  datasource paragraphs filter {
    (reading next paragraph from loop, stack: ) print =stack
    dup 1024 1024 mul string readstring
    (stack after readstring: ) print =stack
    pop  % discard flag
    dup length cvbool (after length cvbool: ) print =stack
    {(paragraph: ") print print (") =}
    {pop (exiting paragraphs loop) = exit}
    ifelse
  } loop
  (stack at end of paragraphs test: ) print =stack
  pop  % remove filtered file object from stack
  (bytes remaining in file: ) print datasource bytesavailable =
} if
(stack remaining at end of test: ) print =stack
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
