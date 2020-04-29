using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace DAL
{
    public class InMemoryData : IRestaurantData
    {
        List<Restaurant> restaurants;

        public InMemoryData()
        {
            restaurants = new List<Restaurant>()
            {
                new Restaurant {Id =1, Name = "La Costa", Location="California"},
                new Restaurant {Id =1, Name = "Pizza bros", Location="Newyork"}
            };
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return (from r in restaurants
                   orderby r.Name
                   select r
                   ).ToList();
        }
    }
}
