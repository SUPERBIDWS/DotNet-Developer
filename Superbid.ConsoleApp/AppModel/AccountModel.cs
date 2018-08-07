using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Superbid.Domain.DomainModels;

namespace Superbid.ConsoleApp.AppModel
{
    public class AccountModel
    {
        public string Id { get; set; }
        public decimal TotalBalance { get; set; }
        public DateTime CreationDate { get; private set; }
    }
}