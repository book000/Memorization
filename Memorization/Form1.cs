using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using VoiceTextWebAPI.Client;

namespace Memorization
{
    public partial class Form1 : Form
    {
        private VoiceTextClient client;
        private Speaker speaker;

        public Form1()
        {
            InitializeComponent();

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            Settings appSettings = new Settings();

            if (!File.Exists("settings.xml")) {
                // new
                StreamWriter sw = new StreamWriter("settings.xml", false, new UTF8Encoding(false));
                serializer.Serialize(sw, appSettings);
                sw.Close();
            }
            StreamReader sr = new StreamReader("settings.xml", new UTF8Encoding(false));
            appSettings = (Settings)serializer.Deserialize(sr);
            sr.Close();

            if (appSettings.Apikey == "ENTER YOUR APIKEY HERE")
            {
                MessageBox.Show("settings.xmlでApikeyを設定してください。",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                Close();
                return;
            }

            if (appSettings.Speaker == "show")
            {
                speaker = Speaker.Show;
            }
            else if (appSettings.Speaker == "haruka")
            {
                speaker = Speaker.Haruka;
            } else if (appSettings.Speaker == "hikari")
            {
                speaker = Speaker.Hikari;
            } else if (appSettings.Speaker == "takeru")
            {
                speaker = Speaker.Takeru;
            } else if (appSettings.Speaker == "santa")
            {
                speaker = Speaker.Santa;
            } else if (appSettings.Speaker == "bear")
            {
                speaker = Speaker.Bear;
            }
            client = new VoiceTextClient
            {
                APIKey = appSettings.Apikey,
                Speaker = speaker,
                Volume = appSettings.Volume,
                Speed = appSettings.Speed,
                Pitch = appSettings.Pitch,
                Format = Format.MP3
            };

            if (!Directory.Exists(".\\output\\"))
            {
                Directory.CreateDirectory(".\\output\\");
            }

            label2.Text = "Setting\n" +
                "Speaker: " + appSettings.Speaker + "\n" +
                "Volume: " + appSettings.Volume + "\n" +
                "Speed: " + appSettings.Speed + "\n" +
                "Pitch: " + appSettings.Pitch;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            progressBar1.Value = 0;

            bool res = await Task.Run(() => Work());

            if (res)
            {
                MessageBox.Show("1つ以上のエラーが発生したため、失敗しました。",
                    "Result",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                int max = progressBar1.Maximum;
                label3.Text = max + " / " + max;
                progressBar1.Value = max;
            }

            button1.Enabled = true;
        }

        delegate void Delegate(int value);

        void setMax(int value) {
            progressBar1.Maximum = value;

            label3.Text = progressBar1.Value + " / " + progressBar1.Maximum;
        }

        void setValue(int value)
        {
            progressBar1.Value = value;

            label3.Text = progressBar1.Value + " / " + progressBar1.Maximum;
        }

        private Boolean Work(){
            string[] lines = textBox1.Text.Trim('\r', '\n').Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Invoke(new Delegate(setMax), lines.Length);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line == "") continue;

                string[] path_text = line.Split('\t');

                if(path_text.Length != 2)
                {
                    MessageBox.Show((i + 1) + "行目を解析できません。",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                
                string path = path_text[0];
                string text = path_text[1];

                var bytes = client.GetVoiceAsync(text);
                try
                {
                    File.WriteAllBytes(".\\output\\" + path + ".mp3", bytes.Result);
                }
                catch (AggregateException e)
                {
                    if (e.Message == "unauthorized")
                    {
                        MessageBox.Show("指定されたAPIキーで認証できません。",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }
                    MessageBox.Show("VoiceText Web APIからエラーを返却されました。\n" + e.InnerException.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    return false;
                }
                Invoke(new Delegate(setValue), i + 1);
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            Settings appSettings = new Settings();

            StreamReader sr = new StreamReader("settings.xml", new UTF8Encoding(false));
            appSettings = (Settings)serializer.Deserialize(sr);
            sr.Close();

            if (appSettings.Apikey == "ENTER YOUR APIKEY HERE")
            {
                MessageBox.Show("settings.xmlでApikeyを設定してください。",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                Close();
                return;
            }

            if (appSettings.Speaker == "show")
            {
                speaker = Speaker.Show;
            }
            else if (appSettings.Speaker == "haruka")
            {
                speaker = Speaker.Haruka;
            }
            else if (appSettings.Speaker == "hikari")
            {
                speaker = Speaker.Hikari;
            }
            else if (appSettings.Speaker == "takeru")
            {
                speaker = Speaker.Takeru;
            }
            else if (appSettings.Speaker == "santa")
            {
                speaker = Speaker.Santa;
            }
            else if (appSettings.Speaker == "bear")
            {
                speaker = Speaker.Bear;
            }
            client = new VoiceTextClient
            {
                APIKey = appSettings.Apikey,
                Speaker = speaker,
                Volume = appSettings.Volume,
                Speed = appSettings.Speed,
                Pitch = appSettings.Pitch,
                Format = Format.MP3
            };

            if (!Directory.Exists(".\\output\\"))
            {
                Directory.CreateDirectory(".\\output\\");
            }

            label2.Text = "Setting\n" +
                "Speaker: " + appSettings.Speaker + "\n" +
                "Volume: " + appSettings.Volume + "\n" +
                "Speed: " + appSettings.Speed + "\n" +
                "Pitch: " + appSettings.Pitch;
        }
    }
}
