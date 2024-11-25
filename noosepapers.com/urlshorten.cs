#! ../../casperscript/bin/bccs --
/urlshorten  ( url - shorturl
  generate a short URL for a long one) docstring {
  mark  % make it easy to clean stack regardless of where it failed
  3  % sufficient number of tries to find a unique random name
    {
      {
        rand 36 10 string cvrs string.lower
        (stack after creating directory name: ) #only #stack
        (../gnixl.com/l/) exch string.add dup 8#755
        (stack before os.mkdir: ) #only #stack
        os.mkdir
        (stack after os.mkdir: ) #only #stack
      }
      stopped
        {(trying again, stack: ) #only #stack cleartomark mark}
        {(succeeded, continuing) # exit}
        ifelse
    }
    repeat
  (after mkdir: ) #only #stack
  dup (/../.htaccess) string.add (a) file
  exch [2] substring  % chop '..' from start of relative path to make URL
  (after substring: ) #only #stack
  (https:/) exch string.add
  (after prepending scheme: ) #only #stack
  (Redirect 301 ) 5 -1 roll string.add  % "from" URL added
  (first part of redirect: ) #only #stack
  ( ) string.add 2 index string.add
  (\n) string.add writestring
  closefile
  #stack
} bind def

scriptname (urlshorten) eq
  (running urlshorten test program) #
  {sys.argv 1 get urlshorten}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
