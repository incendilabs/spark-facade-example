/*
 * Copyright (c) 2021, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System.Collections.Generic;

namespace Spark.Facade.Models
{
    public static class MunicipalityMap
    {
        private static readonly IDictionary<string, string> _municipalityMap = new Dictionary<string, string>
        {
            {"0301", "Oslo"},
            // Add more...
        };

        public static bool TryGetValue(string key, out string value)
        {
            return _municipalityMap.TryGetValue(key, out value);
        }

        public static string GetValue(string code)
        {
            _municipalityMap.TryGetValue(code, out var value);
            return value;
        }
    }
}
