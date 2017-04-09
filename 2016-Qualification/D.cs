//#define START_THIS_FILE

using System.Linq;

namespace CodeJamSharp {
	internal static class D {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig() {
				InputFile = "DTest.in",
				//InputFile = "D-small-attempt0.in",
				//InputFile = "D-large.in",
				Solver = (testCase, cache) => {
					var input = testCase.Input[0].Split(' ');
					var k = int.Parse(input[0]);
					var c = int.Parse(input[1]);
					var s = int.Parse(input[2]);

					if(s == k) {
						return string.Join(" ", Enumerable.Range(0, s).Select(n => n + 1));
					}

					if(s == k - 1 && c > 1) {
						return string.Join(" ", Enumerable.Range(0, s).Select(n => n + 2));
					}

					return "IMPOSSIBLE";
				}
			});
		}
	}
}
