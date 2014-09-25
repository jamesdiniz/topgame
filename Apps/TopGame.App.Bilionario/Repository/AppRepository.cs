using System.Collections.Generic;
using System.Data;
using System.Text;
using EasyAdo;
using EasyAdo.Extensions;
using TopGame.Core.Domain;
using TopGame.Core.Domain.Pontuacao;

namespace TopGame.App.Bilionario.Repository
{
    public class AppRepository
    {
        public void IncluiPontuacao(decimal pontoRanking, decimal pontoFortuna, int jogadorRespostaId, int jogadorId)
        {
            const string query = "INSERT INTO PontuacaoAppBilionarios VALUES (@pontoRanking, @pontoFortuna, @jogadorRespostaId, @jogadorId)";

            using (var data = new SqlContext())
            {
                data.Execute(query, CommandType.Text,
                    data.ParameterBuilder()
                        .Add("pontoRanking", pontoRanking)
                        .Add("pontoFortuna", pontoFortuna)
                        .Add("jogadorRespostaId", jogadorRespostaId)
                        .Add("jogadorId", jogadorId)
                    ).AsNonQuery();
            }
        }

        public IEnumerable<PontuacaoAppBilionarios> GetRanking(int jogoId, bool? completo)
        {
            var sb = new StringBuilder(
                @"SELECT 
	                jogador.JogadorId, 
	                jogador.Nome, 
	                SUM(PontoRanking) as Ranking, 
	                SUM(PontoFortuna) as Fortuna
                FROM PontuacaoAppBilionarios pontuacao
	                INNER JOIN Jogador jogador 
		                ON jogador.JogadorId = pontuacao.JogadorId
	                INNER JOIN PerguntaRespostaJogador resposta
		                ON resposta.PerguntaRespostaJogadorId = pontuacao.PerguntaRespostaJogadorId
	                INNER JOIN Pergunta pergunta
		                ON pergunta.PerguntaId = resposta.PerguntaId
                WHERE pergunta.JogoId = @jogoId");

            if (completo != null)
            {
                sb.Append(@" AND (SELECT COUNT(distinct respostaJogador.PerguntaId)
                    FROM PerguntaRespostaJogador respostaJogador
                    WHERE respostaJogador.Status = 'R' AND respostaJogador.JogadorId = jogador.JogadorId)");

                sb.Append((bool) completo ? " = 3" : " < 3");
            }

            sb.Append(@" GROUP BY jogador.Nome, jogador.JogadorId ORDER BY Ranking, Fortuna");

            var query = sb.ToString();
            var ranking = new List<PontuacaoAppBilionarios>();

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("jogoId", jogoId)).AsDataReader();

                while (reader.Read())
                {
                    ranking.Add(new PontuacaoAppBilionarios
                    {
                        JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                        Jogador = new Jogador
                        {
                            JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                            Nome = reader.GetValueOrDefault<string>("Nome")
                        },
                        PontoRanking = (double)reader.GetValueOrDefault<decimal>("Ranking"),
                        PontoFortuna = (double)reader.GetValueOrDefault<decimal>("Fortuna")
                    });
                }
            }

            return ranking;
        }

        public IEnumerable<PontuacaoAppBilionarios> GetRankingDetalhes(int jogoId, int jogadorId)
        {
            const string query =
                @"SELECT 
	                pontuacao.PontuacaoId,
	                pontuacao.PontoRanking,
	                pontuacao.PontoFortuna,
	                jogador.JogadorId,
	                jogador.Nome,
	                respostaJogador.PerguntaRespostaJogadorId,
	                respostaJogador.Resposta,
	                resposta.PerguntaRespostaId,
	                resposta.QuantidadePonto,
	                resposta.Ordem,
	                pergunta.PerguntaId,
	                pergunta.Descricao
                FROM PontuacaoAppBilionarios pontuacao
	                INNER JOIN Jogador jogador ON jogador.JogadorId = pontuacao.JogadorId
	                INNER JOIN PerguntaRespostaJogador respostaJogador ON respostaJogador.PerguntaRespostaJogadorId = pontuacao.PerguntaRespostaJogadorId
	                INNER JOIN PerguntaResposta resposta ON resposta.PerguntaRespostaId = respostaJogador.PerguntaRespostaId
	                INNER JOIN Pergunta pergunta ON pergunta.PerguntaId = resposta.PerguntaId
                WHERE pergunta.JogoId = @jogoId and jogador.JogadorId = @jogadorId";

            var ranking = new List<PontuacaoAppBilionarios>();

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, 
                    data.ParameterBuilder()
                    .Add("jogoId", jogoId)
                    .Add("jogadorId", jogadorId)
                ).AsDataReader();

                while (reader.Read())
                {
                    ranking.Add(new PontuacaoAppBilionarios
                    {
                        PontuacaoId = reader.GetValueOrDefault<int>("PontuacaoId"),
                        PontoRanking = (double)reader.GetValueOrDefault<decimal>("PontoRanking"),
                        PontoFortuna = (double)reader.GetValueOrDefault<decimal>("PontoFortuna"),
                        JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                        Jogador = new Jogador
                        {
                            JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                            Nome = reader.GetValueOrDefault<string>("Nome")
                        },
                        PerguntaRespostaJogadorId = reader.GetValueOrDefault<int>("PerguntaRespostaJogadorId"),
                        PerguntaRespostaJogador = new PerguntaRespostaJogador
                        {
                            PerguntaRespostaJogadorId = reader.GetValueOrDefault<int>("PerguntaRespostaJogadorId"),
                            Resposta = reader.GetValueOrDefault<string>("Resposta"),
                            PerguntaRespostaId = reader.GetValueOrDefault<int>("PerguntaRespostaId"),
                            PerguntaResposta = new PerguntaResposta
                            {
                                PerguntaRespostaId = reader.GetValueOrDefault<int>("PerguntaRespostaId"),
                                QuantidadePonto = reader.GetValueOrDefault<decimal>("QuantidadePonto"),
                                Ordem = reader.IsDBNull(reader.GetOrdinal("Ordem")) ? (int?)null : reader.GetValueOrDefault<int>("Ordem")
                            },
                            PerguntaId = reader.GetValueOrDefault<int>("PerguntaId"),
                            Pergunta = new Pergunta
                            {
                                PerguntaId = reader.GetValueOrDefault<int>("PerguntaId"),
                                Descricao = reader.GetValueOrDefault<string>("Descricao")
                            }
                        }
                    });
                }
            }

            return ranking;
        }
    }
}
