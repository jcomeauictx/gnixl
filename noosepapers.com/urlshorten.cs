#!/usr/local/casperscript/bin/bccs --
/urlshorten  ( url - shorturl
  generate a short URL for a long one) docstring {
  rand 36 10 string cvrs string.lower dup
  3
    {
      {(../gnixl.com/l/) 1 index string.add dup 8#755 os.mkdir} stopped
        {(trying again, ) #only dup #only ( didn't work) # pop}
        {exit}
        ifelse
    }
    repeat
  (../gnixl.com/.htaccess) string.add (a) file
  #stack
  %dup (Redirect 301 ) 4 index string.add (\n) string.add writestring
  %closefile
} bind def

scriptname (urlshorten) eq
  {sys.argv 1 get urlshorten}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
