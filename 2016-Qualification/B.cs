//#define START_THIS_FILE

using System.Linq;
using System.Text.RegularExpressions;

namespace CodeJamSharp {
	internal static class B {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig() {
				InputFile = "BTest.in",
				//InputFile = "B-small-attempt0.in",
				//InputFile = "B-large.in",
				Solver = (testCase, cache) => {
					Regex r = new Regex("(.)(?<=\\1\\1)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
					string filtered = r.Replace(testCase.Input[0], string.Empty);

					if(filtered.Last() == '+') {
						return (filtered.Length - 1).ToString();
					}
					return filtered.Length.ToString();
				}
			});
		}
	}
}
