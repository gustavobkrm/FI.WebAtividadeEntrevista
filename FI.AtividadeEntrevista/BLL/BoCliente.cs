using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            cliente.CPF = new string(cliente.CPF.Where(char.IsDigit).ToArray()).Trim();

            if (VerificarExistencia(cliente.CPF))
                throw new Exception("CPF Digitado já está cadastrado");

            if (!IsValidCPF(cliente.CPF))
                throw new Exception("CPF Digitado é inválido");


            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            cliente.CPF = new string(cliente.CPF.Where(char.IsDigit).ToArray()).Trim();

            if (VerificarExistencia(cliente.CPF))
                throw new Exception("CPF Digitado já está cadastrado");

            if (!IsValidCPF(cliente.CPF))
                throw new Exception("CPF Digitado é inválido");

            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }

        private static bool IsValidCPF (string CPF)
        {
            if (string.IsNullOrWhiteSpace(CPF))
                return false;

            if (CPF.Length != 11)
                return false;

            
            return CPF.EndsWith(VerificaDigitoCPF(CPF));
        }

        private static string VerificaDigitoCPF(string CPF)
        {
            string cpfAuxiliar = CPF.Substring(0, 9);
            int soma = 0;
            int[] calculoPrimeiroDigito = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] calculoSegundoDigito = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpfAuxiliar[i].ToString()) * calculoPrimeiroDigito[i];

            int resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();

            cpfAuxiliar = cpfAuxiliar + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpfAuxiliar[i].ToString()) * calculoSegundoDigito[i];

            resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;


            return digito + resto.ToString();
        }
    }
}
