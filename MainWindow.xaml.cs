using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOGVaultEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<VaultLine> vaultLines = new List<VaultLine>();
        //public List<SOGItem> sogItemList = new List<SOGItem>();
        SOGItem[] sogItemList = new SOGItem[100];
        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Main Running");
            LoadSOGItemList();
            
        }

        public class ItemCombo
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public override string ToString() { return this.Name; }
            public string getValue() { return this.Value; }
        }
        public class SOGItem
        {
            public string itemID { get; set; }
            public string quality { get; set; }
            public string name { get; set; }
        }

        public class VaultLine
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string E { get; set; }
            public String F { get; set; }
            public string G { get; set; }
            public string H { get; set; }
            public string I { get; set; }
            public string J { get; set; }
            public string K { get; set; }
            public string L { get; set; }
            public string M { get; set; }
            public string N { get; set; }
            public string O { get; set; }
            public string P { get; set; }

        }


        private void GetVault_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button Clicked");

            buildVaultList();
            VaultGridUpdate();



        }

        public void buildVaultList()
        {
            vaultLines.Clear();
            //Find Vault File
            String vaultlocString = VaultLoc.Text;
            FileStream vaultStream = new FileStream(vaultlocString, FileMode.Open);
            
            //Run through the 17 bits of hex
            

            for (int i = 0; i < 18; i++)
            {
                VaultLine vl = new VaultLine();

                //Line Number
                String LineNumber = i.ToString();
                vl.A = LineNumber;

                //Item ID
                vl.B = FixZero(vaultStream.ReadByte().ToString("X"));

                //Item Bonus
                vl.C = FixZero(vaultStream.ReadByte().ToString("X"));

                //Item Quality
                vl.D = FixZero(vaultStream.ReadByte().ToString("X"));

                //Item Quality 256
                vl.E = FixZero(vaultStream.ReadByte().ToString("X"));

                //Item Name Length
                vl.F = FixZero(vaultStream.ReadByte().ToString("X"));

                for (int j = 0; j < 10; j++)
                {
                    //String hexIn = HexToString(FixZero(vaultStream.ReadByte().ToString("X")));
                    String hexIn = FixZero(vaultStream.ReadByte().ToString("X"));

                    switch (j)
                    {
                        case 0: vl.G = hexIn; break;
                        case 1: vl.H = hexIn; break;
                        case 2: vl.I = hexIn; break;
                        case 3: vl.J = hexIn; break;
                        case 4: vl.K = hexIn; break;
                        case 5: vl.L = hexIn; break;
                        case 6: vl.M = hexIn; break;
                        case 7: vl.N = hexIn; break;
                        case 8: vl.O = hexIn; break;
                        case 9: vl.P = hexIn; break;
                        default: Debug.WriteLine("MISSING SOMETHING!!!! :" + j); break;

                    }

                }

                vaultLines.Add(vl);

            }

        }

        public void VaultGridUpdate()
        {
            //Add a Header to the DataGrid - not necessary
            var list = new ObservableCollection<VaultLine>();
            VaultLine header = new VaultLine();
            header.A = "#";
            header.B = "ID";
            header.C = "Bonus";
            header.D = "Quantity";
            header.E = "Quantity256";
            header.F = "Name Len";
            header.G = "Name";
            header.P = "P";
            list.Add(header);

            foreach(var vltLine in vaultLines)
            {
                list.Add((VaultLine)vltLine);
            }

            this.VaultGrid.ItemsSource = list;
        }

        static string HexToString(string hex)
        {
            StringBuilder stringBuilder = new StringBuilder(hex.Length / 2);

            for (int i = 0; i < hex.Length; i += 2)
            {
                //Debug.WriteLine(hex);
                string hexPair = hex.Substring(i, 2);
                byte byteValue = byte.Parse(hexPair, System.Globalization.NumberStyles.HexNumber);
                stringBuilder.Append((char)byteValue);
            }

            return stringBuilder.ToString();
        }

        public String FixZero(String hex) { 
            if(hex.Length == 1)
            {
                String hexFix = "0" + hex;
                return hexFix;
            }
            else
            {
                return hex;
            }
            
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ExportVault_Click(object sender, RoutedEventArgs e)
        {
            //var file = File.Open(VaultDest.Text, FileMode.Create);
            //var bw = new BinaryWriter(file);
            
            var stream = new FileStream(
                VaultDest.Text,
                FileMode.Create,
                FileAccess.ReadWrite
                );
 

            
            string hexString = "";
            foreach (var vl in vaultLines)
            {
                hexString += vl.B + vl.C + vl.D + vl.E + vl.F + vl.G + vl.H + vl.I + vl.J + vl.K + vl.L + vl.M + vl.N + vl.O + vl.P;

            }
            Debug.WriteLine(hexString); 

            WriteHexStringToFile(hexString, stream);
            

            stream.Close();
        }


        private void WriteHexStringToFile(string hexString, FileStream stream)
        {
            var twoCharacterBuffer = new StringBuilder();
            var oneByte = new byte[1];
            foreach (var character in hexString.Where(c => c != ' '))
            {
                twoCharacterBuffer.Append(character);

                if (twoCharacterBuffer.Length == 2)
                {
                    oneByte[0] = (byte)Convert.ToByte(twoCharacterBuffer.ToString(), 16);
                    stream.Write(oneByte, 0, 1);
                    twoCharacterBuffer.Clear();
                }
            }
        }

        private void ChangeVaultItem_Click(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(ItemRow.Text);
            VaultLine vl = new VaultLine();


            //Item ID
            vl.B = Convert.ToByte(ItemID.Text).ToString("x2");

            //Item Bonus
            vl.C = Convert.ToByte(ItemBonus.Text).ToString("x2");

            //Item Quality
            Debug.WriteLine(ItemQuantity.Text);
            vl.D = Convert.ToByte(Int32.Parse(ItemQuantity.Text).ToString("X")).ToString("x2");

            //Item Quality 256
            vl.E = Convert.ToByte(ItemQuantity256.Text).ToString("x2");

            //Item Name Length
            vl.F = Convert.ToByte(ItemNameBuffer.Text).ToString("x2");

            string itemName = ItemName.Text;
            string hexIn = "";

            for (int j = 0; j < 10; j++)
            {
                if (itemName.Length > j)
                {
                    hexIn = Convert.ToByte(itemName[j]).ToString("x2");
                }
                else
                {
                    hexIn = "20";
                }
                switch (j)
                {
                    case 0: vl.G = hexIn; break;
                    case 1: vl.H = hexIn; break;
                    case 2: vl.I = hexIn; break;
                    case 3: vl.J = hexIn; break;
                    case 4: vl.K = hexIn; break;
                    case 5: vl.L = hexIn; break;
                    case 6: vl.M = hexIn; break;
                    case 7: vl.N = hexIn; break;
                    case 8: vl.O = hexIn; break;
                    case 9: vl.P = hexIn; break;
                    default: Debug.WriteLine("MISSING SOMETHING!!!! :" + j); break;

                }

            }

            vaultLines[index] = vl;
            VaultGridUpdate();


        }

        public void LoadSOGItemList() {

            for(int k=0; k<sogItemList.Length; k++)
            {
                SOGItem item = new SOGItem();
                item.itemID = "";
                item.name = "";
                item.quality = "";
                sogItemList[k] = item;  
            }
            string filePath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            filePath = Directory.GetParent(Directory.GetParent(Directory.GetParent(filePath).FullName).FullName).FullName;
            Debug.WriteLine(filePath);
            /*
            foreach (String x in Directory.GetFiles(filePath))
            {
                Debug.WriteLine(x);
            }
            */

            filePath += @"\Data\SwordEdit.ini";
            //Debug.WriteLine(filePath);
            StreamReader sr = new StreamReader(filePath);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Debug.WriteLine(line);  
                String[] linearray = line.Split(new char[] { ',', });
                SOGItem sogItem = new SOGItem();

                sogItem.itemID = linearray[0];
                sogItem.quality = linearray[1];
                sogItem.name = linearray[2];
                /*
                while (sogItem.name.Length < 10)
                {
                    sogItem.name += " ";
                }
                */
                sogItemList[Int32.Parse(sogItem.itemID)] = sogItem;
            }

            for(int i=0; i<sogItemList.Length; i++)
            {
    

                if (sogItemList[i].itemID != "")
                {
                    ItemComboBox.Items.Add(new ItemCombo { Name = sogItemList[i].name, Value = sogItemList[i].itemID });
                }
                


            }
            
            
        }

        private void ItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //string text = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            //Debug.WriteLine(ItemComboBox.Text);
        }

        private void ItemComboBox_DropDownClosed(object sender, EventArgs e)
        {

            var sItem = ItemComboBox.SelectedItem as ItemCombo;
            Debug.WriteLine(sItem.Value);

            var item = sogItemList[Int32.Parse(sItem.Value)];
            ItemID.Text = item.itemID;
            ItemBonus.Text = "0";
            ItemQuantity.Text = item.quality;
            ItemQuantity256.Text = "0";
            ItemNameBuffer.Text = "10";
            ItemName.Text = item.name;
        }



    }
}