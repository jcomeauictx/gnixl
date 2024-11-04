#!/usr/local/casperscript/bin/ccs --
(lorem_ipsum.cs) run
/EOF (D) ord 64 not and chr def  % control-D marks end of file
/paragraphs {  % source -
  % 2 consecutive endlines (LF, \012) we will interpret as paragraph
  % replace *single* endline with space (( ), \040)
  % `readline` returns substring true in normal case;
  % substring false at EOF;
  % rangecheck error if string filled before newline seen
  /source exch def
  {
    {[
      {source 8192 string %(before readline: ) print =stack
        readline %(after readline: ) print =stack
        not /eof exch def  % false means end-of-file
        dup strlen 0 eq  % empty string found
          {pop counttomark 0 eq  % only thing found so far?
            {(ignoring empty line preceding actual content) =}
            {pop  % remove space from end of previous line
              (stack at inner loop exit (end of paragraph): ) print =stack
              exit  % end of paragraph
            }
            ifelse
          }
          {
            % pdftotext replaces end-of-line hyphens with chr(0xac)
            % perhaps it meant chr(0xad), soft hyphen?
            dup strlen 1 sub 2 copy get 16#ac eq %(hyphenated? ) print =stack
              {0 exch getinterval}  % remove final character
              {pop ( )}  % remove length, and append space
              ifelse
          }
          ifelse
        eof {(exiting inner loop on EOF) = exit} if
      }  % end of paragraph read
      loop
      (joining fragments into paragraph, stack: ) print =stack
      ]  % create an array of the strings found
      %(stack before join: ) print =stack
      /paragraph 1024 1024 mul string def  % megabyte string to hold paragraph
      {paragraph exch append} forall paragraph truncate
      %(after join complete: ) print =stack
      dup strlen 0 gt
        {(\n) stradd}
        {pop EOF}
        ifelse
      (exiting outer loop) = exit
    }
    loop
    (outer loop ends) =
  }
  <</EODCount 1 /EODString EOF>>
  /SubFileDecode
} bind def
scriptname (paragraphs) eq {
  /count zero
  sys.argv dup length 1 gt
    {1 get (r) file}
    {pop LoremIpsum}
    ifelse /datasource exch def
  (testing paragraphs filter: ) =
  {datasource paragraphs filter
    1024 dup mul string
    (getting next paragraph, stack: ) print =stack
    readline not exch =
      {(datasource EOF reached) = exit}
      {(continuing text dump) =}
      ifelse
    /count inc count 10000 eq {exit} if  % quit test after 10000 paragraphs
  } loop
  (stack at end of columns test: ) print =stack
  (bytes available: ) print datasource bytesavailable =
  (final stack: ) print =stack
} if
(stack remaining at end of test: ) print =stack
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
