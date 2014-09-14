using System.Data;
using EasyAdo;
using EasyAdo.Extensions;
using TopGame.Core.Domain;
using TopGame.Core.Infrastructure;
using System;

namespace TopGame.Data
{
    public class JogadorRepository : IJogadorService
    {
        public Jogador GetById(int id)
        {
            const string query = 
                @"SELECT TOP 1 JogadorId, Nome, Email, Documento, DataCriacao 
                  FROM Jogador 
                  WHERE JogadorId = @jogadorId";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("jogadorId", id)).AsDataReader();
                if (reader == null || !reader.Read()) return null;

                return new Jogador
                {
                    JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                    Nome = reader.GetValueOrDefault<string>("Nome"),
                    Email = reader.GetValueOrDefault<string>("Email"),
                    Documento = reader.GetValueOrDefault<string>("Documento"),
                    DataCriacao = reader.GetValueOrDefault<DateTime>("DataCriacao")
                };
            }
        }

        public Jogador GetByToken(string token)
        {
            const string query = 
                @"SELECT TOP 1 
                    jogador.JogadorId, jogador.Nome, jogador.Email, jogador.Documento, jogador.DataCriacao
                  FROM Jogador jogador
                    INNER JOIN JogadorToken token ON token.JogadorId = jogador.JogadorId
                  WHERE token.Codigo = @token";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("token", token)).AsDataReader();
                if (reader == null || !reader.Read()) return null;

                return new Jogador 
                {
                    JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                    Nome = reader.GetValueOrDefault<string>("Nome"),
                    Email = reader.GetValueOrDefault<string>("Email"),
                    Documento = reader.GetValueOrDefault<string>("Documento"),
                    DataCriacao = reader.GetValueOrDefault<DateTime>("DataCriacao")
                };
            }
        }

        public Jogador GetByDocumento(string documento)
        {
            const string query = 
                @"SELECT TOP 1 JogadorId, Nome, Email, Documento, DataCriacao
                  FROM Jogador 
                  WHERE Documento = @documento";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text, data.ParameterBuilder().Add("documento", documento)).AsDataReader();
                if (reader == null || !reader.Read()) return null;

                return new Jogador
                {
                    JogadorId = reader.GetValueOrDefault<int>("JogadorId"),
                    Nome = reader.GetValueOrDefault<string>("Nome"),
                    Email = reader.GetValueOrDefault<string>("Email"),
                    Documento = reader.GetValueOrDefault<string>("Documento"),
                    DataCriacao = reader.GetValueOrDefault<DateTime>("DataCriacao")
                };
            }
        }

        public Jogador Add(Jogador jogador)
        {
            const string query = "INSERT INTO Jogador VALUES (@nome, @email, @documento, @data) SELECT SCOPE_IDENTITY()";

            using (var data = new SqlContext())
            {
                jogador.JogadorId = Convert.ToInt32(data.Execute(query, CommandType.Text,
                    data.ParameterBuilder()
                        .Add("nome", jogador.Nome)
                        .Add("email", jogador.Email)
                        .Add("documento", jogador.Documento)
                        .Add("data", jogador.DataCriacao)
                    ).AsScalar()) ;

                return jogador;
            }
        }

        public JogadorToken GetToken(int id, string token)
        {
            const string query = 
                @"SELECT TOP 1 JogadorTokenId, Codigo, DataCriacao, JogadorId 
                  FROM JogadorToken 
                  WHERE JogadorId = @jogadorId AND Codigo = @codigo";

            using (var data = new SqlContext())
            {
                var reader = data.Execute(query, CommandType.Text,
                    data.ParameterBuilder()
                        .Add("jogadorId", id)
                        .Add("codigo", token)
                ).AsDataReader();

                if (reader == null || !reader.Read()) return null;

                return new JogadorToken
                {
                    JogadorTokenId = reader.GetValueOrDefault<int>("JogadorTokenId"),
                    Codigo = reader.GetValueOrDefault<string>("Codigo"),
                    DataCriacao = reader.GetValueOrDefault<DateTime>("DataCriacao"),
                    JogadorId = reader.GetValueOrDefault<int>("JogadorId")
                };
            }
        }

        public JogadorToken CriaToken(JogadorToken jogadorToken)
        {
            const string query = "INSERT INTO JogadorToken VALUES (@codigo, @data, @jogadorId) SELECT SCOPE_IDENTITY()";

            using (var data = new SqlContext())
            {
                jogadorToken.JogadorTokenId = Convert.ToInt32(data.Execute(query, CommandType.Text,
                    data.ParameterBuilder()
                        .Add("codigo", jogadorToken.Codigo)
                        .Add("data", jogadorToken.DataCriacao)
                        .Add("jogadorId", jogadorToken.JogadorId)
                ).AsScalar());

                return jogadorToken;
            }
        }
    }
}