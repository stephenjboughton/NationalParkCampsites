using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
	public class CampgroundListCLI
	{
		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		/// <summary>
		/// method that calls campground DAL to return a list of campgrounds at selected Park
		/// </summary>
		/// <param name="parkId"></param>
		public bool DisplayCampgrounds(int parkId)
		{
			bool reservationMade = false;
			while (true)
			{
				//instantiate a campground DAL and use its method for getting
				//a dictionary of all campgrounds at specified park
				CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);
				Console.Clear();
				IDictionary<int, Campground> campground = dal.GetAllCampgroundsPerPark(parkId);

				//iterate through dictionary of campgrounds and display properties of each
				Console.WriteLine(" ".PadRight(5) + "Name".PadRight(35) + "Open".PadRight(10) + "Close".PadRight(13) + "Daily Fee".PadRight(10));
				foreach (KeyValuePair<int, Campground> camps in campground)
				{
					Console.WriteLine("#"+camps.Key.ToString().PadRight(4) + 
										camps.Value.Name.ToString().PadRight(35) + 
										camps.Value.OpenFrom.ToString().PadRight(10) + 
										camps.Value.OpenTo.ToString().PadRight(13) + 
										camps.Value.DailyFee.ToString("c").ToString().PadRight(10));
				}
				Console.WriteLine();
				Console.WriteLine("Select A Command");
				Console.WriteLine("1) Search for Available Reservation");
				Console.WriteLine("2) Return to park list");

				string campgroundChoice = Console.ReadLine();
				Console.Clear();

				if (campgroundChoice == "1")
				{
					SiteSearchCLI siteSearch = new SiteSearchCLI();
					reservationMade = siteSearch.PromptUserForDateRange(parkId);
					break;
				}

				else if (campgroundChoice == "2")
				{
					break;
				}
				else
				{
					Console.WriteLine("Please enter a valid selection");
				}
			}
			return reservationMade;
		}
	}
}
