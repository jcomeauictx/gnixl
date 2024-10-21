#!/usr/local/casperscript/bin/cs --
(lorem_ipsum.ps) run
sys.argv dup length 1 gt
  {1 get (r) file}
  {loremipsum << /EODCount 0 /EODString () >> /SubFileDecode filter}
  ifelse /datasource exch def
/buffer 128 string def
datasource buffer readstring
 {=}{= exit} ifelse
