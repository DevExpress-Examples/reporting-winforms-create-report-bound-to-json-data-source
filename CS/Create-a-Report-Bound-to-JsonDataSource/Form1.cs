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
            report.DataSource = CreateDataSource();
            return report;
        }
        private JsonDataSource CreateDataSource()
        {

            var jsonDataSource = new JsonDataSource();

            // Specify a string with JSON content
            //string json = "";

            // Specify the a JSON file's name
            //string filename = "customers.txt";

            // Specify a Web Service Endpoint URI with JSON content
            var uri = new Uri("http://northwind.servicestack.net/customers.json");


            // Specify the object that retrieves JSON data
            //jsonDataSource.JsonSource = new CustomJsonSource(json);
            //jsonDataSource.JsonSource = new UriJsonSource(fileName);
            jsonDataSource.JsonSource = new UriJsonSource(uri) { RootElement = "Customers" };

            // Define the data schema
            jsonDataSource.Schema = new JsonSchemaNode()
            {
                Name = "root",
                NodeType = JsonNodeType.Object,
                Selected = true,
                Nodes = {
                    new JsonSchemaNode() {
                        Name = "Customers",
                        Selected = true,
                        NodeType = JsonNodeType.Array,
                        Nodes = {
                            new JsonSchemaNode() { Name = "Id", NodeType = JsonNodeType.Property, Type = typeof(string), DisplayName = "Customer ID", Selected = true },
                            new JsonSchemaNode() { Name =  "CompanyName", NodeType = JsonNodeType.Property, Type = typeof(string), Selected = true },
                            new JsonSchemaNode() { Name = "ContactTitle", NodeType = JsonNodeType.Property, Type = typeof(string), Selected = true },
                            new JsonSchemaNode() { Name = "Address",      NodeType = JsonNodeType.Property, Type = typeof(string), Selected = false }
                        }
                    }
                }
            };

            // Retrieve data from the JSON data source
            jsonDataSource.Fill();

            return jsonDataSource;
        }
    }
}
