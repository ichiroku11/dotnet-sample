using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Collections;

public class BitArrayTest {
	[Fact]
	public void Length_bool配列から生成したBitArrayの長さを確認する() {
		// Arrange
		// Act
		var bits = new BitArray(new[] { true, false });

		// Assert
		Assert.Equal(2, bits.Length);
	}

	[Fact]
	public void Length_byte配列から生成したBitArrayの長さは8() {
		// Arrange
		// Act
		var bits = new BitArray(new byte[] { 1 });

		// Assert
		Assert.Equal(8, bits.Length);
	}

	public static IEnumerable<object[]> GetTestDataForIndexer() {
		yield return new object[] {
				new byte[] { 1 },
				new [] { true, false, false, false, false, false, false, false }
			};
		yield return new object[] {
				new byte[] { 128 },
				new [] { false, false, false, false, false, false, false, true, }
			};
		yield return new object[] {
				new byte[] { 1, 0 },
				new [] {
					true, false, false, false, false, false, false, false,
					false, false, false, false, false, false, false, false,
				}
			};
	}

	[Theory]
	[MemberData(nameof(GetTestDataForIndexer))]
	public void Indexer_指定したインデックスのbool値を確認する(byte[] bytes, bool[] expected) {
		// Arrange
		var actual = new BitArray(bytes);

		// Act
		// Assert
		Assert.Equal(expected.Length, actual.Length);
		foreach (var index in Enumerable.Range(0, actual.Length)) {
			Assert.Equal(expected[index], actual[index]);
		}
	}
}
