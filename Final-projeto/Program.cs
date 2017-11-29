using System;
using tabuleiro;


namespace Final_projeto {
    class Program {
        static void Main(string[] args) {

            Tabuleiro tab = new Tabuleiro(8, 8);

            tela.imprimirTabuleiro(tab);


            Console.ReadLine();
            
        }
    }
}
