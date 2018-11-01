using FrameWork.VO;
using Newtonsoft.Json;
using SistemaTransacao.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace SistemaTransacao.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                VMTransacoes vmRetorno = new VMTransacoes();

                vmRetorno.ContaOrigem = BuscarConta(1);
                vmRetorno.ContaDestino = BuscarConta(2);
                vmRetorno.Contas = BuscarContas();
                vmRetorno.Transacoes = BuscarTransacoes();

                return View(vmRetorno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EnviarTransacao(VMTransacoes vmRetorno)
        {
            try
            {
                if (vmRetorno.Transacao.IDE_CONTA_ORIGEM != vmRetorno.Transacao.IDE_CONTA_DESTINO)
                {
                    double val = 0;

                    double.TryParse(vmRetorno.VALOR.Replace("R$ ", ""), out val);

                    vmRetorno.Transacao.VALOR = val;

                    if (vmRetorno.Transacao.VALOR > 0)
                    {
                        if (VerificaSaldo(vmRetorno.Transacao.IDE_CONTA_ORIGEM, vmRetorno.Transacao.VALOR))
                        {
                            var transacao = sendTransacao(vmRetorno.Transacao);

                            if (transacao != null)
                            {
                                vmRetorno.MensagemRetorno = "Transação realizada com sucesso. Aguarando aprovação.";
                                vmRetorno.boolRetorno = true;
                            }
                            else
                            {
                                vmRetorno.MensagemRetorno = "Um erro ocorreu ao realizar a transação.";
                                vmRetorno.boolRetorno = false;
                            }
                        }
                        else
                        {
                            vmRetorno.MensagemRetorno = "Saldo insuficiente.";
                            vmRetorno.boolRetorno = false;
                        }
                    }
                    else
                    {
                        vmRetorno.MensagemRetorno = "Favor enserir um valor maior que zero(0).";
                        vmRetorno.boolRetorno = false;
                    }
                }
                else
                {
                    vmRetorno.MensagemRetorno = "Favor escolher contas diferentes.";
                    vmRetorno.boolRetorno = false;
                }

                vmRetorno.ContaOrigem = BuscarConta(1);
                vmRetorno.ContaDestino = BuscarConta(2);
                vmRetorno.Contas = BuscarContas();
                vmRetorno.Transacoes = BuscarTransacoes();

                return View("Index", vmRetorno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VOConta BuscarConta(int id)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/GetConta/" + id);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<VOConta>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                return new VOConta();
            }
        }

        public bool VerificaSaldo(int id, double valor)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/VerificaSaldo/" + id + "/" + valor);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<bool>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VOConta> BuscarContas()
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/GetContas");
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<List<VOConta>>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                return new List<VOConta>();
            }
        }

        public List<VOTransacao> BuscarTransacoes()
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/GetTransacoes");
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<List<VOTransacao>>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                return new List<VOTransacao>();
            }
        }

        public VOTransacao sendTransacao(VOTransacao transacao)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://localhost:57438/Api/Transacoes/PostAdicionarTransacao/");
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(transacao);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                WebResponse response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<VOTransacao>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}