using System;
using System.Collections.Generic;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
	public class SiteSearchCLI
	{
		//Connects to Database
		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parkId"></param>
		public void PromptUserForDateRange(int parkId)
		{
			//connects campgrounds database
			CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);
			Console.Clear();

			//Builds Dictionary to show campgrounds per park
			IDictionary<int, Campground> campground = dal.GetAllCampgroundsPerPark(parkId);
			foreach (KeyValuePair<int, Campground> camps in campground)
			{
				Console.WriteLine("#" + camps.Key + " " + camps.Value.Name + " " + camps.Value.OpenFrom + " " + camps.Value.OpenTo + " " + camps.Value.DailyFee.ToString("c"));
			}

			//Holds input variables
			int campgroundChoice;
			string fromDate, toDate;

			ChooseACampground(out campgroundChoice, out fromDate, out toDate);

			//Calls DAL to build a dictionary of sites available for specified date range
			SiteDAL siteDal = new SiteDAL(DatabaseConnection);
			IDictionary<int, Site> AvailableSites = siteDal.GetAvailableSites(campgroundChoice, fromDate, toDate);

			//if there are no available sites ask them for an alternate date range
			if (AvailableSites.Count == 0)
			{
				Console.WriteLine("There are no available sites.");
				Console.Write("Would you like to select another date range? Y or N");
				string selection = Console.ReadLine();
				if (selection.ToLower() == "y")
				{
					ChooseACampground(out campgroundChoice, out fromDate, out toDate);
				}
			}

			//
			else
			{
				Console.WriteLine();
				Console.WriteLine("Results Matching Your Search Criteria");
				Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utility".PadRight(15) + "Cost".PadRight(15));
				foreach (KeyValuePair<int, Site> site in AvailableSites)
				{
					Console.WriteLine(site.Value.SiteNumber.ToString().PadRight(10) +
					site.Value.MaxOccupancy.ToString().PadRight(15) +
					site.Value.Accessible.ToString().PadRight(15) +
					site.Value.MaxRv.ToString().PadRight(15) +
					site.Value.Utilities.ToString().PadRight(15) +
					(TotalDays(fromDate, toDate) * site.Value.DailyFee).ToString().PadRight(15));
				}
				Console.WriteLine("Which site should be reserved(enter 0 to cancel)? ");
				string siteToReserve = Console.ReadLine();
			}
		}

		private static int TotalDays(string fromDate, string toDate)
		{
			DateTime convertedFromDate = Convert.ToDateTime(fromDate);
			DateTime convertedToDate = Convert.ToDateTime(toDate);
			int totalDays = (convertedToDate - convertedFromDate).Days;
			return totalDays;
		}


		/// <summary>
		/// Takes in campground number and date range 
		/// </summary>
		/// <param name="campgroundChoice"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		private static void ChooseACampground(out int campgroundChoice, out string fromDate, out string toDate)
		{

			Console.Write("Which campground (enter 0 to cancel)? ");
			campgroundChoice = int.Parse(Console.ReadLine());
			Console.Write("What is the arrival date? ");
			fromDate = Console.ReadLine();
			Console.Write("What is the departure date? ");
			toDate = Console.ReadLine();
		}
	}
}