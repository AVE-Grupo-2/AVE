﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public class Student
    {
        public int field;
        private int privateField;
        public string Name { get; set; }
        public int Nr { get; set; }

        private int privateProperty
        {
            get; set;
        }

        public void setPrivateProperty(int i)
        {
            this.privateProperty = i;
        }

        public int getPrivateProperty()
        {
            return this.privateProperty;
        }

        public void setPrivateField(int field)
        {
            this.privateField = field;
        }

        public int getPrivateField()
        {
            return this.privateField;
        }

    }

    
}
