using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesteSuperbid.ConsoleApplication
{
    public class Account
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private double balance;

        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }
    }
}
