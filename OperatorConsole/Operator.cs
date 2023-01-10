using System;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using McMaster.Extensions.CommandLineUtils;
using Global.IO;

namespace Operator
{
    [Command(Description = "MyBank")]
    [HelpOption]

    class Operator
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            bool login = false;
            TCPOps.ip = "127.0.0.1";
            TCPOps.port = 8080;

            app.HelpOption("-?|-h|--help");
            app.Command("new", newCommand =>
            {
                newCommand.Description = "Create a new user";
                var forceOption = newCommand.Option("-f|--force", "Force the update", CommandOptionType.NoValue);

                newCommand.HelpOption("-?|-h|--help");

                newCommand.OnExecute(() =>
                {
                    
                    string name = IO.Input("Enter your name: ");
                    string age = IO.Input("Enter Age: ");
                    string password = IO.Input("Enter Password: ");
                    string phone = IO.Input("Enter Phone Number: ");
                    string cCode = IO.Input("Enter country code: ");
                    string email = IO.Input("Enter Email address: ");
                    string type = IO.Input("Enter Account Type: ");
                    string pin = IO.Input("Enter a 6 digit pin: ");

                    bool isNumeric = Regex.IsMatch(pin, @"^\d+$");
                    while (pin.Length != 6 && !isNumeric)
                    {
                        IO.WriteError("Invalid PIN", null);
                        pin = IO.Input("Enter a 6 digit pin: ");
                    }

                    Dictionary<string, string> data = new Dictionary<string, string>()
                    {
                        { "id", "new" },
                        { "name", name },
                        { "age", age },
                        { "password", password },
                        { "phone", phone },
                        { "cCode", cCode },
                        { "email", email },
                        { "type", type },
                        { "pin", pin }
                    };

                    string json = JsonConvert.SerializeObject(data);

                    string response = TCPOps.New(json);
                    IO.WriteLine(response);
                    return 0;
                });
            });

            app.Command("update", updateCommand =>
            {
                updateCommand.Description = "Update an existing user";
                var typeOption = updateCommand.Option("-t|--type <TYPE>", "The type of the User", CommandOptionType.SingleValue);
                var idOption = updateCommand.Option("-i|--id <ID>", "The ID of the User", CommandOptionType.SingleValue);
                var nameOption = updateCommand.Option("-n|--name <NAME>", "The new name of the item", CommandOptionType.SingleValue);
                var forceOption = updateCommand.Option("-f|--force", "Force the update", CommandOptionType.NoValue);
                updateCommand.HelpOption("-?|-h|--help");

                updateCommand.OnExecute(() =>
                {
                    IO.WriteLine(forceOption.Value());
                    IO.WriteLine(idOption.Value());
                    IO.WriteLine(nameOption.Value());
                    IO.WriteLine(typeOption.Value());
                    return 0;
                });
            });

            app.Command("login", loginCommand =>
            {
                loginCommand.Description = "Login to the network";
                var nameArgument = loginCommand.Argument("NAME", "Username to login");
                var passwordArgument = loginCommand.Argument("PASSWORD", "Password to login");
                loginCommand.HelpOption("-?|-h|--help");

                loginCommand.OnExecute(() =>
                {
                    if (!login)
                    {
                        string response = TCPOps.Login(nameArgument.Value, passwordArgument.Value);
                        Console.WriteLine(response);
                        login = true;
                    }
                    else IO.WriteLine("You are already logged in. Logout to create new instance");
                    return 0;
                });
            });

            app.Command("logout", logoutCommand =>
            {
                logoutCommand.Description = "Logout from the network";
                logoutCommand.HelpOption("-?|-h|--help");

                logoutCommand.OnExecute(() =>
                {
                    login = false;
                    app.Dispose();
                    var path = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(path);
                    TCPOps.Close();
                    Environment.Exit(0);
                });
            });

            app.Command("send", sendCommand =>
            {
                sendCommand.Description = "Send ECoin to another user";
                var amountArgument = sendCommand.Argument("AMOUNT", "The amount of ECoin to send");
                var currencyArgument = sendCommand.Argument("CURRENCY", "The currency to use for the transaction (ECoin or Credit)");

                sendCommand.Command("to", toCommand =>
                {
                    toCommand.Description = "Specify the recipient of the ECoin";
                    var recipientArgument = toCommand.Argument("RECIPIENT", "The ID of the recipient");
                    toCommand.OnExecute(() =>
                    {
                        // Get the values of the arguments and options
                        double amount = Convert.ToDouble(amountArgument.Value);
                        string currency = currencyArgument.Value;
                        string recipient = recipientArgument.Value;

                        // Perform the transaction
                        // ...

                        IO.WriteLine($"sending {amount} {currency} to {recipient}");

                        return 0;
                    });
                });
            });




            app.OnExecute(() =>
            {

                while (true)
                {
                    IO.Write($"{TCPOps.ip}> ");
                    var input = Console.ReadLine();

                    if (input == "exit") 
                    {
                        TCPOps.Close();
                        return 0;
                    };

                    // Parse and execute the command
                    var commandArgs = input.Split(' ');
                    try { app.Execute(commandArgs); }
                    catch (Exception error) { IO.WriteError("Invalid input {0}", error.Message); }
                }
            });

            app.Execute(args);
            return 0;
        }
    }
}

