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
  % 2 consecutive endlines (LF, \012) we will interpret as paragraph
  % mark them with vertical tab (VT, \013) for paragraph marker
  % replace *single* endline with space (SP, \040)
  /source exch def
  /endlines zero
  /LF (\n) ord def
  /VT 8#13 chr def
  /P charmap /paragraph get chr def
  /SP ( ) def
  {source read (after read: ) print =stack
    {dup LF eq
      {pop SP /endlines inc (found LF: ) print =stack}
      {chr endlines 1 ge
        {endlines 2 ge
          {VT exch stradd (found paragraph: ) print =stack}
          {(ignoring LF: ) print =stack}
          ifelse
        } if  % (endline(s))
      }
      ifelse % (LF)
    (stack at end of paragraph filter procedure: ) print =stack
    }
    {VT}  % mark end of data with end-of-paragraph marker
    ifelse  % (read flag true or false)
  }  % end of filter procedure
  <</EODCount 0 /EODString VT>>
  /SubFileDecode
} bind def
/column {  % source -
  /source exch def
  /width pagewidth 5 div def
  /height pageheight 2 div def
  /line () def
  {source wordparse filter
    % readstring through wordparse filter will almost always return false
    % (only true with 128-character word)
    buffer readstring
      {buffer /rangecheck signalerror}  % word too long
      {
        /word exch def
        line word stradd (stack after line exch stradd: ) print =stack
        dup xwidth width (stack after dup xwidth width: ) print =stack gt
          {
            gsave
            (showing line at ) print currentpoint exch =only (,) print = 
            show /line word ( ) stradd def
            grestore
            0 -10 rmoveto
            currentpoint exch pop 0 lt {exit} if
          }
          {( ) (stack before append space: ) print =stack stradd /line exch def}
          ifelse  % row width > column width
      }
      ifelse  % readstring filled buffer
  }
  loop
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
  (testing paragraphs filter: ) =
  (federalistpapers1961hami.txt) (r) file
  (dumping federalist papers by paragraph) =
  paragraphs filter {
    dup 1024 string readstring pop dup length cvbool (paragraph: ) print =stack
    {=}
    {pop (exiting paragraphs loop) = exit}
    ifelse
  } loop
  (stack at end of columns test: ) print =stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
