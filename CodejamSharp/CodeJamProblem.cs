using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CodeJamSharp {
	public class TestCase {
		public readonly string Input;
		public string Output;
		public readonly int CaseNumber;

		public TestCase(int caseNumber, string line) {
			if (line == null) {
				throw new Exception("Input cannot be null");
			}
			Input = line;
			CaseNumber = caseNumber;
		}
	}

	public class CodeJamConfig {
		public delegate string SolverFunc(TestCase testCase, IDictionary<object, object> cache);

		public SolverFunc Solver { get; set; }
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public string OutputFormat { get; set; } = "Case #{0}: {1}";
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
							yield return new TestCase(caseNumber, file.ReadLine());
						}
					}
				}
			}

			if (filePath == null) {
				string input;
				var testCaseCount = -1;
				var currentCount = 0;
				while ((input = Console.ReadLine()) != null && currentCount != (testCaseCount - 1)) {
					if (testCaseCount == -1) {
						testCaseCount = Convert.ToInt32(input);
					} else {
						yield return new TestCase(++currentCount, input);
					}
				}
				yield return new TestCase(++currentCount, input);
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