using System;
using System.Collections.Generic;
using System.Threading;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
	public class SiteSearchCLI
	{
		//Connects to Database
		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		/// <summary>
		/// Builds List of sites from input 
		/// </summary>
		/// <param name="parkId"></param>
		public bool PromptUserForDateRange(int parkId)
		{
			bool reservationMade = false;
			while (true)
			{
				//connects campgrounds database
				CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);
				Console.Clear();

				//Builds Dictionary to show campgrounds per park
				IDictionary<int, Campground> campground = dal.GetAllCampgroundsPerPark(parkId);
				Console.WriteLine(" ".PadRight(5) + "Name".PadRight(35) + "Open".PadRight(10) + "Close".PadRight(13) + "Daily Fee".PadRight(10));
				foreach (KeyValuePair<int, Campground> camps in campground)
				{
					Console.WriteLine("#" + camps.Key.ToString().PadRight(4) +
										camps.Value.Name.ToString().PadRight(35) +
										camps.Value.OpenFrom.ToString().PadRight(10) +
										camps.Value.OpenTo.ToString().PadRight(13) +
										camps.Value.DailyFee.ToString("c").ToString().PadRight(10));
				}
				Console.Write("Which campground (enter 0 to cancel)? ");
				string campgroundChoice = Console.ReadLine();
				int campSelection;

				bool campKey = int.TryParse(campgroundChoice, out campSelection);
				if (campKey == false)
				{
					Console.WriteLine("Please enter a valid selection.");
					Thread.Sleep(2000);
				}
				else if (campground.ContainsKey(campSelection))
				{
					//Holds input variables
					DateTime fromDate, toDate;
					// Calls method to choose dates
					ChooseACampground(out fromDate, out toDate);

					//Calls DAL to build a dictionary of sites available for specified date range
					SiteDAL siteDal = new SiteDAL(DatabaseConnection);
					try
					{
						IDictionary<int, Site> AvailableSites = siteDal.GetAvailableSites(campSelection, fromDate, toDate);
						//if there are no available sites ask them for an alternate date range
						if (AvailableSites.Count == 0)
						{
							Console.WriteLine("There are no available sites.");
							Console.Write("Would you like to select another date range? Y or N");
							string selection = Console.ReadLine();
							if (selection.ToLower() == "y")
							{
								ChooseACampground(out fromDate, out toDate);
							}

							//Quit or Return to main menu
							if (selection.ToLower() == "n")
							{
								break;
							}
							else
							{
								Console.Write("Please enter a valid selection");
							}
						}

						//Shows campground selection
						else
						{
							Console.WriteLine();
							Console.WriteLine("Results Matching Your Search Criteria");
							Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utility".PadRight(15) + "Cost".PadRight(15));
							foreach (KeyValuePair<int, Site> site in AvailableSites)
							{
								Console.WriteLine(site.Value.SiteNumber.ToString().PadRight(10) +
								site.Value.MaxOccupancy.ToString().PadRight(15) +
								(site.Value.Accessible ? "Yes" : "No").PadRight(15) +
								((site.Value.MaxRv == 0) ? "N/A" : site.Value.MaxRv.ToString()).PadRight(15) +
								(site.Value.Utilities ? "Yes" : "No").PadRight(15) +
								(TotalDays(fromDate, toDate) * site.Value.DailyFee).ToString().PadRight(15));
							}

							//To Reserve a campground
							Console.WriteLine("Which site should be reserved(enter 0 to cancel)? ");
							int siteToReserve = int.Parse(Console.ReadLine());
							if (siteToReserve == 0)
							{
								break;
							}
							if (AvailableSites.ContainsKey(siteToReserve))
							{
								Console.WriteLine("What name should the reservation be made under ?");
								string reservationName = Console.ReadLine();
								ReservationDAL reservationdDal = new ReservationDAL(DatabaseConnection);
								int reservationId = reservationdDal.MakeAReservation(siteToReserve, fromDate, toDate, reservationName);
								Console.WriteLine("The reservation has been made");
								Console.WriteLine($"The confirmation ID is : {reservationId}");
								Console.WriteLine($"Press Enter to Return to Park List");
								Console.ReadLine();
								return reservationMade = true;
							}
							else
							{
								Console.Write("Please enter a valid selection");
							}
						}
					}
					catch (Exception)
					{
						Console.WriteLine("The input is not valid");
						break;
					}
				}
				else if (campSelection == 0)
				{
					break;
				}
			}
			return reservationMade;
		}

		/// <summary>
		/// Returns total days of stay
		/// </summary>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns>total days as int</returns>
		private static int TotalDays(DateTime fromDate, DateTime toDate)
		{
			int totalDays = (toDate - fromDate).Days;
			return totalDays;
		}

		/// <summary>
		/// Takes in a date range 
		/// </summary>
		/// <param name="campgroundChoice">?????</param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		public void ChooseACampground(out DateTime fromDate, out DateTime toDate)
		{
			fromDate = DateTime.MinValue;
			toDate = DateTime.MinValue;
			bool isValid = false;
			while (!isValid)
			{
				Console.Write("What is the arrival date? ");
				string inputFromDate = Console.ReadLine();
				Console.Write("What is the departure date? ");
				string inputToDate = Console.ReadLine();

				if ((DateTime.TryParse(inputFromDate, out fromDate) && fromDate.Date > DateTime.Now)
					&& (DateTime.TryParse(inputToDate, out toDate) && toDate.Date > fromDate.Date))
				{
					isValid = true;
				}
				if (!isValid)
				{
					Console.WriteLine();
					Console.WriteLine("Date range not valid, please select different dates.");
				}
			}
		}
	}
}