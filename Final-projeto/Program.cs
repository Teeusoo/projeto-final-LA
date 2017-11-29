using System;
using tabuleiro;
using Xadrez;

namespace Final_projeto {
    class Program {
        static void Main(string[] args) {

            Tabuleiro tab = new Tabuleiro(8, 8);

            tab.colocarPeca(new torre(tab, cor.Preta), new Posicao(0, 0));
            tab.colocarPeca(new torre(tab, cor.Preta), new Posicao(1, 3));
            tab.colocarPeca(new rei(tab, cor.Preta), new Posicao(2, 4));

            tela.imprimirTabuleiro(tab);


            Console.ReadLine();

        }
    }
}
