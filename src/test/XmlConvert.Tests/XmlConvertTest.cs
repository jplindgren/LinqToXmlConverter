
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

        [Fact(Skip = "Wait to implement a decente date treatment")]
        //[Fact()]
        public void ShouldWorkWithDateTimeProperties() {
            var now = DateTime.Now;
            DateTimeSampleData data = new DateTimeSampleData() { Id = 1, Description = "foo", Created = now };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<DateTimeSampleData>\r\n  <Created Value=\"{0}\" />\r\n  <Id Value=\"1\" />\r\n  <Description Value=\"foo\" />\r\n</DateTimeSampleData>", now.ToString());
            xDoc.ToString().ShouldEqualWithDiff(expected);
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
    } // class
}
