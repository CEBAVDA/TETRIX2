using System;
using System.Collections.Generic; 
using System.Drawing;
using System.Media;
using System.Diagnostics;

namespace TETRIX
{
    class Program
    {
        //área del juego : AJuego
        public static int[,] Ajuego = new int[20, 10];
        //posición caída : caída
        public static int[,] caída = new int[20, 10];
        public static string cuadrado = "■";
        public static Stopwatch temporizador = new Stopwatch();
        //caída : C
        public static Stopwatch temporizadorC = new Stopwatch();
        //entrada : E
        public static Stopwatch temporizadorE = new Stopwatch();
        public static int tiempoC, velocidadC = 250;
        public static bool dejarC = false;
        static PIEZAS piezas;
        static PIEZAS proxpieza;
        public static ConsoleKeyInfo tecla;
        //tecla pulsada : teclaP
        public static bool teclaP = false;
        //líneas ganadas : líneasG
        public static int lineasG = 0, puntos = 0, nivel = 1;

        static void Main()
        {
            pintarAjuego();

            Console.SetCursorPosition(4, 4);
            Console.WriteLine("Pulse una tecla");
            Console.ReadKey(true);
            /*AQUÍ VA LAS LÍNEAS PARA COLOCAR LA MÚSICA*/
            temporizador.Start();
            temporizadorC.Start();
            long time = temporizador.ElapsedMilliseconds;
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("Nivel " + nivel);
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Puntos " + puntos);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("Lineas " + lineasG);
            proxpieza = new PIEZAS();
            //piezas = proxpieza;
            //piezas.aparecer();
            proxpieza = new PIEZAS();

            actualizar();

            /*aca tmb va el codigo para poner la musica*/

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game over, desea continuar? (Y/N)");
            string teclaP = Console.ReadLine();

            if (teclaP.ToUpper() == "Y" || teclaP.ToUpper() == "Y")
            {
                int[,] Ajuego = new int[20, 10];
                caída = new int[20, 10];
                temporizador = new Stopwatch();
                temporizadorC = new Stopwatch();
                temporizadorE = new Stopwatch();
                velocidadC = 200;
                dejarC = false;
                Program.teclaP = false;
                lineasG = 0;
                puntos = 0;
                nivel = 1;
                GC.Collect();
                Console.Clear();
                Main();
            }

            else return;

        }
        public static void pintarAjuego()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int lengthCount = 0; lengthCount<=19; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.Write("|");
                Console.SetCursorPosition(20, lengthCount);
                Console.Write("|");
            }
            Console.SetCursorPosition(0,  20);
            for (int widthCount = 0; widthCount<=10; ++widthCount)
            {
                Console.Write("-~");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void actualizar()
        {
            while (true)
            {
                tiempoC = (int)temporizadorC.ElapsedMilliseconds;
                if (tiempoC > velocidadC)
                {
                    tiempoC = 0;
                    temporizadorC.Restart();
                    //piezas.aparecer();

                    dejarC = false;
                }

                int j;
                for (j = 0; j < 10; j++)
                {
                    if (caída[0, j] == 1)
                        return;
                }

                pulsación();
                moverB();
            }
        }
        private static void pulsación()
        {
            if (Console.KeyAvailable)
            {
                tecla = Console.ReadKey();
                teclaP = true;
            }
            else teclaP = false;
            //HayAlgoIzquierda y HayAlgoDerecha
            /*if (Program.tecla.key == ConsoleKey.LeftArrow & !PIEZAS.HayAlgoIzquierda() & teclaPulsada)
            {
                for (int i=0; i< 4; i++) 
                {
                    PIEZAS.posición[i][1] -= 1;
                }
                PIEZAS.actualizar();
            }
            if (Program.tecla.key == ConsoleKey.RightArrow & !PIEZAS.HayAlgoDerecha() & teclaPulsada)
            {
                for (int i = 0; i < 4; i++)
                {
                    PIEZAS.posición[i][1] += 1;
                }
                PIEZAS.actualizar();
            }
            if (Program.tecla.key == ConsoleKey.DownArrow & teclaPulsada)
            {
                PIEZAS.soltar();
            }
            if (Program.tecla.key ==  ConsoleKey.DownArrow & teclaPulsada)
            {
                for (; PIEZAS.HayAlgoDebajo()!= true;)
                {
                    PIEZAS.soltar();
                }
            }
            if (Program.tecla.key == ConsoleKey.UpArrow & teclaPulsada)
            {
                PIEZAS.Rotar();
                PIEZAS.actualizar();
            }*/
        }
        private static void moverB()
        {
            int combo = 0;
            for (int i = 0; i < 20; i++)
            {
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (caída[i, j] == 0)
                        break;
                }

                if (j == 10)
                {
                    lineasG++;
                    combo++;
                    for (j = 0; j < 10; j++)
                    {
                        caída[i, j] = 0;
                    }

                    int[,] newpiezas = new int[23, 10];
                    for(int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            newpiezas[k + 1, l] = caída[k, l];
                        }
                    }

                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            caída[k, l] = 0;
                        }
                    }

                    for (int k = 0; k < 20; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            if (newpiezas[k,l] == 1)
                            {
                                caída[k, 1] = 1;
                            }
                        }
                    }

                    dibujar();
    
                }
            }

            if (combo == 1)
                puntos += 50 * nivel;
            else if (combo == 2)
                puntos += 100 * nivel;
            else if (combo == 3)
                puntos += 200 * nivel;
            else if (combo > 3)
                puntos += 200 * combo * nivel;

            if (lineasG < 5) nivel = 1;
            else if (lineasG < 10) nivel = 2;
            else if (lineasG < 15) nivel = 3;
            else if (lineasG < 25) nivel = 4;
            else if (lineasG < 35) nivel = 5;
            else if (lineasG < 50) nivel = 6;
            else if (lineasG < 75) nivel = 7;
            else if (lineasG < 90) nivel = 8;
            else if (lineasG < 115) nivel = 9;
            else if (lineasG < 130) nivel = 10;

            if (combo > 0)
            {
                Console.SetCursorPosition(25, 0);
                Console.WriteLine("Nivel " + nivel);
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Puntos " + puntos);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("Lineas ganadas " + lineasG);
            }

            velocidadC = 250 - 22 * nivel;
        }
        
        public static void dibujar()
        {
            for(int i = 0; i < 20; i++)
            {
                for(int j=0; j < 10; j++)
                {
                    Console.SetCursorPosition(1  +  2  *j, i);
                    if(Ajuego[1,j]==1 || caída[i, j] == 1)
                    {
                        Console.SetCursorPosition(1 + 2 * j, i);

                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(cuadrado);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
        }


        
    }  
}
