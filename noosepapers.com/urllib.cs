#!/usr/local/casperscript/bin/ccs --
/urllib 10 dict begin
  /parse 10 dict begin
    /quote ( string#unquoted [string#safe string#unsafe?] - string#quoted
      like Python3's `urllib.parse.quote`
      but allows any range of safe and unsafe characters) docstring {
      10 dict begin  % for local variables
        /ALPHA [[latin1.A latin1.Z 1 add] range {chr} forall] () string.join
          [[latin1.a latin1.z 1 add] range {chr} forall] () string.join
          string.add def
        /DIGIT
          ([latin1.0 latin1.9 1 add] range {chr} forall] () string.join def
        /unreserved ALPHA DIGIT string.add (-._~) string.add def
        /gen-delims (:/?#[]@) def
        /sub-delims (!$&'()*+,;=) def
        /reserved gen-delims sub-delims string.add def
        
      end  % local variables dict
      dup /unsafe {1 get} stopped {()} def
      /safe {0 get} stopped {()} def
      (safe: ) #only safe ##only (, unsafe: ) #only unsafe ##only
      (, unreserved: ) #only unreserved ##only
      (, reserved: ) #only reserved #
      (unquoted: ) #only #
    } bind def
  end def  % urllib.parse
end def  % urllib
scriptname (urllib) eq
  {sys.argv 1 get sys.argv 2 subarray urllib.parse.quote}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
