using System;
using tabuleiro;
using Xadrez;

namespace Final_projeto {
    class Program {
        static void Main(string[] args) {

            try {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.terminada) {

                    Console.Clear();
                    tela.imprimirTabuleiro(partida.tab);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = tela.lerPosicaoXadrez().toPosicao();

                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                    Console.Clear();
                    tela.imprimirTabuleiro(partida.tab,posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Posicao destino = tela.lerPosicaoXadrez().toPosicao();

                    partida.executaMovimento(origem, destino);
                }

            }
            catch (tabuleiroException e) {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();

        }
    }
}
