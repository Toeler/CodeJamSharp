#define START_THIS_FILE

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
				InputFile = "C-small-attempt1.in",
				//InputFile = "C-large.in",
				Solver = (testCase, cache) => {
					var input = testCase.Input[0].Split(' ');
					var j = int.Parse(input[0]);
					var p = int.Parse(input[1]);
					var s = int.Parse(input[2]);
					var k = int.Parse(input[3]);
					
					var outfits = MakeAllPossibleOutfits(j, p, s).ToList();
					var pastOutfits = new List<Outfit>();

					if (k >= s) {
						// Shortcut yay
						pastOutfits.AddRange(outfits);
						outfits.Clear();
					}

					while (outfits.Count > 0) {
						var outfit = outfits.First();
						outfits.Remove(outfit);
						pastOutfits.Add(outfit);
						PurgeOutfits(outfits, pastOutfits, k);
					}

					if (pastOutfits.Count != pastOutfits.Distinct().Count()) {
						throw new Exception("Duplicates in " + string.Join(", ", pastOutfits));
					}

					return $"{pastOutfits.Count}\r\n{string.Join("\r\n", pastOutfits)}";
				}
			});
		}

		private static IEnumerable<Outfit> MakeAllPossibleOutfits(int jacketCount, int pantsCount, int shirtCount) {
			return from i in Enumerable.Range(1, jacketCount)
				   from j in Enumerable.Range(1, pantsCount)
				   from k in Enumerable.Range(1, shirtCount)
				   select new Outfit(i, j, k);
		}

		private static void PurgeOutfits(ICollection<Outfit> outfits, ICollection<Outfit> pastOutfits, int k) {
			for (var i = 0; i < outfits.Count; i++) {
				var outfit = outfits.ElementAt(i);
				if (pastOutfits.Count(o => o.Jacket == outfit.Jacket && o.Pants == outfit.Pants) < k &&
					pastOutfits.Count(o => o.Jacket == outfit.Jacket && o.Shirt == outfit.Shirt) < k &&
					pastOutfits.Count(o => o.Pants  == outfit.Pants  && o.Shirt == outfit.Shirt) < k) {
					continue;
				}
				outfits.Remove(outfit);
				--i;
			}
		}

		private class Outfit {
			public int Jacket { get; }
			public int Pants { get; }
			public int Shirt { get; }

			public Outfit(int jacket, int pants, int shirt) {
				Jacket = jacket;
				Pants = pants;
				Shirt = shirt;
			}

			public override string ToString() {
				return $"{Jacket} {Pants} {Shirt}";
			}
		}
	}
}
