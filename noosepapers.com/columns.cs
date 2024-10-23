#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
(latin1font.ps) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {pop LoremIpsum}
  ifelse /datasource exch def
0 1 currentfont /Encoding get 3 1 roll
  2 index length 1 sub dup /charmap exch dict def
  {1 index 1 index get charmap 3 1 roll exch put} for pop
/wordparse {
  <</EODCount 0 /EODString ( )>>
  /SubFileDecode
} bind def
/xwidth {stringwidth pop} bind def
/paragraphs {  % source -
  % 2 consecutive endlines (LF, \012) we will interpret as paragraph
  % mark them with vertical tab (VT, \013) for paragraph marker
  % replace *single* endline with space (SP, \020)
  /source exch def
  /endlines zero
  /LF 8#12 def
  /VT 8#13 def
  /SP 8#20 def
  {source dup read {
    dup LF eq
      {pop ( ) /endlines inc}
      {endlines 1 ge
        {endlines 2 ge
          {VT exch}
          {SP exch}
          ifelse
        }
        /endlines zero
      } if
      ifelse
    } ifelse
  }
  <</EODCount 0 /EODString VT>>
  /SubFileDecode
} bind def
/column {  % source -
  /source exch def
  /width pagewidth 5 div def
  /height pageheight 2 div def
  /line () def
  {source wordparse filter
    % readstring through wordparse filter will almost always return false
    % (only true with 128-character word)
    buffer readstring {
      buffer /rangecheck signalerror  % word too long
    }{
      /word exch def
      line word stradd (stack after line exch stradd: ) print =stack
      dup xwidth width (stack after dup xwidth width: ) print =stack gt
        {gsave (showing line at ) print currentpoint exch =only (,) print = show /line word ( ) stradd def grestore 0 -10 rmoveto currentpoint exch pop 0 lt {exit} if}
        {( ) (stack before append space: ) print =stack stradd /line exch def}
        ifelse
    } ifelse
  } loop
} bind def
scriptname (columns) eq {
  /buffer 128 string def
  {datasource paragraphs filter wordparse filter (filter stack: ) print =stack
    buffer readstring
    {print}  % this would only happen with 128-character "word"
    {dup length 0 gt {print} {pop exit} ifelse}
    ifelse
  } loop
  % if the above loop ended, we must be using a regular file, not LoremIpsum
  % `resetfile` doesn't necessarily, work, so just reopen the file 
  /Helvetica-Latin1 12 selectfont
  0 pageheight 10 sub moveto
  (now showing column on page) =
  sys.argv 1 get (r) file column
  showpage
  charmap ===
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
