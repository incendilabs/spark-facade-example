using System.Threading.Tasks;
using System.Web.Http;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Spark.Engine.Core;
using Spark.Engine.Extensions;
using Spark.Engine.Service;
using Spark.Facade.Services;

namespace Spark.Facade.Controllers
{
    [Route("fhir/[controller]")]
    public class PatientController : ApiController
    {
        private const string ResourceTypePatient = "Patient";
        
        private readonly IAsyncFhirService _fhirService;

        public PatientController(PatientService service)
        {
            _fhirService = service;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FhirResponse>> Read(string id)
        {
            ConditionalHeaderParameters parameters = new ConditionalHeaderParameters(Request);
            Key key = Key.Create(ResourceTypePatient, id);
            var response = await _fhirService.ReadAsync(key, parameters).ConfigureAwait(false);
            return new ActionResult<FhirResponse>(response);
        }
        
        [HttpPost]
        public async Task<ActionResult<FhirResponse>> Create([FromBody] Patient resource)
        {
            Key key = Key.Create(ResourceTypePatient, resource?.Id);

            return await _fhirService.CreateAsync(key, resource);
        }

        [HttpGet]
        public async Task<FhirResponse> Search()
        {
            var searchparams = Request.GetSearchParams();

            return await _fhirService.SearchAsync(ResourceTypePatient, searchparams);
        }
    }
}
