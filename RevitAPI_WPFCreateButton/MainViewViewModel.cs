using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPI_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_WPFCreateButton
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand CountPipes { get; }
        public DelegateCommand VolumeWalls { get; }
        public DelegateCommand CountDoors { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            CountPipes = new DelegateCommand(OnCountPipes);
            VolumeWalls = new DelegateCommand(OnVolumeWalls);
            CountDoors = new DelegateCommand(OnCountDoors);
        }

        private void OnCountDoors()
        {
            RaiseHideRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var doors = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .ToList();
            TaskDialog.Show("Сообщение", $"Количество дверей в модели:{doors.Count}");

            RaiseShowRequest();
        }

        private void OnVolumeWalls()
        {
            RaiseHideRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var walls = new FilteredElementCollector(doc)
                 .OfClass(typeof(Wall))
                 .Cast<Wall>()
                 .ToList();

            double volWall = 0;

            foreach (var item in walls)
            {
                volWall += UnitUtils.ConvertFromInternalUnits(item.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters);
            }

            TaskDialog.Show("Объём стен:", volWall.ToString()+" м³");

            RaiseShowRequest();
        }

        private void OnCountPipes()
        {
            RaiseHideRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var pipes = new FilteredElementCollector(doc)
             .OfClass(typeof(Pipe))
             .Cast<Pipe>()
             .ToList();

            TaskDialog.Show("Сообщение", $"Количество труб в модели:{pipes.Count}");

            RaiseShowRequest();
        }


        public event EventHandler HideRequest;
        public event EventHandler ShowRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
