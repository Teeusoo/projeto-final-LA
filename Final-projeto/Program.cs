using System;
using tabuleiro;
using Xadrez;

namespace Final_projeto {
    class Program {
        static void Main(string[] args) {

            try {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new torre(tab, cor.Preta), new Posicao(0, 0));
                tab.colocarPeca(new torre(tab, cor.Preta), new Posicao(1, 9));
                tab.colocarPeca(new rei(tab, cor.Preta), new Posicao(0, 2));

                tela.imprimirTabuleiro(tab);
            }
            catch (tabuleiroException e) {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();

        }
    }
}
