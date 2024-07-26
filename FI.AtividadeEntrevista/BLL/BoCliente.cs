using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

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
            try
            {
                cliente.CPF = new string(cliente.CPF.Where(char.IsDigit).ToArray()).Trim();
                foreach (Beneficiario b in cliente.Beneficiarios)
                {
                    b.CPF = new string(b.CPF.Where(char.IsDigit).ToArray()).Trim();
                }

                ValidaCPF(cliente.CPF);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    DAL.DaoCliente cli = new DAL.DaoCliente();
                    var idCliente = cli.Incluir(cliente);

                    IncluirBeneficiarios(cliente.Beneficiarios, idCliente, cli);

                    transactionScope.Complete();

                    return idCliente;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            try
            {
                cliente.CPF = new string(cliente.CPF.Where(char.IsDigit).ToArray()).Trim();
                foreach (Beneficiario b in cliente.Beneficiarios)
                {
                    b.CPF = new string(b.CPF.Where(char.IsDigit).ToArray()).Trim();
                }

                ValidaCPF(cliente.CPF);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    DAL.DaoCliente cli = new DAL.DaoCliente();
                    cli.Alterar(cliente);

                    AlterarBeneficiarios(cliente.Beneficiarios, cli);

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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

        public List<DML.Beneficiario> GetBeneficiarios(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.GetBeneficiarios(id);
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

        private void IncluirBeneficiarios(List<Beneficiario> beneficiarios, long idCliente, DAL.DaoCliente cli)
        {
            foreach (var beneficiario in beneficiarios)
            {
                beneficiario.IdCliente = idCliente;

                ValidaCPF(beneficiario);

                cli.IncluirBeneficiario(beneficiario);
            }
        }

        private void AlterarBeneficiarios(List<Beneficiario> beneficiarios, DAL.DaoCliente cli)
        {
            foreach (var beneficiario in beneficiarios)
            {
                ValidaCPF(beneficiario);

                cli.AtualizaBeneficiario(beneficiario);
            }
        }
        private void ValidaCPF(string CPF)
        {
            if (VerificarExistencia(CPF))
                throw new Exception("CPF Digitado já está cadastrado");

            if (!IsValidCPF(CPF))
                throw new Exception("CPF Digitado é inválido");
        }
        private void ValidaCPF(Beneficiario b)
        {

            if (VerificarExistencia(b.CPF, b.IdCliente))
                throw new Exception("CPF do beneficiario " + b.Nome + " já está cadastrado para esse cliente.");

            if (!IsValidCPF(b.CPF))
                throw new Exception("CPF do beneficiario " + b.Nome + " é inválido.");
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

        public bool VerificarExistencia(string CPF, long idCliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF, idCliente);
        }

        private bool IsValidCPF (string CPF)
        {
            if (string.IsNullOrWhiteSpace(CPF))
                return false;

            if (CPF.Length != 11)
                return false;

            
            return CPF.EndsWith(VerificaDigitoCPF(CPF));
        }


        private string VerificaDigitoCPF(string CPF)
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
