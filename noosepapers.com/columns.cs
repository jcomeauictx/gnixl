#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {pop LoremIpsum}
  ifelse /datasource exch def
/wordfilter {
  (stack at wordfilter: ) print =stack
  {1 index (stack before read: ) print =stack read {dup 32 le if pop ( ) true} {false} ifelse}
  <</EODCount 1 /EODString ( )>>
  /SubFileDecode filter
} bind def
scriptname (columns) eq {
  /buffer 128 string def
  {datasource wordfilter buffer readstring
    {print}
    {print exit}
    ifelse
  } loop
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
