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
        AddToken(TokenType.Eof);
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
            case '/':
                if (Match('/'))
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                else
                    AddToken(TokenType.Slash);
                break;
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case ',': AddToken(TokenType.Comma); break;
            case '!': AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang); break;
            case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
            case '<': AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less); break;
            case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
            case ' ':
            case '\t':
            case '\r':
                break;
            case '\n': _line += 1; break;
            case '"': ScanString(); break;
            default:
                if (IsDigit(c)) ScanNumber();
                break;
        }
    }

    private void ScanNumber()
    {
        var type = TokenType.Integer;
        while (IsDigit(Peek()) || !IsAtEnd()) Advance();
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            type = TokenType.Float;
            Advance();
        }
        while (IsDigit(Peek()) || !IsAtEnd()) Advance();

        var s = _source[_start.._current];
        if (type == TokenType.Integer)
            AddToken(TokenType.Integer, int.Parse(_source[_start.._current]));
        else
            AddToken(TokenType.Float, double.Parse(_source[_start.._current]));
    }

    private static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private void ScanString()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line += 1;
            Advance();
        }

        if (IsAtEnd())
        {
            Nuvo.Error("Unterminated string.", _line);
            return;
        }

        Advance();
        string s = _source[(_start + 1)..(_current - 1)];
        AddToken(TokenType.String, s);
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

    private char PeekNext()
    {
        return _current + 1 < _source.Length ? _source[_current + 1] : '\0';
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
