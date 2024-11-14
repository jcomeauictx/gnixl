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
/columnheight pageheight margin dup add sub def
(column width: ) #only columnwidth #only (, height: ) #only columnheight #
/columnline {  % words index - endofparagraph newindex string
  %(starting columnline with stack: ) #only #stack
  /wordindex exch def
  /maxindex 1 index length 1 sub def
  /line 1024 string def
  {
    wordindex maxindex gt
      {
        (wordindex exceeds maxindex, stack: ) #only #stack
        exit
      }
      {
        %(stack at start of loop: ) #only #stack
        dup wordindex get dup xwidth spacewidth add
        line strcopy string.truncate  % string with new word appended
        xwidth add  % add to running pixel length total
        %dup (line width after addition would be ) #only #
        linewidth ge wordindex maxindex gt or %(stack after ge: ) #only #stack
          {pop exit}
          {line strlen 0 gt {line ( ) string.append} if
            line exch string.append /wordindex inc
            %(stack after append: ) #only  #stack
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
  %(starting showparagraph with stack: ) #only #stack
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

/column {  % x0 y0 y1 source words pcount pindex - words pcount pindex
  (creating column with stack: ) #only #stack
  7 dict begin  % for local variables
  /pindex exch def /pcount exch def /words exch def
  /source exch def  /y1 exch def  /y exch def  /x exch def
  (source: ) #only source ##only (, y1: ) #only y1 #only
  (, y: ) #only y #only (, x: ) #only x #
  /eof false def
  {source paragraphs filter
    pcount {dup 1024 16 mul string readline} repeat  % skip to next paragraph
    (column loop in progress) #
    words cvbool not
      {
        (refilling words, stack: ) #only #stack
        dup  % make copy of --file-- object
        1024 16 mul string readline not /eof exch def
        4096 array exch () string.split
        /words exch def  /pindex 0 def
      } if
    x y y1 words showparagraph
    exch /pindex exch def
    /pcount inc
    (column: paragraph count so far: ) #only pcount #
    % quit if column complete, or all data processed, or max paragraphs read
    eof {(exiting on EOF) # pop exit} if
    y1 lt {(exiting on column allocation complete) # exit} if
    pcount MAXPARAGRAPHS eq {(exiting on max paragraphs) # exit} if
    pop  % discard file object, we'll reopen it at top of loop
    (not exiting, continuing column loop, stack:) #only #stack
  } loop
  (stack at end of column: ) #only ##stack
  pcount pindex
  end  % end local variables dict
} bind def

/columns {  % columns startcolumn source - pcount pindex
  (starting columns with stack: ) #only #stack
  5 dict begin
  /source exch def
  % (startcolumn is one-based)
  1 sub columnwidth mul margin add /x exch def
  /words [] def  % empty so `column` knows to read source
  /pcount 0 def  /pindex 0 def
  dup ceiling cvi % e.g., 1.5 columns means 2 column width
  {
    x  % starting x of column
    pageheight margin dup add sub  % starting y of column
    0  % y1 of column (FIXME: may be larger if `columns` is fractional)
    source words pcount pindex
    column (after column: ) #only #stack
    /x x columnwidth add def
    /pindex exch def  /pcount exch def  /words exch def
  } repeat
  end
} bind def

scriptname (columns) eq {
  (starting columns test program with stack: ) #only #stack
  1 dict begin
  sys.argv dup length 1 gt
    {1 get (r) file}
    {pop LoremIpsum}
    ifelse /datasource exch def
  (bytes available: ) #only datasource bytesavailable #
  2.5 1 datasource columns
  (now showing columns on page) #
  showpage
  exch (final paragraph shown: ) #only #only (, word index: ) #only #
  (final stack: ) #only ##stack
  end
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
