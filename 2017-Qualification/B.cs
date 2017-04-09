//#define START_THIS_FILE

using System.Linq;
using System.Numerics;

namespace CodeJamSharp {
	internal static class B {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig() {
				//InputFile = "BTest.in",
				//InputFile = "B-small-attempt0.in",
				InputFile = "B-large.in",
				Solver = (testCase, cache) => {
					var nums = testCase.Input[0].Select(x => (int)char.GetNumericValue(x)).ToList();
					var setRestTo9 = false;
					for (var i = 0; i < nums.Count; i++) {
						if (setRestTo9) {
							nums[i] = 9;
							continue;
						}
						if (i == nums.Count - 1) {
							continue;
						}
						if (nums[i + 1] >= nums[i]) continue;
						while (i > 0 && nums[i - 1] == nums[i]) {
							i--;
						}
						nums[i]--;
						setRestTo9 = true;
					}
					return BigInteger.Parse(string.Join("", nums)).ToString();
				}
			});
		}
	}
}
