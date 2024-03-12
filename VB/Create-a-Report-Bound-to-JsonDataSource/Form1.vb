Imports DevExpress.DataAccess.Json
Imports DevExpress.XtraReports.UI
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace Create_a_Report_Bound_to_JsonDataSource

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            Dim report As XtraReport = CreateReport()
            Dim designTool As ReportDesignTool = New ReportDesignTool(report)
            designTool.ShowRibbonDesignerDialog()
        End Sub

        Private Function CreateReport() As XtraReport
            Dim report As XtraReport = New XtraReport()
            Dim DetailBand As DetailBand = New DetailBand()
            DetailBand.HeightF = 50
            Dim XRLabel As XRLabel = New XRLabel()
            XRLabel.WidthF = 300
            XRLabel.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[CompanyName]"))
            DetailBand.Controls.Add(XRLabel)
            report.Bands.Add(DetailBand)
            report.DataSource = CreateDataSourceFromWeb()
            'report.DataSource = CreateDataSourceFromFile();
            'report.DataSource = CreateDataSourceFromText();
            report.DataMember = "Customers"
            Return report
        End Function

        Private Function CreateDataSourceFromWeb() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            jsonDataSource.JsonSource = New UriJsonSource(New Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"))
            Dim root = New JsonSchemaNode()
            root.NodeType = JsonNodeType.Object
            Dim customers = New JsonSchemaNode() With {.NodeType = JsonNodeType.Array, .Name = "Customers", .Selected = True}
            customers.AddChildren({New JsonSchemaNode(New JsonNode("CustomerID", True, JsonNodeType.Property, GetType(String))) With {.DisplayName = "Customer ID"}, New JsonSchemaNode() With {.Name = "CompanyName", .Selected = True, .NodeType = JsonNodeType.Property, .Type = GetType(String)}, New JsonSchemaNode(New JsonNode("ContactTitle", True, JsonNodeType.Property, GetType(String))), New JsonSchemaNode(New JsonNode("Address", False, JsonNodeType.Property, GetType(String)))})
            root.AddChildren(customers)
            jsonDataSource.Schema = root
            'Retrieve data from the JSON data source
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function

        Private Function CreateDataSourceFromFile() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            'Specify the a JSON file's name
            Dim fileUri As Uri = New Uri("file:///../../../../customers.txt")
            jsonDataSource.JsonSource = New UriJsonSource(fileUri)
            Return jsonDataSource
        End Function

        Private Function CreateDataSourceFromText() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            'Specify a string with JSON content
            Dim json As String = "{""Customers"":[{""Id"":""ALFKI"",""CompanyName"":""Alfreds Futterkiste"",""ContactName"":""Maria Anders"",""ContactTitle"":""Sales Representative"",""Address"":""Obere Str. 57"",""City"":""Berlin"",""PostalCode"":""12209"",""Country"":""Germany"",""Phone"":""030-0074321"",""Fax"":""030-0076545""}],""ResponseStatus"":{}}"
            ' Specify the object that retrieves JSON data
            jsonDataSource.JsonSource = New CustomJsonSource(json)
            Return jsonDataSource
        End Function
    End Class
End Namespace
