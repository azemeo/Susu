﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Zarodoga
{
    static class Adatbazis
    {
        private static string homeServerWithout =
                "SERVER=localhost;" +
                "DATABASE=;" +
                "UID=root;" +
                "PASSWORD=;" +
                "CHARSET=utf8;";
        private static string homeServerWithin =
                "SERVER=localhost;" +
                "DATABASE=Players;" +
                "UID=root;" +
                "PASSWORD=;" +
                "CHARSET=utf8;";
        private static MySqlConnection kapcsolodasempty = new MySqlConnection(homeServerWithout);
        private static MySqlConnection kapcsolodas = new MySqlConnection(homeServerWithin);
        private static MySqlCommand command;


        // Adatbázis létrehozása (Players / Player / Loot)
        public static void Adatbazisletrehozas()
        {

            try
            {
                kapcsolodasempty.Open();
                command = kapcsolodasempty.CreateCommand();
                string create = "CREATE DATABASE IF NOT EXISTS Players";
                string tables = "CREATE TABLE Players.Player (id INT NOT NULL AUTO_INCREMENT , username VARCHAR(128) NOT NULL , password VARCHAR(128) NOT NULL , PRIMARY KEY (id)) ENGINE = MyISAM; ";
                string tables_second = "CREATE TABLE players.loot (player_id INT NOT NULL AUTO_INCREMENT, arany INT NOT NULL, tapasztalati_pont INT NOT NULL, jogosultsag INT NOT NULL, palya INT NOT NULL, PRIMARY KEY(player_id)) ENGINE = MyISAM; ";
                command = new MySqlCommand(create, kapcsolodasempty);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
                command = new MySqlCommand(tables, kapcsolodasempty);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
                command = new MySqlCommand(tables_second, kapcsolodasempty);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
                kapcsolodasempty.Close();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        // Insert into player (Username , Password)
        public static void InsertInto_Player(string user, string pass)
        {

            //Fő tábla kitöltése
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "INSERT INTO player(username,password)" +
                "VALUES(@param1,@param2)";
                MySqlCommand cmd = new MySqlCommand(sql, kapcsolodas);
                cmd.Parameters.Add("@param1", MySqlDbType.Text).Value = user;
                cmd.Parameters.Add("@param2", MySqlDbType.Text).Value = pass;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                kapcsolodas.Close();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        // Insert into loot (Username)
        public static void InsertInto_Loot(string user)
        {
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "INSERT INTO loot(arany,tapasztalati_pont,palya,jogosultsag)" +
                "VALUES(@param1,@param2,@param3,@param4)";
                MySqlCommand cmd = new MySqlCommand(sql, kapcsolodas);
                cmd.Parameters.Add("@param1", MySqlDbType.Text).Value = 100;
                cmd.Parameters.Add("@param2", MySqlDbType.Text).Value = 0;
                cmd.Parameters.Add("@param3", MySqlDbType.Text).Value = 0;
                cmd.Parameters.Add("@param4", MySqlDbType.Text).Value = 0;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                kapcsolodas.Close();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        // Select username for register ()
        public static int RegisterCheck(string username)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT username FROM player WHERE username = " +
                "(@param1)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = username;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32((dt.Rows.Count.ToString()));               
            }
            catch (Exception e) { Console.WriteLine(e); }
            if (i == 0)
            {
                kapcsolodas.Close();
                return 0;
            }
            else
            {
                kapcsolodas.Close();
                return 1;
            }


        }

        // Select id for login (Username & Password)
        public static int Select(string username, string password)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT id FROM player WHERE username = " +
                "(@param1)" +
                "and password = " +
                "(@param2)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = username;
                command.Parameters.Add("@param2", MySqlDbType.Text).Value = password;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            catch (Exception e) { Console.WriteLine(e); }
            if (i == 0)
            {
                kapcsolodas.Close();
                return 0;
            }
            else
            {
                kapcsolodas.Close();
                return 1;
            }


        }

        //Select player id
        public static int Select_Player_Id(string username)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT id FROM player WHERE username = " +
                "(@param1)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = username;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows[0]);
                
            }
            catch (Exception e) { Console.WriteLine(e);}
            if (i > 0)
            {
                return i;
            }
            else return 0;
        }

        //Select player arany
        public static int Select_Player_Arany(int id)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT arany FROM loot WHERE player_id = " +
                "(@param1)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = id;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows[0]);

            }
            catch (Exception e) { Console.WriteLine(e); }
            if (i > 0)
            {
                return i;
            }
            else return 0;
        }

        //Select player tapasztalati pont
        public static int Select_Player_Tapasztalati_Pont(int id)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT tapasztalati_pont FROM loot WHERE player_id = " +
                "(@param1)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = id;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows[0]);

            }
            catch (Exception e) { Console.WriteLine(e); }
            if (i > 0)
            {
                return i;
            }
            else return 0;
        }

        //Select player jogosultsag
        public static int Select_Player_Jogosultsag(int id)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT jogosulatsag FROM loot WHERE player_id = " +
                "(@param1)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = id;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows[0]);

            }
            catch (Exception e) { Console.WriteLine(e); }
            if (i > 0)
            {
                return i;
            }
            else return 0;
        }

        //Select player id
        public static int Select_Player_Palya(int id)
        {
            int i = 0;
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "SELECT palya FROM loot WHERE player_id = " +
                "(@param1)";
                command = new MySqlCommand(sql, kapcsolodas);
                command.Parameters.Add("@param1", MySqlDbType.Text).Value = id;
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows[0]);

            }
            catch (Exception e) { Console.WriteLine(e); }
            if (i > 0)
            {
                return i;
            }
            else return 0;
        }

        // Tapasztalati szint hozzáadása
        public static void Update(int id, int tapasztalat)
        {
            //Fő tábla kitöltése
            try
            {
                kapcsolodas.Open();
                command = kapcsolodas.CreateCommand();
                string sql = "UPDATE loot SET tapasztalati_pont += @param1 WHERE loot.player_id = @param2;";
                MySqlCommand cmd = new MySqlCommand(sql, kapcsolodas);
                cmd.Parameters.Add("@param1", MySqlDbType.Text).Value = tapasztalat;
                cmd.Parameters.Add("@param2", MySqlDbType.Text).Value = id;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                kapcsolodas.Close();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
    }
}
