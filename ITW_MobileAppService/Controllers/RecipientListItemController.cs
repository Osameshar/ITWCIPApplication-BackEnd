using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ITW_MobileAppService.DataObjects;
using ITW_MobileAppService.Models;
using System;
using System.Collections.Generic;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.Mobile.Server.Config;

namespace ITW_MobileAppService.Controllers
{
    [Authorize]
    public class RecipientListItemController : TableController<RecipientListItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ITW_MobileAppContext context = new ITW_MobileAppContext();
            DomainManager = new EntityDomainManager<RecipientListItem>(context, Request);
        }

        // GET tables/TodoItem
        public IQueryable<RecipientListItem> GetAllRecipientListItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<RecipientListItem> GetRecipientListItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<RecipientListItem> PatchRecipientListItem(string id, Delta<RecipientListItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostRecipientListItem(RecipientListItem item)
        {
            RecipientListItem current = await InsertAsync(item);
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings =
                this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
                .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = item.EventID + " is the Recipient Items EventID";

            try
            {
                // Send the push notification and log the results.
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                // Write the success result to the logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Write the failure result to the logs.
                config.Services.GetTraceWriter()
                    .Error(ex.Message, null, "Push.SendAsync Error");
            }

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteRecipientListItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}