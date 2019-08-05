using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

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
            XtraReport report = CreateReport();
            ReportDesignTool designTool = new ReportDesignTool(report);
            designTool.ShowRibbonDesignerDialog();
            Application.Exit();
        }
        // ...
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
                //DataSource = CreateDataSourceFromFile(),
                //DataSource = CreateDataSourceFromText(),
                DataMember = "Customers"
            };
            return report;
        }
        // ...
        public static JsonDataSource CreateDataSourceFromWeb() {
            var jsonDataSource = new JsonDataSource();
            // Specify the data source location
            jsonDataSource.JsonSource = new UriJsonSource(new Uri("http://northwind.servicestack.net/customers.json"));
            var root = new JsonSchemaNode();
            root.NodeType = JsonNodeType.Object;

            var customers = new JsonSchemaNode() {NodeType=JsonNodeType.Array, Name="Customers", Selected=true };
            customers.AddChildren(new[] {
                new JsonSchemaNode(new JsonNode("CustomerID", true, JsonNodeType.Property, typeof(string))) { DisplayName = "Customer ID" },
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
            // Retrieve data from the JSON data source to the Report Designer's Field List
            jsonDataSource.Fill();
            return jsonDataSource;
        }
        public static JsonDataSource CreateDataSourceFromFile() {
            var jsonDataSource = new JsonDataSource();
            // Specify the JSON file name
            Uri fileUri = new Uri("customers.json", UriKind.RelativeOrAbsolute);
            jsonDataSource.JsonSource = new UriJsonSource(fileUri);
            // Retrieve data from the JSON data source to the Report Designer's Field List
            jsonDataSource.Fill();
            return jsonDataSource;
        }
        public static JsonDataSource CreateDataSourceFromText() {
            var jsonDataSource = new JsonDataSource();

            // Specify a string with JSON content
            string json = "{\"Customers\":[{\"Id\":\"ALFKI\",\"CompanyName\":\"Alfreds Futterkiste\",\"ContactName\":\"Maria Anders\",\"ContactTitle\":\"Sales Representative\",\"Address\":\"Obere Str. 57\",\"City\":\"Berlin\",\"PostalCode\":\"12209\",\"Country\":\"Germany\",\"Phone\":\"030-0074321\",\"Fax\":\"030-0076545\"}],\"ResponseStatus\":{}}";

            // Specify the object that retrieves JSON data
            jsonDataSource.JsonSource = new CustomJsonSource(json);
            // Retrieve data from the JSON data source to the Report Designer's Field List
            jsonDataSource.Fill();
            return jsonDataSource;
        }
    }
}
