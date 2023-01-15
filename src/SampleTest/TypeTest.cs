namespace SampleTest;

public class TypeTest {
	private class Base {
	}

	private interface IInterface {
	}

	private class Derived : Base, IInterface {
	}

	[Fact]
	public void IsAssignableFrom() {
		// Base型の変数に、Derived型のインスタンスを割り当てることができる
		Assert.True(typeof(Base).IsAssignableFrom(typeof(Derived)));

		// Derived型の変数に、Base型のインスタンスを割り当てることができない
		Assert.False(typeof(Derived).IsAssignableFrom(typeof(Base)));

		// Base型の変数に、Base型のインスタンスを割り当てることができる
		Assert.True(typeof(Base).IsAssignableFrom(typeof(Base)));

		// IInterface型の変数に、Derived型のインスタンスを割り当てることができる
		Assert.True(typeof(IInterface).IsAssignableFrom(typeof(Derived)));
	}

	[Fact]
	public void IsAssignableTo() {
		// Base型は、Derived型に割り当てることができない
		Assert.False(typeof(Base).IsAssignableTo(typeof(Derived)));

		// Derived型は、Base型に割り当てることができる
		Assert.True(typeof(Derived).IsAssignableTo(typeof(Base)));

		// IInterface型は、Derived型に割り当てることができない
		Assert.False(typeof(IInterface).IsAssignableTo(typeof(Derived)));

		// Derived型は、IInterface型に割り当てることができる
		Assert.True(typeof(Derived).IsAssignableTo(typeof(IInterface)));

		// 同じ型は割り当てることができる
		Assert.True(typeof(Base).IsAssignableTo(typeof(Base)));
		Assert.True(typeof(Derived).IsAssignableTo(typeof(Derived)));
		Assert.True(typeof(IInterface).IsAssignableTo(typeof(IInterface)));
	}

	[Fact]
	public void IsSubclassOf() {
		// BaseはDerivedのサブクラスではない
		Assert.False(typeof(Base).IsSubclassOf(typeof(Derived)));

		// DerivedはBaseのサブクラスである
		Assert.True(typeof(Derived).IsSubclassOf(typeof(Base)));

		// Derived（インターフェイスの実装）はIInterfaceのサブクラスではない
		Assert.False(typeof(Derived).IsSubclassOf(typeof(IInterface)));
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
