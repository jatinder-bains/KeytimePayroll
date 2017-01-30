using System;
using System.Linq;
using BainsTech.DocMailer.Adapters;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.DataObjects;
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
            loggerSub = Substitute.For<ILogger>();
            configurationSettingsSub = Substitute.For<IConfigurationSettings>();
            fileSystemAdapterSub = Substitute.For<IFileSystemAdapter>();
            documentHandler = new DocumentHandler(loggerSub, configurationSettingsSub, fileSystemAdapterSub);
        }

        private IDocumentHandler documentHandler;
        private ILogger loggerSub;
        private IConfigurationSettings configurationSettingsSub;
        private IFileSystemAdapter fileSystemAdapterSub;

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
        public void ThrowArgumentNullException_When_GettingDocumentsByExtension_AnddFolderNameOrExtensionIsNotSupplied(
            string folder, string extension)
        {
            Action action = () => documentHandler.GetDocuments(folder, extension);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ReturnDocument_When_GettingDocumentsByExtension_AnddAValidFolderNameAndExtensionIsSupplied()
        {
            const string folder = "SomeFolderPath";
            const string extension = "pdf";

            const string gotCompanyName = "ChavAutos";
            const string mappedCompanyEmail = "Kevib@ChavAutos.co.uk";

            var gotFileName = $"{gotCompanyName} payroll 31-12-17.pdf";
            var gotFiles = new[] {$@"c:\SomePath\{gotFileName}"};

            fileSystemAdapterSub.GetFiles(folder, extension).Returns(gotFiles);
            configurationSettingsSub.GetEmailForCompany(gotCompanyName).Returns(mappedCompanyEmail);

            var result = documentHandler.GetDocuments(folder, extension).ToList();
            result.Count.Should().Be(1);
            var document = result.First();
            document.EmailAddress.Should().Be(mappedCompanyEmail);
            document.FileName.Should().Be(gotFileName);
            document.FilePath.Should().Be(gotFiles[0]);
            document.Status.Should().Be(DocumentStatus.ReadyToSend);
        }
    }
}