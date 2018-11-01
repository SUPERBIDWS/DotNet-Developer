using FrameWork.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Framework.DATA
{
    public class DATAConta : Context<TB_CONTAS, VOConta>
    {
        public bool Excluir(VOConta obj)
        {
            try
            {
                Delete(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public VOConta Inserir(VOConta obj)
        {
            try
            {
                return Insert(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VOConta Atualizar(VOConta obj)
        {
            try
            {
                return Update(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<VOConta> BuscarTodos()
        {
            try
            {
                return GetAll();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<VOConta> BuscarTodosFuncao(Func<TB_CONTAS, bool> predicate)
        {
            try
            {
                return GetAllFunction(predicate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VOConta BuscarPorID(params object[] ID)
        {
            try
            {
                return Find(ID);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool? VerificaSaldo(int id, double valor)
        {
            try
            {
                if (Find(id).SALDO_CONTA > valor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public VOConta AdicionarSaldo(int id, double valor)
        {
            try
            {
                VOConta conta = BuscarPorID(id);
                conta.SALDO_CONTA += valor;

                return Update(conta);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VOConta RemoverSaldo(int id, double valor)
        {
            try
            {
                VOConta conta = BuscarPorID(id);
                conta.SALDO_CONTA -= valor;

                return Update(conta);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}