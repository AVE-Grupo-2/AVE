using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    class Info
    {

        public static void Main(string[] args)
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field=200 };
            Person p = (Person)m.Map(s);

            Console.WriteLine(p.Name+"+"+ p.Id+"+"+p.getfield());
            Console.WriteLine("TESTE1");

        }

    }
}
