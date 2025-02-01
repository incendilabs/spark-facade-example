/*
 * Copyright (c) 2021-2025, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using Spark.Facade.Models;

namespace Spark.Facade.Extensions
{
    public static class PatientExtensions
    {
        public static PatientModel ToPatientModel(this Patient resource)
        {
            var name = resource.Name.FirstOrDefault(name => (name.Use == HumanName.NameUse.Official || !name.Use.HasValue));
            var homeAddress = resource.Address?.FirstOrDefault(a => a.Use == Address.AddressUse.Home);

            return new PatientModel
            {
                Id = Guid.Parse(resource.Id),
                Ssn = resource.Identifier.FirstOrDefault(identifier => identifier.System == Identificators.SYSTEM_SSN)
                    ?.Value,
                Given = string.Join(" ", name?.Given),
                Surname = name?.Family,
                Birthdate = resource.BirthDate,
                Gender = resource.Gender?.GetLiteral(),
                Citizenship = ((resource
                            ?.Extension?.FirstOrDefault(e => e.Url == Identificators.EXTENSION_CITIZENSHIP)
                            ?.Extension?.FirstOrDefault(e => e.Url == "code")
                            ?.Value as CodeableConcept)
                        ?.Coding)?.FirstOrDefault(c => c.System == Identificators.SYSTEM_COUNTRYCODES)
                    ?.Code,
                Phone = resource.Telecom?.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Phone)?.Value,
                MunicipalityCode = (homeAddress
                        ?.Extension?.FirstOrDefault(e => e.Url == Identificators.EXTENSION_PROPERTYINFORMATION)
                        ?.Extension?.FirstOrDefault(e => e.Url == "municipality")
                        ?.Value as Coding)
                    ?.Code,
                AddressLine = homeAddress.Line.FirstOrDefault(),
                ZipCode = homeAddress.PostalCode,
                City = homeAddress.City,
                District = homeAddress.District,
                Country = homeAddress.Country,
                Contact = resource.Contact?.FirstOrDefault()?.Name?.Text
            };
        }
    }
}
