using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;

namespace SmartTalentManagement.Util
{
    public static class PerformExcelExport
    {
        public static void ExportGridViewData(RadGridView gridView, string destination, string fileName)
        {
            GridViewSpreadExport spreadExporter = new GridViewSpreadExport(gridView);
            SpreadExportRenderer exportRenderer = new SpreadExportRenderer();

            spreadExporter.HiddenColumnOption = HiddenOption.DoNotExport;
            spreadExporter.SheetName = fileName;
            spreadExporter.ExportVisualSettings = true;
            spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;

            spreadExporter.RunExportAsync(destination, exportRenderer);

            RadMessageBox.Show("Export completed successfully.", Application.ProductName);
        }
    }
}
