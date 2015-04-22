using System;
using System.Collections.Generic;
using System.Text;
using System.Data;



namespace StatAnalysis
{
    /// 备注：下面的计算，均涉及要检验字段是否是数值型的，否则返回false 和空数组）
    public class StatBasic
    {
        /// <summary>
        /// 计算给定的数据表的列（字段）平均值

        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="CalFields">要计算平均值的字段组</param>
        /// <param name="Means">返回对应字段的平均值数组</param>
        public static bool getMean(DataTable mTable, string[] CalFields, ref float[] Means)
        {
			//自己编写
            //依次调用calFieldMean函数
            int len = 0;
            len = getCalFieldsNum(CalFields);  //计算需要计算平均值的字段个数len
            string[] CalResult = new string[len];
	        for(int i=0;i<len;i++)
			{
                if (IsTableColNumeric(mTable, CalFields[i]) == false)             //倘若字段第一次出现false，则后边就算有数值型字段也不会继续计算了,直接返回false
                {
                    return false;
                }
                else Means[i] = calFieldMean(mTable, CalFields[i]);
                
			}
            return true;
        }

        /// <summary>
        /// 计算给定的数据表的列（字段）标准差
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="CalFields">要计算标准差的字段组</param>
        /// <param name="Stdevs">返回对应字段的标准差数组</param>
        /// <returns></returns>
        public static bool getStandardDev(DataTable mTable, string[] CalFields, ref float[] Stdevs)
        {
            //自己编写
            //依次调用calFieldSTDev函数
            int len = 0;                                //计算需要计算标准差的字段个数
            len = getCalFieldsNum(CalFields);
            string[] CalResult = new string[len];
            for (int i = 0; i < len; i++)
            {
                if (IsTableColNumeric(mTable, CalFields[i]) == false)            //倘若字段第一次出现false，则后边就算有数值型字段也不会继续计算了
                    return false;
                else
                    Stdevs[i] = calFieldSTDev(mTable, CalFields[i]);  
            }
            return true;
        }

        /// <summary>
        /// 计算给定的数据表的列元素中的最小值
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="CalFields">要计算最小值的字段组</param>
        /// <param name="MinVal">返回的最小值</param>
        /// <returns></returns>
        public static bool getMinValue(DataTable mTable, string[] CalFields, ref float[] MinVal)
        {
            //自己编写
            //依次调用calFieldMin函数
            int len = 0;                           
            len = getCalFieldsNum(CalFields);                //计算需要计算最小值的字段个数
            //计算需要计算标准差的字段个数
            string[] CalResult = new string[len];
            for (int i = 0; i < len; i++)
            {
                if (IsTableColNumeric(mTable, CalFields[i]) == false)         //倘若字段第一次出现false，则后边就算有数值型字段也不会继续计算了
                    return false;
                else
                    MinVal[i] = calFieldMin(mTable, CalFields[i]);
            }
            return true;
        }

        /// <summary>
        /// 计算给定的数据表的列元素中的最大值
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="CalFields">>要计算最大值的字段组</param>
        /// <param name="MaxVal"></param>
        /// <returns></returns>
        public static bool getMaxValue(DataTable mTable, string[] CalFields, ref float[] MaxVal)
        {
            //自己编写
            //依次调用calFieldMax函数
            int len = 0;
            len = getCalFieldsNum(CalFields);//计算需要计算最大值的字段个数
            string[] CalResult = new string[len];
            for (int i = 0; i < len; i++)
            {
                if (IsTableColNumeric(mTable, CalFields[i]) == false)    //倘若字段第一次出现false，则后边就算有数值型字段也不会继续计算了
                    return false;
                else
                {
                    MaxVal[i] = calFieldMax(mTable, CalFields[i]);
                }
            }
            return true;
        }

        /// <summary>
        /// 计算给定的数据表中指定两列数据的相关系数
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="CalFields">要计算的字段名数组（只取前2个）</param>
        /// <param name="CoeffVal">返回的相关系数</param>
        /// <returns></returns>
        public static bool getCoefficients(DataTable mTable, string[] CalFields, ref float CoeffVal)
        {
            float[] avg = new float[2];
			float[] dev = new float[2];
			float[] cov = new float[2];
			float c = 0f,sum = 0f;
            int len = 0;               //计算需要计算最大值的字段个数
            len = getCalFieldsNum(CalFields);
			DataView dataview = mTable.DefaultView;
			DataTable dt1 = dataview.ToTable(false,CalFields[0]);
			DataTable dt2 = dataview.ToTable(false,CalFields[1]);
			int[] Row = new int[2];
			Row[0] = dt1.Rows.Count;
			Row[1] = dt2.Rows.Count;
			if(Row[0]!=Row[1])
				return false;
			else
			{
                if(((IsTableColNumeric(mTable,CalFields[0])==true) && ((IsTableColNumeric(mTable,CalFields[1])==true))))
                {
				    for(int j=0;j<2;j++)
				    {
					    for(int i=0;i<Row[j];i++)
					    {
						    string s = mTable.Rows[i][CalFields[j]].ToString();
						    if(float.TryParse(s,out c)==false)
							    return false;
						    else continue;
					    }
					    avg[j] = calFieldMean(mTable,CalFields[j]);
                        dev[j] = calFieldSTDev(mTable, CalFields[j]) * (float)Math.Sqrt(Row[j]);				
				    }
                    string s1 = CalFields[0];
                    string s2 = CalFields[1];
				    for(int k=0;k<Row[0];k++)
				    {
                        float m,n;
                        m = float.Parse(dt1.Rows[k][s1].ToString());
                        n = float.Parse(dt2.Rows[k][s2].ToString());
                       sum += (m - avg[0]) * (n- avg[1]);
				    }
				    CoeffVal = sum/(dev[0]*dev[1]);
                    return true;
                }
                else return false;
			}

        }

        //计算需要计算最大值的字段个数
        public static int getCalFieldsNum(string[] CalFields)
        {
            int len=0; 
            List<string> listString = new List<string>();
            foreach (string eachString in CalFields) 
            { 
                if (!listString.Contains(eachString))
                    len += 1;                                     
            }
            return len;
        }

        //函数结果输出
        public static void printResult(int i,ref float[] result, bool result_out)
        {

            if (result_out == true)
            {
                foreach (float j in result)
                    Console.Write(j + ", ");
            }
            else
                Console.Write(result_out);
        }

        

        //主函数
        class Program
        {
            static void Main(string[] args)
            {
                DataTable T = new DataTable();
                T.Columns.Add("Store_Name", typeof(System.String));
                T.Columns.Add("Store_Assist", typeof(System.String));
                T.Columns.Add("Total_Sale", typeof(System.String));
                T.Columns.Add("Assist_Name", typeof(System.String));
                T.Columns.Add("Assist_Gender", typeof(System.String));
                T.Columns.Add("Assist_Age", typeof(System.String));
                DataRow dr = T.NewRow();
                DataRow dr2 = T.NewRow();
                DataRow dr3 = T.NewRow();
                DataRow dr4 = T.NewRow();
                DataRow dr5 = T.NewRow();
                DataRow dr6 = T.NewRow();
                dr[0] = "餐饮部"; 　dr[1] = "店1"; dr[2] = 200.0; dr[3] = "Ann"; dr[4] = "1"; dr[5] = 21;    //假设性别中0为男，1为女
                dr2[0] = "人事部"; dr2[1] = "店2"; dr2[2]= 300.0;dr2[3] = "Bill";dr2[4] = "0";dr2[5] = 25;
                dr3[0] = "后勤部"; dr3[1] = "店3"; dr3[2]= 100.0;dr3[3] = "Jane";dr3[4] = "1";dr3[5] = 24;
                dr4[0] = "餐饮部"; dr4[1] = "店2"; dr4[2]= 150;dr4[3]= "Joe"; dr4[4] = "0"; dr4[5] = 45;
                dr5[0] = "人事部"; dr5[1] = "店4"; dr5[2]= 600; dr5[3] = "Kate"; dr5[4] = "1"; dr5[5] = "";
                dr6[0] = "人事部"; dr6[1] = "店4"; dr6[2] =150; dr6[3] = "Kate"; dr6[4] = "1"; dr6[5] = 15;
                T.Rows.Add(dr);
                T.Rows.Add(dr2);
                T.Rows.Add(dr3);
                T.Rows.Add(dr4);
                T.Rows.Add(dr5);
                T.Rows.Add(dr6);
                string[] str = { "Total_Sale", "Assist_Age"};
                int i = str.Length;
                float CoeffVal = 0;
                float[] avg = new float[i], stdev = new float[i], max = new float[i], min = new float[i],coeff = new float[i];
                string[] result_avg = new string[i], result_std = new string[i], result_max = new string[i], result_min = new string[i],result_coeff = new string[i];
                bool a = getMean(T, str, ref avg);
                bool b = getStandardDev(T, str, ref stdev);
                bool c = getMaxValue(T, str, ref max);
                bool d = getMinValue(T, str, ref min);
                bool e = getCoefficients(T, str,ref CoeffVal);
                //string a = b.ToString();
                Console.Write("Statistics: Average    Stdev   Max    Min   Coefficient\n ");
                Console.Write("Statement: " + a + "       " + b + "   "+c+"   "+d+"    "+e);                           /*需要求均植的字段先判断是否能求均值，若能求，则返回True并且非空均值数组;
                                                                 若为False，则返回的数组中不能求均值的字段均值为NULL*/
                { //均值+标准差+最大+最小值测试
                    Console.Write("\n");
                    Console.Write("\nMeasure: ");
                    foreach (string k in str)
                        Console.Write(k + ", ");              //输出需要求均值的字段
                    Console.Write("\n");
                    Console.Write("\nAverage: ");
                    printResult(i,ref avg,a);
                    Console.Write("\nStDev: ");
                    printResult(i,ref stdev,b);
                    Console.Write("\nMax: ");
                    printResult(i,ref max,c);
                    Console.Write("\nMin: ");
                    printResult(i,ref min,d);
                    Console.Write("\nCoefficient: ");
                    if (e == true) Console.Write(CoeffVal);
                    else Console.Write(false);
                    
                } //均值+标准差+最大+最小值测试
                    Console.Read();
            }
        }

        #region 内部引用的函数
        /// <summary>
        ///计算数据表中指定的列的平均值
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="FieldName">指定的列</param>
        /// <returns>该列的平均值</returns>
        private static float calFieldMean(DataTable mTable, string FieldName)
        {
            float ColMean = 0f;
            //自己编写
            DataView dataView = mTable.DefaultView;
            DataTable dt = dataView.ToTable(false, FieldName);
            int RowNum = dt.Rows.Count;
            int CountNotNum = 0;
            float Sum = 0f, c = 0f;

                for (int i = 0; i < RowNum; i++)
                {
                    string s = dt.Rows[i][FieldName].ToString();
                     if(float.TryParse(s, out c)==false)//判断传入的字段是否能转换为float型（包括空值）
                    {
                        CountNotNum++;
                    }
                    else
                    {
                        Sum += c;
                    }
                }
                int TrueValueNum = RowNum - CountNotNum; //计算非空数值个数
                ColMean = Sum / TrueValueNum;
                return ColMean;
        }

        /// <summary>
        /// 计算数据表中指定的列的标准差
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="FieldName">指定的列</param>
        /// <returns>该列的标准差</returns>
        private static float calFieldSTDev(DataTable mTable, string FieldName)
        {
            float ColSTDev = 0f;

            //自己编写
			float Sum = 0f,c = 0f;
			float mean = calFieldMean(mTable,FieldName);
            DataView dataView = mTable.DefaultView;
            DataTable data = dataView.ToTable(false, FieldName);
			int Row = data.Rows.Count;
			int NotNum = 0;
			for(int i=0;i<Row;i++)
			{
				string s = data.Rows[i][FieldName].ToString();
				
				if(float.TryParse(s, out c)==false)
				{
					NotNum++;
				}
				else
				{				
					Sum+=(c-mean)*(c-mean);
				}
			}
			int IsNum = Row - NotNum;
            ColSTDev = (float)Math.Sqrt(Sum / IsNum);
            //自己编写
            return ColSTDev;
        }




        /// <summary>
        /// 计算数据表中指定的列的最小值
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="FieldName">指定的列</param>
        /// <returns>该列的最小值</returns>
        private static float calFieldMin(DataTable mTable, string FieldName)
        {
            float ColMin = 0f;
            float c = 0f,d = 0f;
			DataView dataview = mTable.DefaultView;
			DataTable dt = dataview.ToTable(true,FieldName);
			int RowNum = dt.Rows.Count;
            for(int j=0;j<=RowNum-1;j++)
				for(int i=RowNum-1;i>=j;i--)
				{					
					string a = dt.Rows[j][FieldName].ToString();//将选定字段的第i行元素转换为字符串
                    string b = dt.Rows[i][FieldName].ToString();
                    if (float.TryParse(a, out c)==false) continue;
					else
					{
						if(float.TryParse(b, out d)==false) continue;
						else
						{
							object s = dt.Rows[j][FieldName];
							if(c<d)
							{
								dt.Rows[j][FieldName] = dt.Rows[i][FieldName]; //找出最小值
								dt.Rows[i][FieldName] = s;
							}						
							else continue;								
						}					
					}
				}
				for(int i=0;i<RowNum;i++)
				{
					float k = 0f;
					if(float.TryParse(dt.Rows[i][FieldName].ToString(),out k)==false)
						continue;
					else 
					{
						 ColMin = k;
					}
                }
            //自己编写
            return ColMin;

        }

        /// <summary>
        /// 计算数据表中指定的列的最大值
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="FieldName">指定的列</param>
        /// <returns>该列的最大值</returns>
        private static float calFieldMax(DataTable mTable, string FieldName)
        {
            float ColMax = 0f;
            //自己编写
            float c = 0f, d = 0f;        
			DataView dataview = mTable.DefaultView;
			DataTable dt = dataview.ToTable(true,FieldName);
			int RowNum = dt.Rows.Count;
            for (int j = 0; j <= RowNum-1; j++)
                for (int i = RowNum-1; i>j; i--)
              {               
                string a = dt.Rows[j][FieldName].ToString();//将选定字段的第i行元素转换为字符串
                string b = dt.Rows[i][FieldName].ToString();
                if (float.TryParse(a, out c) == false) continue;
                else
                {
                    if (float.TryParse(b, out d) == false) continue;
                    else
                    {
                        object s = dt.Rows[j][FieldName];
                        if (c > d)
                        {
                            dt.Rows[j][FieldName] = dt.Rows[i][FieldName]; //找出最大值
                            dt.Rows[i][FieldName] = s;
                        }
                        else continue;
                    }
                }
             }
			for(int i=0;i<RowNum;i++)
			{
				float k = 0f;
				if(float.TryParse(dt.Rows[i][FieldName].ToString(),out k)==false)
					continue;
				else 
				{
					ColMax = k;
				}
			}//遍历dt
            return ColMax;
        }

        /// <summary>
        /// 检验给定数据表中的指定的列是否是数值型的
        /// </summary>
        /// <param name="mTable">传入的数据表</param>
        /// <param name="FieldName">指定的列名称</param>
        /// <returns>是或否</returns>
        private static bool IsTableColNumeric(DataTable mTable, string FieldName)
        {
            bool RetVal = true;
            DataView dataView = mTable.DefaultView;
            DataTable dtDistinct = dataView.ToTable(true, FieldName);	//取mTable表中FieldName字段distinct去重值
            int CountIsNum = 0, CountNotNum = 0;
            int RowNum = mTable.Rows.Count;
            int RowNum2 = dtDistinct.Rows.Count; //计算mTable表中FieldName字段的去重数值个数
                for (int i = 0; i < RowNum2; i++)
                {
                    float c = 0f;
                    string s = dtDistinct.Rows[i][FieldName].ToString();
                    if (RetVal = float.TryParse(s, out c))  //若能够转换为数值型，数值型元素个数+1
                        ++CountIsNum;
                    else
                        ++CountNotNum;                 //若不能够转换为数值型，非数值型元素个数+1
                }
                if (CountIsNum >  CountNotNum)       //若数值型大于非数值型元素个数的2倍，视为可转换为数值型（一定是2?）
                {
                    if (RowNum2 >= RowNum)      //分类最少是2种分类，除以3则无法排除2分法
                        return true;
                    else
                    {
                        if (RowNum2 < 3) return false; //目前规定分类最多分3层。
                        else return true;
                    }   
                }
                else
                    return false;
        }

            /*此方法可以排除不可算均值的字段如city等，以及分法（0/1）、
            多层次的字段（即类型为数值型但是不能算均值）	
            要确定判断的字段是否有求均值的意义*/


            //自己编写
        }

        #endregion 内部引用的函数

}


