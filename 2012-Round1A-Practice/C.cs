#define START_THIS_FILE

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CodeJamSharp {
	internal static class C {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig() {
				LinesPerCase = 0,
				InputFile = "CTest.in",
				//InputFile = "C-small-attempt0.in",
				//InputFile = "C-large.in",
				Solver = (testCase, cache) => {
					return string.Join(":", testCase.Input);
				}
			});
		}
	}
}
