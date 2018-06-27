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

		}
	}
}
