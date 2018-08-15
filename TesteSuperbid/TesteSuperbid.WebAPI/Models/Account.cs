using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TesteSuperbid.WebAPI.Models
{
    public class Account
    {
        private int id;
        [Key]
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
