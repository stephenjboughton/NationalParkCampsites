using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;

namespace Capstone.Models
{
	class ParksCLI
	{
		//Database Connection
		const string DatabaseConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security=True";

		public void RunCLI()
		{
			Console.Clear();

			while (true)
			{
				//Calls method to show header
				ShowHeader();

				//Calls method to show park list - from dictionary
				BuildParkListForMenu();

				//Input
				string selection = Console.ReadLine();
				int parkSelection;

				ParkInfoCLI parkInfo = new ParkInfoCLI();
				ParkDAL dal = new ParkDAL(DatabaseConnection);
				IDictionary<int, Park> parks = dal.GetAllParks();

				// Try Parse park selection
				bool parkKey = int.TryParse(selection, out parkSelection);
				if (parkKey == false)
				{
					if (selection.ToLower() == "q")
					{
						break;
					}
					else
					{
						Console.WriteLine("Please enter a valid selection.");
					}
				}
				else
				{
					if (parks.ContainsKey(parkSelection))
					{
						//Call sub menu for park info
						parkInfo.DisplayParkInfo(parks[parkSelection]);

					}
					else
					{
						Console.WriteLine("Please enter a valid selection.");
					}
				}

			}
		}



		/// <summary>
		/// Show header for park selection menu
		/// </summary>
		private void ShowHeader()
		{
			//Ask user to select Park
			Console.WriteLine("Select a Park for Further Details");
		}

		/// <summary>
		/// Build Park Selection Menu
		/// </summary>
		private void BuildParkListForMenu()
		{
			//int parkNumber = 1;

			//Connect to Park DAL class
			ParkDAL dal = new ParkDAL(DatabaseConnection);
			IDictionary<int, Park> parks = dal.GetAllParks();

			foreach (KeyValuePair<int, Park> park in parks)
			{
				Console.WriteLine(park.Key + ") " + park.Value.Name);
				//parkNumber++;
			}

			Console.WriteLine($"Q) Quit");

		}


	}
}
