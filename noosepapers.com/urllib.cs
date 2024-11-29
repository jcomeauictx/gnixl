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
