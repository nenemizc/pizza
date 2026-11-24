using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace pizza
{
    internal class Adatbazis
    {
        readonly string connectionString = "server=localhost;database=pizzeria;uid=root;pwd=;";

        public List<Rendeles> Feltolt()
        {
            List<Rendeles> lista = new();
            Dictionary<int, Pizza> pizzak = new();
            Dictionary<int, Cim> cimek = new();
            try
            {
                using (MySqlConnection conn = new(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("Database connection opened.");
                    string query = "SELECT * FROM pizza";
                    using (MySqlCommand command = new(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                Pizza p = new(id, reader.GetString("nev"), reader.GetInt32("meret"), reader.GetInt32("ar"));
                                pizzak[id] = p;
                            }
                        }
                    }
                    query = "SELECT * FROM cim";
                    using (MySqlCommand command = new(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                Cim c = new(id, reader.GetString("nev"), reader.GetString("utca"), reader.GetString("hsz"));
                                cimek[id] = c;
                            }
                        }
                    }
                    query = "SELECT * FROM rendeles";
                    using (MySqlCommand command = new(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                Pizza p = pizzak[reader.GetInt32("pizzaid")];
                                Cim c = cimek[reader.GetInt32("cimid")];
                                int db = reader.GetInt32("darab");
                                TimeSpan t = reader.GetTimeSpan("szallitas");
                                lista.Add(new Rendeles(id, p, db, c, t));
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Hiba történt az adatbázis elérésénél." + ex.Message);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt: " + ex.Message);
                Environment.Exit(0);
            }
            return lista;
        }
    }
    }
