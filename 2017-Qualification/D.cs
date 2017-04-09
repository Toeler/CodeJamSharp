#define START_THIS_FILE

using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJamSharp {
	internal static class D {

#if START_THIS_FILE
		private static void Main() {
#else
		private static void NotMain() {
#endif
			new CodeJamProblem(new CodeJamConfig {
				LinesPerCaseFunc = input => int.Parse(input.Split(' ')[1]),
				//InputFile = "DTest.in",
				//InputFile = "D-small-attempt0.in",
				InputFile = "D-large.in",
				Solver = (testCase, cache) => {
					var n = int.Parse(testCase.LinesPerCaseInput.Split(' ')[0]);

					IList<Cell> grid = Enumerable.Range(0, n*n).Select(i => new Cell(i%n, i/n)).ToList();
					
					foreach (var line in testCase.Input) {
						var split = line.Split(' ');
						var x = int.Parse(split[1]) - 1;
						var y = int.Parse(split[2]) - 1;
						grid[x*n+y].Type = CellTypes[split[0]];
					}

					Dictionary<int, Cell> changes = new Dictionary<int, Cell>();
					foreach (var cell in grid.Where(x => x.Type == CellType.Empty).OrderBy(c => c.SortScore(n))) {
						if (
							!grid.Where(c => c != cell && (c.X + c.Y == cell.X + cell.Y || c.X - c.Y == cell.X - cell.Y))
								 .Any(c => c.Type == CellType.O || c.Type == CellType.Plus)) {
							cell.Type = CellType.Plus;
							changes.Add(cell.X * n + cell.Y, cell);
						} else if(!grid.Where(c => c != cell && (c.X == cell.X || c.Y == cell.Y)).Any(c => c.Type == CellType.O || c.Type == CellType.X)) {
							cell.Type = CellType.X;
							changes.Add(cell.X * n + cell.Y, cell);
						}
					}

					foreach (var cell in grid.Where(x => x.Type != CellType.Empty && x.Type != CellType.O)
						.Where(cell => !grid.Where(c => c != cell && (c.X == cell.X || c.Y == cell.Y)).Any(c => c.Type == CellType.O || c.Type == CellType.X))
						.Where(cell => !grid.Where(c => c != cell && (c.X + c.Y == cell.X + cell.Y || c.X - c.Y == cell.X - cell.Y)).Any(c => c.Type == CellType.O || c.Type == CellType.Plus))) {
						cell.Type = CellType.O;

						if (!changes.ContainsKey(cell.X*n + cell.Y)) {
							changes[cell.X*n + cell.Y] = cell;
						}
					}
					
					//return ScoreGrid(grid) + Environment.NewLine + DebugGrid(grid, n);
					string tmp = changes.Count > 0 ? Environment.NewLine : string.Empty;
					return $"{ScoreGrid(grid)} {changes.Count}{tmp}{string.Join(Environment.NewLine, changes.Values.Select(c => $"{c.ToString()} {c.Y + 1} {c.X + 1}"))}";
				}
			});
		}

		private enum CellType {
			Empty,
			X,
			Plus,
			O
		}
		private static readonly IDictionary<string, CellType> CellTypes = new Dictionary<string, CellType> {
			{ "x", CellType.X },
			{ "+", CellType.Plus },
			{ "o", CellType.O }
		};

		private class Cell {
			public int X;
			public int Y;
			public CellType Type = CellType.Empty;

			public Cell(int x, int y) {
				X = x;
				Y = y;
			}

			public int SortScore(int gridSize) {
				var distanceVert = Math.Min(Y, gridSize - Y - 1);
				var distanceHorz = Math.Min(X, gridSize - X - 1);
				return distanceVert + distanceHorz;
			}

			public override string ToString() {
				switch (Type) {
					case CellType.O: {
						return "o";
					}
					case CellType.Plus: {
						return "+";
					}
					case CellType.X: {
						return "x";
					}
					default: {
						return ".";
					}
				}
			}
		}

		private static int ScoreGrid(IEnumerable<Cell> grid) {
			return grid.Sum(cell => cell.Type == CellType.O ? 2 : cell.Type == CellType.Empty ? 0 : 1);
		}

		private static string DebugGrid(IEnumerable<Cell> grid, int n) {
			return string.Join(string.Empty, grid.Select(cell => cell.ToString() + (cell.X == n-1 ? Environment.NewLine : " ")));
		}
	}
}
