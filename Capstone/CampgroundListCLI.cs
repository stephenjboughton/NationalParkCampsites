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
		public void DisplayCampgrounds(int parkId)
		{
			while (true)
			{
				//instantiate a campground DAL and use its method for getting
				//a dictionary of all campgrounds at specified park
				CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);
				Console.Clear();
				IDictionary<int, Campground> campground = dal.GetAllCampgroundsPerPark(parkId);

				//iterate through dictionary of campgrounds and display properties of each
				foreach (KeyValuePair<int, Campground> camps in campground)
				{
					Console.WriteLine("#" + camps.Key + " " + camps.Value.Name + " " + camps.Value.OpenFrom + " " + camps.Value.OpenTo + " " + camps.Value.DailyFee.ToString("c"));
				}
				Console.WriteLine();
				Console.WriteLine("Select A Command");
				Console.WriteLine("1) Search for Available Reservation");
				Console.WriteLine("2) Return to Previous Screen");

				string campgroundChoice = Console.ReadLine();
				Console.Clear();

				if (campgroundChoice == "1")
				{
					SiteSearchCLI siteSearch = new SiteSearchCLI();
					siteSearch.PromptUserForDateRange(parkId);
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
		}
	}
}
