using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Umbraco.Courier.Core;
using Umbraco.Courier.Core.Storage;
using Umbraco.Courier.Core.Packaging;
using Umbraco.Courier.Core.Collections.Manifests;

namespace Cshake.Courier.Tasks
{
    public class Package : Cshake.Core.Task
    {
        public string Config { get; set; }
        public string Source { get; set; }
        public string Revision { get; set; }
        public string Manifest { get; set; }

        static string application = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static string directory = Directory.GetCurrentDirectory();
        static string plugins = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "courierplugins");

        private Cshake.Core.Context _ctx;

        public Package(string config, string source, string revision, string manifest)
        {
            Config = config;
            Source = source;
            Revision = revision;
            Manifest = manifest;
        }

        public override void Init(Core.Context cts)
        {
            _ctx = cts;

            //To init our app, we need to load all provider dlls from our plugins dir
            //The application needs the DLL umbraco.courier.providers.dll to work with the build-in providers
            //you can add any dll in there to load your own

            Umbraco.Courier.Core.Helpers.TypeResolver.LoadAssembliesIntoAppDomain(plugins, "*.dll");

            //we also need to tell it where to get it's config xml
            //this is the standard courier file which contains settings for the engines and providers
            Umbraco.Courier.Core.Context.Current.SettingsFilePath = Config;

            //finally we need to redirect the revisions root for correct mapping
            Umbraco.Courier.Core.Context.Current.BaseDirectory = directory;
            Umbraco.Courier.Core.Context.Current.HasHttpContext = false;
        }

        public override void ExecuteTask(Core.Context ctx)
        {
            try
            {
                Repository source = null;

                using (var rs = new RepositoryStorage())
                {
                    source = rs.GetByAlias(Source);
                }

                var engine = new RevisionPackaging(Revision);
                engine.Source = source;

                engine.StoredItem += new EventHandler<ItemEventArgs>(engine_StoredItem);
                engine.SkippedItem += new EventHandler<ItemEventArgs>(engine_SkippedItem);

                _ctx.Important("Loading manifest...");
                var manifest = RevisionManifest.Load(Manifest);
                engine.AddToQueue(manifest);

                _ctx.Important("Starting packaging...");
                engine.Package();
                engine.Dispose();

                _ctx.Success("All done, yay!");
            }
            catch (Exception ex)
            {
                _ctx.Error(ex);
            }
        }

        void engine_SkippedItem(object sender, ItemEventArgs e)
        {
            _ctx.Error("Skipped: " + e.Item.Name);
        }

        void engine_StoredItem(object sender, ItemEventArgs e)
        {
            _ctx.Success("Packaged: " + e.Item.Name);
        }

    }
}
