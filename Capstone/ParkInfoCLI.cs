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
				Console.WriteLine("  1) View campgrounds");
				Console.WriteLine("  2) View all reservations for this park in the next 30 days");
				Console.WriteLine("  3) Search for reservation");
				Console.WriteLine("  4) Return to previous screen");

				string input = Console.ReadLine();

				if (input == "1")
				{
					//instantiate a campground list CLI and call campground display method
					CampgroundListCLI campgroundList = new CampgroundListCLI();
					reservationMade = campgroundList.DisplayCampgrounds(park.Id);
					if (reservationMade)
					{
						break;
					}

				}
				else if (input == "2")
				{
					ReservationDAL rDAL = new ReservationDAL(DatabaseConnection);
					IDictionary<int, Reservation> UpcomingReservations = rDAL.GetAllReservationsNextThirty(park.Id);

					if (UpcomingReservations.Count == 0)
					{
						Console.WriteLine("There are no reservations for this park within the next thirty days");
						Console.Write("To return to a list of the parks, press Q ");
						string userInput = Console.ReadLine().ToLower();
						if (userInput == "q")
						{
							break;
						}
					}

					else
					{
						Console.WriteLine();
						Console.WriteLine("Here are all the reservations in the next thirty days at " + park.Name
							+ " National Park");
						Console.WriteLine("Site ID".PadRight(10) + "Name".PadRight(30) + "From Date".PadRight(15) + "To Date".PadRight(15) + "Creation Date".PadRight(15));
						foreach (KeyValuePair<int, Reservation> reservation in UpcomingReservations)
						{
							Console.WriteLine(
							reservation.Value.SiteId.ToString().PadRight(10) +
							reservation.Value.Name.PadRight(30) +
							reservation.Value.FromDate.ToShortDateString().PadRight(15) +
							reservation.Value.ToDate.ToShortDateString().PadRight(15) +
							reservation.Value.CreateDate.ToShortDateString().PadRight(15));
						}

						Console.Write("To return to a list of the parks, press Q ");
						string userInput = Console.ReadLine().ToLower();
						if (userInput == "q")
						{
							break;
						}
					}
				}

				else if (input == "3")
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
						else if (AvailableSites.ContainsKey(siteToReserve))
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
				else if (input == "4")
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
