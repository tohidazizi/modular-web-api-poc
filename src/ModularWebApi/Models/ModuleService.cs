using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace ModularWebApi.Models
{
    public class ModuleService : IModuleService
    {
        public const string ModulesPath = "~/PlugIns/";

        private static readonly IDictionary<string, Assembly> _modules =
            new ConcurrentDictionary<string, Assembly>();

        public IEnumerable<Assembly> GetAssemblies()
        {
            return _modules.Select(m => m.Value);
        }

        public void Load(string moduleName)
        {
            var mappedPath = HostingEnvironment.MapPath(ModulesPath + moduleName + ".dll");

            if (!File.Exists(mappedPath))
                throw new FileNotFoundException();

            var assembly = Assembly.LoadFile(mappedPath);
            _modules.Add(moduleName, assembly);
            AppDomain.CurrentDomain.Load(assembly.GetName());
        }

        internal void Remove(string id)
        {
            _modules.Remove(id);
        }

        public bool Install(string moduleUrl)
        {
            throw new NotImplementedException();
        }
    }
}