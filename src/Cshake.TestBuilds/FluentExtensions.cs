// -----------------------------------------------------------------------
// <copyright file="FluentExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Cshake.TestBuilds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fluent.IO;
    using System.Xml;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class FluentExtensions
    {
        public static Path XmlPoke(this Path path, string xpath, string value){

            if (path == null)
                return path;

            foreach (var file in path.AllFiles())
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file.FullPath);
                    
                    foreach(XmlNode n in doc.SelectNodes(xpath)){
                        n.Value = value;
                    }

                    doc.Save(file.FullPath);

                }catch(Exception ex){}
            }

            return path;
        }
    }
}
