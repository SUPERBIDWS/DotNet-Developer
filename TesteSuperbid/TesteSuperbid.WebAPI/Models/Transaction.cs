using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TesteSuperbid.WebAPI.Models
{
    public class Transaction
    {
        private int id;

        [Key]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private Account accountFrom;

        public Account AccountFrom
        {
            get { return accountFrom; }
            set { accountFrom = value; }
        }

        private Account accountTo;

        public Account AccountTo
        {
            get { return accountTo; }
            set { accountTo = value; }
        }

        private double ammount;

        public double Ammount
        {
            get { return ammount; }
            set { ammount = value; }
        }

        private bool pending;

        public bool Pending
        {
            get { return pending; }
            set { pending = value; }
        }
    }
}
