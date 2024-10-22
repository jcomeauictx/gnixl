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
/column {  % source -
  /source exch def
  /width pagewidth 5 div def
  /height pageheight 2 div def
  /line () def
  {source wordparse filter
    buffer readstring {
      line exch stradd
      dup stringwidth width gt
        {gsave line show /line () def grestore 0 -10 rmoveto}
        {( ) stradd /line exch def}
        ifelse
    }{
      (stack at end of `columns`: ) print =stack exit
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
  datasource resetfile
  /Helvetica 12 selectfont
  0 pageheight 10 sub moveto
  (now showing column on page) =
  datasource column
  showpage
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
