//#define START_THIS_FILE

using System;
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
				//InputFile = "CTest.in",
				//InputFile = "C-small-2-attempt0.in",
				InputFile = "C-large.in",
				Solver = (testCase, cache) => {
					var n= BigInteger.Parse(testCase.Input[0].Split(' ')[0]);
					var k = BigInteger.Parse(testCase.Input[0].Split(' ')[1]);

					var bits2 = new BigInteger(BigInteger.Log(k, 2)) + 1;
					var bitPow = new BigInteger(Math.Pow(2, (double)bits2));
					var max = (n - k % (bitPow / 2)) / bitPow;
					var min = (n - k)/bitPow;

					// System.Diagnostics.Debug.Assert($"{max} {min}" == BruteForce((int)n, (int)k), $"Input: {n} {k} expected {BruteForce((int)n, (int)k)} but got {max} {min}");

					return $"{max.ToString("0")} {min.ToString("0")}";
				}
			});
		}

		private static string BruteForce(int n, int k) {
			IList<Stall> stalls = new List<Stall>();
			foreach(var x in Enumerable.Range(0, n + 2)) {
				stalls.Add(new Stall(stalls, x, x == 0 || x == n + 1));
			}

			Stall lastEnteredStall = null;
			for(var i = 0; i < k; i++) {
				var orderedStalls = stalls.Where(stall => stall.IsAvailable())
					.OrderByDescending(stall => Math.Min(stall.L, stall.R))
					.ThenByDescending(stall => Math.Max(stall.L, stall.R))
					.ThenBy(stall => stall.Index);

				lastEnteredStall = orderedStalls.First();
				lastEnteredStall.Occupied = true;
			}

			return $"{Math.Max(lastEnteredStall.L, lastEnteredStall.R)} {Math.Min(lastEnteredStall.L, lastEnteredStall.R)}";
		}

		private class Stall {
			private readonly IList<Stall> _stalls;

			public bool Occupied;
			public readonly int Index;

			public Stall(IList<Stall> stalls, int index, bool occupied) {
				_stalls = stalls;
				Index = index;
				Occupied = occupied;
			}

			public int L {
				get {
					int i = Index - 1;
					int score = 0;
					while (i > 0 && _stalls[i].IsAvailable()) {
						score++;
						i--;
					}
					return score;
				}
			}

			public int R {
				get {
					int i = Index + 1;
					int score = 0;
					while(i < _stalls.Count && _stalls[i].IsAvailable()) {
						score++;
						i++;
					}
					return score;
				}
			}

			public bool IsAvailable() => !Occupied;

			public override string ToString() => $"{Index} - {Occupied} (L:{L}, R:{R})";
		}
	}
}

/*
 * My Working:

-------------
------1------
-------------
1  1: 0 0
2  1: 1 0
3  1: 1 1
4  1: 2 1
5  1: 2 2
6  1: 3 2
7  1: 3 3
8  1: 4 3
9  1: 4 4
10 1: 5 4
11 1: 5 5
12 1: 6 5
13 1: 6 6
14 1: 7 6
15 1: 7 7
...
998 1: 499 ?
999 1: 499 499
1000 1: 500 499
X Y: fl(X/2) fl(X-1/2)

-------------			-------------
------2------			------3------
-------------			-------------
2  2: 0 0				3  3: 0 0
3  2: 0 0				4  3: 0 0
4  2: 1 0				5  3: 1 0
5  2: 1 0				6  3: 1 0
6  2: 1 1				7  3: 1 1
7  2: 1 1				8  3: 1 1
8  2: 2 1				9  3: 2 1
9  2: 2 1				10 3: 2 1
10 2: 2 2				11 3: 2 2
11 2: 2 2				12 3: 2 2
12 2: 3 2				13 3: 3 2
13 2: 3 2				14 3: 3 2
14 2: 3 3				15 3: 3 3
15 2: 3 3				16 3: 3 3
16 2: 4 3				17 3: 4 3
...						...
998 2: 249 ?			998 3: 249 ?
999 2: 249 249			999 3: 249 249
1000 2: 250 249			1000 3: 249 249
X Y: fl(X/4) fl(X-2/4)	X Y: fl(X-1/4) fl(X-3/4)

-------------			-------------			-------------			-------------
------4------			------5------			------6------			------7------
-------------			-------------			-------------			-------------
4  4: 0 0				5  5: 0 0				6  6: 0 0				7  7: 0 0
5  4: 0 0				6  5: 0 0				7  6: 0 0				8  7: 0 0
6  4: 0 0				7  5: 0 0				8  6: 0 0				9  7: 0 0
7  4: 0 0				8  5: 0 0				9  6: 0 0				10 7: 0 0
8  4: 1 0				9  5: 1 0				10 6: 1 0				11 7: 1 0
9  4: 1 0				10 5: 1 0				11 6: 1 0				12 7: 1 0
10 4: 1 0				11 5: 1 0				12 6: 1 0				13 7: 1 0
11 4: 1 0				12 5: 1 0				13 6: 1 0				14 7: 1 0
12 4: 1 1				13 5: 1 1				14 6: 1 1				15 7: 1 1
13 4: 1 1				14 5: 1 1				15 6: 1 1				16 7: 1 1
14 4: 1 1				15 5: 1 1				16 6: 1 1				17 7: 1 1
15 4: 1 1				16 5: 1 1				17 6: 1 1				18 7: 1 1
16 4: 2 1				17 5: 2 1				18 6: 2 1				19 7: 2 1
17 4: 2 1				18 5: 2 1				19 6: 2 1				20 7: 2 1
18 4: 2 1				19 5: 2 1				20 6: 2 1				21 7: 2 1
...						...						...						...						
998 2: 124 124			998 3: 249 ?			998 2: 249 ?			998 3: 249 ?			
999 2: 124 124			999 3: 249 249			999 2: 249 249			999 3: 249 249			
1000 2: 125 124			1000 3: 249 249			1000 2: 250 249			1000 3: 249 249			
X Y: fl(X/8) fl(X-4/8)	X Y: fl(X-1/8) fl(X-5/8)X Y: fl(X-2/8) fl(X-6/8)X Y: fl(X-3/8) fl(X-7/8)

-------------			-------------			-------------
------8------			------9------			------10------
-------------			-------------			-------------
8  8: 0 0				9  9: 0 0				10 10: 0 0
9  8: 0 0				10 9: 0 0				11 10: 0 0
10 8: 0 0				11 9: 0 0				12 10: 0 0
11 8: 0 0				12 9: 0 0				13 10: 0 0
12 8: 0 0				13 9: 0 0				14 10: 0 0
13 8: 0 0				14 9: 0 0				15 10: 0 0
14 8: 0 0				15 9: 0 0				16 10: 0 0
15 8: 0 0				16 9: 0 0				17 10: 0 0
16 8: 1 0				17 9: 1 0				18 10: 1 0
17 8: 1 0				18 9: 1 0				19 10: 1 0
18 8: 1 0				19 9: 1 0				20 10: 1 0
19 8: 1 0				20 9: 1 0				21 10: 1 0
20 8: 1 0				21 9: 1 0				22 10: 1 0
21 8: 1 0				22 9: 1 0				23 10: 1 0
22 8: 1 0				23 9: 1 0				24 10: 1 0


Case #151: 0 0
Case #152: 500 499

*/
