using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public class Teacher
    {
       
        public Teacher(int nr, string name)
        {
            this.name = name;
            Id = nr;
        }

        public Teacher(string name)
        {
            this.name = name;
        }

        public int field;
        public string name { get; set; }
        public int Id { get; set; }

        
    }
}
