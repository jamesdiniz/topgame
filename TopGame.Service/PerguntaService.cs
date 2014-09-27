using System;
using System.Collections.Generic;
using System.Linq;
using TopGame.Core.Data;
using TopGame.Core.Domain;
using TopGame.Core.Infrastructure.Services;

namespace TopGame.Service
{
    public class PerguntaService : IPerguntaService
    {
        #region Fields

        private readonly IRepository<Pergunta> _perguntaRepository;
        private readonly IRepository<PerguntaResposta> _respostaRepository;
        private readonly IRepository<PerguntaRespostaJogador> _respostaJogadorRepository;

        #endregion

        #region Ctor

        public PerguntaService(IRepository<Pergunta> perguntaRepository,
            IRepository<PerguntaResposta> respostaRepository,
            IRepository<PerguntaRespostaJogador> respostaJogadorRepository)
        {
            _perguntaRepository = perguntaRepository;
            _respostaRepository = respostaRepository;
            _respostaJogadorRepository = respostaJogadorRepository;
        }

        #endregion

        #region Methods

        public Pergunta GetAleatoria(int jogoId, string token) 
        {
            var query = from p in _perguntaRepository.Table
                        where p.JogoId == jogoId &&
                            !(from r in _respostaJogadorRepository.Table
                             where r.PerguntaId == p.PerguntaId && r.JogadorToken.Codigo == token && r.Status == "R"
                             select r).Any()
                        orderby Guid.NewGuid()
                        select p;

            var pergunta = query.FirstOrDefault();
            return pergunta;
        }

        public Pergunta GetByStatus(int jogoId, string token, string status = "P")
        {
            var query = from p in _perguntaRepository.Table
                        where p.JogoId == jogoId &&
                            (from r in _respostaJogadorRepository.Table
                             where r.PerguntaId == p.PerguntaId && r.JogadorToken.Codigo == token && r.Status == status
                            select r).Any()
                        select p;

            var pergunta = query.FirstOrDefault();
            return pergunta;
        }

        public int CountByStatus(int jogoId, string token, string status = "P")
        {
            var query = from p in _perguntaRepository.Table
                        where p.JogoId == jogoId &&
                            (from r in _respostaJogadorRepository.Table
                             where r.PerguntaId == p.PerguntaId && r.JogadorToken.Codigo == token && r.Status == status
                             select r).Any()
                        select p;

            var total = query.Count();
            return total;
        }

        public IEnumerable<PerguntaResposta> GetRespostas(Pergunta pergunta)
        {
            var query = from r in _respostaRepository.Table
                        where r.PerguntaId == pergunta.PerguntaId
                        select r;

            // ordena com base no tipo da pergunta
            query = pergunta.TipoRespostaAleatoria ? query.OrderBy(x => Guid.NewGuid()) : query.OrderBy(x => x.Ordem);

            var respostas = query.ToList();
            return respostas;
        }

        public PerguntaRespostaJogador GetRespostaDoJogador(int perguntaId, int? respostaId, int jogadorId, int jogadorTokenId)
        {
            var query = from r in _respostaJogadorRepository.Table
                        where r.PerguntaId == perguntaId && 
                            r.JogadorId == jogadorId && 
                            r.JogadorTokenId == jogadorTokenId &&
                            r.PerguntaRespostaId == respostaId
                        select r;

            var resposta = query.FirstOrDefault();
            return resposta;
        }

        public void IncluiRespostaDoJogador(PerguntaRespostaJogador respostaJogador)
        {
            _respostaJogadorRepository.Add(respostaJogador);
        }

        public void AtualizaRespostaDoJogador(PerguntaRespostaJogador respostaJogador)
        {
            _respostaJogadorRepository.Update(respostaJogador);
        }

        #endregion
    }
}