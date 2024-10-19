#!/usr/local/casperscript/bin/gs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. -C
% NY POST banner font is about 1/12 page height
(pnmimage.cs) run
/centershow { % bool bool -
  % show centered on page, vertically and/or horizontally
  /centerhorizontally exch def /centervertically exch def
  gsave
  dup true charpath
  (pathbbox: ) print [pathbbox] ==
  pathbbox /ury exch def /urx exch def /lly exch def /llx exch def
  grestore
  pagewidth 2 div /widthcenter exch def
  pageheight 2 div /heightcenter exch def
  /pathwidth urx llx sub def
  (pathwidth: ) print pathwidth =
  /pathheight ury lly sub def
  (pathheight: ) print pathheight =
  gsave
  centerhorizontally {
    (centering horizontally; ) print
    widthcenter pathwidth 2 div sub
  }{currentpoint pop} ifelse
  centervertically {
    (centering vertically; ) print
    heightcenter pathheight 2 div sub
  }{currentpoint exch pop} ifelse
  (moving to: ) print 2 copy 2 array astore ==
  (stack: ) print printstack
  moveto
  show
  grestore
  (stack: ) print printstack
} def
/CloisterBlack 47 selectfont
/inch {72 mul} def
(page height: ) print pageheight =
(page width: ) print pagewidth =
1 inch 1 inch moveto (The Noose Papers) false false centershow
1 inch 2 inch moveto (The Noose Papers 2) true false centershow
1 inch 3 inch moveto (The Noose Papers 3) false true centershow
1 inch 4 inch moveto (The Noose Papers 4) true true centershow
2 inch 10 inch moveto
save
/image {
  dup /Decode get /decoder exch def
  % translate all values to 1 (white)
  0 1 decoder length 1 sub {decoder exch 0 put} for
  (/Decoder: ) print dup /Decode get ===
  image
} bind def
/show {true charpath} def  % just append to path rather than show on page
(first: ) print [pathbbox] == (The Noose ) show
(second: ) print [pathbbox] == (gallows.pgm) (TNPp) pnminline
(third: ) print [pathbbox] == ( Papers) show
(final: ) print [pathbbox] ==
% pathbbox exch 4 -1 roll sub 3 1 roll exch sub pageheight printstack
restore
1 inch 10 inch moveto (The Noose ) show (gallows.pgm) (TNPp) pnminline ( Papers) show
(stack: ) print printstack
showpage
