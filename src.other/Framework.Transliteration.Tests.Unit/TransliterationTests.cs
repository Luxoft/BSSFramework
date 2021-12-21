using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Framework.Transliteration.Test
{
    [TestFixture]
    public class TransliterationTests
    {

        [Test]
        [Ignore("Used for local hand running")]
        public void TestThread()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 100000; i++)
            {
                int i1 = i;
                tasks.Add(new Task(() =>
                                      {
                                          var ukr2 = new Transliterator();
                                          GC.KeepAlive(ukr2);
                                          Console.WriteLine(i1);
                                      }));
            }
            tasks.ForEach(x=>x.Start());
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void TestUA()
        {
            var ua = new CultureInfo("uk-UA");
            var ukr = new Transliterator();

            Replace(ukr, ua, "Alushta", ("Алушта"));
            Replace(ukr, ua, "Andrii", ("Андрій"));
            Replace(ukr, ua, "Borshchahivka", ("Борщагівка"));
            Replace(ukr, ua, "Borysenko", ("Борисенко"));
            Replace(ukr, ua, "Vinnytsia", ("Вінниця"));
            Replace(ukr, ua, "Volodymyr", ("Володимир"));
            Replace(ukr, ua, "Hadiach", ("Гадяч"));
            Replace(ukr, ua, "Bohdan", ("Богдан"));
            Replace(ukr, ua, "Zghurskyi", ("Згурський"));
            Replace(ukr, ua, "Galagan", ("Ґалаґан"));
            Replace(ukr, ua, "Gorgany", ("Ґорґани"));
            Replace(ukr, ua, "Donetsk", ("Донецьк"));
            Replace(ukr, ua, "Dmytro", ("Дмитро"));
            Replace(ukr, ua, "Rivne", ("Рівне"));
            Replace(ukr, ua, "Oleh", ("Олег"));
            Replace(ukr, ua, "Esman", ("Есмань"));
            Replace(ukr, ua, "Yenakiieve", ("Єнакієве"));
            Replace(ukr, ua, "Haievych", ("Гаєвич"));
            Replace(ukr, ua, "Koropie", ("Короп'є"));
            Replace(ukr, ua, "Zakarpattia", ("Закарпаття"));
            Replace(ukr, ua, "Yizhakevych", ("Їжакевич"));
            Replace(ukr, ua, "Kadyivka", ("Кадиївка"));
            Replace(ukr, ua, "Yosypivka", ("Йосипівка"));
            Replace(ukr, ua, "Stryi", ("Стрий"));
            Replace(ukr, ua, "Reshetylivka", ("Решетилівка"));
            Replace(ukr, ua, "Bila Tserkva", ("Біла Церква"));
            Replace(ukr, ua, "Yurii", ("Юрій"));
            Replace(ukr, ua, "Koriukivka", ("Корюківка"));
            Replace(ukr, ua, "Yahotyn", ("Яготин"));
            Replace(ukr, ua, "Kostiantyn", ("Костянтин"));
            Replace(ukr, ua, "Znamianka", ("Знам'янка"));
            Replace(ukr, ua, "Feodosiia", ("Феодосія"));
            Replace(ukr, ua, "Zghorany", ("Згорани"));
        }

        [Test]
        public void TestRU()
        {
            var ru = new CultureInfo("ru-RU");
            var t = new Transliterator();
            Console.WriteLine("Привет - " + t.Translite("Привет", ru));
            Console.WriteLine("Привет Мир - " + t.Translite("Привет Мир", ru));
            Console.WriteLine("Hello Мир - " + t.Translite("Привет Мир", ru));
        }
        [Test]
        public void TestRU_DefaultCulture()
        {
            var t = new Transliterator(new CultureInfo("ru-RU"));
            Console.WriteLine("Привет - " + t.Translite("Привет"));
            Console.WriteLine("Привет Мир - " + t.Translite("Привет Мир"));
            Console.WriteLine("Hello Мир - " + t.Translite("Привет Мир"));
        }
        private static void Replace(Transliterator t, CultureInfo info, string lat, string nat)
        {
            Console.WriteLine("{0} -> {1}", nat, lat);
            Assert.AreEqual(lat, t.Translite(nat, info));
        }
    }
}
