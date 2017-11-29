namespace tabuleiro {
    class Tabuleiro {

        public int linhas { get; set; }
        public int colunas { get; set; }
        private peca[,] pecas;

        public Tabuleiro(int linhas, int colunas) {
            this.linhas = linhas;
            this.colunas = colunas;
            pecas = new peca[linhas, colunas];

        }

        public peca peca(int linha, int coluna) {
            return pecas[linha, coluna];
        }

        public peca peca(Posicao pos) {
            return pecas[pos.linha, pos.coluna];
        }

        public bool existePeca(Posicao pos) {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public void colocarPeca(peca p,Posicao pos) {
            if(existePeca(pos)) {
                throw new tabuleiroException("Já existe uma peça nessa posição!");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }

        public bool posicaoValida(Posicao pos) {
            if (pos.linha<0 || pos.linha>=linhas || pos.coluna<0 || pos.coluna>=colunas) {
                return false;
            }
            return true;
        }

        public void validarPosicao(Posicao pos) {
            if(!posicaoValida(pos)) {
                throw new tabuleiroException("Posição inválida!");
            }
        }
    }
}
