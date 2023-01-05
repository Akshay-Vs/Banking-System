using System;
using McMaster.Extensions.CommandLineUtils;
using MongoDB.Driver;
using MongoDB.Bson;
using Global.IO;
using OperatorConsole.TCPServer;

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
                var typeArgument = newCommand.Argument("TYPE", "The type of the User");
                var nameOption = newCommand.Option("-n|--name <NAME>", "The name of the User", CommandOptionType.SingleValue);
                var idOption = newCommand.Option("-i|--id <ID>", "The ID of the User", CommandOptionType.SingleValue);
                var forceOption = newCommand.Option("-f|--force", "Force the update", CommandOptionType.NoValue);
                newCommand.HelpOption("-?|-h|--help");

                newCommand.OnExecute(() =>
                {
                    //CreateUser();
                    IO.WriteLine(forceOption.Value());
                    IO.WriteLine(idOption.Value());
                    IO.WriteLine(nameOption.Value());
                    IO.WriteLine(typeArgument.Value);
                    
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
                        IO.WriteLine(response);
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
                    TCPOps.Close();
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

