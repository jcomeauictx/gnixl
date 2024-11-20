#!/usr/local/casperscript/bin/cs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. --
(loading noosepaper imports, banner...) #
/banner where {pop} {(banner.cs) run} ifelse
(loading noosepaper imports, columns...) #
/columns where {pop} {(columns.cs) run} ifelse
(loading noosepaper imports, loremipsum...) #
/loremipsum where {pop} {(loremipsum.cs) run} ifelse
(end loading noosepaper) #
scriptname (noosepaper) eq {
  /margin where {pop} {/margin 10 def} ifelse
  /fontsize where {pop} {/fontsize 12 def} ifelse
  /lineheight where
    {pop}
    {/lineheight {currentfont font.size 1.5 mul floor} def}
    ifelse
  (starting noosepaper: ) #only #stack
  {sys.argv 1 get} stopped {pop (The Noose gallows.pgm Papers)} if
  gsave /CloisterBlack 47 banner grestore
  (now painting columns) #
  /TimesRoman 15 selectfont
  % first subtrace margin and lineheight from banner baseline
  lineheight sub margin sub
  2.5 1  % 2.5 columns, starting at column 1
  {sys.argv 2 get (r) file} stopped {LoremIpsum} if (Headline Goes Here) columns
  showpage
}
if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
