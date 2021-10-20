/*
 * Copyright (c) 2021, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System.Threading.Tasks;
using System.Web.Http;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Spark.Engine.Core;
using Spark.Engine.Extensions;
using Spark.Engine.Service;
using Spark.Facade.Services;

namespace Spark.Facade.Controllers
{
    [Route("fhir/[controller]")]
    public class PatientController : ApiController
    {
        private const string ResourceTypePatient = "Patient";

        private readonly IAsyncFhirService _fhirService;

        public PatientController(PatientService service)
        {
            _fhirService = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FhirResponse>> Read(string id)
        {
            var parameters = new ConditionalHeaderParameters(Request);
            var key = Key.Create(ResourceTypePatient, id);
            return await _fhirService.ReadAsync(key, parameters);
        }

        [HttpPost]
        public async Task<ActionResult<FhirResponse>> Create([FromBody] Patient patient)
        {
            var key = Key.Create(ResourceTypePatient, patient?.Id);

            return await _fhirService.CreateAsync(key, patient);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FhirResponse>> Update([FromBody] Resource resource, string id)
        {
            return await _fhirService.UpdateAsync(Key.Create(ResourceTypePatient, id), resource);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<FhirResponse>> Patch([FromBody] Parameters patch, string id)
        {
            var key = Key.Create(ResourceTypePatient, id);
            return await _fhirService.PatchAsync(key, patch);
        }

        [HttpGet]
        public async Task<ActionResult<FhirResponse>> Search()
        {
            var searchParams = Request.GetSearchParams();

            return await _fhirService.SearchAsync(ResourceTypePatient, searchParams);
        }

        [HttpPost("_search")]
        public async Task<ActionResult<FhirResponse>> SearchByPost()
        {
            var searchParams = Request.GetSearchParamsFromBody();

            return await _fhirService.SearchAsync(ResourceTypePatient, searchParams);
        }
    }
}
