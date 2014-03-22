using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadow_Mission
{
    class Opcodes
    {
        public List<Opcode> opcodes = new List<Opcode>();

        public Opcodes()
        {
            opcodes.Add(new Opcode("nop", 1, "No operation"));
            opcodes.Add(new Opcode("iadd", 1, "Adds the top 2 items on the stack"));
            opcodes.Add(new Opcode("isub", 1, "Subtracts the top 2 items on the stack "));
            opcodes.Add(new Opcode("imul", 1, "Multiplies the top 2 items on the stack "));
            opcodes.Add(new Opcode("idiv", 1, "Divides the top 2 items on the stack "));
            opcodes.Add(new Opcode("imod", 1, "Mods the top 2 items on the stack "));
            opcodes.Add(new Opcode("iszero", 1, "Checks the first item on the stack to see if it equals 0 "));
            opcodes.Add(new Opcode("ineg", 1, "No operation"));
            opcodes.Add(new Opcode("icmpeq", 1, "No operation"));
            opcodes.Add(new Opcode("icmpne", 1, "No operation"));
            opcodes.Add(new Opcode("icmpgt", 1, "No operation"));
            opcodes.Add(new Opcode("icmpge", 1, "No operation"));
            opcodes.Add(new Opcode("icmplt", 1, "No operation"));
            opcodes.Add(new Opcode("icmple", 1, "No operation"));
            opcodes.Add(new Opcode("fadd", 1, "No operation"));
            opcodes.Add(new Opcode("fsub", 1, "No operation"));
            opcodes.Add(new Opcode("fmul", 1, "No operation"));
            opcodes.Add(new Opcode("fdiv", 1, "No operation"));
            opcodes.Add(new Opcode("fmod", 1, "No operation"));
            opcodes.Add(new Opcode("fneg", 1, "No operation"));
            opcodes.Add(new Opcode("fcmpeq", 1, "No operation"));
            opcodes.Add(new Opcode("fcmpne", 1, "No operation"));
            opcodes.Add(new Opcode("fcmpgt", 1, "No operation"));
            opcodes.Add(new Opcode("fcmpge", 1, "No operation"));
            opcodes.Add(new Opcode("fcmplt", 1, "No operation"));
            opcodes.Add(new Opcode("fcmple", 1, "No operation"));
            opcodes.Add(new Opcode("vadd", 1, "No operation"));
            opcodes.Add(new Opcode("vsub", 1, "No operation"));
            opcodes.Add(new Opcode("vmul", 1, "No operation"));
            opcodes.Add(new Opcode("vdiv", 1, "No operation"));
            opcodes.Add(new Opcode("vneg", 1, "No operation"));
            opcodes.Add(new Opcode("iand", 1, "No operation"));
            opcodes.Add(new Opcode("ior", 1, "No operation"));
            opcodes.Add(new Opcode("ixor", 1, "No operation"));
            opcodes.Add(new Opcode("jmp", 5, "No operation"));
            opcodes.Add(new Opcode("jmpf", 5, "No operation"));
            opcodes.Add(new Opcode("jmpt", 5, "No operation"));
            opcodes.Add(new Opcode("itof", 1, "No operation"));
            opcodes.Add(new Opcode("ftoi", 1, "No operation"));
            opcodes.Add(new Opcode("ftov", 1, "No operation"));
            opcodes.Add(new Opcode("ipush2", 3, "No operation"));
            opcodes.Add(new Opcode("ipush", 5, "No operation"));
            opcodes.Add(new Opcode("fpush", 5, "No operation"));
            opcodes.Add(new Opcode("dup", 1, "No operation"));
            opcodes.Add(new Opcode("pop", 1, "No operation"));
            opcodes.Add(new Opcode("native", 7, "No operation"));
            opcodes.Add(new Opcode("call", 5, "No operation"));
            opcodes.Add(new Opcode("enter", 4, "No operation"));
            opcodes.Add(new Opcode("ret", 3, "No operation"));
            opcodes.Add(new Opcode("pget", 1, "No operation"));
            opcodes.Add(new Opcode("pset", 1, "No operation"));
            opcodes.Add(new Opcode("ppeekset", 1, "No operation"));
            opcodes.Add(new Opcode("explode", 1, "No operation"));
            opcodes.Add(new Opcode("implode", 1, "No operation"));
            opcodes.Add(new Opcode("flvar0", 1, "No operation"));
            opcodes.Add(new Opcode("flvar1", 1, "No operation"));
            opcodes.Add(new Opcode("flvar2", 1, "No operation"));
            opcodes.Add(new Opcode("flvar3", 1, "No operation"));
            opcodes.Add(new Opcode("flvar4", 1, "No operation"));
            opcodes.Add(new Opcode("flvar5", 1, "No operation"));
            opcodes.Add(new Opcode("flvar6", 1, "No operation"));
            opcodes.Add(new Opcode("flvar7", 1, "No operation"));
            opcodes.Add(new Opcode("flvar", 1, "No operation"));
            opcodes.Add(new Opcode("local", 1, "No operation"));
            opcodes.Add(new Opcode("global", 1, "No operation"));
            opcodes.Add(new Opcode("array", 1, "No operation"));
            opcodes.Add(new Opcode("switch", 1, "No operation"));
            opcodes.Add(new Opcode("spush", 2, "No operation"));
            opcodes.Add(new Opcode("null", 1, "No operation"));
            opcodes.Add(new Opcode("scpy", 1, "No operation"));
            opcodes.Add(new Opcode("itos", 1, "No operation"));
            opcodes.Add(new Opcode("sadd", 1, "No operation"));
            opcodes.Add(new Opcode("saddi", 1, "No operation"));
            opcodes.Add(new Opcode("catch", 1, "No operation"));
            opcodes.Add(new Opcode("throw", 1, "No operation"));
            opcodes.Add(new Opcode("memcpy", 1, "No operation"));
            opcodes.Add(new Opcode("getxprotect", 1, "No operation"));
            opcodes.Add(new Opcode("setxprotect", 1, "No operation"));
            opcodes.Add(new Opcode("refxprotect", 1, "No operation"));
            opcodes.Add(new Opcode("exit", 1, "No operation"));
            for (int i = 80; i <= 255; i++)
            {
                opcodes.Add(new Opcode("ipush1", 1, "No operation"));
            }
        }
    }
}
