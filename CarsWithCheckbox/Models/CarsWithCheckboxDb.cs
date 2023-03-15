using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarsWithCheckbox.Models
{
    public class CarsWithCheckboxDb
    {
        private string _conStr;

        public CarsWithCheckboxDb(string conStr)
        {
            _conStr = conStr;
        }

        public List<Car> GetCars()
        {
            using var connection = new SqlConnection(_conStr);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Cars";
            connection.Open();
            var reader = cmd.ExecuteReader();
            var results = new List<Car>();
            while (reader.Read())
            {
                var car = new Car
                {
                    Make = (string)reader["Make"],
                    Model = (string)reader["Model"],
                    Year = (int)reader["Year"],
                    Price = (decimal)reader["Price"],
                    HasLeatherSeats = (bool)reader["HasLeatherSeats"],
                    CarType = (CarType)reader["CarType"]
                };
                results.Add(car);
            }


            return results;
        }

        public int AddCar(Car car)
        {
            using var connection = new SqlConnection(_conStr);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Cars (Make, Model, Year, Price, CarType, HasLeatherSeats) " +
                                  "VALUES (@make, @model, @year, @price, @carType, @hasLeather) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@make", car.Make);
            command.Parameters.AddWithValue("@model", car.Model);
            command.Parameters.AddWithValue("@year", car.Year);
            command.Parameters.AddWithValue("@price", car.Price);
            command.Parameters.AddWithValue("@carType", car.CarType);
            command.Parameters.AddWithValue("@hasLeather", car.HasLeatherSeats);
            connection.Open();

            //Example of how to get the most recent id. Useful for the upcoming blog homework
            int id = (int)(decimal)command.ExecuteScalar();
            return id;
        }
    }
}