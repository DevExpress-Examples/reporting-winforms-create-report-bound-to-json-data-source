﻿#region usings
using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.UI;
#endregion
using System;
using System.Net;
using System.Windows.Forms;


namespace Create_a_Report_Bound_to_JsonDataSource
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Enables security protocol versions to access the JSON web data source.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Ssl3;

            XtraReport report = CreateReport();
            ReportDesignTool designTool = new ReportDesignTool(report);
            designTool.ShowRibbonDesignerDialog();
            Application.Exit();
        }
        #region CreateReport_start
        private XtraReport CreateReport() {
            XtraReport report = new XtraReport() {
                Bands = {
                    new DetailBand() {
                        Controls = {
                            new XRLabel() {
                                ExpressionBindings = {
                                    new ExpressionBinding("BeforePrint", "Text", "[CompanyName]")
                                },
                                WidthF = 300
                            }
                        },
                        HeightF = 50
                    }
                },
                DataSource = CreateDataSourceFromWeb(),
                #endregion
                //DataSource = CreateDataSourceFromFile(),
                //DataSource = CreateDataSourceFromText(),
                #region CreateReport_end
                DataMember = "Customers"
            };
            return report;
        }
        #endregion
        #region CreateDataSourceFromWeb_start
        public static JsonDataSource CreateDataSourceFromWeb() {
            var jsonDataSource = new JsonDataSource();
            // Specify the endpoint.
            jsonDataSource.JsonSource = new UriJsonSource(
                new Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"));
            #endregion
            #region CreateDataSourceFromWeb_schema
            var root = new JsonSchemaNode();
            root.NodeType = JsonNodeType.Object;

            var customers = new JsonSchemaNode() {NodeType=JsonNodeType.Array, Name="Customers", Selected=true };
            customers.AddChildren(new[] {
                new JsonSchemaNode(new JsonNode("CustomerID", true,
                JsonNodeType.Property, typeof(string))) 
                { 
                DisplayName = "Customer ID" },
                new JsonSchemaNode() {
                    Name =  "CompanyName",
                    Selected = true,
                    NodeType = JsonNodeType.Property,
                    Type = typeof(string)
                },
                new JsonSchemaNode(new JsonNode("ContactTitle", true, JsonNodeType.Property, typeof(string))),
                new JsonSchemaNode(new JsonNode("Address", false, JsonNodeType.Property, typeof(string)))
            });

            root.AddChildren(customers);
            jsonDataSource.Schema = root;
            #endregion
            #region CreateDataSourceFromWeb_end
            // The schema is built, you do not have to call the Fill method to populate the Field List.
			// The Designer calls the Fill method automatically when a document is generated for preview.
            //jsonDataSource.Fill();
            return jsonDataSource;
        }
        #endregion
        #region CreateDataSourceFromFile
        public static JsonDataSource CreateDataSourceFromFile() {
            var jsonDataSource = new JsonDataSource();
            // Specify the JSON file name.
            Uri fileUri = new Uri("customers.json", UriKind.RelativeOrAbsolute);
            jsonDataSource.JsonSource = new UriJsonSource(fileUri);
            // Populate the data source with data.
            jsonDataSource.Fill();
            return jsonDataSource;
        }
        #endregion
        #region CreateDataSourceFromText
        public static JsonDataSource CreateDataSourceFromText() {
            var jsonDataSource = new JsonDataSource();

            // Specify a string with JSON data.
            string json = "{\"Customers\":[{\"Id\":\"ALFKI\",\"CompanyName\":\"Alfreds Futterkiste\"," +
                "\"ContactName\":\"Maria Anders\",\"ContactTitle\":\"Sales Representative\"," +
                "\"Address\":\"Obere Str. 57\",\"City\":\"Berlin\",\"PostalCode\":\"12209\"," +
                "\"Country\":\"Germany\",\"Phone\":\"030-0074321\",\"Fax\":\"030-0076545\"}]," +
                "\"ResponseStatus\":{}}";

            // Specify the object that retrieves JSON data.
            jsonDataSource.JsonSource = new CustomJsonSource(json);
            // Populate the data source with data.
            jsonDataSource.Fill();
            return jsonDataSource;
        }
        #endregion
    }
}
