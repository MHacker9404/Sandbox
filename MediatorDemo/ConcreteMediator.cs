using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediatorDemo
{
    public class ConcreteMediator : Mediator
    {
        public Colleague _colleague1;
        public Colleague _colleague2;

        public override void Send(string message, Colleague colleague)
        {
            switch (colleague)
            {
                case ConcreteColleague1 _:
                {
                    _colleague2.HandleNotification(message);
                    return;
                }
                case ConcreteColleague2 _:
                {
                    _colleague1.HandleNotification(message);
                    return;
                }
                default:
                {
                    return;
                }
            }
        }
    }
}