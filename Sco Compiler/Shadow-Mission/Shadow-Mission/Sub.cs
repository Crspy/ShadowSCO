using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadow_Mission
{
    class Sub
    {
        public int adress;
        public String name;
        public int args;
        public int vars;

        public Sub(int adress, String name, int args, int vars)
        {
            this.adress = adress;
            this.name = name;
            this.args = args;
            this.vars = vars;
        }
    }
}
