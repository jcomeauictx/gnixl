#!/usr/local/casperscript/bin/cs -S -I. --
/loremipsum where {pop} {(loremipsum.cs) run} ifelse
/latin1font where {pop} {(latin1font.cs) run} ifelse
/paragraphs where {pop} {(paragraphs.cs) run} ifelse
/urlshorten where {pop} {(urlshorten.cs) run} ifelse
/banner where {pop} {(banner.cs) run} ifelse
/margin where {pop} {/margin 10 def} ifelse
/red {255 0 0} def
/green {0 255 0} def
/blue {0 0 255} def
/hr {  % x y color -  % horizontal rule, for debugging vertical space problems
  gsave setrgbcolor pagewidth 2 index dup add sub  % right margin = left
  3 1 roll #stack 0 #stack 3 1 roll #stack moveto #stack rlineto stroke #stack
  grestore
} bind def
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
