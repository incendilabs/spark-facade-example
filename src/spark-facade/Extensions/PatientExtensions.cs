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
            var patientModel = new PatientModel();
            patientModel.Id = Guid.Parse(resource.Id);
            patientModel.Ssn = resource.Identifier.Where(identifier => identifier.System == Identificators.SYSTEM_SSN).FirstOrDefault()?.Value;
            var name = resource.Name.Where(name => (name.Use == HumanName.NameUse.Official || !name.Use.HasValue)).FirstOrDefault();
            patientModel.Given = string.Join(" ", name?.Given);
            patientModel.Surname =  name?.Family;
            patientModel.Birthdate = resource.BirthDate;
            patientModel.Gender = resource.Gender?.GetLiteral();
            patientModel.Citizenship = (resource.Extension
                ?.Where(e => e.Url == Identificators.EXTENSION_CITIZENSHIP).FirstOrDefault()
                ?.Extension.Where(e => e.Url == "code").FirstOrDefault()?.Value as CodeableConcept)
                ?.Coding.Where(c => c.System == Identificators.SYSTEM_COUNTRYCODES).FirstOrDefault()
                ?.Code;
            patientModel.Phone = resource.Telecom?.Where(t => t.System == ContactPoint.ContactPointSystem.Phone).FirstOrDefault()?.Value;
            var homeAddress = resource.Address?.Where(a => a.Use == Address.AddressUse.Home).FirstOrDefault();
            patientModel.MunicipalityCode = (homeAddress
                ?.Extension?.Where(e => e.Url == Identificators.EXTENSION_PROPERTYINFORMATION).FirstOrDefault()
                ?.Extension.Where(e => e.Url == "municipality").FirstOrDefault()
                ?.Value as Coding)
                ?.Code;
            patientModel.AddressLine = homeAddress.Line.FirstOrDefault();
            patientModel.ZipCode = homeAddress.PostalCode;
            patientModel.City = homeAddress.City;
            patientModel.District = homeAddress.District;
            patientModel.Country = homeAddress.Country;

            return patientModel;
        }
    }
}
