using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient.Services
{
    // Service class for managing locations information.
    public class LocationService
    {
        // Class representing location info.
        public class LocationInfo
        {
            public string CityName { get; set; }
            public decimal? CityLongitude { get; set; }
            public decimal? CityLatitude { get; set; }
            public string StateName { get; set; }
            public string CountryName { get; set; }
            public bool? CanOpen { get; set; }
        }

        // Method to look up country info.
        public static IEnumerable<LocationInfo> LookUpLocation(string selectedZipCode)
        {
            // List to store state objects.
            var locationTypeLists = new List<LocationInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpLocation]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@PostalCode", selectedZipCode));

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var locationTypeList = new LocationInfo
                            {
                                CityName = reader["City Name"] as string,
                                CityLongitude = reader["City Longitude"] as decimal?,
                                CityLatitude = reader["City Latitude"] as decimal?,
                                StateName = reader["State Name"] as string,
                                CountryName = reader["Country Name"] as string,
                                CanOpen = reader["Can Open"] as bool?
                            };
                            locationTypeLists.Add(locationTypeList);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return locationTypeLists;
        }

        // Class representing state info.
        public class StateInfo
        {
            public string StateAbbr { get; set; }
            public string StateName { get; set; }
        }


        // Method to look up state info.
        public static IEnumerable<StateInfo> LookUpState(string selectedCountry)
        {
            // List to store state objects.
            var stateTypeLists = new List<StateInfo>();

            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[MainClient_LookUpState]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar) { Value = selectedCountry });

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var stateTypeList = new StateInfo
                            {
                                StateAbbr = reader["State Abbr"] as string,
                                StateName = reader["State Name"] as string,
                            };
                            stateTypeLists.Add(stateTypeList);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return stateTypeLists;
        }
    }
}
