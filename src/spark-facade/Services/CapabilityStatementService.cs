/*
 * Copyright (c) 2021-2025, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using Hl7.Fhir.Model;
using Spark.Engine.Core;
using Spark.Engine.Service.FhirServiceExtensions;
using Spark.Facade.Extensions;

namespace Spark.Facade.Services
{
    /// <inheritdoc />
    public class CapabilityStatementService : ICapabilityStatementService
    {
        public CapabilityStatement GetSparkCapabilityStatement()
        {
            return new CapabilityStatementBuilder()
                .CreateCapabilityStatementHeader()
                .WithAcceptFormat(["xml", "json"])
                .WithRest(restBuilder => restBuilder
                    .WithMode(CapabilityStatement.RestfulCapabilityMode.Server)
                    .WithDocumentation("Main FHIR endpoint for Spark Facade Example")
                    .WithResource(resourceBuilder => resourceBuilder
                        .WithType("Patient")
                        .WithProfile("http://hl7.no/fhir/StructureDefinition/no-basis-Patient")
                        .WithVersioning(CapabilityStatement.ResourceVersionPolicy.NoVersion)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.Create)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.Update)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.Read)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.Patch)
                        .WithInteraction(CapabilityStatement.TypeRestfulInteraction.SearchType)
                        .WithSearchParam(
                            "identifier",
                            SearchParamType.Token,
                            documentation:
                            "Supports search by norwegian FNR. Use OID 'urn:oid:2.16.578.1.12.4.1.4.1' for the system part."
                        )
                    )
                )
                .Build();
        }
    }
}
