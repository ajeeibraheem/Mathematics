using System;
using BusinessEntities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Mathematics
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped<IBodmasOperationInterface, BodmasOperation>()
                .AddScoped<IReadExpression, ReadExpression>()
                .AddScoped<IOrderOfExecution, OrderOfExecution>()
                .BuildServiceProvider();

            var work = serviceProvider.GetService<IBodmasOperationInterface>();
            Console.WriteLine("Please enter the expression:");
            var operation = Console.ReadLine();
            var output = work.ExecuteExpression(operation);
            Console.WriteLine("Result:{0}", output);

        }
    }
}
