using System;
using Xunit;

namespace SampleTest {
	public class TypeTest {
		private class Base {
		}

		private interface IInterface {
		}

		private class Derived : Base, IInterface {
		}

		[Fact]
		public void IsAssignableFrom() {
			var baseType = typeof(Base);
			var derivedType = typeof(Derived);
			var interfaceType = typeof(IInterface);

			// Base型の変数に、Derived型のインスタンスを割り当てることができる
			Assert.True(baseType.IsAssignableFrom(derivedType));

			// Derived型の変数に、Base型のインスタンスを割り当てることができない
			Assert.False(derivedType.IsAssignableFrom(baseType));

			// Base型の変数に、Base型のインスタンスを割り当てることができる
			Assert.True(baseType.IsAssignableFrom(baseType));

			// IInterface型の変数に、Derived型のインスタンスを割り当てることができる
			Assert.True(interfaceType.IsAssignableFrom(derivedType));
		}

		[Fact]
		public void IsSubclassOf() {
			var baseType = typeof(Base);
			var derivedType = typeof(Derived);
			var interfaceType = typeof(IInterface);

			// BaseはDerivedのサブクラスではない
			Assert.False(baseType.IsSubclassOf(derivedType));

			// DerivedはBaseのサブクラスである
			Assert.True(derivedType.IsSubclassOf(baseType));

			// Derived（インターフェイスの実装）はIInterfaceのサブクラスではない
			Assert.False(derivedType.IsSubclassOf(interfaceType));
		}

		// リフレクションではないけど
		[Fact]
		public void IsOperator() {
			var @base = new Base();
			var derived = new Derived();

			Assert.True(@base is Base);
			Assert.True(derived is Base);
			Assert.True(derived is IInterface);
		}
	}
}
