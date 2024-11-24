#! ../../casperscript/bin/bccs --
/urlshorten  ( url - shorturl
  generate a short URL for a long one) docstring {
  rand 36 10 string cvrs string.lower dup
  (stack after creating directory name: ) #only #stack
  3
    {
      {
        (../gnixl.com/l/) 1 index string.add dup 8#755
        (stack before os.mkdir: ) #only #stack
        os.mkdir
        (stack after os.mkdir: ) #only #stack
      }
      stopped
        {(trying again, stack: ) #only #stack pop}
        {(succeeded, continuing) # exit}
        ifelse
    }
    repeat
  (after mkdir: ) #only #stack
  (../gnixl.com/.htaccess) string.add (a) file
  #stack
  %dup (Redirect 301 ) 4 index string.add (\n) string.add writestring
  %closefile
} bind def

scriptname (urlshorten) eq
  (running urlshorten test program) #
  {sys.argv 1 get urlshorten}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
