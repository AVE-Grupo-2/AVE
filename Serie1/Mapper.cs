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

        private Dictionary<string, string> matcher;

        private FieldInfo[] fieldsSrc;
        private FieldInfo[] fieldsDest;

        private string value;

        public Mapper(Type klassSrc, Type klassDest)
        {
            this.klassSrc = klassSrc;

            this.klassDest = klassDest;
            matcher = new Dictionary<string, string>();

            SelectEqualProperties();
            selectEqualFields();
        }


        private void selectEqualFields()
        {
            fieldsSrc = klassSrc.GetFields();
            fieldsDest = klassDest.GetFields();

            for (int i = 0; i < fieldsSrc.Length; ++i)
            {
                for (int j = 0; j < fieldsDest.Length; ++j)
                    if (fieldsSrc[i].Name.Equals(fieldsDest[j].Name) && fieldsSrc[i].GetType() == fieldsDest[j].GetType())
                    {
                        matcher.Add(fieldsSrc[i].Name, fieldsDest[j].Name);
                    }
            }
        }


        public Mapper Bind(Mapping m)
        {
            matcher = new Dictionary<string, string>();
            if (m.metodo().Equals("Fields"))
            {
                selectEqualFields();
            }
            else if (m.metodo().Equals("Properties"))
            {
                SelectEqualProperties();
            }
            else
            {
                selectPropertiesCustomAttributes(m.CustomAtrib);
                selectFieldsCustomAttributes(m.CustomAtrib);
            }


            return this;
        }

        public object[] Map(object[] src)
        {
            object[] dest = new object[src.Length];
            for (int i = 0; i < src.Length; ++i)
            {
                dest[i] = this.Map(src[i]);
            }
            return dest;
        }

        public object Map(object src)
        {

            if (!klassSrc.IsInstanceOfType(src))
                return null;

            Object objDest = Activator.CreateInstance(klassDest);

            setProperties(src, objDest);

            setFields(src, objDest);

            return objDest;
        }


        public Mapper Match(string nameFrom, string nameDest)
        {

            IMapper auxMapper = AutoMapper.Build(klassSrc.GetMember(nameFrom).GetType(), klassDest.GetMember(nameDest).GetType());
            matcher.Add(nameFrom, nameDest);
            return this;
        }


        private void SelectEqualProperties()
        {
            PropertyInfo[] propSrc = klassSrc.GetProperties();
            PropertyInfo[] propDest = klassDest.GetProperties();

            for (int i = 0; i < propSrc.Length; ++i)
            {
                for (int j = 0; j < propDest.Length; ++j)
                    if (propSrc[i].Name.Equals(propDest[j].Name) && propSrc[i].GetType() == propDest[j].GetType())
                    {
                        matcher.Add(propSrc[i].Name, propDest[j].Name);
                    }
            }
        }


        private void setProperties(object src, object objDest)
        {
            PropertyInfo[] piSrc = src.GetType().GetProperties();
            PropertyInfo piDest;
            for (int i = 0; i < piSrc.Length; ++i)
            {

                if (matcher.TryGetValue(piSrc[i].Name, out value))

                {
                    piDest = klassDest.GetProperty(value);
                    if (objDest.GetType().GetProperty(value).PropertyType.IsValueType || objDest.GetType().GetProperty(value).PropertyType == typeof(string) || objDest.GetType().GetProperty(value).PropertyType.IsArray)
                    {
                        piDest.SetValue(objDest, piSrc[i].GetValue(src));
                    }
                    else
                    {
                        Type srcPropertyType = piSrc[i].PropertyType;
                        if (null != srcPropertyType.GetConstructor(new Type[0]))
                        {
                            IMapper auxMapper = AutoMapper.Build(srcPropertyType, klassDest.GetProperty(value).PropertyType);
                            klassDest.GetProperty(value).SetValue(objDest, auxMapper.Map(piSrc[i].GetValue(src)));
                        }
                        else if (piSrc[i].GetValue(src) != null)
                        {
                            PropertyInfo propertyDest = objDest.GetType().GetProperty(value);
                            Type propertyDestType = objDest.GetType().GetProperty(value).PropertyType;


                            ConstructorInfo[] constructors = propertyDestType.GetConstructors();

                            createReferenceTypeMemberWithConstructor(constructors,srcPropertyType, piDest, piSrc[i].GetValue(src), objDest);

                        }
                    }
                }
            }
        }



        public void createReferenceTypeMemberWithConstructor(ConstructorInfo[] constructors, Type srcMemberType, MemberInfo destMemberInfo, object srcValue, object objDest)
        {
            
            ConstructorInfo chosenOne = null;
            Stack<MemberInfo> chosenParameters = new Stack<MemberInfo>();
            Stack<MemberInfo> auxParameterStack = new Stack<MemberInfo>();

            int size = 0;

            for (int f = 0; f < constructors.Length; ++f)
            {
                ParameterInfo[] parameters = constructors[f].GetParameters();
                auxParameterStack = new Stack<MemberInfo>();

                for (int fy = 0; fy < parameters.Length; fy++)
                {
                    FieldInfo[] fields = srcMemberType.GetFields();
                    PropertyInfo[] properties = srcMemberType.GetProperties();

                    Boolean parameterMatch = false;

                    for (int pp = 0; pp < fields.Length; ++pp)
                    {
                        if (parameters[fy].Name.ToLower().Equals(fields[pp].Name.ToLower()))
                        {
                            parameterMatch = true;
                            auxParameterStack.Push(fields[pp]);
                            break;
                        }

                    }

                    for (int pp = 0; pp < properties.Length; ++pp)
                    {
                        if (parameters[fy].Name.ToLower().Equals(properties[pp].Name.ToLower()))
                        {
                            parameterMatch = true;
                            auxParameterStack.Push(properties[pp]);
                            break;
                        }

                    }

                    if (parameterMatch == false)
                    {
                        auxParameterStack = new Stack<MemberInfo>();
                        break;
                    }


                    if (fy + 1 == parameters.Length && size < parameters.Length)
                    {
                        chosenOne = constructors[f];
                        chosenParameters = auxParameterStack;
                        size = parameters.Length;
                    }
                }
            }

            if (chosenOne != null)
            {
                object[] parametersToPass = new object[chosenOne.GetParameters().Length];
                for (int p = parametersToPass.Length - 1; p >= 0; --p)
                {
                    MemberInfo parameter = chosenParameters.Pop();
                    if (parameter.MemberType == MemberTypes.Field)
                    {
                        parametersToPass[p] = ((FieldInfo)parameter).GetValue(srcValue);
                        
                    }
                    else if (parameter.MemberType == MemberTypes.Property)
                    {
                        parametersToPass[p] = ((PropertyInfo)parameter).GetValue(srcValue);
                        
                    }

                }

                if (destMemberInfo.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)destMemberInfo).SetValue(objDest, chosenOne.Invoke(parametersToPass));
                }
                else if (destMemberInfo.MemberType == MemberTypes.Field)
                {
                    ((FieldInfo)destMemberInfo).SetValue(objDest, chosenOne.Invoke(parametersToPass));
                }
                
            }
        }
    
        private void setFields(object src, object objDest)
        {
            FieldInfo[] fiSrc = src.GetType().GetFields();
            FieldInfo fiDest;
            for (int i = 0; i < fiSrc.Length; ++i)
            {

                if (matcher.TryGetValue(fiSrc[i].Name, out value))

                {
                    fiDest = klassDest.GetField(value);
                    if (objDest.GetType().GetField(value).FieldType.IsValueType || objDest.GetType().GetField(value).FieldType == typeof(string) || objDest.GetType().GetField(value).FieldType.IsArray)
                    {
                        fiDest.SetValue(objDest, fiSrc[i].GetValue(src));
                    }
                    else
                    {
                        Type srcFieldType = fiSrc[i].FieldType;
                        if (null != srcFieldType.GetConstructor(new Type[0]))
                        {
                          
                            IMapper auxMapper = AutoMapper.Build(srcFieldType, klassDest.GetField(value).FieldType);
                            klassDest.GetField(value).SetValue(objDest, auxMapper.Map(fiSrc[i].GetValue(src)));
                        }
                        else if (fiSrc[i].GetValue(src) != null)
                        {
                            Type fieldDestType = objDest.GetType().GetField(value).FieldType;
                            

                            ConstructorInfo[] constructors = fieldDestType.GetConstructors();

                            createReferenceTypeMemberWithConstructor(constructors, srcFieldType, fiDest, fiSrc[i].GetValue(src), objDest);
                        }
                    }
                }
            }
        }

        private void selectPropertiesCustomAttributes(Type CustomAttri)
        {

            PropertyInfo[] propSrc = klassSrc.GetProperties();
            PropertyInfo[] propDest = klassDest.GetProperties();

            for (int i = 0; i < propSrc.Length; ++i)
            {
                var attributes = propSrc[i].GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == CustomAttri)
                    {
                        for (int j = 0; i < propDest.Length; ++i)
                            if (propSrc[i].Name.Equals(propDest[j].Name) &&
                                propSrc[i].GetType() == propDest[j].GetType())
                            {
                                matcher.Add(propSrc[i].Name, propDest[j].Name);
                            }
                    }
                }
            }
        }

        private void selectFieldsCustomAttributes(Type CustomAttrib)
        {
            FieldInfo[] fieldSrc = klassSrc.GetFields();
            FieldInfo[] fieldDest = klassDest.GetFields();

            for (int i = 0; i < fieldSrc.Length; ++i)
            {
                var attributes = fieldSrc[i].GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == CustomAttrib)
                    {
                        for (int j = 0; i < fieldDest.Length; ++i)
                            if (fieldSrc[i].Name.Equals(fieldDest[j].Name) &&
                                fieldSrc[i].GetType() == fieldDest[j].GetType())
                            {
                                matcher.Add(fieldSrc[i].Name, fieldDest[j].Name);
                            }
                    }
                }
            }
        }
    }
}
