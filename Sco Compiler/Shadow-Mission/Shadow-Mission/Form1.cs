using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Shadow_Mission
{
    public partial class Form1 : Form
    {
        private Opcodes opcodes = new Opcodes();
        private Stack[] stack = new Stack[10];
        private byte[] header;
        private byte[] code;
        private byte[] local;
        private byte[] global;
        private int currentI = 0;
        private int[] locals;
        private int[] globals;
        private List<String> strings = new List<String>();
        private int stringPointer = 0;
        private List<Sub> subs = new List<Sub>();
        private Boolean firstEnter = true;
        private List<byte> codeList = new List<byte>();
        private List<String> natives = new List<String>();

        private int currentStack = 0;

        private String debugCode = "";
        private String codeCode = "";

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            stack[0] = new Stack();
            loadNativesToMem();
        }

        public static byte[] Decrypt(byte[] dataIn)
        {
            byte[] data = new byte[dataIn.Length];
            dataIn.CopyTo(data, 0);

            // Create our Rijndael class
            Rijndael rj = Rijndael.Create();
            rj.BlockSize = 128;
            rj.KeySize = 256;
            rj.Mode = CipherMode.ECB;
            rj.Key = getKey();
            rj.IV = new byte[16];
            rj.Padding = PaddingMode.None;

            ICryptoTransform transform = rj.CreateDecryptor();

            int dataLen = data.Length & ~0x0F;

            // Decrypt!

            // R* was nice enough to do it 16 times...
            // AES is just as effective doing it 1 time because it has multiple internal rounds

            if (dataLen > 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    transform.TransformBlock(data, 0, dataLen, data, 0);
                }
            }

            return data;
        }

        public static byte[] Encrypt(byte[] dataIn)
        {
            byte[] data = new byte[dataIn.Length];
            dataIn.CopyTo(data, 0);

            // Create our Rijndael class
            Rijndael rj = Rijndael.Create();
            rj.BlockSize = 128;
            rj.KeySize = 256;
            rj.Mode = CipherMode.ECB;
            rj.Key = getKey();
            rj.IV = new byte[16];
            rj.Padding = PaddingMode.None;

            ICryptoTransform transform = rj.CreateEncryptor();

            int dataLen = data.Length & ~0x0F;

            // Decrypt!

            // R* was nice enough to do it 16 times...
            // AES is just as effective doing it 1 time because it has multiple internal rounds

            if (dataLen > 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    transform.TransformBlock(data, 0, dataLen, data, 0);
                }
            }

            return data;
        }

        private String searchNative(UInt32 hash)
        {
            StreamReader file = new StreamReader("natives.ini");
            String native = "<unknown> " + hash.ToString();
            String line = "";
            while ((line = file.ReadLine()) != null)
            {
                String[] split = line.Split('=');
                if (split[0].Equals(hash.ToString()))
                {
                    native = split[1];
                }
            }
            file.Close();
            file.Dispose();
            return native;
        }

        private static byte[] getKey()
        {
            // TODO: Point to your local GTA IV.exe
            BinaryReader tempBR = new BinaryReader(new FileStream("D:/Games/Rockstar Games/Grand Theft Auto IV/GTAIV.exe", FileMode.Open));
            // TODO: Change the offset to your version offset
            tempBR.BaseStream.Seek(0xA94204, SeekOrigin.Begin);
            byte[] key = tempBR.ReadBytes(32);
            tempBR.Close();
            tempBR.Dispose();
            return key;
        }

        public static uint Hash(string str)
        {
            uint value = 0, temp;
            var index = 0;
            var quoted = false;

            if (str[index] == '"')
            {
                quoted = true;
                index++;
            }

            str = str.ToLower();

            for (; index < str.Length; index++)
            {
                var v = str[index];

                if (quoted && (v == '"')) break;

                if (v == '\\')
                    v = '/';

                temp = v;
                temp = temp + value;
                value = temp << 10;
                temp += value;
                value = temp >> 6;
                value = value ^ temp;
            }

            temp = value << 3;
            temp = value + temp;
            var temp2 = temp >> 11;
            temp = temp2 ^ temp;
            temp2 = temp << 15;

            value = temp2 + temp;

            if (value < 2) value += 2;

            return value;
        }

        private void loadNativesToMem()
        {
            String line = "";
            StreamReader file = new StreamReader("natives.ini");
            while ((line = file.ReadLine()) != null)
            {
                String[] split = line.Split('=');
                natives.Add(split[1]);
            }
            file.Close();
            file.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            for (currentI = 0; currentI < code.Length;)
            {
                Debug.WriteLine(currentI + " opcode: " + code[currentI]);
                Opcode temp = opcodes.opcodes[code[currentI]];

                parseOpcode(code, currentI);
                textDebug.AppendText(debugCode + Environment.NewLine);
            }
            debugCode = "";
        }

        private void parseOpcode(byte[] code, int index){
            Opcode temp = opcodes.opcodes[code[index]];
            String ret = index + ": " + code[index] + " - ";

            switch (code[index])
                {
                    case 0:
                        ret += "nop";
                        MessageBox.Show("Todo " + ret);
                        break; 
                    case 1:
                        stack[currentStack].add();
                        ret += "iadd";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 2:
                        stack[currentStack].sub();
                        ret += "isub";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 3:
                        stack[currentStack].mul();
                        ret += "imul";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 4:
                        stack[currentStack].div();
                        ret += "idiv";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 5:
                        stack[currentStack].mod();
                        ret += "imod";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 6:
                        ret += "iszero";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 7:
                        ret += "ineg";
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 8:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 9:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 10:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 11:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 12:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 13:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 14:
                        stack[currentStack].add();
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 15:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 16:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 17:
                        MessageBox.Show("Todo " + ret);
                        ret += temp.getName();
                        break;
                    case 18:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 19:
                        int old19 = stack[currentStack].get(stack[currentStack].size() - 1);
                        stack[currentStack].neg();
                        int new19 = stack[currentStack].get(stack[currentStack].size() - 1);
                        ret += temp.getName() + " " + old19 + " to " + new19;
                        //MessageBox.Show("Todo " + ret);
                        break;
                    case 20:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 21:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 22:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 23:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 24:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 25:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 26:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 27:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 28:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 29:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 30:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 31:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 32:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 33:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 34:
                        codeCode = "jump @Label" + BitConverter.ToInt32(code, index + 1) + Environment.NewLine;
                        ret += temp.getName() + " to " + BitConverter.ToInt32(code, index + 1);
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 35:
                        codeCode = "jump_if_false @Label" + (BitConverter.ToInt32(code, index + 1) + 5);
                        ret += temp.getName() + " to " + (BitConverter.ToInt32(code, index + 1) + 5);
                        //temp!
                        //currentI = BitConverter.ToInt32(code, index + 1);
                        //end temp!
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 36:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 37:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 38:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 39:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 40:
                        Int16 sh = BitConverter.ToInt16(code, index + 1);
                        stack[currentStack].push(Convert.ToInt32(sh), false);
                        ret += temp.getName() + " " + sh;
                        break;
                    case 41:
                        int var41 = BitConverter.ToInt32(code, index + 1);
                        stack[currentStack].push(var41, false);
                        ret += temp.getName() + " " + var41;
                        break;
                    case 42:
                        float var42 = BitConverter.ToSingle(code, index + 1);
                        stack[currentStack].push(Convert.ToInt32(var42), false);
                        ret += temp.getName() + " " + var42;
                        break;
                    case 43:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 44:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 45:
                        int argCount45 = code[index+1];
                        ret += searchNative(BitConverter.ToUInt32(code, index + 3)) + " argCount: " + argCount45 + " retCount: " + code[index+2];
                        MessageBox.Show("Todo args" + ret);
                        codeCode = searchNative(BitConverter.ToUInt32(code, index + 3)) + "(";
                        for(int i = 0; i < argCount45; i++){
                            codeCode += stack[currentStack].pop();
                            if((i+1) < argCount45){
                                codeCode += ", ";
                            }
                        }
                        codeCode += ")";
                        break;
                    case 46:
                        Sub sub46 = searchSub(BitConverter.ToInt32(code, index + 1));
                        codeCode = "call @Label" + BitConverter.ToInt32(code, index + 1) + "(";
                        for (int i = 0; i < sub46.args; i++)
                        {
                            codeCode += stack[currentStack].pop();
                            if ((i + 1) < sub46.args)
                            {
                                codeCode += ", ";
                            }
                        }
                        codeCode += ")";
                        //stack[currentStack].push(index, false);
                        ret += temp.getName() + " @Label" + BitConverter.ToInt32(code, index + 1);

                        MessageBox.Show("Todo " + ret);
                        break;
                    case 47:
                        subs.Add(new Sub(index, "" + index, code[index+1], BitConverter.ToInt16(code, index+2)));
                        ret += ":Label" + index + " arg: " + code[index+1] + " vars: " + BitConverter.ToInt16(code, index+2);
                        codeCode = ":Label" + index + "(";
                        for(int i = 0; i < code[index+1]; i++){
                            codeCode += "var" + i;
                            if ((i + 1) < code[index + 1])
                            {
                                codeCode += ", ";
                            }
                            else
                            {
                                codeCode += ") vars: " + BitConverter.ToInt16(code, index + 2);
                            }
                        }
                        //if (!firstEnter)
                        //{
                            MessageBox.Show("Swap to temp stack " + currentStack);
                            //swap to temp stack[currentStack]
                            currentStack += 1;
                            stack[currentStack] = new Stack();
                            //
                          //  firstEnter = false;
                        //}
                        break;
                    case 48:
                        //swap back
                        MessageBox.Show("Swap back");
                        stack[currentStack].clear();
                        currentStack -= 1;
                        //
                        codeCode = "Return" + Environment.NewLine;
                        ret += "return " + " args: " + code[index+1] + " stack[currentStack] return: " + code[index+2];
                        break;
                    case 49:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 50:
                        int lPointer = stack[currentStack].pop();
                        int value = stack[currentStack].pop();
                        if (lPointer >= locals.Length)
                        {
                            globals[lPointer - locals.Length] = value;
                            ret += "pset global " + (lPointer - locals.Length) + " = " + value;
                            codeCode = "$G_" + (lPointer - locals.Length) + " = " + value;
                        }
                        else
                        {
                            locals[lPointer] = value;
                            ret += "pset local " + lPointer + " = " + value;
                            codeCode = "$L_" + lPointer + " = " + value;
                        }
                        break;
                    case 51:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 52:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 53:
                        int pArray53 = stack[currentStack].pop();
                        int array53Size = stack[currentStack].pop();
                        int[] array53 = new int[array53Size];
                        String array53Text = "";
                        for (int i = 0; i < array53Size; i++)
                        {
                            array53[i] = stack[currentStack].pop();
                            array53Text += array53[i] + ", ";
                        }
                        if (pArray53 >= locals.Length)
                        {
                            //globals[pArray53 - locals.Length] = value;
                            ret += "implode global " + (pArray53 - locals.Length) + " = " + array53Text;
                            codeCode = "$G_" + (pArray53 - locals.Length) + " = array[" + array53Size + "](" + array53Text + ")";
                        }
                        else
                        {
                            //locals[pArray53] = value;
                            ret += "implode local " + pArray53 + " = " + array53Text;
                            codeCode = "$L_" + (pArray53) + " = array[" + array53Size + "](" + array53Text + ")";
                        }
                        //MessageBox.Show("Adress: " + pArray53 + " size: " + array53Size);
                        //ret += temp.getName();
                        //MessageBox.Show("Todo " + ret);
                        break;
                    case 54:
                        stack[currentStack].push(0, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 55:
                        stack[currentStack].push(1, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 56:
                        stack[currentStack].push(2, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 57:
                        stack[currentStack].push(3, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 58:
                        stack[currentStack].push(4, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 59:
                        stack[currentStack].push(5, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 60:
                        stack[currentStack].push(6, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 61:
                        stack[currentStack].push(7, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 62:
                        stack[currentStack].push(8, true);
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 63:
                        int localIndex = stack[currentStack].pop();   //pop the index
                        stack[currentStack].push(localIndex, true);         //Push pointer to local var (we don't use that so..)
                        ret += "local";
                        break;
                    case 64:
                        int globalIndex = stack[currentStack].pop();   //pop the index
                        stack[currentStack].push(locals.Length + globalIndex, true);         //Push pointer to global var (we don't use that so..)
                        ret += "global";
                        break;
                    case 65:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 66:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 67:
                        int strLength = code[index + 1];
                        currentI += strLength;
                        String str = "";
                        for (int i = 0; i < strLength-1; i++)
                        {
                            str += Convert.ToChar(code[index + 2 + i]);
                        }
                        strings.Add(str);
                        ret += "Push String - " + str;
                        stack[currentStack].push(locals.Length + globals.Length + strings.Count, true);
                        updateStringBox();
                        break;
                    case 68:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 69:
                        int varSPointer = stack[currentStack].pop();
                        int sPointer = stack[currentStack].pop();
                        if (varSPointer >= locals.Length)
                        {
                            globals[varSPointer - locals.Length] = sPointer;
                            ret += "pset global " + (varSPointer - locals.Length) + " = " + sPointer;
                            codeCode = "$G_" + (varSPointer - locals.Length) + " = \"" + strings[sPointer-stringPointer-1] + "\"";
                        }
                        else
                        {
                            locals[varSPointer] = sPointer;
                            ret += "pset local " + varSPointer + " = " + sPointer;
                            codeCode = "$L_" + varSPointer + " = " + strings[sPointer - stringPointer];
                        }
                        currentI += 1; //skip '16'
                        break;
                    case 70:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 71:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 72:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 73:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 74:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 75:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 76:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 77:
                        int lPointer77 = stack[currentStack].pop();
                        int value77 = stack[currentStack].pop();
                        if (lPointer77 >= locals.Length)
                        {
                            globals[lPointer77 - locals.Length] = value77;
                            ret += "xlive set global " + (lPointer77 - locals.Length) + " = " + value77;
                            codeCode = "$G_" + (lPointer77 - locals.Length) + " = " + value77;
                        }
                        else
                        {
                            locals[lPointer77] = value77;
                            ret += "xlive set local " + lPointer77 + " = " + value77;
                            codeCode = "$L_" + lPointer77 + " = " + value77;
                        }
                        break;
                    case 78:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    case 79:
                        ret += temp.getName();
                        MessageBox.Show("Todo " + ret);
                        break;
                    default:
                        ret += temp.getName() + " " + (code[index] - 96);
                        stack[currentStack].push(code[index] - 96, false);
                        break;
                }
            currentI += temp.getLength();
            debugCode = ret;
            if (!codeCode.Equals(""))
            {
                textCode.AppendText(codeCode + Environment.NewLine);
                codeCode = "";
            }
        }

        private Sub searchSub(int subIndex)
        {
            Sub temp = null;
            for (int i = 0; i < subs.Count; i++)
            {
                if (subs[i].adress == subIndex)
                {
                    temp = subs[i];
                }
            }
            return temp;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            /*String line;
            StreamReader file = new StreamReader("natives.txt");
            StreamWriter writer = new StreamWriter("natives.ini");
            while ((line = file.ReadLine()) != null)
            {
                writer.WriteLine(Hash(line) + "=" + line);
            }
            writer.Flush();
            file.Close();
            writer.Close();*/
            stack[currentStack].push(1, false);
            updatestackBox();
            stack[currentStack].push(2, false);
            updatestackBox();

            stack[currentStack].push(3, false);
            updatestackBox();
            stack[currentStack].add();
            updatestackBox();
        }

        private void updatestackBox()
        {
            listBox1.Items.Clear();
            for (int i = stack[currentStack].size()-1; i >= 0; i--)
            {
                if(stack[currentStack].isPointer(i))
                {
                    listBox1.Items.Add("&" + stack[currentStack].get(i));
                }
                else
                {
                    listBox1.Items.Add(stack[currentStack].get(i));
                }
            }
        }

        private void updateLocalBox()
        {
            //Debug.WriteLine("Updating locals");
            listBox2.Items.Clear();
            for (int i = 0; i < locals.Length - 1; i++)
            {
                listBox2.Items.Add(locals[i].ToString());
            }
        }

        private void updateGlobalBox()
        {
            //Debug.WriteLine("Updating locals");
            listBox3.Items.Clear();
            for (int i = 0; i < globals.Length - 1; i++)
            {
                listBox3.Items.Add(globals[i].ToString());
            }
        }

        private void updateStringBox()
        {
            listBox4.Items.Clear();
            for (int i = 0; i < strings.Count; i++)
            {
                listBox4.Items.Add("&" + (locals.Length + globals.Length + i) + ": " + strings[i]);
            }
        }

        //step
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!toolStripTextSkip.Text.Equals(""))
            {
                while (currentI < Convert.ToInt32(toolStripTextSkip.Text))
                {
                    Debug.WriteLine(currentI + " opcode: " + code[currentI]);
                    Opcode temp = opcodes.opcodes[code[currentI]];

                    parseOpcode(code, currentI);
                    textDebug.AppendText(debugCode + Environment.NewLine);
                }
                updatestackBox();
                updateLocalBox();
                //updateGlobalBox();
                updateStringBox();
                toolStripTextSkip.Text = "";
                debugCode = "";
            }
            else
            {

                if (currentI < code.Length)
                {
                    Opcode temp = opcodes.opcodes[code[currentI]];
                    parseOpcode(code, currentI);
                    updatestackBox();
                    updateLocalBox();
                    //updateGlobalBox();
                    updateStringBox();
                }
                else
                {
                    Debug.WriteLine("No code left");
                }
                textDebug.AppendText(debugCode + Environment.NewLine);
                debugCode = "";
            }
        }

        //open
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // TODO: Hardcoded path, change this to a filechoser
            BinaryReader br = new BinaryReader(new FileStream("D:/Data/Projects/Sco Compiler/startup.sco", FileMode.Open));

            //read the header
            header = br.ReadBytes(24);
            br.BaseStream.Seek(0, SeekOrigin.Begin);

            Debug.WriteLine("ID: " + br.ReadInt32());
            int codeSize = br.ReadInt32();
            Debug.WriteLine("Code Size: " + codeSize);
            int localCount = br.ReadInt32();
            Debug.WriteLine("Local Count: " + localCount);
            int globalCount = br.ReadInt32();
            Debug.WriteLine("Global Count: " + globalCount);
            Debug.WriteLine("Flags: " + br.ReadUInt32());
            Debug.WriteLine("Signature: " + br.ReadUInt32());

            toolStripText.Text = "Size: " + codeSize + " Local: " + localCount + " Global: " + globalCount;

            code = Decrypt(br.ReadBytes(codeSize));
            local = Decrypt(br.ReadBytes(localCount * 4));
            global = Decrypt(br.ReadBytes(globalCount * 4));

            locals = new int[localCount];
            for (int i = 0; i < localCount; i++)
            {
                locals[i] = BitConverter.ToInt32(local, i * 4);
            }
            updateLocalBox();

            globals = new int[globalCount];
            for (int i = 0; i < globalCount; i++)
            {
                globals[i] = BitConverter.ToInt32(global, i * 4);
            }

            stringPointer = locals.Length + globals.Length;
            //updateGlobalBox();

            br.Close();
            br.Dispose();
        }

        //export
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream("D:/Data/Projects/Sco Compiler/startup3.sco.dec", FileMode.Create));
            bw.Write(header);
            bw.Flush();
            bw.Write(Encrypt(code));
            //bw.Write(code);
            bw.Flush();
            //bw.Write(local);
            bw.Write(Encrypt(local));
            bw.Flush();
            //bw.Write(global);
            bw.Write(Encrypt(global));
            bw.Flush();

            bw.Close();
            bw.Dispose();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {   
            // index 16093
            byte[] newCode = new byte[code.Length];
            int i2 = 0;
            for (int i = 0; i < code.Length;)
            {
                if (i == 16095)
                {
                    newCode[i2] = 0;
                    i++;
                    i2++;
                    newCode[i2] = 0x2D; //call native
                    i++;
                    i2++;
                    newCode[i2] = 0x01; //one arg
                    i++;
                    i2++;
                    newCode[i2] = 0x00; //no return
                    i++;
                    i2++;
                    newCode[i2] = 0xDF; //DF45B0D6
                    i++;
                    i2++;
                    newCode[i2] = 0x45; //DF45B0D6
                    i++;
                    i2++;
                    newCode[i2] = 0xB0; //DF45B0D6
                    i++;
                    i2++;
                    newCode[i2] = 0xD6; //DF45B0D6
                    i++;
                    i2++;
                    //i += 10;
                    //i2++;
                }
                else
                {
                    newCode[i2] = code[i];
                    i++;
                    i2++;
                }
            }
            /*for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == 0x2D)
                {
                    Debug.Print("Found 0x2D " + i);
                    if (code[i + 1] == 0x02 && code[i + 2] == 0x01 && code[i + 3] == 0x0E && code[i + 4] == 0x6D)
                    {
                        Debug.Print("It's the correct one");
                    }
                }
            }*/
            code = newCode;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            foreach (string line in textCode.Lines)
            {
                if (!line.Trim().Equals(""))
                {
                    switch (line.Substring(0, 1))
                    {
                        case ":":
                            codeList.Add(Convert.ToByte(47));
                            codeList.Add(0x0);
                            codeList.Add(0x4);
                            codeList.Add(0x0);
                            Debug.Print("Label: " + line);
                            break;
                        case "$":
                            parseVar(line);
                            break;
                        case "if":
                            parseIf(line);
                            break;
                        default:
                            parseLine(line);
                            break;
                    }
                }
            }
            writeFile();            
        }

        private void parseIf(String line)
        {
            Debug.Print("We got an if");
        }

        private void parseLine(String line)
        {
            if (line.Contains("("))
            {
                //we got a function or native
                String[] split = line.Split('(');
                if (isNative(split[0]))
                {
                    MessageBox.Show("You typed a native");
                }
                else
                {
                    MessageBox.Show("Why didn't you type a native :( " + split[0]);
                }
            }

        }

        private Boolean isNative(String posNative)
        {
            Boolean bIsNative = false;
            foreach (String native in natives)
            {
                if(native.Equals(posNative)) bIsNative = true;
            }
            return bIsNative;
        }

        private void writeFile()
        {
            // TODO: Filechoser
            BinaryWriter bw = new BinaryWriter(new FileStream("D:/Data/Projects/Sco Compiler/startup3.sco.dec", FileMode.Create));
            //bw.Write(header);
            //bw.Write(Encrypt(code));
            //bw.Write(Encrypt(local));
            //bw.Write(Encrypt(global));

            foreach (byte b in codeList)
            {
                bw.Write(b);
            }
            bw.Flush();
            bw.Close();
            bw.Dispose();
        }

        private void parseVar(String line)
        {
            String[] split = line.Split(' ');
            String[] values = split[0].Split('_');
            Debug.Print("Values: " + values[1]);
            pushValue(split[2]);
            pushValue(values[1]);
            
            if (line.StartsWith("$L"))
            {
                codeList.Add(63);
                Debug.Print("Writing local");
            }
            else if (line.StartsWith("$G"))
            {
                codeList.Add(64);
                Debug.Print("Writing global");
            }
            if (split[2].StartsWith("\""))
            {
                Debug.WriteLine(split[2]);
                codeList.Add(69);
                codeList.Add(0x10);
            }
            else
            {
                codeList.Add(50);
            }
        }

        private void pushValue(String sValue)
        {
            if(sValue.StartsWith("\""))
            {
                sValue = sValue.Replace("\"", ""); //prepare it
                // String
                // 67 - Push String
                codeList.Add(67);
                codeList.Add((byte)(sValue.Length+1));
                for(int i = 0; i < sValue.Length; i++)
                {
                    codeList.Add((byte)sValue[i]);
                }
                codeList.Add(0);
            }
            else if (sValue.Contains("."))
            {
                // 42 - Push float
                codeList.Add(42);
                byte[] bf = BitConverter.GetBytes(Convert.ToSingle(sValue));
                codeList.Add(bf[0]);
                codeList.Add(bf[1]);
                codeList.Add(bf[2]);
                codeList.Add(bf[3]);
            }
            else
            {
                int value = Convert.ToInt32(sValue);
                if (value <= 159 && value >= -16)
                {
                    // 80>255 - Push int (Opcode-96)
                    value += 96;
                    codeList.Add(Convert.ToByte(value));
                }
                else if (value >= 160 && value <= 0xFFFF)
                {
                    // 40 - Push short
                    codeList.Add(40);
                    Debug.Print("To int16: " + value);
                    byte[] bs = BitConverter.GetBytes(Convert.ToInt16(value));
                    codeList.Add(bs[0]);
                    codeList.Add(bs[1]);
                }
                else
                {
                    // 41 - Push int
                    codeList.Add(41);
                    byte[] bi = BitConverter.GetBytes(Convert.ToInt32(sValue));
                    codeList.Add(bi[0]);
                    codeList.Add(bi[1]);
                    codeList.Add(bi[2]);
                    codeList.Add(bi[3]);
                }
            }
        }
    }
}
