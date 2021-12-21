using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Framework.Transliteration
{
    /// <summary>
    ///     String transliteration
    ///     This class search file <c>Transliteration.xml</c> in assembly base directory or use file set in <c>AppSettings</c> <c>TransliterationFileName</c>
    /// </summary>
    public class Transliterator
    {
        private readonly CultureInfo defaultCulture;
        private readonly Dictionary<string, Alphabet> alphabets = new Dictionary<string, Alphabet>();
        private readonly char[] startWorkChars = new[] { ' ', ',', '.' }; //use instead of regex \b
        /// <summary>
        ///     Create Transliterator
        /// </summary>
        public Transliterator()
        {
            var fileName = ConfigurationManager.AppSettings["TransliterationFileName"];
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Transliteration.xml");
            }
            try
            {
                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(stream);

                    if (doc.Root == null)
                    {
                        throw new Exception("Root not found");
                    }
                    var alphabetsXml = doc.Root.Descendants("alphabet").ToList();

                    if (!alphabetsXml.Any())
                    {
                        throw new Exception(string.Format("Alphabets not found"));
                    }

                    foreach (var element in alphabetsXml)
                    {
                        var c = element.Attribute("culture");
                        if (c == null)
                        {
                            throw new Exception("Attribute culture not found");
                        }

                        var alphabet = new Alphabet();
                        alphabet.ParseAlphabetXml(element);
                        this.alphabets.Add(c.Value.ToLower(), alphabet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading file '{fileName}'", ex);
            }
        }
        public Transliterator(CultureInfo defaultCulture)
            : this()
        {
            if (null == defaultCulture) throw new ArgumentNullException(nameof(defaultCulture));
            this.defaultCulture = defaultCulture;
        }

        /// <summary>
        ///     Transliterate string
        /// </summary>
        public string Translite(string str, CultureInfo culture = null)
        {
            if (null == culture && null == this.defaultCulture) throw new ArgumentNullException(nameof(culture), "Culture or Default culture not initialized");

            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            var cc = culture ?? this.defaultCulture;
            Alphabet alphabet;
            if (!this.alphabets.TryGetValue(cc.Name.ToLower(), out alphabet))
            {
                throw new ArgumentOutOfRangeException(nameof(culture), $"Culture '{cc}' not found");
            }

            var text = new StringBuilder(str);
            foreach (var c in alphabet.CodesAtStart)
            {
                text.Replace(c.Native, c.Lat, 0, c.Native.Length);
                text.Replace(GetUpperCode(c.Native), GetUpperCode(c.Lat), 0, c.Native.Length);

                foreach (var startWorkChar in this.startWorkChars)
                {
                    text.Replace(startWorkChar + c.Native, startWorkChar + c.Lat);
                    text.Replace(startWorkChar + GetUpperCode(c.Native), startWorkChar + GetUpperCode(c.Lat));
                }
            }
            foreach (var c in alphabet.Codes)
            {
                text.Replace(c.Native, c.Lat);
                text.Replace(GetUpperCode(c.Native), GetUpperCode(c.Lat));
            }
            return text.ToString();
        }

        private static string GetUpperCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return string.Empty;
            }
            if (code.Length == 1)
            {
                return code.ToUpper();
            }

            var c = code.ToCharArray();
            c[0] = Char.ToUpper(c[0]);
            return new string(c);
        }

        private class Alphabet
        {
            private readonly List<Code> codes = new List<Code>();
            private readonly List<Code> codesAtStart = new List<Code>();

            public IEnumerable<Code> Codes
            {
                get { return this.codes; }
            }

            public IEnumerable<Code> CodesAtStart
            {
                get { return this.codesAtStart; }
            }

            public void ParseAlphabetXml(XContainer element)
            {
                foreach (var descendant in element.Descendants("code"))
                {
                    this.AddCode(descendant);
                }
                this.codes.Sort();
            }

            private void AddCode(XElement element)
            {
                var native = element.Attribute("native").Value;
                var lat = element.Attribute("lat").Value;
                var atStartAtt = element.Attribute("atStart");
                var atStart = atStartAtt != null ? atStartAtt.Value : null;

                this.codes.Add(new Code(native, lat));

                if (atStart != null)
                {
                    this.codesAtStart.Add(new Code(native, atStart));
                }
            }
        }

        private class Code : IComparable<Code>
        {
            public Code(string native, string lat)
            {
                this.Native = native;
                this.Lat = lat;
            }

            public string Native { get; private set; }

            public string Lat { get; private set; }


            public int CompareTo(Code other)
            {
                return other.Native.Length - this.Native.Length;
            }

            public override string ToString()
            {
                return $"Native: {this.Native}, Lat: {this.Lat}";
            }
        }
    }
}