using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
	public class ParkInfoCLI
	{

		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		public void DisplayParkInfo(Park park)
		{
			while (true)
			{
				Console.Clear();
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

				Console.WriteLine();
				Console.WriteLine("Select a Command");
				Console.WriteLine("1) View Campgrounds");
				Console.WriteLine("2) Search for Reservation");
				Console.WriteLine("3) Return to Previous Screen");

				string input = Console.ReadLine();

				if (input == "1")
				{
					//instantiate a campground list CLI and call campground display method
					//CampgroundListCLI campgroundList = new CampgroundListCLI();
					CampgroundListCLI campgroundList = new CampgroundListCLI();
					campgroundList.DisplayCampgrounds(park.Id);
				}
				else if (input == "2")
				{
					Console.Write("What is the arrival date? ");
					string fromDate = Console.ReadLine();
					Console.Write("What is the departure date? ");
					string toDate = Console.ReadLine();

					SiteDAL dal = new SiteDAL(DatabaseConnection);
					IDictionary<int, Site> AvailableSites = dal.GetSitesParkwide(park.Id, fromDate, toDate);

					Console.WriteLine();
					Console.WriteLine("Results Matching Your Search Criteria");
					Console.WriteLine("Campground".PadRight(20) + "Site No.".PadRight(10) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utility".PadRight(15) + "Cost".PadRight(15));
					foreach (KeyValuePair<int, Site> site in AvailableSites)
					{
						Console.WriteLine(
						site.Value.CampgroundName.ToString().PadRight(20) +
						site.Value.SiteNumber.ToString().PadRight(10) +
						site.Value.MaxOccupancy.ToString().PadRight(15) +
						site.Value.Accessible.ToString().PadRight(15) +
						site.Value.MaxRv.ToString().PadRight(15) +
						site.Value.Utilities.ToString().PadRight(15) +
						(TotalDays(fromDate, toDate) * site.Value.DailyFee).ToString().PadRight(15));
					}
					Console.WriteLine("Which site should be reserved(enter 0 to cancel)? ");
					string siteToReserve = Console.ReadLine();

				}
				else if (input == "3")
				{
					ParksCLI cli = new ParksCLI();
					cli.RunCLI();
				}
				else
				{
					Console.WriteLine("Please enter a valid selection");
				}

			}

		}

		private static int TotalDays(string fromDate, string toDate)
		{
			DateTime convertedFromDate = Convert.ToDateTime(fromDate);
			DateTime convertedToDate = Convert.ToDateTime(toDate);
			int totalDays = (convertedToDate - convertedFromDate).Days;
			return totalDays;
		}
	}
}
