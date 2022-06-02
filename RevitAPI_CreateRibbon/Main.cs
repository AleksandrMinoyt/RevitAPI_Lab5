using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace RevitAPI_CreateRibbon
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Учебные проекты";

            application.CreateRibbonTab(tabName);

            string folderPath = @"D:\RevitAPITrainig\";

            var panel = application.CreateRibbonPanel(tabName, "Лабораторка 5");
            var buttonChangeWallType = new PushButtonData("Тип стен", "Смена типа выбраных стен", Path.Combine(folderPath, "RevitAPI_ChangeWallType.dll"), "RevitAPI_ChangeWallType.Main");
            var buttonWPFFormModelProperty = new PushButtonData("Информация", "Вывод информации о модели", Path.Combine(folderPath, "RevitAPI_WPFCreateButton.dll"), "RevitAPI_WPFCreateButton.Main");

            Uri UriImagebuttonChangeWallType = new Uri(Path.Combine(folderPath, @"images\Process.png"),UriKind.Absolute);
            Uri UriImagebuttonWPFFormModelProperty = new Uri(Path.Combine(folderPath, @"images\Report.png"), UriKind.Absolute);

            BitmapImage ImagebuttonChangeWallType = new BitmapImage(UriImagebuttonChangeWallType);
            BitmapImage ImagebuttonWPFFormModelProperty = new BitmapImage(UriImagebuttonWPFFormModelProperty);

            buttonChangeWallType.LargeImage = ImagebuttonChangeWallType;
            buttonWPFFormModelProperty.LargeImage = ImagebuttonWPFFormModelProperty;

            panel.AddItem(buttonChangeWallType);
            panel.AddItem(buttonWPFFormModelProperty);


            return Result.Succeeded;
        }
    }
}
