#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
(latin1font.ps) run
sys.argv dup length 1 gt
0 1 currentfont /Encoding get 3 1 roll
  2 index length 1 sub dup /charmap exch dict def
  {1 index 1 index get charmap 3 1 roll exch put} for pop
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
  loop
  <</EODCount 0 /EODString VT>>
  /SubFileDecode
} bind def
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
scriptname (paragraphs) eq {
  {1 get (r) file}
  {pop loremipsum}
  ifelse /datasource exch def
  (testing paragraphs filter: ) =
  (federalistpapers1961hami.txt) (r) file
  (dumping federalist papers by paragraph) =
  paragraphs filter {
    dup 1024 string readstring pop dup length cvbool (paragraph: ) print =stack
    {=}
    {pop (exiting paragraphs loop) = exit}
    ifelse
  } loop
  (stack at end of paragraphs test: ) print =stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
