using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
	public class SubParkInfoCLI
	{

		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		public void DisplayParkInfo(Park park)
		{
			//Display park name
			Console.WriteLine(park.Name + " National Park");
			//Dispaly park Location
			Console.WriteLine("Location: " + park.Location);
			//Display est date
			Console.WriteLine("Established: " + park.EstablishDate);
			//Display area
			Console.WriteLine("Area: " + park.Area);
			//Display annual visitors
			Console.WriteLine("Annual Visitors: " + park.Visitors);
			//Display description
			Console.WriteLine(park.Description);

			Console.WriteLine("Select a Command");
			Console.WriteLine("1) View Campgrounds");
			Console.WriteLine("2) Search for Reservation");
			Console.WriteLine("3) Return to Previous Sreen");

			string input = Console.ReadLine();

			CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);

			if (input == "1")
			{
				IDictionary <int, Campground> campground = dal.GetAllCampgroundsPerPark(park.Id);
				foreach (KeyValuePair<int, Campground> camps in campground)
				{
					Console.WriteLine("#" + camps.Key + " " + camps.Value.Name + " " + camps.Value.OpenFrom + " " + camps.Value.OpenTo + " " + camps.Value.DailyFee.ToString("c"));
				}
			}
			else if (input == "2")
			{
			}
			else if (input == "3")
			{
				return;
			}
			else
			{
			}



		}


	}
}
