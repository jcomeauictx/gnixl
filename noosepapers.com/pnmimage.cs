#!/home/default/bin/cs --
% insert pnm image at currentpoint
% code stolen from casperscript/vdiff.cs

/inch {72 mul} bind def
/zero (0) 0 get def  % value of ASCII zero (0x30)

/bits <<  % translate maxvalue into bits per sample
  1 1
  255 8
>> def

/decode <<  % Pn type into Decode matrix
  /P1 [0 1]
  /P2 [0 1]
  /P3 [0 1 0 1 0 1]
>> def

/binarize <<  % Pn type into binary stream
  /P1 {
    % read until (0), (1), or EOF
    {infile read
      {dup 1 not and zero eq
        {zero - 1 string dup 0 4 -1 roll put exit}
        {pop}
        ifelse}
      {() exit}
      ifelse
    } loop
  }
  /P2 {infile token {1 string dup 0 4 -1 roll put} {()} ifelse}
  /P3 {infile token {1 string dup 0 4 -1 roll put} {()} ifelse}
>> def

/colorspaces <<  % Pn type into colorspace
  /P1 /DeviceGray
  /P2 /DeviceGray
  /P3 /DeviceRGB
>> def

/pnm <<  % valid type 1 image dict once fleshed out
  /ImageType 1
  /Height 0
  /Width 0
  /ImageMatrix [/w 0 0 /-h 0 /h]  % placeholder for top-bottom left-right data
  /MultipleDataSources false  % optional (default) but let's be explicit
  /DataSource null  % replace with input stream
  /BitsPerComponent 1  % default for P1
  /Decode [0 1]  % map colors into range 0 to 1 ([0 1 0 1 0 1] for RGB)
  /Interpolate false  % another optional, default false
>> def

/comment (#) 0 get def  % comment introducer

/readpnm {  % filename - pnminstance colorspace
  % read PNM file into dict
  % we confine ourselves to gs-created pnm files, which only have a single
  % whole-line comment on 2nd line. this won't work with end-of-line comments.
  % netpbm programs pngtopnm and pnmnoraw don't insert such comments,
  % and this should work with those as well.
  pnm dup length dict copy /instance exch def  % new instance of pnm dictionary
  (r) file /infile exch def  % store open file handle in `infile`.
  /buffer 1024 string def  % hopefully enough for any PNM line length
  /pnmtype infile token pop cvlit def  % /P1, /P2, or /P3
  instance /Decode decode pnmtype get put
  /colorspace colorspaces pnmtype get def
  % read and discard the expected comment.
  % if not a comment, parse the width and height
  infile buffer readline pop dup 0 get comment ne
    {  % not a comment, parse width and height from string
      token pop instance /Width 3 -1 roll cvi dup /width exch def put
      token pop instance /Height 3 -1 roll cvi dup /height exch def put
      pop  % now-empty string
    }
    {  % comment found, toss it and parse width and height from infile
      pop  % discard comment
      instance /Width infile token pop cvi dup /width exch def put
      instance /Height infile token pop cvi dup /height exch def put
    }
  ifelse
  (stack remaining after processing 2nd line: ) = pstack
  instance /ImageMatrix get dup dup  % we're going to overwrite the /w etc.
    0 width put
    3 height neg put
    5 height put
  pnmtype (P1) ne {  % P2 and P3 have an extra line, max value
    instance /BitsPerComponent bits infile token pop cvi get put
  } if
  instance /DataSource binarize pnmtype get /ReusableStreamDecode filter put
  instance dup (instance: ) print === colorspace
} bind def

/pnmimage { % filename -
  % insert image from pnm (pgm, pbm) file on page at currentpoint
  readpnm setcolorspace dup dup /Width get exch /Height get
  currentpoint translate scale image
} bind def

% test run using `cs -- pnmimage gallows.pgm`
scriptname (pnmimage) eq {
  1 inch 1 inch moveto
  argv 1 get pnmimage
  showpage
} if
% vim: tabstop=8 shiftwidth=2 expandtab softtabstop=2
