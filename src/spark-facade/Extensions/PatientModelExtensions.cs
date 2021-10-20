using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using Spark.Facade.Models;

namespace Spark.Facade.Extensions
{
    public static class PatientModelExtensions
    {
        public static Resource ToPatient(this PatientModel patientModel)
        {
            var resource = new Patient
            {
                Id = patientModel.Id.ToString("D"),
                Meta = new Meta
                {
                    Profile = new[] {Identificators.PROFILE_PATIENT},
                },
                BirthDate = string.IsNullOrWhiteSpace(patientModel.Birthdate) ? null : patientModel.Birthdate,
                Gender = string.IsNullOrWhiteSpace(patientModel.Gender) ? null : EnumUtility.ParseLiteral<AdministrativeGender>(patientModel.Gender),
            };
            if (!string.IsNullOrWhiteSpace(patientModel.Citizenship))
            {
                resource.Extension = new List<Extension>
                {
                    new Extension
                    {
                        Url = Identificators.EXTENSION_CITIZENSHIP,
                        Extension = new List<Extension>
                        {
                            new Extension
                            {
                                Url = "code",
                                Value = new CodeableConcept
                                {
                                    Coding = new List<Coding>
                                    {
                                        new Coding
                                        {
                                            System = Identificators.SYSTEM_COUNTRYCODES,
                                            Code = patientModel.Citizenship,
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(patientModel.Ssn))
            {
                resource.Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = Identificators.SYSTEM_SSN,
                        Value = patientModel.Ssn,
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(patientModel.Given) || !string.IsNullOrWhiteSpace(patientModel.Surname))
            {
                resource.Name = new List<HumanName>()
                {
                    new HumanName()
                    {
                        Given = patientModel.Given.Split(' '),
                        Family = patientModel.Surname,
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(patientModel.Phone))
            {
                resource.Telecom = new List<ContactPoint>
                {
                    new ContactPoint
                    {
                        System = ContactPoint.ContactPointSystem.Phone,
                        Use = ContactPoint.ContactPointUse.Home,
                        Value = patientModel.Phone,
                    }
                };
            }

            Address address = null;
            if (!string.IsNullOrWhiteSpace(patientModel.AddressLine)
                || !string.IsNullOrWhiteSpace(patientModel.Citizenship)
                || !string.IsNullOrWhiteSpace(patientModel.City)
                || !string.IsNullOrWhiteSpace(patientModel.District)
                || !string.IsNullOrWhiteSpace(patientModel.ZipCode)
                || !string.IsNullOrWhiteSpace(patientModel.Country))
            {
                address = new Address
                {
                    Use = Address.AddressUse.Home,
                    Line = string.IsNullOrWhiteSpace(patientModel.AddressLine) ? null : new[] {patientModel.AddressLine},
                    City = string.IsNullOrWhiteSpace(patientModel.City) ? null : patientModel.City,
                    District = string.IsNullOrWhiteSpace(patientModel.District) ? null : patientModel.District,
                    PostalCode = string.IsNullOrWhiteSpace(patientModel.ZipCode) ? null : patientModel.ZipCode,
                    Country = string.IsNullOrWhiteSpace(patientModel.Country) ? null : patientModel.Country,
                };
            }

            if (!string.IsNullOrWhiteSpace(patientModel.MunicipalityCode))
            {
                address.Extension = new List<Extension>
                {
                    new Extension
                    {
                        Url = Identificators.EXTENSION_PROPERTYINFORMATION,
                        Extension = new List<Extension>
                        {
                            new Extension
                            {
                                Url = "municipality",
                                Value = new Coding
                                {
                                    System = Identificators.SYSTEM_MUNICIPALITY,
                                    Code = patientModel.MunicipalityCode,
                                    Display = MunicipalityMap.GetValue(patientModel.MunicipalityCode),
                                }
                            }
                        }
                    }
                };
            }

            if (address != null)
            {
                resource.Address = new List<Address>
                {
                    address,
                };
            }

            if (!string.IsNullOrWhiteSpace(patientModel.Contact))
            {
                resource.Contact = new List<Patient.ContactComponent>
                {
                    new Patient.ContactComponent
                    {
                        Name = new HumanName
                        {
                            Text = patientModel.Contact
                        }
                    }
                };
            }

            return resource;
        }
    }
}
