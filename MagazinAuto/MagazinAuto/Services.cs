using MagazinAuto.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinAuto
{
    public class Services
    {
        private readonly string connectionString;
        public Services(IConfiguration config)
        {
            connectionString = config.GetConnectionString("Database");
        }

        public void AddUser(RegisterModel user)
        {
            var sql = "INSERT INTO public.users VALUES(@id,@nume, @email, @parola, @telefon)";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("id", user.Id);
                    command.Parameters.AddWithValue("nume", user.Nume);
                    command.Parameters.AddWithValue("email", user.Email);
                    command.Parameters.AddWithValue("parola", user.Password);
                    command.Parameters.AddWithValue("telefon", user.Telefon);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var sql = "SELECT * FROM public.users WHERE email = @email";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("email", email);
                    
                    using(var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        var user = new User
                        {
                            Id = (Guid)reader[0],
                            Nume = (string)reader[1],
                            Email = (string)reader[2],
                            Telefon = (string)reader[4]
                        };

                        if (reader.Read())
                        {
                            return null;
                        }

                        return user;
                    }
                }
            }
        }

        public Guid GetUser()
        {
            var sql = "SELECT * FROM public.users";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return Guid.Empty;
                        }

                        return (Guid)reader[0];
                    }
                }
            }
        }

        public User CheckUser(string email, string password)
        {
            var sql = "SELECT * FROM public.users WHERE email = @email AND parola = @parola";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("parola", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        var user = new User
                        {
                            Id = (Guid)reader[0],
                            Nume = (string)reader[1],
                            Email = (string)reader[2],
                            Telefon = (string)reader[4]
                        };

                        if (reader.Read())
                        {
                            return null;
                        }

                        return user;
                    }
                }
            }
        }

        public void AddCar(MasinaAdd car, Guid? userId, byte[] poza)
        {
            if(userId == null)
            {
                userId = GetUser();
            }

            if(userId == Guid.Empty)
            {
                return;
            }

            car.Id = Guid.NewGuid();

            var sql = "INSERT INTO public.anunturi VALUES(@id, @caroserie, @cutie, @transmisie, @normapoluare, @combustibil, @cp, @capacitatecilindrica," +
                "@km, @pret, @anfabricatie, @marca, @model, @descriere, @proprietarid, @poza)";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("id", car.Id);
                    command.Parameters.AddWithValue("caroserie", (int)car.Caroserie);
                    command.Parameters.AddWithValue("cutie", (int)car.Cutie);
                    command.Parameters.AddWithValue("transmisie", (int)car.Transmisie);
                    command.Parameters.AddWithValue("normapoluare", (int)car.NormaPoluare);
                    command.Parameters.AddWithValue("combustibil", (int)car.Combustibil);
                    command.Parameters.AddWithValue("cp", car.CP);
                    command.Parameters.AddWithValue("capacitatecilindrica", car.CapacitateCilindrica);
                    command.Parameters.AddWithValue("km", car.Km);
                    command.Parameters.AddWithValue("pret", car.Pret);
                    command.Parameters.AddWithValue("anfabricatie", car.AnFabricatie);
                    command.Parameters.AddWithValue("marca", car.Marca);
                    command.Parameters.AddWithValue("model", car.Model);
                    command.Parameters.AddWithValue("descriere", car.Descriere);
                    command.Parameters.AddWithValue("proprietarid", userId);
                    command.Parameters.AddWithValue("poza", poza);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<MasinaView> GetCars()
        {
            var sql = "SELECT * FROM public.anunturi A, public.users B WHERE A.proprietarid = B.id";
            var result = new List<MasinaView>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new MasinaView
                            {
                                Id = (Guid)reader[0],
                                Caroserie = (Caroserie)decimal.ToInt32((decimal)reader[1]),
                                Cutie = (Cutie)decimal.ToInt32((decimal)reader[2]),
                                Transmisie = (Transmisie)decimal.ToInt32((decimal)reader[3]),
                                NormaPoluare = (NormaPoluare)decimal.ToInt32((decimal)reader[4]),
                                Combustibil = (Combustibil)decimal.ToInt32((decimal)reader[5]),
                                CP = decimal.ToInt32((decimal)reader[6]),
                                CapacitateCilindrica = decimal.ToInt32((decimal)reader[7]),
                                Km = decimal.ToInt32((decimal)reader[8]),
                                Pret = decimal.ToInt32((decimal)reader[9]),
                                AnFabricatie = decimal.ToInt32((decimal)reader[10]),
                                Marca = (string)reader[11],
                                Model = (string)reader[12],
                                Descriere = (string)reader[13],
                                Poza = (byte[])reader[15],
                                Proprietar = new User
                                {
                                    Id = (Guid)reader[16],
                                    Nume = (string)reader[17],
                                    Email = (string)reader[18],
                                    Telefon = (string)reader[20]
                                }
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
