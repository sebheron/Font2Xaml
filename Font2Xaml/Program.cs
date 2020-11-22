using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Reflection;

namespace Font2Xaml
{
    internal class Program
    {
        private const string SVG = ".svg";

        private static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("No file specified. Type 'Font2Xaml help'.");
                return;
            }
            else if (args[0] == "help")
            {
                Console.WriteLine("Font2Xaml\n---------"
                    + "\nUsage: Font2Xaml <svg-file-location> <color>..."
                    + "\n<svg-file-location> should be the file location of the SVG font."
                    + "\n<color> should be written as a,r,g,b or r,g,b. Each value should be between 0 and 1."
                    + "\nEach <color> will be iterated through according to the amount of characters in the supplied font.");
                return;
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found. Type 'Font2Xaml help'.");
            }
            List<SolidColorBrush> brushes = new List<SolidColorBrush>();
            for (int i = 1; i < args.Length; i++)
            {
                string[] arbgvals = args[i].Split(',');
                Console.WriteLine(String.Join(",", arbgvals));
                if (arbgvals.Length == 3 && float.TryParse(arbgvals[0].ToString(), out float r) && float.TryParse(arbgvals[1].ToString(), out float g) && float.TryParse(arbgvals[2].ToString(), out float b))
                {
                    brushes.Add(new SolidColorBrush(Color.FromScRgb(1, r, g, b)));
                    Console.WriteLine($"Color added: r={r} g={g} b={b}");
                }
                else if (arbgvals.Length == 4 && float.TryParse(args[0].ToString(), out float a) && float.TryParse(arbgvals[1].ToString(), out float rr) && float.TryParse(arbgvals[2].ToString(), out float gg) && float.TryParse(arbgvals[3].ToString(), out float bb))
                {
                    Console.WriteLine($"Color added with alpha: a={a} r={rr} g={gg} b={bb}");
                    brushes.Add(new SolidColorBrush(Color.FromScRgb(a, rr, gg, bb)));
                }
                else
                {
                    Console.WriteLine("Invalid color used. 'Type Font2Xaml help'.");
                    return;
                }
            }

            if (brushes.Count <= 0)
            {
                brushes.Add(Brushes.Black);
                Console.WriteLine("No colors supplied. Defaulting to black");
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine();
            }

            Console.WriteLine($"File recieved: {Path.GetFileName(args[0])}");
            if (Path.GetExtension(args[0]) == SVG)
            {
                Console.WriteLine("File is SVG");
                SVGFont(args[0], brushes);
            }
        }

        private static void SVGFont(string path, List<SolidColorBrush> brushes)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            XDocument doc = XDocument.Load(path);
            IEnumerable<XElement> elements = doc.Descendants().Where(x => x.Name.LocalName.Contains("glyph"));
            int brushCount = 0;

            foreach (XElement element in elements)
            {
                SolidColorBrush brush = brushes[brushCount % brushes.Count];
                XAttribute name = element.Attribute("glyph-name");
                XAttribute data = element.Attribute("d");

                if (name == null || data == null) continue;
                GeometryDrawing drawing = new GeometryDrawing
                {
                    Brush = brush,
                    Geometry = Geometry.Combine(Geometry.Empty, Geometry.Parse(data.Value), GeometryCombineMode.Union, new ScaleTransform(1, -1))
                };

                DrawingImage drawingImage = new DrawingImage(drawing);
                dictionary.Add(name.Value, drawingImage);
                brushCount++;
            }

            Console.WriteLine($"Successfully converted {dictionary.Count} characters");
            if (brushCount < brushes.Count) Console.WriteLine($"Too many colors were supplied. {brushes.Count - brushCount} colors were ignored");

            using (XmlWriter writer = XmlWriter.Create(Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.xaml"),
                new XmlWriterSettings { Indent = true }))
            {
                XamlDesignerSerializationManager sm = new XamlDesignerSerializationManager(writer);
                sm.XamlWriterMode = XamlWriterMode.Expression;
                XamlWriter.Save(dictionary, writer);
            }
        }
    }
}