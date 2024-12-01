#!/usr/local/casperscript/bin/ccs --
/urllib 10 dict def urllib begin
  /parse 10 dict def parse begin
    /quote ( string#unquoted [string#safe string#unsafe?] - string#quoted
      like Python3's `urllib.parse.quote`
      but allows any range of safe and unsafe characters) docstring {
      10 dict begin  % for local variables
        /ALPHA [[latin1.A latin1.Z 1 add] range {chr} forall] () string.join
          [[latin1.a latin1.z 1 add] range {chr} forall] () string.join
          string.add def
        /DIGIT
          [[latin1.0 latin1.9 1 add] range {chr} forall] () string.join def
        /unreserved ALPHA DIGIT string.add (-._~) string.add def
        /gen-delims (:/?#[]@) def
        /sub-delims (!$&'()*+,;=) def
        /reserved gen-delims sub-delims string.add def
        /escape ( string#character - string#escaped
          escape the character, i.e. ( ) becomes (\04520)) docstring {
          ord (\045) exch 8 3 string cvrs string.append
        } bind def
      (stack before local defs: ) #only #stack
      dup /unsafe #stack exch {1 get} stopped {pop pop ()} if def
      /safe exch #stack {0 get} stopped {#stack pop pop ()} if def
      (safe: ) #only safe ##only (, unsafe: ) #only unsafe ##only
      (, unreserved: ) #only unreserved ##only
      (, reserved: ) #only reserved #
      % convoluted logic we're using:
      % IF marked as safe, don't escape it
      % ELIF marked as unsafe, escape it
      % ELIF in unreserved, don't escape it
      % ELIF in reserved, escape it
      % ELSE default is to leave it unescaped (*this may need to be changed*)
      dup length 3 mul string exch % allow for entire string to be escaped
      (remaining stack: ) #only #stack
      end  % local variables dict
    } bind def
  end  % urllib.parse
end  % urllib
scriptname (urllib) eq
  %urllib ###
  {
    {sys.argv 1 get} stopped {pop pop (this is a test)} if
    sys.argv [2] subarray urllib.parse.quote
  }
  if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
