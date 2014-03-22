using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Shadow_Mission
{
    class Stack
    {
        public List<int> stack = new List<int>();
        public List<bool> sPointer = new List<bool>();

        public void clear()
        {
            stack.Clear();
            sPointer.Clear();
        }

        public void push(int value, bool pointer)
        {
            stack.Add(value);
            sPointer.Add(pointer);
        }

        public int pop()
        {
            int pop = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            return pop;
        }

        public int size()
        {
            return stack.Count;
        }

        public int get(int index)
        {
            return stack[index];
        }

        public bool isPointer(int index)
        {
            return sPointer[index];
        }

        public void add()
        {
            int value1 = stack[stack.Count - 1];
            int value2 = stack[stack.Count - 2];
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.Add(value1 + value2);
            sPointer.Add(false);
        }

        public void sub()
        {
            int value1 = stack[stack.Count - 1];
            int value2 = stack[stack.Count - 2];
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.Add(value1 - value2);
            sPointer.Add(false);
        }

        public void mul()
        {
            int value1 = stack[stack.Count - 1];
            int value2 = stack[stack.Count - 2];
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.Add(value1 * value2);
            sPointer.Add(false);
        }

        public void div()
        {
            int value1 = stack[stack.Count - 1];
            int value2 = stack[stack.Count - 2];
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            //stack.Add(value1 / value2);
        }

        public void mod()
        {
            int value1 = stack[stack.Count - 1];
            int value2 = stack[stack.Count - 2];
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.RemoveAt(stack.Count - 1);
            sPointer.RemoveAt(sPointer.Count - 1);
            stack.Add(value1 % value2);
            sPointer.Add(false);
        }

        public void neg()
        {
            int value = pop();
            value = 0 - value;
            push(value, false);
        }

    }
}
