using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopGame.App.Bilionario.Service;
using TopGame.Core.Domain;
using TopGame.Core.Domain.Pontuacao;
using TopGame.Core.Infrastructure.Services;
using TopGame.Web.Models;

namespace TopGame.Web.Controllers
{
    public class JogatinaController : Controller
    {
        #region Fields

        private readonly IConfiguracaoService _configuracaoService;
        private readonly IJogoService _jogoService;
        private readonly IPerguntaService _perguntaService;
        private readonly IJogadorService _jogadorService;
        private readonly IPontuacaoService _pontuacaoService;
        private readonly AppService _appService;
        
        #endregion

        #region Ctor

        public JogatinaController(IConfiguracaoService configuracaoService,
            IJogoService jogoService,
            IPerguntaService perguntaService,
            IJogadorService jogadorService,
            IPontuacaoService pontuacaoService) 
        {
            _configuracaoService = configuracaoService;
            _jogoService = jogoService;
            _perguntaService = perguntaService;
            _jogadorService = jogadorService;
            _pontuacaoService = pontuacaoService;
            _appService = new AppService();
        }

        #endregion

        #region Methods

        public ActionResult Index(string jogoId)
        {
            var jogo = _jogoService.GetByToken(jogoId);
            if (jogo == null) return Content("Jogo solicitado não encontrado ou expirado.");

            var premiacoes = _jogoService.GetPremios(jogo.JogoId).ToList();

            var model = new JogoViewModel
            {
                Token = jogo.Token,
                Titulo = jogo.Titulo,
                Descricao = jogo.Descricao,
                Logo = "/Images/Jogo/" + jogo.JogoId + "/logo.png",
                TemPremios = premiacoes.Count > 0,
                Premios = premiacoes,
                Jogador = new JogadorViewModel()
            };

            return View(model);
        }

        public ActionResult Pergunta(string jogoId, int jogadorId, string token)
        {
            try
            {
                var jogo = _jogoService.GetByToken(jogoId);
                if (jogo == null) throw new Exception("Jogo solicitado não encontrado ou expirado.");

                var jogadorToken = Autentica(jogadorId, token);
                var configuracao = ValidaConfiguracao(jogo.JogoId, jogadorToken);
                var pergunta = ValidaPergunta(jogo.JogoId, jogadorToken);

                var model = new RodadaViewModel
                {
                    PerguntaId = pergunta.PerguntaId,
                    Pergunta = pergunta.Descricao,
                    Foto = "/Images/Jogo/" + pergunta.JogoId + "/Baixa/" + pergunta.PerguntaId + ".jpg",
                    Configuracao = configuracao,
                    TipoResposta = pergunta.TipoResposta,
                    Respostas = pergunta.PerguntaRespostas.Select(resposta => new RespostaViewModel
                    {
                        Id = resposta.PerguntaRespostaId,
                        Descricao = resposta.Descricao,
                        AtributoItem = pergunta.TipoResposta == TipoResposta.Digitacao ? new { maxlength = 5, size = 2, style = "margin-left: 5px;" } : null
                    }).ToList()
                };

                return PartialView(model);
            }
            catch (Exception e)
            {
                return new PartialViewResult
                {
                    ViewName = "_Erro",
                    ViewData = new ViewDataDictionary { Model = new ErroViewModel { Mensagem = e.Message } }
                };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Pergunta(string jogoId, int jogadorId, string token, FormCollection formCollection)
        {
            try
            {
                var jogadorToken = Autentica(jogadorId, token);

                foreach (var key in formCollection.AllKeys.Where(x => x.StartsWith("RSP|")))
                {
                    var parts = GetParts(key);
                    var pergunta = new Pergunta
                    {
                        PerguntaId = Convert.ToInt32(parts[0]),
                        TipoResposta = (TipoResposta)Convert.ToInt32(formCollection["TipoResposta"])
                    };

                    var jogadorResposta = _perguntaService.GetRespostaDoJogador(
                        pergunta.PerguntaId,
                        pergunta.TipoResposta == TipoResposta.Digitacao ? Convert.ToInt32(parts[1]) : (int?) null,
                        jogadorId,
                        jogadorToken.JogadorTokenId
                    );

                    if (jogadorResposta == null) throw new Exception("Erro ao localizar pergunta.");
                    
                    jogadorResposta.Resposta = formCollection[key];
                    jogadorResposta.Status = "R";

                    _perguntaService.AtualizaRespostaDoJogador(jogadorResposta);
                    GeraPontuacao(pergunta, _perguntaService.GetRespostas(pergunta), jogadorResposta);
                }

                return Json(new { status = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { status = "ERRO", message = e.Message });
            }
        }

        private void GeraPontuacao(Pergunta pergunta, IEnumerable<PerguntaResposta> respostas, PerguntaRespostaJogador jogadorResposta)
        {
            switch (pergunta.TipoResposta)
            {
                case TipoResposta.Alternativa:
                {
                    var resposta = respostas.FirstOrDefault(x => x.Correta != null && (bool)x.Correta);

                    if (resposta != null &&
                        resposta.PerguntaRespostaId.Equals(Convert.ToInt32(jogadorResposta.Resposta)))
                    {
                        var app = new PontuacaoApp
                        {
                            Quantidade = resposta.QuantidadePonto,
                            PerguntaId = pergunta.PerguntaId,
                            JogadorId = jogadorResposta.JogadorId
                        };
                        _pontuacaoService.Add(app);
                    }
                        
                }
                    break;
                case TipoResposta.Digitacao:
                {
                    var resposta = respostas.FirstOrDefault(x => x.PerguntaRespostaId == jogadorResposta.PerguntaRespostaId);
                    if (resposta == null) return;

                    decimal pontoPosicao = 0;
                    decimal pontoFortuna = 0;

                    if (resposta.Ordem == 0)
                        pontoPosicao = resposta.QuantidadePonto - Convert.ToDecimal(jogadorResposta.Resposta);
                    else
                        pontoFortuna = resposta.QuantidadePonto - Convert.ToDecimal(jogadorResposta.Resposta);

                    if (pontoPosicao < 0) pontoPosicao = pontoPosicao*-1;
                    if (pontoFortuna < 0) pontoFortuna = pontoFortuna*-1;

                    _appService.IncluiPontuacao(pontoPosicao, pontoFortuna, jogadorResposta.PerguntaRespostaJogadorId, jogadorResposta.JogadorId);
                }
                    break;
            }
        }

        /// <summary>
        /// Cria uma associação da pergunta a ser respondida pelo jogador
        /// </summary>
        /// <param name="jogadorToken">Instância do token criptografado do jogador</param>
        /// <param name="pergunta">Instância da pergunta a ser respondida</param>
        /// <param name="respostaId">Sequência de identificação da resposta que será respondida, caso tipo seja de digitação</param>
        private void CriaPerguntaDoJogador(JogadorToken jogadorToken, Pergunta pergunta, int? respostaId)
        {
            _perguntaService.IncluiRespostaDoJogador(new PerguntaRespostaJogador
            {
                Resposta = null,
                Status = "P",
                PerguntaId = pergunta.PerguntaId,
                PerguntaRespostaId = respostaId,
                JogadorId = jogadorToken.JogadorId,
                JogadorTokenId = jogadorToken.JogadorTokenId
            });
        }

        /// <summary>
        /// Verifica se existe pergunta a ser respondida ou traz alguma nova aleatória
        /// </summary>
        /// <param name="jogoId">Sequência de identificação do jogo</param>
        /// <param name="jogadorToken">Sequência criptografada do jogador</param>
        /// <returns></returns>
        private Pergunta ValidaPergunta(int jogoId, JogadorToken jogadorToken)
        {
            var pergunta = _perguntaService.GetByStatus(jogoId, jogadorToken.Codigo);
            if (pergunta != null) return pergunta;

            pergunta = _perguntaService.GetAleatoria(jogoId, jogadorToken.Codigo);
            if (pergunta == null) throw new Exception("Erro ao tentar localizar pergunta.");

            if (pergunta.TipoResposta == TipoResposta.Digitacao)
                foreach (var resposta in pergunta.PerguntaRespostas)
                    CriaPerguntaDoJogador(jogadorToken, pergunta, resposta.PerguntaRespostaId);
            else
                CriaPerguntaDoJogador(jogadorToken, pergunta, null);

            return pergunta;
        }

        /// <summary>
        /// Verifica se jogador já respondeu todas as perguntas
        /// </summary>
        /// <param name="jogoId">Sequência de identificação do jogo</param>
        /// <param name="jogadorToken">Instância do token criptografado do jogador</param>
        /// <returns></returns>
        private ConfiguracaoViewModel ValidaConfiguracao(int jogoId, JogadorToken jogadorToken)
        {
            var qtdPergunta = _configuracaoService.GetConfiguracao(jogoId).QuantidadePergunta;
            var qtdRespondida = _perguntaService.CountByStatus(jogoId, jogadorToken.Codigo, "R");
            if (qtdRespondida >= qtdPergunta) throw new Exception("Você já respondeu todas as perguntas.<br />Obrigado por participar.<br /><br />Os resultados serão divulgados até o final do evento.<br /><a href=\"/Jogos/Bilionarios/356a192b7913b04c54574d18c28d46e6395428ab\" class=\"btn-reiniciar\">Reiniciar</a>");

            var configuracao = new ConfiguracaoViewModel
            {
                QuantidadePergunta = qtdPergunta,
                QuantidadeRespondida = qtdRespondida + 1
            };

            return configuracao;
        }

        /// <summary>
        /// Verifica autenticidade do jogador 
        /// </summary>
        /// <param name="jogadorId">Sequência de identificação do jogo</param>
        /// <param name="token">Sequência criptografada no formato SHA1</param>
        /// <returns></returns>
        private JogadorToken Autentica(int jogadorId, string token)
        {
            var jogador = _jogadorService.GetById(jogadorId);
            var jogadorToken = _jogadorService.GetToken(jogador.JogadorId, token);
            if (jogadorToken == null) throw new Exception("Token de autenticação inválido.");

            return jogadorToken;
        }

        #endregion

        #region Private static methods

        private static string[] GetParts(string value)
        {
            var parts = value.Split('|').Where(part => part != "RSP").ToArray();

            return parts;
        }

        #endregion
    }
}