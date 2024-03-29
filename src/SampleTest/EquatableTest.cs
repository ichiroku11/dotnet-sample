namespace SampleTest;

public class EquatableTest {
	// クイックアクションでEquals、GetHashCode、演算子を生成できる
	private class Vector(int x, int y) : IEquatable<Vector> {
		public int X { get; } = x;
		public int Y { get; } = y;

		public override bool Equals(object? obj) => Equals(obj as Vector);

		public bool Equals(Vector? other) {
			return
				other != null &&
				X == other.X &&
				Y == other.Y;
		}

		public override int GetHashCode() => HashCode.Combine(X, Y);

		public static bool operator ==(Vector? left, Vector? right) {
			return EqualityComparer<Vector>.Default.Equals(left, right);
		}

		public static bool operator !=(Vector? left, Vector? right) {
			return !(left == right);
		}
	}

	[Fact]
	public void Equals_正しく比較できる() {
		// Arrange
		// Act
		// Assert
		Assert.True(new Vector(1, 1).Equals(new Vector(1, 1)));
		Assert.False(new Vector(1, 1).Equals(new Vector(1, 2)));
	}

	[Fact]
	public void OperatorEquality_正しく比較できる() {
		// Arrange
		// Act
		// Assert
		Assert.True(new Vector(1, 1) == new Vector(1, 1));
		Assert.False(new Vector(1, 1) == new Vector(1, 2));
	}
}
