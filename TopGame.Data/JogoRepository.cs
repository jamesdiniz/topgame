using System.Collections.Generic;
using System.Data;
using EasyAdo;
using EasyAdo.Extensions;
using TopGame.Core.Domain;
using TopGame.Core.Domain.Pontuacao;
using TopGame.Core.Infrastructure;
using System;

namespace TopGame.Data
{
    public class JogoRepository : IJogoRepository
    {
        public Jogo GetByToken(string token)
        {
            const string query = 
                @"SELECT TOP 1 
                    JogoId, Titulo, TituloUrl, Descricao, Ativo, 
                    DataCriacao, DataPublicacao, DataExpiracao, Token
                  FROM Jogo WHERE Token = @token";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("token", token)).AsDataReader();
                if (reader == null || !reader.Read()) return null;

                return new Jogo
                {
                    JogoId = reader.GetValueOrDefault<int>("JogoId"),
                    Titulo = reader.GetValueOrDefault<string>("Titulo"),
                    TituloUrl = reader.GetValueOrDefault<string>("TituloUrl"),
                    Descricao = reader.GetValueOrDefault<string>("Descricao"),
                    Ativo = reader.GetValueOrDefault<bool>("Ativo"),
                    DataCriacao = reader.GetValueOrDefault<DateTime>("DataCriacao"),
                    DataPublicacao = reader.GetValueOrDefault<DateTime>("DataPublicacao"),
                    DataExpiracao = !reader.IsDBNull(reader.GetOrdinal("DataExpiracao")) ? (DateTime?)reader["DataExpiracao"] : null,
                    Token = reader.GetValueOrDefault<string>("Token")
                };
            }
        }

        public IEnumerable<Premiacao> GetPremios(int id)
        {
            const string query = @"
                SELECT PremiacaoId, Posicao, Descricao, Brinde, JogoId 
                FROM Premiacao
                WHERE JogoId = @jogoId
                ORDER BY Posicao";

            var premiacao = new List<Premiacao>();

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("jogoId", id)).AsDataReader();

                while (reader.Read())
                {
                    premiacao.Add(new Premiacao
                    {
                        PremiacaoId = reader.GetValueOrDefault<int>("PremiacaoId"),
                        Posicao = reader.GetValueOrDefault<int>("Posicao"),
                        Descricao = reader.GetValueOrDefault<string>("Descricao"),
                        Brinde = reader.GetValueOrDefault<bool>("Brinde"),
                        JogoId = reader.GetValueOrDefault<int>("JogoId")
                    });
                }
            }

            return premiacao;
        }

        public IEnumerable<PontuacaoApp> GetRanking(int id)
        {
            const string query =
                @"SELECT jogador.JogadorId, jogador.Nome, SUM(Quantidade) as Quantidade
                  FROM PontuacaoApp pontuacao
                    INNER JOIN Jogador jogador 
                        ON jogador.JogadorId = pontuacao.JogadorId
                    INNER JOIN Pergunta pergunta
                        ON pergunta.PerguntaId = pontuacao.PerguntaId
                  WHERE pergunta.JogoId = @jogoId
                  GROUP BY jogador.Nome, jogador.JogadorId
                  ORDER BY Quantidade DESC";

            var ranking = new List<PontuacaoApp>();

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("jogoId", id)).AsDataReader();

                while (reader.Read())
                {
                    ranking.Add(new PontuacaoApp
                    {
                        JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                        Jogador = new Jogador
                        {
                            JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                            Nome = reader.GetValueOrDefault<string>("Nome")
                        },
                        Quantidade = reader.GetValueOrDefault<decimal>("Quantidade")
                    });
                }
            }

            return ranking;
        }
    }
}