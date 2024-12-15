namespace NuvoLang;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage NuvoLang [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            Nuvo.RunFile(args[0]);
        }
        else
        {
            Nuvo.RunPrompt();
        }
    }
}
