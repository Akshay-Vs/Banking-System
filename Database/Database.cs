using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Database
{
    class Database
    {
        public static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("BankingSystem");
            IMongoCollection<User> collection = database.GetCollection<User>("User");
            Console.WriteLine("Database is online");

            Credentials credentials = new Credentials
            {
                Name = "UserName",
                Age = 18,
                Password = "User#pass123",
                CountryCode = 91,
                Phone = 7032902781,
                RegistrationDate = DateTime.Now.ToString(),
                AccountType = "Savings"
            };

            Access access = new Access
            {
                AccessType = "endUser",
                Pin = "746708"
            };

            Balance balance = new Balance
            {
                Savings = 1200,
                ECoin = 8000,
                Credit = 130 
            };

            Transactions transaction1 = new Transactions
            {
                Date = DateTime.Now,
                Recipient = "128493",
                IsEcoin = true,
                Amount = 1000,
                Notes = "Test transaction"
            };

            Transactions transaction2 = new Transactions
            {
                Date = DateTime.Now,
                Recipient = "654397",
                IsEcoin = false,
                Amount = 1000,
                Notes = "Test transaction"
            };

            User user = new User
            {
                Credentials = credentials,
                Access = access,
                Balance = balance,
                Transactions = new List<Transactions>
                {
                    transaction1,
                    transaction2
                }
            };
            collection.InsertOne(user);

            Console.WriteLine("User added to the database.");
        }

    }
}
