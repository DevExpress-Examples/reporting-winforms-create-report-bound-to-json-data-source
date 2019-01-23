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
        }

        private XtraReport CreateReport()
        {
            XtraReport report = new XtraReport();
            DetailBand DetailBand = new DetailBand();
            DetailBand.HeightF = 50;

            XRLabel XRLabel = new XRLabel();
            XRLabel.WidthF = 300;
            XRLabel.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CompanyName]"));

            DetailBand.Controls.Add(XRLabel);
            report.Bands.Add(DetailBand);



            report.DataSource = CreateDataSourceFromWeb();
            //report.DataSource = CreateDataSourceFromFile();
            //report.DataSource = CreateDataSourceFromText();
            //report.DataMember = "Customers";
            return report;
        }
        private JsonDataSource CreateDataSourceFromWeb()
        {
            var jsonDataSource = new JsonDataSource();
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
            //Retrieve data from the JSON data source
            jsonDataSource.Fill();
            return jsonDataSource;
        }

        private JsonDataSource CreateDataSourceFromFile()
        {
            var jsonDataSource = new JsonDataSource();
            //Specify the a JSON file's name
            Uri fileUri = new Uri(@"file:///../../../../customers.txt");
            jsonDataSource.JsonSource = new UriJsonSource(fileUri);
            return jsonDataSource;
        }

        private JsonDataSource CreateDataSourceFromText()
        {

            var jsonDataSource = new JsonDataSource();

            //Specify a string with JSON content
            string json = "{\"Customers\":[{\"Id\":\"ALFKI\",\"CompanyName\":\"Alfreds Futterkiste\",\"ContactName\":\"Maria Anders\",\"ContactTitle\":\"Sales Representative\",\"Address\":\"Obere Str. 57\",\"City\":\"Berlin\",\"PostalCode\":\"12209\",\"Country\":\"Germany\",\"Phone\":\"030-0074321\",\"Fax\":\"030-0076545\"}],\"ResponseStatus\":{}}";

            // Specify the object that retrieves JSON data
            jsonDataSource.JsonSource = new CustomJsonSource(json);
            return jsonDataSource;
        }
    }
}
