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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Xml;
using CommonCode.ReportWriter;
using ConfigurationTests;
using ConfigurationTests.Tests;
using System.Globalization;

namespace InstallationSmokeTest
{
    internal class Program
    {
        private const string RunOperation = "Run";
        private const string RunOperationWithXmlReport = "RunWithXmlReport";
        private const string RunOperationWithCsvReport = "RunWithCsvReport";
        private const string RunOperationWithTxtReport = "RunWithTxtReport";

        private const string CreateOperation = "Create";
        private const string AbortOperation = "Abort";
        private const string UnexpectedResponseMessage = "Unexpected response.";
        private const string TestPassedMessage = "OK!";
        private const string OverwritePrompt = "Overwrite {0}? [y/N] ";
        private const ConsoleKey OverwriteAffirmativeKey = ConsoleKey.Y;
        private const string SmokeTestFileExtension = ".xml";
        private const string StandardNumberFormat = "#,##0";

        private enum RunMode
        {
            None = 0,            
            RunWithXmlReport = 1,
            RunWithCsvReport = 2,
            RunWithTxtReport = 3
        }

        private static string _outputFile;

        private static readonly ReportBuilder ReportBuilder = new ReportBuilder();

        internal static bool SmokeTestsPassed { get; private set; }

        internal static void Main(string[] args)
        {
            SmokeTestsPassed = false;
            var runWithUi = true;

            try
            {
                var operation = RunOperation;

                if (args.Length > 0)
                {
                    runWithUi = false;
                    operation = args[0];
                }

                var file = args.Length > 1 ? args[1] : SelectFile(operation == RunOperation);

                _outputFile = args.Length > 2 ? args[2] : null;

                WriteLine();
                WriteLine("Post-Deployment Smoke Test Tool");

                if (file == null)
                {
                    return;
                }

                if (Path.GetExtension(file).ToLower() != SmokeTestFileExtension)
                {
                    file += SmokeTestFileExtension;
                }

                switch (operation)
                {
                    case CreateOperation:
                        CreateConfiguration(file);
                        break;
                    case RunOperation:
                        CheckConfiguration(file, RunMode.None);
                        break;
                    case RunOperationWithCsvReport:
                        CheckConfiguration(file, RunMode.RunWithCsvReport);
                        break;
                    case RunOperationWithXmlReport:
                        CheckConfiguration(file, RunMode.RunWithXmlReport);
                        break;
                    case RunOperationWithTxtReport:
                        CheckConfiguration(file, RunMode.RunWithTxtReport);
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

                    if (runWithUi && _outputFile != null)
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
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("Smoke Tester : Command Line Test Runner Version 0.03");
            Console.WriteLine("http://smoketester.codeplex.com");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("{0} {1} <filename> <outputfilename>", exeName, RunOperation);
            Console.WriteLine("\tRun the tests contained in the given filename.");
            Console.WriteLine();
            Console.WriteLine("\tThe available modes are :");
            Console.WriteLine("\tRun - run a specified test script.");
            Console.WriteLine("\tRunWithXmlReport - run a test script and write xml test report.");
            Console.WriteLine("\tRunWithCsvReport - run a test script and write csv test report.");
            Console.WriteLine("\tRunWithTxtReport - run a test script and write text test report.");
            Console.WriteLine();
            Console.WriteLine("{0} {1} <filename> <outputfilename>", exeName, CreateOperation);
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
                    WriteLine(OverwritePrompt, file);
                    Console.ForegroundColor = ConsoleColor.White;
                    var overwrite = Console.ReadKey(true);

                    if (overwrite.Key != OverwriteAffirmativeKey)
                    {
                        WriteLine("Not overwriting.");
                        return;
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                WriteLine("Preparing example data...");
                var configurationInformation = new ConfigurationTestSuite();
                configurationInformation.CreateExampleData();
                var xmlString = configurationInformation.ToXmlString();
                Console.Write("Writing file...");
                File.WriteAllText(Path.Combine(".", file), xmlString, Encoding.Unicode);
                WriteLine(" Done.");
            }
            finally
            {
                Console.ForegroundColor = temp;
            }
        }

        private static void CheckConfiguration(string file, RunMode mode)
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
                var xml = File.ReadAllText(file, Encoding.Unicode);
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

            WriteLine("Running Tests: " + DateTime.Now.ToString("G", CultureInfo.CurrentCulture));
            WriteLine();

            ReportBuilder.ClearEntries();
            var successfulTests = info.Tests.Select(RunTest).Count(result => result);

            var reportPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestReport";
            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            var fileFriendlyDate =
                    DateTime.Now.ToString(CultureInfo.InvariantCulture)
                        .Replace("/", "")
                        .Replace(" ", "-")
                        .Replace(":", "");

            string fileName;
            switch (mode)
            {
                case RunMode.RunWithCsvReport:
                    fileName = Path.GetFullPath(reportPath + @"\" + fileFriendlyDate) + ".csv";
                    ReportBuilder.WriteReport(fileName, ReportType.CsvReport);
                    break;
                case RunMode.RunWithXmlReport:
                    fileName = Path.GetFullPath(reportPath + @"\" + fileFriendlyDate) + ".xml";
                    ReportBuilder.WriteReport(fileName, ReportType.XmlReport);
                    break;
                case RunMode.RunWithTxtReport:
                    fileName = Path.GetFullPath(reportPath + @"\" + fileFriendlyDate) + ".txt";
                    ReportBuilder.WriteReport(fileName, ReportType.TextReport);
                    break;
            }

            WriteLine();
            WriteLine("Completed Tests: " + DateTime.Now.ToString("G", CultureInfo.CurrentCulture));
            var totalTests = info.Tests.Count();

            var totalTestsString = totalTests.ToString(StandardNumberFormat);
            var totalWidth = totalTestsString.Length;
            var failedTests = totalTests - successfulTests;

            WriteLine("Tests Run:    {0}", totalTestsString);
            WriteLine("Tests Passed: {0}", successfulTests.ToString(StandardNumberFormat).PadLeft(totalWidth));
            WriteLine("Tests Failed: {0}", failedTests.ToString(StandardNumberFormat).PadLeft(totalWidth));

            if (failedTests > 0)
            {
                DisplayError("SMOKE TEST FAILED!");
            }
            else
            {
                SmokeTestsPassed = true;
                DisplaySuccess("Smoke test passed successfully");
            }

            if (failedTests > 0)
            {
                Environment.ExitCode = 1;
            }
            else
            {
                Environment.ExitCode = 0;
            }            
        }

        private static void WriteLine(string text = "", params object[] parameters)
        {
            if (_outputFile == null)
            {
                Console.WriteLine(text, parameters);
            }
            else
            {
                File.AppendAllLines(_outputFile, new[] {string.Format(text, parameters)});
            }
        }

        private static bool RunTest(Test test)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            WriteLine("{0}, {1}, {2}", test.GetType().Name, test.TestName, DateTime.Now.ToString("G", CultureInfo.CurrentCulture));

            DateTime startTime = DateTime.Now;
            DateTime stopTime;

            try
            {
                test.Run();
                stopTime = DateTime.Now;

                var entry = new ReportEntry
                {
                    TestName = test.TestName,
                    Result = true,
                    TestStartTime = startTime,
                    TestStopTime = stopTime
                };

                ReportBuilder.AddEntry(entry);

                Console.ForegroundColor = ConsoleColor.Green;
                WriteLine("\t\t{0}", TestPassedMessage);
                
                return true;
            }
            catch (Exception ex)
            {
                stopTime = DateTime.Now;

                Console.ForegroundColor = ConsoleColor.Red;
                WriteLine("\tMessage: {0}", ex.Message);
                WriteLine("\tStackTrace: {0}", ex.StackTrace);

                var entry = new ReportEntry
                {
                    TestName = test.TestName,
                    Result = false,
                    ErrorMessage = ex.Message,
                    TestStartTime = startTime,
                    TestStopTime = stopTime
                };

                ReportBuilder.AddEntry(entry);

                return false;
            }
            finally
            {
                Console.ForegroundColor = temp;
            }
        }

        private static string SelectFile(bool mustExist)
        {
            var files = Directory.GetFiles(".", "*" + SmokeTestFileExtension);
            var suites =
                files.Select(f =>
                {
                    var fileName = Path.GetFileName(f);
                    return fileName != null ? fileName.Replace(SmokeTestFileExtension, "") : null;
                })
                    .Where(f => f.ToUpper() != AbortOperation.ToUpper())
                    .ToArray();
            ChooseFile:

            PresentSelectionOptions(suites, AbortOperation);
            var input = GetInput().Trim();

            if (input == "?")
            {
                DisplayUsageHelp();
                goto ChooseFile;
            }

            if (string.IsNullOrWhiteSpace(input) ||
                input.Equals(AbortOperation, StringComparison.InvariantCultureIgnoreCase) || input == "0")
            {
                return null;
            }

            string file = null;

            if (suites.Any(e => e == input))
            {
                file = Path.Combine(".", string.Format("{0}{1}", input, SmokeTestFileExtension));
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
                        DisplayError(UnexpectedResponseMessage);
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

            for (var i = 0; i < suites.Length; i++)
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
            var input = Console.ReadLine();
            Console.ForegroundColor = temp;

            return input;
        }
    }
}