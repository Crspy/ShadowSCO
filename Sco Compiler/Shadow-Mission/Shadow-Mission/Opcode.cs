using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadow_Mission
{
    class Opcode
    {
        private String name;
        private int length;
        private String description;

        public Opcode(String name, int length, String description)
        {
            this.name = name;
            this.length = length;
        }

        public String getName()
        {
            return name;
        }

        public int getLength()
        {
            return length;
        }

        public string getDescription()
        {
            return description;
        }
    }
}
