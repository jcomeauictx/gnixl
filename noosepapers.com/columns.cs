#!/usr/local/casperscript/bin/cs --
% typographicwebdesign.com/setting-text/font-size-line-height-measure-alignment/
(starting columns.cs) #
(lorem_ipsum.cs) run
(latin1font.cs) run
(paragraphs.cs) run
/fontsize 12 def
/Times-Roman latin1font
/Times-Roman-Latin1 fontsize selectfont
/lineheight fontsize 1.5 mul floor def (lineheight: ) #only lineheight #
% NOTE on x and y width (from PLRM section 5.4):
% Most Indo-European alphabets, including the Latin alphabet,
% have a positive x width and a zero y width. Semitic alphabets have a
% negative x width, and some Asian writing systems have a nonzero y width.
/xwidth {stringwidth pop} bind def
/spacewidth ( ) xwidth def
(spacewidth: ) #only spacewidth #
% broadsheet typically has 5 columns, tabloid 4, zine maybe 2 or 3
/columnwidth pagewidth 3 div def
/columnheight pageheight 2 div def
(column width: ) #only columnwidth #only (, height: ) #only columnheight #
/columnline {  % words index - newindex string endofparagraph
  (starting columnline with stack: ) #only #stack
  /wordindex exch def
  /maxindex 1 index length 1 sub def
  /line 1024 string def
  {
    (stack at start of loop: ) #only #stack
    dup wordindex get dup xwidth spacewidth add line strcopy
    (stack after strcopy: ) #only #stack
    string.truncate xwidth add
    dup (line width after addition would be ) #only #
    columnwidth ge wordindex maxindex eq or (stack after ge: ) #only #stack
      {pop (exiting with stack: ) #only #stack exit}
      {line strlen 0 gt {line ( ) string.append} if
        line exch string.append /wordindex inc
        (stack after append: ) #only  #stack
      }
      ifelse
  } loop
  length 1 sub wordindex eq wordindex line string.truncate 3 -1 roll
  (stack at end of columnline: ) #only #stack
} bind def

/lineshow {
  (before show: ) #only #stack show
} bind def

/showparagraph {  % x0 y0 y1 words - index
  (starting showparagraph with stack: ) #only #stack
  % use local variables to simplify coding
  /wordlist exch def  /ymin exch def  /y exch def  /x exch def
  /wordindex 0 def % index to beginning of paragraph
  {wordlist wordindex columnline (after columnline: ) #only #stack
    exch x y moveto lineshow (after lineshow: ) #only #stack
    {(end of paragraph: ) #only #stack}
    {/wordindex exch def y lineheight sub /y exch def}
    ifelse
    y ymin lt {(column height exceeded: ) #only #stack exit} if
  }
  loop
  showpage
  (stack at end of showparagraph: ) #only #stack
} bind def
scriptname (columns) eq {
  (testing columnline) #
  16 array
  (This is a test of the ability of columnline to determine column fit.)
  () string.split 0 (before columnline: ) # #stack columnline
  (end of paragraph: ) #only #only (, line of text: ") #only #only
  (", new index: ) #only #
  (testing showparagraph) #
  10  % x0
  pageheight 10 sub lineheight sub  % y0
  dup lineheight 10 mul sub  % y1 (10 lines desired)
  128 array loremipsum () string.split  % words
  showparagraph
  % /defaultdevice cvx 1 .quit
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
    4096 array () 3 -1 roll exch string.split ==
    eof {(exiting on EOF) # exit} if  % quit after all data processed
    /count inc count 10000 eq {exit} if  % quit test after 10000 paragraphs
  } loop
  (stack at end of columns test: ) #only ##stack
  (bytes available: ) # datasource bytesavailable #
  10 pageheight 10 sub moveto
  (now showing column on page) #
  showpage
  (final stack: ) #only ##stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
