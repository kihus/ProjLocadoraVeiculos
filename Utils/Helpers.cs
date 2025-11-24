using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Helpers
    {
        public static void PressionerEnterParaContinuar()
        {
            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
            Console.Clear();
        }
        public static decimal LerDecimal(string mensagem)
        {
            decimal valor;
            while (true)
            {
                Console.Write(mensagem);
                var input = Console.ReadLine();
                if (decimal.TryParse(input, out valor) && valor >= 0)
                {
                    return valor;
                }
                Console.WriteLine("Valor inválido, tente novamente.");
            }
        }
        public static string SolicitarNumeroDocumento(string tipoDocumento)
        {
            var tamanhoEsperado = tipoDocumento switch
            {
                "CPF" => 11,
                "CNH" => 11,
                "RG" => 9,
                _ => throw new Exception("Tipo de documento desconhecido.")
            };

            string numero;
            do
            {
                Console.WriteLine($"\nDigite o número do documento {tipoDocumento} " +
                                  $"({tamanhoEsperado} dígitos, apenas números): ");
                numero = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(numero) &&
                    numero.All(char.IsDigit) &&
                    numero.Length == tamanhoEsperado)
                {
                    return numero;
                }
                Console.WriteLine($"Número inválido! O {tipoDocumento} deve conter exatamente " +
                                  $"{tamanhoEsperado} dígitos numéricos.");
            } while (true);
        }
        public static string SolicitarTelefone()
        {
            string telefone;
            do
            {
                Console.Write("\nDigite o telefone (apenas números, com DDD): ");
                telefone = Console.ReadLine()?.Trim();

                if (
                    !string.IsNullOrWhiteSpace(telefone) &&
                    telefone.All(char.IsDigit) &&
                    (telefone.Length == 10 || telefone.Length == 11)
                )
                {
                    return telefone;
                }
                Console.WriteLine("Telefone inválido! Digite o DDD seguido do número, totalizando 10 ou 11 dígitos, exemplo: 11912345678.");
            } while (true);
        }
    }
}
