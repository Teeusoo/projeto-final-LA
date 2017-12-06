using System;
using tabuleiro;
namespace Xadrez {
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public cor jogadorAtual { get; private set; }
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
        public void realizaJogada(Posicao origem,Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            mudaJogador();
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new tabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if(jogadorAtual != tab.peca(pos).cor)
            {
                throw new tabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new tabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }
        public void validarPosicaoDeDestino(Posicao origem,Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino))
            {
                throw new tabuleiroException("Posição de destino inválida!");
            }
        }
        
        private void mudaJogador()
        {
            if (jogadorAtual == cor.Branca)
            {
                jogadorAtual = cor.Preta;
            }
            else
            {
                jogadorAtual = cor.Branca;
            }
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
