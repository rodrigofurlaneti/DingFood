namespace DingFood.Domain.Constants;

/// <summary>
/// Normalização e validação de CNPJ (dígitos verificadores da Receita Federal).
/// Usado antes de qualquer chamada à CNPJá para não queimar a cota de 5 consultas/minuto
/// com um número que já sabemos ser inválido.
/// </summary>
public static class CnpjValidator
{
    private const int CnpjLength = 14;

    private static readonly int[] FirstCheckDigitWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] SecondCheckDigitWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    /// <summary>
    /// Remove qualquer caractere que não seja dígito. "14.486.046/0001-77" -> "14486046000177".
    /// </summary>
    public static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var digits = new char[value.Length];
        var length = 0;

        foreach (var character in value)
        {
            if (char.IsDigit(character))
                digits[length++] = character;
        }

        return length == 0 ? string.Empty : new string(digits, 0, length);
    }

    /// <summary>
    /// Valida os 14 dígitos e os dois dígitos verificadores. Aceita entrada com ou sem pontuação.
    /// </summary>
    public static bool IsValid(string? value)
    {
        var digits = Normalize(value);

        if (digits.Length != CnpjLength)
            return false;

        if (AllDigitsAreTheSame(digits))
            return false;

        return digits[12] == CalculateCheckDigit(digits, FirstCheckDigitWeights)
            && digits[13] == CalculateCheckDigit(digits, SecondCheckDigitWeights);
    }

    /// <summary>
    /// Formata para exibição: "14486046000177" -> "14.486.046/0001-77".
    /// Devolve a entrada normalizada quando não houver 14 dígitos.
    /// </summary>
    public static string Format(string? value)
    {
        var digits = Normalize(value);

        return digits.Length != CnpjLength
            ? digits
            : $"{digits[..2]}.{digits[2..5]}.{digits[5..8]}/{digits[8..12]}-{digits[12..]}";
    }

    private static bool AllDigitsAreTheSame(string digits)
    {
        for (var i = 1; i < digits.Length; i++)
        {
            if (digits[i] != digits[0])
                return false;
        }

        return true;
    }

    private static char CalculateCheckDigit(string digits, int[] weights)
    {
        var sum = 0;

        for (var i = 0; i < weights.Length; i++)
            sum += (digits[i] - '0') * weights[i];

        var remainder = sum % 11;
        var checkDigit = remainder < 2 ? 0 : 11 - remainder;

        return (char)('0' + checkDigit);
    }
}
