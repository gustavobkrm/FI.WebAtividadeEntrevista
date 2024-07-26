using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using System.Reflection;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            try
            {

                BoCliente bo = new BoCliente();

                if (!this.ModelState.IsValid)
                {
                    List<string> erros = (from item in ModelState.Values
                                          from error in item.Errors
                                          select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
                else
                {
                    var beneficiarios = string.IsNullOrEmpty(model.Beneficiarios) ? new List<BeneficiarioModel>() : JsonConvert.DeserializeObject<List<BeneficiarioModel>>(model.Beneficiarios) ?? new List<BeneficiarioModel>();

                    model.Id = bo.Incluir(new Cliente()
                    {
                        CEP = model.CEP,
                        Cidade = model.Cidade,
                        Email = model.Email,
                        Estado = model.Estado,
                        Logradouro = model.Logradouro,
                        Nacionalidade = model.Nacionalidade,
                        Nome = model.Nome,
                        Sobrenome = model.Sobrenome,
                        Telefone = model.Telefone,
                        CPF = model.CPF,
                        Beneficiarios = beneficiarios.Select(b => new Beneficiario
                        {
                            CPF = b.CPF,
                            Nome = b.Nome
                        }).ToList()
                    });


                    return Json("Cadastro efetuado com sucesso");
                }
            }
            catch (Exception ex) {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            try
            {
                BoCliente bo = new BoCliente();

                if (!this.ModelState.IsValid)
                {
                    List<string> erros = (from item in ModelState.Values
                                          from error in item.Errors
                                          select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }
                else
                {

                    var beneficiarios = string.IsNullOrEmpty(model.Beneficiarios) ? new List<BeneficiarioModel>() : JsonConvert.DeserializeObject<List<BeneficiarioModel>>(model.Beneficiarios) ?? new List<BeneficiarioModel>();


                    bo.Alterar(new Cliente()
                    {
                        Id = model.Id,
                        CEP = model.CEP,
                        Cidade = model.Cidade,
                        Email = model.Email,
                        Estado = model.Estado,
                        Logradouro = model.Logradouro,
                        Nacionalidade = model.Nacionalidade,
                        Nome = model.Nome,
                        Sobrenome = model.Sobrenome,
                        Telefone = model.Telefone,
                        CPF = model.CPF,
                        Beneficiarios = beneficiarios.Select(b => new Beneficiario
                        {
                            Id = b.Id,
                            CPF = b.CPF,
                            Nome = b.Nome,
                            IdCliente = b.IdCliente
                        }).ToList()
                    });

                    return Json("Cadastro alterado com sucesso");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            try
            {
                BoCliente bo = new BoCliente();
                Cliente cliente = bo.Consultar(id);
                cliente.Beneficiarios = bo.GetBeneficiarios(id);
                Models.ClienteModel model = null;

                if (cliente != null)
                {
                    model = new ClienteModel()
                    {
                        Id = cliente.Id,
                        CEP = cliente.CEP,
                        Cidade = cliente.Cidade,
                        Email = cliente.Email,
                        Estado = cliente.Estado,
                        Logradouro = cliente.Logradouro,
                        Nacionalidade = cliente.Nacionalidade,
                        Nome = cliente.Nome,
                        Sobrenome = cliente.Sobrenome,
                        Telefone = cliente.Telefone,
                        CPF = cliente.CPF,
                        Beneficiarios = JsonConvert.SerializeObject(cliente.Beneficiarios)
                    };
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Excluir(long id)
        {
            try
            {
                BoCliente bo = new BoCliente();
                bo.Excluir(id);
                return View();
            }
            catch (Exception ex) {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }
    }
}