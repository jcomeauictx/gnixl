#!/usr/local/casperscript/bin/gs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. -C
% NY POST banner font is about 1/12 page height
(pnmimage.cs) run
/CloisterBlack 47 selectfont
/inch {72 mul} def
2 inch 2 inch moveto
(The Noose Papers) show
2 inch 4 inch moveto
save
/image {pop} def
/show {true charpath} def
(The Noose ) show (gallows.pgm) (TNPp) pnminline ( Papers) show
(pathbbox: ) print [ pathbbox ] ==
restore
showpage
