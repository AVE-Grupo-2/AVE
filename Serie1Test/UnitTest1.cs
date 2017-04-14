using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie1;

namespace Serie1Test
{
    [TestClass]
    public class IMapperTest
    {
        [TestMethod]
        public void TestMatch()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200 };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Nr, p.Id);
        }

        [TestMethod]
        public void TestPublicProperties()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person));
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200 };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
        }

        [TestMethod]
        public void TestPublicFields()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person));
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200};
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.field, p.field);
        }

        [TestMethod]
        public void TestPrivateProperties()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person));
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200 };
            Person p = (Person)m.Map(s);

            Assert.AreEqual(s.getPrivateProperty(), p.getPrivateProperty());
            
        }

        [TestMethod]
        public void TestPrivateFields()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person));
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200 };
            Person p = (Person)m.Map(s);

            Assert.AreEqual(s.getPrivateField(), p.getPrivateField());
        }

        /*Test is a compilation of all other Map tests*/
        [TestMethod]
        public void TestMapObject() {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200 };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Nr, p.Id);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.field, p.field);
            Assert.AreEqual(s.getPrivateProperty(), p.getPrivateProperty());
            Assert.AreEqual(s.getPrivateField(), p.getPrivateField());
        }

        [TestMethod]
        public void TestObjectNotMapped()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Teacher teacher = new Teacher(123,"t");
            Person p = (Person)m.Map(teacher);
            Assert.IsNull(p);
        }

        [TestMethod]
        public void TestObjectArray()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");

            Object[] array =
            {
                new Student {Nr = 27721, Name = "Ze Manel", field = 200},
              //  new Teacher {Name = "IamTeacher", Id = 123, field = 300 },
                null,
                new Student {Nr = 12345, Name = "IamStudent", field = 400}
            };

            Object[] res = m.Map(array);
                                 
        }

        [TestMethod]
        public void ReferenceField()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200, teacher = new Teacher(123,"t") };
            Person p = (Person)m.Map(s);
            Object.ReferenceEquals(s.teacher, p.teacher);
            Assert.IsFalse( Object.ReferenceEquals(s.teacher, p.teacher));
            Assert.AreEqual(s.teacher.name, p.teacher.name);
        }

        [TestMethod]
        public void ReferenceProperty()
        {
            int[] ids = { 1, 2, 3, 4 };
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200, teacher = new Teacher(123, "t") ,
                org = new School { location = "ab street",
                membersIDs = ids,
                name = "aSchool"

                }
            };

            Person p = (Person)m.Map(s);

            Assert.AreEqual(s.org.name, p.org.name);
            Assert.AreEqual(s.org.membersIDs[0], p.org.membersIDs[0]);
        }
    }

}
