#!/usr/local/casperscript/bin/gs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. -C --
% NY POST banner font is about 1/12 page height
(pnmimage.cs) run
/inch {72 mul} def
/topmargin 0.25 inch def  % leave a little room at top of page
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
  (stack: ) print =stack
  moveto
  show
  grestore
  (stack: ) print =stack
} def

/banner {
  /CloisterBlack 47 selectfont
  save
  % first, a dry run to get the banner size
  % redefine `image` and `show` for that purpose
  /image {
    dup /Decode get /decoder exch def
    % translate all values to 1 (white)
    0 1 decoder length 1 sub {decoder exch 1 put} for
    (/Decoder: ) print dup /Decode get ===
    image
  } bind def
  /show {true charpath} def  % just append to path rather than show on page
  1 inch 7 inch moveto
  (first: ) print [pathbbox] == (The Noose ) show
  (stack after first: ) print =stack
  (second: ) print [pathbbox] == (gallows.pgm) (TNPp) pnminline
  (third: ) print [pathbbox] == ( Papers) show
  (final: ) print [pathbbox] ==
  pathbbox exch 4 -1 roll sub 3 1 roll exch sub
  (banner width, height for determining banner position: ) print  =stack
  pageheight exch sub topmargin sub exch  % y start of banner, x width
  pagewidth 2 div exch 2 div sub  % x start of banner
  exch  % put x and y in order for moveto
  3 -1 roll  % put --save-- to TOS
  restore
  moveto (The Noose ) show (gallows.pgm) (TNPp) pnminline ( Papers) show
  (stack: ) print =stack
} def

% test run using `cs -- banner`
scriptname (banner) eq {
  /CloisterBlack 47 selectfont
  (page height: ) print pageheight =
  (page width: ) print pagewidth =
  1 inch 1 inch moveto (The Noose Papers) false false centershow
  gsave .5 setgray  % 2 vertical centershows will overlap
  1 inch 2 inch moveto (The Noose Papers 2) true false centershow
  grestore
  1 inch 3 inch moveto (The Noose Papers 3) false true centershow
  1 inch 4 inch moveto (The Noose Papers 4) true true centershow
  banner
  showpage
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
