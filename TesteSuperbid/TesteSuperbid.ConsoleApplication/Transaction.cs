using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TesteSuperbid.ConsoleApplication
{
    [DataContract]
    public class Transaction
    {
        private int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime date;
        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private Account accountFrom;
        [DataMember]
        public Account AccountFrom
        {
            get { return accountFrom; }
            set { accountFrom = value; }
        }

        private Account accountTo;
        [DataMember]
        public Account AccountTo
        {
            get { return accountTo; }
            set { accountTo = value; }
        }

        private double ammount;
        [DataMember]
        public double Ammount
        {
            get { return ammount; }
            set { ammount = value; }
        }

        private bool pending;
        [DataMember]
        public bool Pending
        {
            get { return pending; }
            set { pending = value; }
        }

        public Transaction()
        {
            accountFrom = new Account();
            accountTo = new Account();
        }
    }
}
