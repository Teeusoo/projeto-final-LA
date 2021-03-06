﻿using System.Collections.Generic;
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
        public peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = cor.Branca;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
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

            // #jogadaespecial roque pequeno
            if(p is rei && destino.coluna == origem.coluna +2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial en passant
            if (p is peao)
            {
                if(origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if(p.cor == cor.Branca)
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                } 
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

            // #jogadaespecial roque pequeno
            if (p is rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial en passant
            if (p is peao)
            {
                if(origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
                {
                    peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.cor == cor.Branca) { 
                        posP = new Posicao(3, destino.coluna);
                    }
                    else{
                        posP = new Posicao(4, destino.coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }

        public void realizaJogada(Posicao origem,Posicao destino)
        {
            peca pecaCapturada = executaMovimento(origem, destino);

            if(estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new tabuleiroException("Você não pode se colocar em xeque!");
            }

            peca p = tab.peca(destino);

            // #jogadaespecial promocao
            if(p is peao)
            {
                if((p.cor == cor.Branca && destino.linha == 0) || (p.cor == cor.Preta && destino.linha == 7)) {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    peca dama = new dama(tab, p.cor);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
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

            

            // #jogadaespecial en passant
            if(p is peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha +2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
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

            colocarNovaPeca('a',1,new torre(tab, cor.Branca));
            colocarNovaPeca('b',1,new cavalo(tab, cor.Branca));
            colocarNovaPeca('c',1,new bispo(tab, cor.Branca));
            colocarNovaPeca('d',1, new dama(tab, cor.Branca));
            colocarNovaPeca('e',1,new rei(tab, cor.Branca, this));
            colocarNovaPeca('f',1,new bispo(tab, cor.Branca));
            colocarNovaPeca('g',1,new cavalo(tab, cor.Branca));
            colocarNovaPeca('h',1, new torre(tab, cor.Branca));
            colocarNovaPeca('a',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('b',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('c',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('d',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('e',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('f',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('g',2, new peao(tab, cor.Branca, this));
            colocarNovaPeca('h',2, new peao(tab, cor.Branca, this));

            colocarNovaPeca('a', 8, new torre(tab, cor.Preta));
            colocarNovaPeca('b', 8, new cavalo(tab, cor.Preta));
            colocarNovaPeca('c', 8, new bispo(tab, cor.Preta));
            colocarNovaPeca('d', 8, new dama(tab, cor.Preta));
            colocarNovaPeca('e', 8, new rei(tab, cor.Preta, this));
            colocarNovaPeca('f', 8, new bispo(tab, cor.Preta));
            colocarNovaPeca('g', 8, new cavalo(tab, cor.Preta));
            colocarNovaPeca('h', 8, new torre(tab, cor.Preta));
            colocarNovaPeca('a', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('b', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('c', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('d', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('e', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('f', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('g', 7, new peao(tab, cor.Preta, this));
            colocarNovaPeca('h', 7, new peao(tab, cor.Preta, this));



        }
    }
}
