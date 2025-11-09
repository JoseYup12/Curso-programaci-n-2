using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GestionAcademicaLINQ
{
    // Clase base Persona
    class Persona
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
    }

    // Subclase Estudiante
    class Estudiante : Persona
    {
        public string Curso { get; set; }
        public double Nota { get; set; }

        public Estudiante(string nombre, int edad, string curso, double nota)
        {
            Nombre = nombre;
            Edad = edad;
            Curso = curso;
            Nota = nota;
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, Edad: {Edad}, Curso: {Curso}, Nota: {Nota}";
        }
    }

    // Subclase Profesor
    class Profesor : Persona
    {
        public string Curso { get; set; }

        public Profesor(string nombre, int edad, string curso)
        {
            Nombre = nombre;
            Edad = edad;
            Curso = curso;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Lista de estudiantes (simulando el archivo)
            List<Estudiante> estudiantes = new List<Estudiante>()
            {
                new Estudiante("Ana López", 20, "Matemática", 85),
                new Estudiante("Carlos Pérez", 21, "Matemática", 72),
                new Estudiante("María Gómez", 19, "Física", 90),
                new Estudiante("José Martínez", 22, "Física", 65),
                new Estudiante("Lucía Rivera", 20, "Química", 88),
                new Estudiante("Diego Castillo", 23, "Química", 95),
                new Estudiante("Elena Soto", 18, "Matemática", 55),
                new Estudiante("Luis Herrera", 21, "Física", 78),
                new Estudiante("Rosa Ramírez", 20, "Química", 60),
                new Estudiante("Pedro Molina", 19, "Matemática", 100)
            };

            // Lista de profesores
            List<Profesor> profesores = new List<Profesor>()
            {
                new Profesor("Ing. Morales", 45, "Matemática"),
                new Profesor("Lic. Duarte", 38, "Física"),
                new Profesor("Dr. García", 50, "Química")
            };

            Console.WriteLine("=== Gestión Académica (LINQ) ===\n");

            // a) Estudiantes aprobados (nota >= 70)
            var aprobados = from e in estudiantes
                            where e.Nota >= 70
                            select e;

            Console.WriteLine("Estudiantes aprobados (nota >= 70):\n");
            foreach (var e in aprobados) Console.WriteLine(e);

            // b) Top 5 de estudiantes por curso
            Console.WriteLine("\nTop 5 de estudiantes por curso:\n");
            var top5PorCurso = from e in estudiantes
                               group e by e.Curso into grupo
                               select new
                               {
                                   Curso = grupo.Key,
                                   Top5 = grupo.OrderByDescending(x => x.Nota).Take(5)
                               };

            foreach (var grupo in top5PorCurso)
            {
                Console.WriteLine($"Curso: {grupo.Curso}");
                foreach (var e in grupo.Top5) Console.WriteLine($"  {e.Nombre} - {e.Nota}");
                Console.WriteLine();
            }

            // c) Calcular promedio por curso
            Console.WriteLine("Promedio por curso:\n");
            var promedioPorCurso = from e in estudiantes
                                   group e by e.Curso into grupo
                                   select new
                                   {
                                       Curso = grupo.Key,
                                       Promedio = grupo.Average(x => x.Nota)
                                   };
            foreach (var g in promedioPorCurso)
                Console.WriteLine($"Curso: {g.Curso}, Promedio: {g.Promedio:F2}");

            // d) Top 10 general de todos los cursos
            Console.WriteLine("\nTop 10 general (todas las materias):\n");
            var top10 = estudiantes.OrderByDescending(e => e.Nota).Take(10);
            foreach (var e in top10) Console.WriteLine(e);

            // e) Ranking general (posición según nota)
            Console.WriteLine("\nRanking general de estudiantes:\n");
            var ranking = estudiantes.OrderByDescending(e => e.Nota)
                                     .Select((e, index) => new { Posición = index + 1, Estudiante = e });
            foreach (var r in ranking)
                Console.WriteLine($"#{r.Posición} - {r.Estudiante.Nombre} ({r.Estudiante.Nota})");

            // f) Mejor estudiante por curso
            Console.WriteLine("\nMejor estudiante por curso:\n");
            var mejorPorCurso = from e in estudiantes
                                group e by e.Curso into grupo
                                select grupo.OrderByDescending(x => x.Nota).First();
            foreach (var e in mejorPorCurso)
                Console.WriteLine($"Curso: {e.Curso}, Mejor: {e.Nombre}, Nota: {e.Nota}");

            // g) Mostrar estudiantes por intervalos de nota
            Console.WriteLine("\nEstudiantes por intervalos de nota:\n");
            var intervalos = new[]
            {
                new { Rango = "0-59", Est = estudiantes.Where(e => e.Nota >= 0 && e.Nota <= 59) },
                new { Rango = "60-79", Est = estudiantes.Where(e => e.Nota >= 60 && e.Nota <= 79) },
                new { Rango = "80-100", Est = estudiantes.Where(e => e.Nota >= 80 && e.Nota <= 100) }
            };

            foreach (var rango in intervalos)
            {
                Console.WriteLine($"Rango {rango.Rango}:");
                foreach (var e in rango.Est) Console.WriteLine($"  {e.Nombre} - {e.Nota}");
                Console.WriteLine();
            }

            // Exportar resultados a un archivo .txt (en el Escritorio)
            string rutaArchivo = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\reporte_academico.txt";
            using (StreamWriter sw = new StreamWriter(rutaArchivo))
            {
                sw.WriteLine("=== Reporte Académico ===\n");

                sw.WriteLine("Estudiantes aprobados (nota >= 70):");
                foreach (var e in aprobados) sw.WriteLine(e);
                sw.WriteLine();

                sw.WriteLine("Promedio por curso:");
                foreach (var g in promedioPorCurso)
                    sw.WriteLine($"Curso: {g.Curso}, Promedio: {g.Promedio:F2}");
                sw.WriteLine();

                sw.WriteLine("Mejor estudiante por curso:");
                foreach (var e in mejorPorCurso)
                    sw.WriteLine($"Curso: {e.Curso}, Mejor: {e.Nombre}, Nota: {e.Nota}");
                sw.WriteLine();

                sw.WriteLine("Ranking general:");
                foreach (var r in ranking)
                    sw.WriteLine($"#{r.Posición} - {r.Estudiante.Nombre} ({r.Estudiante.Nota})");
            }

            Console.WriteLine($"\n📁 Reporte exportado exitosamente en: {rutaArchivo}");
            Console.ReadLine();
        }
    }
}
