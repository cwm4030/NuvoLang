namespace NuvoLang;

public class Token
{
    public TokenType Type { get; set; }
    public string Lexemme { get; set; } = string.Empty;
    public object? Literal { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }
    public string Display => $"{Type} {Lexemme} {Literal}";
}
