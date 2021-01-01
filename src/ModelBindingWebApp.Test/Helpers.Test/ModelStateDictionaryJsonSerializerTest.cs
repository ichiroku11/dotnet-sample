using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using Xunit;

namespace ModelBindingWebApp.Helpers.Test {
	public class ModelStateDictionaryJsonSerializerTest {
		// RawValue is null
		private const string _jsonRawValueIsNull
			= @"[{""key"":""k"",""rawValues"":[],""attemptedValue"":""a"",""errorMessages"":[]}]";

		[Fact(DisplayName = "Serialize_RawValueがnullの場合に正しく処理できる")]
		public void Serialize_RawValueIsNull() {
			// Arrange
			var modelStates = new ModelStateDictionary();
			modelStates.SetModelValue("k", null, "a");

			// Act
			var json = ModelStateDictionaryJsonSerializer.Serialize(modelStates);

			// Assert
			Assert.Equal(_jsonRawValueIsNull, json);
		}

		[Fact(DisplayName = "Deserialize_RawValueがnullの場合に正しく処理できる")]
		public void Deserialize_RawValueIsNull() {
			// Arrange
			// Act
			var modelStates = ModelStateDictionaryJsonSerializer.Deserialize(_jsonRawValueIsNull);

			// Assert
			Assert.Single(modelStates);

			var (key, modelState) = modelStates.First();
			Assert.Equal("k", key);

			Assert.Null(modelState.RawValue);

			Assert.Equal("a", modelState.AttemptedValue);

			Assert.Empty(modelState.Errors);
		}

		// RawValue is string
		private const string _jsonRawValueIsString
			= @"[{""key"":""k"",""rawValues"":[""r""],""attemptedValue"":""a"",""errorMessages"":[]}]";

		[Fact(DisplayName = "Serialize_RawValueがstringの場合に正しく処理できる")]
		public void Serialize_RawValueIsString() {
			// Arrange
			var modelStates = new ModelStateDictionary();
			modelStates.SetModelValue("k", "r", "a");

			// Act
			var json = ModelStateDictionaryJsonSerializer.Serialize(modelStates);

			// Assert
			Assert.Equal(_jsonRawValueIsString, json);
		}

		[Fact(DisplayName = "Deserialize_RawValueがstringの場合に正しく処理できる")]
		public void Deserialize_RawValueIsString() {
			// Arrange
			// Act
			var modelStates = ModelStateDictionaryJsonSerializer.Deserialize(_jsonRawValueIsString);

			// Assert
			Assert.Single(modelStates);

			var (key, modelState) = modelStates.First();
			Assert.Equal("k", key);

			Assert.IsType<string>(modelState.RawValue);
			Assert.Equal("r", modelState.RawValue);

			Assert.Equal("a", modelState.AttemptedValue);

			Assert.Empty(modelState.Errors);
		}

		// RawValue is string[]
		private const string _jsonRawValueIsStringArray
			= @"[{""key"":""k"",""rawValues"":[""r1"",""r2""],""attemptedValue"":""a"",""errorMessages"":[]}]";

		[Fact(DisplayName = "Serialize_RawValueがstring[]の場合に正しく処理できる")]
		public void Serialize_RawValueIsStringArray() {
			// Arrange
			var modelStates = new ModelStateDictionary();
			modelStates.SetModelValue("k", new[] { "r1", "r2" }, "a");

			// Act
			var json = ModelStateDictionaryJsonSerializer.Serialize(modelStates);

			// Assert
			Assert.Equal(_jsonRawValueIsStringArray, json);
		}

		[Fact(DisplayName = "Deserialize_RawValueがstring[]の場合に正しく処理できる")]
		public void Deserialize_RawValueIsStringArray() {
			// Arrange
			// Act
			var modelStates = ModelStateDictionaryJsonSerializer.Deserialize(_jsonRawValueIsStringArray);

			// Assert
			Assert.Single(modelStates);

			var (key, modelState) = modelStates.First();
			Assert.Equal("k", key);

			Assert.IsType<string[]>(modelState.RawValue);
			var rawValues = modelState.RawValue as string[];
			Assert.Equal(2, rawValues.Length);
			Assert.Equal("r1", rawValues[0]);
			Assert.Equal("r2", rawValues[1]);

			Assert.Equal("a", modelState.AttemptedValue);

			Assert.Empty(modelState.Errors);
		}
	}
}
