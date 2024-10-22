#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.cs) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {LoremIpsum}
  ifelse /datasource exch def
/buffer 128 string def
scriptname (columns) eq {
  {datasource buffer readstring
    {print}
    {print exit}
    ifelse
  } loop
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
