using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
   public class Mapper : IMapper
    {
        public Mapper(Type klassSrc, Type klassDest)
        {

        }

        public Mapper Bind(Mapping m)
        {
            throw new NotImplementedException();
        }

        public object[] Map(object[] src)
        {
            throw new NotImplementedException();
        }

        public object Map(object src)
        {
            throw new NotImplementedException();
        }

        public Mapper Match(string nameForm, string nameDest)
        {
            throw new NotImplementedException();
        }
    }




}
