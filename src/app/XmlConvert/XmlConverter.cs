using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;
using System.Linq.Expressions;
using XmlConvert.Specifications;

[assembly: CLSCompliant(true)]
namespace XmlConvert {
    public class XmlConverter : IXmlConverter {
        private XmlConvertSettings settings;

        public XmlConverter(XmlConvertSettings settings) {
            this.settings = settings ?? XmlConvertSettings.CreateDefault();
        }

        public XDocument Convert(IXmlConvertible entity) {
            XDocument xdoc = new XDocument(CreateNodeXElement(entity.GetType().Name, entity));
            return xdoc;
        }

        public string ConvertAsString(IXmlConvertible entity) {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.None : SaveOptions.DisableFormatting;
            XDocument xdoc = Convert(entity);            
            return xdoc.ToString(options);
        }

        public XDocument ConvertAndSave(IXmlConvertible entity, string path) {
            XDocument xdoc = Convert(entity);
            xdoc.Save(path);
            return xdoc;
        }

        public string ConvertAsStringAndSave(IXmlConvertible entity, string path) {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.DisableFormatting : SaveOptions.None;
            XDocument xdoc = ConvertAndSave(entity, path);
            return xdoc.ToString(options);
        }


        public XDocument Convert<T>(IList<T> list) where T : IXmlConvertible {
            var collectionName = list.GetType().GetGenericArguments().FirstOrDefault(type => IsXmlConvertible(type)).Name;
            XDocument xdoc = new XDocument(new XElement(string.Format(CultureInfo.CurrentCulture , "{0}Collection", collectionName),
                from _item in list
                select CreateNodeXElement(_item.GetType().Name, _item)
            ));
            return xdoc;
        }

        public string ConvertAsString<T>(IList<T> list) where T : IXmlConvertible {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.DisableFormatting : SaveOptions.None;
            XDocument xdoc = Convert(list);
            return xdoc.ToString(options);
        }

        public XDocument ConvertAndSave<T>(IList<T> list, string path) where T : IXmlConvertible {
            XDocument xdoc = Convert<T>(list);
            xdoc.Save(path);
            return xdoc;
        }

        public string ConvertAsStringAndSave<T>(IList<T> list, string path) where T : IXmlConvertible {
            SaveOptions options = settings.Formatting == Formatting.Indented ? SaveOptions.DisableFormatting : SaveOptions.None;
            XDocument xdoc = ConvertAndSave<T>(list, path);
            return xdoc.ToString(options);
        }


        #region private methods
        private XElement CreateNodeXElement(string nodeName, IXmlConvertible entity) {
            var properties = GetElementProperties(entity.GetType());

            var elementList = properties.Select(property => CreateWrapper(entity, property));

            //TODO: Find a more elegant way to apply specifictions based on settings
            if (settings.NullValueHandling == NullValueHandling.Ignore)
                elementList = elementList.Where(new NotNullSpecification().IsSatisfiedBy());

            IQueryable<object> innerElements = elementList.Select(property => CreateElement(property, entity));            

            var xElement = new XElement(nodeName, innerElements);
                     
            return xElement;
        }

        private XmlElementWrapper CreateWrapper(IXmlConvertible entity, PropertyInfo propertyInfo) {
            return new XmlElementWrapper() {
                PropertyInfo = propertyInfo,
                Value = propertyInfo.GetValue(entity, null),
                Name = propertyInfo.Name
            };
        }

        public bool IsXmlConvertible(Type type) {
            return type.GetInterfaces().Contains(typeof(IXmlConvertible));
        }

        private IQueryable<PropertyInfo> GetElementProperties(Type type) {
            var specificationsList = new ISpecification<PropertyInfo>[] { 
                new PropertyNotIgnoredSpecification()
            };
            
            var properties = type.GetProperties().AsQueryable();
            return ApplySpecification(properties, new AndCompositeSpecification<PropertyInfo>(specificationsList));
        }

        private object CreateElement(XmlElementWrapper element, IXmlConvertible entity) {
            if (element.IsXmlConvertible() && element.IsNotNullValue()) {
                // recursive create others elements inside
                return CreateNodeXElement(element.Name, (IXmlConvertible)element.Value);
            } else {
                //Create element with value
                return CreateLeafXElement(element);
            }
        }

        private static XElement CreateLeafXElement(XmlElementWrapper element) {
            return new XElement(element.Name, element.Value ?? string.Empty);
        }

        private IQueryable<PropertyInfo> ApplySpecification(IQueryable<PropertyInfo> properties, ISpecification<PropertyInfo> specification) {
            return properties.Where(specification.IsSatisfiedBy());
        }

        //public XAttribute CreateXAttributeFromProperty(PropertyInfo property, IXmlConvertible entity){
        //    var value =  property.GetValue(entity, null);
        //    return new XAttribute("Value", value ?? string.Empty);
        //}

        #endregion

    }// class
}
