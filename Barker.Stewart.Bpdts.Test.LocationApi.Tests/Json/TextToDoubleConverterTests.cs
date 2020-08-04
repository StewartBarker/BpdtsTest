using Barker.Stewart.Bpdts.Test.LocationApi.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Barker.Stewart.Bpdts.Test.LocationApi.Tests.Json
{
    public class TextToDoubleConverterTests
    {
        public class Deserialize
        {
            [Test]
            public void ShouldDeserialiseToDouble_WhenDoubleIsText()
            {
                // Arrange
                var json = "{ \"TestDouble\": \"32.33\" }";
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new TextToDoubleConverter());

                // Act
                var testObject = JsonSerializer.Deserialize<TestObject>(json, serializeOptions);

                // Assert
                Assert.AreEqual(32.33, testObject.TestDouble);
            }

            [Test]
            public void ShouldDeserialiseToDouble_WhenDoubleIsDouble()
            {
                // Arrange
                var json = "{ \"TestDouble\": 32.33 }";
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new TextToDoubleConverter());

                // Act
                var testObject = JsonSerializer.Deserialize<TestObject>(json, serializeOptions);

                // Assert
                Assert.AreEqual(32.33, testObject.TestDouble);
            }

            [Test]
            public void ShouldDeserialiseToDouble_WhenDoubleIsInt()
            {
                // Arrange
                var json = "{ \"TestDouble\": 32 }";
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new TextToDoubleConverter());

                // Act
                var testObject = JsonSerializer.Deserialize<TestObject>(json, serializeOptions);

                // Assert
                Assert.AreEqual(32, testObject.TestDouble);
            }

            public class TestObject
            {
                public double TestDouble { get; set; }
            }
        }       
    }
}
