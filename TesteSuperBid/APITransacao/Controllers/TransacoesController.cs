using Framework.DATA;
using FrameWork.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace APITransacao.Controllers
{
    public class TransacoesController : ApiController
    {
        DATAConta dbConta = new DATAConta();
        DATATransacao dbTransacao = new DATATransacao();

        [HttpGet]
        [Route("Api/Transacoes/GetConta/{id}")]
        public JsonResult<VOConta> GetConta(int id)
        {
            try
            {
                VOConta conta = dbConta.BuscarPorID(id);

                return Json(conta);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        [Route("Api/Transacoes/VerificaSaldo/{id}/{valor}")]
        public JsonResult<bool> VerificaSaldo(int id, string valor)
        {
            try
            {
                bool? retorno = dbConta.VerificaSaldo(id, double.Parse(valor));

                if (retorno != null)
                {
                    return Json(retorno.Value);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Um erro ocorreu ao verificar o saldo."));
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        [Route("Api/Transacoes/GetContas")]
        public JsonResult<List<VOConta>> GetContas()
        {
            try
            {
                List<VOConta> contas = dbConta.BuscarTodos().ToList();

                return Json(contas);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        [Route("Api/Transacoes/GetTransacoes")]
        public JsonResult<List<VOTransacao>> GetTransacoes()
        {
            try
            {
                List<VOTransacao> transacoes = dbTransacao.BuscarTodos().ToList();

                return Json(transacoes);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        [Route("Api/Transacoes/GetTransacoesPendentes")]
        public JsonResult<List<VOTransacao>> GetTransacoesPendentes()
        {
            try
            {
                List<VOTransacao> transacoes = dbTransacao.BuscarTodosFuncao(x => x.IDE_STATUS == 1).ToList();

                return Json(transacoes);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("Api/Transacoes/PostAdicionarTransacao")]
        public JsonResult<VOTransacao> PostAdicionarTransacao(VOTransacao transacao)
        {
            try
            {
                VOTransacao novo = dbTransacao.Inserir(transacao);

                dbConta.RemoverSaldo(novo.IDE_CONTA_ORIGEM, novo.VALOR);

                return Json(novo);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("Api/Transacoes/PostAlterarTransacao")]
        public JsonResult<VOTransacao> PostAlterarTransacao(VOTransacao transacao)
        {
            try
            {
                transacao = dbTransacao.Atualizar(transacao.ID_TRANSACAO, transacao.IDE_STATUS);

                if (transacao.IDE_STATUS == 2)
                {
                    dbConta.AdicionarSaldo(transacao.IDE_CONTA_DESTINO, transacao.VALOR);
                }
                else
                {
                    if (transacao.IDE_STATUS == 3)
                    {
                        dbConta.AdicionarSaldo(transacao.IDE_CONTA_ORIGEM, transacao.VALOR);
                    }
                }

                return Json(transacao);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
