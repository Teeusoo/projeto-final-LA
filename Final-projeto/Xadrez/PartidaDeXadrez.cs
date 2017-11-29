using System;
using tabuleiro;
namespace Xadrez {
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        private int turno;
        private cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = cor.Branca;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino) {
            peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
        }
        private void colocarPecas() {
            tab.colocarPeca(new torre(tab, cor.Branca), new PosicaoXadrez('c', 1).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Branca), new PosicaoXadrez('c', 2).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Branca), new PosicaoXadrez('d', 2).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Branca), new PosicaoXadrez('e', 2).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Branca), new PosicaoXadrez('e', 1).toPosicao());
            tab.colocarPeca(new rei(tab, cor.Branca), new PosicaoXadrez('d', 1).toPosicao());

            tab.colocarPeca(new torre(tab, cor.Preta), new PosicaoXadrez('c', 7).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Preta), new PosicaoXadrez('c', 8).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Preta), new PosicaoXadrez('d', 7).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Preta), new PosicaoXadrez('e', 7).toPosicao());
            tab.colocarPeca(new torre(tab, cor.Preta), new PosicaoXadrez('e', 8).toPosicao());
            tab.colocarPeca(new rei(tab, cor.Preta), new PosicaoXadrez('d', 8).toPosicao());



        }
    }
}
