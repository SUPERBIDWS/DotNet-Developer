using FrameWork.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.DATA
{
    public class DATATransacao : Context<TB_TRANSACOES, VOTransacao>
    {
        public bool Excluir(VOTransacao obj)
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

        public VOTransacao Inserir(VOTransacao obj)
        {
            try
            {
                obj.IDE_STATUS = 1;
                obj.DATA_TRANSACAO = DateTime.Now;

                return Insert(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VOTransacao Atualizar(int idTransacao, int idStatus)
        {
            try
            {
                VOTransacao transacao = BuscarPorID(idTransacao);

                transacao.IDE_STATUS = idStatus;
                transacao.DATA_RESULTADO = DateTime.Now;

                return Update(transacao);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<VOTransacao> BuscarTodos()
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

        public IList<VOTransacao> BuscarTodosFuncao(Func<TB_TRANSACOES, bool> predicate)
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

        public VOTransacao BuscarPorID(params object[] ID)
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
    }
}