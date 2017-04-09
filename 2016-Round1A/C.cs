#define START_THIS_FILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace CodeJamSharp {
	internal static class C {
#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig() {
				LinesPerCase = 2,
				InputFile = "CTest.in",
				//InputFile = "C-small-attempt0.in",
				//InputFile = "C-large.in",
				Solver = (testCase, cache) => {
					var n = Convert.ToInt32(testCase.Input[0]);
					var f = testCase.Input[1].Split(' ');

					/*if (testCase.CaseNumber != 4) {
						return "";
					}*/

					var ans = 0;
					var chain = new List<List<string>>();
					foreach (var i in Enumerable.Range(0, n)) {
						chain.Add(new List<string> { i.ToString() });
						var nextkid = i + 1;

						while (true) {
							var bff = f[nextkid - 1];
							if (!chain[i].Contains(bff)) {
								chain[i].Add(bff);
								nextkid = Convert.ToInt32(bff);
								continue;
							} else if (bff == chain[i].LastOrDefault()) {
								ans = Math.Max(ans, chain[i].Count);
								break;
							} else if (bff == i.ToString()) {
								ans = Math.Max(ans, chain[i].Count);
								chain[i] = new List<string>();
								break;
							} else {
								chain[i] = new List<string>();
								break;
							}
						}
					}

					foreach (var i in Enumerable.Range(0, n)) {
						if (!chain[i].Any()) {
							continue;
						}

						foreach (var j in Enumerable.Range(i + 1, n - 1 - i)) {
							if (!chain[j].Any()) {
								continue;
							}
							if (chain[i].LastOrDefault() != chain[j].LastOrDefault()) {
								continue;
							}
							if (chain[i].Count >= chain[j].Count) {
								chain[j].Clear();
							} else {
								chain[j].Clear();
								break;
							}
						}
					}

					IEnumerable<string> sitters = new List<string>();
					foreach (var i in Enumerable.Range(0, n)) {
						sitters = sitters.Union(chain[i].Distinct());
					}

					return Math.Max(ans, sitters.Count()).ToString();

					var test = Testing(Enumerable.Range(1, f.Length).ToList());

					Console.WriteLine("---" + testCase.CaseNumber + "---");
					foreach(var item in test.Where(x => x.Count() > 1).OrderBy(x => x.ElementAt(0))/*.Where(x => IsValid(x, f))*/) {
						Console.WriteLine(string.Join(" ", item.Select(x => x.ToString())));
					}

					return test.Where(x => IsValid(x, f)).Max(x => x.Count()).ToString();

					return Solve(string.Empty, Enumerable.Range(0, f.Length).Select(i => new Tuple<string, string>((i + 1).ToString(), f[i]))).ToString();
				}
			});
		}

		private static IEnumerable<IEnumerable<int>> Testing(IList<int> input) {
			return Enumerable.Range(0, (int)Math.Pow(2, input.Count)).Select(i => {
				var temp = Convert.ToString(i, 2).ToCharArray();
				return input.Where((x, pos) => temp.Length >= (pos + 1) && temp[pos] == '1');
			});
		}

		public static List<List<int>> powerSet(List<int> list) {
			if(list == null)
				throw new ArgumentNullException("list may not be null.");

			return powerSet(list, list.Count);
		}

		public static List<List<int>> powerSet(List<int> list, int j) {
			if(list == null)
				throw new ArgumentNullException("list may not be null.");

			if(j < 0)
				throw new ArgumentOutOfRangeException("j must be not be negative.");

			List<List<int>> results = new List<List<int>>() { new List<int>(list) };

			for(int i = 0; i < j; i++) {
				int x = list[i];
				list.RemoveAt(i);
				results.AddRange(powerSet(list, i));
				list.Insert(i, x);
			}

			return results;
		}

		private static bool IsValid(IEnumerable<int> circle, string[] bffs) {
			var enumerable = circle as IList<int> ?? circle.ToList();
			for (var index = 0; index < enumerable.Count; index++) {
				var child = enumerable.ElementAt(index);
				var left = enumerable.ElementAt(index == 0 ? enumerable.Count - 1 : index - 1);
				var right = enumerable.ElementAt(index == enumerable.Count - 1 ? 0 : index + 1);
				var bff = bffs[child - 1];

				if (left.ToString() == bff || right.ToString() == bff) {
					continue;
				}

				return false;
			}
			return true;
		}

		private static IEnumerable<IEnumerable<T>> GetPowerSet<T>(List<T> list) {
			return from m in Enumerable.Range(0, 1 << list.Count)
				   select
					   from i in Enumerable.Range(0, list.Count)
					   where (m & (1 << i)) != 0
					   select list[i];
		}

		private static IEnumerable<IEnumerable<T>> Subsets<T>(IEnumerable<T> source) {
			List<T> list = source.ToList();
			int length = list.Count;
			int max = (int)Math.Pow(2, list.Count);

			for(int count = 0; count < max; count++) {
				List<T> subset = new List<T>();
				uint rs = 0;
				while(rs < length) {
					if((count & (1u << (int)rs)) > 0) {
						subset.Add(list[(int)rs]);
					}
					rs++;
				}
				yield return subset;
			}
		}

		public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items) {
			if(items.Count() > 1) {
				return items.SelectMany(item => GetPermutations(items.Where(i => !i.Equals(item))),
									   (item, permutation) => new[] { item }.Concat(permutation));
			} else {
				return new[] { items };
			}
		}

		static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences) {
			IEnumerable<IEnumerable<T>> emptyProduct = new[] {Enumerable.Empty<T>()};
			return sequences.Aggregate(
				emptyProduct,
				(accumulator, sequence) =>
					from accseq in accumulator
					from item in sequence
					select accseq.Concat(new[] {item}));
		}

		private static int Solve(string chain, IEnumerable<Tuple<string, string>> pairs) {
			var split = chain.Split(' ');
			var current = split.LastOrDefault();
			var isStart = current == string.Empty;

			if (!pairs.Any(p => isStart || p.Item1 == current || p.Item2 == current)) {
				return current == split.FirstOrDefault() ? split.Length - 1 : 0;
			}

			var max = 0;
			foreach (var pair in pairs.Where(p => isStart || p.Item1 == current || p.Item2 == current)) {
				if (isStart || pair.Item1 == current) {
					max = Math.Max(max, Solve((isStart ? pair.Item1 : chain) + ' ' + pair.Item2, pairs.Where(p => !p.Equals(pair))));
				}
				if (isStart || pair.Item2 == current) {
					max = Math.Max(max, Solve((isStart ? pair.Item2 : chain) + ' ' + pair.Item1, pairs.Where(p => !p.Equals(pair))));
				}

				if (isStart && max == pairs.Count()) {
					return max;
				}
			}
			return max;
		}
	}
}
