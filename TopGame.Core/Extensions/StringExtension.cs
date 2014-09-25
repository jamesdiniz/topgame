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
        /// <param name="value">Valor a ser formatado</param>
        /// <returns>Retorna entrada criptografada</returns>
        public static string ToHashed(this string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value.ToLowerInvariant());

            return BitConverter
                .ToString(new SHA1Managed().ComputeHash(bytes))
                .Replace("-", "")
                .ToLowerInvariant();
        }

        /// <summary>
        /// Criar um token para o jogador no formato SHA1
        /// </summary>
        /// <param name="documento">Número do documento do jogador</param>
        /// <param name="data">Data de cadastro do jogador</param>
        /// <returns>Retorna o token do jogador</returns>
        public static string ToToken(this string documento, DateTime data)
        {
            return String.Concat(data.Date.Ticks.ToString("x2"), documento.RemoveNaoNumericos()).ToHashed();
        }

        /// <summary>
        /// Limpa caracteres qua não sejam numéricos
        /// </summary>
        /// <param name="value">Valor a ser formatado</param>
        /// <returns>String sem caracteres</returns>
        public static string RemoveNaoNumericos(this string value)
        {
            return Regex.Replace(value, @"\D+", String.Empty);
        }
    }
}