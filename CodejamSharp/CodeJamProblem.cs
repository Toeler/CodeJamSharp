using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CodeJamSharp {
	public class TestCase {
		public readonly string LinesPerCaseInput;
		public readonly string[] Input;
		public string Output;
		public readonly int CaseNumber;

		public TestCase(int caseNumber, string[] lines, string linesPerCaseInput = null) {
			if (lines == null) {
				throw new Exception("Input cannot be null");
			}
			Input = lines;
			CaseNumber = caseNumber;
			LinesPerCaseInput = linesPerCaseInput;
		}
	}

	public class CodeJamConfig {
		public delegate string SolverFunc(TestCase testCase, IDictionary<object, object> cache);

		public SolverFunc Solver { get; set; }
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public string OutputFormat { get; set; } = "Case #{0}: {1}";
		public int LinesPerCase { get; set; } = 1;
		public Func<string, int> LinesPerCaseFunc { get; set; } 
	}

	public sealed class CodeJamProblem {
		private readonly CodeJamConfig _config;

		public CodeJamProblem(CodeJamConfig config) {
			_config = config;

			var testCases = GetTestCases().ToList();

			IDictionary<object, object> cache = new Dictionary<object, object>();
			foreach (var testCase in testCases) {
				testCase.Output = config.Solver(testCase, cache);
			}

			OutputTestCases(testCases);
		}

		private IEnumerable<TestCase> GetTestCases() {
			string filePath = null;
			if (!string.IsNullOrEmpty(_config.InputFile)) {
				var currentDirectory = Directory.GetCurrentDirectory();
				while (filePath == null && currentDirectory != null) {
					filePath = Directory.GetFiles(currentDirectory, _config.InputFile, SearchOption.TopDirectoryOnly).FirstOrDefault();
					currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
				}
				if (filePath != null) {
					_config.InputFile = filePath;
					using (var file = File.OpenText(filePath)) {
						var testCaseCount = Convert.ToInt32(file.ReadLine());

						foreach (var caseNumber in Enumerable.Range(1, testCaseCount)) {
							var linesPerCase = _config.LinesPerCase;
							string linesPerCaseInput = null;
							if (_config.LinesPerCaseFunc != null) {
								linesPerCaseInput = file.ReadLine();
								linesPerCase = _config.LinesPerCaseFunc(linesPerCaseInput);
							} else if (linesPerCase == 0) {
								linesPerCaseInput = file.ReadLine();
								linesPerCase = Convert.ToInt32(linesPerCaseInput);
							}
							string[] lines = new string[linesPerCase];
							foreach (var line in Enumerable.Range(0, linesPerCase)) {
								lines[line] = file.ReadLine();
							}
							yield return new TestCase(caseNumber, lines, linesPerCaseInput);
						}
					}
				}
			}

			if (filePath == null) {
				// UNTESTED
				string input;

				var testCaseCount = -1;
				while ((input = Console.ReadLine()) != null && testCaseCount == -1) {
					testCaseCount = Convert.ToInt32(input);
				}

				var linesPerCase = _config.LinesPerCase;
				var currentLineCount = 0;
				var currentCaseCount = 0;
				string[] lines = new string[linesPerCase];
				while((input = Console.ReadLine()) != null && currentCaseCount != testCaseCount) {
					if (linesPerCase == 0) {
						linesPerCase = Convert.ToInt32(input);
						currentLineCount = 0;
						lines = new string[linesPerCase];
						continue;
					}

					lines[currentLineCount++] = input;

					if (currentLineCount == linesPerCase) {
						linesPerCase = _config.LinesPerCase;
						yield return new TestCase(++currentCaseCount, lines);
					}
				}
				yield return new TestCase(++currentCaseCount, lines);
			}
		}

		private void OutputTestCases(IEnumerable<TestCase> testCases) {
			if (string.IsNullOrEmpty(_config.OutputFile) && !string.IsNullOrEmpty(_config.InputFile)) {
				_config.OutputFile = (_config.InputFile.EndsWith(".in")
					? _config.InputFile.Substring(0, _config.InputFile.Length - 3)
					: _config.InputFile) + ".out";
			}
			StreamWriter writer = null;
			if (!string.IsNullOrEmpty(_config.OutputFile)) {
				writer = new StreamWriter(_config.OutputFile);
			}
			foreach (
				var text in
					testCases.Select(
						testCase =>
							string.Format(CultureInfo.InvariantCulture, _config.OutputFormat, testCase.CaseNumber, testCase.Output))) {
				writer?.WriteLine(text);
				Console.WriteLine(text);
			}
			writer?.Dispose();
		}

		private static void Main() {
		}
	}
}