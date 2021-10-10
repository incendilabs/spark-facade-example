/* 
 * Copyright (c) 2016, Furore (info@furore.com) and contributors
 * Copyright (c) 2021, Incendi (info@incendi.no) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/spark/stu3/master/LICENSE
 */

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Spark.Engine;
using Spark.Engine.Core;
using Spark.Engine.Service.FhirServiceExtensions;
using Spark.Facade.Extensions;
using Spark.Facade.Models;

namespace Spark.Facade.Services
{
    public class QueryService : IQueryService
    {
        private readonly StoreSettings _settings;

        public QueryService(StoreSettings settings)
        {
            _settings = settings;
        }
        
        public async IAsyncEnumerable<Entry> GetAsync(string type, SearchParams searchParams)
        {
            var param = searchParams.Parameters.Where(p => p.Item1 == "identifier").FirstOrDefault();
            if (param == null)
                yield break;

            var criteriaValue = param.Item2.Split('|')[1];
            using var connection = new SqlConnection(_settings.ConnectionString);
            var command = connection.CreateSelectCommandWithCriteriaFrom("Patient", new Dictionary<string, object> {{"Ssn", criteriaValue}}, typeof(PatientModel));
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            var patientModels = reader.TransformTo<PatientModel>();
            foreach (var patientModel in patientModels)
            {
                var resource = patientModel.ToPatient();
                var key = Key.Create(resource.TypeName, resource.Id);
                yield return Entry.Create(key, resource);
            }
        }
    }
}
