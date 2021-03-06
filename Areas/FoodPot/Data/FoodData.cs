﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.FoodPot.Models.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatePot.Areas.FoodPot.Data
{
    public class FoodData
    {
        public List<RestaurantDetails> GetRestaurantDetails(string cs, int RestaurantID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetRestaurantDetails"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                IList<RestaurantDetails> wsl = new List<RestaurantDetails>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new RestaurantDetails();
                    ws.RestaurantID = reader.GetInt32("RestaurantID");
                    ws.RestaurantName = reader.GetString("RestaurantName");
                    wsl.Add(ws);
                }
                reader.Close();
                return (List<RestaurantDetails>)wsl;

            }
        }
        public List<FoodTypes> GetFoodTypeList(string cs)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetFoodTypeList";
            using var cmd = new MySqlCommand(sql, con);
            IList<FoodTypes> wsl = new List<FoodTypes>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new FoodTypes();
                ws.FoodTypeID = reader.GetInt32("FoodTypeID");
                ws.FoodTypeText = reader.GetString("FoodTypeText");
                wsl.Add(ws);
            }
            reader.Close();
            con.Close();
            return (List<FoodTypes>)wsl;
        }
        public List<When> GetWhenList(string cs)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetWhenList";
            using var cmd = new MySqlCommand(sql, con);
            IList<When> wsl = new List<When>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new When();
                ws.WhenID = reader.GetInt32("WhenID");
                ws.WhenText = reader.GetString("WhenText");
                wsl.Add(ws);
            }
            reader.Close();
            con.Close();
            return (List<When>)wsl;
        }
        public void UpdateRestaurant(string cs, UpdateRestaurantDetails updatefiledetails)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spUpdateRestaurant"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantID", updatefiledetails.RestaurantID);
                cmd.Parameters.AddWithValue("@RestaurantName", updatefiledetails.RestaurantName);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public void ArchiveRestaurant(string cs, int RestaurantID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spArchiveRestaurant"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public List<RestaurantList> GetRestaurantList(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetRestaurantList";
            using var cmd = new MySqlCommand(sql, con);
            IList<RestaurantList> wsl = new List<RestaurantList>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new RestaurantList();
                ws.RestaurantID = reader.GetInt32("RestaurantID");
                ws.RestaurantName = reader.GetString("RestaurantName");
                ws.FoodTypeText = reader.GetString("FoodTypeText");
                ws.WhenText = reader.GetString("WhenText");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<RestaurantList>)wsl;
        }
        public List<UserList> GetUserList(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetUserList";
            using var cmd = new MySqlCommand(sql, con);
            IList<UserList> wsl = new List<UserList>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new UserList();
                ws.UserID = reader.GetInt32("UserID");
                ws.UserName = reader.GetString("UserName");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<UserList>)wsl;
        }

        public int AddRestaurant(string cs, string RestaurantName)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddRestaurant"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantName", RestaurantName);
                cmd.Parameters.Add("@RestaurantID", MySqlDbType.Int32);
                cmd.Parameters["@RestaurantID"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@RestaurantID"].Value;
                var lParam = (Int32)obj;    // more useful datatype
                con.Close();
                return lParam;
            }
        }
        public void AddFoodType(string cs, string FoodTypeText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddFoodType"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FoodTypeText", FoodTypeText);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddWhen(string cs, string WhenText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddWhen"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@WhenText", WhenText);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public bool FoodTypeDupeCheck(string cs, string FoodTypeText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spFoodTypeDupeCheck"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FoodTypeText", FoodTypeText);
                cmd.Parameters.Add("@FoodTypeExists", MySqlDbType.Bool);
                cmd.Parameters["@FoodTypeExists"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@FoodTypeExists"].Value;
                var lParam = (Boolean)obj;    // more useful datatype

                cmd.ExecuteNonQuery();
                con.Close();
                return lParam;
            }
        }
        public bool WhenDupeCheck(string cs, string WhenText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spWhenDupeCheck"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@WhenText", WhenText);
                cmd.Parameters.Add("@WhenExists", MySqlDbType.Bool);
                cmd.Parameters["@WhenExists"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@WhenExists"].Value;
                var lParam = (Boolean)obj;    // more useful datatype

                cmd.ExecuteNonQuery();
                con.Close();
                return lParam;
            }
        }
        public List<RandomRestaurant> GetRandomRestaurant(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spRandomRestaurant";
            using var cmd = new MySqlCommand(sql, con);
            IList<RandomRestaurant> wsl = new List<RandomRestaurant>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new RandomRestaurant();
                ws.RestaurantID = reader.GetInt32("RestaurantID");
                ws.RestaurantName = reader.GetString("RestaurantName");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<RandomRestaurant>)wsl;
        }
        public void RestaurantWatched(string cs, int RestaurantID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spRestaurantWatched"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddRestaurantFoodType(string cs, int RestaurantID, int FoodTypeID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddRestaurantFoodType"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                cmd.Parameters.AddWithValue("@FoodTypeID", FoodTypeID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddRestaurantWhen(string cs, int RestaurantID, int WhenID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddRestaurantWhen"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                cmd.Parameters.AddWithValue("@WhenID", WhenID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
