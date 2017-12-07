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
        public bool xeque { get; private set; }

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = cor.Branca;
            terminada = false;
            xeque = false;
            pecas = new HashSet<peca>();
            capturadas = new HashSet<peca>();
            colocarPecas();
        }

        public peca executaMovimento(Posicao origem, Posicao destino) {
            peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, peca pecaCapturada)
        {
            peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if(pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);
        }

        public void realizaJogada(Posicao origem,Posicao destino)
        {
            peca pecaCapturada = executaMovimento(origem, destino);

            if(estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new tabuleiroException("Você não pode se colocar em xeque!");
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }

            else
            {
                xeque = false;
            }
            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {

                turno++;
                mudaJogador();
            }
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
            if (!tab.peca(origem).movimentoPossivel(destino))
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

        private cor adversaria(cor cor)
        {
            if(cor == cor.Branca)
            {
                return cor.Preta;
            }
            else
            {
                return cor.Branca;
            }
        }

        private peca rei(cor cor)
        {
            foreach ( peca x in pecasEmJogo(cor))
            {
                if(x is rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmXeque(cor cor)
        {
            peca R = rei(cor);
            if(R == null)
            {
                throw new tabuleiroException("Não tem rei da cor" + cor + "no tabuleiro!");
            }
            foreach(peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha , R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for(int j=0;j<tab.colunas;j++)
                    {
                        if(mat[i,j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if(!testeXeque)
                            {
                                return false;
                            }

                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas() {

            colocarNovaPeca('c', 1, new torre(tab, cor.Branca));
            colocarNovaPeca('h', 7, new torre(tab, cor.Branca));
            colocarNovaPeca('d', 1, new rei(tab, cor.Branca));


            colocarNovaPeca('b', 8, new torre(tab, cor.Preta));
            colocarNovaPeca('a', 8, new rei(tab, cor.Preta));          

        }
    }
}
