Grabbed an SVG logo, used Inkscape to produce a path:   (Expand, Diff, Convert to Bitmap, Trace Bitmap)
Manually connected discrete parts, manually turned into a single path.

Wrote svg2thr to convert, resize, and add automatic entrance.

Improvements:
   Think about how to write generic Shape -> Path code that doesn't require a bitmap convert
   Put the entrance/exit code somewhere general, add clear support too