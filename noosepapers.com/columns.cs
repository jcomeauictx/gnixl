#!/usr/local/casperscript/bin/cs --
(starting columns.cs) #
(lorem_ipsum.cs) run
(latin1font.cs) run
(paragraphs.cs) run
/Times-Roman latin1font
/Times-Roman-Latin1 12 selectfont
/xwidth {stringwidth pop} bind def
/spacewidth ( ) xwidth def
(spacewidth: ) #only spacewidth #
% broadsheet typically has 5 columns, tabloid 4, zine maybe 2 or 3
/columnwidth pagewidth 2 div def
/columnheight pageheight 2 div def
(column width: ) #only columnwidth #only (, height: ) #only columnheight #
/columnline {  % words index - words newindex
  (starting columnline with stack: ) #only #stack
  /wordindex exch def
  /line 1024 string def
  dup wordindex get dup xwidth spacewidth add line strcopy
  string.truncate xwidth add
  dup (line width after addition would be ) #only #
  columnwidth ge (stack after ge: ) #only #stack
    {(exiting with stack: ) #only line string.truncate #stack}
    {line exch string.append /wordindex inc
      (after append: ) #only  #stack}
    ifelse
  wordindex
  (stack at end of columnline: ) #only #stack
  1 .quit
} bind def
(
/columns  
      {
        dup /word exch def
	xwidth line xwidth spacewidth add add dup
	(length after adding ") #only word #only (": ) print #
	(compare to width: ) #only width #
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
} bind def
) pop
scriptname (columns) eq {
  (testing columnline) #
  32 array
  (This is a test of the ability of columnline to determine column fit.)
  () string.split 0 (before columnline: ) # #stack columnline
  (
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
  (now showing column on page) #
  showpage
  (final stack: ) #only ##stack
  ) pop
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
