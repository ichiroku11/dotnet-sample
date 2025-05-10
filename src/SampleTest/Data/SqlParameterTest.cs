using Microsoft.Data.SqlClient;
using System.Data;

namespace SampleTest.Data;

public class SqlParameterTest {
	[Fact]
	public void ParameterName_コンストラクターで指定した文字列を取得できる() {
		// Arrange
		var parameter = new SqlParameter("@p", 1);

		// Act
		// Assert
		Assert.Equal("@p", parameter.ParameterName);
	}

	[Fact]
	public void ParameterName_アットマークは付与されない() {
		// Arrange
		var parameter = new SqlParameter("p", 1);

		// Act
		// Assert
		Assert.Equal("p", parameter.ParameterName);
	}

	[Fact]
	public void Value_値を設定しない場合はnullになる() {
		// Arrange
		var parameter = new SqlParameter("p", SqlDbType.Int);

		// Act
		// Assert
		Assert.Null(parameter.Value);
	}
}
