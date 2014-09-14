using System;
using System.Collections.Generic;
using TopGame.Core.Domain;
using TopGame.Core.Domain.Pontuacao;
using TopGame.Core.Infrastructure;
using TopGame.Data;

namespace TopGame.Service
{
    public class JogoService : IJogoRepository
    {
        #region Fields

        private readonly JogoRepository _jogoRepository;

        #endregion

        #region Ctor

        public JogoService()
        {
            _jogoRepository = new JogoRepository();
        }

        #endregion

        #region Methods

        public Jogo GetByToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            var jogo = _jogoRepository.GetByToken(token);

            return jogo == null || !jogo.Ativo || (DateTime.Now < jogo.DataPublicacao || DateTime.Now > jogo.DataExpiracao) ? null : jogo;
        }

        public IEnumerable<Premiacao> GetPremios(int id)
        {
            return _jogoRepository.GetPremios(id);
        }


        public IEnumerable<PontuacaoApp> GetRanking(int id)
        {
            return _jogoRepository.GetRanking(id);
        }

        #endregion
    }
}