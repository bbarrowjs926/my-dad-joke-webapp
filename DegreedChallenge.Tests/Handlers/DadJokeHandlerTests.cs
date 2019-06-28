using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DegreedChallenge.Handlers;
using DegreedChallenge.Models;
using DegreedChallenge.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DegreedChallenge.Tests.Handlers
{
    /// <summary>
    /// Summary description for DadJokeHandlerTests
    /// </summary>
    [TestClass]
    public class DadJokeHandlerTests
    {
        private Mock<IJokeService> _jokeService;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            _jokeService = new Mock<IJokeService>();
        }

        #region GetRandomJoke Tests

        [TestMethod]
        public void VerifyGetRandomJokeHydratesModel()
        {
            // Arrange
            _jokeService.Setup(s => s.GetRandomJoke()).Returns(Task.FromResult(new DadJoke
                {
                    Id = "1235",
                    Joke = "Knock knock...whos there..."
                })
            );

            var handler = new DadJokeHandler(_jokeService.Object);

            // Act
            var vm = handler.GetRandomJoke();

            // Assert
            Assert.IsTrue(!string.IsNullOrWhiteSpace(vm.Joke));
        }
        #endregion

        #region GetJokesWithTerm Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void VerifyGetJokesWithTermThrowsApplicationErrorWhenASearchIsPerformedWithNoTerm()
        {
            // Arrange
            var handler = new DadJokeHandler(_jokeService.Object);

            // Act
            var vm = handler.GetJokesWithTerm(string.Empty);
        }

        [TestMethod]
        public void VerifyGetJokesWithTermHydratesVMWhenNoMatchingJokesWithTermFound()
        {
            // Arrange
            var jokes = new List<DadJoke>();
            string jokeTerm = "Knock";
            _jokeService.Setup(s => s.GetJokesWithTerm(It.IsAny<string>())).Returns(Task.FromResult<IEnumerable<DadJoke>>(jokes));
            var handler = new DadJokeHandler(_jokeService.Object);
            
            // Act
            var vm = handler.GetJokesWithTerm(jokeTerm);

            // Assert
            Assert.AreEqual(jokeTerm, vm.Term);
            Assert.IsFalse(vm.JokesFound);
        }


        [TestMethod]
        public void VerifyGetJokesWithTermHydratesVMAndPutsJokeIntoCorrectJokeSizeGrouping()
        {
            var foundJokes = new List<DadJoke>
            {
                new DadJoke
                {
                    Id = "123",
                    Joke = "Knock, Knock, whose there"
                },
                new DadJoke
                {
                    Id = "456",
                    Joke = "The wheels on the bus go knock and knock, I mean round and round"
                },
                new DadJoke
                {
                    Id = "987",
                    Joke = "This is a very long joke that takes a lot of time to develop and by the time you get to the punchline you would have been better off with knock going with something else"
                }
            };

            // Arrange
            string jokeTerm = "Knock";
            _jokeService.Setup(s => s.GetJokesWithTerm(It.IsAny<string>())).Returns(Task.FromResult<IEnumerable<DadJoke>>(foundJokes));
            var handler = new DadJokeHandler(_jokeService.Object);

            // Act
            var vm = handler.GetJokesWithTerm(jokeTerm);

            // Assert
            Assert.AreEqual(jokeTerm, vm.Term);
            Assert.IsTrue(vm.JokesFound);
            Assert.IsTrue(vm.ShortJokes.Count == 1 && vm.MediumJokes.Count == 1 && vm.LargeJokes.Count == 1);
        }

        #endregion

        #region GetWordCount Tests

        [TestMethod]
        public void VerifyGetWordCountWorks()
        {
            // Arrange
            var handler = new DadJokeHandler(_jokeService.Object);

            // Act
            int count = handler.GetWordCount("How now brown cow");

            // Assert
            Assert.IsTrue(count == 4);
        }

        [TestMethod]
        public void VerifyGetWordCountWorksWithNoWords()
        {
            // Arrange
            var handler = new DadJokeHandler(_jokeService.Object);

            // Act
            int count = handler.GetWordCount(string.Empty);

            // Assert
            Assert.IsTrue(count == 0);
        }
        #endregion

        #region FormatJokeForDisplay Tests
        [TestMethod]
        public void VerifyFormatJokeForDisplayWrapsTermWithSpanTags()
        {
            // Arrange
            string joke = "Why did the chicken cross the road? To get to the other side.";
            string expectedValue = "Why did the <span class='look-at-me'>chicken</span> cross the road? To get to the other side.";
            string jokeTerm = "chicken";
            var handler = new DadJokeHandler(_jokeService.Object);

            // Act
            var vm = handler.FormatJokeForDisplay(joke, jokeTerm);

            // Assert
            Assert.AreEqual(vm.Joke, expectedValue);
        }
        #endregion
    }
}
