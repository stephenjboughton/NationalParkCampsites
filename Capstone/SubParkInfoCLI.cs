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

			Console.WriteLine("Select a Command");
			Console.WriteLine("1) View Campgrounds");
			Console.WriteLine("2) Search for Reservation");
			Console.WriteLine("3) Return to Previous Screen");

			string input = Console.ReadLine();

			if (input == "1")
			{
				DisplayCampgrounds(park.Id);
			}
			else if (input == "2")
			{
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

		private void DisplayCampgrounds(int parkId)
		{
			while (true)
			{
				CampgroundDAL dal = new CampgroundDAL(DatabaseConnection);
				Console.Clear();
				IDictionary<int, Campground> campground = dal.GetAllCampgroundsPerPark(parkId);
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

				//DOES NOT BREAK OUT OF LOOP WITH ON COMMAND. REQUIRES USER TO ENTER TWO COMMANDS
				else if (campgroundChoice == "2")
				{
					//SubParkInfoCLI subPark = new SubParkInfoCLI();
					//subPark.DisplayParkInfo();
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
