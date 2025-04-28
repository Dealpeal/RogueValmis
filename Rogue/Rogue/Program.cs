using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public class Program
    {
        public static void Main()
        {

            

            Console.Clear();
            while (true)
            {
                Game rogue = new Game();
                rogue.Run();
            }

        }
    }
}
