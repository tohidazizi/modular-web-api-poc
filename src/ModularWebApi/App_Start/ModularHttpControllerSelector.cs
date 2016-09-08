using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace ModularWebApi
{
    public class ModularHttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;

        public ModularHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            var result = base.GetControllerMapping();
            AddPluginsControllerMapping(ref result);
            return result;
        }

        private void AddPluginsControllerMapping(ref IDictionary<string, HttpControllerDescriptor> controllerMappings)
        {
            var controllerDic = GetControllerDescriptorDictionary();

            foreach (var item in controllerDic)
            {
                if (controllerMappings.ContainsKey(item.Key))
                    controllerMappings[item.Key] = item.Value;
                else
                    controllerMappings.Add(item.Key, item.Value);
            }
        }

        // NOTE: This method has been stolen from InitializeControllerInfoCache() private 
        //   method in System.Web.Http.Dispatcher.DefaultHttpControllerSelector class of 
        //   the ASP.NET Web API source code, with a little change.
        private ConcurrentDictionary<string, HttpControllerDescriptor> GetControllerDescriptorDictionary()
        {
            var result = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            var duplicateControllers = new HashSet<string>();
            Dictionary<string, ILookup<string, Type>> controllerTypeGroups = GetControllerTypeGroups();
            // NOTE: originally the above line was:
            //     Dictionary<string, ILookup<string, Type>> controllerTypeGroups = _controllerTypeCache.Cache;

            foreach (KeyValuePair<string, ILookup<string, Type>> controllerTypeGroup in controllerTypeGroups)
            {
                string controllerName = controllerTypeGroup.Key;

                foreach (IGrouping<string, Type> controllerTypesGroupedByNs in controllerTypeGroup.Value)
                {
                    foreach (Type controllerType in controllerTypesGroupedByNs)
                    {
                        if (result.Keys.Contains(controllerName))
                        {
                            duplicateControllers.Add(controllerName);
                            break;
                        }

                        result.TryAdd(controllerName, new HttpControllerDescriptor(_configuration, controllerName, controllerType));
                    }
                }
            }

            foreach (string duplicateController in duplicateControllers)
            {
                HttpControllerDescriptor descriptor;
                result.TryRemove(duplicateController, out descriptor);
            }

            return result;
        }

        // NOTE: This method has been stolen from InitializeCache() private method in 
        //   System.Web.Http.Dispatcher.HttpControllerTypeCache class of the
        //   ASP.NET Web API source code, with a little change.
        private Dictionary<string, ILookup<string, Type>> GetControllerTypeGroups()
        {
            IAssembliesResolver assembliesResolver = new DefaultAssembliesResolver();
            // NOTE: the above line was originally: 
            //     _configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = new DefaultHttpControllerTypeResolver();
            // NOTE: the above line was originally: 
            //     _configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
            var groupedByName = controllerTypes.GroupBy(
                t => t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length),
                StringComparer.OrdinalIgnoreCase);

            return groupedByName.ToDictionary(
                g => g.Key,
                g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}