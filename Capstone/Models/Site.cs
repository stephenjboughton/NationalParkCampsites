using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
	public class Site
	{
		public int Id {get; set;}
		public int CampgroundId { get; set; }
		public int SiteNumber { get; set; }
		public int MaxOccupancy { get; set; }
		public bool Accessible { get; set; }
		public int MaxRv { get; set; }
		public bool Utilities { get; set; }
		public decimal DailyFee { get; set; }
		public string CampgroundName { get; set; }
	}
}
