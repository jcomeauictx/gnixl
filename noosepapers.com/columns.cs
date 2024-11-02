#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
(latin1font.ps) run
(paragraphs.cs) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {pop loremipsum}
  ifelse /datasource exch def
0 1 currentfont /Encoding get 3 1 roll
  2 index length 1 sub dup /charmap exch dict def
  {1 index 1 index get charmap 3 1 roll exch put} for pop
/wordbuffer 128 string def
/wordparse {
  <</EODCount 0 /EODString ( )>>
  /SubFileDecode
} bind def
/xwidth {stringwidth pop} bind def
/column {  % source -
  /source exch def
  /width pagewidth 2 div def
  /height pageheight 2 div def
  /spacewidth ( ) xwidth def
  /line () def
  (width: ) print width =
  (spacewidth: ) print spacewidth =
  {source wordparse filter
    % readstring through wordparse filter will almost always return false
    % (only true with 128-character word)
    wordbuffer readstring
      {wordbuffer /rangecheck signalerror}  % word too long
      {
        dup /word exch def
	xwidth line xwidth spacewidth add add dup
	(length after adding ) print word print (: ) print =
	(compare to width: ) print width =
	(stack: ) print =stack
        width gt
          {
            gsave
            (showing line at ) print currentpoint exch =only (,) print = 
            line show /line word ( ) stradd def
            grestore
            0 -10 rmoveto
            currentpoint exch pop 0 lt {exit} if
          }
          {line ( ) stradd /line exch def}
          ifelse  % row width > column width
      }
      ifelse  % readstring filled wordbuffer
  }
  loop
} bind def
scriptname (columns) eq {
  {datasource wordparse filter
    wordbuffer readstring
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
  % charmap ===
  (testing paragraphs filter: ) =
  /source (federalistpapers1961hami.txt) (r) file def
  (dumping federalist papers by paragraph) =
  {source paragraphs filter
    1024 dup mul string readstring
    pop  % discard readstring flag
    dup length cvbool
      %(paragraph: ) print =stack
      {=}
      {pop (exiting paragraphs loop) = exit}
      ifelse
  } loop
  (stack at end of columns test: ) print =stack
  (bytes available: ) print source bytesavailable =
  (final stack: ) print =stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
