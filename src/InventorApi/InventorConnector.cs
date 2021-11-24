using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
		/// Значение для перевода в сантиметры.
		/// </summary>
		private const double Unit = 10.0;

		/// <summary>
		/// Радиус для построения сопряжения верхней части моста.
		/// </summary>
		private const double RadiusTopBridge = 0.15;

		/// <summary>
		/// Радиус для построения сопряжения нижней части моста.
		/// </summary>
		private const double RadiusBottomBridge = 0.4;

		/// <summary>
		/// Радиус для построения сопряжения верхних частей концевых элементов.
		/// </summary>
		private const double RadiusTopEndPieces = 0.2;

		/// <summary>
		/// Радиус для построения сопряжения нижних частей концевых элементов.
		/// </summary>
		private const double RadiusBottomEndPieces = 0.3;

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
				InventorApplication = 
					(Application)Marshal.GetActiveObject("Inventor.Application");
			}
			catch (Exception)
			{
				try
				{
					//Если приложение не открыто, то пытаемся его открыть
					//TODO: RSDN (+)
					Type inventorApplicationType = Type.GetTypeFromProgID("Inventor.Application");

					InventorApplication = 
						(Application)Activator.CreateInstance(inventorApplicationType);
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
		/// <param name="offset">Расстояние от плоскости.</param>
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
		/// <param name="mmRadius">Радиус окружности.</param>
		public void DrawCircle(Point2d centerPoint, double mmRadius)
		{
			var mmCenterPoint = TransientGeometry.CreatePoint2d
				(GetCM(centerPoint.X), GetCM(centerPoint.Y));
			mmRadius /= Unit;
			Sketch.SketchCircles.AddByCenterRadius(mmCenterPoint, mmRadius);
		}

		/// <summary>
		/// Рисует прямоугольник.
		/// </summary>
		/// <param name="pointOne">Первая точка.</param>
		/// <param name="pointTwo">Вторая точка.</param>
		public void DrawRectangle(Point2d pointOne, Point2d pointTwo)
		{
			var mmPointOne = TransientGeometry.CreatePoint2d(GetCM(pointOne.X),
				GetCM(pointOne.Y));
			var mmPointTwo = TransientGeometry.CreatePoint2d(GetCM(pointTwo.X),
				GetCM(pointTwo.Y));
			Sketch.SketchLines.AddAsTwoPointRectangle(mmPointOne, mmPointTwo);
		}

		/// <summary>
		/// Перевод в сантиметры.
		/// </summary>
		/// <param name="value">Значение.</param>
		/// <returns>Значение в сантиметрах.</returns>
		private double GetCM(double value)
		{
			return value / Unit;
		}

		/// <summary>
		/// Выдавливание.
		/// </summary>
		/// <param name="distance">Расстояние.</param>
		public void Extrude(double distance)
		{
			var sketchProfile = Sketch.Profiles.AddForSolid();
			var extrudeDef = PartDefinition.Features.ExtrudeFeatures
				.CreateExtrudeDefinition(sketchProfile, PartFeatureOperationEnum.kJoinOperation);
			extrudeDef.SetDistanceExtent(GetCM(distance),
				PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
			PartDefinition.Features.ExtrudeFeatures.Add(extrudeDef);
		}

		/// <summary>
		/// Создание множества сопряжений.
		/// </summary>
		public void Fillets()
		{
			//TODO: Убрать дублирование (+)
			//TODO: RSDN (+)
			var fillets = new List<FilletDefinition>
			{
				//Сопряжение верхней части моста
				CreateFillet(6, 3, 3, RadiusTopBridge),
				//Сопряжение нижней части моста
				CreateFillet(7, 4, 3, RadiusBottomBridge),
				//Сопряжение верхних частей концевых элементов
				CreateFillet(6, 3, 1, RadiusTopEndPieces),
				//Сопряжение нижних частей концевых элементов
				CreateFillet(7, 4, 1, RadiusBottomEndPieces)
			};

			foreach (var fillet in fillets)
			{
				PartDocument.ComponentDefinition.Features.FilletFeatures.Add(fillet);
			}
		}

		/// <summary>
		/// Сопряжение.
		/// </summary>
		/// <param name="firstFaceIndex">Индекс первой грани.</param>
		/// <param name="secondFaceIndex">Индекс второй грани.</param>
		/// <param name="edgeIndex">Индекс края.</param>
		/// <param name="radius">Радиус.</param>
		/// <returns>Сопряжение.</returns>
		private FilletDefinition CreateFillet(int firstFaceIndex, int secondFaceIndex,
			int edgeIndex, double radius)
		{
			EdgeCollection edgeTopBridge = InventorApplication
				.TransientObjects.CreateEdgeCollection();
			edgeTopBridge.Add(PartDocument.ComponentDefinition
				.Features.ExtrudeFeatures[1].Faces[firstFaceIndex].Edges[edgeIndex]);
			edgeTopBridge.Add(PartDocument.ComponentDefinition
				.Features.ExtrudeFeatures[1].Faces[secondFaceIndex].Edges[edgeIndex]);

			FilletDefinition filletDefinitionTopBridge = PartDocument
				.ComponentDefinition.Features.FilletFeatures.CreateFilletDefinition();
			filletDefinitionTopBridge.AddConstantRadiusEdgeSet(edgeTopBridge, radius);
			return filletDefinitionTopBridge;
		}
	}
}
