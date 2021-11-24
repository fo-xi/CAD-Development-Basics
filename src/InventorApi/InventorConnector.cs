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
	/// <summary>
	/// Класс, обеспечивающий взаимодействие с необходимыми методами Inventor API.
	/// </summary>
	public class InventorConnector
	{
		/// <summary>
		/// Возвращает и задает приложение Inventor.
		/// </summary>
		public Application InventorApplication { get; set; }

		/// <summary>
		/// Возвращает и задает документ в приложении.
		/// </summary>
		public PartDocument PartDocument { get; set; }

		/// <summary>
		/// Возвращает и задает описание документа.
		/// </summary>
		public PartComponentDefinition PartDefinition { get; set; }

		/// <summary>
		/// Возвращает и задает геометрию приложения.
		/// </summary>
		public TransientGeometry TransientGeometry { get; set; }

		/// <summary>
		/// Возвращает и задает скетч.
		/// </summary>
		public PlanarSketch Sketch { get; set; }

		/// <summary>
		/// Создание нового документа.
		/// </summary>
		public void CreateNewDocument()
		{
			//Инициализация Inventor
			try
			{
				InventorApplication = (Application)Marshal.GetActiveObject("Inventor.Application");
			}
			catch (Exception)
			{
				try
				{
					//TODO: RSDN
					Type _inventorApplicationType = Type.GetTypeFromProgID("Inventor.Application");

					InventorApplication = (Application)Activator.CreateInstance(_inventorApplicationType);
					InventorApplication.Visible = true;
				}
				catch (Exception)
				{
					MessageBox.Show("Не получилось запустить Inventor.");
				}
			}

			//Инициализация документа сборки
			PartDocument = (PartDocument)InventorApplication.Documents.Add
			(DocumentTypeEnum.kPartDocumentObject,
				InventorApplication.FileManager.GetTemplateFile
				(DocumentTypeEnum.kPartDocumentObject,
					SystemOfMeasureEnum.kMetricSystemOfMeasure));

			PartDefinition = PartDocument.ComponentDefinition;
			TransientGeometry = InventorApplication.TransientGeometry;
		}

		/// <summary>
		/// Создает скетч.
		/// </summary>
		/// <param name="offset">Номер плоскости: 1 - ZY, 2 - ZX, 3 - XY.</param>
		/// <returns>Скетч.</returns>
		public PlanarSketch MakeNewSketch(double offset)
		{
			int planeNumber = 3;
			//Получаем ссылку на рабочую плоскость.
			var mainPlane = PartDefinition.WorkPlanes[planeNumber];
			var offsetPlane = PartDefinition.WorkPlanes.AddByPlaneAndOffset(mainPlane, offset);
			//Создаем на плоскости скетч.
			var sketch = PartDefinition.Sketches.Add(offsetPlane);
			offsetPlane.Visible = false;
			return sketch;
		}

		/// <summary>
		/// Рисует круг.
		/// </summary>
		/// <param name="centerPoint">Центральная точка.</param>
		/// <param name="radius">Радиус окружности.</param>
		public void DrawCircle(Point2d centerPoint, double radius)
		{
			var mmCenterPoint = TransientGeometry.CreatePoint2d(centerPoint.X / 10.0,
				centerPoint.Y / 10.0);
			radius /= 10.0;
			Sketch.SketchCircles.AddByCenterRadius(mmCenterPoint, radius);
		}

		/// <summary>
		/// Рисует линию.
		/// </summary>
		/// <param name="startPoint">Точка начала линиии.</param>
		/// <param name="endPoint">Точка конца линии.</param>
		public void DrawLine(Point2d startPoint, Point2d endPoint)
		{
			var mmStartPoint = TransientGeometry.CreatePoint2d(startPoint.X / 10.0,
				startPoint.Y / 10.0);
			var mmEndPoint = TransientGeometry.CreatePoint2d(endPoint.X / 10.0,
				endPoint.Y / 10.0);
			Sketch.SketchLines.AddByTwoPoints(mmStartPoint, mmEndPoint);
		}

		/// <summary>
		/// Рисует прямоугольник.
		/// </summary>
		/// <param name="pointOne">Первая точка.</param>
		/// <param name="pointTwo">Вторая точка.</param>
		public void DrawRectangle(Point2d pointOne, Point2d pointTwo)
		{
			var mmPointOne = TransientGeometry.CreatePoint2d(pointOne.X / 10.0,
				pointOne.Y / 10.0);
			var mmPointTwo = TransientGeometry.CreatePoint2d(pointTwo.X / 10.0,
				pointTwo.Y / 10.0);
			Sketch.SketchLines.AddAsTwoPointRectangle(mmPointOne, mmPointTwo);
		}

		/// <summary>
		/// Выдавливание.
		/// </summary>
		/// <param name="distance">Расстояние.</param>
		public void Extrude(double distance)
		{
			var sketchProfile = Sketch.Profiles.AddForSolid();
			var extrudeDef = PartDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(sketchProfile,
				PartFeatureOperationEnum.kJoinOperation);
			extrudeDef.SetDistanceExtent(distance / 10.0, PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
			PartDefinition.Features.ExtrudeFeatures.Add(extrudeDef);
		}

		/// <summary>
		/// Сопряжение.
		/// </summary>
		public void Fillet()
		{
			//TODO: Убрать дублирование
            //TODO: RSDN
			EdgeCollection edgeTopBridge = InventorApplication.TransientObjects.CreateEdgeCollection();
			edgeTopBridge.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[6].Edges[3]);
			edgeTopBridge.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[3].Edges[3]);

            FilletDefinition filletDefinitionTopBridge = PartDocument.ComponentDefinition.Features.FilletFeatures.CreateFilletDefinition();
            filletDefinitionTopBridge.AddConstantRadiusEdgeSet(edgeTopBridge, 0.15);
            PartDocument.ComponentDefinition.Features.FilletFeatures.Add(filletDefinitionTopBridge);

			EdgeCollection edgeBottomBridge = InventorApplication.TransientObjects.CreateEdgeCollection();
			edgeBottomBridge.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[7].Edges[3]);
			edgeBottomBridge.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[4].Edges[3]);


			EdgeCollection edgeTopEndPiece = InventorApplication.TransientObjects.CreateEdgeCollection();
			edgeTopEndPiece.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[6].Edges[1]);
			edgeTopEndPiece.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[3].Edges[1]);

			EdgeCollection edgeBottomEndPiece = InventorApplication.TransientObjects.CreateEdgeCollection();
			edgeBottomEndPiece.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[7].Edges[1]);
			edgeBottomEndPiece.Add(PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1].Faces[4].Edges[1]);

			

			FilletDefinition filletDefinitionBottomBridge = PartDocument.ComponentDefinition.Features.FilletFeatures.CreateFilletDefinition();
			filletDefinitionBottomBridge.AddConstantRadiusEdgeSet(edgeBottomBridge, 0.4);

			FilletDefinition filletDefinitionTopEndPiece = PartDocument.ComponentDefinition.Features.FilletFeatures.CreateFilletDefinition();
			filletDefinitionTopEndPiece.AddConstantRadiusEdgeSet(edgeTopEndPiece, 0.2);

			FilletDefinition filletDefinitionBottomEndPiece = PartDocument.ComponentDefinition.Features.FilletFeatures.CreateFilletDefinition();
			filletDefinitionBottomEndPiece.AddConstantRadiusEdgeSet(edgeBottomEndPiece, 0.3);

			PartDocument.ComponentDefinition.Features.FilletFeatures.Add(filletDefinitionBottomBridge);
			PartDocument.ComponentDefinition.Features.FilletFeatures.Add(filletDefinitionTopEndPiece);
			PartDocument.ComponentDefinition.Features.FilletFeatures.Add(filletDefinitionBottomEndPiece);
		}
	}
}
