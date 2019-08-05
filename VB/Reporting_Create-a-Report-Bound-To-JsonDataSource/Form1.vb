Imports DevExpress.DataAccess.Json
Imports DevExpress.XtraReports.UI
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Xml.Linq
Imports System.IO

Namespace Create_a_Report_Bound_to_JsonDataSource
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Dim report As XtraReport = CreateReport()
            Dim designTool As New ReportDesignTool(report)
            designTool.ShowRibbonDesignerDialog()
            Application.Exit()
        End Sub
        ' ...
        Private Function CreateReport() As XtraReport
            Dim report As New XtraReport() With {
                .Bands = {
                    New DetailBand() With {
                        .Controls = {
                            New XRLabel() With {
                                .ExpressionBindings = { New ExpressionBinding("BeforePrint", "Text", "[CompanyName]") },
                                .WidthF = 300
                            }
                        },
                        .HeightF = 50
                    }
                },
                .DataSource = CreateDataSourceFromWeb(),
                .DataMember = "Customers"
            }
            Return report
        End Function
        ' ...
        Public Shared Function CreateDataSourceFromWeb() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            ' Specify the data source location
            jsonDataSource.JsonSource = New UriJsonSource(New Uri("http://northwind.servicestack.net/customers.json"))
            Dim root = New JsonSchemaNode()
            root.NodeType = JsonNodeType.Object

            Dim customers = New JsonSchemaNode() With {
                .NodeType=JsonNodeType.Array,
                .Name="Customers",
                .Selected=True
            }
            customers.AddChildren( {
                New JsonSchemaNode(New JsonNode("CustomerID", True, JsonNodeType.Property, GetType(String))) With {.DisplayName = "Customer ID"},
                New JsonSchemaNode() With {
                    .Name = "CompanyName",
                    .Selected = True,
                    .NodeType = JsonNodeType.Property,
                    .Type = GetType(String)
                },
                New JsonSchemaNode(New JsonNode("ContactTitle", True, JsonNodeType.Property, GetType(String))),
                New JsonSchemaNode(New JsonNode("Address", False, JsonNodeType.Property, GetType(String)))
            })

            root.AddChildren(customers)
            jsonDataSource.Schema = root
            ' Retrieve data from the JSON data source to the Report Designer's Field List
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function
        Public Shared Function CreateDataSourceFromFile() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            ' Specify the JSON file name
            Dim fileUri As New Uri("customers.json", UriKind.RelativeOrAbsolute)
            jsonDataSource.JsonSource = New UriJsonSource(fileUri)
            ' Retrieve data from the JSON data source to the Report Designer's Field List
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function
        Public Shared Function CreateDataSourceFromText() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()

            ' Specify a string with JSON content
            Dim json As String = "{""Customers"":[{""Id"":""ALFKI"",""CompanyName"":""Alfreds Futterkiste"",""ContactName"":""Maria Anders"",""ContactTitle"":""Sales Representative"",""Address"":""Obere Str. 57"",""City"":""Berlin"",""PostalCode"":""12209"",""Country"":""Germany"",""Phone"":""030-0074321"",""Fax"":""030-0076545""}],""ResponseStatus"":{}}"

            ' Specify the object that retrieves JSON data
            jsonDataSource.JsonSource = New CustomJsonSource(json)
            ' Retrieve data from the JSON data source to the Report Designer's Field List
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function
    End Class
End Namespace
