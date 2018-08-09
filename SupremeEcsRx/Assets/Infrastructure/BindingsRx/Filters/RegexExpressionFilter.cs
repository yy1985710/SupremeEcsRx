using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniRx;

namespace BindingsRx.Filters
{
    public class RegexExpressionFilter : IFilter<string>
    {
        public string RegexExpression { get; set; }
        public ReactiveProperty<bool> Result { get; set; }

        public RegexExpressionFilter()
        {
            Result = new ReactiveProperty<bool>();
        }

        public IObservable<string> InputFilter(IObservable<string> inputStream)
        {
            return inputStream;
        }

        public IObservable<string> OutputFilter(IObservable<string> outputStream)
        {
            return outputStream.DistinctUntilChanged().Select(s =>
            {
                if (s == null)
                {
                    return "";
                }
                Result.Value = Regex.IsMatch(s, RegexExpression);
                return Regex.Match(s, RegexExpression).Value;
            });
        }
    }
}
