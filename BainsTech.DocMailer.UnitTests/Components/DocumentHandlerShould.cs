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
        public void ThrowArgumentNullException_When_GettingDocuments_And_FolderNameOrExtensionIsNotSupplied(
            string folder, string extension)
        {
            Action action = () => documentHandler.GetDocuments(folder, extension);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        [TestCase("Paye", "BainsTech", "31-12-16")]
        public void ExtractFileNameComponents_When_GivenAValidPayeFileName(string type, string companyName, string date)
        {
            // Arrange
            var fileName = $"{type} {companyName} {date}.pdf";
            string extractedCompanyName;
            DocumentType extractedDocType;
            string extractedMonth;

            // Act
            var errorMessage = documentHandler.ExtractFileNameComponents(fileName, out extractedCompanyName, out extractedDocType, out extractedMonth);

            // Assert
            errorMessage.Should().BeNullOrEmpty();
            extractedDocType.Should().Be(DocumentType.Paye);
            //extractedMonth.Should().Be(date.Split('-').ToArray()[1]);
        }

        [Test]
        [TestCase("Payroll", "BainsTech", "All", "31-12-16")]
        [TestCase("Payroll", "BainsTech", "jsbainsNic", "30-06-16")]
        [TestCase("Payroll", "BainsTech", "nkbainsNic", "30-06-16")]
        public void ExtractFileNameComponents_When_GivenAValidPayrollFileName(
            string type, 
            string companyName, 
            string employee, 
            string date)
        {
            // Arrange
            var fileName = $"{type} {companyName} {employee} {date}.pdf";
            string extractedCompanyName;
            DocumentType extractedDocType;
            string extractedMonth;

            // Act
            var errorMessage = documentHandler.ExtractFileNameComponents(fileName, out extractedCompanyName, out extractedDocType, out extractedMonth);

            // Assert
            errorMessage.Should().BeNullOrEmpty();
            extractedDocType.Should().Be(DocumentType.Payroll);
            //extractedMonth.Should().Be(date.Split('-').ToArray()[1]);
        }

        [Test]
        [TestCase("OneComponent.pdf")]
        [TestCase("Two Component.pdf")]
        [TestCase("Five Component Invalid File Name.pdf")]
        public void ReturnError_When_ExtractingFileNameComponentsForAFilenameWihoutAmInvalidNumberOfComponents(string badFileName)
        {
            // Arrange
            string extractedCompanyName;
            DocumentType extractedDocType;
            string extractedMonth;

            // Act
            var errorMessage = documentHandler.ExtractFileNameComponents(badFileName, out extractedCompanyName, out extractedDocType, out extractedMonth);

            // Assert
            errorMessage.Should().Be("Unsupported file name format");
        }


        [Test]
        [TestCase("Bad Paye FileName.pdf")]
        [TestCase("Invalid Payroll File Name.pdf")]
        public void ReturnError_When_ExtractingFileNameComponents_AndFilenameIsNotOfType_Paye_Or_Payroll(string badFileName)
        {
            // Arrange
            string extractedCompanyName;
            DocumentType extractedDocType;
            string extractedMonth;

            // Act
            var errorMessage = documentHandler.ExtractFileNameComponents(badFileName, out extractedCompanyName, out extractedDocType, out extractedMonth);

            // Assert
            var suppliedBadDocumentType = badFileName.Split(' ').First();
            errorMessage.Should().Be($"Unsupported document type '{suppliedBadDocumentType}'");
        }

        [Test]
        public void GetDocuments_WhenAValidFolderNameAndExtensionIsSupplied()
        {
            // Arrange
            const string folder = "SomeFolderPath";
            const string extension = "pdf";

            const string gotCompanyName = "ChavAutos";
            const string mappedCompanyEmail = "Kevib@ChavAutos.co.uk";

            var gotFileName = $"{gotCompanyName} payroll 31-12-17.pdf";
            var gotFiles = new[] {$@"c:\SomePath\{gotFileName}"};

            fileSystemAdapterSub.GetFiles(folder, extension).Returns(gotFiles);
            configurationSettingsSub.GetEmailForCompany(gotCompanyName).Returns(mappedCompanyEmail);

            // Act
            var result = documentHandler.GetDocuments(folder, extension).ToList();

            // Assert
            result.Count.Should().Be(1);
            var document = result.First();
            document.EmailAddress.Should().Be(mappedCompanyEmail);
            document.FileName.Should().Be(gotFileName);
            document.FilePath.Should().Be(gotFiles[0]);
            document.Status.Should().Be(DocumentStatus.ReadyToSend);
        }
    }
}