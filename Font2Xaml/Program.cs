using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Font2Xaml
{
    internal class Program
    {
        private const string SVG = ".svg";
        private const string TTF = ".ttf";

        private static void Main(string[] args)
        {
            if (args.Length <= 0) return;

            Console.WriteLine($"File recieved: {Path.GetFileName(args[0])}");
            if (Path.GetExtension(args[0]) == SVG)
            {
                Console.WriteLine("File is SVG.");
                SVGFont(args[0]);
            }
            else if (Path.GetExtension(args[0]) == TTF)
            {
                Console.WriteLine("File is TrueType.");
            }
        }

        private static void SVGFont(string path)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            XDocument doc = XDocument.Load(path);
            IEnumerable<XElement> elements = doc.Descendants();
            foreach (XElement element in elements)
            {
                if (!element.Name.LocalName.Contains("glyph")) continue;

                XAttribute name = element.Attribute("glyph-name");
                XAttribute data = element.Attribute("d");

                if (name == null || data == null) continue;
                GeometryDrawing drawing = new GeometryDrawing
                {
                    Brush = Brushes.Black,
                    Geometry = Geometry.Parse(data.Value)
                };
                DrawingImage drawingImage = new DrawingImage(drawing);

                dictionary.Add(name.Value, drawingImage);
            }

            using (XmlWriter writer = XmlWriter.Create(Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.xaml"),
                new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
            {
                XamlDesignerSerializationManager sm = new XamlDesignerSerializationManager(writer);
                sm.XamlWriterMode = XamlWriterMode.Expression;
                XamlWriter.Save(dictionary, writer);
            }
        }
    }
}