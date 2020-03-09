using AForge.Video.DirectShow;
using ArcFaceDemo.SDKModels;
using ArcFaceDemo.SDKUtil;
using ArcFaceDemo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcFaceDemo
{
    public partial class MainFrm : Form
    {
        /// <summary>
        /// 引擎Handle
        /// </summary>
        private IntPtr pImageEngine = IntPtr.Zero;
        /// <summary>
        /// 保存对比图片的列表
        /// </summary>
        private List<string> imagePathList = new List<string>();
        /// <summary>
        /// 左侧图库人脸特征列表
        /// </summary>
        private List<IntPtr> imagesFeatureList = new List<IntPtr>();
        private Dictionary<IntPtr, string> imagesFeatureDictionary = new Dictionary<IntPtr, string>();

        IntPtr currentLeftFeature;
        /// <summary>
        /// 右侧图片人脸特征
        /// </summary>
        private IntPtr image1Feature;
        /// <summary>
        /// 相似度
        /// </summary>
        private float threshold = 0.8f;
        /// <summary>
        /// 图片最大大小
        /// </summary>
        private long maxSize = 1024 * 1024 * 2;
        /// <summary>
        /// RGB 摄像头索引
        /// </summary>
        private int rgbCameraIndex = 0;
        /// <summary>
        /// IR 摄像头索引
        /// </summary>
        private int irCameraIndex = 0;

        #region 视频模式下相关
        /// <summary>
        /// 视频引擎Handle
        /// </summary>
        private IntPtr pVideoEngine = IntPtr.Zero;
        /// <summary>
        /// RGB视频引擎 FR Handle 处理   FR和图片引擎分开，减少强占引擎的问题
        /// </summary>
        private IntPtr pVideoRGBImageEngine = IntPtr.Zero;
        /// <summary>
        /// IR视频引擎 FR Handle 处理   FR和图片引擎分开，减少强占引擎的问题
        /// </summary>
        private IntPtr pVideoIRImageEngine = IntPtr.Zero;
        /// <summary>
        /// 视频输入设备信息
        /// </summary>
        private FilterInfoCollection filterInfoCollection;
        /// <summary>
        /// RGB摄像头设备
        /// </summary>
        private VideoCaptureDevice rgbDeviceVideo;
        /// <summary>
        /// IR摄像头设备
        /// </summary>
        private VideoCaptureDevice irDeviceVideo;
        #endregion

        public MainFrm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //初始化引擎
            InitEngines();
        }

        private void InitEngines()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("SDKKEY32", typeof(string));
            rgbCameraIndex = (int)reader.GetValue("RGB_CAMERA_INDEX", typeof(int));
            irCameraIndex = (int)reader.GetValue("IR_CAMERA_INDEX", typeof(int));
            //判断CPU位数
            var is64CPU = Environment.Is64BitProcess;
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(is64CPU ? sdkKey64 : sdkKey32))
            {
                //禁用相关功能按钮
                //ControlsEnable(false, chooseMultiImgBtn, matchBtn, btnClearFaceList, chooseImgBtn);
                MessageBox.Show(string.Format("请在App.config配置文件中先配置APP_ID和SDKKEY{0}!", is64CPU ? "64" : "32"));
                return;
            }

            //在线激活引擎    如出现错误，1.请先确认从官网下载的sdk库已放到对应的bin中，2.当前选择的CPU为x86或者x64
            int retCode = 0;
            try
            {
                retCode = ASFFunctions.ASFActivation(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                //禁用相关功能按钮
                //ControlsEnable(false, chooseMultiImgBtn, matchBtn, btnClearFaceList, chooseImgBtn);
                if (ex.Message.Contains("无法加载 DLL"))
                {
                    MessageBox.Show("请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");
                }
                else
                {
                    MessageBox.Show("激活引擎失败!");
                }
                return;
            }
            Console.WriteLine("Activate Result:" + retCode);

            //初始化引擎
            uint detectMode = DetectionMode.ASF_DETECT_MODE_IMAGE;
            //Video模式下检测脸部的角度优先值
            int videoDetectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_HIGHER_EXT;
            //Image模式下检测脸部的角度优先值
            int imageDetectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_ONLY;
            //人脸在图片中所占比例，如果需要调整检测人脸尺寸请修改此值，有效数值为2-32
            int detectFaceScaleVal = 16;
            //最大需要检测的人脸个数
            int detectFaceMaxNum = 5;
            //引擎初始化时需要初始化的检测功能组合
            int combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_AGE | FaceEngineMask.ASF_GENDER | FaceEngineMask.ASF_FACE3DANGLE;
            //初始化引擎，正常值为0，其他返回值请参考http://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=19&_dsign=dbad527e
            retCode = ASFFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pImageEngine);
            Console.WriteLine("InitEngine Result:" + retCode);
            AppendText((retCode == 0) ? "引擎初始化成功!\r\n" : string.Format("引擎初始化失败!错误码为:{0}\r\n", retCode));
            if (retCode != 0)
            {
                //禁用相关功能按钮
                //ControlsEnable(false, chooseMultiImgBtn, matchBtn, btnClearFaceList, chooseImgBtn);
            }

            //初始化视频模式下人脸检测引擎
            uint detectModeVideo = DetectionMode.ASF_DETECT_MODE_VIDEO;
            int combinedMaskVideo = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION;
            retCode = ASFFunctions.ASFInitEngine(detectModeVideo, videoDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMaskVideo, ref pVideoEngine);
            //RGB视频专用FR引擎
            detectFaceMaxNum = 1;
            combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_LIVENESS;
            retCode = ASFFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pVideoRGBImageEngine);

            //IR视频专用FR引擎
            combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_IR_LIVENESS;
            retCode = ASFFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pVideoIRImageEngine);

            Console.WriteLine("InitVideoEngine Result:" + retCode);


            initVideo();
        }

        private void initVideo()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //如果没有可用摄像头，“启用摄像头”按钮禁用，否则使可用
            //if (filterInfoCollection.Count == 0)
            //{
            //    btnStartVideo.Enabled = false;
            //}
            //else
            //{
            //    btnStartVideo.Enabled = true;
            //}
        }

        private void btnSelectImageToRegister_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select";
            openFileDialog.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png";
            //openFileDialog.Multiselect = true;
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var numStart = imagePathList.Count;
                string fileName = openFileDialog.FileName;
                if (!checkImage(fileName))
                    return;

                pictureBoxSelected.ImageLocation = fileName;
                currentLeftFeature = IntPtr.Zero;

                //人脸检测以及提取人脸特征
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    Image image = ImageUtil.readFromFile(fileName);
                    if (image == null)
                    {
                        return;
                    }
                    if (image.Width > 1536 || image.Height > 1536)
                    {
                        image = ImageUtil.ScaleImage(image, 1536, 1536);
                    }
                    if (image == null)
                    {
                        return;
                    }
                    if (image.Width % 4 != 0)
                    {
                        image = ImageUtil.ScaleImage(image, image.Width - (image.Width % 4), image.Height);
                    }

                    //人脸检测
                    ASF_MultiFaceInfo multiFaceInfo = FaceUtil.DetectFace(pImageEngine, image);
                    //判断检测结果
                    if (multiFaceInfo.faceNum > 0)
                    {
                        MRECT rect = MemoryUtil.PtrToStructure<MRECT>(multiFaceInfo.faceRects);
                        image = ImageUtil.CutImage(image, rect.left, rect.top, rect.right, rect.bottom);
                    }
                    else
                    {
                        if (image != null)
                        {
                            image.Dispose();
                        }
                        return;
                    }

                    //提取人脸特征
                    ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
                    Image image1 = ImageUtil.readFromFile(fileName);
                    if (image1 == null)
                    {
                        return;
                    }
                    currentLeftFeature = FaceUtil.ExtractFeature(pImageEngine, image1, out singleFaceInfo);
                    this.Invoke(new Action(delegate
                    {
                        if (singleFaceInfo.faceRect.left == 0 && singleFaceInfo.faceRect.right == 0)
                        {
                            AppendText(string.Format("No face detected\r\r\n"));
                        }
                        else
                        {
                            AppendText(string.Format("Face landmark detected，[left:{0},right:{1},top:{2},bottom:{3},orient:{4}]\r\r\n", singleFaceInfo.faceRect.left, singleFaceInfo.faceRect.right, singleFaceInfo.faceRect.top, singleFaceInfo.faceRect.bottom, singleFaceInfo.faceOrient));
                            imagesFeatureList.Add(currentLeftFeature);
                        }
                    }));
                    if (image1 != null)
                    {
                        image1.Dispose();
                    }

                }));
            }
        }

        private void btnRegisterFace_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Set a name for current person");
                return;
            }

            imagesFeatureDictionary.Add(currentLeftFeature, textBoxName.Text);
            AppendText(string.Format(textBoxName.Text + " register success!\r\r\n"));
        }

        private void btnSelectImageToRecognize_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select";
            openFileDialog.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png";
            //openFileDialog.Multiselect = true;
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var numStart = imagePathList.Count;
                string fileName = openFileDialog.FileName;
                if (!checkImage(fileName))
                    return;

                image1Feature = IntPtr.Zero;
                pictureBoxToRecognize.ImageLocation = fileName;
                Image srcImage = ImageUtil.readFromFile(fileName);

                ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
                //提取人脸特征
                image1Feature = FaceUtil.ExtractFeature(pImageEngine, srcImage, out singleFaceInfo);

                if (imagesFeatureList.Count == 0)
                {
                    MessageBox.Show("请注册人脸!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (image1Feature == IntPtr.Zero)
                {
                    if (pictureBoxToRecognize.Image == null)
                    {
                        MessageBox.Show("请选择识别图!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("比对失败，识别图未提取到特征值!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                for (int i = 0; i < imagesFeatureDictionary.Count; i++)
                {
                    IntPtr feature = imagesFeatureDictionary.ElementAt(i).Key;
                    float similarity = 0f;
                    int ret = ASFFunctions.ASFFaceFeatureCompare(pImageEngine, image1Feature, feature, ref similarity);
                    //增加异常值处理
                    if (similarity.ToString().IndexOf("E") > -1)
                        similarity = 0f;

                    if(similarity > threshold)
                    {
                        string name = imagesFeatureDictionary.ElementAt(i).Value;
                        AppendText("对比结果：" + name + "  可信度：" + similarity + "\r\n");
                        return;
                    }
                }
                AppendText("无结果\r\n");
            }
        }
        
        private void AppendText(string message)
        {
            logBox.AppendText(message);
        }

        private bool checkImage(string imagePath)
        {
            if (imagePath == null)
            {
                AppendText("Image not existed\r\r\n");
                return false;
            }
            try
            {
                //判断图片是否正常，如将其他文件把后缀改为.jpg，这样就会报错
                Image image = ImageUtil.readFromFile(imagePath);
                if (image == null)
                {
                    throw new Exception();
                }
                else
                {
                    image.Dispose();
                }
            }
            catch
            {
                AppendText(string.Format("{0} Bad image format\r\r\n", imagePath));
                return false;
            }
            FileInfo fileCheck = new FileInfo(imagePath);
            if (fileCheck.Exists == false)
            {
                AppendText(string.Format("{0} not existed\r\r\n", fileCheck.Name));
                return false;
            }
            else if (fileCheck.Length > maxSize)
            {
                AppendText(string.Format("{0} File size larger than 2MB\r\r\n", fileCheck.Name));
                return false;
            }
            else if (fileCheck.Length < 2)
            {
                AppendText(string.Format("{0} File size too small\r\r\n", fileCheck.Name));
                return false;
            }
            return true;
        }

    }
}
