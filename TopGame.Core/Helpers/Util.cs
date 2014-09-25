using System;
using System.Globalization;
using TopGame.Core.Extensions;

namespace TopGame.Core.Helpers
{
    public static class Util
    {
        /// <summary>
        /// Validação de CPF
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns>Retorna se valor informado é válido</returns>
        public static bool ValidaCpf(string cpf)
        {
            cpf = cpf.RemoveNaoNumericos();

            if (cpf.Length != 11)
                return false;

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "01234567890")
                return false;

            var numeros = new int[11];
            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString(CultureInfo.InvariantCulture));

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
                return numeros[10] == 0;

            return numeros[10] == 11 - resultado;
        }
    }
}