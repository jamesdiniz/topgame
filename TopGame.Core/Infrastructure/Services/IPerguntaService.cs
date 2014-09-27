using System.Collections.Generic;
using TopGame.Core.Domain;

namespace TopGame.Core.Infrastructure.Services
{
    public interface IPerguntaService
    {
        Pergunta GetAleatoria(int jogoId, string token);

        Pergunta GetByStatus(int jogoId, string token, string status = "P");
        
        int CountByStatus(int jogoId, string token, string status = "P");

        IEnumerable<PerguntaResposta> GetRespostas(Pergunta pergunta);

        PerguntaRespostaJogador GetRespostaDoJogador(int perguntaId, int? respostaId, int jogadorId, int jogadorTokenId);

        void IncluiRespostaDoJogador(PerguntaRespostaJogador respostaJogador);

        void AtualizaRespostaDoJogador(PerguntaRespostaJogador respostaJogador);
    }
}