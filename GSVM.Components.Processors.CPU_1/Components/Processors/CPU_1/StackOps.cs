using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSVM.Components.Processors.CPU_1;
using GSVM.Constructs.DataTypes;
using GSVM.Components.Clocks;
using GSVM.Constructs;

namespace GSVM.Components.Processors
{
    public partial class CPU1 : CPU
    {
        void PushRegister(Register_t reg)
        {
            int size = registers.SizeOf(reg);
            switch (size)
            {
                case 8:
                    uint64_t value64 = registers.Read<uint64_t>(reg);
                    System.Diagnostics.Debug.Assert(value64 != null);
                    stack.Push(value64);
                    break;

                case 4:
                    uint32_t value32 = registers.Read<uint32_t>(reg);
                    System.Diagnostics.Debug.Assert(value32 != null);
                    stack.Push(value32);
                    break;

                case 2:
                    uint16_t value16 = registers.Read<uint16_t>(reg);
                    System.Diagnostics.Debug.Assert(value16 != null);
                    stack.Push(value16);
                    break;

                case 1:
                    uint8_t value8 = registers.Read<uint8_t>(reg);
                    System.Diagnostics.Debug.Assert(value8 != null);
                    stack.Push(value8);
                    break;
            }
        }

        void PushLiteral(uint16_t literal)
        {
            System.Diagnostics.Debug.Assert(literal != null);
            stack.Push(literal);
        }

        void PushAll()
        {
            PushRegister(Register.EAX);
            PushRegister(Register.EBX);
            PushRegister(Register.ECX);
            PushRegister(Register.EDX);
        }

        void Pop(Register_t reg)
        {
            ushort size = registers.SizeOf(reg);

            if (stack.Peek().Length > size)
            {
                // Throw a value indicating that the source value is larger than the destination register
                throw new Exception("Source larger than destination.");
            }
            else
            {
                registers.Write(reg, stack.Pop().ToBinary());
            }
        }

        void PopAll()
        {
            Pop(Register.EDX);
            Pop(Register.ECX);
            Pop(Register.EBX);
            Pop(Register.EAX);
        }
    }
}
