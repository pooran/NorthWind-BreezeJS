NorthWind-BreezeJS
==================

This is the demo code built for BDotNet https://www.facebook.com/groups/BDotNet/ event WebEd http://webed.azurewebs
ites.net/ or https://www.facebook.com/groups/BDotNet/permalink/785436024830188/

I am using NorthWind db backup from http://northwinddatabase.codeplex.com/releases/view/71634

Client side it is Breeze client with Metadata cached in Localstorage to avoid subsequent metadata calls as recommended in http://stackoverflow.com/questions/24205565/in-webapi-breeze-implementation-can-i-avoid-exposing-metadata-funtion/24309887#24309887

Below are the steps

Server Side
===========

Created a WebAPI project [VS 2013 Update 2] with CORS enabled in Web.Config

<system.webServer>
 <httpProtocol>
      <customHeaders>
        <add name="Access-Control-Allow-Origin" value="*" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>

Added Nuget packages for Entity Framework 6, Breeze Server - for Web API 2, Breeze Server ContextProvider for Entity Framework 6.

Created a blank APIController called NorthWindController

Added following lines

        readonly EFContextProvider<NorthWindEntities> _contextProvider
            = new EFContextProvider<NorthWindEntities>();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

Added  IQueryable<T> HttpGet functions for the Entities that have to be exposed

In WebApiConfig.cs, added JSON Serializer code

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            
Also added MetadataScriptWriter class to save metadata as file

public static class MetadataScriptWriter
    {
        public static void Write()
        {
            var metadata = new EFContextProvider<NorthWindEntities>().Metadata();
            var fileName = HostingEnvironment.MapPath("~/m.js");
            const string prefix = "test= JSON.stringify(";
            const string postfix = ");";
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(prefix + metadata + postfix);
            }
        }
    }
    
Called MetadataScriptWriter.Write(); in WebApiConfig class, Register method.

Commented out default API MapRoute.


Client side
===========

Client side I am loading metadata from server to localStorage if not already downloaded. 

         function loadjsfile(filename) {
             var fileref = document.createElement('script')
             fileref.setAttribute("type", "text/javascript")
             fileref.setAttribute("src", filename)             
         }
         if (localStorage.getItem("m") == null)
         {
             loadjsfile("http://localhost/NorthWind/m.js");
             localStorage.setItem("m", test);
             test = localStorage.getItem("m")
         }
         else {
             test = localStorage.getItem("m");
         }
         
Calling createEntityManager which loads metadata from local variable instead of loading it from server everytime.

         var serviceName = 'http://localhost/NorthWind/breeze/NorthWind'; // your service root here
         var manager = new breeze.EntityManager();
         function createEntityManager() {

             // define the Breeze `DataService` for this app
             var dataService = new breeze.DataService({
                 serviceName: serviceName,
                 hasServerMetadata: false  // don't ask the server for metadata
             });

             // create the metadataStore
             var metadataStore = new breeze.MetadataStore({
             });

             // initialize it from the application's metadata variable
             metadataStore.importMetadata(test);


             // create a new EntityManager that uses this metadataStore
             return new breeze.EntityManager({
                 dataService: dataService,
                 metadataStore: metadataStore
             });
         }
         
Rest of the code is for UI, fetching the data from server and binding the UI with data.
