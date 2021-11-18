using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Inventor;
using Application = Inventor.Application;

namespace InventorApi
{
    public class InventorConnector
    {
        public Application InventorApplication { get; set; }

        public PartDocument PartDocument { get; set; }

        public PartComponentDefinition PartDefinition { get; set; }

        public TransientGeometry TransientGeometry { get; set; }

        public PlanarSketch Sketch { get; set; }

        public ObjectCollection CreateObjectCollection()
        {
            return InventorApplication.TransientObjects.CreateObjectCollection();
        }

        public void CreateNewDocument()
        {
            InventorApplication = null;

            try
            {
                InventorApplication = (Application)Marshal.GetActiveObject("Inventor.Application");
            }
            catch (Exception)
            {
                try
                {
                    Type _inventorApplicationType = Type.GetTypeFromProgID("Inventor.Application");

                    InventorApplication = (Application)Activator.CreateInstance(_inventorApplicationType);
                    InventorApplication.Visible = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Не получилось запустить Inventor.");
                }
            }

            PartDocument = (PartDocument)InventorApplication.Documents.Add
            (DocumentTypeEnum.kPartDocumentObject,
                InventorApplication.FileManager.GetTemplateFile
                (DocumentTypeEnum.kPartDocumentObject,
                    SystemOfMeasureEnum.kMetricSystemOfMeasure));

            PartDefinition = PartDocument.ComponentDefinition;
            TransientGeometry = InventorApplication.TransientGeometry;
        }

        public PlanarSketch MakeNewSketch(double offset)
        {
            //[1 - ZY; 2 - ZX; 3 - XY]
            int planeNumber = 3;
            var mainPlane = PartDefinition.WorkPlanes[planeNumber];
            var offsetPlane = PartDefinition.WorkPlanes.AddByPlaneAndOffset(mainPlane, offset);
            var sketch = PartDefinition.Sketches.Add(offsetPlane);
            return sketch;
        }

        public void DrawCircle(Point2d centerPoint, double radius)
        {
            Sketch.SketchCircles.AddByCenterRadius(centerPoint, radius);
        }

        public void DrawLine(Point2d startPoint, Point2d endPoint)
        {
            Sketch.SketchLines.AddByTwoPoints(startPoint, endPoint);
        }

        public void Extrude(double distance)
        {
            var sketchProfile = Sketch.Profiles.AddForSolid();
            var extrudeDef = PartDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(sketchProfile,
                PartFeatureOperationEnum.kJoinOperation);
            extrudeDef.SetDistanceExtent(distance, PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
            PartDefinition.Features.ExtrudeFeatures.Add(extrudeDef);
        }
    }
}
