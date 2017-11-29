namespace tabuleiro {
    class peca {

        public Posicao posicao { get; set; }
        public cor cor { get; protected set; }
        public int qteMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        public peca(Tabuleiro tab, cor cor) {
            this.posicao = null;
            this.tab = tab;
            this.cor = cor;
            this.qteMovimentos = 0;
        }

        public void incrementarQteMovimentos() {
            qteMovimentos++;
        }
    }
}
