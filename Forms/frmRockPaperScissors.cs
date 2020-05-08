﻿using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.ML;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MachineLearningForKids.Forms
{
    public partial class frmRockPaperScissors : Form
    {
        private MLContext mlContext;
        private GenerateModel generateModel;
        private Prediction prediction;
        public frmRockPaperScissors()
        {
            InitializeComponent();
        }
        
        private void frmRockPaperScissors_Load(object sender, EventArgs e)
        {
            AygitYukle();
        }
        private FilterInfoCollection goruntuAygitlari;
        private VideoCaptureDevice kameramiz;
        private void kameramiz_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bit = (Bitmap)e.Frame.Clone();
            imgGiris.Image = bit;
        }
        public void AygitYukle()
        {
            goruntuAygitlari = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo aygit in goruntuAygitlari)
            {
                comboBox1.Items.Add(aygit);
            }
            comboBox1.SelectedIndex = 0;
            kameramiz = new VideoCaptureDevice(goruntuAygitlari[comboBox1.SelectedIndex].MonikerString);
            kameramiz.NewFrame += new NewFrameEventHandler(kameramiz_NewFrame);
            kameramiz.Start();
        }
        public void Predict(MLContext mLContext, ITransformer model)
        {

            prediction = new Prediction();
            string _predictSingleImage = Application.StartupPath + "\\images\\predict.jpg";
            string result = prediction.ClassifySingleImage(mlContext, model, _predictSingleImage);

            MessageBox.Show(result);
        }

        
        public void Result()
        {
            string dataName = "./data/rockpaperscissors.csv";
            generateModel = new GenerateModel();
            mlContext = new MLContext();
            var model = generateModel.TrainModel(mlContext, dataName);
            Predict(mlContext, model);
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            imgTahmin.Image = imgGiris.Image;
            imgTahmin.Image.Save(Application.StartupPath + "\\images\\predict.jpg");
            Result();
        }
    }
}
