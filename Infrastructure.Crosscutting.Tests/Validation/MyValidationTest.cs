using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Infrastructure.Crosscutting.Validator;
using Infrastructure.Crosscutting.Validator.Rules;
using Infrastructure.Crosscutting.Declaration;

namespace Infrastructure.Crosscutting.Tests.Validation
{
     [TestFixture]
    public class MyValidationTest
    {
         [Test]
         public void Text_String_Format()
         {
             A instent = new A() {IsSecuss = false,ItemA = 4,Name = "jim"};
             
             //ValidationManager.GetValidator<A>().GreaterThan(a=>a.ItemA,3,"不能大于3").MaxLength(a=>a.Name,4,"不能超过4个字符");

             Console.WriteLine(instent.FormatToString("B"));

             DateTime dt = DateTime.Now;
             Guid g = Guid.NewGuid();
             Console.WriteLine(g.FormatToString("D"));
             Console.WriteLine(dt.FormatToString("yyyy-MM-dd"));
             
             ValidationManager.Validate(instent);
             
         }
    }

    public class A
    {
        [GreaterThanOrEqual(2,"sss")]
        public int ItemA { get; set; }

        public string Name { get; set; }

        public bool IsSecuss { get; set; }
        }
}
