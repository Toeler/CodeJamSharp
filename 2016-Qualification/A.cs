#define START_THIS_FILE

using System.Linq;

namespace CodeJamSharp {
	internal static class A {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
		new CodeJamProblem(new CodeJamConfig() {
			InputFile = "ATest.in",
			//InputFile = "A-small-attempt0.in",
			//InputFile = "A-large.in",
			Solver = (testCase, cache) => {
					var n = long.Parse(testCase.Input[0].Split(' ')[0]);

					if(n != 0) {
						char[] digitsNotSeen = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
						long i = 1;

						var num = string.Empty;
						while(digitsNotSeen.Length > 0) {
							num = (i++ * n).ToString();
							var digits = num.ToCharArray();
							digitsNotSeen = digitsNotSeen.Where(digit => !digits.Contains(digit)).ToArray();
						}

						return num;
					}

					return "INSOMNIA";
				}
			});
		}
	}
}
