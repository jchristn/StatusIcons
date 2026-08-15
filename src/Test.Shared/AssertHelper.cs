namespace Test.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Custom assertion helpers for test methods.
    /// Each method throws an Exception on failure with a descriptive message.
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Assert that a value is not null.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void IsNotNull(object value, string name)
        {
            if (value == null)
                throw new Exception($"Expected {name} to be non-null, but was null.");
        }

        /// <summary>
        /// Assert that a value is null.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void IsNull(object value, string name)
        {
            if (value != null)
                throw new Exception($"Expected {name} to be null, but was '{value}'.");
        }

        /// <summary>
        /// Assert that two values are equal.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void AreEqual<T>(T expected, T actual, string name)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                throw new Exception($"Expected {name} to be '{expected}', but was '{actual}'.");
        }

        /// <summary>
        /// Assert that two values are not equal.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="expected">Value that actual should not equal.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void AreNotEqual<T>(T expected, T actual, string name)
        {
            if (EqualityComparer<T>.Default.Equals(expected, actual))
                throw new Exception($"Expected {name} to differ from '{expected}', but they were equal.");
        }

        /// <summary>
        /// Assert that two object references are the same instance.
        /// </summary>
        /// <param name="expected">Expected reference.</param>
        /// <param name="actual">Actual reference.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void AreSame(object expected, object actual, string name)
        {
            if (!ReferenceEquals(expected, actual))
                throw new Exception($"Expected {name} to be the same instance, but references differed.");
        }

        /// <summary>
        /// Assert that two object references are not the same instance.
        /// </summary>
        /// <param name="expected">Reference that actual should not equal.</param>
        /// <param name="actual">Actual reference.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void AreNotSame(object expected, object actual, string name)
        {
            if (ReferenceEquals(expected, actual))
                throw new Exception($"Expected {name} to be different instances, but references were the same.");
        }

        /// <summary>
        /// Assert that a condition is true.
        /// </summary>
        /// <param name="value">Condition to check.</param>
        /// <param name="message">Description of the assertion.</param>
        public static void IsTrue(bool value, string message)
        {
            if (!value)
                throw new Exception($"Assertion failed: {message}");
        }

        /// <summary>
        /// Assert that a condition is false.
        /// </summary>
        /// <param name="value">Condition to check.</param>
        /// <param name="message">Description of the assertion.</param>
        public static void IsFalse(bool value, string message)
        {
            if (value)
                throw new Exception($"Assertion failed (expected false): {message}");
        }

        /// <summary>
        /// Assert that a string contains a substring.
        /// </summary>
        /// <param name="actual">String to search.</param>
        /// <param name="substring">Expected substring.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void StringContains(string actual, string substring, string name)
        {
            IsNotNull(actual, name);
            if (!actual.Contains(substring))
                throw new Exception($"Expected {name} to contain '{substring}', but was '{actual}'.");
        }

        /// <summary>
        /// Assert that a collection has a specific count.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="collection">Collection to check.</param>
        /// <param name="expected">Expected count.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void HasCount<T>(ICollection<T> collection, int expected, string name)
        {
            IsNotNull(collection, name);
            if (collection.Count != expected)
                throw new Exception($"Expected {name} to have {expected} items, but had {collection.Count}.");
        }

        /// <summary>
        /// Assert that a collection contains a specific key or element.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <param name="value">Value expected to be present.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void Contains<T>(ICollection<T> collection, T value, string name)
        {
            IsNotNull(collection, name);
            if (!collection.Contains(value))
                throw new Exception($"Expected {name} to contain '{value}', but it did not.");
        }

        /// <summary>
        /// Assert that a collection does not contain a specific key or element.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <param name="value">Value expected to be absent.</param>
        /// <param name="name">Descriptive name for error messages.</param>
        public static void DoesNotContain<T>(ICollection<T> collection, T value, string name)
        {
            IsNotNull(collection, name);
            if (collection.Contains(value))
                throw new Exception($"Expected {name} to not contain '{value}', but it did.");
        }

        /// <summary>
        /// Assert that a synchronous action throws a specific exception type.
        /// </summary>
        /// <typeparam name="TException">Expected exception type.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <param name="description">Description of the assertion.</param>
        public static void Throws<TException>(Action action, string description) where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new Exception($"Expected {typeof(TException).Name} for: {description}, but caught {ex.GetType().Name}: {ex.Message}");
            }

            throw new Exception($"Expected {typeof(TException).Name} for: {description}, but no exception was thrown.");
        }

        /// <summary>
        /// Assert that an async action throws a specific exception type.
        /// </summary>
        /// <typeparam name="TException">Expected exception type.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <param name="description">Description of the assertion.</param>
        public static async Task ThrowsAsync<TException>(Func<Task> action, string description) where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                await action().ConfigureAwait(false);
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new Exception($"Expected {typeof(TException).Name} for: {description}, but caught {ex.GetType().Name}: {ex.Message}");
            }

            throw new Exception($"Expected {typeof(TException).Name} for: {description}, but no exception was thrown.");
        }
    }
}
