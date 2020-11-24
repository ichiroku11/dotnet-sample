using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SampleTest.Expressions {
	public static class ExpressionHelper {
		// "@object => @object.property"の式からプロパティ名を取得
		public static string GetMemberName<TSource>(Expression<Func<TSource, object>> expression) {
			if (expression.Body is MemberExpression member) {
				return member.Member.Name;

			} else if (expression.Body is UnaryExpression unary) {
				// プロパティが値型の場合はBOX化されるため？
				if (unary.NodeType == ExpressionType.Convert &&
					unary.Operand is MemberExpression operand) {
					return operand.Member.Name;
				}
			}

			// nullで良いか悩む
			return null;
		}

		// "@object => new { @object.property1, @object.property2 }"の式からプロパティ名を取得
		public static IEnumerable<string> GetMemberNames<TSource>(Expression<Func<TSource, object>> expression) {
			if (expression.Body is NewExpression @new) {
				return @new.Members.Select(member => member.Name);
			}

			return Enumerable.Empty<string>();
		}
	}
}
