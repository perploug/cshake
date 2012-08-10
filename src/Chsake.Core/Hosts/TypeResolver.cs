using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Cshake.Core.Hosts
{
    [Serializable]
    internal class TypeResolver : MarshalByRefObject
    {
        #region Methods (4)

        // Public Methods (4) 

        /// <summary>
        /// Gets a collection of assignables of type T from a collection of files
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public static string[] GetAssignablesFromType<T>(string[] files)
        {

            AppDomain sandbox = AppDomain.CurrentDomain;
           
            try
            {
                TypeResolver typeResolver = (TypeResolver)sandbox.CreateInstanceAndUnwrap(
                    typeof(TypeResolver).Assembly.GetName().Name,
                    typeof(TypeResolver).FullName);

                return typeResolver.GetTypes(typeof(T), files);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                /*
                if ((!GlobalSettings.UseMediumTrust) && (GlobalSettings.ApplicationTrustLevel > AspNetHostingPermissionLevel.Medium))
                {
                    AppDomain.Unload(sandbox);
                }
                 */
            }

            return new string[0];
        }

        /// <summary>
        /// Gets a collection of assignables of type T from a collection of a specific file type from a specified path.
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="filePattern">The file pattern.</param>
        /// <returns></returns>
        public static string[] GetAssignablesFromType<T>(string path, string filePattern)
        {
            FileInfo[] fis = Array.ConvertAll<string, FileInfo>(
                Directory.GetFiles(path, filePattern),
                delegate(string s) { return new FileInfo(s); });
            string[] absoluteFiles = Array.ConvertAll<FileInfo, string>(
                fis, delegate(FileInfo fi) { return fi.FullName; });
            return GetAssignablesFromType<T>(absoluteFiles);
        }

        /// <summary>
        /// Returns of a collection of type names from a collection of assembky files.
        /// </summary>
        /// <param name="assignTypeFrom">The assign type.</param>
        /// <param name="assemblyFiles">The assembly files.</param>
        /// <returns></returns>
        public string[] GetTypes(Type assignTypeFrom, string[] assemblyFiles)
        {
            List<string> result = new List<string>();
            foreach (string fileName in assemblyFiles)
            {
                if (!File.Exists(fileName))
                    continue;
                try
                {
                    Assembly assembly = Assembly.LoadFile(fileName);

                    if (assembly != null)
                    {
                        foreach (Type t in assembly.GetTypes())
                        {
                            if (!t.IsInterface && assignTypeFrom.IsAssignableFrom(t))
                                result.Add(t.AssemblyQualifiedName);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format("Error loading assembly: {0}\n{1}", fileName, e));
                }
            }

            /*
            try{
                System.Collections.IList list = System.Web.Compilation.BuildManager.CodeAssemblies;
                if (list != null && list.Count > 0) {
                    Assembly asm;
                    foreach (object o in list) {
                        asm = o as Assembly;

                        Log.Add(LogTypes.Debug, -1, "assembly " + asm.ToString()  );
                        if (asm != null) {
                            foreach (Type t in asm.GetTypes()) {
                                if (!t.IsInterface && assignTypeFrom.IsAssignableFrom(t))
                                    result.Add(t.AssemblyQualifiedName);
                            }
                        }
                    }
                } else {
                    Log.Add(LogTypes.Debug, -1, "No assemblies");
                }
            } catch(Exception ee) {
                Log.Add(LogTypes.Debug, -1, ee.ToString());
            }
            */

            return result.ToArray();
        }

        public static void LoadAssembliesIntoAppDomain(string path, string filePattern)
        {
            PreLoadAssembliesFromPath(path);
        }

        private static IEnumerable<string> GetBinFolders()
        {
            //TODO: The AppDomain.CurrentDomain.BaseDirectory usage is not correct in 
            //some cases. Need to consider PrivateBinPath too
            List<string> toReturn = new List<string>();
            //slightly dirty - needs reference to System.Web.  Could always do it really
            //nasty instead and bind the property by reflection!
            //TODO: as before, this is where the PBP would be handled.
            toReturn.Add(AppDomain.CurrentDomain.BaseDirectory);
            
            return toReturn;
        }

        private static void PreLoadDeployedAssemblies()
        {
            foreach (var path in GetBinFolders())
            {
                PreLoadAssembliesFromPath(path);
            }
        }

        private static void PreLoadAssembliesFromPath(string p)
        {
            //S.O. NOTE: ELIDED - ALL EXCEPTION HANDLING FOR BREVITY

            //get all .dll files from the specified path and load the lot
            FileInfo[] files = null;
            //you might not want recursion - handy for localised assemblies 
            //though especially.
            files = new DirectoryInfo(p).GetFiles("*.dll",
                SearchOption.AllDirectories);

            AssemblyName a = null;
            string s = null;
            foreach (var fi in files)
            {
                s = fi.FullName;
                //now get the name of the assembly you've found, without loading it
                //though (assuming .Net 2+ of course).
                a = AssemblyName.GetAssemblyName(s);
                //sanity check - make sure we don't already have an assembly loaded
                //that, if this assembly name was passed to the loaded, would actually
                //be resolved as that assembly.  Might be unnecessary - but makes me
                //happy :)
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                  AssemblyName.ReferenceMatchesDefinition(a, assembly.GetName())))
                {
                    //crucial - USE THE ASSEMBLY NAME.
                    //in a web app, this assembly will automatically be bound from the 
                    //Asp.Net Temporary folder from where the site actually runs.
                    Assembly.Load(a);
                }
            }
        }

        #endregion Methods
    }
}
