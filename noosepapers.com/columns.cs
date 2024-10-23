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
  % 2 consecutive endlines (\n) we will interpret as paragraph
  % mark them with the "backwards P" glyph for paragraph marker
  % replace *single* endline with space
  /source exch def
  /endlines zero
  /P charmap /paragraph get def  % paragraph marker, stylized backwards P
  {source dup read {
    dup 10 eq
      {pop ( ) /endlines inc}
      {endlines 1 ge {endlines 2 ge {P exch}{( ) exch} ifelse} if /endlines zero}
      ifelse
    } ifelse
  }
  <</EODCount 0 /EODString 1 string 0 charmap /paragraph get put>>
  /SubFileDecode
  filter
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
  {datasource wordparse filter
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
