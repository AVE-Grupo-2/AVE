using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
   public class Mapper : IMapper
   {
       private Type klassSrc;
       private Type klassDest;
       private Dictionary<string,string> matcher;
      
        private FieldInfo[] fieldsSrc;
        private FieldInfo[] fieldsDest;

        private PropertyInfo[] propertiesSrc;
        private PropertyInfo[] propertiesDest;
       private string value;

        public Mapper(Type klassSrc, Type klassDest)
        {
            this.klassSrc = klassSrc;
            this.klassDest = klassDest;
            matcher = new Dictionary<string, string>();
            SelectEqualsMembers();

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
            // this.klassSrc = src.GetType();
            ConstructorInfo ci= klassDest.GetConstructor(null);
            Object obj= ci.Invoke(null);

            PropertyInfo[] piSrc = src.GetType().GetProperties();
         

            Console.Write(klassSrc.Name + ": ");

            for (int i = 0; i < piSrc.Length; ++i)
            {
                if (matcher.TryGetValue(piSrc[i].Name, out value))
                {
                    piSrc[i].GetValue(src);
                    obj.

                }
                    
            }
            foreach (PropertyInfo pi in piSrc)
            {
                Console.Write(pi.Name + ": " + pi.GetValue(src) + ", ");
            }

            return null;

        }

        public Mapper Match(string nameForm, string nameDest)
        {
            matcher.Add(nameForm,nameDest);
            return null;
        }


        private  void SelectEqualsMembers()
        {
            PropertyInfo[] propSrc = klassSrc.GetProperties();
            PropertyInfo[] propDest = klassDest.GetProperties();
            for (int i = 0; i < propSrc.Length; ++i)
            {
                for(int j=0; i<propDest.Length;++i)
                    if (propSrc[i].Name.Equals(propDest[j].Name) && propSrc[i].GetType()== propDest[j].GetType())
                {
                  matcher.Add(propSrc[i].Name,propDest[j].Name);
                }
         
            }
        }
    }




}
