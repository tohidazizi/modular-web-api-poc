using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace ModularWebApi.Models
{
    public interface IModuleService
    {
        IEnumerable<Assembly> GetAssemblies();
        bool Install(string moduleUrl);
        void Load(string moduleName);
    }
}