//#define START_THIS_FILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace CodeJamSharp {
	internal static class A {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig() {
				//InputFile = "ATest.in",
				//InputFile = "A-small-attempt0.in",
				InputFile = "A-large.in",
				Solver = (testCase, cache) => {
					var k = int.Parse(testCase.Input[0].Split(' ')[1]);
					var board = testCase.Input[0].Split(' ')[0];

					// Generate input
					//IList<string> lines = Enumerable.Range(2, 1022).Select(i => string.Join("", Convert.ToString(i, 2).Select(x => x == '1' ? '+' : '-')) + " 3").ToList();
					//var str = string.Join(Environment.NewLine, lines);
					
					var score = 0;
					var sb = new StringBuilder(board);
					for(var i = 0; i < board.Length - k + 1; i++) {
						if (sb[i] != '-') continue;
						for(var j = 0; j < k; j++) {
							sb[i + j] = Flip(sb[i + j]);
						}
						score++;
					}
					return sb.ToString().Any(x => x != '+') ? "IMPOSSIBLE" : score.ToString();
				}
			});
		}

		private static char Flip(char input) {
			return input == '+' ? '-' : '+';
		}
	}
}
