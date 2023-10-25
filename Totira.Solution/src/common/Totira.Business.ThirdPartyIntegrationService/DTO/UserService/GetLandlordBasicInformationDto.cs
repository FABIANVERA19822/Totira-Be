﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
    public class GetLandlordBasicInformationDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? SocialInsuranceNumber { get; set; }
        public string AboutMe { get; set; } = default!;
        public LandlordBirthday Birthday { get; set; } = default!;
    }
    public class LandlordBirthday
    {
        public LandlordBirthday(int year, int day, int month)
        {
            this.Year = year;
            this.Day = day;
            this.Month = month;
        }

        public int Year { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
    }
}