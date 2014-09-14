using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using EasyAdo;
using EasyAdo.Extensions;
using TopGame.Core.Domain;
using TopGame.Core.Infrastructure;

namespace TopGame.Data
{
    public class PerguntaRepository : IPerguntaRepository
    {
        public Pergunta GetAleatoria(int jogoId, string token)
        {
            const string query = @"
                SELECT 
	                pergunta.PerguntaId,
	                pergunta.Descricao,
	                pergunta.Dica,
	                pergunta.TipoResposta,
                    pergunta.TipoRespostaAleatoria,
	                pergunta.Ativo,
	                pergunta.JogoId
                FROM Pergunta pergunta
                WHERE 
	                pergunta.JogoId = @jogoId
	                AND not exists(
		                SELECT resposta.PerguntaRespostaJogadorId
		                FROM PerguntaRespostaJogador resposta 
			                INNER JOIN JogadorToken token ON token.JogadorTokenId = resposta.JogadorTokenId
		                WHERE 
			                resposta.PerguntaId = pergunta.PerguntaId 
			                AND resposta.Status = 'R'
			                AND token.Codigo = @token
	                )
                ORDER BY NEWID()";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text,
                    data.ParameterBuilder()
                        .Add("jogoId", jogoId)
                        .Add("token", token)
                    ).AsDataReader();

                if (reader == null || !reader.Read()) return null;

                var idPergunta = reader.GetValueOrDefault<int>("PerguntaId");

                var pergunta = new Pergunta
                {
                    PerguntaId = idPergunta,
                    Descricao = reader.GetValueOrDefault<string>("Descricao"),
                    Dica = reader.GetValueOrDefault<string>("Dica"),
                    TipoResposta = (TipoResposta)reader.GetValueOrDefault<int>("TipoResposta"),
                    TipoRespostaAleatoria = reader.GetValueOrDefault<bool>("TipoRespostaAleatoria"),
                    Ativo = reader.GetValueOrDefault<bool>("Ativo"),
                    JogoId = reader.GetValueOrDefault<int>("JogoId")
                };

                pergunta.PerguntaRespostas = new Collection<PerguntaResposta>(GetRespostas(pergunta).ToList());

                return pergunta;
            }
        }

        public Pergunta GetNaoRespondida(int jogoId, string token)
        {
            const string query = @"
                SELECT 
	                pergunta.PerguntaId,
	                pergunta.Descricao,
	                pergunta.Dica,
	                pergunta.TipoResposta,
                    pergunta.TipoRespostaAleatoria,
	                pergunta.Ativo,
	                pergunta.JogoId
                FROM Pergunta pergunta
                WHERE 
	                pergunta.JogoId = @jogoId
	                AND exists(
		                SELECT resposta.PerguntaRespostaJogadorId
		                FROM PerguntaRespostaJogador resposta
			                INNER JOIN JogadorToken token ON token.JogadorTokenId = resposta.JogadorTokenId
		                WHERE 
			                resposta.PerguntaId = pergunta.PerguntaId 
			                AND resposta.Status = 'P'
			                AND token.Codigo = @token
                    )";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, 
                    data.ParameterBuilder()
                        .Add("jogoId", jogoId)
                        .Add("token", token)
                    ).AsDataReader();

                if (reader == null || !reader.Read()) return null;

                var idPergunta = reader.GetValueOrDefault<int>("PerguntaId");

                var pergunta = new Pergunta
                {
                    PerguntaId = idPergunta,
                    Descricao = reader.GetValueOrDefault<string>("Descricao"),
                    Dica = reader.GetValueOrDefault<string>("Dica"),
                    TipoResposta = (TipoResposta)reader.GetValueOrDefault<int>("TipoResposta"),
                    TipoRespostaAleatoria = reader.GetValueOrDefault<bool>("TipoRespostaAleatoria"),
                    Ativo = reader.GetValueOrDefault<bool>("Ativo"),
                    JogoId = reader.GetValueOrDefault<int>("JogoId")
                };

                pergunta.PerguntaRespostas = new Collection<PerguntaResposta>(GetRespostas(pergunta).ToList());

                return pergunta;
            }
        }

        public int CountTotalRespondida(int jogoId, string token)
        {
            const string query = @"
                SELECT COUNT(pergunta.PerguntaId)
                FROM Pergunta pergunta
                WHERE 
	                pergunta.JogoId = @jogoId
	                AND exists(
		                SELECT resposta.PerguntaRespostaJogadorId
		                FROM PerguntaRespostaJogador resposta
			                INNER JOIN JogadorToken token ON token.JogadorTokenId = resposta.JogadorTokenId
		                WHERE 
			                resposta.PerguntaId = pergunta.PerguntaId 
			                AND resposta.Status = 'R'
			                AND token.Codigo = @token
                    )";

            using (var data = new SqlContext())
            {
                return Convert.ToInt32(data.Execute(query, CommandType.Text, 
                    data.ParameterBuilder()
                        .Add("jogoId", jogoId)
                        .Add("token", token)
                ).AsScalar());
            }
        }

        public IEnumerable<PerguntaResposta> GetRespostas(Pergunta pergunta)
        {
            var query = @"
                SELECT 
                    PerguntaRespostaId, Descricao, Correta, 
                    QuantidadePonto, Ordem, PerguntaId 
                FROM PerguntaResposta
                WHERE PerguntaId = @perguntaId";

            query = string.Format("{0} ORDER BY {1}", query, (pergunta.TipoRespostaAleatoria ? "NEWID()" : "Ordem ASC"));

            var respostas = new List<PerguntaResposta>();

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("perguntaId", pergunta.PerguntaId)).AsDataReader();

                while (reader.Read())
                {
                    respostas.Add(new PerguntaResposta
                    {
                        PerguntaRespostaId = reader.GetValueOrDefault<int>("PerguntaRespostaId"),
                        Descricao = reader.GetValueOrDefault<string>("Descricao"),
                        Correta = !reader.IsDBNull(reader.GetOrdinal("Correta")) ? reader.GetValueOrDefault<bool>("Correta") : (bool?)null,
                        QuantidadePonto = reader.GetValueOrDefault<decimal>("QuantidadePonto"),
                        Ordem = !reader.IsDBNull(reader.GetOrdinal("Ordem")) ? reader.GetValueOrDefault<int>("Ordem") : (int?)null,
                        PerguntaId = reader.GetValueOrDefault<int>("PerguntaId")
                    });
                }
            }

            return respostas;
        }

        public PerguntaRespostaJogador GetRespostaDoJogador(int perguntaId, int? respostaId, int jogadorId, int jogadorTokenId)
        {
            var query = @"
                SELECT TOP 1 
                    PerguntaRespostaJogadorId, Resposta, Status,
                    PerguntaId, PerguntaRespostaId, JogadorId, JogadorTokenId
                FROM PerguntaRespostaJogador
                WHERE PerguntaId = @perguntaId 
                    AND JogadorId = @jogadorId 
                    AND JogadorTokenId = @jogadorTokenId
                    AND PerguntaRespostaId = @perguntaRespostaId";

            query = string.Format("{0} AND PerguntaRespostaId {1}", query, (respostaId == null ? "is null" : "= @perguntaRespostaId"));

            using (var data = new SqlContext())
            {
                var parameter = data.ParameterBuilder()
                    .Add("perguntaId", perguntaId)
                    .Add("jogadorId", jogadorId)
                    .Add("jogadorTokenId", jogadorTokenId);

                if (respostaId != null)
                    parameter.Add("perguntaRespostaId", respostaId);

                var reader = data.Execute(query, CommandType.Text, parameter).AsDataReader();
                if (reader == null || !reader.Read()) return null;

                var resposta = new PerguntaRespostaJogador
                {
                    PerguntaRespostaJogadorId = reader.GetValueOrDefault<int>("PerguntaRespostaJogadorId"),
                    Resposta = reader.GetValueOrDefault<string>("Resposta"),
                    Status = reader.GetValueOrDefault<string>("Status"),
                    PerguntaId = reader.GetValueOrDefault<int>("PerguntaId"),
                    PerguntaRespostaId = !reader.IsDBNull(reader.GetOrdinal("PerguntaRespostaId")) ? reader.GetValueOrDefault<int>("PerguntaRespostaId") : (int?)null,
                    JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                    JogadorTokenId = reader.GetValueOrDefault<int>("JogadorTokenId")
                };

                return resposta;
            }
        }

        public void IncluiRespostaDoJogador(PerguntaRespostaJogador respostaJogador)
        {
            const string query = @"INSERT INTO PerguntaRespostaJogador (Status, PerguntaId, PerguntaRespostaId, JogadorId, JogadorTokenId) 
                                   VALUES (@status, @perguntaId, @perguntaRespostaId, @jogadorId, @jogadorTokenId)";

            using (var data = new SqlContext())
            {
                data.Execute(query, CommandType.Text,
                    data.ParameterBuilder()
                        .Add("status", respostaJogador.Status)
                        .Add("perguntaId", respostaJogador.PerguntaId)
                        .Add("perguntaRespostaId", respostaJogador.PerguntaRespostaId == null ? DBNull.Value : (object)respostaJogador.PerguntaRespostaId)
                        .Add("jogadorId", respostaJogador.JogadorId)
                        .Add("jogadorTokenId", respostaJogador.JogadorTokenId)
                    ).AsNonQuery();
            }
        }

        public void AtualizaRespostaDoJogador(PerguntaRespostaJogador respostaJogador)
        {
            var query = @"
                UPDATE PerguntaRespostaJogador 
                  SET Status = @status, Resposta = @resposta 
                WHERE PerguntaId = @perguntaId 
                    AND JogadorId = @jogadorId 
                    AND JogadorTokenId = @jogadorTokenId";

            query = string.Format("{0} AND PerguntaRespostaId {1}", query, (respostaJogador.PerguntaRespostaId == null ? "is null" : "= @perguntaRespostaId"));

            using (var data = new SqlContext())
            {
                var parameter = data.ParameterBuilder()
                    .Add("status", respostaJogador.Status)
                    .Add("resposta", respostaJogador.Resposta)
                    .Add("perguntaId", respostaJogador.PerguntaId)
                    .Add("jogadorId", respostaJogador.JogadorId)
                    .Add("jogadorTokenId", respostaJogador.JogadorTokenId);

                if (respostaJogador.PerguntaRespostaId != null)
                    parameter.Add("perguntaRespostaId", respostaJogador.PerguntaRespostaId);

                data.Execute(query, CommandType.Text, parameter).AsNonQuery();
            }
        }
    }
}