using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using ITW_MobileAppService.DataObjects;
using ITW_MobileAppService.Models;
using Owin;

namespace ITW_MobileAppService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new ITW_MobileAppInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            //Database.SetInitializer<ITW_MobileAppContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class ITW_MobileAppInitializer : CreateDatabaseIfNotExists<ITW_MobileAppContext>
    {
        protected override void Seed(ITW_MobileAppContext context)
        {

            List<EventItem> eventItems = new List<EventItem>
            {
                new EventItem { Id = Guid.NewGuid().ToString(), EventRecipients = "Employee One, Employee Two", EventDate = DateTime.Now, EventTime = "9:00 pm", Location = "Bruner", Category = "Meeting", EventPriority = "High", EventDescription = "description",EventID = 1, EmployeeID = 2, IsDeleted = false},
            };
            List<EmployeeItem> employeeItems = new List<EmployeeItem>
            {
                new EmployeeItem { Id = Guid.NewGuid().ToString(), Name = "Employee One", Email = "test@gmail.com", EmployeeID = 2, Department = "Test", PrivledgeLevel ="User"},
                new EmployeeItem { Id = Guid.NewGuid().ToString(), Name = "Employee Two", Email = "test2@gmail.com",  EmployeeID = 3, Department = "Test2", PrivledgeLevel ="User"},
            };
            List<RecipientListItem> recipientListItems = new List<RecipientListItem>
            {
                new RecipientListItem { Id = Guid.NewGuid().ToString(), EmployeeID = 2, EventID = 1 },
                new RecipientListItem { Id = Guid.NewGuid().ToString(), EmployeeID = 3, EventID = 1 },
            };
            List<EmployeeLoginItem> employeeLoginItems = new List<EmployeeLoginItem>
            {
                new EmployeeLoginItem { Id = Guid.NewGuid().ToString(), EmployeeID = 2, Hash = "1234", Salt = "abcd"},
                new EmployeeLoginItem { Id = Guid.NewGuid().ToString(), EmployeeID = 3, Hash = "1234",  Salt = "abcd"},
            };

            foreach (EventItem eventItem in eventItems)
            {
                context.Set<EventItem>().Add(eventItem);
            }
            foreach (EmployeeItem employeeItem in employeeItems)
            {
                context.Set<EmployeeItem>().Add(employeeItem);
            }
            foreach (RecipientListItem recipientListItem in recipientListItems)
            {
                context.Set<RecipientListItem>().Add(recipientListItem);
            }
            foreach (EmployeeLoginItem employeeLoginItem in employeeLoginItems)
            {
                context.Set<EmployeeLoginItem>().Add(employeeLoginItem);
            }

            base.Seed(context);
        }
    }
}

