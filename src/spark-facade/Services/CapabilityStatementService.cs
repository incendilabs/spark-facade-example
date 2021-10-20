using Hl7.Fhir.Model;
using Spark.Engine.Core;
using Spark.Engine.Service.FhirServiceExtensions;
using Spark.Facade.Extensions;
using CapabilityStatementBuilder = Spark.Engine.Core.CapabilityStatementBuilder;

namespace Spark.Facade.Services
{
    /// <inheritdoc />
    public class CapabilityStatementService : ICapabilityStatementService
    {
        public CapabilityStatement GetSparkCapabilityStatement(string version)
        {
            return new CapabilityStatementBuilder()
                .CreateCapabilityStatementHeader()
                .WithAcceptFormat(new[] {"xml", "json"})
                .WithRest(() => new RestComponentBuilder()
                    .WithMode(CapabilityStatement.RestfulCapabilityMode.Server)
                    .WithDocumentation("Main FHIR endpoint for Spark Facade Example")
                    .WithResource(new ResourceComponentBuilder()
                        .WithType(ResourceType.Patient)
                        .WithProfile("http://hl7.no/fhir/StructureDefinition/no-basis-Patient")
                        .WithVersioning(CapabilityStatement.ResourceVersionPolicy.NoVersion)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.Create)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.Read)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.SearchType)
                        .WithSearchParam("identifier", SearchParamType.Token, documentation:"Supports search by norwegian FNR. Use OID 'urn:oid:2.16.578.1.12.4.1.4.1' for the system part.")
                        .Build())
                    .Build())
                .Build();
        }
    }
}
