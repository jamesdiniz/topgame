using System;
using System.Web.Mvc;
using TopGame.Core.Domain;
using TopGame.Core.Extensions;
using TopGame.Service;
using TopGame.Web.Models;

namespace TopGame.Web.Controllers
{
    public class ContaController : Controller
    {
        #region Fields

        private readonly JogadorService _jogadorService;

        #endregion

        #region Ctor

        public ContaController()
        {
            _jogadorService = new JogadorService();
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Identificacao(JogadorViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Informações inválidas.");

                var jogador = _jogadorService.Add(new Jogador
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Documento = model.Documento.LimpaCaracter()
                });

                if (jogador == null) throw new Exception("Erro ao registrar o jogador");

                var token = _jogadorService.CriaToken(jogador);
                if (token == null) throw new Exception("Erro ao gerar chave de acesso do jogador");

                return Json(new
                {
                    status = "OK",
                    message = "Cadastro efetuado com sucesso",
                    jogadorId = jogador.JogadorId,
                    token = token.Codigo
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = "ERRO",
                    message = e.Message
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Busca(string documento)
        {
            var jogador = _jogadorService.GetByDocumento(documento);

            return Json(jogador != null ? (object) new { status = "OK", nome = jogador.Nome, email = jogador.Email } : new { status = "" });
        }
    }
}