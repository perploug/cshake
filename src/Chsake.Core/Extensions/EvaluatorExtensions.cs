using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.CSharp;

namespace Cshake.Core.Extensions
{
    public static class EvaluatorExtensions
    {
        public static void Run(this string code, bool repQuotes = false)
        {
            var run = repQuotes ?
                      code.Replace("'", "\"") : code;
            Evaluator.Run(run);
        }
    }  
}
