using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TopGame.Core.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Criptografa uma string no formato SHA1
        /// </summary>
        /// <param name="entrada">Entrada a ser formatada</param>
        /// <returns>Retorna entrada criptografada</returns>
        public static string ToHashed(this string entrada)
        {
            var strBytes = Encoding.UTF8.GetBytes(entrada.ToLowerInvariant());

            return BitConverter
                .ToString(new SHA1Managed().ComputeHash(strBytes))
                .Replace("-", "")
                .ToLowerInvariant();
        }

        /// <summary>
        /// Criar um token para o jogador no formato SHA1
        /// </summary>
        /// <param name="documento">Número do documento do jogador</param>
        /// <param name="dataCadastro">Data de cadastro do jogador</param>
        /// <returns>Retorna o token do jogador</returns>
        public static string ConvertToToken(string documento, DateTime dataCadastro)
        {
            return String.Concat(dataCadastro.Date.Ticks.ToString("x2"), LimpaCaracter(documento)).ToHashed();
        }

        /// <summary>
        /// Limpa caracteres qua não forem digitos
        /// </summary>
        /// <param name="entrada">Entrada a ser formatada</param>
        /// <returns>String sem caracteres</returns>
        public static string LimpaCaracter(this string entrada)
        {
            return Regex.Replace(entrada, @"\D+", String.Empty);
        }
    }
}