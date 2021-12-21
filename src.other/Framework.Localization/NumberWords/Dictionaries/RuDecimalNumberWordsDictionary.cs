using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class RuDecimalNumberWordsDictionary : IDecimalNumberWordsDictionary
    {
        private bool male = true;

        private string[] hunds =
        {
        "", "сто ", "двести ", "триста ", "четыреста ",
        "пятьсот ", "шестьсот ", "семьсот ", "восемьсот ", "девятьсот "
        };

        private string[] tens =
        {
        "", "десять ", "двадцать ", "тридцать ", "сорок ", "пятьдесят ",
        "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто "
        };

        private string[] frac =
        {
        "", "один ", "два ", "три ", "четыре ", "пять ", "шесть ",
        "семь ", "восемь ", "девять ", "десять ", "одиннадцать ",
        "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ",
        "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "
        };

        private string[] teminineGenderFrac =
        {
        "", "одна ", "две ", "три ", "четыре ", "пять ", "шесть ",
        "семь ", "восемь ", "девять ", "десять ", "одиннадцать ",
        "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ",
        "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "
        };

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
            get
            {
                if (this.male)
                    return this.frac;
                else
                    return this.teminineGenderFrac;
            }
        }
    }
}
