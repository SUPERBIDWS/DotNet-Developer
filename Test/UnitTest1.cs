using System;
using Infraestrutura;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var da = new DataAccess();
            var kk = da.CarregarContas();
        }

        [TestMethod]
        public void testMethod2()
        {
            var kk = new OperadorTransacao();
            kk.SelecionarConta(Entidades.TipoConta.Destino,"X");
            
           //kk.sinconizarDados();
        }
    }
}
