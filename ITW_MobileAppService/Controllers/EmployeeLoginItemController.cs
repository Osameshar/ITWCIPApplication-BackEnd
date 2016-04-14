using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ITW_MobileAppService.DataObjects;
using ITW_MobileAppService.Models;

namespace ITW_MobileAppService.Controllers
{
    public class EmployeeLoginItemController : TableController<EmployeeLoginItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ITW_MobileAppContext context = new ITW_MobileAppContext();
            DomainManager = new EntityDomainManager<EmployeeLoginItem>(context, Request);
        }

        // GET tables/TodoItem
        public IQueryable<EmployeeLoginItem> GetAllEmployeeLoginItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<EmployeeLoginItem> GetEmployeeLoginItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<EmployeeLoginItem> PatchEmployeeLoginItem(string id, Delta<EmployeeLoginItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostEmployeeLoginItem(EmployeeLoginItem item)
        {
            EmployeeLoginItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteEmployeeLoginItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}