using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ddd_valueObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var codes = args[0] == null ? 
                    new List<string>() { "01" } : 
                    args[0].Split(",").ToList();

                var entity = new Entity()
                {
                    Codes = "01;020",
                    ValueObjectCodes = new Codes(codes)
                };

                Console.WriteLine($"Action codes valid: {entity.ValueObjectCodes}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

    class Entity
    {
        public string Codes { get; set; }
        public Codes ValueObjectCodes { get; set; }
    }

    public class Codes
    {
        private readonly string _regex = @"((^RAN$|^\d{2}$)|^((RAN;)|(\d{2};))+(RAN|\d{2})$)|(^$)";
        private readonly IEnumerable<string> _codes;

        public Codes(IEnumerable<string> codes)
        {
            Validate(codes);
            this._codes = codes;
        }

        private void Validate(IEnumerable<string> codes)
        {
            if (codes is not IEnumerable<string> values)
            {
                return;
            }

            var invalidValues = values.Where(s => !Regex.IsMatch(s, _regex));
            if (invalidValues.Any())
                throw new ArgumentException($"Invalid action codes {string.Join(",", invalidValues)}");
        }

        public override string ToString()
        {
            return string.Join(";", _codes);
        }
    }
}
