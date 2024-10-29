#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
(latin1font.ps) run
0 1 currentfont /Encoding get 3 1 roll
  2 index length 1 sub dup /charmap exch dict def
  {1 index 1 index get charmap 3 1 roll exch put} for pop
/paragraphs {  % source -
  % 2 consecutive endlines (LF, \012) we will interpret as paragraph
  % mark them with vertical tab (VT, \013) for paragraph marker
  % replace *single* endline with space (SP, \040)
  % `readline` returns substring true in normal case;
  % substring false at EOF;
  % rangecheck error if string filled before newline seen
  /source exch def
  /linebuffer 8192 string def
  /buffer 1024 dup mul string def  % megabyte string
  /paragraph buffer strcopy def
  /VT 8#13 chr def
  /SP ( ) def
  {source linebuffer readline not /eof exch def  % false means end-of-file
    /line exch def
    paragraph strlen 0 gt
  }  % end of filter procedure
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
  paragraphs filter {
    dup 8192 string readstring pop dup length cvbool (paragraph: ) print =stack
    {=}
    {pop (exiting paragraphs loop) = exit}
    ifelse
  } loop
  (stack at end of paragraphs test: ) print =stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
