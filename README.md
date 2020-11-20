# Font2Xaml
Found myself needing access to icons for XAML, tried Icon fonts but implementation felt clunky and so I then tried MetroStudio, but soon realised there wasn't enough variety.
Font2Xaml parses the XML SVG fonts into a ResourcesDictionary of DrawingImages wrapped up in a XAML file, this means you technically have unlimited icons you can use, assuming you can find a font for them on the internet.

### Usage
1. Download an SVG icon font there's 100s of these which are completely free scattered around the web, but my method is to build a custom font (I use https://fontello.com) and download that.
2. Build Font2Xaml and drag the "your-font.SVG" file onto the executable to create "your-font.XAML".
3. Copy "your-font.XAML" into your WPF project and reference it in your App.XAML (https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/how-to-use-an-application-scope-resource-dictionary?view=netframeworkdesktop-4.8).

### Notes
- This only works for SVG fonts and SVGs themselves can be imported using other programs.
- Color is limited to black by default with a thickness of 0.75.
- Make sure you have permission for the fonts you use.
