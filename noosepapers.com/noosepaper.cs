#!/usr/local/casperscript/bin/cs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. --
/banner where {pop} {(banner.cs) run} ifelse
/columns where {pop} {(columns.cs) run} ifelse
/loremipsum where {pop} {(loremipsum.cs) run} ifelse
scriptname (noosepaper) eq {
  (The Noose gallows.pgm Papers) /CloisterBlack 47 banner
  2.5 0 LoremIpsum columns
  showpage
}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
