#!/usr/local/casperscript/bin/cs --
% typographicwebdesign.com/setting-text/font-size-line-height-measure-alignment/
(starting columns.cs) #
(lorem_ipsum.cs) run
(latin1font.cs) run
(paragraphs.cs) run
/margin 10 def  % top and bottom, left and right, between columns
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
/columnsperpage 3 def
/columnwidth pagewidth margin dup add sub columnsperpage div def
/linewidth columnwidth margin sub def
/columnheight pageheight margin dup add def
(column width: ) #only columnwidth #only (, height: ) #only columnheight #
/columnline {  % words index - endofparagraph newindex string
  (starting columnline with stack: ) #only #stack
  /wordindex exch def
  /maxindex 1 index length 1 sub def
  /line 1024 string def
  {
    wordindex maxindex gt
      {(wordindex exceeds maxindex, exiting with stack: ) #only #stack exit}
      {
        (stack at start of loop: ) #only #stack
        dup wordindex get dup xwidth spacewidth add line strcopy
        (stack after strcopy: ) #only #stack
        string.truncate xwidth add
        dup (line width after addition would be ) #only #
        linewidth ge wordindex maxindex gt or (stack after ge: ) #only #stack
          {pop (columnline complete, exiting with stack: ) #only #stack exit}
          {line strlen 0 gt {line ( ) string.append} if
            line exch string.append /wordindex inc
            (stack after append: ) #only  #stack
          }
          ifelse
      }
      ifelse
  } loop
  pop  % discard words array
  wordindex maxindex gt  % set endofparagraph flag
  wordindex  % newindex
  line string.truncate  % trim trailing nulls off string
  (stack at end of columnline: ) #only #stack
} bind def

/lineshow {  % string final -
  (before show: ) #only #stack
  {show}
  {dup ( ) string.count dup cvbool  % count the spaces and set flag
    {1 index xwidth linewidth exch sub exch div  % pixels space must occupy
      0 ( ) ord 4 -1 roll (stack before widthshow: ) #only #stack widthshow
    }
    {pop show}
    ifelse
  }
  ifelse
} bind def

/showparagraph {  % x0 y0 y1 words - index y
  (starting showparagraph with stack: ) #only #stack
  % use local variables to simplify coding
  /wordlist exch def  /ymin exch def  /y exch def  /x exch def
  /wordindex 0 def % index to beginning of paragraph
  {wordlist wordindex columnline (after columnline: ) #only #stack
    x y moveto 2 index lineshow (after lineshow: ) #only #stack
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
  /source exch def  /y1 exch def  /y exch def  /x exch def
  (source: ) #only source ##only (, y1: ) #only y1 #only
  (, y: ) #only y #only (, x: ) #only x #
  /pcount zero
  {source paragraphs filter
    (column loop in progress) #
    1024 dup mul string
    readline not /eof exch def
    x y y1 4096 array () 6 -1 roll exch #stack string.split showparagraph
    exch /pindex exch def
    /pcount inc
    (column: paragraph count so far: ) #only pcount #
    % quit if column complete, or all data processed, or max paragraphs read
    eof {(exiting on EOF) # pop exit} if
    y1 lt {(exiting on column allocation complete) # exit} if
    pcount MAXPARAGRAPHS eq {(exiting on max paragraphs) # exit} if
    (not exiting, continuing column loop) #
  } loop
  (stack at end of column: ) #only ##stack
  pcount pindex
} bind def

/columns {  % source columns startcolumn (startcolumn is one-based)
  2 dict begin
  1 sub columnwidth mul margin add /x exch def
  dup ceiling /width def  % e.g., 1.5 columns means 2 column width
  end
} bind def

scriptname (columns) eq {
  (testing columnline) #
  16 array
  (This is a test of the ability of columnline to determine column fit.)
  () string.split 0 (before columnline: ) # #stack columnline
  (end of paragraph: ) #only #only (, line of text: ") #only #only
  (", new index: ) #only #
  (testing showparagraph) #
  margin  % x0
  pageheight margin sub lineheight sub  % y0
  dup lineheight 9 mul sub  % y1 (10 lines desired)
  128 array loremipsum () string.split  % words
  showparagraph
  (after show paragraph, y: ) #only #only (, index: ) #only #
  % /defaultdevice cvx 1 .quit
  (starting columns test program with stack: ) #only #stack
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
  (bytes available: ) #only datasource bytesavailable #
  margin columnwidth add  % x0
  pageheight margin sub lineheight sub % y0
  margin  % y1
  datasource column datasource closefile
  (now showing column on page) #
  showpage
  exch (final paragraph shown: ) #only #only (, word index: ) #only #
  (final stack: ) #only ##stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
