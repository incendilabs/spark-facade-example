/*
 * Copyright (c) 2021-2025, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System;
using Hl7.Fhir.Model;
using Spark.Core;

namespace Spark.Facade
{
    public class GuidGenerator : IIdentityGenerator
    {
        public string NextResourceId(Resource resource)
        {
            return Guid.NewGuid().ToString("D");
        }

        public string NextVersionId(string resourceIdentifier)
        {
            return string.Empty;
        }

        public string NextVersionId(string resourceType, string resourceIdentifier)
        {
            return string.Empty;
        }
    }
}
