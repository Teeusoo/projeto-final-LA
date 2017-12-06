using System.Collections.Generic;
using tabuleiro;
namespace Xadrez {
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<peca> pecas;
        private HashSet<peca> capturadas;

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = cor.Branca;
            terminada = false;
            pecas = new HashSet<peca>();
            capturadas = new HashSet<peca>();
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino) {
            peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
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

        public HashSet<peca> pecasCapturadas(cor cor)
        {
            HashSet<peca> aux = new HashSet<peca>();
            foreach (peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<peca> pecasEmJogo(cor cor)
        {
            HashSet<peca> aux = new HashSet<peca>();
            foreach (peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void colocarNovaPeca(char coluna, int linha, peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas() {

            colocarNovaPeca('c', 1, new torre(tab, cor.Branca));
            colocarNovaPeca('c', 2, new torre(tab, cor.Branca));
            colocarNovaPeca('d', 2, new torre(tab, cor.Branca));
            colocarNovaPeca('e', 2, new torre(tab, cor.Branca));
            colocarNovaPeca('e', 1, new torre(tab, cor.Branca));
            colocarNovaPeca('d', 1, new rei(tab, cor.Branca));

            colocarNovaPeca('c', 7, new torre(tab, cor.Preta));
            colocarNovaPeca('c', 8, new torre(tab, cor.Preta));
            colocarNovaPeca('d', 7, new torre(tab, cor.Preta));
            colocarNovaPeca('e', 7, new torre(tab, cor.Preta));
            colocarNovaPeca('e', 8, new torre(tab, cor.Preta));
            colocarNovaPeca('d', 8, new rei(tab, cor.Preta));          

        }
    }
}
