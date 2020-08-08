using System;
using System.Linq;
using System.Collections.Generic;
using CoronaStore.Models;

namespace CoronaStore.Services
{
    public class LocationsService
    {
        public IList<Location> SearchLocations(string name, int? population)
        {
            using (var context = new CoronaPageContext())
            {
                IQueryable<Location> locations = context.Locations.Where(p => p.City.ToLower().Contains(name.ToLower()));

                if (population != null)
                {
                    locations = locations.Where(p => p.Population <= population);
                }

                return locations.ToList();
            }
        }


    }
}