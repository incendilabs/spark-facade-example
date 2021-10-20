/*
 * Copyright (c) 2021, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using System;

namespace Spark.Facade.Models
{
    public class PatientModel
    {
        public Guid Id { get; set; }
        public string Ssn { get; set; }
        public string Given { get; set; }
        public string Surname { get; set; }
        public string Birthdate { get; set; }
        public string Gender { get; set; }
        public string Citizenship { get; set; }
        public string Phone { get; set; }
        public string MunicipalityCode { get; set; }
        public string AddressLine { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
    }
}
