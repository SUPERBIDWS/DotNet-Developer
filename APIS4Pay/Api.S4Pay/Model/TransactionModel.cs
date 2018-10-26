using System;
using System.ComponentModel.DataAnnotations;

namespace Api.S4Pay.Model
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid AnotherUserId { get; set; }
        [Required]
        public decimal Value { get; set; }
    }
}