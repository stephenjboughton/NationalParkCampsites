using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
	class CampgroundDAL
	{
			//
		private string connectionString;

		/// <summary>
		/// Constructor for Connection to Database
		/// </summary>
		/// <param name="dbConnectionString"></param>
		public CampgroundDAL(string dbConnectionString)
		{
			connectionString = dbConnectionString;
		}

		/// <summary>
		/// Builds a dictionary of campgrounda Setting a numerical key to a campground object value
		/// </summary>
		/// <returns></returns>
		public IDictionary<int,Campground> GetAllCampgroundsPerPark(int parkId)
		{

			Dictionary<int, Campground> campgrounds = new Dictionary<int, Campground>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sql = $"SELECT * FROM campground WHERE campground.park_id = @parkid ORDER BY name ASC;";

					SqlCommand cmd = new SqlCommand(sql, conn);

					cmd.Parameters.AddWithValue("@parkid", parkId);

					SqlDataReader reader = cmd.ExecuteReader();

					int campDictionaryKey = 1;

					while (reader.Read())
					{
						Campground camp = new Campground();
						camp.Id = Convert.ToInt32(reader["campground_id"]);
						camp.ParkId = Convert.ToInt32(reader["park_id"]);
						camp.Name = Convert.ToString(reader["name"]);
						camp.OpenFrom = Convert.ToInt32(reader						["open_from_mm"]);
						camp.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
						camp.DailyFee = Convert.ToDecimal(reader					["daily_fee"]);
				
						campgrounds[campDictionaryKey++] = camp;
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);

			}
			return campgrounds;
		}


		
	}
}
