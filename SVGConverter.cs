using System;
using System.Windows;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using Path = System.Windows.Shapes.Path;

namespace FabricCuttingAutomationTool
{

   
    public class SVGConverter
    {
        public static void SaveAsSVG(string fileName,Canvas canvas)
        {

            StringBuilder data = new StringBuilder();
            data=data.Append("<svg width=\"" + canvas.ActualWidth + "\" height=\"" + canvas.ActualHeight + "\" viewBox=\"0 0 "+canvas.ActualWidth +" "+canvas.ActualHeight+"\" >");
            foreach (IShape item in canvas.Children)
            {
                
                foreach (var shapeItem in item.GetSVGCODE())
                {
                    data = data.Append(Environment.NewLine);
                    
                    data = data.Append(ConvertToSVG(shapeItem));
                }

            }
            data = data.Append(Environment.NewLine);

            data = data.Append("</svg>");

            string shapeData = data.ToString();
            File.WriteAllText(fileName, shapeData);

        }

        private static string ConvertToSVG(SVG SVGraphis)
        {
            if (SVGraphis.Shape.GetType() == typeof(Path))
            {
                return CreatePath(SVGraphis.Shape as Path, SVGraphis.Left, SVGraphis.Top, SVGraphis.Angle, SVGraphis.Center);
            }
            if (SVGraphis.Shape.GetType() == typeof(Polygon))
            {
                return CreatePlygon(SVGraphis.Shape as Polygon, SVGraphis.Left, SVGraphis.Top, SVGraphis.Angle, SVGraphis.Center);
            }
            if (SVGraphis.Shape.GetType() == typeof(Rectangle))
            {
                return CreateRectangle(SVGraphis.Shape as Rectangle, SVGraphis.Left, SVGraphis.Top, SVGraphis.Angle, SVGraphis.Center);
            }
            if (SVGraphis.Shape.GetType() == typeof(Line))
            {
                return CreateLine(SVGraphis.Shape as Line, SVGraphis.Left, SVGraphis.Top, SVGraphis.Angle, SVGraphis.Center);
            }
            return string.Empty;
        }

        private static string GetTransformString(double left, double top, double angle, Point center)
        {
            StringBuilder transform = new StringBuilder();
            transform = transform.Append("transform=\"");
            transform = transform.Append("translate("+left+" "+top+")");

            if(angle>0)
            transform = transform.Append(" rotate(" + angle + " " + center.X +" "+center.Y+ ")");


            transform = transform.Append("\"");
            return transform.ToString();
        }
    

        public static string CreatePath(Path data, double left, double top, double angle,Point center)
        {
            string pathData = "<path d=\"" + data.Data + "\"" + GetTransformString(left,top,angle,center)+" />";
            return pathData;
        }
      
        public static string CreatePlygon(string data, double left, double top, double angle, Point center)
        {
            string pathData =  "< polygon points =\"" + data + "\"" + GetTransformString(left, top, angle, center) + " />";
            return pathData;
        }
        public static string CreatePlygon(Polygon shape, double left, double top, double angle, Point center)
        {
            string data = string.Empty;
            foreach (var point in shape.Points)
            {
                data = data + point.X + "," + point.Y + " ";
            }
            string pathData = "< polygon points =\"" + data + "\"" + GetTransformString(left, top, angle, center) + " />";
            return pathData;
        }
        public static string CreateRectangle(Rectangle shape, double left, double top, double angle, Point center)
        {
          
            string pathData = "<rect width=\""+ shape.Width+" \" height=\""+shape.Height + "\"" + GetTransformString(left, top, angle, center) + " />";
            return pathData;
        }
        public static string CreateLine(Line line, double left, double top, double angle, Point center)
        {
            string pathData = "<line x1=\""+ line.X1+" \" y1=\""+line.Y1+" \" x2=\""+line.X2+" \" y2=\""+line.Y2 + "\"" + GetTransformString(left, top, angle, center) + " />";
            return pathData;
        }
    }
    public class SVG
    {
      
        public double Left { get; set; }
        public double Top { get; set; }
        public double Angle { get; set; }
        public Shape Shape { get; set; }
        public Point Center { get; set; }

    }


}
