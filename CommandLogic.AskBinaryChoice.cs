using System;


namespace Std.Tools
{
    public partial class CommandLogic
    {
        private static bool AskBinaryChoice(string question,
            string yes = "Yes",
            string no = "No",
            bool defaultChoiceIsYes = true)
        {
            Console.Out.Flush();
            var yesCharLower = char.ToLowerInvariant(yes[0]);
            var noCharLower = char.ToLowerInvariant(no[0]);

            var yesChar = defaultChoiceIsYes
                ? char.ToUpperInvariant(yes[0])
                : yesCharLower;

            var noChar = defaultChoiceIsYes
                ? noCharLower
                : char.ToUpperInvariant(no[0]);

            Console.Write($"{question} ({yesChar}/{noChar}) ");
            Console.Out.Flush();
            bool? result = null;

            while (!result.HasValue)
            {
                result = DetermineKeyChoice(Console.ReadKey(true), yesCharLower, noCharLower, defaultChoiceIsYes);
            }

            var realResult = result.Value;

            Console.WriteLine(realResult
                ? yes
                : no);

            Console.Out.Flush();
            return realResult;
        }

        private static bool? DetermineKeyChoice(ConsoleKeyInfo info, char yesChar, char noChar, bool defaultChoice)
        {
            switch (char.ToLowerInvariant(info.KeyChar))
            {
                case 'y':
                case 't':
                case '1':
                case var c when c == yesChar:
                    return true;
                case 'n':
                case 'f':
                case '0':
                case var c when c == noChar:
                    return false;
            }

            return info.Key switch
            {
                ConsoleKey.LeftArrow => true,
                ConsoleKey.RightArrow => false,
                ConsoleKey.Enter => defaultChoice,
                _ => null
            };
        }
    }
}
