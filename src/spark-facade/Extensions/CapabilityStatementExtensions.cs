/*
 * Copyright (c) 2021-2025, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Spark.Engine.Core;

namespace Spark.Facade.Extensions
{
    public static class CapabilityStatementExtensions
    {
        public static CapabilityStatementBuilder CreateCapabilityStatementHeader(this CapabilityStatementBuilder builder)
        {
            return builder
                .WithTitle("FHIR Facade Example")
                .WithName("Facade-Example")
                .WithVersion("1.0")
                .WithFhirVersion(FHIRVersion.N4_0_1)
                .WithStatus(PublicationStatus.Active)
                .WithDate(new DateTimeOffset(new DateTime(2021, 10, 06)))
                .WithPublisher("Incendi")
                .WithContact(new ContactDetail
                {
                    Name = "Incendi",
                    Telecom = new List<ContactPoint>
                    {
                        new ContactPoint
                        {
                            System = ContactPoint.ContactPointSystem.Email,
                            Value = "info@incendi.no",
                        }
                    },
                })
                .WithJurisdiction(new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "urn:iso:std:iso:3166",
                            Code = "NO",
                            Display = "Norway",
                        }
                    }
                })
                .WithPurpose("Show case for a FHIR Facade Endpoint")
                .WithCopyright("Copyright © 2021 Incendi")
                .WithKind(CapabilityStatementKind.Instance)
                .WithSoftware(name: "Spark", version: "1.5.10");
        }
    }
}
