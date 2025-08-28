using System.Globalization;

namespace Framework.Core;

public partial struct Period
{
    /// <summary>
    /// Преобразует дату начала периода в строковое представление месяца и года в предложном падеже для Российского стандарта
    /// </summary>
    /// <returns>Строка, содержащая полное имя месяца и год в предложном падеже</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetInMonthAndYearStrRus instead")]
    public string GetInMonthAndYearStrRus()
    {
        return CommonPeriodExtensions.GetInMonthAndYearStrRus(this);
    }

    /// <summary>
    /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом для Российского стандарта
    /// </summary>
    /// <returns>Строка, содержащая содержащая имя месяца и год если период в одном месяце, иначе содержащяя имя начального и конечного месяца с годом</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetMonthAndYearStrRus instead")]
    public string GetMonthAndYearStrRus()
    {
        return CommonPeriodExtensions.GetMonthAndYearStrRus(this);
    }

    /// <summary>
    /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом
    /// </summary>
    /// <returns>Строка, содержащая содержащая имя месяца и год если период в одном месяце, иначе содержащяя имя начального и конечного месяца с годом</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetMonthAndYearStr instead")]
    public string GetMonthAndYearStr()
    {
        return CommonPeriodExtensions.GetMonthAndYearStr(this);
    }

    /// <summary>
    /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом для Римского стандарта
    /// </summary>
    /// <returns>Строка, содержащая содержащая римский номер месяца и год если период в одном месяце, иначе содержащяя римские номера начального и конечного месяца с годом</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetMonthAndYearStrRoman instead")]
    public string GetMonthAndYearStrRoman()
    {
        return CommonPeriodExtensions.GetMonthAndYearStrRoman(this);
    }

    /// <summary>
    /// Возвращает неделю года, к которой относится дата начала периода для текущего стандарта
    /// </summary>
    /// <returns>Положительное целое число, представляющее неделю года, к которой относится дата начала периода для текущего стандарта</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetWeekNumber instead")]
    public int GetWeekNumber()
    {
        return CommonPeriodExtensions.GetWeekNumber(this);
    }

    /// <summary>
    /// Возвращает неделю года, к которой относится дата начала периода для <see cref="isoCultureName"/> стандарта
    /// </summary>
    /// <param name="isoCultureName">Предварительно определенное имя CultureInfo, существующего объекта CultureInfo или имя языка и региональных параметров, свойственных только Windows, не учитывая регистр</param>
    /// <returns>Положительное целое число, представляющее неделю года, к которой относится дата начала периода для указанного стандарта</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetWeekNumber instead")]
    public int GetWeekNumber(string isoCultureName)
    {
        return CommonPeriodExtensions.GetWeekNumber(this, isoCultureName);
    }

    /// <summary>
    /// Преобразует дату начала и конца периода в короткое строковое описание интервала представленый месяцами и годом
    /// </summary>
    /// <returns>Строка, содержащая содержащая номер месяца и год если период в одном месяце, иначе содержащяя намера начального и конечного месяца с годом</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.ToExtraShortString instead")]
    public string ToExtraShortString()
    {
        return CommonPeriodExtensions.ToExtraShortString(this);
    }

    /// <summary>
    /// Преобразует дату начала и конца периода в короткое строковое описание интервала представленый месяцами и годом для <see cref="cultureinfo"/> региональных параметров
    /// </summary>
    /// <param name="cultureinfo">Предоставляет сведения об определенном языке и региональных параметрах используемое для форматирование дат периода</param>
    /// <returns>Строка, содержащая содержащая номер месяца и год если период в одном месяце, иначе содержащяя намера начального и конечного месяца с годом</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.ToExtraShortString instead")]
    public string ToExtraShortString(CultureInfo cultureinfo)
    {
        return CommonPeriodExtensions.ToExtraShortString(this, cultureinfo);
    }

    /// <summary>
    /// Возвращает формат даты для <see cref="cultureinfo"/> региональных параметров содержащий только месяц и год
    /// </summary>
    /// <param name="cultureinfo">Предоставляет сведения об определенном языке и региональных параметрах используемое для форматирование дат периода</param>
    /// <returns>Строка, представляющая формат даты, в указанных региональных параметрах, для вывода только месяца и года</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetDayAndMonthDateFormat instead")]
    public string GetDayAndMonthDateFormat(CultureInfo cultureinfo)
    {
        return CommonPeriodExtensions.GetDayAndMonthDateFormat(this, cultureinfo);
    }
}
