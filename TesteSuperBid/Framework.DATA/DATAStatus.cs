using FrameWork.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.DATA
{
    public class DATAStatus : Context<TB_STATUS, VOStatus>
    {
        public bool Excluir(VOStatus obj)
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

        public VOStatus Inserir(VOStatus obj)
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

        public VOStatus Atualizar(VOStatus obj)
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

        public IList<VOStatus> BuscarTodos()
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

        public IList<VOStatus> BuscarTodosFuncao(Func<TB_STATUS, bool> predicate)
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

        public VOStatus BuscarPorID(params object[] ID)
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