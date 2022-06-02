using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_ChangeWallType
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public DelegateCommand ApplyWallsType { get; }

        public List<WallType> WallsTypes { get; } = new List<WallType>();

        public List<Wall> walls { get; } = new List<Wall>();

        public WallType SelectedWallsType { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ApplyWallsType = new DelegateCommand(OnApplyWallsType);

            WallsTypes = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_Walls)
                            .WhereElementIsElementType()
                            .Cast<WallType>()
                            .ToList();

            walls = uidoc.Selection.PickObjects(ObjectType.Element, new SelectWallsFilter(), "Выберите стены").Select(x => doc.GetElement(x)).Cast<Wall>().ToList();

        }

        private void OnApplyWallsType()
        {
            RaiseCloseRequest();
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (walls.Count == 0 || SelectedWallsType == null)
                return;
            using (Transaction trans = new Transaction(doc, "Изменение типа стен"))
            {
                trans.Start();
                foreach (var item in walls)
                {
                    item.WallType = SelectedWallsType;
                }
                trans.Commit();
            }
        }



        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

    }
    public class SelectWallsFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is Wall;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
