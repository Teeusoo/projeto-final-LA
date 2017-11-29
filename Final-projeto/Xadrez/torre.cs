using tabuleiro;

namespace Xadrez {
    class torre : peca {

        public torre(Tabuleiro tab,cor cor) : base(tab,cor) {
        }
       

        public override string ToString() {
            return "T";
        }
    }
}

