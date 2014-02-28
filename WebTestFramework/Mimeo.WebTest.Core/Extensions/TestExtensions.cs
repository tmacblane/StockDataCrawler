using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace Mimeo.TestExtensions
{
	public static class TestExtensions
	{
		#region Type specific methods

		public static void ShouldBe<T>(this T actual, T expected)
		{
			Assert.AreEqual<T>(expected, actual);
		}

		public static string ShouldEqual(this string actual, string expected)
		{
			Assert.AreEqual(expected, actual);
			return expected;
		}

		public static void ShouldBeFalse(this bool condition, string message = "")
		{
			Assert.IsFalse(condition, message);
		}

		public static void ShouldBeTrue(this bool condition, string message = "")
		{
			Assert.IsTrue(condition, message);
		}

		public static object ShouldEqual(this object actual, object expected)
		{
			Assert.AreEqual(expected, actual);
			return expected;
		}

		public static object ShouldNotEqual(this object actual, object expected)
		{
			Assert.AreNotEqual(expected, actual);
			return expected;
		}

		public static void ShouldBeNull(this object anObject)
		{
			Assert.IsNull(anObject);
		}

		public static void ShouldNotBeNull(this object anObject)
		{
			Assert.IsNotNull(anObject);
		}

		public static object ShouldBeTheSameAs(this object actual, object expected)
		{
			Assert.AreSame(expected, actual);
			return expected;
		}

		public static object ShouldNotBeTheSameAs(this object actual, object expected)
		{
			Assert.AreNotSame(expected, actual);
			return expected;
		}

		public static void ShouldBeOfType(this object actual, Type expectedType)
		{
			Assert.IsInstanceOfType(actual, expectedType);
		}

		public static void ShouldNotBeOfType(this object actual, Type expectedType)
		{
			Assert.IsNotInstanceOfType(actual, expectedType);
		}

		public static void ShouldContain(this ICollection actual, object expected)
		{
			CollectionAssert.Contains(actual, expected);
		}

		public static void ShouldNotContain(this IList collection, object expected)
		{
			CollectionAssert.DoesNotContain(collection, expected);
		}

		public static void ShouldBeEmpty(this ICollection collection)
		{
			Assert.IsTrue(collection.Count == 0);
		}

		public static void ShouldBeEmpty(this string aString)
		{
			Assert.IsTrue(string.IsNullOrEmpty(aString));
		}

		public static void ShouldNotBeEmpty(this ICollection collection)
		{
			Assert.IsTrue(collection.Count > 0);
		}

		public static void ShouldNotBeEmpty(this string aString)
		{
			Assert.IsFalse(string.IsNullOrEmpty(aString));
		}

		public static void ShouldContain(this string actual, string expected)
		{
			StringAssert.Contains(expected, actual);
		}

		public static void ShouldStartWith(this string actual, string expected)
		{
			StringAssert.StartsWith(expected, actual);
		}

		public static void ShouldEndWith(this string actual, string expected)
		{
			StringAssert.EndsWith(expected, actual);
		}

		public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter, string expectedEndDelimiter)
		{
			StringAssert.StartsWith(expectedStartDelimiter, actual);
			StringAssert.EndsWith(expectedEndDelimiter, actual);
		}

		public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
		{
			StringAssert.StartsWith(expectedDelimiter, actual);
			StringAssert.EndsWith(expectedDelimiter, actual);
		}

		public static void ShouldContainErrorMessage(this Exception exception, string expected)
		{
			StringAssert.Contains(expected, exception.Message);
		}

		#endregion
	}
}