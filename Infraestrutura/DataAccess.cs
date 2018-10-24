using Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public class DataAccess
    {
        /// <summary>
        /// Carrega a fonte de dados na memória
        /// </summary>
        /// <returns></returns>
        public List<Conta> CarregarContas()
        {
            List<Conta> Contas;
            //Navegação relativa apenas para rodar em qualquer diretório.
            using (StreamReader r = new StreamReader(Path.GetFullPath(@"../../../Infraestrutura\Dados\Fonte.js")))
            {
                var json = r.ReadToEnd();
                Contas = JsonConvert.DeserializeObject<List<Conta>>(json);
            }
            return Contas;
        }

        /// <summary>
        /// Atualiza a operação na fonte de dados
        /// </summary>
        /// <param name="Contas">Coleção de dados atualizada.</param>
        public void AtualizarContas(List<Conta> Contas)
        {
            using (StreamWriter w = new StreamWriter(Path.GetFullPath(@"../../../Infraestrutura\Dados\Fonte.js"), false))
            {
                var json = JsonConvert.SerializeObject(Contas);
                w.Write(json);
            }
        }

    }
}
