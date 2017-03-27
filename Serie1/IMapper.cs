using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public interface IMapper
    {
        Object Map(Object src);
        Object[] Map(Object[] src);
        Mapper Bind(Mapping m);
        Mapper Match(string nameForm, string nameDest);
    }

}
