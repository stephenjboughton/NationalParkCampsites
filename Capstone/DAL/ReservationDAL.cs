using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
	public class ReservationDAL
	{
		private string connectionString;

		/// <summary>
		/// Constructor for Connection to Database
		/// </summary>
		/// <param name="dbConnectionString"></param>
		public ReservationDAL(string dbConnectionString)
		{
			connectionString = dbConnectionString;
		}

		/// <summary>
		/// Builds a dictionary of campgrounda Setting a numerical key to a campground object value
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, Reservation> GetAllReservations(int parkId)
		{

			Dictionary<int, Reservation> reservations = new Dictionary<int, Reservation>();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sql = $"SELECT reservation.*, campground.park_id FROM reservation INNER JOIN site ON reservation.site_id = site.site_id INNER JOIN campground ON site.campground_id = campground.campground_id WHERE park_id = @parkid ORDER BY reservation.reservation_id; ";

					SqlCommand cmd = new SqlCommand(sql, conn);

					cmd.Parameters.AddWithValue("@parkid", parkId);

					SqlDataReader reader = cmd.ExecuteReader();

					int reservationDictionaryKey = 1;

					while (reader.Read())
					{
						Reservation reservation = new Reservation();
						reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
						reservation.SiteId = Convert.ToInt32(reader["site_id"]);
						reservation.Name = Convert.ToString(reader["name"]);
						reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
						reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
						reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

						reservations[reservationDictionaryKey++] = reservation;
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return reservations;
		}

		/// <summary>
		/// Creates a reservation, adds a reservation to the reservation table
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <param name="name"></param>
		/// <returns>reservation Id</returns>
		public int MakeAReservation(int siteToReserve, DateTime fromDate, DateTime toDate, string reservationName)
		{
			//instantiate dicitonary
			Dictionary<int, Reservation> reservations = new Dictionary<int, Reservation>();

			//instantiate new variables
			int reservationId;

			//add new reservation to table 
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					string sql = $"INSERT INTO reservation (site_id, name, from_date, to_date, create_date) VALUES(@siteId, @name, @fromDate, @toDate, @createDate) SELECT SCOPE_IDENTITY();";

					SqlCommand cmd = new SqlCommand(sql, conn);

					//New variables
					cmd.Parameters.AddWithValue("@siteid", siteToReserve);
					cmd.Parameters.AddWithValue("@name", reservationName);
					cmd.Parameters.AddWithValue("@fromDate", fromDate);
					cmd.Parameters.AddWithValue("@toDate", toDate);
					cmd.Parameters.AddWithValue("@createDate", DateTime.Now);

					//
					reservationId = Convert.ToInt32(cmd.ExecuteScalar());
					return reservationId;
				}
			}
			catch (SqlException)
			{
				throw;
			}
		}
	}
}
