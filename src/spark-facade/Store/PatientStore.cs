/*
 * Copyright (c) 2021-2025, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Microsoft.Data.SqlClient;
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

            await using var connection = new SqlConnection(_settings.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateExistsCommandByPrimaryKeyFrom("Patient", "Id", entry.Key.ResourceId);
            var resourceExists = (int)await command.ExecuteScalarAsync() == 1;
            command = resourceExists
                ? connection.CreateUpdateCommandFrom(patientModel, "Id", entry.Key.ResourceId)
                : connection.CreateInsertCommandFrom(patientModel);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<Entry> GetAsync(IKey key)
        {
            await using var connection = new SqlConnection(_settings.ConnectionString);
            var command = connection.CreateSelectCommandByPrimaryKeyFrom("Patient", "Id", key.ResourceId, typeof(PatientModel));

            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            var patientModel = reader.TransformTo<PatientModel>().FirstOrDefault();
            if (patientModel == null) throw new SparkException(HttpStatusCode.NotFound, $"No 'Patient' resource with id {key.ResourceId} was found.");

            var resource = patientModel.ToPatient();

            return Entry.Create(key, resource);
        }

        public IList<Entry> Get(IEnumerable<IKey> localIdentifiers, IEnumerable<string> elements = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Entry>> GetAsync(IEnumerable<IKey> localIdentifiers, IEnumerable<string> elements = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Entry>> GetAsync(IEnumerable<IKey> localIdentifiers) =>  throw new NotImplementedException();

        public void Add(Entry entry) => throw new NotImplementedException();
        public Entry Get(IKey key) => throw new NotImplementedException();
        public IList<Entry> Get(IEnumerable<IKey> localIdentifiers) => throw new NotImplementedException();
    }
}
