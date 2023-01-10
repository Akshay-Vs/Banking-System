using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MongoDB.Driver;
using Global.Types;

namespace Global.Database
{
    public class DataBase
    {
        private readonly IMongoCollection<User> collection;
        private readonly IMongoDatabase database;
        private readonly MongoClient client;

        public DataBase(string connectionString, string _database, string _collection)
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(_database);
            collection = database.GetCollection<User>(_collection);
        }

        public string Insert(string query)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(query);
            User user = new User
            {
                Credentials = new Credentials
                {
                    Name = json.name,
                    Age = json.age,
                    Email = json.email,
                    Password = json.password,
                    CountryCode = json.cCcode,
                    Phone = json.phone,
                    AccountType = json.type,
                    RegistrationDate = DateTime.Now
                },
                Access = new Access
                {
                    Pin = json.pin
                },
                Balance = new Balance
                {
                    Savings = 0.00,
                    ECoin = 0.00,
                    Credit = 0.00
                },
            };
            
            try
            {
                collection.InsertOne(user);
                return $"Created {json}";
            }

            catch (Exception e) { return e.Message; }
        }
    }
}
