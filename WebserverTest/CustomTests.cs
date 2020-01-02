using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webserver;

namespace BIF.SWE1.UnitTests
{
    [TestFixture]
    public class CustomTests : AbstractTestFixture<Uebungen.CustomTests>
    {
        [Test]
        public void HelloWorld()
        {
            var obj = CreateInstance();
            obj.HelloWorld();
        }

        #region Database

        [Test]
        public void db_get_database_connection()
        {
            var obj = CreateInstance().GetDatabaseConnection();
            Assert.That(obj, Is.Not.Null, "DatabaseConnection returned null");
        }

        [Test]
        public void db_get_database_connectionstring()
        {
            var obj = CreateInstance().GetDatabaseConnection().ConnectionString;
            Assert.That(obj, Is.Not.Null, "ConnectionString returned null");
        }

        [Test]
        public void db_test_databaseconnection()
        {
            var obj = CreateInstance().GetDatabaseConnection().TestConnection();
            Assert.That(obj, Is.Not.Null, "Cannot connect to database");
        }

        [Test]
        public void db_insert_temperature_data()
        {
            var obj = CreateInstance().GetDatabaseConnection();
            obj.InsertTemperature(DateTime.Now, 42);
        }

        [Test]
        public void db_select_test_data()
        {
            var obj = CreateInstance().GetDatabaseConnection();
            var result = obj.SelectTemperatureRange(DateTime.MinValue, DateTime.Now);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(10000));
        }

        [Test]
        public void db_select_temperature_range()
        {
            var obj = CreateInstance().GetDatabaseConnection();
            var result = obj.SelectTemperatureRange(new DateTime(14, 1, 1), new DateTime(14, 1, 2));
            Assert.That(result.Count, Is.Not.Zero);
        }

        [Test]
        public void db_select_temperature_range_no_data_found()
        {
            var obj = CreateInstance().GetDatabaseConnection();
            var result = obj.SelectTemperatureRange(DateTime.MinValue, DateTime.MinValue.AddHours(1));
            Assert.That(result.Count, Is.Zero);
        }

        [Test]
        public void db_read_sensor_data()
        {
            var obj = CreateInstance().GetDatabaseConnection();
            obj.ReadSensorData();
        }

        #endregion

        #region TemperaturePlugin

        [Test]
        public void temp_plugin_get_temperature_plugin()
        {
            var obj = CreateInstance().GetTempPlugin();
            Assert.That(obj, Is.Not.Null, "TempPlugin returned null");
        }

        [Test]
        public void temp_plugin_should_parse_string()
        {
            var obj = CreateInstance().GetTempPlugin();
            var result = obj.ParseToDateTime("2019-12-18");
            Assert.That(result, Is.EqualTo(new DateTime(2019, 12, 18)));
        }

        [Test]
        public void temp_plugin_should_not_parse_empty_string()
        {
            var obj = CreateInstance().GetTempPlugin();
            Assert.That(() => obj.ParseToDateTime(string.Empty), Throws.InstanceOf<FormatException>());
        }

        [Test]
        public void temp_plugin_should_not_parse_invalid_string()
        {
            var obj = CreateInstance().GetTempPlugin();
            Assert.That(() => obj.ParseToDateTime("invalidString"), Throws.InstanceOf<FormatException>());
        }

        [Test]
        public void temp_plugin_create_temp_html()
        {
            var url = new Url("/static-files/temp.html?from=2020-01-01&until=2020-01-09&page=2&GetTemperature=");
            var data = new Dictionary<string, double> {{"a", 1}, {"b", 2}, {"c", 3}};
            var obj = CreateInstance().GetTempPlugin().CreateNaviHtml(data, 2, url);
            Assert.That(obj, Is.Not.Null, "Cannot create html");
        }

        [Test]
        public void temp_plugin_create_temp_xml()
        {
            var data = new Dictionary<string, double> {{"a", 1}, {"b", 2}, {"c", 3}};
            var obj = CreateInstance().GetTempPlugin().CreateRestNaviXml(data);
            Assert.That(obj, Is.Not.Null, "Cannot create xml");
        }

        #endregion

        #region StaticFilePlugin

        [Test]
        public void staticfileplugin_get_plugin()
        {
            var obj = CreateInstance().GetStaticFilePlugin();
            Assert.That(obj, Is.Not.Null, "Static File Plugin is null");
        }

        [Test]
        public void staticfileplugin_set_mimetype_html()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("html");
            Assert.That(obj, Is.EqualTo("text/html"));
        }

        [Test]
        public void staticfileplugin_set_mimetype_css()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("css");
            Assert.That(obj, Is.EqualTo("text/css"));
        }

        [Test]
        public void staticfileplugin_set_mimetype_js()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("js");
            Assert.That(obj, Is.EqualTo("text/javascript"));
        }

        [Test]
        public void staticfileplugin_set_mimetype_ico()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("ico");
            Assert.That(obj, Is.EqualTo("image/x-icon"));
        }

        [Test]
        public void staticfileplugin_set_mimetype_txt()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("txt");
            Assert.That(obj, Is.EqualTo("text/plain"));
        }

        [Test]
        public void staticfileplugin_set_mimetype_json()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("json");
            Assert.That(obj, Is.EqualTo("application/json"));
        }
        
        [Test]
        public void staticfileplugin_set_mimetype_png()
        {
            var obj = CreateInstance().GetStaticFilePlugin().GetMimeType("png");
            Assert.That(obj, Is.EqualTo("image/png"));
        }
        #endregion
    }
}