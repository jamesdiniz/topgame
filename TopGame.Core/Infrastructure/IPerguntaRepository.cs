using System.Collections.Generic;
using TopGame.Core.Domain;

namespace TopGame.Core.Infrastructure
{
    public interface IPerguntaRepository
    {
        Pergunta GetAleatoria(int jogoId, string token);
        
        Pergunta GetNaoRespondida(int jogoId, string token);

        int CountTotalRespondida(int jogoId, string token);

        IEnumerable<PerguntaResposta> GetRespostas(Pergunta pergunta);

        PerguntaRespostaJogador GetRespostaDoJogador(int perguntaId, int? respostaId, int jogadorId, int jogadorTokenId);

        void IncluiRespostaDoJogador(PerguntaRespostaJogador respostaJogador);

        void AtualizaRespostaDoJogador(PerguntaRespostaJogador respostaJogador);
    }
}