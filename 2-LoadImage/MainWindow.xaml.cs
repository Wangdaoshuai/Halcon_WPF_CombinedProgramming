using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace _2_LoadImage
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

        //加载图片的按钮事件
        private void Btn_LoadImage_Click(object sender, RoutedEventArgs e)
        {
            //1 创建打开图片的对话框
            var imageFileDialog = new OpenFileDialog();
            //2 设置对话框能够打开图片类型
            imageFileDialog.Filter = "图片|*.jpg;*.png";
            //3 设置默认打开图片扩展名称
            imageFileDialog.DefaultExt = ".png";
            //4 判断对话框是否打开
            if(imageFileDialog.ShowDialog() == true)
            {
                //5 获取选择的图片名称
                var fileName = imageFileDialog.FileName;
                //6 创建Halcon图片对象
                var image = new HImage();
                //7 把打开的图片文件读取到HImage对象中
                image.ReadImage(fileName);
                //8 清空控件上显示的图片
                Hsmart.HalconWindow.ClearWindow();
                //9 使用Halcon控件显示图片
                Hsmart.HalconWindow.DispObj(image);
            }
        }
    }
}
