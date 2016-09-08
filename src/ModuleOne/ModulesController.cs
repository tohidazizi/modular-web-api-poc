using System.Collections.Generic;
using System.Web.Http;

namespace ModuleOne
{
    // NOTE: With the current design, this cannot overwrite the original "ModulesController" in ModularWebApi project.
    [RoutePrefix("api/modules")]
    public class ModulesController : ApiController
    {
        [Route]
        public IEnumerable<string> Get()
        {
            return new[] { "A", "B" };
        }
    }
}