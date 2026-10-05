using System;
using System.IO;
using Iced.Intel;

class Program
{
    static string exePath = @"C:\Program Files (x86)\Auran\TS2010\bin\trainz.exe";

    static void Main()
    {
        byte[] exeBytes = File.ReadAllBytes(exePath);
        uint imageBase = 0x400000;
        int start = 0x46E1E0;
        int len = 0x150;

        var codeReader = new ByteArrayCodeReader(exeBytes, start, len);
        var decoder = Decoder.Create(32, codeReader);
        decoder.IP = (ulong)(imageBase + start);

        var formatter = new NasmFormatter();
        var output = new StringOutput();

        while (codeReader.CanReadByte)
        {
            decoder.Decode(out var instr);
            output.Reset();
            formatter.Format(instr, output);
            Console.WriteLine($"{instr.IP:X8}  {output.ToStringAndReset()}");
        }
    }
}
