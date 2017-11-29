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

        public void colocarPeca(peca p,Posicao pos) {
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }
    }
}
