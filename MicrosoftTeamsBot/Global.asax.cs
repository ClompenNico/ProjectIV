using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using MicrosoftTeamsBot.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MicrosoftTeamsBot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Use an in-memory store for bot data.
            // This registers a IBotDataStore singleton that will be used throughout the app.

            //var config = GlobalConfiguration.Configuration;

            Conversation.UpdateContainer(builder =>
                {
                    //builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    var store = new SqlBotDataStore(ConfigurationManager.ConnectionStrings["BotDataContextConnectionString"].ConnectionString);

                    //var store = new InMemoryDataStore();

                    builder.Register(c => store)
                            .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                            .AsSelf()
                            .SingleInstance();

                    builder.Register(c => new CachingBotDataStore(store, 
                            CachingBotDataStoreConsistencyPolicy
                            .ETagBasedConsistency))
                            .As<IBotDataStore<BotData>>()
                            .AsSelf()
                            .InstancePerLifetimeScope();

                    // Register your Web API controllers.
                    //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
                    //builder.RegisterWebApiFilterProvider(config);

                });

            GlobalConfiguration.Configure(WebApiConfig.Register);

            //config.DependencyResolver = new AutofacWebApiDependencyResolver(Conversation.Container);

            #region NotWorking
            //var store = new SqlBotDataStore(ConfigurationManager.ConnectionStrings["BotDataContextConnectionString"].ConnectionString);
            //Conversation.UpdateContainer(builder =>
            //{
            //    builder.Register(c => store)
            //        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
            //        .AsSelf()
            //        .SingleInstance();
            //});
            #endregion

            #region WORKING
            //var store = new InMemoryDataStore();

            //Conversation.UpdateContainer(builder =>
            //{
            //    builder.Register(c => new CachingBotDataStore(store,
            //             CachingBotDataStoreConsistencyPolicy
            //             .ETagBasedConsistency))
            //             .As<IBotDataStore<BotData>>()
            //             .AsSelf()
            //             .InstancePerLifetimeScope();
            //});
            #endregion
        }
    }
}
