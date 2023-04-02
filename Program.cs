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
        static string[] feeling = { "How are you today?", "How would you describe your current emotional state?", "How are you feeling today?" };
        static string[] meds = { "Have you taken your meds?", "Did you remember to take your medication today?", "Have you taken your medication yet today, darling?"};
        static string[] drink = { "Have you drank at least 0.5 liters today?", "Have you drank enough water today?", "Have you gotten enough water?" };
        static string[] anyFood = { "Have you eaten something filling?", "Have you had a satisfying meal today?", "Have you eaten a meal that made you feel full?" };
        static string[] goodFood = { "Have you eaten something nutritious?", "Did you eat any healthy foods today?", "Have you eaten a meal or snack that has good nutritional value?" };
        static string[] drugs = { "How long ago did you drink alcohol or take drugs?", "When was the last time you consumed alcohol or drugs?", "How long ago it last was when you took drugs or alcohol?" };
        static string[] sleep = { "When did you go to sleep yesterday?", "What time did you go to bed yesterday?", "What time did you go to sleep last night?" };
        static string[] sleepTime = { "How long did you sleep for?", "How many hours did you sleep last night?", "How many hours did you sleep for?" };
        static string[] sun = { "Have you spent at least 20 min outside today?", "Have you spent any time outdoors today?", "Have you gotten any fresh air today?" };

        static string[][] question =
        {
            feeling,
            meds,
            drink,
            anyFood,
            goodFood,
            drugs,
            sleep,
            sleepTime,
            sun
        };
        //true is text
        //flase is yes no
        static bool[] answerType = { true, false, false, false, false, true, true, true, false};
        

        static bool firstTime = true;
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += OnExit;



            firstTime = GetFirstTimeOpened();
            if(firstTime)
            {
                //SleepUntill(22);
                MessageBox.Show("Hello Jack. This is JackCare, a program that is unclosable and unremovable. " +
                    "It will monitor your health through a multitude of questions that will be asked daily. " +
                    "This is for the betterment of your own health and as such it is important that you answer truthfully. " +
                    "\n I love you so very much and this is my nerdy way of helping. <3" +
                    "\n Your first set of questions will come tomorrow.");
            }
            List<string> questions =new List<string>();
            List<string> answers = new List<string>();
            while(true)
            {
                SleepUntill(17);
                SystemSounds.Exclamation.Play();
                BoolInputForm BinputForm;
                TextInputForm TinputForm;
                Random rnd = new Random();
                #region Forms
                for (int i = 0; i < question.GetLength(0); i++)
                {
                    string q = question[i] [rnd.Next(question.GetLength(1))];
                    if (answerType[i])
                    {
                        TinputForm = new TextInputForm(q,250);

                        answers.Add(TinputForm.GetAnswer());
                    }
                    else
                    {
                        BinputForm = new BoolInputForm(q);

                        answers.Add(BinputForm.GetAnswer());
                    }
                    questions.Add(q = question[i][0]);
                }
                #endregion
                WriteFile(questions.ToArray(), answers.ToArray());
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

        public static void SleepUntill(int hour, int min = 0, int second = 0)
        {
            DateTime now = DateTime.Now;

            // Calculate the time until 9:00 AM
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, hour, min, second);
            if (now > targetTime)
            {
                targetTime = targetTime.AddDays(1);
            }
            TimeSpan timeUntilTarget = targetTime - now;

            // Sleep the thread until the target time
            Thread.Sleep(timeUntilTarget);
        }


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
            DialogResult = DialogResult.OK;
        }

        public string GetAnswer()
        {
            ShowDialog();
            
            return TextBox.Text;
            
            
        }

    }

}
