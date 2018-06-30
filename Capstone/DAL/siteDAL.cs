using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
	public class SiteDAL
	{

		private string connectionString;

		/// <summary>
		/// Constructor for Connection to Database
		/// </summary>
		/// <param name="dbConnectionString"></param>
		public SiteDAL(string dbConnectionString)
		{
			connectionString = dbConnectionString;
		}

		/// <summary>
		/// Builds a dictionary of sites using an incrementint int for a key and a site object as the value; 
		/// Relies on SQL query that inner joins the reservation table in order to search for 
		/// determine which sites do not overlap on date range with an existing reservation for 
		/// the range requested by the user and also an inner join on the campground in order to
		/// set a daily fee property for the site which can be used to calculate the total cost for 
		/// the site during the users' requested stay.
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, Site> GetAvailableSites(int campgroundId, DateTime fromDate, DateTime toDate)
		{
			Dictionary<int, Site> sites = new Dictionary<int, Site>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					////THESE DID NOT WORK

					//string sql = $"SELECT TOP 5 site.* , campground.daily_fee FROM site INNER JOIN reservation ON reservation.site_id = site.site_id INNER JOIN campground ON site.campground_id = campground.campground_id  WHERE site.campground_id = @campgroundID AND @fromDate NOT BETWEEN reservation.from_date AND reservation.to_date AND @toDate NOT BETWEEN reservation.from_date AND reservation.to_date;";

					//string sql = $"SELECT TOP 5 site.* , campground.daily_fee " +
					//	$"FROM site " +
					//	$"INNER JOIN reservation ON reservation.site_id = site.site_id " +
					//	$"INNER JOIN campground ON site.campground_id = campground.campground_id  " +
					//	$"WHERE site.campground_id = @campgroundID AND site.site_id NOT IN " +
					//		$"(SELECT site_id FROM reservation WHERE reservation.from_date BETWEEN @fromDate AND @toDate)" +
					//		$" AND site.site_id NOT IN" +
					//		$"(SELECT site_id FROM reservation WHERE reservation.to_date BETWEEN @fromDate AND @toDate);";

					//string sql = $"SELECT TOP 5 site.* , campground.daily_fee " +
					//	$"FROM site " +
					//	$"INNER JOIN reservation ON reservation.site_id = site.site_id " +
					//	$"INNER JOIN campground ON site.campground_id = campground.campground_id  " +
					//	$"WHERE site.campground_id = @campgroundID AND site.site_id NOT IN " +
					//		$"(SELECT site_id FROM reservation WHERE reservation.from_date BETWEEN @fromDate AND @toDate)" +
					//		$" AND site.site_id NOT IN" +
					//		$"(SELECT site_id FROM reservation WHERE reservation.to_date BETWEEN @fromDate AND @toDate);";

					//string sql = $"SELECT site.*, campground.daily_fee FROM site INNER JOIN campground ON site.campground_id = campground.campground_id WHERE site.campground_id = 2 AND site.site_id NOT IN(SELECT site.site_id FROM reservation INNER JOIN site ON site.site_id = reservation.site_id INNER JOIN campground ON site.campground_id = campground.campground_id WHERE campground.campground_id = 2 AND((reservation.from_date BETWEEN '2018/06/30' AND '2018/07/03') OR (reservation.to_date BETWEEN '2018/06/30' AND '2018/07/03')));";

					//////////////////////

					//SQL Query to return "top 5" avaiable sites per campground
					string sql = $"SELECT TOP 5 site.*, campground.daily_fee FROM site INNER JOIN campground ON site.campground_id = campground.campground_id WHERE site.campground_id = @campgroundId AND site.site_id NOT IN (SELECT site_id FROM reservation WHERE(reservation.to_date BETWEEN @fromDate AND @toDate) OR(reservation.from_date BETWEEN @fromDate AND @toDate) OR " +
						$"(reservation.from_date < @fromDate AND reservation.to_date > @toDate));";

					SqlCommand cmd = new SqlCommand(sql, conn);

					//
					cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
					cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
					cmd.Parameters.AddWithValue("@toDate", toDate.Date);

					SqlDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						Site site = new Site();
						site.Id = Convert.ToInt32(reader["site_id"]);
						site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
						site.SiteNumber = Convert.ToInt32(reader["site_number"]);
						site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
						site.Accessible = Convert.ToBoolean(reader["accessible"]);
						site.MaxRv = Convert.ToInt32(reader["max_rv_length"]);
						site.Utilities = Convert.ToBoolean(reader["utilities"]);
						site.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

						//Return Dictionary
						sites[site.SiteNumber] = site;
						
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);

			}
			return sites;
		}

		/// <summary>
		/// Builds a dictionary to show all available camp sites for all campgrounds throughout a park.
		/// </summary>
		/// <param name="parkId"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		public IDictionary<int, Site> GetSitesParkwide(int parkId, DateTime fromDate, DateTime toDate)
		{
			Dictionary<int, Site> sites = new Dictionary<int, Site>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sql = $"SELECT TOP 5 site.*, campground.daily_fee, campground.name FROM site INNER JOIN campground ON site.campground_id = campground.campground_id WHERE campground.park_id = @parkId AND site.site_id NOT IN (SELECT site_id FROM reservation WHERE(reservation.to_date BETWEEN @fromDate AND @toDate) OR(reservation.from_date BETWEEN @fromDate AND @toDate) OR " +
						$"(reservation.from_date < @fromDate AND reservation.to_date > @toDate));";

					SqlCommand cmd = new SqlCommand(sql, conn);

					cmd.Parameters.AddWithValue("@parkId", parkId);
					cmd.Parameters.AddWithValue("@fromDate", fromDate);
					cmd.Parameters.AddWithValue("@toDate", toDate);

					SqlDataReader reader = cmd.ExecuteReader();

					int siteDictionaryKey = 1;

					//Add Sites to dictionary
					while (reader.Read())
					{
						Site site = new Site();
						site.Id = Convert.ToInt32(reader["site_id"]);
						site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
						site.SiteNumber = Convert.ToInt32(reader["site_number"]);
						site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
						site.Accessible = Convert.ToBoolean(reader["accessible"]);
						site.MaxRv = Convert.ToInt32(reader["max_rv_length"]);
						site.Utilities = Convert.ToBoolean(reader["utilities"]);
						site.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
						site.CampgroundName = Convert.ToString(reader["name"]);

						//Return dictionary of all campgrounds within a park
						sites[siteDictionaryKey++] = site;
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);

			}
			return sites;
		}

	}
}
