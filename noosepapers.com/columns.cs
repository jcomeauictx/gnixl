#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {pop LoremIpsum}
  ifelse /datasource exch def
/wordparse {
  <</EODCount 0 /EODString ( )>>
  /SubFileDecode
} bind def
/xwidth {stringwidth pop} bind def
/column {  % source -
  /source exch def
  /width pagewidth 5 div def
  /height pageheight 2 div def
  /line () def
  {source wordparse filter
    % readstring through wordparse filter will almost always return false
    % (only true with 128-character word)
    buffer readstring {
      /rangecheck signalerror  % word too long
    }{
      line exch stradd (stack after line exch stradd: ) print =stack
      dup xwidth width (stack after dup xwidth width: ) print =stack gt
        {gsave line show /line () def grestore 0 -10 rmoveto}
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
  /Helvetica 12 selectfont
  0 pageheight 10 sub moveto
  (now showing column on page) =
  sys.argv 1 get (r) file column
  showpage
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
