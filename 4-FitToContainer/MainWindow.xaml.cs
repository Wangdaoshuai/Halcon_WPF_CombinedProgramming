﻿using HalconDotNet;
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

namespace _4_FitToContainer
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
        /// <summary>
        /// 加载HWindowControlWPF控件的图片  
        /// 默认无法拖动、缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_LoadHWImage_Click(object sender, RoutedEventArgs e)
        {
            int width, height;
            var image = new HImage();
            image.ReadImage("C:\\CodeLearning\\FirstHalcon\\3-SmartAndHWDemo\\test.jpg");
            image.GetImageSize(out width, out height);
            HW.HalconWindow.SetPart(0, 0, width, height);
            HW.HalconWindow.DispObj(image);
            HW.SetFullImagePart(image);
        }

        /// <summary>
        /// 加载HSmartWindowControlWPF控件的图片 
        /// 默认可以拖动、缩放  两次点击鼠标右键可以自适应窗口
        /// SetFullImagePart()可以设置自适应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_LoadHSImage_Click(object sender, RoutedEventArgs e)
        {
            var image = new HImage();
            image.ReadImage("C:\\CodeLearning\\FirstHalcon\\3-SmartAndHWDemo\\test.jpg");
            HS.HalconWindow.DispObj(image);
            HS.SetFullImagePart();
            
        }
    }
}
