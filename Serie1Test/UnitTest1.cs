using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie1;

namespace Serie1Test
{
    [TestClass]
    public class AutoMapperTest
    {
        [TestMethod]
        public void TestNumberAndName()
        {
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person)).Match("Nr", "Id");
            Student s = new Student { Nr = 27721, Name = "Ze Manel", field = 200 };
            Person p = (Person)m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Id);
        }

    }
}
