using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
	public class ParkDAL
	{
		//
		private string connectionString;

		/// <summary>
		/// Constructor for Connection to Database
		/// </summary>
		/// <param name="dbConnectionString"></param>
		public ParkDAL(string dbConnectionString)
		{
			connectionString = dbConnectionString;
		}

		/// <summary>
		/// Builds a dictionary of parks Setting a numerical key to a park object value
		/// </summary>
		/// <returns></returns>
		public IDictionary<int,Park> GetAllParks()
		{
			Dictionary<int, Park> parks = new Dictionary<int, Park>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sql = $"SELECT * FROM park ORDER BY name ASC;";

					SqlCommand cmd = new SqlCommand(sql, conn);

					SqlDataReader reader = cmd.ExecuteReader();

					int parkDictionaryKey = 1;

					while (reader.Read())
					{

						Park park = new Park();
						park.Id = Convert.ToInt32(reader["park_id"]);
						park.Name = Convert.ToString(reader["name"]);
						park.Location = Convert.ToString(reader["location"]);
						park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
						park.Area = Convert.ToInt32(reader["area"]);
						park.Visitors = Convert.ToInt32(reader["visitors"]);
						park.Description = Convert.ToString(reader["description"]);

						parks[parkDictionaryKey++] = park;
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);

			}
			return parks;
		}


		// List of Parks

		//public IList<Park> GetAllParks()
		//{
		//	List<Park> parks = new List<Park>();

		//	try
		//	{
		//		using (SqlConnection conn = new SqlConnection(connectionString))
		//		{
		//			conn.Open();

		//			string sql = $"SELECT * FROM park ORDER BY name ASC;";

		//			SqlCommand cmd = new SqlCommand(sql, conn);

		//			SqlDataReader reader = cmd.ExecuteReader();

		//			while (reader.Read())
		//			{
		//				Park park = new Park();
		//				park.Id = Convert.ToInt32(reader["park_id"]);
		//				park.Name = Convert.ToString(reader["name"]);
		//				park.Location = Convert.ToString(reader["location"]);
		//				park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
		//				park.Area = Convert.ToInt32(reader["area"]);
		//				park.Visitors = Convert.ToInt32(reader["visitors"]);
		//				park.Description = Convert.ToString(reader["description"]);

		//				parks.Add(park);
		//			}
		//		}
		//	}
		//	catch (SqlException ex)
		//	{
		//		Console.WriteLine(ex.Message);
		//	}

		//	return parks;
		//}
	}
}
