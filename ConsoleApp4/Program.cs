using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [RankColumn]
    public class Program
    {

        private List<string> _strings;

        public int ListSize => 5;

        [IterationSetup]
        public void init()
        {
            _strings = new List<string>();
            for (int i = 0; i < this.ListSize; i++)
            {
                _strings.Add(Guid.NewGuid().ToString());
            }
        }

        [Benchmark]
        public string Plus()
        {
            var outString = String.Empty;
            foreach (var str in this._strings)
            {
                outString += str;
            }

            return outString;
        }

        [Benchmark]
        public string DollerForLoop()
        {
            var temp = string.Empty;
            foreach (var str in this._strings)
            {
                temp = $"{temp}{str}";
            }

            return temp;
        }

        [Benchmark]
        public string StringBuilder()
        {
            var sb = new StringBuilder(string.Empty);
            foreach (var str in this._strings)
            {
                sb.Append(str);
            }

            return sb.ToString();
        }

        [Benchmark]
        public string Concat()
        {
            var outString = String.Concat(this._strings);
            return outString;
        }

        [Benchmark]
        public string SpanWithOutStringConcat()
        {
            char[] temp = string.Empty.AsSpan().ToArray();

            foreach (var str in this._strings)
            {
                temp = temp.Concat(str.AsSpan().ToArray()).ToArray();
            }

            return new string(temp);
        }

        [Benchmark]
        public string ConcatForLoop()
        {
            var outString = String.Empty;
            foreach (var str in this._strings)
            {
                outString = String.Concat(outString, str);
            }

            return outString;
        }

        [Benchmark]
        public string ConcatSpanForLoop()
        {
            var outString = String.Empty;
            foreach (var str in this._strings)
            {
                outString = String.Concat(outString.AsSpan(), str.AsSpan());
            }

            return outString;
        }


        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }
    }
}
