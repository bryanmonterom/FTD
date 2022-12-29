using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for rptRequestHistoryv2
/// </summary>
public class rptRequestHistoryv2 : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRLabel xrLabel27;
    private XRLabel xrLabel28;
    private XRLabel xrLabel29;
    private XRLabel xrLabel33;
    private XRLabel xrLabel42;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private DevExpress.DataAccess.EntityFramework.EFDataSource efDataSource1;
    private PageHeaderBand pageHeaderBand1;
    private XRLabel xrLabel2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel6;
    private XRLabel xrLabel8;
    private XRLabel xrLabel10;
    private XRLabel xrLabel19;
    private XRLabel xrLabel20;
    private XRLine xrLine1;
    private XRLine xrLine2;
    private GroupHeaderBand groupHeaderBand1;
    private GroupFooterBand groupFooterBand1;
    private XRLabel xrLabel24;
    private XRLabel xrLabel26;
    private ReportHeaderBand reportHeaderBand1;
    private XRLabel xrLabel47;
    private XRLine xrLine3;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private XRLabel xrLabel11;
    private XRLabel xrLabel9;
    private XRLabel xrLabel4;
    private XRLabel xrLabel32;
    private XRLabel xrLabel5;
    private XRLabel xrLabel1;
    private XRLabel xrLabel21;
    private XRLabel xrLabel7;
    private XRPictureBox xrPictureBox1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public rptRequestHistoryv2()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.EntityFramework.EFConnectionParameters efConnectionParameters1 = new DevExpress.DataAccess.EntityFramework.EFConnectionParameters();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rptRequestHistoryv2));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.efDataSource1 = new DevExpress.DataAccess.EntityFramework.EFDataSource(this.components);
            this.pageHeaderBand1 = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.groupHeaderBand1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.groupFooterBand1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.efDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrLabel9,
            this.xrLabel4,
            this.xrLabel27,
            this.xrLabel28,
            this.xrLabel29,
            this.xrLabel32,
            this.xrLabel33,
            this.xrLabel42});
            this.Detail.HeightF = 23F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StyleName = "DataField";
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.status")});
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(597.7421F, 0F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(113.0577F, 23F);
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.directorTo")});
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(350.1668F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(97.75586F, 23F);
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.departmentTo")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(250.5343F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(99.63254F, 23F);
            // 
            // xrLabel27
            // 
            this.xrLabel27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.Date", "{0:d/M/yyyy}")});
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(6.000001F, 0F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(62.2687F, 23F);
            this.xrLabel27.Text = "xrLabel27";
            // 
            // xrLabel28
            // 
            this.xrLabel28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.departmentFrom")});
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(68.2687F, 0F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(62.06852F, 23F);
            this.xrLabel28.Text = "xrLabel28";
            // 
            // xrLabel29
            // 
            this.xrLabel29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.directorFrom")});
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(130.3372F, 0F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(120.197F, 23F);
            // 
            // xrLabel32
            // 
            this.xrLabel32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.step")});
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(447.9227F, 0F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(77.71338F, 23F);
            // 
            // xrLabel33
            // 
            this.xrLabel33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.requestor")});
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(525.636F, 0F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(72.10602F, 23F);
            // 
            // xrLabel42
            // 
            this.xrLabel42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.Identifier")});
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(710.7999F, 0F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(88.62335F, 23F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 100F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo1.StyleName = "PageInfo";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "Página {0} de {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(577F, 9.999996F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // efDataSource1
            // 
            efConnectionParameters1.ConnectionString = "";
            efConnectionParameters1.ConnectionStringName = "DefaultConnection";
            efConnectionParameters1.Source = typeof(AdminLteMvc.FTDEntities);
            this.efDataSource1.ConnectionParameters = efConnectionParameters1;
            this.efDataSource1.Name = "efDataSource1";
            // 
            // pageHeaderBand1
            // 
            this.pageHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel5,
            this.xrLabel1,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel6,
            this.xrLabel19,
            this.xrLabel20,
            this.xrLabel21,
            this.xrLine1,
            this.xrLine2});
            this.pageHeaderBand1.HeightF = 46.00001F;
            this.pageHeaderBand1.Name = "pageHeaderBand1";
            // 
            // xrLabel10
            // 
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(710.7997F, 6.999964F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(88.62341F, 36F);
            this.xrLabel10.StyleName = "FieldCaption";
            this.xrLabel10.Text = "Identifier";
            // 
            // xrLabel5
            // 
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(327.7256F, 6.999983F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(120.197F, 36F);
            this.xrLabel5.StyleName = "FieldCaption";
            this.xrLabel5.Text = "Director";
            // 
            // xrLabel1
            // 
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(250.5342F, 6.999977F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(77.19141F, 36F);
            this.xrLabel1.StyleName = "FieldCaption";
            this.xrLabel1.Text = "Hacia";
            // 
            // xrLabel2
            // 
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(6.000001F, 7.000002F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(47.14581F, 36F);
            this.xrLabel2.StyleName = "FieldCaption";
            this.xrLabel2.Text = "Date";
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(53.14581F, 7.000002F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(77.19141F, 36F);
            this.xrLabel3.StyleName = "FieldCaption";
            this.xrLabel3.Text = "Desde";
            // 
            // xrLabel6
            // 
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(130.3372F, 7.00002F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(120.197F, 36F);
            this.xrLabel6.StyleName = "FieldCaption";
            this.xrLabel6.Text = "Director";
            // 
            // xrLabel19
            // 
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(525.5908F, 7.00002F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(72.15125F, 36F);
            this.xrLabel19.StyleName = "FieldCaption";
            this.xrLabel19.Text = "Solicitante";
            // 
            // xrLabel20
            // 
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(597.7421F, 6.999983F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(113.0577F, 36F);
            this.xrLabel20.StyleName = "FieldCaption";
            this.xrLabel20.Text = "Estado";
            // 
            // xrLabel21
            // 
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(447.9227F, 7.00002F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(77.66815F, 36F);
            this.xrLabel21.StyleName = "FieldCaption";
            this.xrLabel21.Text = "Paso Actual";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(6.000005F, 4.999983F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(884.0001F, 2.000001F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(6.000005F, 42.99997F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(884.0001F, 2.000042F);
            // 
            // xrLabel8
            // 
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(9.999996F, 0F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(65.37921F, 36F);
            this.xrLabel8.StyleName = "FieldCaption";
            this.xrLabel8.Text = "Flujo";
            // 
            // groupHeaderBand1
            // 
            this.groupHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel7,
            this.xrLabel8});
            this.groupHeaderBand1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("IdFlowType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.groupHeaderBand1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.groupHeaderBand1.HeightF = 36.00002F;
            this.groupHeaderBand1.Name = "groupHeaderBand1";
            this.groupHeaderBand1.StyleName = "DataField";
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.flowtype")});
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(75.37921F, 0F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(89.18848F, 36.00002F);
            // 
            // groupFooterBand1
            // 
            this.groupFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel24,
            this.xrLabel26});
            this.groupFooterBand1.HeightF = 30F;
            this.groupFooterBand1.Name = "groupFooterBand1";
            // 
            // xrLabel24
            // 
            this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwRequest.Id")});
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(290.8293F, 6.00001F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(96.55988F, 18F);
            this.xrLabel24.StyleName = "FieldCaption";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel24.Summary = xrSummary1;
            this.xrLabel24.Text = "xrLabel24";
            // 
            // xrLabel26
            // 
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(6.000009F, 6.000027F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(94.69421F, 18F);
            this.xrLabel26.StyleName = "FieldCaption";
            this.xrLabel26.Text = "Cantidad";
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel47,
            this.xrLine3});
            this.reportHeaderBand1.HeightF = 85.1736F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrLabel47
            // 
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(290.8293F, 20.88709F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(408.2797F, 39.00001F);
            this.xrLabel47.StyleName = "Title";
            this.xrLabel47.Text = "Historico de Solicitudes";
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(6.000005F, 4.000003F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(884.0001F, 2.000002F);
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Times New Roman", 24F);
            this.Title.ForeColor = System.Drawing.Color.Black;
            this.Title.Name = "Title";
            // 
            // FieldCaption
            // 
            this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
            this.FieldCaption.BorderColor = System.Drawing.Color.Black;
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.FieldCaption.BorderWidth = 1F;
            this.FieldCaption.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.FieldCaption.ForeColor = System.Drawing.Color.Black;
            this.FieldCaption.Name = "FieldCaption";
            // 
            // PageInfo
            // 
            this.PageInfo.BackColor = System.Drawing.Color.Transparent;
            this.PageInfo.BorderColor = System.Drawing.Color.Black;
            this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PageInfo.BorderWidth = 1F;
            this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.PageInfo.ForeColor = System.Drawing.Color.Black;
            this.PageInfo.Name = "PageInfo";
            // 
            // DataField
            // 
            this.DataField.BackColor = System.Drawing.Color.Transparent;
            this.DataField.BorderColor = System.Drawing.Color.Black;
            this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DataField.BorderWidth = 1F;
            this.DataField.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.DataField.ForeColor = System.Drawing.Color.Black;
            this.DataField.Name = "DataField";
            this.DataField.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 10.00001F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(211.4586F, 74.00001F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            // 
            // rptRequestHistoryv2
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageHeaderBand1,
            this.groupHeaderBand1,
            this.groupFooterBand1,
            this.reportHeaderBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.efDataSource1});
            this.DataMember = "vwRequest";
            this.DataSource = this.efDataSource1;
            this.Landscape = true;
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.efDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
