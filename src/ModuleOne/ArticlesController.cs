using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ModuleOne
{
    [RoutePrefix("api/articles")]
    public class ArticlesController : ApiController
    {
        [Route]
        public IEnumerable<Article> Get()
        {
            return new Article[] { new Article(101), new Article(102) };
        }
    }
}
