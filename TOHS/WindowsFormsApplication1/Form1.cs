using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public string alfabe = "* BbCcDdFfGgHhJjKkLlMmNnPpQqRrSsTtVvWwXxYyZzÇçĞğŞşAaEeIıİiUuÜüOoÖö";
        public string sesliler = "*AaEeIıİiUuÜüOoÖö";
        public string sessizler = "*BbCcDdFfGgHhJjKkLlMmNnPpQqRrSsTtVvWwXxYyZzÇçĞğŞş";
       

        int[] htable= new int[10000] ;
       
        public int wordsize = 0;
        public int syllablesize = 0;
        public int mode = 1;
        public ulong zaman=0;

        public int comp = 0; // Number of comparisons
        public int assign = 0; // Number of assignments
        public int aritmetik = 0; // Number of arithmetic operations
        public int logic = 0; //Number of logic operations

        public Form1()
        {
            InitializeComponent();
            // Constructs hashtable
            for (int i = 0; i < alfabe.Length; i++)
            {
                htable[Convert.ToInt16(alfabe[i])] = i;
            }
        }
               
        public String [] preprocess(String metin)
        {
            String[] kelimeler = new String[200000];
            string metinson=" " ; // Başta en az bir boş karakter var olacak
            metin=metin.ToLower();


            for (int i = 0; i < metin.Length; i++)
                if ((htable[Convert.ToInt16(metin[i])] > 0))
                    metinson = metinson + Char.ToString(metin[i]);
                else if (metin[i] != '\'')
                {
                    metinson = metinson + " ";
                }

            metinson = metinson + " "; // en sonda en az bir boş karakter olacak 
            int bas=-1, son=-1,kelimesay=0;
            for (int i = 1; i < metinson.Length; i++)
            {
                if ((metinson[i-1] == ' ')&(metinson[i] != ' '))
                {
                    bas = i;
                }
                else if ((metinson[i - 1] != ' ') & (metinson[i] == ' '))
                {
                    son = i - 1;
                }

                if ((bas != -1) & (son != -1))
                {
                    kelimeler[kelimesay] = metinson.Substring(bas, son - bas + 1);
                    kelimesay++;
                    bas = -1;
                    son = -1;
                }
            }

                return kelimeler;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            String t;
            double fark;
            DateTime   startTime;
            TimeSpan elapsed;
            

            String [] str;
            String hecelistr = "";
            syllablesize = 0;


            comp = 0;
            assign = 0;
            aritmetik = 0;
            logic = 0; 

            str = preprocess(richTextBox1.Text);

            int say1 = 0;

            startTime = DateTime.Now;

            if (("Modes" == comboBox1.Text) || ("Normal" == comboBox1.Text))
            {


               while (null != str[say1])
               {
                   hecelistr = hecelistr + " " + hecele(str[say1]);
                   say1++;
               }
           
            }
            else if  ("Lines" == comboBox1.Text)
            {
                
                while (null != str[say1])
                {
                    hecelistr = hecelistr + hecele(str[say1]) + "\n";
                    say1++;
                }
               
            }
            else if ("LinesNo" == comboBox1.Text)
            {
                int say2=1;
                
                while (null != str[say1])
                {
                    
                    hecelistr = hecelistr + say2.ToString()+ " " +hecele(str[say1]) + "\n";
                    say1++; say2++;
                }
                
            }
            else if ("WordS" == comboBox1.Text)
            {
                int say2 = 1;
                
                while (null != str[say1])
                {

                    hecelistr = hecelistr + say2.ToString() + " " + str[say1] + " " + hecele(str[say1]) + "\n";
                    say1++; say2++;
                }
                
            }
            elapsed = DateTime.Now - startTime;
            wordsize = say1; // Toplam kelime sayısı

            Word.Text ="Word: "+ wordsize.ToString();
            Syllable.Text = "Syllable: " + syllablesize.ToString();

           
                fark = (ulong) ((1000 * wordsize) / (elapsed.TotalMilliseconds)); // difference of time durations in milliseconds
                t = fark.ToString();
                if (fark>0)
                  WS.Text = "WS: " + t;
                else
                  WS.Text = "WS: ";

                fark = (ulong)((1000 * syllablesize) / (elapsed.TotalMilliseconds)); // difference of time durations in milliseconds 
                t = fark.ToString();
                
                if (fark > 0)
                    SS.Text = "SS: " + t;
                else
                    SS.Text = "SS: ";
           

            richTextBox2.Text = hecelistr;
            label5.Text = assign.ToString();
            label6.Text = comp.ToString();
            label7.Text = aritmetik.ToString();
            label8.Text = logic.ToString();
            label10.Text = elapsed.TotalSeconds.ToString();
          
        }

   
        public string hecele(string kelime)
        {
           int hecesay, keliuzun, say, v1, v2, bas, son, sessizsay, indis;
           int [] hecelerindis = new int[50];
          
            string heceler;

            hecesay = 0;
            v1 = 0;
            v2 = 0;
            keliuzun=kelime.Length;
            hecelerindis[hecesay]=-1;
            assign += 2;
            heceler=String.Empty;

            // find first vowel

            say=0;
           
            while (say < keliuzun)
            {
                comp++;
                if (htable[Convert.ToInt16(kelime[say])] > 49)  
                {
                    comp++;
                    v1 = say;
                    assign++;
                    break;
                }
                say++;
                aritmetik++;
            }

            bas = v1 + 1;
            aritmetik++;
            son = keliuzun;
            assign++;

            
            for (indis = bas; indis < son; indis++)
            {
                if (htable[Convert.ToInt16(kelime[indis])] < 50)  // This finds vowels
                {
                    comp++;
                    continue;
                }
                else
                {
                    v2 = indis;
                    assign++;
                    sessizsay = v2 - v1 - 1;
                    aritmetik++;
                   if (sessizsay == 0)
                    {
                        comp++;
                        hecesay++;
                        aritmetik++;
                        hecelerindis[hecesay] = v2 - 1;
                        aritmetik++;
                        heceler = heceler + kelime.Substring(1 + hecelerindis[hecesay - 1], 1 + hecelerindis[hecesay] - (1 + hecelerindis[hecesay - 1])) + "-";
                        v1 = v2;
                        assign++;
                    }
                    else
                    {
                       
                            hecesay++;
                            aritmetik++;
                            hecelerindis[hecesay] = v2 - 2;
                            aritmetik++;
                            heceler = heceler + kelime.Substring(1 + hecelerindis[hecesay - 1], 1 + hecelerindis[hecesay] - (1 + hecelerindis[hecesay - 1])) + "-";
                            v1 = v2;
                            assign++;
                        
                    }
                }
             }
             
             hecesay=hecesay+1;
             aritmetik++;
             hecelerindis[hecesay]=keliuzun-1;
             aritmetik++;
             heceler=heceler + kelime.Substring(1+hecelerindis[hecesay-1],1+hecelerindis[hecesay]-(1+hecelerindis[hecesay-1]));
             syllablesize =syllablesize + hecesay;
            return heceler;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
    
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult diaresult;
            diaresult =saveFileDialog1.ShowDialog();
            if (1 == diaresult.GetHashCode())
            {
                System.IO.Stream f1;
                f1 = saveFileDialog1.OpenFile();
                richTextBox2.SaveFile(f1, RichTextBoxStreamType.PlainText);
                f1.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            
            System.IO.Stream f1 ;
            f1 = openFileDialog1.OpenFile();
            richTextBox1.LoadFile(f1, RichTextBoxStreamType.PlainText);
            f1.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void WS_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

   


  
    }
}
