using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using XmlConvert.Tests.Data;

namespace XmlConvert.Tests {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class XmlConvertTest {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Convert_to_Xml_Should_Return_An_XDocument_Even_With_Not_Initialized_Values() {
            SampleDataClass data = new SampleDataClass();
            XDocument xDoc = data.ConvertToXml();
            var expected = "<SampleDataClass>\r\n  <Id>0</Id>\r\n  <Description></Description>\r\n</SampleDataClass>";

            Assert.IsNotNull(xDoc);
            Assert.AreEqual(expected, xDoc.ToString());
        }

        [TestMethod]
        public void Should_Convert_Basic_Properties() {
            SampleDataClass data = new SampleDataClass() { Id = 1, Description = "foo" };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<SampleDataClass>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</SampleDataClass>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [TestMethod]
        public void Should_Not_Convert_Property_With_IgnoreXmlConvert() {
            SampleDataClassWithIgnoreMember data = new SampleDataClassWithIgnoreMember() { Id = 1, Description = "foo" };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<SampleDataClassWithIgnoreMember>\r\n  <Id>1</Id>\r\n</SampleDataClassWithIgnoreMember>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [TestMethod]
        public void Should_Work_With_Nullable_Properties() {
            NullableSampleData data = new NullableSampleData() { Id = 1, Description = "foo", Money = 100.05m };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<NullableSampleData>\r\n  <Money>100.05</Money>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</NullableSampleData>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [TestMethod]
        public void Should_Work_With_Nullable_Properties_Passing_Null() {
            NullableSampleData data = new NullableSampleData() { Id = 1, Description = "foo", Money = null };
            XDocument xDoc = data.ConvertToXml();
            var expected = "<NullableSampleData>\r\n  <Money></Money>\r\n  <Id>1</Id>\r\n  <Description>foo</Description>\r\n</NullableSampleData>";
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [TestMethod]
        [Ignore]
        public void Should_Work_With_DateTime_Properties() {
            var now = DateTime.Now;
            DateTimeSampleData data = new DateTimeSampleData() { Id = 1, Description = "foo", Created = now };
            XDocument xDoc = data.ConvertToXml();
            var expected = string.Format("<DateTimeSampleData>\r\n  <Created Value=\"{0}\" />\r\n  <Id Value=\"1\" />\r\n  <Description Value=\"foo\" />\r\n</DateTimeSampleData>", now.ToString());
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [TestMethod]
        public void Should_Work_With_Complex_Type_Property() {
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

        [TestMethod]
        public void Should_Not_Convert_Complexy_Type_Property_With_IgnoreXmlConvert() {
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

        [TestMethod]
        public void Should_Work_With_Complex_Type_Property_With_Null_Reference() {
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

        [TestMethod]
        public void Should_Handle_With_Auto_Relation() {
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

        [TestMethod]
        public void Should_Be_Able_To_Convert_Lists() {
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
            var expected = string.Format("<Collection>\r\n  " +
                                         "<SampleDataClass>\r\n    " +
                                         "<Id>1</Id>\r\n    " +
                                         "<Description>foo</Description>\r\n  " +
                                         "</SampleDataClass>\r\n  " +
                                         "<SampleDataClass>\r\n    " +
                                         "<Id>2</Id>\r\n    " +
                                         "<Description>foo2</Description>\r\n  " +
                                         "</SampleDataClass>\r\n" +
                                         "</Collection>" );
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }

        [TestMethod]
        public void Should_Be_Able_To_Convert_Empty_List() {
            IList<SampleDataClass> list = new List<SampleDataClass>();
            XDocument xDoc = list.ConvertToXml<SampleDataClass>();
            var expected = string.Format("<Collection />");
            xDoc.ToString().ShouldEqualWithDiff(expected);
        }
    } // class
}
