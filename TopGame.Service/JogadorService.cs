using System;
using TopGame.Core.Domain;
using TopGame.Core.Extensions;
using TopGame.Core.Infrastructure;
using TopGame.Data;

namespace TopGame.Service
{
    public class JogadorService : IJogadorService
    {
        #region Fields

        private readonly JogadorRepository _jogadorRepository;

        #endregion

        #region Ctor

        public JogadorService()
        {
            _jogadorRepository = new JogadorRepository();
        }

        #endregion

        #region Methods

        public Jogador GetById(int id)
        {
            return _jogadorRepository.GetById(id);
        }

        public Jogador GetByToken(string token)
        {
            return _jogadorRepository.GetByToken(token);
        }

        public Jogador GetByDocumento(string documento)
        {
            return _jogadorRepository.GetByDocumento(documento);
        }

        public Jogador Add(Jogador jogador)
        {
            jogador = _jogadorRepository.GetByDocumento(jogador.Documento) ??
                    _jogadorRepository.Add(new Jogador
                    {
                        Nome = jogador.Nome,
                        Email = jogador.Email,
                        Documento = jogador.Documento,
                        DataCriacao = DateTime.Now
                    });

            return jogador;
        }

        public JogadorToken GetToken(int id, string token)
        {
            return _jogadorRepository.GetToken(id, token);
        }

        public JogadorToken CriaToken(Jogador jogador)
        {
            var jogadorToken = StringExtension.ConvertToToken(jogador.Documento, jogador.DataCriacao);
            var token = _jogadorRepository.GetToken(jogador.JogadorId, jogadorToken) ??
                _jogadorRepository.CriaToken(new JogadorToken
                {
                    JogadorId = jogador.JogadorId,
                    Jogador = jogador,
                    Codigo = jogadorToken,
                    DataCriacao = DateTime.Now
                });

            return token;
        }

        #endregion
    }
}