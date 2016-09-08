using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ModularWebApi.Models;

namespace ModularWebApi.Controllers
{
    [RoutePrefix("api/modules")]
    public class ModulesController : ApiController
    {
        private readonly IModuleService _moduleService = new ModuleService();

        // GET: api/Modules
        [Route]
        public IEnumerable<string> Get()
        {
            return _moduleService.GetAssemblies().Select(a => a.FullName);
        }

        // POST: api/Modules/{module-id}
        [Route("{id}")]
        public void Post([FromUri]string id)
        {
            _moduleService.Load(id);
            GlobalConfiguration.Configuration.Routes.Clear();
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
            GlobalConfiguration.Configuration.Initializer(Configuration);
        }

        // DELETE: api/Modules/5
        [Route("{id}")]
        public void Delete([FromUri]string id)
        {
            throw new NotImplementedException();
        }
    }
}
