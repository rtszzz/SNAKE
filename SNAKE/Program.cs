using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        enum Direccion
        {
            Abajo, Izq, Der, Arriba
        }
        public static void Main()
        {
            var velocidad = 150;
            var posicionComida = Point.Empty;
            var tamPantalla = new Size(70, 20);
            var serpiente = new List<Point>();
            var longSerpiente = 4;
            var puntuacion = 0;
            var posicion = new Point(0, 9);
            serpiente.Add(posicion);
            var direccion = Direccion.Abajo;

            Pantalla(tamPantalla);
            Marcador(puntuacion);


            while (Movimiento(serpiente, posicion, longSerpiente, tamPantalla))
            {
                Thread.Sleep(velocidad);
                direccion = DameDireccion(direccion);
                posicion = ProximaPosicion(direccion, posicion);
                if (posicion.Equals(posicionComida))
                {
                    posicionComida = Point.Empty;
                    longSerpiente++;
                    puntuacion += 5;
                    Marcador(puntuacion);
                }

                if (posicionComida == Point.Empty)
                    posicionComida = mostrarComida(tamPantalla, serpiente);

            }

            Console.ResetColor();
            Console.SetCursorPosition(tamPantalla.Width / 2 - 4, tamPantalla.Height / 2);
            Console.Write("Game Over");
            Thread.Sleep(2000);
            Console.ReadKey();

        }

        private static void Pantalla(Size tam)
        {
            Console.WindowHeight = tam.Height + 2;
            Console.WindowWidth = tam.Width + 2;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.Title = "Serpiente";
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;

            for (int row = 0; row < tam.Height; row++)
                for (int col = 0; col < tam.Width; col++)
                {
                    Console.SetCursorPosition(col + 1, row + 1);
                    Console.Write(" ");
                }

        }

        private static void Marcador(int puntos)
        {
            Console.SetCursorPosition(1, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Puntos " + puntos.ToString());

        }
        private static Direccion DameDireccion(Direccion dirActual)
        {
            if (!Console.KeyAvailable) return dirActual;

            var tecla = Console.ReadKey(true).Key;
            switch (tecla)
            {
                case ConsoleKey.DownArrow:
                    dirActual = Direccion.Abajo;
                    break;
                case ConsoleKey.LeftArrow:
                    dirActual = Direccion.Izq;
                    break;
                case ConsoleKey.RightArrow:
                    dirActual = Direccion.Der;
                    break;
                case ConsoleKey.UpArrow:
                    dirActual = Direccion.Arriba;
                    break;


            }
            return dirActual;
        }

        private static Point ProximaPosicion(Direccion dir, Point posActual)
        {
            Point proximaPosicion = new Point(posActual.X, posActual.Y);
            switch (dir)
            {
                case Direccion.Arriba:
                    proximaPosicion.Y--;
                    break;
                case Direccion.Izq:
                    proximaPosicion.X--;
                    break;
                case Direccion.Abajo:
                    proximaPosicion.Y++;
                    break;
                case Direccion.Der:
                    proximaPosicion.X++;
                    break;
            }
            return proximaPosicion;
        }

        private static Point mostrarComida(Size tampPantalla, List<Point> Serpiente)
        {
            var puntoComida = Point.Empty;
            var cabezaSerpiente = Serpiente.Last();
            var rnd = new Random();
            do
            {
                var x = rnd.Next(0, tampPantalla.Width - 1);
                var y = rnd.Next(0, tampPantalla.Height - 1);
                if (Serpiente.All(p => p.X != x || p.Y != y) &&
                    Math.Abs(x - cabezaSerpiente.X) + Math.Abs(y - cabezaSerpiente.Y) > 8)
                    puntoComida = new Point(x, y);

            } while (puntoComida == Point.Empty);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(puntoComida.X + 1, puntoComida.Y + 1);

            Console.Write(" ");
            return puntoComida;
        }

        private static bool Movimiento(List<Point> serpiente, Point posicion, int longSerpiente, Size tamPantalla)
        {
            var ultimoPunto = serpiente.Last();
            if (ultimoPunto.Equals(posicion)) return true;
            if (serpiente.Any(x => x.Equals(posicion))) return false;

            if (posicion.X < 0 || posicion.X >= tamPantalla.Width || posicion.Y < 0 || posicion.Y >= tamPantalla.Height)
                return false;

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(ultimoPunto.X + 1, ultimoPunto.Y + 1);
            Console.WriteLine(" ");

            serpiente.Add(posicion);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(posicion.X + 1, posicion.Y + 1);
            Console.Write(" ");

            if (serpiente.Count > longSerpiente)
            {
                var removePoint = serpiente[0];
                serpiente.RemoveAt(0);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(removePoint.X + 1, removePoint.Y + 1);
                Console.Write(" ");
            }

            return true;
        }
    }

}
