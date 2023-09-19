using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace PP_Human
{
    /// <summary>
    /// 识别检测结果类
    /// </summary>
    public class ResBboxs 
    {
        public List<Rect> bboxs; // 预测框
        public float[] scores; // 预测分数
        public int[] classes; // 预测分类

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bboxs">预测框</param>
        /// <param name="scores">预测分数</param>
        /// <param name="classes">预测分类</param>
        public ResBboxs(List<Rect> bboxs, float[] scores, int[] classes)
        {
            this.bboxs = bboxs;
            this.scores = scores;
            this.classes = classes;
        }
    }

    public class KeyPoints
    {
        // 视频帧
        public int mot_id { get; set; }
        // 关键点和分数
        public float[,] points { get; set; }
        // 行人位置框
        public Rect bbox { get; set; }
        // 识别框中心
        public Point center { get; set; }

        public KeyPoints(int mot_id, float[,] points, Rect bbox)
        {
            this.mot_id = mot_id;
            this.points = (float[,])points.Clone();
            this.bbox = bbox;
            this.center = new Point((int)(bbox.X + bbox.Width / 2), (int)(bbox.Y + bbox.Height / 2));
        }
    }

    public class MotPoint
    {
        public List<List<KeyPoints>> mot_point;
        public bool flag_predict=false;
        public List<int> predict_id = new List<int>();

        public MotPoint() 
        {
            mot_point = new List<List<KeyPoints>>();
        }
        public bool add_point(KeyPoints point) 
        {
            bool flag = false;
            for (int c = 0; c < mot_point.Count; c++) 
            {
                List<KeyPoints> points = mot_point[c];
                // 添加新的的关键点
                Point center1 = point.center;
                Point center2 = points[points.Count - 1].center;
                if (Math.Sqrt(Math.Pow((center1.X - center2.X), 2.0) + Math.Pow((center1.Y - center2.Y), 2.0)) < 500) 
                {
                    mot_point[c].Add(point);
                    flag = true;
                }
                // 判断是否满足预测条件
                if (mot_point[c].Count >= 50) 
                {
                    flag_predict = true;
                    predict_id.Add(c);
                }
            }
            if (!flag)
            {
                mot_point.Add(new List<KeyPoints>());
                mot_point[mot_point.Count-1].Add(point);
                return false;
            }
            //Console.WriteLine(mot_point.Count);
            return flag_predict;

        }


        public List<List<KeyPoints>> get_points() 
        {
            List <List<KeyPoints>>  points = new List<List <KeyPoints>> ();
            if (flag_predict) 
            {
                for (int l = 0; l < predict_id.Count; l++) 
                {
                    List<KeyPoints> new_points = new List<KeyPoints> (mot_point[predict_id[l]]);
                    points.Add(new_points);
                    Console.WriteLine(mot_point.Count);
                    for (int i = 0; i < 20; i++) 
                    {
                        mot_point[predict_id[l]].RemoveAt(0);
                    }
                    flag_predict = false;
                }
                flag_predict = false;
                predict_id.Clear();
            }
            return points;
        }
    }
}
