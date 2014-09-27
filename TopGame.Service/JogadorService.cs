using System;
using System.Linq;
using TopGame.Core.Data;
using TopGame.Core.Domain;
using TopGame.Core.Extensions;
using TopGame.Core.Infrastructure.Services;

namespace TopGame.Service
{
    public class JogadorService : IJogadorService
    {
        #region Fields

        private readonly IRepository<Jogador> _jogadorRepository;
        private readonly IRepository<JogadorToken> _jogadorTokenRepository;

        #endregion

        #region Ctor

        public JogadorService(IRepository<Jogador> jogadorRepository,
            IRepository<JogadorToken> jogadorTokenRepository)
        {
            _jogadorRepository = jogadorRepository;
            _jogadorTokenRepository = jogadorTokenRepository;
        }

        #endregion

        #region Methods

        public Jogador GetById(int id)
        {
            return _jogadorRepository.GetById(id);
        }

        public Jogador GetByToken(string token)
        {
            var query = from j in _jogadorRepository.Table
                        join tk in _jogadorTokenRepository.Table on j.JogadorId equals tk.JogadorId
                        where tk.Codigo == token
                        select j;

            var jogador = query.FirstOrDefault();
            return jogador;
        }

        public Jogador GetByDocumento(string documento)
        {
            var query = from j in _jogadorRepository.Table
                        where j.Documento == documento
                        select j;

            var jogador = query.FirstOrDefault();
            return jogador;
        }

        public void AddJogador(Jogador jogador)
        {
            _jogadorRepository.Add(jogador);
        }

        public JogadorToken GetToken(int id, string token)
        {
            var query = from t in _jogadorTokenRepository.Table
                        where t.JogadorId == id && t.Codigo == token
                        select t;

            var jogadorToken = query.FirstOrDefault();
            return jogadorToken;
        }

        public JogadorToken AddToken(Jogador jogador)
        {
            var token = jogador.Documento.ToToken(jogador.DataCriacao);
            var jogadorToken = GetToken(jogador.JogadorId, token);

            if (jogadorToken == null)
            {
                jogadorToken = new JogadorToken
                {
                    JogadorId = jogador.JogadorId,
                    Codigo = token,
                    DataCriacao = DateTime.Now
                };

                _jogadorTokenRepository.Add(jogadorToken);
            }

            return jogadorToken;
        }

        #endregion
    }
}