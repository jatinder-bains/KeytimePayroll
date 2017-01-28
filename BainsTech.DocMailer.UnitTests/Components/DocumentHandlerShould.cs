using System;
using BainsTech.DocMailer.Adapters;
using BainsTech.DocMailer.Components;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BainsTech.DocMailer.UnitTests.Components
{
    [TestFixture]
    public class DocumentHandlerShould
    {
        [SetUp]
        public void Initialise()
        {
            logger = Substitute.For<ILogger>();
            configurationSettings = Substitute.For<IConfigurationSettings>();
            fileSystemAdapter = Substitute.For<FileSystemAdapter>();
            documentHandler = new DocumentHandler(logger, configurationSettings, fileSystemAdapter);
        }

        private IDocumentHandler documentHandler;
        private ILogger logger;
        private IConfigurationSettings configurationSettings;
        private IFileSystemAdapter fileSystemAdapter;

        [Test]
        public void ExtractFileNameComponentsTest()
        {
            Assert.Fail();
        }

        [Test]
        public void IsValidFileNameTest()
        {
            Assert.Fail();
        }

        [Test]
        public void MoveDocumentTest()
        {
            Assert.Fail();
        }
        
        [Test]
        [TestCase(null, null)]
        [TestCase(null, "")]
        [TestCase("", null)]
        [TestCase("", "")]
        public void ThrowArgumentNullException_When_GetDocumentsByExtension_IsCalledWithNullOrEmptyFolderNameOrExtension
        (
            string folder, string extension)
        {
            Action action = () => documentHandler.GetDocumentsByExtension(folder, extension);
            action.ShouldThrow<ArgumentNullException>();
        }

    }
}