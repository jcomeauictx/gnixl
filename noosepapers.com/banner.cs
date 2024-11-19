#!/usr/local/casperscript/bin/gs -S -I. -sFONTMAP#noosefonts.ps -sFONTPATH#. -C --
% NY POST banner font is about 1/12 page height
(pnmimage.cs) run
/inch {72 mul} def
/margin 10 def  % leave a little room at top of page
/centershow { % bool bool -
  % show centered on page, vertically and/or horizontally
  /centerhorizontally exch def /centervertically exch def
  gsave
  dup true charpath
  (pathbbox: ) #only [pathbbox] ##
  pathbbox /ury exch def /urx exch def /lly exch def /llx exch def
  grestore
  pagewidth 2 div /widthcenter exch def
  pageheight 2 div /heightcenter exch def
  /pathwidth urx llx sub def
  (pathwidth: ) #only pathwidth #
  /pathheight ury lly sub def
  (pathheight: ) #only pathheight #
  gsave
  centerhorizontally {
    (centering horizontally; ) #only
    widthcenter pathwidth 2 div sub
  }{currentpoint pop} ifelse
  centervertically {
    (centering vertically; ) #only
    heightcenter pathheight 2 div sub
  }{currentpoint exch pop} ifelse
  (moving to: ) #only 2 copy 2 array astore ##
  (stack: ) #only #stack
  moveto
  show
  grestore
  (stack: ) #only #stack
} def

/bannerdraw {  % dryrun array - pathbbox
  % dryrun setup
  save 2 index (bannerdraw stack: ) #only #stack
  {
    % first, a dry run to get the banner size
    % redefine `image` and `show` for that purpose
    % (must be done in userdict to work! not local definitions)
    /image {
      dup /Decode get /decoder exch def
      % translate all values to 1 (white)
      0 1 decoder length 1 sub {decoder exch 1 put} for
      (/Decoder: ) #only dup /Decode get ###
      image
    } bind def
    /show {
      true charpath
    } bind def  % just append to path rather than show on page
  }
  if
  1 index 0 1 2 index length 1 sub  % setup `for` loop
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
  pop  % toss copy of word array
  (pathbbox before restore: ) #only [pathbbox] ##
  pathbbox 5 -1 roll
  restore
  6 -2 roll pop pop
  (bannerdraw end stack: ) #only #stack
} def

/banner {  % string - textbottom
  % generic banner function; returns bottom of banner
  % (for use in layout of remainder of page)
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
  (before true bannerwords bannerdraw: ) #only #stack
  true bannerwords bannerdraw
  (stack after pathbbox: ) #only #stack
  % calculate width/height from pathbbox
  2 index pageheight sub /descender exch def
  (descender: ) #only descender #
  exch 4 -1 roll sub 3 1 roll exch sub
  (banner width, height for determining banner position: ) #only #stack
  % calculate y baseline
  pageheight exch sub margin sub
  % return textbottom to caller, to determine columns top
  /textbottom 1 index def
  descender sub  % y start of banner
  (banner y top: ) #only dup #
  exch  % swap to calculate x center
  pagewidth 2 div exch 2 div sub  % x start of banner
  (banner x left: ) #only dup #
  exch  % x and y in order for moveto
  (banner: stack before moveto: ) #only #stack
  moveto false bannerwords bannerdraw
  pop pop pop pop  % toss pathbbox
  textbottom
  (banner final stack: ) #only #stack
  end
} bind def

% test run using `cs -- banner`
scriptname (banner) eq {
  /CloisterBlack 47 selectfont
  (page height: ) #only pageheight #
  (page width: ) #only pagewidth #
  {sys.argv 1 get} stopped
    {
      (no banner text supplied, using (Dolorem Ipsum)) #
      (Dolorem Ipsum)
    }
    if
  banner
  showpage
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2 syntax=postscr
