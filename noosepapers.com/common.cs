#!/usr/local/casperscript/bin/ccs -S -I. --
/inch {72 mul} def
/loremipsum where {pop} {(loremipsum.cs) run} ifelse
/latin1font where {pop} {(latin1font.cs) run} ifelse
/paragraphs where {pop} {(paragraphs.cs) run} ifelse
/urlshorten where {pop} {(urlshorten.cs) run} ifelse
/banner where {pop} {(banner.cs) run} ifelse
/margin where {pop} {/margin 10 def} ifelse
/red {255 0 0} def
/green {0 255 0} def
/blue {0 0 255} def
/cyan {0 255 255} def
/yellow {255 255 0} def
/magenta {255 0 255} def
/black {0 0 0} bind def
/hr ( x y color -  % horizontal rule
  from HTML; for debugging vertical space problems) docstring {
  gsave setrgbcolor pagewidth 2 index dup add sub  % right margin = left
  3 1 roll #stack 0 #stack 3 1 roll #stack moveto #stack rlineto stroke #stack
  grestore
} bind def
[/TimesRoman-Latin1 /Helvetica-Latin1]
  {
  dup findfont /FontName get dup length string cvs (-Latin1) string.endswith
    {
      (creating ) #only dup #
      latin1font
    }
    if
  }
  forall
scriptname (common) eq {
  (common definitions complete) #
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
