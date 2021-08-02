#Region "usings"
Imports System.Net
Imports DevExpress.DataAccess.Json
Imports DevExpress.XtraReports.UI
#End Region


Namespace Create_a_Report_Bound_to_JsonDataSource
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            'Enables security protocol versions to access the JSON web data source.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or
                SecurityProtocolType.Tls11 Or
                SecurityProtocolType.Tls12 Or
                SecurityProtocolType.Ssl3

            Dim report As XtraReport = CreateReport()
            Dim designTool As New ReportDesignTool(report)
            designTool.ShowRibbonDesignerDialog()
            Application.Exit()
        End Sub
#Region "CreateReport_start"
        Private Function CreateReport() As XtraReport
            Dim report As New XtraReport()
            Dim DetailBand As New DetailBand()
            DetailBand.HeightF = 50

            Dim XRLabel As New XRLabel()
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
#End Region
#Region "CreateDataSourceFromWeb"
        Public Shared Function CreateDataSourceFromWeb() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            ' Specify the endpoint.
            jsonDataSource.JsonSource = New UriJsonSource(
                New Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"))
            Dim root = New JsonSchemaNode()
            root.NodeType = JsonNodeType.Object

            Dim customers = New JsonSchemaNode() With {.NodeType = JsonNodeType.Array, .Name = "Customers", .Selected = True}
            customers.AddChildren({
                New JsonSchemaNode(New JsonNode("CustomerID", True, JsonNodeType.Property,
                                                GetType(String))) With {.DisplayName = "Customer ID"},
                New JsonSchemaNode() With {.Name = "CompanyName", .Selected = True,
                .NodeType = JsonNodeType.Property, .Type = GetType(String)},
                New JsonSchemaNode(New JsonNode("ContactTitle", True, JsonNodeType.Property,
                                                GetType(String))),
                New JsonSchemaNode(New JsonNode("Address", False, JsonNodeType.Property,
                                                GetType(String)))
            })

            root.AddChildren(customers)
            jsonDataSource.Schema = root
            ' Populate the data source with data.
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function
#End Region
#Region "CreateDataSourceFromFile"
        Public Shared Function CreateDataSourceFromFile() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()
            ' Specify the JSON file name.
            Dim fileUri As New Uri("customers.json", UriKind.RelativeOrAbsolute)
            jsonDataSource.JsonSource = New UriJsonSource(fileUri)
            ' Populate the data source with data.
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function
#End Region
#Region "CreateDataSourceFromText"
        Public Shared Function CreateDataSourceFromText() As JsonDataSource
            Dim jsonDataSource = New JsonDataSource()

            ' Specify a string with JSON data.
            Dim json As String = "{""Customers"":[{""Id"":""ALFKI"",""CompanyName"":""Alfreds Futterkiste""," +
                """ContactName"":""Maria Anders"",""ContactTitle"":""Sales Representative""," +
            """Address"":""Obere Str. 57"",""City"":""Berlin"",""PostalCode"":""12209""," +
            """Country"":""Germany"",""Phone"":""030-0074321""," +
            """Fax"":""030-0076545""}],""ResponseStatus"":{}}"

            ' Specify the object that retrieves JSON data.
            jsonDataSource.JsonSource = New CustomJsonSource(json)
            ' Populate the data source with data.
            jsonDataSource.Fill()
            Return jsonDataSource
        End Function
#End Region
    End Class
End Namespace
