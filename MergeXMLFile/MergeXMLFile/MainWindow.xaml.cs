using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml;

namespace MergeXMLFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyFile fileInfo;
        public MainWindow()
        {
            InitializeComponent();
            fileInfo = new MyFile();
            resultNodeList = new List<XmlNode>();
            this.DataContext = fileInfo;
        }

        public XmlNodeList GetElementsByTagName(string path, string tagName)
        {
            try
            {
                // read xml file
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                // Get the root Xml element
                XmlElement root = doc.DocumentElement;
                // Get a list of all Name
                XmlNodeList nameList = root.GetElementsByTagName(tagName);
                return nameList;
            }
            catch (XmlException ex)
            {
                MessageBox.Show("Bad news: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// get xml file all children nodes
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public XmlNodeList GetAllChildrenNodes(string path)
        {
            try
            {
                // read xml file
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                //XmlElement root = doc.DocumentElement;
                //string rootNode = doc.ChildNodes[0].OuterXml;
                //rootNode = doc.ChildNodes[1].OuterXml;
                //XmlNodeList nodes = root.ChildNodes;
                XmlNodeList nodes = doc.SelectNodes("//EmulatedBrowser");
                return nodes;
            }
            catch(XmlException ex)
            {
                MessageBox.Show("Bad news: " + ex.Message);
            }
            return null;
        }

        private void File1btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            // when user select file and press OK
            if(fileDialog.ShowDialog()== true)
            {
                // get the file path
                fileInfo.FileName1 = fileDialog.FileName;
                long size = new FileInfo(fileInfo.FileName1).Length;
                if(size > MAX_FILE_SIZE)
                {
                    MessageBox.Show("Uploaded file size must be less than 5 MB", "Maximum file size exceeded",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                fileName1 = fileDialog.FileName;

                //// read file content
                //txtContent1 = File.ReadAllText(fileDialog.FileName);
            }
        }

        private void File2btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            // when user select file and press OK
            if (fileDialog.ShowDialog() == true)
            {
                // get the file path
                fileInfo.FileName2 = fileDialog.FileName;
                long size = new FileInfo(fileInfo.FileName2).Length;
                if (size > MAX_FILE_SIZE)
                {
                    MessageBox.Show("Uploaded file size must be less than 5 MB", "Maximum file size exceeded",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //// read file content
                //txtContent2 = File.ReadAllText(fileDialog.FileName);
                fileName2 = fileDialog.FileName;

            }
        }

        private void MergedFilebtn(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlDocNode = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.AppendChild(xmlDocNode);

            XmlElement LiftsMainNode = xmlDoc.CreateElement("ArrayOfEmulatedBrowser");
            LiftsMainNode.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            LiftsMainNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            foreach (XmlNode node in resultNodeList)
            {
                //necessary for crossing XmlDocument contexts
                XmlNode importNode = LiftsMainNode.OwnerDocument.ImportNode(node, true);
                LiftsMainNode.AppendChild(importNode);
            }

            xmlDoc.AppendChild(LiftsMainNode);

            xmlDoc.Save("C:\\Users\\JJiao\\Desktop\\EmulatedDevices.xml");
        }

        public class MyFile : INotifyPropertyChanged
        {
            private string fileName1;
            public string FileName1
            {
                get
                {
                    return fileName1;
                }
                set
                {
                    if (value != fileName1)
                    {
                        fileName1 = value;
                        OnPropertyChanged("FileName1");
                    }
                }
            }

            private string fileName2;
            public string FileName2
            {
                get
                {
                    return fileName2;
                }
                set
                {
                    if (value != fileName2)
                    {
                        fileName2 = value;
                        OnPropertyChanged("FileName2");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            // Create the OnPropertyChanged method to raise the event
            // The calling member's name will be used as the parameter.
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        //private string txtContent1;
        //private string txtContent2;
        private string fileName1;
        private string fileName2;
        //private string fileName3;
        private List<XmlNode> resultNodeList;
        private const long MAX_FILE_SIZE = 5242880;// 5MB * 1024 * 1024 = 5242880

        private void CompareFilebtn(object sender, RoutedEventArgs e)
        {
            //string tagName = "EmulatedBrowser";
            //XmlNodeList nodeList = GetElementsByTagName(fileName1, tagName);
            resultNodeList.Clear();
            XmlNodeList nodeList1 = GetAllChildrenNodes(fileName1);
            if (nodeList1 == null)
            {
                MessageBox.Show("Current XML File has no child nodes");
                return;
            }
            //XmlNodeList nodeList2 = GetElementsByTagName(fileName2,tagName);
            XmlNodeList nodeList2 = GetAllChildrenNodes(fileName2);
            if (nodeList2 == null)
            {
                MessageBox.Show("Current XML File has no child nodes");
                return;
            }
            // Compare algorithm
            int node1Cnt = nodeList1.Count;
            int node2Cnt = nodeList2.Count;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName1);

            for (int i=0;i< node1Cnt; ++i)
            {
                // add show_by_default tag to node1
                //Create a new element node
                XmlNode newElem = doc.CreateElement("show_by_default");
                newElem.InnerText = "false";
                //necessary for crossing XmlDocument contexts
                XmlNode importNode = nodeList1.Item(i).OwnerDocument.ImportNode(newElem, true);
                nodeList1.Item(i).AppendChild(importNode);

                // Here we want to compare two xml file "Name" tag field
                // iterate nodeList1.Item(i) all child node to find "Name" child
                XmlNodeList nodeList = nodeList1.Item(i).ChildNodes;
                Dictionary<string, string> dic1 = new Dictionary<string, string>();// store nodeList1 all properties
                List<Dictionary<string, string>> dic2 = new List<Dictionary<string, string>>();// store nodeList2 all properties
                foreach (XmlNode it in nodeList)
                {
                    dic1.Add(it.Name, it.InnerText);
                }
                bool flag = false;
                
                for (int j=0;j<node2Cnt;++j)
                {
                    nodeList = nodeList2.Item(j).ChildNodes;
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    foreach (XmlNode it in nodeList)
                    { 
                        temp.Add(it.Name, it.InnerText);
                    }
                    dic2.Add(temp);
                }
                string name = "Name";
                ///dic struct
                // < EmulatedBrowser >
                //  < Name > Amazon Kindle Fire HDX </ Name >
                //  < width > 1600 </ width >
                //  < height > 2560 </ height >
                //  < deviceScaleFactor > 2 </ deviceScaleFactor >
                //  < userAgent > Mozilla / 5.0(Linux; U; en - us; KFAPWI Build/ JDQ39) AppleWebKit / 535.19(KHTML, like Gecko) Silk / 3.13 Safari / 535.19 Silk - Accelerated = true </ userAgent >
                //   < touch > true </ touch >
                //   < mobile > true </ mobile >
                //   < fit > true </ fit >
                // </ EmulatedBrowser >
                foreach (var dic in dic2) // iterate dic2 to confirm whether node1 contains in node2 
                {
                    // when have the same Name. compare other nodes
                    // all the nodes have same value, set flag=true
                    string value;
                    dic.TryGetValue(name, out value);
                    // split value, for example:iPhone 5/SE
                    if (value.Contains("/"))
                    {
                        string[] strArray = value.Split('/');
                        foreach(string s in strArray)
                        {
                            value = s;
                            if (value.Contains(dic1[name]) || dic1[name].Contains(value) == false)
                            {
                                continue;
                            }
                            flag = TwoDictoryIsEquals(dic1, dic, name);
                            if (flag)
                            {
                                // in node2List found the node
                                break;
                            }
                        }
                        continue;
                    }
                    if(value.Contains(dic1[name]) || dic1[name].Contains(value) == false)
                    {
                        continue;
                    }
                    flag = TwoDictoryIsEquals(dic1, dic, name);
                    if(flag)
                    {
                        // in node2List found the node
                        break;
                    }

                }

                if (flag == false)
                {
                    // node2 list had no node list1 node, we should add it to node2 list
                    resultNodeList.Add(nodeList1.Item(i));
                }
            }
            foreach(XmlNode node in nodeList2)
            {
                resultNodeList.Add(node);
            }
        }

        /// <summary>
        /// confirm two Dictionary<string, string> is equals or not
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="name"></param> name field need to be special treated
        /// <returns></returns>
        private bool TwoDictoryIsEquals(Dictionary<string, string> d1, Dictionary<string, string> d2, string name)
        {
            bool flag = false;
            if(d1.Count == d2.Count)// Require equal count
            {
                flag = true;
                foreach(var item in d1) // iterte d1
                {
                    string value;
                    if(d2.TryGetValue(item.Key, out value))
                    {
                        //if (item.Key == name && (value.Contains(d1[name]) || d1[name].Contains(value) == false))
                        //{
                        //    flag = false;
                        //    break;
                        //}
                        if(item.Key == name || item.Key == "show_by_default" || item.Key == "userAgent")
                        {
                            continue;
                        }
                        // if key-value is a number, need to be convert to int to compare. e.g: 8.0 and 8
                        string val = item.Value;
                        if(val == value)
                        {
                            flag = true;
                            continue;
                        }
                        if (!string.IsNullOrEmpty(val) && IsNumber(val))
                        {
                            if (val.Contains(".")) // double
                            {
                                // double
                                double tmp;
                                if (Double.TryParse(val, out tmp))
                                {
                                    val = Convert.ToDouble(tmp).ToString("0.00");//保留小数点后两位
                                }
                            }
                            else
                            {
                                val = Int32.Parse(val).ToString("0.00"); // int to double
                            }
                        }
                        if (!string.IsNullOrEmpty(value) && IsNumber(value))
                        {
                            if (value.Contains("."))
                            {
                                // double
                                double tmp;
                                if (Double.TryParse(value, out tmp))
                                {
                                    value = Convert.ToDouble(tmp).ToString("0.00");//保留小数点后两位
                                }
                            }
                            else
                            {
                                value = Int32.Parse(value).ToString("0.00"); // convert int to double
                            }
                        }
                        if(string.Compare(val,value) != 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        private bool IsNumber(string str)
        {
            // check string a a number
            bool flag = true;
            int count = 0;
            if(str.Length == 0)
            {
                flag = false;
            }
            else
            {
                char[] x = str.ToCharArray();
                for(int i=0; i < str.Length; ++i)
                {
                    if(!char.IsNumber(x[i]) && x[i] != '.')
                    {
                        flag = false;
                        break;
                    }
                    if(x[i] == '.')
                    {
                        count++;
                        if(i == 0 || i == str.Length - 1)
                        {
                            flag = false;
                        }
                    }
                    if(count > 1)
                    {
                        flag = false;
                    }
                    if(!flag)
                    {
                        break;
                    }
                }
            }
            return flag;
        }
    }
}
