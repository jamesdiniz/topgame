using System;
using System.Drawing;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using TopGame.Core.Domain;
using TopGame.Core.Extensions;
using TopGame.Core.Helpers;
using TopGame.Core.Infrastructure;
using TopGame.Web.Models;

namespace TopGame.Web.Controllers
{
    public class ContaController : Controller
    {
        #region Fields

        private readonly IJogadorService _jogadorService;

        #endregion

        #region Ctor

        public ContaController(IJogadorService jogadorService)
        {
            _jogadorService = jogadorService;
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Identificacao(JogadorViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Informações inválidas.");

                var jogador = _jogadorService.GetByDocumento(model.Documento.RemoveNaoNumericos());
                if (jogador == null)
                {
                    jogador = new Jogador
                    {
                        Nome = model.Nome,
                        Email = model.Email,
                        Documento = model.Documento.RemoveNaoNumericos(),
                        DataCriacao = DateTime.Now
                    };

                    _jogadorService.AddJogador(jogador);

                    if (!string.IsNullOrEmpty(model.Foto))
                    {
                        var diretorio = HostingEnvironment.MapPath("~/Images/Jogador/");
                   
                        using (var stream = new MemoryStream(Convert.FromBase64String(model.Foto)))
                        {
                            Image image = new Bitmap(stream);
                            image.Save(string.Format("{0}/{1}", diretorio, "large-" + jogador.JogadorId + ".png"));
                            var thumb = image.GetThumbnailImage(96, 96, null, new IntPtr(0));
                            thumb.Save(string.Format("{0}/{1}", diretorio, "thumb-" + jogador.JogadorId + ".png"));
                        }
                    }
                }

                var token = _jogadorService.AddToken(jogador);
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
            var jogador = _jogadorService.GetByDocumento(documento.RemoveNaoNumericos());
            var resultado = jogador != null
                ? (object) new {status = "OK", nome = jogador.Nome, email = jogador.Email}
                : new {status = ""};
            
            return Json(resultado);
        }
    }
}