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
        
    }
}
