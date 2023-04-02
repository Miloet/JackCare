using System;
using System.IO;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace JackCare
{
    internal static class Program
    {
        string[] feeling = {"","" };

        static string[][] question =
        {
            "How are you today?",
            "Have you taken your meds?",
            "Have you drank atleast 0.5 liters?",
            "Have you ate something filling?",
            "Have you ate something nutritious?",
        };
        

        static bool firstTime = true;
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += OnExit;



            firstTime = GetFirstTimeOpened();
            if(firstTime)
            {
                Thread.Sleep(1000);
            }
            List<string> questions =new List<string>();
            List<string> answers = new List<string>();
            while(true)
            {
                SystemSounds.Exclamation.Play();

                #region Forms

                BoolInputForm inputForm = new BoolInputForm();
                 
                answers.Add(inputForm.GetName());

                #endregion
                WriteFile(questions.ToArray(), answers.ToArray());
                Thread.Sleep(1000);

                firstTime = false;
            }

            AppDomain.CurrentDomain.ProcessExit -= OnExit;
        }
        
        #region Save and exit functions
        private static void OnExit(object sender, EventArgs e)
        {
            string filePath = @"C:\JackCare\Save.txt";
            
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(firstTime.ToString());
            }
            MessageBox.Show("I love you", "I love you", MessageBoxButtons.OK);
        }
        public static bool GetFirstTimeOpened()
        {
            string filePath = @"C:\JackCare\Save.txt";
            // Read the bool value from the file
            using (StreamReader reader = new StreamReader(filePath))
            {
                bool firstTime;
                if(bool.TryParse(reader.ReadToEnd(), out firstTime))
                {
                    return firstTime;
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(firstTime.ToString());
                        return true;
                    }
                }

            }
        }
        #endregion 


        private static void WriteFile(string[] question, string[] answer)
        {
            string text = "";
            for(int i= 0; i < question.Length; i++)
            {
                string t = question[i] + "\n" + answer[i] + "\n\n";
                text += t;
            }


            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderPath = Path.Combine(documentsPath, "JackCare");
            Directory.CreateDirectory(folderPath);

            DateTime dateTime = DateTime.UtcNow.Date;
            string name = dateTime.ToString("dd / MM / yy");

            // Create a new text file inside the new folder and write the user's input to it
            string filePath = Path.Combine(folderPath, "JackCare-" + name + ".txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(text);
            }
        }
    }

    class BoolInputForm : Form
    {
        private Label nameLabel;
        private Button yesButton;
        private Button noButton;

        public BoolInputForm(string question)
        {
            // Set up the form
            Text = "JackCare";
            Width = 300;
            Height = 150;

            // Add a label
            nameLabel = new Label();
            nameLabel.Text = question;
            nameLabel.Left = 20;
            nameLabel.Top = 20;
            nameLabel.AutoSize = true;
            Controls.Add(nameLabel);

            // Add a button
            yesButton = new Button();
            yesButton.Text = "Yes";
            yesButton.Left = 50;
            yesButton.Top = 90;
            yesButton.Click += yesButton_Click;
            Controls.Add(yesButton);

            // Add a button
            noButton = new Button();
            noButton.Text = "No";
            noButton.Left = 150;
            noButton.Top = 90;
            noButton.Click += noButton_Click;
            Controls.Add(noButton);
        }
        
        private void yesButton_Click(object sender, EventArgs e)
        {
            // Close the form and return the user's input
            DialogResult = DialogResult.Yes;
        }
        private void noButton_Click(object sender, EventArgs e)
        {
            // Close the form and return the user's input
            DialogResult = DialogResult.No;
        }

        public string GetAnswer()
        {
            if (ShowDialog() == DialogResult.Yes)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }
        
    }


    class TextInputForm : Form
    {
        private Label nameLabel;
        private TextBox TextBox;
        private Button nextButton;
        public TextInputForm(string question, int imputSize)
        {
            // Set up the form
            Text = "JackCare";
            Width = 300;
            Height = 150;

            // Add a label
            nameLabel = new Label();
            nameLabel.Text = question;
            nameLabel.Left = 20;
            nameLabel.Top = 20;
            nameLabel.AutoSize = true;
            Controls.Add(nameLabel);

            // Add a text box
            TextBox = new TextBox();
            TextBox.Left = 20;
            TextBox.Top = 50;
            TextBox.Width = imputSize;
            Controls.Add(TextBox);

            // Add a button
            nextButton = new Button();
            nextButton.Text = "next";
            nextButton.Left = 100;
            nextButton.Top = 90;
            nextButton.Click += nextButton_Click;
            Controls.Add(nextButton);

          
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            // Close the form and return the user's input
            DialogResult = DialogResult.Yes;
        }

    }

}
