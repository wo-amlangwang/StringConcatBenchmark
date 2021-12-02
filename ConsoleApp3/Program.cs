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

        public int ListSize => 4;

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
        public string Doller()
        {
            return $"{this._strings[0]}{this._strings[1]}{this._strings[2]}{this._strings[3]}";
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
        public string Concat4()
        {
            var outString = String.Concat(this._strings[0], this._strings[1], this._strings[2], this._strings[3]);
            return outString;
        }

        [Benchmark]
        public string ConcatSpanOnlyAt3And4IgnoreThisAtOtherStage()
        {
            if (this.ListSize == 2)
            {
                return String.Concat(this._strings[0].AsSpan(), this._strings[1].AsSpan());
            }

            if (this.ListSize == 3)
            {
                return String.Concat(this._strings[0].AsSpan(), this._strings[1].AsSpan(), this._strings[2].AsSpan());
            }

            if (this.ListSize == 4)
            {
                return String.Concat(this._strings[0].AsSpan(), this._strings[1].AsSpan(), this._strings[2].AsSpan(), this._strings[3].AsSpan());
            }

            return string.Empty;
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
