#!/usr/local/casperscript/bin/gs -S -I. -sFONTMAP=noosefonts.ps -sFONTPATH=. -C --
% NY POST banner font is about 1/12 page height
(pnmimage.cs) run
/inch {72 mul} def
/margin 10 inch def  % leave a little room at top of page
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

/noosepapersbanner {
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
  pageheight exch sub margin sub exch  % y start of banner, x width
  pagewidth 2 div exch 2 div sub  % x start of banner
  exch  % put x and y in order for moveto
  3 -1 roll  % put --save-- to TOS
  restore
  moveto (The Noose ) show (gallows.pgm) (TNPp) pnminline ( Papers) show
  (stack: ) print =stack
} def

/bannerdraw {  % array dryrun - [pathbbox]
  % dryrun setup
  {
    % first, a dry run to get the banner size
    % redefine `image` and `show` for that purpose
    % (must be done in userdict to work! not local definitions)
    /image {
      dup /Decode get /decoder exch def
      % translate all values to 1 (white)
      0 1 decoder length 1 sub {decoder exch 1 put} for
      (/Decoder: ) print dup /Decode get ===
      image
    } bind def
    /show {true charpath} def  % just append to path rather than show on page
  }
  if
  0 1 2 index length 1 sub
    { % separate words and images with spaces
      (before get: ) #only #stack 1 index 1 index get (got: ) #only #stack
      exch (show space if not first word: ) #only #stack 0 gt {( ) show} if
      dup images exch known  % is this an image to be drawn?
        {
          (found image) #
          justwords () (before string.join: ) #only #stack
          string.join (after string.join: ) #only #stack
          (before pnminline: ) #only #stack pnminline
          ( ) show  % follow with a space
          (stack after showing space after pnmimage: ) #only #stack
        }
        {show}
        ifelse
    }
    for
  [pathbbox]
  exch
} bind def

/banner {  % string -
  % generic banner function
  10 dict begin  % for local variables, languagelevel 3 will grow as needed
  dup ( ) string.count 1 add array exch () string.split /bannerwords exch def
  /images bannerwords length dict def
  /justwords bannerwords length array def
  bannerwords {
    dup (.) string.count 0 gt 1 index os.path.exists and
      {images exch true put}
      {justwords exch array.append}
      ifelse
  }
  forall
  /justwords justwords array.truncate def  % eliminate any trailing nulls
  currentdict ###
  % first dry-run the banner at the top of the page. it won't make any
  % marks anyway, but if there's anything on the lower part of the page
  % we don't want to white it out with `image`
  0 pageheight moveto
  (before bannerwords true bannerdraw: ) #only #stack
  bannerwords true bannerdraw pathbbox
  (stack after pathbbox: ) #only #stack
  end
} bind def

% test run using `cs -- banner`
scriptname (banner) eq {
  /CloisterBlack 47 selectfont
  (page height: ) print pageheight =
  (page width: ) print pagewidth =
  sys.argv 1 get banner
  showpage
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
