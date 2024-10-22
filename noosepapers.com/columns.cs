#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {pop LoremIpsum}
  ifelse /datasource exch def
/wordparse {
  <</EODCount 1 /EODString ( )>>
  /SubFileDecode
} bind def
scriptname (columns) eq {
  /buffer 128 string def
  datasource wordparse filter
  {dup buffer readstring
    {print}
    {print exit}
    ifelse
  } loop
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
