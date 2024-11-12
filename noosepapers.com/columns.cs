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
/MAXPARAGRAPHS 100 def  % NOTE: this is for testing with Lorem ipsum generator
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
/columnline {  % words index - endofparagraph newindex string
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
  length 1 sub wordindex eq wordindex line string.truncate
  (stack at end of columnline: ) #only #stack
} bind def

/lineshow {  % string -
  (before show: ) #only #stack
  dup ( ) string.count  % count the spaces
  1 index xwidth columnwidth exch sub exch div  % pixels each space must occupy
  0 ( ) ord 4 -1 roll (stack before widthshow: ) #only #stack widthshow
} bind def

/showparagraph {  % x0 y0 y1 words - index y
  (starting showparagraph with stack: ) #only #stack
  % use local variables to simplify coding
  /wordlist exch def  /ymin exch def  /y exch def  /x exch def
  /wordindex 0 def % index to beginning of paragraph
  {wordlist wordindex columnline (after columnline: ) #only #stack
    x y moveto lineshow (after lineshow: ) #only #stack
    /wordindex exch def y lineheight sub /y exch def
    dup {y lineheight sub /y exch def} if  % subtract another line at end
    % done if end of paragraph OR column height exceeded
    y ymin lt or {wordindex y exit} if
  }
  loop
  (stack at end of showparagraph: ) #only #stack
} bind def

/column {  % x0 y0 y1 source - pcount pindex
  (testing column creation with stack: ) #only #stack
  /datasource exch def  /y1 exch def  /y exch def  /x exch def
  /pcount zero
  {datasource paragraphs filter
    (column loop in progress) #
    1024 dup mul string
    readline not /eof exch def
    x y y1 4096 array () 6 -1 roll exch #stack string.split showparagraph
    exch /pindex exch def
    /pcount inc
    % quit if column complete, or all data processed, or max paragraphs read
    eof {(exiting on EOF) # pop exit} if
    y1 lt {(exiting on paragraph complete) # exit} if
    pcount MAXPARAGRAPHS eq {(exiting on max paragraphs) # exit} if
  } loop
  (stack at end of columns test: ) #only ##stack
  pcount pindex
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
  (bytes available: ) # datasource bytesavailable #
  10 columnwidth add pageheight 10 sub lineheight sub 10 datasource column
  (now showing column on page) #
  showpage
  (final stack: ) #only ##stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
