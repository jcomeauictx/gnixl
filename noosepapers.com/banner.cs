#!/usr/local/casperscript/bin/gs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. -C
% NY POST banner font is about 1/12 page height
(pnmimage.cs) run
/printstack {
  (current stack: ) print count array astore dup == aload pop
} bind def
/centershow { % bool bool -
  % show centered on page, vertically and/or horizontally
  /centerhorizontally exch def /centervertically exch def
  pathbbox /ury exch def /urx exch def /lly exch def /llx exch def
  pagewidth 2 div /widthcenter exch def
  pageheight 2 div /heightcenter exch def
  /pathwidth urx llx sub def
  /pathheight ury lly sub def
  gsave
  centerhorizontally {
    (centering horizontally;) print
    widthcenter pathwidth 2 div sub
  }{currentpoint pop} ifelse
  centervertically {
    (centering vertically;) print
    heightcenter pathheight 2 div sub
  }{currentpoint exch pop} ifelse
  (translating to: ) print 2 copy 2 array astore ==
  printstack
  translate
  show
  grestore
  printstack
} def
/CloisterBlack 47 selectfont
/inch {72 mul} def
2 inch 2 inch moveto
(The Noose Papers) dup dup dup show true true centershow true false centershow false true centershow
(
2 inch 4 inch moveto
save
/image {pop} def
/show {true charpath} def
(The Noose ) show (gallows.pgm) (TNPp) pnminline ( Papers) show
(pathbbox: ) print [ pathbbox ] ==
restore
) pop
showpage
