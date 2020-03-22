using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MediatorDemo
{
    public class ConcreteMediator : Mediator
    {
        //public Colleague _colleague1;
        //public Colleague _colleague2;

        private List<Colleague> _colleagues = new List<Colleague>();

        public void Register(Colleague colleague)
        {
            colleague.SetMediator(this);
            _colleagues.Add(colleague);
        }

        public T CreateColleague<T>() where T : Colleague, new()
        {
            var colleague = new T();
            colleague.SetMediator(this);
            _colleagues.Add(colleague);
            return colleague;
        }

        public override void Send(string message, Colleague colleague)
        {
            //switch (colleague)
            //{
            //    case ConcreteColleague1 _:
            //    {
            //        _colleague2.HandleNotification(message);
            //        return;
            //    }
            //    case ConcreteColleague2 _:
            //    {
            //        _colleague1.HandleNotification(message);
            //        return;
            //    }
            //    default:
            //    {
            //        return;
            //    }
            //}
            _colleagues.Where(c => c != colleague).ToList().ForEach(c => c.HandleNotification(message));
        }
    }
}