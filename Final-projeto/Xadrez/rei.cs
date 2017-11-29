using tabuleiro;

namespace Xadrez {
    class rei: peca {

        public rei(Tabuleiro tab, cor cor) : base(tab,cor) {
        }

        public override string ToString() {
            return "R";
        }
    }
}
