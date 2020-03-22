using System;
using System.Threading.Tasks;

namespace MediatorDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.ReadKey();
        }

        //  original example code prior to ChatApp restructuring
        private static void StructuralExample()
        {
            var mediator = new ConcreteMediator();

            //var c1 = new ConcreteColleague1(mediator);
            //var c2 = new ConcreteColleague2(mediator);

            //mediator._colleague1 = c1;
            //mediator._colleague2 = c2;

            //var c1 = new ConcreteColleague1();
            //var c2 = new ConcreteColleague2();
            //mediator.Register(c1);
            //mediator.Register(c2);

            var c1 = mediator.CreateColleague<ConcreteColleague1>();
            var c2 = mediator.CreateColleague<ConcreteColleague2>();

            c1.Send("From C1");
            c2.Send("From C2");
        }
    }
}
