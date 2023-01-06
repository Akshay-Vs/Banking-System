using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Access")]
        public Access Access { get; set; }

        [BsonElement("Credentials")]
        public Credentials Credentials { get; set; }

        [BsonElement("Balance")]
        public Balance Balance { get; set; }

        [BsonElement("Transactions")]
        public List<Transactions> Transactions { get; set; }

    }

    public class Credentials
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Phone")]
        public decimal Phone { get; set; }

        [BsonElement("CountryCode")]
        public int CountryCode { get; set; }

        [BsonElement("Age")]
        public int Age { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("AccountType")]
        public string AccountType { get; set; }

        [BsonElement("RegistrationDate")]
        public string RegistrationDate { get; set; }
    }

    public class Access
    {
        [BsonElement("AccessType")]
        public string AccessType { get; set; }

        [BsonElement]
        public string Pin { get; set; }
    }

    public class Balance
    {
        [BsonElement("Savings")]
        public decimal Savings { get; set; }

        [BsonElement("ECoin")]
        public decimal ECoin { get; set; }

        [BsonElement("Credit")]
        public decimal Credit { get; set; }
    }

    public class Transactions
    {
        [BsonElement("Date")]
        public BsonDateTime Date { get; set; }

        [BsonElement("IsEcoin")]
        public bool IsEcoin { get; set; }

        [BsonElement("Amount")]
        public decimal Amount { get; set; }

        [BsonElement("Recipient")]
        public string Recipient { get; set; }

        [BsonElement("Notes")]
        public string Notes { get; set; }

    }
}
