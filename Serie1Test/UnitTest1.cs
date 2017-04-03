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
            IMapper m = AutoMapper.Build(typeof(Student), typeof(Person));
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = (Person)m.Map(s);
            Assert.Equals(s.Name, p.Name);
            Assert.Equals(0, p.Id);
        }
    }
}
