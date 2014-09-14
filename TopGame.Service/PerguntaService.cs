using System.Collections.Generic;
using TopGame.Core.Domain;
using TopGame.Core.Infrastructure;
using TopGame.Data;

namespace TopGame.Service
{
    public class PerguntaService : IPerguntaRepository
    {
        #region Fields

        private readonly PerguntaRepository _perguntaRepository;

        #endregion

        #region Ctor

        public PerguntaService()
        {
            _perguntaRepository = new PerguntaRepository();
        }

        #endregion

        #region Methods

        public Pergunta GetAleatoria(int jogoId, string token) 
        {
            return _perguntaRepository.GetAleatoria(jogoId, token);
        }

        public Pergunta GetNaoRespondida(int jogoId, string token)
        {
            return _perguntaRepository.GetNaoRespondida(jogoId, token);
        }

        public int CountTotalRespondida(int jogoId, string token)
        {
            return _perguntaRepository.CountTotalRespondida(jogoId, token);
        }

        public IEnumerable<PerguntaResposta> GetRespostas(Pergunta pergunta)
        {
            return _perguntaRepository.GetRespostas(pergunta);
        }

        public PerguntaRespostaJogador GetRespostaDoJogador(int perguntaId, int? respostaId, int jogadorId, int jogadorTokenId)
        {
            return _perguntaRepository.GetRespostaDoJogador(perguntaId, respostaId, jogadorId, jogadorTokenId);
        }

        public void IncluiRespostaDoJogador(PerguntaRespostaJogador respostaJogador)
        {
            _perguntaRepository.IncluiRespostaDoJogador(respostaJogador);
        }

        public void AtualizaRespostaDoJogador(PerguntaRespostaJogador respostaJogador)
        {
            _perguntaRepository.AtualizaRespostaDoJogador(respostaJogador);
        }

        #endregion
    }
}