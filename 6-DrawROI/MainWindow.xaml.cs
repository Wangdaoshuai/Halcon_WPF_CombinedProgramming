using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _6_DrawROI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        HImage image;

        private void Btn_LoadImage_Click(object sender, RoutedEventArgs e)
        {
            int width,height;
            image = new HImage();
            image.ReadImage("C:\\CodeLearning\\Halcon_Learning\\15\\standard.bmp");
            image.GetImageSize(out width, out height);
            HS.HalconWindow.SetPart(0,0,width,height);
            HS.HalconWindow.DispObj(image);
            HS.SetFullImagePart(image);
        }
        List<HDrawingObject> hDrawingObjects= new List<HDrawingObject>();
        List<DrawingObjectExtension> drawingObjectExtensions = new List<DrawingObjectExtension>();  
        private void Btn_LoadCircleImage_Click(object sender, RoutedEventArgs e)
        {
            var tuples = new HTuple[] { 100, 100, 100 };
            var circle = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE, tuples);
            hDrawingObjects.Add(circle);
            drawingObjectExtensions.Add(new DrawingObjectExtension
            {
                DrawObj = circle,
                HTuples =tuples
            });
            //注册拖拽的回调
            circle.OnDrag(HDrawingObjectDragCallbackClass);
            //注册图形尺寸变化的回调
            circle.OnResize(HDrawingObjectResizeCallbackClass);

            HS.HalconWindow.AttachDrawingObjectToWindow(circle);
        }

        private void Btn_LoadRectImage_Click(object sender, RoutedEventArgs e)
        {
            var rect = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, new HTuple[] { 100, 100, 100,150 });
            hDrawingObjects.Add(rect);
            HS.HalconWindow.AttachDrawingObjectToWindow(rect);
        }

        private void Btn_LoadEpImage_Click(object sender, RoutedEventArgs e)
        {
            var ellpse = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.ELLIPSE, new HTuple[] { 100, 100, 100, 150,100,100 });
            ellpse.OnDrag(HDrawingObjectDragCallbackClass);
            hDrawingObjects.Add(ellpse);
            HS.HalconWindow.AttachDrawingObjectToWindow(ellpse);
        }
        /// <summary>
        /// 圆形拖拽的回调方法
        /// </summary>
        /// <param name="drawid"></param>
        /// <param name="window"></param>
        /// <param name="type"></param>
        public void HDrawingObjectDragCallbackClass(HDrawingObject drawid, HWindow window, string type)
        {
            Render(drawid);
        }
        // 圆形尺寸变化的回调方法
        public void HDrawingObjectResizeCallbackClass(HDrawingObject drawid, HWindow window, string type)
        {
            Render(drawid);
        }
        /// <summary>
        /// 监控图形属性变化的方法
        /// </summary>
        /// <param name="drawid"></param>
        private void Render(HDrawingObject drawid)
        {
            //获取当前拖拽对象的坐标和半径
            var row = drawid.GetDrawingObjectParams("row");
            var column = drawid.GetDrawingObjectParams("column");
            var radius = drawid.GetDrawingObjectParams("radius");
            //创建属性数组
            var tuples = new HTuple[3];
            tuples[0] = row;
            tuples[1] = column;
            tuples[2] = radius;
            //获取拖拽对象，判断是否是同一个对象
            var obj = drawingObjectExtensions.FirstOrDefault(d => d.DrawObj?.ID == drawid.ID);
            if (obj != null)
            {
                //更新数据
                obj.HTuples = tuples;
            }

            Debug.WriteLine($"当前属性值:row:{row},column:{column},radius:{radius}");
        }

        HObject ho_Circle;
        HObject ho_ImageReduced;
        HTuple hv_ModelID = new HTuple();
        /// <summary>
        /// 创建模板的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CreateModel_Click(object sender, RoutedEventArgs e)
        {
            //获取ROI对象属性
            var roiAttrs = drawingObjectExtensions[0].HTuples;

            HOperatorSet.GenCircle(out ho_Circle, roiAttrs?[0], roiAttrs?[1], roiAttrs?[2]);
            HOperatorSet.ReduceDomain(image, ho_Circle, out ho_ImageReduced);
            //做模板
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
       
                HOperatorSet.CreateScaledShapeModel(ho_ImageReduced, "auto", (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), "auto", 0.9, 1.1, "auto", "auto", "use_polarity",
                    "auto", "auto", out hv_ModelID);
            }
        }

        //显示匹配结果
        private void Btn_Result_Click(object sender, RoutedEventArgs e)
        {
            new ModelService().ExcuteModel(HS.HalconWindow, image, hv_ModelID);

        }
    }
    public class DrawingObjectExtension
    {
        //加问号代表可为空类型
        public HDrawingObject? DrawObj { get; set; }
        public HTuple[]? HTuples { get; set; }
    }
}
