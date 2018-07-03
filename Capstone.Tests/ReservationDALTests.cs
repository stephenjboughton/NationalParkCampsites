using System;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
	[TestClass]
	public class ReservationDALTest : CampgroundDBTests
	{
		[TestMethod]
		public void GetAllReservations_Test()
		{
			//Arrange
			ReservationDAL reservationDAL = new ReservationDAL(ConnectionString);

			//Act
			var reservations = reservationDAL.GetAllReservations(1);
			//Assert
			Assert.AreEqual(6, reservations.Count);
		}

		[TestMethod]
		public void MakeAReservation_Test()
		{
			//Arrange
			ReservationDAL reservationDAL = new ReservationDAL(ConnectionString);
			int initialRowCount = GetRowCount();
			Reservation reservation = new Reservation();

			reservation.SiteId = 1;
			reservation.Name = "Test";
			reservation.FromDate = Convert.ToDateTime("2018/08/01");
			reservation.ToDate = Convert.ToDateTime("2018/08/02");
			reservation.CreateDate = DateTime.Today.Date;

			//Act
			reservationDAL.MakeAReservation(reservation.SiteId, reservation.ToDate, reservation.FromDate, reservation.Name);

			//Assert
			int finalRowCount = GetRowCount();
			Assert.AreEqual(initialRowCount + 1, finalRowCount);
		}

		[TestMethod]
		public void GetAllReservationsNextThirty_Test()
		{
			//Arrange
			ReservationDAL reservationDAL = new ReservationDAL(ConnectionString);

			//Act
			var reservations = reservationDAL.GetAllReservationsNextThirty(1);
			//Assert
			Assert.AreEqual(1, reservations.Count);
		}

		private int GetRowCount()
		{
			using (SqlConnection conn = new SqlConnection(ConnectionString))
			{
				conn.Open();

				SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM reservation" , conn);

				int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
				return rowCount;
			}
		}
	}

}
