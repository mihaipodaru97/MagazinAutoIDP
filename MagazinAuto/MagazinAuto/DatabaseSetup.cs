using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinAuto
{
    public class DatabaseSetup
    {
        private readonly string connectionString;
        public DatabaseSetup(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void Setup()
        {
            var sql = @"CREATE TABLE IF NOT EXISTS public.users (
                        id uuid NOT NULL,
                        nume varchar(30) NOT NULL,
                        email varchar(30) NOT NULL,
                        parola varchar(30) NOT NULL,
                        telefon varchar(13) NOT NULL,
                        CONSTRAINT users_pk PRIMARY KEY(id)
                        );

                        CREATE TABLE IF NOT EXISTS public.anunturi (
                        id uuid NOT NULL,
	                    caroserie numeric NOT NULL,
	                    cutie numeric NOT NULL,
	                    transmisie numeric NOT NULL,
	                    normapoluare numeric NOT NULL,
	                    combustibil numeric NOT NULL,
	                    cp numeric NOT NULL,
	                    capacitatecilindrica numeric NOT NULL,
	                    km numeric NOT NULL,
	                    pret numeric NOT NULL,
	                    anfabricatie numeric NOT NULL,
	                    marca varchar(100) NOT NULL,
	                    model varchar(100) NOT NULL,
	                    descriere varchar(500) NULL,
	                    proprietarid uuid NOT NULL,
	                    poza bytea NULL,
	                    CONSTRAINT anunturi_users_fk FOREIGN KEY (proprietarid) REFERENCES public.users(id),
                        CONSTRAINT anunturi_pk PRIMARY KEY(id)
                        );";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

        }
    }
}
