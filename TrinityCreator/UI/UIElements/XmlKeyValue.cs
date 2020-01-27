using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TrinityCreator.Data;

namespace TrinityCreator.UI.UIElements
{
    public class XmlKeyValue : IKeyValue
    {
        public XmlKeyValue(int id, string description) 
            : base(id, description) { }

        public static IKeyValue[] FromXml(string name)
        {
            List<IKeyValue> result = new List<IKeyValue>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.cb_data);
            XmlElement target = (XmlElement)xml.SelectSingleNode("/KeyValues/" + name);
            foreach (XmlElement element in target.ChildNodes)
            {
                int id = 0;
                string description = "";

                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.Name == "Id")
                        id = int.Parse(attr.Value);
                    else if (attr.Name == "Description")
                        description = attr.Value;
                }
                result.Add(new XmlKeyValue(id, description));
            }
            return result.ToArray();
        }
    }
}
