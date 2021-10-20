using System.Collections.Generic;
using System.Linq;
using Spark.Engine.Core;
using Spark.Engine.FhirResponseFactory;
using Spark.Engine.Service;
using Spark.Engine.Service.FhirServiceExtensions;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Spark.Engine.Extensions;
using Task = System.Threading.Tasks.Task;

namespace Spark.Facade.Services
{
    public class PatientService : AsyncFhirService
    {
        public PatientService(
            IFhirServiceExtension[] extensions, 
            IFhirResponseFactory responseFactory, 
            ICompositeServiceListener serviceListener = null) 
            : base(extensions, responseFactory, serviceListener)
        {
        }

        public override Task<FhirResponse> SearchAsync(string type, SearchParams searchCommand, int pageIndex = 0)
        {
            var queryService = GetFeature<IQueryService>();
            var entries = queryService.GetAsync(type, searchCommand);

            return Task.FromResult(CreateBundleResponse(Bundle.BundleType.Searchset, entries.ToEnumerable()));
        }

        private FhirResponse CreateBundleResponse(Bundle.BundleType type, IEnumerable<Entry> entries)
        {
            var bundle = new Bundle
            {
                Type = type,
                Total = entries.Count(),
            };
            bundle.Append(entries);

            return _responseFactory.GetFhirResponse(bundle);
        }
    }
}
