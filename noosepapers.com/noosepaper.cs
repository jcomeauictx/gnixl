#!/usr/local/casperscript/bin/cs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. --
(loading noosepaper imports, banner...) #
/banner where {pop} {(banner.cs) run} ifelse
(loading noosepaper imports, columns...) #
/columns where {pop} {(columns.cs) run} ifelse
(loading noosepaper imports, loremipsum...) #
/loremipsum where {pop} {(loremipsum.cs) run} ifelse
(end loading noosepaper) #
scriptname (noosepaper) eq {
  (starting noosepaper: ) #only #stack
  (The Noose gallows.pgm Papers) /CloisterBlack 47 banner
  (now painting columns) #
  2.5 1 LoremIpsum columns
  showpage
}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
