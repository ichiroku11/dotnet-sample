using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Xunit;

namespace SampleTest {
	public class DynamicObjectTest {
		private class DynamicBag : DynamicObject {
			private readonly Dictionary<string, object> _storage = new Dictionary<string, object>();

			public override bool TryGetMember(GetMemberBinder binder, out object result) {
				//return base.TryGetMember(binder, out result);

				if (_storage.ContainsKey(binder.Name)) {
					result = _storage[binder.Name];
					return true;
				}

				result = null;
				return false;
			}

			public override bool TrySetMember(SetMemberBinder binder, object value) {
				//return base.TrySetMember(binder, value);

				if (_storage.ContainsKey(binder.Name)) {
					_storage[binder.Name] = value;
				} else {
					_storage.Add(binder.Name, value);
				}

				return true;
			}
		}

		[Fact]
		public void TryGetMemberTrySetMember_DynamicObjectを使ってみる() {
			// Arrange
			dynamic bag = new DynamicBag();

			// Act
			bag.Name = "Abc";

			// Assert
			Assert.Equal("Abc", bag.Name);
		}
	}
}
