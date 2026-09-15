namespace DingFood.Domain.Constants;

/// <summary>
/// Normalização e validação de CEP. O ViaCEP devolve 400 para formato inválido e avisa que
/// "uso massivo poderá automaticamente bloquear seu acesso" — validar antes evita as duas coisas.
/// </summary>
public static class CepValidator
{
    private const int CepLength = 8;

    /// <summary>Remove tudo que não for dígito. "01001-000" -> "01001000".</summary>
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
    /// Valida o formato: exatamente 8 dígitos, e não uma repetição do mesmo dígito
    /// ("00000000" é sintaticamente válido mas nunca existe na base dos Correios).
    /// </summary>
    public static bool IsValid(string? value)
    {
        var digits = Normalize(value);

        if (digits.Length != CepLength)
            return false;

        for (var i = 1; i < digits.Length; i++)
        {
            if (digits[i] != digits[0])
                return true;
        }

        return false;
    }

    /// <summary>Formata para exibição. "01001000" -> "01001-000".</summary>
    public static string Format(string? value)
    {
        var digits = Normalize(value);
        return digits.Length != CepLength ? digits : $"{digits[..5]}-{digits[5..]}";
    }
}
