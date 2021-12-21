using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class UkrDecimalNumberWordsDictionary : IDecimalNumberWordsDictionary
    {
        private bool male = false;

        private string[] hunds =
        {
        "", "сто ", "двiстi ", "триста ", "чотириста ",
        "П'ятсот ", "шiстсот ", "сiмсот ", "вiсiмсот ", "дев'ятсот "
        };

        private string[] tens =
        {
        "", "десять ", "двадцять ", "тридцять ", "сорок ", "п'ятдесят ",
        "шiстдесят ", "сiмдесят ", "вiсiмдесят ", "дев'яносто "
        };

        private string[] frac =
        {
        "", "одна ", "двi ", "три ", "чотири ", "п'ять ", "шiсть ",
        "сiм ", "вiсiм ", "дев'ять ", "десять ", "одинадцять ",
        "дванадцять ", "тринадцять ", "чотирнадцять ", "п'ятнадцять ",
        "шiстнадцять ", "сiмнадцять ", "вiсiмнадцять ", "дев'ятнадцять "
        };

        private string[] teminineGenderFrac =
        {
        "", "одна ", "двi ", "три ", "чотири ", "п'ять ", "шість ",
        "сім ", "вісім ", "дев'ять ", "десять ", "одинадцять ",
        "дванадцять ", "тринадцять ", "чотирнадцять ", "п'ятнадцять ",
        "шiстнадцять ", "сiмнадцять ", "вiсiмнадцять ", "дев'ятнадцять "
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