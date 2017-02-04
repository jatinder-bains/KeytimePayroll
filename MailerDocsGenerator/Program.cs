using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MailerDocsGenerator
{
    internal class Program
    {
        private const string PayeDocsDir = @"TestData\PAYE";
        private const string PayeDocsSentDir = @"TestData\PAYE\Sent";
        private const string PayrollDocsDir = @"TestData\Payroll";
        private const string PayerollSentDir = @"TestData\Payroll\Sent";
        private static MailerTestData testData;

        private static void Main(string[] args)
        {
            Initialise();
            AddCompanies();
            CreateDocuments();

            using (var myWriter = new StreamWriter(@"DocMailerTestData.xml", false))
            {
                var mySerializer = new XmlSerializer(typeof(MailerTestData));
                mySerializer.Serialize(myWriter, testData);
            }

            Console.WriteLine("==== DONE ====");
            Console.WriteLine("Press any key to exit and see Test Mode config settings for Doc Mailer");
            Console.Read();
            Process.Start("ConfigSettings.txt");
        }

        private static void Initialise()
        {
            testData = new MailerTestData();
            EnsureDirectory(PayeDocsDir);
            EnsureDirectory(PayeDocsSentDir);
            EnsureDirectory(PayrollDocsDir);
            EnsureDirectory(PayerollSentDir);
        }

        private static void EnsureDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        private static void AddCompanies()
        {
            Console.WriteLine("Adding companies...");
            var companies = new List<MailerTestDataCompany>(100);
            for (var i = 1; i <= 100; i++)
            {
                var name = $"Company{i}";

                var email = i % 10 == 0 ? null : $"info@{name}.co.uk";
                companies.Add(new MailerTestDataCompany {Name = name, Email = email});
            }

            testData.Companies = companies.ToArray();

            var companyEmailMappings = new List<string>
            {
                "**INSTRUCTIONS  FOR RUNNING DOC MAILER IN TEST MODE ***",
                "<!-- Enable Test Mode -->",
                "<add key=\"TestMode\" value=\"true\"/>",
                "",
                $"<!-- Change DocumentsLocation to '{PayeDocsDir}' or '{PayeDocsDir}' -->",
                "<!-- Start - Test Mode Email Mappings -->"
            };

            companyEmailMappings.AddRange(companies
                .Where(c => c.Email != null)
                .Select(c => $"<add key=\"{c.Name}\" value=\"{c.Email}\" />"));

            companyEmailMappings.Add("<!-- End - Test Mode Email Mappings -->");

            File.WriteAllLines("ConfigSettings.txt", companyEmailMappings);

            Console.WriteLine("Added companies");
        }

        private static string DateToMonthName(string date)
        {
            string[] months =
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

            return months[int.Parse(date.Split('-')[1]) - 1];
        }

        private static void CreateDocuments()
        {
            Console.WriteLine("Adding PAYE documents...");
            var dates = new[]
            {
                "31-03-16" //, "31-03-15", "30-06-16", "30-06-15", "31-12-16"
            };

            var payeDocs = new List<MailerTestDataPayeDocument>();
            foreach (var company in testData.Companies)
            {
                Console.WriteLine($"Adding PAYE documents for company {company.Name}...");
                // Create PAYE docs
                for (var datesIdx = 0; datesIdx < dates.Length; datesIdx++)
                {
                    var date = dates[datesIdx];
                    var fileName = $@"{PayeDocsDir}\Paye {company.Name} {date}.pdf";
                    File.Copy("DocTemplates\\PAYE.pdf", fileName);

                    var expectedSubject = $"{company.Name} PAYE {DateToMonthName(date)}";
                    payeDocs.Add(new MailerTestDataPayeDocument
                    {
                        FileName = fileName,
                        SendTo = company.Email,
                        ExpectedSubject = expectedSubject
                    });
                }
            }

            testData.PayeDocuments = payeDocs.ToArray();
            Console.WriteLine("Added PAYE documents");

            Console.WriteLine("Adding Payroll documents...");

            var payrollDocs = new List<MailerTestDataPayrollDocument>();
            var companyNum = 0;
            foreach (var company in testData.Companies)
            {
                companyNum++;
                Console.WriteLine($"Adding payroll documents for company {company.Name}...");
                // Create PAYROLL docs
                for (var datesIdx = 0; datesIdx < dates.Length; datesIdx++)
                {
                    var date = dates[datesIdx];

                    var expectedSubject = $"{company.Name} payroll {DateToMonthName(date)}";
                    var fileNames = new List<string>();

                    if (companyNum % 2 == 0)
                    {
                        // even numbered companies have single payslip for multiple employees
                        fileNames.Add($@"{PayrollDocsDir}\Payroll {company.Name} All {date}.pdf");
                    }
                    else
                    {
                        // odd numbered companies have individual payslip for each employee
                        fileNames.Add($@"{PayrollDocsDir}\Payroll {company.Name} Emp001 {date}.pdf");
                        fileNames.Add($@"{PayrollDocsDir}\Payroll {company.Name} Emp002 {date}.pdf");
                        fileNames.Add($@"{PayrollDocsDir}\Payroll {company.Name} Emp003 {date}.pdf");
                    }

                    foreach (var fn in fileNames)
                    {
                        File.Copy("DocTemplates\\Payroll.pdf", fn);
                        payrollDocs.Add(new MailerTestDataPayrollDocument
                        {
                            FileName = fn,
                            SendTo = company.Email,
                            ExpectedSubject = expectedSubject
                        });
                    }
                }
            }

            testData.PayrollDocuments = payrollDocs.ToArray();
            Console.WriteLine("Added Payroll documents");
        }
    }
}