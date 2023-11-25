/*
 * Copyright (c) 2021-2023, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Spark.Engine;
using Spark.Engine.Core;
using Spark.Engine.Service;

namespace Spark.Facade.Controllers
{
    [Route("fhir")]
    public class SystemController : ApiController
    {
        private readonly IAsyncFhirService _fhirService;
        private readonly SparkSettings _settings;

        public SystemController(IAsyncFhirService service, SparkSettings settings)
        {
            _fhirService = service;
            _settings = settings;
        }

        [HttpGet("metadata")]
        public async Task<ActionResult<FhirResponse>> Metadata()
        {
            return await _fhirService.CapabilityStatementAsync(_settings.Version);
        }
    }
}
