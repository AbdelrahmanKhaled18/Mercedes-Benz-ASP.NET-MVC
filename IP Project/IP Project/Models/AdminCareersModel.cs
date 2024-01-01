namespace IP_Project.Models
{
    using System;
    using System.Collections.Generic;

    public class AdminCareersModel
    {
        public AdminCareersModel(IEnumerable<Career> careers, IEnumerable<Location> locations)
        {
            Careers = careers;
            Locations = locations;
        }

        public IEnumerable<Career> Careers { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}
