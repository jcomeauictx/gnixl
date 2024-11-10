#!/usr/local/casperscript/bin/bccs
% font modified to use Latin-1 encoding
% PLRM page 349
/latin1font { %  fontname -
  % add Latin1 character set to font
  findfont
  dup length dict begin {
    1 index /FID ne  % skip font identifier
    {def}  % otherwise copy key-value pair
    {pop pop}  % drop FID
    ifelse
  } forall
  /Encoding ISOLatin1Encoding def
  currentdict
  end
  sys.argv 1 get (-Latin1) add cvn
  dup (Made ) #only #only ( from ) #only sys.argv 1 get #
  exch definefont pop
} bind def
scriptname (latin1font) eq {
  sys.argv 1 get cvn latin1font
  (stack at end: ) #only #stack
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr