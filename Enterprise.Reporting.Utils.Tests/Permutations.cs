using System.Linq;

namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class PermutationsTests
{
    [TestMethod]
    public void IntegerPermutations()
    {
        var x = new int[] { 1, 2, 3 };

        var permutations = Permutations.GetPermutations(x, 3);

        var benchmark = new int[][] { [1, 2, 3], [1, 3, 2], [2, 1, 3], [2, 3, 1], [3, 1, 2], [3, 2, 1] }; 

        Assert.AreEqual(permutations.Count(), 6);

        foreach (var sequence in permutations)
            Assert.IsTrue(benchmark.Any(y => y.SequenceEqual(sequence)));

        foreach (var sequence in benchmark)
            Assert.IsTrue(permutations.Any(y => y.SequenceEqual(sequence)));
    }

    [TestMethod]
    public void StringPermutations()
    {
        var x = new string[] { "a", "b", "c" };

        var permutations = Permutations.GetPermutations(x, 3);

        var benchmark = new string[][] { ["a", "b", "c"], ["a", "c", "b"], ["b", "a", "c"], ["b", "c", "a"], ["c", "a", "b"], ["c", "b", "a"] };

        Assert.AreEqual(permutations.Count(), 6);

        foreach (var sequence in permutations)
            Assert.IsTrue(benchmark.Any(y => y.SequenceEqual(sequence)));

        foreach (var sequence in benchmark)
            Assert.IsTrue(permutations.Any(y => y.SequenceEqual(sequence)));
    }
}
