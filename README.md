# Font2Xaml
Font2Xaml parses SVG fonts into a ResourcesDictionary of DrawingImages wrapped up in a XAML file.

### Usage
1. Download an SVG icon font there's 100s of these which are completely free scattered around the web, but my method is to build a custom font (I use https://fontello.com) and download that.
2. Build Font2Xaml (or [download](https://github.com/Swegrock/Font2Xaml/releases/download/v0.0.0.1/Font2Xaml.exe)) and drag the "your-font.SVG" file onto the executable to create "your-font.XAML".
3. Copy "your-font.XAML" into your WPF project and reference it in your App.XAML (https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/how-to-use-an-application-scope-resource-dictionary?view=netframeworkdesktop-4.8).

### Advanced
For advanced usage run: `Font2Xaml help`.

### Notes
- This only works for SVG fonts and SVGs themselves can be imported using other programs.
- Make sure you have permission for the fonts you use.
