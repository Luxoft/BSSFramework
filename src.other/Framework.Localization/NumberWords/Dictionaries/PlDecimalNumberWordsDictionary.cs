using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class PlDecimalNumberWordsDictionary: IDecimalNumberWordsDictionary
    {
        private bool male=true;

        private string[] hunds=
            {
                "", "sto ", "dwieście ", "trzysta ", "czterysta ",
                "pięćset ", "sześćset ", "siedemset ", "osiemset ", "dziewięćset "
            };

        private string[] tens=
            {
                "", "ten ", "dwadzieścia ", "trzydzieści ", "czterdzieści ", "pół ",
                 "sześćdziesiąt ", "siedemdziesiąt ", "osiemdziesiąt ", "dziewięćdziesiąt "
            };

        private string[] frac=
            {
                "", "jeden ", "dwa ", "trzy ", "cztery ", "pięć ", "sześć ",
                 "siedem ", "osiem ", "dziewięć ", "dziesięć ", "jedenaście ",
                 "dwanaście ", "trzynaście ", "czternaście ", "piętnaście ",
                 "szesnaście ", "siedemnaście ", "osiemnaście ", "dziewiętnaście "
            };

        private string[] teminineGenderFrac=
            {
                "", "jeden ", "dwa ", "trzy ", "cztery ", "pięć ", "sześć ",
                "siedem ", "osiem ", "dziewięć ", "dziesięć ", "jedenaście ",
                "dwanaście ", "trzynaście ", "czternaście ", "piętnaście ",
                "szesnaście ", "siedemnaście ", "osiemnaście ", "dziewiętnaście "
            };

        public bool Male {
            get {
            return this.male;
            }
            set {
                this.male=value;
            }
        }

        public string[] Hunds {
            get {
            return this.hunds;
            }
        }

        public string[] Tens {
            get {
            return this.tens;
            }
        }

        public string[] Frac {
            get {
            if (this.male)
                return this.frac;
            else
                return this.teminineGenderFrac;
            }
        }
    }
}