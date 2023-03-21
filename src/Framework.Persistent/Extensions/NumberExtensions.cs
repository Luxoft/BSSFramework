using System;

namespace Framework.Persistent;

public static class NumberExtensions
{
    public const int MoneyRoundDecimals = 2;

    public const int CoeffRoundDecimals = 4;

    public const int PercentRoundDecimals = 2;

    public static bool IsEqualsMoney(this decimal value1, decimal value2)
    {
        return (value1 - value2).IsNullMoney();
    }

    public static bool IsNullMoney(this decimal value)
    {
        return Math.Abs(value).RoundMoney() == 0;
    }

    public static decimal RoundMoney(this decimal value)
    {
        return decimal.Round(value, MoneyRoundDecimals);
    }
    public static decimal? RoundMoney(this decimal? value)
    {
        if (value == null)
        {
            return null;
        }
        return decimal.Round(value.Value, MoneyRoundDecimals);
    }

    public static decimal RoundCoeff(this decimal value)
    {
        return decimal.Round(value, CoeffRoundDecimals);
    }

    //public static double RoundCoeff(this double value)
    //{
    //    return Math.Round(value, CoeffRoundDecimals);
    //}

    public static decimal RoundPercent(this decimal value)
    {
        return decimal.Round(value, PercentRoundDecimals);
    }
    public static decimal? RoundPercent(this decimal? value)
    {
        if (value == null)
        {
            return null;
        }
        return decimal.Round(value.Value, PercentRoundDecimals);
    }


    public static decimal AwayFromZeroRound (this decimal value, int rank)
    {
        return decimal.Round(value, rank, MidpointRounding.AwayFromZero);
    }
}
