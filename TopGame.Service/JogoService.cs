using System;
using System.Collections.Generic;
using System.Linq;
using TopGame.Core.Data;
using TopGame.Core.Domain;
using TopGame.Core.Infrastructure.Services;

namespace TopGame.Service
{
    public class JogoService : IJogoService
    {
        #region Fields

        private readonly IRepository<Jogo> _jogoRepository;

        #endregion

        #region Ctor

        public JogoService(IRepository<Jogo> jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        #endregion

        #region Methods

        public Jogo GetByToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var query = from j in _jogoRepository.Table
                        where j.Token == token && j.Ativo && (j.DataPublicacao < DateTime.Now && j.DataExpiracao > DateTime.Now)
                        select j;

            var jogo = query.FirstOrDefault();
            return jogo;
        }

        public IEnumerable<Premiacao> GetPremios(int id)
        {
            return _jogoRepository.Table.Where(x => x.JogoId == id).SelectMany(x => x.Premiacoes);
        }

        #endregion
    }
}