using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Template
{

    public sealed class SimpleTemplate : ITemplate
    {

        private const string PatternText = @"(\$\{([a-zA-Z0-9_]*)\})";
        
        private static readonly Regex Pattern = new Regex(PatternText, RegexOptions.CultureInvariant);

        private readonly string _source;
        private readonly LinkedList<SimpleSegment> _segments = new LinkedList<SimpleSegment>();

        public SimpleTemplate(string input)
        {
            _source = input;
        }

        public string Generate(string input)
        {
            return string.Empty;
        }

        public string Generate(string input, TemplateContext context)
        {
            foreach (var segment in _segments)
            {
                if (segment.IsExpression)
                {

                }
                else
                {

                }
            }
            return string.Empty;
        }

        private void AddText(string value)
        {
            _segments.AddLast(SimpleSegment.Of(value));
        }

        private void AddExpression(string value)
        {
            _segments.AddLast(SimpleSegment.Of(value, true));
        }

        public static SimpleTemplate Of(string input)
        {
            var template = new SimpleTemplate(input);
            if(string.IsNullOrEmpty(input))
            {
                return template;
            }

            var match = Pattern.Match(input);
            if(!match.Success)
            {
                template.AddText(input);
            }

            var start = 0;
            while(match.Success)
            {
                var outer = match.Groups[1];
                var inner = match.Groups[2];

                if(outer.Index > start)
                {
                    template.AddText(input.Substring(start, outer.Index - start));
                }

                template.AddExpression(input.Substring(inner.Index, inner.Length));
                start = outer.Index + outer.Length;
                match = match.NextMatch();
            }

            return template;
        }

        internal class SimpleSegment
        {
            public bool IsExpression
            {
                get; set;
            }

            public string Text
            {
                get; set;
            }

            public static SimpleSegment Of(string value)
            {
                return Of(value, false);
            }

            public static SimpleSegment Of(string value, bool isExpression)
            {
                return new SimpleSegment
                {
                    Text = value,
                    IsExpression = isExpression
                };
            }

        }

    }

}
