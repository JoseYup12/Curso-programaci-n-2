using System;
using System.Linq; // Se importa LINQ

namespace EjercicioLINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Lista de números de ejemplo
            int[] numeros = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Consulta LINQ para filtrar solo los números pares
            var numerosPares = from n in numeros
                               where n % 2 == 0
                               select n;

            // Mostrar los números pares en pantalla
            Console.WriteLine("Números pares en la lista:");
            foreach (var num in numerosPares)
            {
                Console.WriteLine(num);
            }

            // Espera para que la consola no se cierre automáticamente
            Console.ReadLine();
        }
    }
}
