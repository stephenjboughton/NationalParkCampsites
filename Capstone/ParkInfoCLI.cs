using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
	public class ParkInfoCLI
	{

		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		public bool DisplayParkInfo(Park park)
		{
			bool reservationMade = false;
			while (true)
			{
				Console.Clear();
				//Display park name
				Console.WriteLine(park.Name + " National Park");
				//Dispaly park Location
				Console.WriteLine();
				Console.WriteLine("Location: " + park.Location);
				//Display est date
				Console.WriteLine("Established: " + park.EstablishDate.ToShortDateString());
				//Display area
				Console.WriteLine("Area: " + park.Area);
				//Display annual visitors
				Console.WriteLine("Annual Visitors: " + park.Visitors);
				//Display description
				Console.WriteLine();
				Console.WriteLine(park.Description);

				Console.WriteLine();
				Console.WriteLine("Select a Command");
				Console.WriteLine("  1) View Campgrounds");
				Console.WriteLine("  2) Search for Reservation");
				Console.WriteLine("  3) Return to Previous Screen");

				string input = Console.ReadLine();

				if (input == "1")
				{
					//instantiate a campground list CLI and call campground display method
					CampgroundListCLI campgroundList = new CampgroundListCLI();
					return reservationMade = campgroundList.DisplayCampgrounds(park.Id);
					
				}
				else if (input == "2")
				{
					DateTime fromDate;
					DateTime toDate;

					SiteSearchCLI siteSearch = new SiteSearchCLI();
					siteSearch.ChooseACampground(out fromDate, out toDate);

					//Call site DAL and use GetSitesParkwide to give the user a dictionary of all 
					//campsites in the park to select from
					SiteDAL dal = new SiteDAL(DatabaseConnection);
					try
					{
						IDictionary<int, Site> AvailableSites = dal.GetSitesParkwide(park.Id, fromDate, toDate);

						Console.WriteLine();
						Console.WriteLine("Results Matching Your Search Criteria");
						Console.WriteLine("Campground".PadRight(35) + "Site No.".PadRight(10) + "Max Occup.".PadRight(15) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utility".PadRight(15) + "Cost".PadRight(15));
						foreach (KeyValuePair<int, Site> site in AvailableSites)
						{
							Console.WriteLine(
							site.Value.CampgroundName.ToString().PadRight(35) +
							site.Value.SiteNumber.ToString().PadRight(10) +
							site.Value.MaxOccupancy.ToString().PadRight(15) +
							(site.Value.Accessible ? "Yes" : "No").PadRight(15) +
							(site.Value.MaxRv == 0 ? "N/A" : site.Value.MaxRv.ToString()).PadRight(15) +
							(site.Value.Utilities ? "Yes" : "No").PadRight(15) + 
							(TotalDays(fromDate, toDate) * site.Value.DailyFee).ToString().PadRight(15));
						}
						Console.Write("Which site should be reserved(enter 0 to cancel)? ");
						int siteToReserve = int.Parse(Console.ReadLine());
						if (siteToReserve == 0)
						{
							Console.WriteLine("Returning to Park List");
							Thread.Sleep(2000);
							break;
						}
						else if(AvailableSites.ContainsKey(siteToReserve))
						{
							Console.Write("What name should the reservation be made under? ");
							string reservationName = Console.ReadLine();

							//Call reservation Database
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
							Thread.Sleep(2000);
						}
						
					}
					catch (Exception ex)
					{
						Console.WriteLine("Please enter a valid selection");
					}
				}
				else if (input == "3")
				{
					break;
				}
				else
				{
					Console.WriteLine("Please enter a valid selection.");
					Thread.Sleep(2000);
				}
			}
			return reservationMade;
		}
		private static int TotalDays(DateTime fromDate, DateTime toDate)
		{
			int totalDays = (toDate - fromDate).Days;
			return totalDays;
		}
	}
}
