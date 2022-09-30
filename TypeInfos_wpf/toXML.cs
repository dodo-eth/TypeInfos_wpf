 
using System.Collections.Generic; 
using System.Xml.Linq; 

namespace TypeInfos_wpf
{
    class toXML
    {
        public static void XMLWrite(List<string> Type, List<int> Offset, string filename)
        {
            //LINQ to XML
            XDocument doc = new XDocument(new XElement("root") ); 
             
            for (int i=0;i<Type.Count ;i++ )
                doc.Root.Add(new XElement("item", new XAttribute("Binding", "Introduced"), new XElement("node-path", Type[i].ToString()), new XElement("address", Offset[i])));
             
            doc.Save(filename);
        }
    }
}
