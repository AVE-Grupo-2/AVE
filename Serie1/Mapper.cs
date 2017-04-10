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
            SelectEqualsFieldsAndProps();

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
           
            Object objDest = Activator.CreateInstance(klassDest);

            setProperties(src, objDest);

            setFields(src,objDest);
           
            return objDest;
        }

      

       public Mapper Match(string nameForm, string nameDest)
        {
            matcher.Add(nameForm,nameDest);
            return null;
        }


        private  void SelectEqualsFieldsAndProps()
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

         private void setProperties(object src, object objDest)
       {
            PropertyInfo[] piSrc = src.GetType().GetProperties();
            PropertyInfo pi;
            for (int i = 0; i < piSrc.Length; ++i)
            {

                if (matcher.TryGetValue(piSrc[i].Name, out value))
                {
                    pi = klassDest.GetProperty(value);
                    pi.SetValue(objDest, piSrc[i].GetValue(src));
                }

            }
        }

        private void setFields(object src, object objDest)
        {
            FieldInfo[] fiSrc = src.GetType().GetFields();
            FieldInfo fi;
            for (int i = 0; i < fiSrc.Length; ++i)
            {

                if (matcher.TryGetValue(fiSrc[i].Name, out value))
                {
                    fi = klassDest.GetField(value);
                    fi.SetValue(objDest, fiSrc[i].GetValue(src));
                }

            }
        }
    }




}
