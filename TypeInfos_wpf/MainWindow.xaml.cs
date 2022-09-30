using System; 
using System.Collections.Generic;
using System.Linq; 
using System.Windows;  
using Microsoft.Win32;
using System.IO;
using System.Data; 
using Newtonsoft.Json; 

namespace TypeInfos_wpf
{  
    public partial class MainWindow : Window  
    {
        public static string file_TypeInfo;
        public MainWindow()
        {
            InitializeComponent();           
          
        }

        public class input
        {
            public string Tag { get; set; }
            public string Type { get; set; } 
            public string Address { get; set; }
            public input(string tag, string type,  string address)
            {
                Tag = tag;
                Type = type;
                Address = address; 
            }
        }
        public IEnumerable<input> ReadCSV(string fileName)
        { 
            string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
             
            return lines.Select(line =>
            {
                string[] data = line.Split(';'); 
                return new input(data[0], data[1], data[2]);
            });
        }

        private void btnOpeninput_Click(object sender, RoutedEventArgs e)
        {
            //input_view.ItemsSource = ReadCSV(@"C:\Users\Egor\Desktop\тестовое\транснефть\Input.csv");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV-File | *.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                input_view.ItemsSource = ReadCSV(openFileDialog.FileName);
            }

        }

        public void TypeInfos_Click(object sender, RoutedEventArgs e)
        {
            //file_TypeInfo = File.ReadAllText(@"C:\Users\Egor\Desktop\тестовое\транснефть\TypeInfos.json");
             
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON-File | *.json";
            if (openFileDialog.ShowDialog() == true)
            {
                file_TypeInfo = File.ReadAllText(openFileDialog.FileName);
            }
        }
      

        void indexed()
        {
            try
            {
                var obj = input_view.Items[input_view.SelectedIndex];
                var nameOfProperty = "Type";
                var propertyInfo = obj.GetType().GetProperty(nameOfProperty);
                var selected_type = propertyInfo.GetValue(obj, null).ToString();

                nameOfProperty = "Tag";
                propertyInfo = obj.GetType().GetProperty(nameOfProperty);
                var selected_tag = propertyInfo.GetValue(obj, null).ToString();

                TypeInfos_json.Root myDeserializedClass = JsonConvert.DeserializeObject<TypeInfos_json.Root>(file_TypeInfo);

                for (int i = 0; i < myDeserializedClass.TypeInfos.Count(); i++)
                {
                    if (myDeserializedClass.TypeInfos[i].TypeName == selected_type)
                    {
                        string s = myDeserializedClass.TypeInfos[i].Propertys.ToString();
                        object dserialize_object = null;

                        if (selected_type == "AI")
                            dserialize_object = JsonConvert.DeserializeObject<AI_class.Rootobject>(s);
                        if (selected_type == "ZRP")
                            dserialize_object = JsonConvert.DeserializeObject<ZRP_class.Rootobject>(s);
                        if (selected_type == "ZD")
                            dserialize_object = JsonConvert.DeserializeObject<ZD_class.Rootobject>(s);
                        if (selected_type == "SAR")
                            dserialize_object = JsonConvert.DeserializeObject<SAR_class.Rootobject>(s);

                        List<string> type_list = new List<string>();
                        List<int> offset_list = new List<int>();

                        int address = 0;

                        foreach (var prop in dserialize_object.GetType().GetProperties())
                        {
                            var type = prop.GetValue(dserialize_object, null);
                            type_list.Add(selected_tag + "." + prop.Name);
                            offset_list.Add(address);


                            if (type.ToString() == "double")
                            {
                                address += 8;
                            }

                            if (type.ToString() == "int")
                            {
                                address += 4;
                            }

                            if (type.ToString() == "bool")
                            {
                                address += 1;
                            }
                        }

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "XML-File | *.xml";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            toXML.XMLWrite(type_list, offset_list, saveFileDialog.FileName);
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Start_convert_Click(object sender, RoutedEventArgs e)
        {
            
            indexed();
        }

       
    }
}
