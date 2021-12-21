using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class EnDecimalNumberWordsDictionary : IDecimalNumberWordsDictionary
    {
        private bool male;

        private string[] hunds =
                         { "", "one hundred ", "two hundred ", "three hundred ", "four hundred ","five hundred ",
                           "six hundred ", "seven hundred ", "eight hundred ", "nine hundred " };

        private string[] tens =
                         { "", "ten ", "twenty ", "thirty ", "forty ", "fifty ",
                           "sixty ", "seventy ", "eighty ", "ninety " };

        private string[] frac =
                         { "", "one ", "two ", "three ", "four ", "five ", "six ",
                           "seven ", "eight ", "nine ", "ten ", "eleven ",
                           "twelve ", "thrirteen ", "fourteen ", "fiveteen ",
                           "sixteen ", "seventeen ", "eighteen ", "nineteen " };

        public bool Male
        {
            get { return this.male; }
            set { this.male = value; }
        }

        public string[] Hunds
        {
            get { return this.hunds; }
        }

        public string[] Tens
        {
            get { return this.tens; }
        }

        public string[] Frac
        {
            get { return this.frac; }
        }
    }
}
