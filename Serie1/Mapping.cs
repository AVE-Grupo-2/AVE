using System;

namespace Serie1
{
    public class Mapping
    {
        public static Mapping Fields { get { return new MappingFields(); } }
        public static Mapping Properties { get { return new MappingProperties(); } }

        public Type CustomAtrib { get; set; }

        public Mapping() { }
        public Mapping(Type t)
        {

            this.CustomAtrib = t;
        }

        public virtual string metodo()
        {
            return "atri";
        }

        public class MappingFields : Mapping
        {
            static MappingFields()
            { }

            public override string metodo()
            {
                return "fields";
            }
        }
    }

    public class MappingProperties : Mapping
    {
        static MappingProperties()
        { }

        public override string metodo()
        {
            return "Properties";
        }
    }
}
