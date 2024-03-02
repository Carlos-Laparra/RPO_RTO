using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexion_Mysql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                int opcionElgida = 0;

                while (opcionElgida != 6)
                {
                    Clases.CConexion objConexion = new Clases.CConexion();
                    Console.WriteLine("Ejercicio de virtualización RTO - RPO: " +
                    "\r\nPresione 1 para testear la base de datos.");

                    opcionElgida = Convert.ToInt32(Console.ReadLine()); ;

                    /*PARA HACER UNA CONSULTA*/
                    if (opcionElgida == 1)
                    {
                        Console.Clear();
                        objConexion.establecerConexion();
                        opcionElgida = Salir();
                    }
                    /*PARA HACER UNA CONSULTA*/
                   
                    Console.Clear();
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }

        static int Salir()
        {
            Console.ResetColor();
            Console.WriteLine("Si desea salir del programa presione el número 6, de lo contrario presione cualquier número");
            int num = Convert.ToInt32(Console.ReadLine());

            if (num == 6)
            {
                Console.WriteLine("Fin del programa, adios :)");
            }
            return num;

        }

    }
}