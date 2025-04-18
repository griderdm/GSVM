﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSVM.Components.Mem;
using GSVM.Components.Processors.CPU_1;
using GSVM.Constructs;
using GSVM.Constructs.DataTypes;
using GSVM.Components.Clocks;
using GSVM.Components.Controllers;

namespace GSVM.Components.Processors
{
    public partial class CPU1 : CPU
    {
        Stack<IDataType> stack;
        public IDataType[] Stack 
        { 
            get 
            { 
                return stack.ToArray();
            } 
        }

        Dictionary<Opcodes, Delegate> opcodes;

        Registers<Register> registers;

        Opcode opcode;
        Delegate operation;

        Integral<ushort> operandA, operandB;

        public string Name { get { return "GSVM CPU1"; } }
        public double Speed { get; private set; }

        bool speedTesting = false;
        int cycleCount = 0;
        DateTime startTime;

        bool debug;
        public override bool Debug
        {
            get
            {
                return debug;
            }

            set
            {
                debug = value;
                if (debug)
                {
                    Northbridge.Clock.Stop();
                }
                else if (!Northbridge.Clock.Enabled)
                {
                    Northbridge.Clock.Start();
                }
            }
        }

        bool cycleException = false;

        public CPU1()
        {
            stack = new Stack<IDataType>();
            opcodes = new Dictionary<Opcodes, Delegate>();

            registers = new Registers<Register>();

            registers.Append<uint16_t>(Register.PC);       // Program counter
            registers.Append<uint16_t>(Register.MAR);      // Memory address register
            registers.Append<uint16_t>(Register.MLR);      // Memory length register

            registers.Append<uint64_t>(Register.MDR);      // Memory data register (64-bits)
            registers.Subdivide(Register.MDR, Register.MDR32);    // 32-bit MDR register
            registers.Subdivide(Register.MDR32, Register.MDR16);  // 16-bit MDR register
            registers.Subdivide(Register.MDR16, Register.MDR8);   // 8-bit MDR register

            registers.Append<Opcode>(Register.CIR);      // Current instruction register

            registers.Append<uint16_t>(Register.IDT);   // Interrupt Descriptor Table address

            registers.Append<uint64_t>(Register.FLAGS); // Flags register

            registers.Append<uint32_t>(Register.SVM); // Shared Video Memory address

            registers.Append<uint32_t>(Register.EAX);
            registers.Subdivide(Register.EAX, Register.AX);
            registers.Subdivide(Register.AX, Register.AL, Register.AH);

            registers.Append<uint32_t>(Register.EBX);
            registers.Subdivide(Register.EBX, Register.BX);
            registers.Subdivide(Register.BX, Register.BL, Register.BH);

            registers.Append<uint32_t>(Register.ECX);
            registers.Subdivide(Register.ECX, Register.CX);
            registers.Subdivide(Register.CX, Register.CL, Register.CH);

            registers.Append<uint32_t>(Register.EDX);
            registers.Subdivide(Register.EDX, Register.DX);
            registers.Subdivide(Register.DX, Register.DL, Register.DH);

            registers.Append<uint16_t>(Register.ABP);
            registers.Append<uint16_t>(Register.AEI);
            registers.Append<uint16_t>(Register.AEL);
            registers.Append<uint16_t>(Register.AEP);

            registers.BuildMemory();
            SetupOpCodes();

            registers.Write<uint64_t>(Register.FLAGS, new uint64_t((ulong)CPUFlags.Empty));
        }

        void SetupOpCodes()
        {
            opcodes.Add(Opcodes.nop, new Action(NoOp));
            opcodes.Add(Opcodes.movr, new Action<Register_t, Register_t>(MoveR));
            opcodes.Add(Opcodes.movl, new Action<Register_t, uint16_t>(MoveL));
            opcodes.Add(Opcodes.readr, new Action<Register_t, Register_t>(Read));
            opcodes.Add(Opcodes.readl, new Action<Register_t, uint16_t>(Read));
            opcodes.Add(Opcodes.writerr, new Action<Register_t, Register_t>(Write));
            opcodes.Add(Opcodes.writelr, new Action<uint16_t, Register_t>(Write));
            opcodes.Add(Opcodes.writell, new Action<uint16_t, uint16_t>(Write));
            opcodes.Add(Opcodes.writerl, new Action<Register_t, uint16_t>(Write));
            opcodes.Add(Opcodes.pushr, new Action<Register_t>(PushRegister));
            opcodes.Add(Opcodes.pushl, new Action<uint16_t>(PushLiteral));
            opcodes.Add(Opcodes.pusha, new Action(PushAll));
            opcodes.Add(Opcodes.pop, new Action<Register_t>(Pop));
            opcodes.Add(Opcodes.popa, new Action(PopAll));

            opcodes.Add(Opcodes.addr, new Action<Register_t, Register_t>(Add));
            opcodes.Add(Opcodes.addl, new Action<Register_t, uint16_t>(Add));
            opcodes.Add(Opcodes.subr, new Action<Register_t, Register_t>(Subtract));
            opcodes.Add(Opcodes.subl, new Action<Register_t, uint16_t>(Subtract));
            opcodes.Add(Opcodes.multr, new Action<Register_t, Register_t>(Multiply));
            opcodes.Add(Opcodes.multl, new Action<Register_t, uint16_t>(Multiply));
            opcodes.Add(Opcodes.divr, new Action<Register_t, Register_t>(Divide));
            opcodes.Add(Opcodes.divl, new Action<Register_t, uint16_t>(Divide));
            opcodes.Add(Opcodes.modr, new Action<Register_t, Register_t>(Mod));
            opcodes.Add(Opcodes.modl, new Action<Register_t, uint16_t>(Mod));
            opcodes.Add(Opcodes.andr, new Action<Register_t, Register_t>(And));
            opcodes.Add(Opcodes.andl, new Action<Register_t, uint16_t>(And));
            opcodes.Add(Opcodes.orr, new Action<Register_t, Register_t>(Or));
            opcodes.Add(Opcodes.orl, new Action<Register_t, uint16_t>(Or));
            opcodes.Add(Opcodes.xorr, new Action<Register_t, Register_t>(Xor));
            opcodes.Add(Opcodes.xorl, new Action<Register_t, uint16_t>(Xor));
            opcodes.Add(Opcodes.not, new Action<Register_t>(Not));
            opcodes.Add(Opcodes.neg, new Action<Register_t>(Neg));
            opcodes.Add(Opcodes.cmpr, new Action<Register_t, Register_t>(Compare));
            opcodes.Add(Opcodes.cmpl, new Action<Register_t, uint16_t>(Compare));
            opcodes.Add(Opcodes.lsr, new Action<Register_t, Register_t>(LeftShift));
            opcodes.Add(Opcodes.lsl, new Action<Register_t, uint16_t>(LeftShift));
            opcodes.Add(Opcodes.rsr, new Action<Register_t, Register_t>(RightShift));
            opcodes.Add(Opcodes.rsl, new Action<Register_t, uint16_t>(RightShift));

            opcodes.Add(Opcodes.hlt, new Action(Halt));

            opcodes.Add(Opcodes._out, new Action(_out));
            opcodes.Add(Opcodes._in, new Action(_in));
            opcodes.Add(Opcodes.intr, new Action<Register_t>(intr));
            opcodes.Add(Opcodes.intl, new Action<uint16_t>(intl));

            opcodes.Add(Opcodes.jmpr, new Action<Register_t>(JumpR));
            opcodes.Add(Opcodes.jmpl, new Action<uint16_t>(JumpL));
            opcodes.Add(Opcodes.je, new Action<uint16_t>(JumpEqual));
            opcodes.Add(Opcodes.jne, new Action<uint16_t>(JumpNotEqual));
            opcodes.Add(Opcodes.jg, new Action<uint16_t>(JumpGreater));
            opcodes.Add(Opcodes.jge, new Action<uint16_t>(JumpGreaterEqual));
            opcodes.Add(Opcodes.jl, new Action<uint16_t>(JumpLess));
            opcodes.Add(Opcodes.jle, new Action<uint16_t>(JumpLessEqual));
            opcodes.Add(Opcodes.callr, new Action<Register_t>(CallR));
            opcodes.Add(Opcodes.calll, new Action<uint16_t>(CallL));
            opcodes.Add(Opcodes.ret, new Action(Ret));

            opcodes.Add(Opcodes.derefr, new Action<Register_t, Register_t>(Deref));
            opcodes.Add(Opcodes.derefl, new Action<Register_t, uint16_t>(Deref));

            opcodes.Add(Opcodes.brk, new Action(Brk));

            opcodes.Add(Opcodes.cpuid, new Action(CPUID));

            opcodes.Add(Opcodes.sall, new Action<uint16_t, uint16_t>(sall));
            opcodes.Add(Opcodes.salr, new Action<uint16_t, Register_t>(salr));
            opcodes.Add(Opcodes.sarl, new Action<Register_t, uint16_t>(sarl));
            opcodes.Add(Opcodes.sarr, new Action<Register_t, Register_t>(sarr));
            opcodes.Add(Opcodes.inca, new Action(inca));
            opcodes.Add(Opcodes.deca, new Action(deca));
            opcodes.Add(Opcodes.sail, new Action<uint16_t>(sail));
            opcodes.Add(Opcodes.sair, new Action<Register_t>(sair));
        }

        public override byte[] GetRegisters()
        {
            return registers.Dump();
        }

        protected override void Fetch()
        {
            cycleException = false;
            if (speedTesting)
                cycleCount++;

            if (!HasFlag(CPUFlags.WaitForInterrupt))
            {
                uint svm = registers.Read<uint32_t>(Register.SVM).Value;
                if (Northbridge.videoMemoryAddress != svm)
                    Northbridge.videoMemoryAddress = svm;

                MoveR(Register.MAR, Register.PC);
                MoveL(Register.MLR, 8);
                ReadMemory();
                MoveR(Register.CIR, Register.MDR);

                Add(Register.PC, Register.MLR);

                Parent.RaiseUpdateDebugger();
            }
        }

        protected override void Decode()
        {
            if (!cycleException)
            {
                opcode = registers.Read<Opcode>(Register.CIR);
                try
                {
                    operation = opcodes[opcode.Code];
                }
                catch (KeyNotFoundException)
                {
                    cycleException = true;
                    // TODO: Interrupt with an invalid opcode
                }

                if (opcode.Flags.HasFlag(OpcodeFlags.Register1))
                    operandA = new Register_t(opcode.OperandA);

                if (opcode.Flags.HasFlag(OpcodeFlags.Register2))
                    operandB = new Register_t(opcode.OperandB);

                if (opcode.Flags.HasFlag(OpcodeFlags.Literal1))
                    operandA = opcode.OperandA;

                if (opcode.Flags.HasFlag(OpcodeFlags.Literal2))
                    operandB = opcode.OperandB;
            }
        }

        protected override void Execute()
        {
            if (!cycleException)
            {
                int arguments = operation.Method.GetParameters().Length;

                // TODO: Wrap in try-catch
                try
                {
                    switch (arguments)
                    {
                        case 0:
                            operation.DynamicInvoke();
                            break;

                        case 1:
                            operation.DynamicInvoke(operandA);
                            break;

                        case 2:
                            operation.DynamicInvoke(operandA, operandB);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Northbridge.WriteDisplay(0, Encoding.ASCII.GetBytes(ex.Message));
                }

                Parent.RaiseUpdateDebugger();

                if ((speedTesting) && (cycleCount == 5))
                {
                    TimeSpan time = DateTime.Now - startTime;
                    Speed = 5.0f / time.TotalSeconds;
                    speedTesting = false;
                }
            }
        }

        public override void Start()
        {
            Enabled = true;
            Northbridge.Clock.Start();
            speedTesting = true;
            startTime = DateTime.Now;
        }

        public override void Stop()
        {
            Enabled = false;
        }

        void NoOp()
        {
            // DO NOTHING
        }

        void Halt()
        {
            SetFlag(CPUFlags.WaitForInterrupt);
        }

        public override void Interrupt(int channel)
        {
            Interrupt((ushort)channel);
        }

        void Interrupt(ushort interrupt)
        {
            // Get the interrupt address
            MoveR(Register.MAR, Register.IDT);
            Add(Register.MAR, new uint16_t((ushort)(interrupt * 2)));
            MoveL(Register.MLR, 4);
            ReadMemory();

            //// call that address as a function
            ushort address = registers.Read<uint16_t>(Register.MDR16);
            CallR(Register.MDR16);
        }

        void CPUID()
        {
            uint32_t eax = registers.Read<uint32_t>(Register.EAX);
            uint32_t ebx, ecx, edx;
            byte[] str = new byte[4];
            switch (eax.Value)
            {
                case 0:
                    List<byte> name = new List<byte>(Encoding.ASCII.GetBytes(Name));
                    while (name.Count < 12) { name.Add(32); }
                    while (name.Count > 12) { name.RemoveAt(name.Count - 1); }

                    byte[] bName = name.ToArray();
                    Array.Copy(bName, str, 4);
                    ebx = new uint32_t(str);
                    Array.Copy(bName, 4, str, 0, 4);
                    ecx = new uint32_t(str);
                    Array.Copy(bName, 8, str, 0, 4);
                    edx = new uint32_t(str);

                    registers.Write(Register.EBX, ebx);
                    registers.Write(Register.ECX, ecx);
                    registers.Write(Register.EDX, edx);
                    break;

                case 1:
                    List<byte> speed = new List<byte>(Encoding.ASCII.GetBytes(string.Format("{0:F2} Hz\0", Speed)));
                    while (speed.Count < 12) { speed.Add(32); }
                    while (speed.Count > 12) { speed.RemoveAt(speed.Count - 1); }

                    byte[] bSpeed = speed.ToArray();
                    Array.Copy(bSpeed, str, 4);
                    ebx = new uint32_t(str);
                    Array.Copy(bSpeed, 4, str, 0, 4);
                    ecx = new uint32_t(str);
                    Array.Copy(bSpeed, 8, str, 0, 4);
                    edx = new uint32_t(str);

                    registers.Write(Register.EBX, ebx);
                    registers.Write(Register.ECX, ecx);
                    registers.Write(Register.EDX, edx);
                    break;
            }
        }

        void SetFlag(CPUFlags flag)
        {
            CPUFlags state = (CPUFlags)registers.Read<uint64_t>(Register.FLAGS).Value;
            state = state | flag;
            registers.Write<uint64_t>(Register.FLAGS, (ulong)state);
        }

        void UnsetFlag(CPUFlags flag)
        {
            CPUFlags state = (CPUFlags)registers.Read<uint64_t>(Register.FLAGS).Value;
            state = state & ~flag;
            registers.Write<uint64_t>(Register.FLAGS, (ulong)state);
        }

        bool HasFlag(CPUFlags flag)
        {
            CPUFlags state = (CPUFlags)registers.Read<uint64_t>(Register.FLAGS).Value;
            return (state & flag) == flag;
        }

        protected void ReadMemory()
        {
            if (Parent == null)
                throw new Exception("Not connected to VM. Parent is null.");
            if (Northbridge == null)
                throw new Exception("Not connected to northbridge.");

            uint16_t address = registers.Read<uint16_t>(Register.MAR);
            uint16_t length = registers.Read<uint16_t>(Register.MLR);
            try
            {
                byte[] value = Northbridge.ReadMemory(address.Value, length.Value);
                registers.Write(Register.MDR, value);
            }
            catch (IndexOutOfRangeException)
            {
                SetFlag(CPUFlags.MemoryError);
            }
        }

        protected void WriteMemory()
        {
            if (Parent == null)
                throw new Exception("Not connected to VM. Parent is null.");
            if (Northbridge == null)
                throw new Exception("Not connected to northbridge.");

            uint16_t address = registers.Read<uint16_t>(Register.MAR);
            uint16_t length = registers.Read<uint16_t>(Register.MLR);

            byte[] value = new byte[0];

            switch (length.Value)
            {
                case 8:
                    value = registers.Read(Register.MDR);
                    break;

                case 4:
                    value = registers.Read(Register.MDR32);
                    break;

                case 2:
                    value = registers.Read(Register.MDR16);
                    break;

                case 1:
                    value = registers.Read(Register.MDR8);
                    break;
            }

            try
            {
                Northbridge.WriteMemory(address, value, (uint)value.Length);
            }
            catch (MemoryAccessException)
            {
                // Raise interrupt
                SetFlag(CPUFlags.MemoryError);
            }
        }
    }
}
