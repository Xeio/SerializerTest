﻿using BenchmarkDotNet.Running;

namespace SerializerBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }        
    }
}
