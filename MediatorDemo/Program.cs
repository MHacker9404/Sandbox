using System;
using System.Threading.Tasks;

namespace MediatorDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mediator = new ConcreteMediator();

            var c1 = new ConcreteColleague1(mediator);
            var c2 = new ConcreteColleague2(mediator);

            mediator._colleague1 = c1;
            mediator._colleague2 = c2;

            c1.Send("From C1");
            c2.Send("From C2");

            Console.ReadKey();
        }
    }
}
