using System;
using System.Collections.Generic;
using System.Linq;
using SampleApi.Api.Models;
using SampleApi.Options;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace SampleApi.Api.Controllers
{
    [Route("api/[controller]")]
    public class ExampleController : Controller
    {
        public ExampleController(ILogger<ExampleController> logger, IOptions<AppSettings> appSettings)
        {
            Logger = logger;

            // This is an example of how you inject configuration options that are read from config files and registered in Startup.cs.
            AppSettings = appSettings.Value;
        }

        public ILogger<ExampleController> Logger { get; private set; }
        public AppSettings AppSettings { get; private set; }

        // Normally this data would come from an injected business or repository class
        private List<Example> _examples = new List<Example>()
        {
            new Example() { Id = 1, Name = "Peter Parker" },
            new Example() { Id = 2, Name = "Clark Kent" },
            new Example() { Id = 3, Name = "Bruce Wayne" }
        };

        // GET /api/example
        [HttpGet]
        public IActionResult GetAll()
        {
            // this is how you log an information message
            Logger.LogInformation("Consumer requested all examples.");

            // this will return a HTTP Status Code 200 (OK) along with the data
            return Ok(_examples);
        }

        // GET /api/example/2
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var example = _examples.Where(e => e.Id == id).FirstOrDefault();

            if ( example == null )
            {
                // This is how you log a warning message
                Logger.LogWarning("Consumer requested example {0} that does not exist.", id);

                // this will return a HTTP Status Code 404 (Not Found) along with the message
                return HttpNotFound($"No example found with id = {id}");
            }

            // this will return a HTTP Status Code 200 (OK) along with the data
            return Ok(example);
        }

        [HttpPost]
        public IActionResult Create(Example example)
        {
            // try posting an example object with no name to get a non-valid model
            
            if ( !ModelState.IsValid )
            {
                // This is how you log a warning message
                Logger.LogWarning("Consumer tried to add an invalid example.");

                // this will return a HTTP Status Code 400 (Bad Request) along with the failed validations
                return HttpBadRequest(ModelState);
            }

            Logger.LogInformation("Consumer added an example with Name = {0}", example.Name);

            // if the save succeeds, return a HTTP Status Code 201 (Created) along with the route where the consumer can request the new record
            return CreatedAtAction("Get", new { id = example.Id });
        }
    }
}
