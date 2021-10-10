using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using Spark.Engine;
using Spark.Engine.Core;
using Spark.Engine.Store.Interfaces;
using Spark.Facade.Extensions;
using Spark.Facade.Models;
using Task = System.Threading.Tasks.Task;

namespace Spark.Facade.Store
{
    public class PatientStore : IFhirStore
    {
        private readonly StoreSettings _settings;

        public PatientStore(StoreSettings settings)
        {
            _settings = settings;
        }
        
        public async Task AddAsync(Entry entry)
        {
            var resource = entry.Resource as Patient;
            var patientModel = resource.ToPatientModel();
            
            using SqlConnection connection = new SqlConnection(_settings.ConnectionString);
            var command = connection.CreateInsertCommandFrom(patientModel);
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        
        public async Task<Entry> GetAsync(IKey key)
        {
            using SqlConnection connection = new SqlConnection(_settings.ConnectionString);
            var command = connection.CreateSelectCommandByPrimaryKeyFrom("Patient", "Id", key.ResourceId, typeof(PatientModel));

            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            
            var patientModel = reader.TransformTo<PatientModel>().FirstOrDefault();
            if (patientModel == null) throw new SparkException(HttpStatusCode.NotFound, $"No 'Patient' resource with id {key.ResourceId} was found.");

            var resource = patientModel.ToPatient();
            
            return Entry.Create(key, resource);
        }
        
        public Task<IList<Entry>> GetAsync(IEnumerable<IKey> localIdentifiers)
        {
            throw new System.NotImplementedException();
        }
        
        public void Add(Entry entry) => throw new System.NotImplementedException();

        public Entry Get(IKey key) => throw new System.NotImplementedException();

        public IList<Entry> Get(IEnumerable<IKey> localIdentifiers) => throw new System.NotImplementedException();
    }
}
