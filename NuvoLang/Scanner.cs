namespace NuvoLang;

public class Scanner(string source)
{
    private readonly string _source = source;
    private readonly List<Token> _tokens = [];
    private int _start = 0;
    private int _current = 0;
    private int _line = 0;

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }
        return _tokens;
    }

    private void ScanToken()
    {
        var c = Advance();
        switch (c)
        {
            case '+': AddToken(TokenType.Plus); break;
            case '-': AddToken(Match('>') ? TokenType.BlockStart : TokenType.Minus); break;
            case '*': AddToken(TokenType.Star); break;
            case '/': AddToken(TokenType.Slash); break;
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case ',': AddToken(TokenType.Comma); break;
            case '!': AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang); break;
            case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
            case '<': AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less); break;
            case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
        }
    }

    private void AddToken(TokenType type, object? literal = null)
    {
        _tokens.Add(new Token
        {
            Type = type,
            Lexemme = _source[_start.._current],
            Literal = literal,
            Line = _line
        });
    }

    private bool Match(char c)
    {
        if (Peek() != c) return false;
        _current += 1;
        return true;
    }

    private char Peek()
    {
        return _current < _source.Length ? _source[_current] : '\0';
    }

    private char Advance()
    {
        _current += 1;
        return _source[_current - 1];
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }
}
