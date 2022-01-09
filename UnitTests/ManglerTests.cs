using AutoIncorrect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class ManglerTests
    {
        private const int EnduranceIterationCount = 1000000;
        private const int MaxTermLength = 10;
        private const int MaxRemangleCount = 5;
        private const string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private Random _random;
        private string _originalTerm;

        private Mangler _mangler;

        [TestInitialize]
        public void TestInitialize()
        {
            _random = new Random();
            _originalTerm = "Proset";
            _mangler = new Mangler(_originalTerm);
        }

        [TestMethod]
        public void UnMangledShouldBeSameAsOriginalTerm()
        {
            Assert.AreEqual(_originalTerm, _mangler.Unmangled, "Unexpected unmangled term.");
        }

        [TestMethod]
        public void MangledShouldBeDifferentFromUnmangled()
        {
            Assert.AreNotEqual(_mangler.Mangled, _mangler.Unmangled, "Mangled and unmangled terms must not be equal.");
        }

        [TestMethod]
        public void NullOrEmptyTermShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() => new Mangler(""), "Empty term did not throw.");
            Assert.ThrowsException<ArgumentException>(() => new Mangler(null), "Null term did not throw.");
        }

        [TestMethod]
        public void ToStringShouldReturnMangled()
        {
            Assert.AreEqual(_mangler.Mangled, _mangler.ToString(), "Unexpected mangler string representation.");
        }

        [TestMethod]
        public void MangledShouldBeDifferentFromUnmangled_Endurance()
        {
            for(var iteration = 0; iteration < EnduranceIterationCount; ++iteration)
            {
                var termLength = _random.Next(1, MaxTermLength);
                var term = string.Empty;
                for(var charIndex = 0; charIndex < termLength; ++charIndex)
                {
                    term = term.Insert(term.Length, CharSet[_random.Next(CharSet.Length)].ToString());
                }
                _mangler = new Mangler(term);
                MangledShouldBeDifferentFromUnmangled();
            }
        }

        [TestMethod]
        public void RemangledShouldBeDifferentFromUnmangled_Endurance()
        {
            for (var iteration = 0; iteration < EnduranceIterationCount; ++iteration)
            {
                var termLength = _random.Next(1, MaxTermLength);
                var term = string.Empty;
                for (var charIndex = 0; charIndex < termLength; ++charIndex)
                {
                    term = term.Insert(term.Length, CharSet[_random.Next(CharSet.Length)].ToString());
                }
                _mangler = new Mangler(term);
                var remangleCount = _random.Next(MaxRemangleCount);
                for (var remangleIteration = 0; remangleIteration <= remangleCount; ++remangleIteration)
                {
                    _mangler.Mangle();
                }
                MangledShouldBeDifferentFromUnmangled();
            }
        }

        [TestMethod]
        public void MangledShouldNeverBeNullOrEmptyString()
        {
            for (var iteration = 0; iteration < EnduranceIterationCount; ++iteration)
            {
                var term = CharSet[_random.Next(CharSet.Length)].ToString();
                _mangler = new Mangler(term);
                Assert.IsFalse(string.IsNullOrEmpty(_mangler.Mangled), "Mangled term is null or empty.");
            }
        }
    }
}
