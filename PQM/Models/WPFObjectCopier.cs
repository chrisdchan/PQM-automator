using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace PQM.Models
{
    public class WPFCanvasCopier
    {
        public static Canvas Clone(Canvas canvas)
        {
            String objXaml = XamlWriter.Save(canvas);
            StringReader stringReader = new StringReader(objXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Canvas clone = (Canvas)XamlReader.Load(xmlReader);
            return clone;
        }
    }
}
