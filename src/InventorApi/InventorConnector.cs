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
            int planeNumber = 3;
            var mainPlane = PartDefinition.WorkPlanes[planeNumber];
            var offsetPlane = PartDefinition.WorkPlanes.AddByPlaneAndOffset(mainPlane, offset);
            var sketch = PartDefinition.Sketches.Add(offsetPlane);
            return sketch;
        }

        public void DrawCircle(Point2d centerPoint, double radius)
        {
            var mmCenterPoint = TransientGeometry.CreatePoint2d(centerPoint.X / 10.0,
                centerPoint.Y / 10.0);
            radius /= 10.0;
            Sketch.SketchCircles.AddByCenterRadius(mmCenterPoint, radius);
        }

        public void DrawLine(Point2d startPoint, Point2d endPoint)
        {
            var mmStartPoint = TransientGeometry.CreatePoint2d(startPoint.X / 10.0,
                startPoint.Y / 10.0);
            var mmEndPoint = TransientGeometry.CreatePoint2d(endPoint.X / 10.0,
                endPoint.Y / 10.0);
            Sketch.SketchLines.AddByTwoPoints(mmStartPoint, mmEndPoint);
        }

        public void DrawRectangle(Point2d pointOne, Point2d pointTwo)
        {
            var mmPointOne = TransientGeometry.CreatePoint2d(pointOne.X / 10.0,
                pointOne.Y / 10.0);
            var mmPointTwo = TransientGeometry.CreatePoint2d(pointTwo.X / 10.0,
                pointTwo.Y / 10.0);
            Sketch.SketchLines.AddAsTwoPointRectangle(mmPointOne, mmPointTwo);
        }

        public void Extrude(double distance)
        {
            var sketchProfile = Sketch.Profiles.AddForSolid();
            var extrudeDef = PartDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(sketchProfile,
                PartFeatureOperationEnum.kJoinOperation);
            extrudeDef.SetDistanceExtent(distance / 10.0, PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
            PartDefinition.Features.ExtrudeFeatures.Add(extrudeDef);
        }
    }
}
