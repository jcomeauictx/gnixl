#!/usr/local/casperscript/bin/ccs --
  scriptname (columns) eq {
  (lorem_ipsum.cs) run
  sys.argv dup length 1 gt
    {1 get (r) file false}
    {LoremIpsum true}
    ifelse /infinite exch def /datasource exch def
  /buffer 128 string def
  {datasource buffer readstring
    {print}
    {
      infinite
        {print datasource resetfile}
        {print exit}
      ifelse
    }
    ifelse
  } loop
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
