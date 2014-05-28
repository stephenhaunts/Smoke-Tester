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
using ConfigurationTests;
using ConfigurationTests.Tests;
using Common.Xml;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InstallationSmokeTest
{
    internal class Program
    {
        private const string RUN_OPERATION = "Run";
        private const string CREATE_OPERATION = "Create";
        private const string ABORT_OPERATION = "Abort";
        private const string UNEXPECTED_RESPONSE_MESSAGE = "Unexpected response.";
        private const string TEST_PASSED_MESSAGE = "OK!";
        private const string OVERWRITE_PROMPT = "Overwrite {0}? [y/N] ";
        private const ConsoleKey OVERWRITE_AFFIRMATIVE_KEY = ConsoleKey.Y;
        private const string SMOKE_TEST_FILE_EXTENSION = ".xml";
        private const string STANDARD_DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        private const string STANDARD_NUMBER_FORMAT = "#,##0";

        private static string _OutputFile;

        internal static bool SmokeTestsPassed { get; private set; }

        internal static void Main(string[] args)
        {
            SmokeTestsPassed = false;
            bool runWithUi = true;

            try
            {
                string operation = RUN_OPERATION;

                if (args.Length > 0)
                {
                    runWithUi = false;
                    operation = args[0];
                }

                string file = args.Length > 1 ? args[1] : SelectFile(operation == RUN_OPERATION);

                _OutputFile = args.Length > 2 ? args[2] : null;

                WriteLine();
                WriteLine("Post-Deployment Smoke Test Tool");

                if (file == null)
                {
                    return;
                }

                if (Path.GetExtension(file).ToLower() != SMOKE_TEST_FILE_EXTENSION)
                {
                    file += SMOKE_TEST_FILE_EXTENSION;
                }

                switch (operation)
                {
                    case CREATE_OPERATION:
                        CreateConfiguration(file);
                        break;
                    case RUN_OPERATION:
                        CheckConfiguration(file);
                        break;
                    default:
                        DisplayUsageHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Environment.ExitCode = int.MaxValue;

                if (Environment.UserInteractive)
                {
                    WriteLine("Message:" + ex.Message);
                    WriteLine("StackTrace: " + ex.StackTrace);
                    
                    if (runWithUi && _OutputFile != null)
                    {
                        Console.Write("Press any key to end . . .");
                        Console.ReadKey(true);
                    }
                }

                else throw;
            }
        }

        private static void DisplayUsageHelp()
        {
            string exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("{0} {1} <filename> <outputfilename>", exeName, RUN_OPERATION);
            Console.WriteLine("\tRun the tests contained in the given filename.");
            Console.WriteLine();
            Console.WriteLine("{0} {1} <filename> <outputfilename>", exeName, CREATE_OPERATION);
            Console.WriteLine("\tCreate a new XML file with examples of the usage.");
            Console.WriteLine();
            Console.WriteLine("\tThe default mode is Run.");
            Console.WriteLine("\tIf the filename does not end with .xml then .xml will be appended.");
            Console.WriteLine("\tIf the filename is omitted, you will be prompted for it.");
        }

        private static void CreateConfiguration(string file)
        {
            var temp = Console.ForegroundColor;

            try
            {
                if (File.Exists(file))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteLine(OVERWRITE_PROMPT, file);
                    Console.ForegroundColor = ConsoleColor.White;
                    var overwrite = Console.ReadKey(true);

                    if (overwrite.Key != OVERWRITE_AFFIRMATIVE_KEY)
                    {
                        WriteLine("Not overwriting.");
                        return;
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                WriteLine("Preparing example data...");
                var configurationInformation = new ConfigurationTestSuite();
                configurationInformation.CreateExampleData();
                string xmlString = configurationInformation.ToXmlString();
                Console.Write("Writing file...");
                File.WriteAllText(Path.Combine(".", file), xmlString, Encoding.Unicode);
                WriteLine(" Done.");
            }
            finally
            {
                Console.ForegroundColor = temp;
            }
        }

        private static void CheckConfiguration(string file)
        {
            if (!File.Exists(file))
            {
                WriteLine("Could not find file {0}", file);
                Environment.ExitCode = int.MaxValue;
                return;
            }

            ConfigurationTestSuite info;

            try
            {
                string xml = File.ReadAllText(file, Encoding.Unicode);
                info = xml.ToObject<ConfigurationTestSuite>();
            }
            catch (InvalidOperationException ex)
            {
                DisplayError("Unable to read file, check that the file is in Unicode.");
                DisplayError(ex.ToString());
                return;
            }

            if (info == null)
            {
                DisplayError(String.Format("Could not convert {0} to ConfigurationTestSuite object.", file));
                return;
            }

            WriteLine("Running Tests: " + DateTime.Now.ToString(STANDARD_DATETIME_FORMAT));
            WriteLine();

            int successfulTests = 0;

            foreach (Test test in info.Tests)
            {
                bool result = RunTest(test);

                if (result)
                {
                    successfulTests++;
                }
            }

            WriteLine();
            WriteLine("Completed Tests: " + DateTime.Now.ToString(STANDARD_DATETIME_FORMAT));
            int totalTests = info.Tests.Count();

            string totalTestsString = totalTests.ToString(STANDARD_NUMBER_FORMAT);
            int totalWidth = totalTestsString.Length;
            int failedTests = totalTests - successfulTests;

            WriteLine("Tests Run:    {0}", totalTestsString);
            WriteLine("Tests Passed: {0}", successfulTests.ToString(STANDARD_NUMBER_FORMAT).PadLeft(totalWidth));
            WriteLine("Tests Failed: {0}", failedTests.ToString(STANDARD_NUMBER_FORMAT).PadLeft(totalWidth));

            if (failedTests > 0)
            {
                DisplayError("SMOKE TEST FAILED!");
            }
            else
            {
                SmokeTestsPassed = true;
                DisplaySuccess("Smoke test passed successfully");
            }

            Environment.ExitCode = failedTests;

            if (Environment.UserInteractive && _OutputFile == null)
            {
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey(true);
            }
        }

        private static void WriteLine(string text = "", params object[] parameters)
        {
            if (_OutputFile == null)
            {
                Console.WriteLine(text, parameters);
            }
            else
            {
                File.AppendAllLines(_OutputFile, new[] {string.Format(text, parameters)});
            }
        }

        private static bool RunTest(Test test)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            WriteLine("{0}, {1}, {2}", test.GetType().Name, test.TestName, DateTime.Now.ToString(STANDARD_DATETIME_FORMAT));

            try
            {
                test.Run();
                Console.ForegroundColor = ConsoleColor.Green;
                WriteLine("\t\t{0}", TEST_PASSED_MESSAGE);

                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteLine("\tMessage: {0}", ex.Message);
                WriteLine("\tStackTrace: {0}", ex.StackTrace);

                return false;
            }
            finally
            {
                Console.ForegroundColor = temp;
            }
        }

        private static string SelectFile(bool mustExist)
        {
            string[] files = Directory.GetFiles(".", "*" + SMOKE_TEST_FILE_EXTENSION);
            string[] suites = files.Select(f => Path.GetFileName(f).Replace(SMOKE_TEST_FILE_EXTENSION, "")).Where(f => f.ToUpper() != ABORT_OPERATION.ToUpper()).ToArray();
        ChooseFile:

            PresentSelectionOptions(suites, ABORT_OPERATION);
            string input = GetInput().Trim();

            if (input == "?")
            {
                DisplayUsageHelp();
                goto ChooseFile;
            }

            if (string.IsNullOrWhiteSpace(input) ||
                input.Equals(ABORT_OPERATION, StringComparison.InvariantCultureIgnoreCase) || input == "0")
            {
                return null;
            }

            string file = null;

            if (suites.Any(e => e == input))
            {
                file = Path.Combine(".", string.Format("{0}{1}", input, SMOKE_TEST_FILE_EXTENSION));
            }
            else
            {
                int fileIndex;

                if (int.TryParse(input, out fileIndex) && fileIndex <= files.Length)
                {
                    if (fileIndex > 0)
                    {
                        file = files[fileIndex - 1];
                    }
                }
                else
                {
                    if (mustExist)
                    {
                        DisplayError(UNEXPECTED_RESPONSE_MESSAGE);
                        goto ChooseFile;
                    }

                    file = input;
                }
            }

            return file;
        }

        private static void PresentSelectionOptions(string[] suites, string abort)
        {
            var temp = Console.ForegroundColor;

            Console.WriteLine();
            Console.WriteLine("Choose a suite or type its name, or type 0 to {0} or ? for CLI help:", abort);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("0: {0}", abort);

            for (int i = 0; i < suites.Length; i++)
            {
                Console.WriteLine("{0}: {1}", i + 1, suites[i]);
            }

            Console.ForegroundColor = temp;
        }

        private static void DisplayError(string errorMessage)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine(errorMessage);

            Console.ForegroundColor = temp;
        }

        private static void DisplaySuccess(string message)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine(message);

            Console.ForegroundColor = temp;
        }

        private static string GetInput()
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();
            Console.ForegroundColor = temp;

            return input;
        }
    }
}
