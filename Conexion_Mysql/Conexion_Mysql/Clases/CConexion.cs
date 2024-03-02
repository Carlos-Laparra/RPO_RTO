using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Conexion_Mysql.Clases
{
    class CConexion
    {
        MySqlConnection conexion = new MySqlConnection();

        static string servidor = "localhost";
        static string bd = "db_ejercicio";
        static string usuario = "root";
        static string password = "CONTRA";
        static string puerto = "3304";
        Stopwatch stopwatch = new Stopwatch();
        int contador = 1;
        TimeSpan tiempoTranscurrido;
        string cadenaConexion = "server=" + servidor + ";" + "port=" + puerto + ";" + "user id=" + usuario + ";" + "password=" + password + ";" + "database=" + bd + ";";

        public MySqlConnection establecerConexion()
        {
            try
            {
                conexion.ConnectionString = cadenaConexion;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (contador < 11)
                {
                    try
                    {
                        conexion.Open();
                        // Si la conexión se establece correctamente, sale del bucle
                        break;
                    }
                    catch (MySqlException)
                    {
                        // Si hay un error, incrementa el contador y espera 1 segundo antes de intentar nuevamente
                        contador++;
                        Thread.Sleep(1000);
                    }
                }

                stopwatch.Stop();

                if (conexion.State == ConnectionState.Open)
                {
                    tiempoTranscurrido = stopwatch.Elapsed;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Se logró conectar a la base de datos correctamente. El tiempo de la consulta fue de: " + stopwatch.Elapsed.TotalSeconds + " segundos.");
                }
                else
                {
                    tiempoTranscurrido += stopwatch.Elapsed;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Se intentó hacer la conexión a base de datos y no se pudo, el tiempo total fue de: " + stopwatch.Elapsed.TotalSeconds + " segundos.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error general: " + e.Message);
            }

            return conexion;
        }

        public string Select()
        {

            conexion.ConnectionString = cadenaConexion;
            conexion.Open();

            MySqlCommand comando = new MySqlCommand("SELECT idnombres as ID, nombrescol as Nombre FROM nombres", conexion);
            MySqlDataAdapter adaptador = new MySqlDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);

            string resultados = ConvertirDataTableAString(tabla);

            return resultados;
        }


        public void Insertar(string nombre)
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            // Preparar la consulta SQL de inserción
            string consulta = "INSERT INTO `db_ejercicio`.`nombres` (`nombrescol`) VALUES ('" + nombre + "')";

            try
            {
                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    // Parámetro para evitar SQL injection
                    comando.Parameters.AddWithValue("@nombre", nombre);

                    // Ejecutar la consulta de inserción
                    comando.ExecuteNonQuery();
                }

                Console.WriteLine("Inserción realizada con éxito.");
            }
            catch
            {
                Console.WriteLine("No se ha realizado la inserción");
            }
        }

        public void Actualizar(int id, string nuevoNombre)
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            // Preparar la consulta SQL de actualización
            string consulta = "UPDATE `db_ejercicio`.`nombres` SET `nombrescol` = '" + nuevoNombre + "' WHERE (`idnombres` = '" + id + "');";

            try
            {
                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    // Parámetros para evitar SQL injection
                    comando.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                    comando.Parameters.AddWithValue("@id", id);

                    // Ejecutar la consulta de actualización
                    comando.ExecuteNonQuery();

                    Console.WriteLine("Actualización realizada con éxito.");
                }
            }
            catch
            {
                Console.WriteLine("No se ha realizado el update");
            }
        }

        public void Borrar(int id)
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            // Preparar la consulta SQL de actualización
            string consulta = "DELETE FROM `db_ejercicio`.`nombres` WHERE (`idnombres` = '" + id + "');";

            try
            {
                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    // Parámetros para evitar SQL injection
                    comando.Parameters.AddWithValue("@id", id);

                    // Ejecutar la consulta de actualización
                    comando.ExecuteNonQuery();

                    Console.WriteLine("Delete realizado con éxito.");
                }
            }
            catch
            {
                Console.WriteLine("No se ha realizado el delete");
            }
        }

        static string ConvertirDataTableAString(DataTable tabla)
        {
            string resultados = "";

            // Iterar sobre las filas de la tabla
            foreach (DataRow fila in tabla.Rows)
            {
                // Iterar sobre las columnas de cada fila
                foreach (DataColumn columna in tabla.Columns)
                {
                    resultados += $"{columna.ColumnName}: {fila[columna]} | ";
                }

                resultados += Environment.NewLine; // Nueva línea para la siguiente fila
            }

            return resultados;
        }

    }
}