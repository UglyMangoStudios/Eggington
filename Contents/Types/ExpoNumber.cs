using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace Eggington.Contents.Types;

public class ExpoNumberValueComparer : ValueComparer<ExpoNumber>
{
    public ExpoNumberValueComparer() :
        base(
            (l, r) => l != null && r != null && l.ToString(false) == r.ToString(false),
            v => v == null ? 0 : v.ToString(false).GetHashCode())
    {

    }
}

public class ExpoNumberValueConverter : ValueConverter<ExpoNumber, string>
{
    public ExpoNumberValueConverter()
    : base(number => number.ToString(false),
          asString => ExpoNumber.Parse(asString))
    { }
}


/// <summary>
/// An exponent numbering system that allows for extremely large numbers for a fraction of memory.
/// </summary>
public struct ExpoNumber
{

    private static readonly string[] EXPONENTS =
    {
        "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
    };

    /// <summary>
    /// Constant that stores the max set threshold
    /// </summary>
    private static readonly int MAX_EXPONENT = EXPONENTS.Length - 1;


    public static readonly ExpoNumber MIN = new(0, 0);
    public static readonly ExpoNumber MAX = new(1000, MAX_EXPONENT);

    /// <summary>
    /// Constant that controls the exponent difference when doing arithmetic. If the difference exceeds this constant,
    /// then the operation will be ignored. This saves on time when performing something like 2 Nonillion + 10. There's no need to perform
    /// that operation
    /// </summary>
    public const int MAX_THRESHOLD_DIFFERENCE = 5;

    /// <summary>
    /// Used for random operations within the class
    /// </summary>
    private static readonly Random RANDOM = new();
    

    /// <summary>
    /// A number 0-1000 that stores the base value
    /// </summary>
    private double baseNumber = 0;

    /// <summary>
    /// The threshold enum that stores the exponent. i.e. million, billion, trillion... etc.
    /// </summary>
    private int exponent = 0;

    /// <summary>
    /// Creates an exponential number
    /// </summary>
    /// <param name="baseNumber">The base value of the number</param>
    /// <param name="thousandExponent">The exponent of the number. 1000^x where x is a non-negative integer. </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ExpoNumber(double baseNumber, int thousandExponent)
    {
        if (thousandExponent < 0)
            throw new ArgumentOutOfRangeException(nameof(thousandExponent), "Value must be greater or equal to 0");
        else if (thousandExponent > MAX_EXPONENT)
            throw new ArgumentOutOfRangeException(nameof(thousandExponent), "Value must be less than or equal to " + MAX_EXPONENT);

        this.baseNumber = baseNumber;
        exponent = thousandExponent;

        //Update the number to something that fits
        Update();
    }

    public ExpoNumber() : this(0, 0) { }

    /// <summary>
    /// Updates this object so the base number always sits between 1 and 1000. The exponent threshold updates accordingly
    /// </summary>
    private void Update()
    {
        //Take the absolute value so this supports negative numbers
        while (Math.Abs(baseNumber) >= 1000 && exponent < MAX_EXPONENT)
        {
            baseNumber /= 1000;
            exponent++;
        }

        while (Math.Abs(baseNumber) < 1 && exponent > 0)
        {
            baseNumber *= 1000;
            exponent--;
        }

    }

    /// <summary>
    /// Converts this object into a string
    /// </summary>
    /// <returns>The generated string</returns>
    public override string ToString() => ToString(false);

    /// <summary>
    /// Converts this object into a string
    /// </summary>
    /// <param name="asExponential">If true, this object will display in exponential notation. Otherwise the standard suffixes will be used</param>
    /// <returns>The generated string</returns>
    public string ToString(bool asExponential = false)
    {
        string numberString = string.Format(new CultureInfo("en-US"), ConvertBaseNumberToString());

        if (asExponential)
        {
            //EX: 68e19 or 17e1
            return numberString + "e" + (exponent * 3);
        }

        //EX: 68a or 18
        return numberString + EXPONENTS[exponent];
    }

    /// <summary>
    /// Attempts to parse the provided string into an exponential number
    /// </summary>
    /// <param name="s">The string to parse</param>
    /// <returns>The generated expo number</returns>
    public static ExpoNumber Parse(string s)
    {
        //Trim the string to remove any whitespaces
        s = s.Trim().Replace(" ", "");

        //If the string is a simple number
        if (double.TryParse(s, out double parse))
            return new ExpoNumber(parse, 0);

        //Utilize the fact the suffix is only one character long
        double number = double.Parse(s[..^1]);
        int exponent = EXPONENTS.ToList().IndexOf("" + s[^1]);

        return new ExpoNumber(number, exponent);
    }

    /// <summary>
    /// Attempts to parse a string into an <see cref="ExpoNumber"/> returning the status of the parse as a boolean.
    /// </summary>
    /// <param name="s">The string to parse</param>
    /// <param name="expoNumber">The created number</param>
    /// <returns>True/False. If true, the parse was successful</returns>
    public static bool TryParse(string? s, out ExpoNumber? expoNumber)
    {
        if (s is null)
        {
            expoNumber = null;
            return false;
        }

        try
        {
            expoNumber = ExpoNumber.Parse(s);
            return true;
        }
        catch (Exception)
        {
            expoNumber = null;
            return false;
        }
    }

    /// <summary>
    /// Generates a random <see cref="ExpoNumber"/> with the provided bounds
    /// </summary>
    /// <param name="lowerBound">The lower bound</param>
    /// <param name="upperBound">The upper bound</param>
    /// <returns>The randomly generated number</returns>
    public static ExpoNumber RandomNext(ExpoNumber lowerBound, ExpoNumber upperBound)
    {
        if (lowerBound > upperBound)
            throw new ArgumentOutOfRangeException(nameof(upperBound), "Upper bound cannot be less than lower bound!");
        else if (lowerBound < 0)
            throw new ArgumentOutOfRangeException(nameof(lowerBound), "Lower bound cannot be less than 0");
        else if (upperBound < 0)
            throw new ArgumentOutOfRangeException(nameof(upperBound), "Upper bound cannot be less than 0");

        int randomExponent = RANDOM.Next(lowerBound.exponent, upperBound.exponent + 1);
        double randomBaseValue = nextDouble(RANDOM, lowerBound.baseNumber, upperBound.baseNumber);

        return new(randomBaseValue, randomExponent);
    }
    private static double nextDouble(Random random, double min, double max)
         => random.NextDouble() * (max - min) + min;

    /// <summary>
    /// Converts the base number into a formatted string
    /// </summary>
    /// <returns>The converted string</returns>
    private string ConvertBaseNumberToString()
    {
        if (baseNumber < 10) return $"{baseNumber:N2}";
        else if (baseNumber < 100) return $"{baseNumber:N1}";
        else return $"{baseNumber:N1}";
    }


    public override bool Equals(object? obj) => obj is ExpoNumber number && this == number;

    public override int GetHashCode() => HashCode.Combine(baseNumber, exponent);


    public static explicit operator ExpoNumber(string s) => Parse(s);

    public static implicit operator ExpoNumber(double d) => new(d, 0);

    public static implicit operator ExpoNumber((double value, int threshold) t) => new(t.value, t.threshold);

    public static ExpoNumber operator *(ExpoNumber a, ExpoNumber b)
    {
        double numbersMultiplied = a.baseNumber * b.baseNumber;
        int thresholdsAdd = a.exponent + b.exponent;

        return new(numbersMultiplied, thresholdsAdd);
    }

    public static ExpoNumber operator /(ExpoNumber a, ExpoNumber b)
    {
        double numbersDivided = a.baseNumber / b.baseNumber;
        int thresholdsSubtracted = a.exponent - b.exponent;

        return new(numbersDivided, thresholdsSubtracted);
    }

    public static ExpoNumber operator %(ExpoNumber a, ExpoNumber b)
    {
        if (b > a) return a;

        ExpoNumber quotient = a / b;
        ExpoNumber quotientFloored = new(Math.Floor(quotient.baseNumber), quotient.exponent);

        ExpoNumber remultiply = quotientFloored * b;
        ExpoNumber difference = a - remultiply;

        return difference;
    }

    public static ExpoNumber operator +(ExpoNumber a, ExpoNumber b)
    {
        if (a.exponent == b.exponent)
        {
            double cheatSum = a.baseNumber + b.baseNumber;
            return new(cheatSum, a.exponent);
        }

        ExpoNumber big = a > b ? a : b;
        ExpoNumber small = a < b ? a : b;

        int thresholdDifference = big.exponent - small.exponent;
        if (thresholdDifference > MAX_THRESHOLD_DIFFERENCE)
            return big;

        double scalar = Math.Pow(1000, thresholdDifference);
        double smallScaled = small.baseNumber / scalar;

        double sum = big.baseNumber + smallScaled;
        return new(sum, big.exponent);
    }

    public static ExpoNumber operator -(ExpoNumber a, ExpoNumber b) => a + -b;


    public static ExpoNumber operator +(ExpoNumber a) => new(Math.Abs(a.baseNumber), a.exponent);
    public static ExpoNumber operator -(ExpoNumber a) => -1 * a;


    public static ExpoNumber operator ++(ExpoNumber a) { a.baseNumber++; return a; }
    public static ExpoNumber operator --(ExpoNumber a) { a.baseNumber--; return a; }




    public static bool operator >(ExpoNumber a, ExpoNumber b)
        => a.exponent > b.exponent || a.baseNumber > b.baseNumber && a.exponent >= b.exponent;

    public static bool operator <(ExpoNumber a, ExpoNumber b) => !(a > b);

    public static bool operator ==(ExpoNumber? a, ExpoNumber? b)
        => a?.exponent == b?.exponent && a?.baseNumber == b?.baseNumber;

    public static bool operator !=(ExpoNumber? a, ExpoNumber? b) => !(a == b);
}