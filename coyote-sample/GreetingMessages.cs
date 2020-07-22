namespace Coytee_Sample
{
    using System;
    using Microsoft.Coyote.Specifications;
    using Microsoft.Coyote.Tasks;

    public class GreetingMessages
    {
        private const string HelloWorld = "Hello World!";
        private const string GoodMorning = "Good Morning";

        private string Value;

        private async Task SetValue(string value)
        {
            await Task.Delay(100);
            this.Value = value;
        }

        public async Task Greet()
        {
            Task task2 = this.SetValue("v1");
            Task task3 = this.SetValue("v1");
            Task task1 = this.SetValue("v2");
            Task task4 = this.SetValue("v1");
            Task task5 = this.SetValue("v1");

            await Task.WhenAll(task1, task2, task3, task4, task5);

            Console.WriteLine(this.Value);

            Specification.Assert(this.Value == HelloWorld, $"Value is '{this.Value}' instead of '{HelloWorld}'.");
        }
    }
}