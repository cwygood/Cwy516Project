using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwy516Project
{
    public class Test : ITest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public void Show()
        {
            Console.WriteLine("AAAA");
        }
    }
}
