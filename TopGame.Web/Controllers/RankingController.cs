using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopGame.App.Bilionario.Service;
using TopGame.Core.Domain.Pontuacao;
using TopGame.Core.Infrastructure.Services;
using TopGame.Web.Models;

namespace TopGame.Web.Controllers
{
    [RoutePrefix("Ranking")]
    public class RankingController : Controller
    {
        #region Fields

        private readonly IJogoService _jogoService;
        private readonly AppService _appService;

        #endregion

        #region Ctor

        public RankingController(IJogoService jogoService)
        {
            _jogoService = jogoService;
            _appService = new AppService();
        }

        #endregion

        #region Methods

        #endregion

        [Route("Bilionarios/{jogoId}")]
        public ActionResult AppBilionariosLista(string jogoId, bool? c)
        {
            var jogo = _jogoService.GetByToken(jogoId);
            var ranking = _appService.GetRanking(jogo.JogoId, c);
            var model = ranking.Select(x => new RankingViewModel
            {
                JogadorId = x.Jogador.JogadorId, 
                Jogador = x.Jogador.Nome, 
                Pontuacao = x.PontoRanking,
                PontuacaoAdicional = x.PontoFortuna
            }).ToList();

            ViewBag.Jogo = jogo.Titulo;

            return View(model);
        }

        [Route("Bilionarios/{jogoId}/Detalhes")]
        public PartialViewResult AppBilionariosDetalhes(string jogoId, int jogadorId, int posicao)
        {
            var jogo = _jogoService.GetByToken(jogoId);
            var ranking = _appService.GetRankingDetalhes(jogo.JogoId, jogadorId).ToList();
            var jogador = ranking.FirstOrDefault();

            var model = new RankingDetalheViewModel
            {
                JogoId = jogo.JogoId,
                Posicao = posicao,
                JogadorId = jogador.JogadorId,
                Jogador = jogador.Jogador.Nome,
                Perguntas = MontaDadosViewModels(ranking)
            };

            ViewBag.Jogo = jogo.Titulo;

            return PartialView("_AppBilionariosDetalhes", model);
        }

        [Route("Bilionarios/{jogoId}/Posicao/{posicao}")]
        public ActionResult AppBilionariosPosicao(string jogoId, int posicao)
        {
            var jogo = _jogoService.GetByToken(jogoId);
            var index = posicao - 1;
            var ranking = _appService.GetRanking(jogo.JogoId, true).ToList();
            var rankingJogador = ranking.ElementAt(index);

            var model = new RankingPosicaoViewModel
            {
                JogoId = jogo.JogoId,
                JogadorId = rankingJogador.Jogador.JogadorId,
                Jogador = rankingJogador.Jogador.Nome,
                Posicao = posicao,
                PontoRanking = rankingJogador.PontoRanking,
                PontoFortuna = rankingJogador.PontoFortuna
            };

            return View(model);
        }

        // TODO: Isso está completamente grotesco, mas devido a pressa (como sempre) não tive outra alternativa, portanto, sérá resolvido quando for necessário
        private static IEnumerable<RankingPerguntaViewModel> MontaDadosViewModels(IEnumerable<PontuacaoAppBilionarios> ranking)
        {
            var lista = new List<RankingPerguntaViewModel>();

            foreach (var t in ranking.GroupBy(x => x.PerguntaRespostaJogador.PerguntaId))
            {
                var perguntaRanking = t.ElementAt(0);
                var perguntaFortuna = t.ElementAt(1);
                var jogadorResposta1 = perguntaRanking.PerguntaRespostaJogador;
                var jogadorResposta2 = perguntaFortuna.PerguntaRespostaJogador;

                lista.Add(new RankingPerguntaViewModel
                {
                    PerguntaId = jogadorResposta1.PerguntaId,
                    PerguntaDescricao = jogadorResposta1.Pergunta.Descricao,
                    PontoRanking = (decimal)perguntaRanking.PontoRanking,
                    PontoFortuna = (decimal)perguntaFortuna.PontoFortuna,
                    RespostaCorretaRanking = jogadorResposta1.PerguntaResposta.QuantidadePonto,
                    RespostaCorretaFortuna = jogadorResposta2.PerguntaResposta.QuantidadePonto,
                    RespostaInformadaRanking = Convert.ToDecimal(jogadorResposta1.Resposta),
                    RespostaInformadaFortuna = Convert.ToDecimal(jogadorResposta2.Resposta),
                    Ordem = 0
                });
            }

            return lista;
        }
    }
}