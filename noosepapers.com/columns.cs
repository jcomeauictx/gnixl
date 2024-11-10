#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
(latin1font.ps) run
(paragraphs.cs) run
/xwidth {stringwidth pop} bind def
% broadsheet typically has 5 columns, tabloid 4, zine maybe 2 or 3
/columnwidth pagewidth 2 div def
/columnheight pageheight 2 div def
/columnline {  % words index - words newindex
  /index exch def
  (column width: ) print width =only (, column height: ) print height =
  /spacewidth ( ) xwidth def
  /line () def
  (width: ) print width =
  (spacewidth: ) print spacewidth =
  {source paragraphs filter
      {
        dup /word exch def
	xwidth line xwidth spacewidth add add dup
	(length after adding ") print word print (": ) print =
	(compare to width: ) print width =
	(stack: ) #only ###stack
        width gt
          {
            gsave
            (showing ") line + (" at ) + print
              currentpoint exch =only (,) print =
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
  /Helvetica-Latin1 12 selectfont
  (starting columns test program) #
  sys.argv dup length 1 gt
    {1 get (r) file}
    {pop loremipsum}
    ifelse /datasource exch def
  0 1 currentfont /Encoding get 3 1 roll
    2 index length 1 sub dup /charmap exch dict def
    {1 index 1 index get charmap 3 1 roll exch put} for pop
  /count zero
  sys.argv dup length 1 gt
    {1 get (r) file}
    {pop LoremIpsum}
    ifelse /datasource exch def
  (testing column creation) #
  {datasource paragraphs filter
    1024 dup mul string
    readline not /eof exch def
    4096 array () 3 -1 roll exch string.split array.truncate ==
    eof {(exiting on EOF) # exit} if  % quit after all data processed
    /count inc count 10000 eq {exit} if  % quit test after 10000 paragraphs
  } loop
  (stack at end of columns test: ) #only ##stack
  (bytes available: ) # datasource bytesavailable #
  10 pageheight 10 sub moveto
  (now showing column on page) =
  showpage
  (final stack: ) #only ##stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
