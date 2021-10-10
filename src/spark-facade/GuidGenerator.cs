using System;
using Hl7.Fhir.Model;
using Spark.Core;

namespace Spark.Facade
{
    public class GuidGenerator : IGenerator
    {
        public string NextResourceId(Resource resource)
        {
            return Guid.NewGuid().ToString("D");
        }

        public string NextVersionId(string resourceIdentifier)
        {
            return string.Empty;
        }

        public string NextVersionId(string resourceType, string resourceIdentifier)
        {
            return string.Empty;
        }
    }
}
