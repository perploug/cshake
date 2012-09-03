// -----------------------------------------------------------------------
// <copyright file="XmlPoke.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Cshake.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cshake.Core;
    using System.Xml;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XmlPoke : Task
    {
        public string XmlFile { get; set; }
        public string XPath { get; set; }
        public string Value { get; set; }

        public XmlPoke OpenXmlFile(string file){
            this.XmlFile = file;
            return this;
        }

        public XmlPoke QueryFor(string query)
        {
            XPath = query;
            return this;
        }

        public XmlPoke SetValue(string value)
        {
            Value = value;
            return this;
        }

        public override void ExecuteTask(IContext ctx)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFile);

            foreach(var x in doc.SelectNodes(XPath)){
                
            }
        }


        public override void Init(IContext cts)
        {
        }

    }
}
