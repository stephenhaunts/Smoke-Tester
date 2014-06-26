/**
* Smoke Tester Tool : Post deployment smoke testing tool.
* 
* http://www.stephenhaunts.com
* 
* This file is part of Smoke Tester Tool.
* 
* Smoke Tester Tool is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Smoke Tester Tool is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Curator: Stephen Haunts
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Common.Text
{
    public static class TextExtensionMethods
    {
        private const string NumericCharacters = "-0123456789.";

        public static bool IsNumeric(this string candidate)
        {
            return !string.IsNullOrWhiteSpace(candidate) && candidate.All(NumericCharacters.Contains);
        }

        public static string s(this int value)        
        {
            return value.pl("", "s");
        }

        public static string es(this int value)        
        {
            return value.pl("", "es");
        }

        public static string ies(this int value)        
        {
            return value.pl("y", "ies");
        }
    
        public static string pl(this int value, string singular, string plural)      
        {
            return value == 1 ? singular : plural;
        }

        public static int CountOf(this string candidate, string pattern, bool allowOverlapping = false)
        {
            int count = 0;
            int i = 0;

            while ((i = candidate.IndexOf(pattern, i)) != -1)
            {
                i += allowOverlapping ? 1 : pattern.Length;
                count++;
            }

            return count;
        }

        public static string MatchCharacters(this string candidate, MatchType matchType)
        {
            return MatchCharacters(candidate, matchType, false);
        }

        private static readonly int matchTypeCeiling = MatchType.MaxValue;

        public static string MatchCharacters(this string candidate, MatchType matchType, bool caseSensitive)
        {
            string chars = string.Empty;

            for (int i = 1; i <= matchTypeCeiling; i *= 2)
            {
                if ((matchType & i) == i)
                {
                    chars += MatchType.MatchCharacters[i];
                }
            }

            if (!caseSensitive)
            {
                chars += chars.ToUpper();                
            }

            return MatchCharacters(candidate, chars);
        }

        public static string MatchCharacters(this string candidate, string chars)
        {
            return new string(candidate.Where(chars.Contains).ToArray());
        }

        public static string Truncate(this string candidate, int length)
        {
            return candidate.Substring(0, Math.Min(length, candidate.Length));
        }

        static readonly ConsoleKey[] disallowedKeys =
        {
            ConsoleKey.Applications, ConsoleKey.Attention, ConsoleKey.BrowserBack, ConsoleKey.BrowserFavorites,ConsoleKey.BrowserForward, ConsoleKey.BrowserHome, ConsoleKey.BrowserRefresh, ConsoleKey.BrowserSearch, ConsoleKey.BrowserStop, ConsoleKey.Clear,ConsoleKey.CrSel, ConsoleKey.Delete, ConsoleKey.DownArrow, ConsoleKey.End, ConsoleKey.EraseEndOfFile, ConsoleKey.Escape, ConsoleKey.ExSel, ConsoleKey.Execute, ConsoleKey.Help, ConsoleKey.Home, ConsoleKey.Insert, ConsoleKey.LaunchApp1, ConsoleKey.LaunchApp2, ConsoleKey.LaunchMail, ConsoleKey.LaunchMediaSelect, ConsoleKey.LeftArrow, ConsoleKey.LeftWindows, ConsoleKey.MediaNext, ConsoleKey.MediaPlay, ConsoleKey.MediaPrevious, ConsoleKey.MediaStop, ConsoleKey.OemClear, ConsoleKey.Pa1, ConsoleKey.Packet, ConsoleKey.PageDown, ConsoleKey.PageUp, ConsoleKey.Pause, ConsoleKey.Play, ConsoleKey.Print, ConsoleKey.PrintScreen, ConsoleKey.Process, ConsoleKey.RightArrow, ConsoleKey.RightWindows, ConsoleKey.Select, ConsoleKey.Sleep, ConsoleKey.Tab, ConsoleKey.UpArrow, ConsoleKey.VolumeDown, ConsoleKey.VolumeMute, ConsoleKey.VolumeUp, ConsoleKey.Zoom
        };

        public static string Prompt(this string promptText, bool passwordInput = false, string prepend = "", uint maxLength = uint.MaxValue, IEnumerable<char> allowedChars = null)
        {
            if (!Environment.UserInteractive)
            {
                return string.Empty;
            }

            Console.Write(promptText);
            string result = prepend;
            Console.Write(prepend);
            ConsoleKeyInfo nextKey;

            while ((nextKey = Console.ReadKey(true)).Key != ConsoleKey.Enter
                && !disallowedKeys.Contains(nextKey.Key)
                && (allowedChars == null || allowedChars.Contains(nextKey.KeyChar)))
            {
                if (nextKey.Key == ConsoleKey.Backspace)
                {
                    if (result.Length > 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (result.Length < maxLength)
                {
                    Console.Write(passwordInput ? "*" : nextKey.KeyChar.ToString());
                    result += nextKey.KeyChar;
                }
            }

            Console.WriteLine();
            return result;
        }

        public static char Choice(this string promptText, IEnumerable<char> options, int timeout = -1, bool displayTime = false, int defaultChoice = 0, bool caseSensitive = false)
        {
            char[] optionsArray = !caseSensitive ? new string(options.ToArray()).ToUpper().ToCharArray() : options.ToArray();

            char defaultChar = optionsArray[defaultChoice];
            char result = defaultChar;

            if (!Environment.UserInteractive)
            {
                return result;
            }

            Console.Write("{0} [", promptText);

            for (int i = 0; i < optionsArray.Length; i++)
            {
                ConsoleColor c = Console.ForegroundColor;

                if (i == defaultChoice)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(optionsArray[i]);
                Console.ForegroundColor = c;
            }
            Console.Write("] ");

            Tuple<int, int> cursor = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);

            if (timeout < 0)
            {
                while (!optionsArray.Contains(result = GetNextChar(caseSensitive)) && result != '\r')
                {
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                }
            }
            else
            {
                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();
                int elapsedSeconds;

                while ((elapsedSeconds = (int) stopwatch.Elapsed.TotalSeconds) < timeout)
                {
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                    Console.Write("({0}s) {1} ", timeout - elapsedSeconds, defaultChar);
                    Console.SetCursorPosition(Console.CursorLeft - 2, cursor.Item2);

                    if (Console.KeyAvailable && optionsArray.Contains(result = GetNextChar(caseSensitive)))
                    {
                        return result;
                    }

                    Thread.Sleep(1);
                }

                Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                Console.Write("(0s) {0}", defaultChar);
            }

            Console.WriteLine();

            return optionsArray.Contains(result) ? result : defaultChar;
        }

        private static char GetNextChar(bool caseSensitive)
        {
            char keyChar = Console.ReadKey().KeyChar;
            return caseSensitive ? keyChar : keyChar.ToString().ToUpper()[0];
        }
    }
}
