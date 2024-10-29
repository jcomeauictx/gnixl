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
  /paragraph 1024 dup mul string def  % megabyte string
  /endlines zero
  /LF (\n) ord def
  /VT 8#13 chr def
  /SP ( ) def
  {source linebuffer readline (after read: ) print =stack
    {dup length 0 gt
      {paragraph strlen 0 gt
      	{pop SP /endlines inc (found LF: ) print =stack}
      	{chr endlines 1 ge
        {endlines 2 ge
          {VT exch stradd (found paragraph: ) print =stack}
          {(ignoring LF: ) print =stack}
          ifelse
        } if  % (endline(s))
      }
      ifelse % (LF)
    (stack at end of paragraph filter procedure: ) print =stack
    }
    {VT}  % mark end of data with end-of-paragraph marker
    ifelse  % (read flag true or false)
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
  (federalistpapers1961hami.txt) (r) file
  (dumping federalist papers by paragraph) =
  paragraphs filter {
    dup 1024 string readstring pop dup length cvbool (paragraph: ) print =stack
    {=}
    {pop (exiting paragraphs loop) = exit}
    ifelse
  } loop
  (stack at end of paragraphs test: ) print =stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
