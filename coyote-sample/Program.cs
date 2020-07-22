
namespace Coytee_Sample
{
    using Microsoft.Coyote.Tasks;

    public static class Program
    {
        public static async System.Threading.Tasks.Task Main()
        {
            await Greetme();
        }

        [Microsoft.Coyote.SystematicTesting.Test]
        public static async Task Greetme()
        {
            var greeter = new GreetingMessages();
            await greeter.Greet();
        }
    }
}