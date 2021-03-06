﻿
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XmlConvert.Tests.Data;
using Xunit;

namespace XmlConvert.Tests {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    public class XmlConvertTest {
        [Fact]
        public void ConvertToXmlShouldReturnAnXDocumentEvenWithNotInitializedValues() {
            SampleDataClass data = new SampleDataClass();
            XDocument xDoc = data.ConvertToXml();
            var expected = "<SampleDataClass>\r\n  <Id>0</Id>\r\n  <Description></Description>\r\n</SampleDataClass>";

            Assert.NotNull(xDoc);
            Assert.Equal(expected, xDoc.ToString());
        }

        [Fact]
        public void ShouldConvertBasicProperties() {
            SampleDataClass data = new SampleDataClass() { Id = 1, Description = "foo" };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<SampleDataClass>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</SampleDataClass>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldNotConvertPropertyWithIgnoreXmlConvert() {
            SampleDataClassWithIgnoreMember data = new SampleDataClassWithIgnoreMember() { Id = 1, Description = "foo" };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<SampleDataClassWithIgnoreMember>\r\n  <Id>1</Id>\r\n</SampleDataClassWithIgnoreMember>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldWorkWithNullableProperties() {
            NullableSampleData data = new NullableSampleData() { Id = 1, Description = "foo", Money = 100.05m };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<NullableSampleData>\r\n  <Money>100.05</Money>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</NullableSampleData>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldWorkWithNullablePropertiesPassingNull() {
            NullableSampleData data = new NullableSampleData() { Id = 1, Description = "foo", Money = null };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<NullableSampleData>\r\n  <Money></Money>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</NullableSampleData>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldWorkWithDateTimeProperties() {
            var now = DateTime.Now;
            DateTimeSampleData data = new DateTimeSampleData() { Id = 1, Description = "foo", Created = now };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<DateTimeSampleData>\r\n  <Created>{0}</Created>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</DateTimeSampleData>", GetISO8601Date(now));
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        private string GetISO8601Date(DateTime date) {
            return date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz");
        }

        [Fact]
        public void ShouldWorkWithComplexTypeProperty() {
            ParentSampleData data = new ParentSampleData() {
                Id = 1,
                Description = "foo",
                SampleChildClass = new SampleChildClass() { 
                    Id = 1,
                    Description = "bar"
                }
            };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<ParentSampleData>\r\n  " +
                                         "<SampleChildClass>\r\n  " +
                                         "  <Id>1</Id>\r\n  " + 
                                         "  <Description>bar</Description>\r\n  " + 
                                         "</SampleChildClass>\r\n  " + 
                                         "<Id>1</Id>\r\n  " +
                                         "<Description>foo</Description>\r\n" +
                                         "</ParentSampleData>");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldNotConvertComplexyTypePropertyWithIgnoreXmlConvert() {
            ParentSampleDataWithChildIgnored data = new ParentSampleDataWithChildIgnored() { 
                Id = 1, 
                Description = "foo",
                Child = new SampleChildWithIgnoredMember() {
                    Id = 1,
                    Description = "bar"
                }
            };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<ParentSampleDataWithChildIgnored>\r\n  " +
                                         "<Child>\r\n  " +
                                         "  <Description>bar</Description>\r\n  " +
                                         "</Child>\r\n  " +
                                         "<Id>1</Id>\r\n  " +
                                         "<Description>foo</Description>\r\n" +
                                         "</ParentSampleDataWithChildIgnored>");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldWorkWithComplexTypePropertyWithNullReference() {
            ParentSampleData data = new ParentSampleData() {
                Id = 1,
                Description = "foo",
            };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<ParentSampleData>\r\n  " +
                                         "<SampleChildClass>" +
                                         "</SampleChildClass>\r\n  " +
                                         "<Id>1</Id>\r\n  " +
                                         "<Description>foo</Description>\r\n" +
                                         "</ParentSampleData>");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldHandleWithAutoRelation() {
            AutoRelationData data = new AutoRelationData() {
                Id = 1,
                Description = "foo",
                SampleDataClass = new SampleDataClass() {
                    Id = 1,
                    Description = "bar"
                }
            };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<AutoRelationData>\r\n  " +
                                         "<SampleDataClass>\r\n  " +
                                         "  <Id>1</Id>\r\n  " +
                                         "  <Description>bar</Description>\r\n  " +
                                         "</SampleDataClass>\r\n  " +
                                         "<Id>1</Id>\r\n  " +
                                         "<Description>foo</Description>\r\n" +
                                         "</AutoRelationData>");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldBeAbleToConvertLists() {
            IList<SampleDataClass> list = new List<SampleDataClass>() {
                new SampleDataClass(){
                    Id = 1,
                    Description = "foo",
                },
                new SampleDataClass(){
                    Id = 2,
                    Description = "foo2",
                }                
            };
            XDocument xDoc = list.ConvertToXml<SampleDataClass>();
            var expected = string.Format("<SampleDataClassCollection>\r\n  " +
                                         "<SampleDataClass>\r\n    " +
                                         "<Id>1</Id>\r\n    " +
                                         "<Description>foo</Description>\r\n  " +
                                         "</SampleDataClass>\r\n  " +
                                         "<SampleDataClass>\r\n    " +
                                         "<Id>2</Id>\r\n    " +
                                         "<Description>foo2</Description>\r\n  " +
                                         "</SampleDataClass>\r\n" +
                                         "</SampleDataClassCollection>");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldBeAbleToConvertEmptyList() {
            IList<SampleDataClass> list = new List<SampleDataClass>();
            XDocument xDoc = list.ConvertToXml<SampleDataClass>();
            var expected = string.Format("<SampleDataClassCollection />");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldHandleIgnoreNullValuesSetting() {
            ParentSampleData data = new ParentSampleData() {
                Id = 1,
                Description = "foo",
                SampleChildClass = null
            };

            XDocument xDoc = data.ConvertToXml(new XmlConvertSettings() { NullValueHandling = NullValueHandling.Ignore });
            var expected = string.Format("<ParentSampleData>\r\n  " +
                                         "<Id>1</Id>\r\n  " +
                                         "<Description>foo</Description>\r\n" +
                                         "</ParentSampleData>");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        //**************************CONVERT AS STRING **********************************//
        [Fact]
        public void ShouldBeAbleToConvertAsStringWhitoutIndentation() {
            SampleDataClass data = new SampleDataClass() { Id = 1, Description = "foo" };
            string result = data.Convert(new XmlConvertSettings() { Formatting = Formatting.None });
            var expected = "<SampleDataClass><Id>1</Id><Description>foo</Description></SampleDataClass>";
            result.ShouldEqualWithDiff(expected);
        }

        [Fact]
        public void ShouldBeAbleToConvertAsStringWhithIndentation() {
            SampleDataClass data = new SampleDataClass() { Id = 1, Description = "foo" };
            string result = data.Convert(new XmlConvertSettings() { Formatting = Formatting.Indented });
            var expected = "<SampleDataClass>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</SampleDataClass>";
            result.ShouldEqualWithDiff(expected);
        }











        /**** new tests, TODO: Organized it!! *************/
        //[Fact]
        //public void MoreComplexType() {
        //    var riskEvent = new RiskEvent();
        //    riskEvent.Id = Guid.NewGuid();
        //    riskEvent.Title = "Event0001";
        //    riskEvent.Status = EventStatus.Closed;
        //    riskEvent.CustomAttributes = new Dictionary<string, object>() {
        //        { "HasInvolveds", true },
        //        { "NumberOfInvolveds", 3 },
        //        { "InvolvedNames", new string[] { "Joao Paulo", "Aline", "Rafael" } }
        //    };
        //    riskEvent.CreatedAt = DateTime.Now;
        //    riskEvent.USR = 125;
        //    riskEvent.Coordinator = new Coordinator() {
        //        Id = Guid.NewGuid(),
        //        Name = "Administrator"
        //    };
        //    riskEvent.Vulnerabilities = new string[] { "Vulnerability1", "Vulnerability2" };
        //    riskEvent.Requirements = new List<string> { "Requirement1", "Requirement2" };
        //    riskEvent.HasContinuityPlan = false;


        //}

        //public class Event : IXmlConvertible {
        //    public Guid Id { get; set; }
        //    public string Title { get; set; }
        //    public EventStatus Status { get; set; }
        //    public IDictionary<string, object> CustomAttributes { get; set; }
        //    public DateTime CreatedAt { get; set; }
        //    public int USR { get; set; }
        //    public Coordinator Coordinator { get; set; }
        //}

        //public class RiskEvent : Event {
        //    public string[] Vulnerabilities { get; set; }
        //    public IList<string> Requirements { get; set; }
        //    public bool HasContinuityPlan { get; set; }
        //}

        //public enum EventStatus {
        //    Open = 1,
        //    Closed = 2,
        //    Cancelled = 3,
        //    Deleted = 4
        //}

        //public class Coordinator {
        //    public Guid Id { get; set; }
        //    public string Name { get; set; }
        //}
    } // class
}
