#! ../../casperscript/bin/bccs --
/MAX_TRIES 10 def
/urlshorten  ( url - shorturl true OR url false
  generate a short URL for a long one) docstring {
  mark  % make it easy to clean stack regardless of where it failed
  MAX_TRIES  % sufficient number of tries to find a unique random name
    {
      {
        urand 36 10 string cvrs string.lower
        (stack after creating directory name: ) #only #stack
        (../gnixl.com/l/) exch string.add dup 8#755
        (stack before os.mkdir: ) #only #stack
        os.mkdir
        (stack after os.mkdir: ) #only #stack
      }
      stopped
        {(trying again, stack: ) #only #stack cleartomark mark}
        {(succeeded, continuing) # true exit}  % mark success with `true`
        ifelse
    }
    repeat
  (after mkdir: ) #only #stack
  true ne  % failure would leave only a -mark- on stack
    {(failed after ) #only MAX_TRIES #only ( attempts) # false}
    {
      dup (/../../.htaccess) string.add (a) file
      exch [12] substring  % chop first part of path to form URL
      (after substring: ) #only #stack
      (after forming URL: ) #only #stack
      (Redirect 301 ) 1 index string.add  % "from" URL added
      (first part of redirect: ) #only #stack
      ( ) string.add 5 -1 roll string.add #stack
      (\n) string.add 2 index exch writestring #stack
      exch closefile #stack
      exch pop  % discard `mark`
      (gnixl.com) exch string.add
      true
    } ifelse
    (stack after urlshorten: ) #only #stack
} bind def

scriptname (urlshorten) eq
  (running urlshorten test program) #
  {sys.argv 1 get urlshorten}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
