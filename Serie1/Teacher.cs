using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public class Teacher
    {
        public Teacher()
        {

        }

        Teacher(int nr, string name)
        {
            Name = name;
            Id = nr;
        }

        public int field;
        public string Name { get; set; }
        public int Id { get; set; }

        
    }
}
