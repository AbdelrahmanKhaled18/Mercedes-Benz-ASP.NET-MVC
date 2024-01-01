using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP_Project.Models
{
    public class TestDriveFormModel
    {

        public TestDriveFormModel() { }

        public TestDriveFormModel(IEnumerable<Location> locations, IEnumerable<Car> cars)
        {
            Locations = locations;
            Cars = cars;
        }

        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<Car> Cars { get; set; }
    }
}