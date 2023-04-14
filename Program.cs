using System;
using System.IO;
using System.Media;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace JackCare
{
    internal static class Program
    {
        #region Questions

        static string[] feeling = { "How are you today?", "How would you describe your current emotional state?", "How are you feeling today?" };
        static string[] meds = { "Have you taken your meds?", "Did you remember to take your medication today?", "Have you taken your medication yet today, darling?"};
        static string[] drink = { "Have you drank at least 0.5 liters today?", "Have you drank enough water today?", "Have you gotten enough water?" };
        static string[] anyFood = { "Have you eaten something filling?", "Have you had a satisfying meal today?", "Have you eaten a meal that made you feel full?" };
        static string[] goodFood = { "Have you eaten something nutritious?", "Did you eat any healthy foods today?", "Have you eaten a meal or snack that has good nutritional value?" };
        static string[] drugs = { "How long ago did you drink alcohol or take drugs?", "When was the last time you consumed alcohol or drugs?", "How long ago it last was when you took drugs or alcohol?" };
        static string[] sleep = { "When did you go to sleep yesterday?", "What time did you go to bed yesterday?", "What time did you go to sleep last night?" };
        static string[] sleepTime = { "How long did you sleep for?", "How many hours did you sleep last night?", "How many hours did you sleep for?" };
        static string[] sun = { "Have you spent at least 20 min outside today?", "Have you spent any time outdoors today?", "Have you gotten any fresh air today?" };
        static string[] love = { "Do you know that I love you?", "Have you thought about that I love you today?", "Hey, I love you <3! (yes = I love you too!, no = ...)" };

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
            sun,
            love
        };
        
        //true is text
        //flase is yes no
        static bool[] answerType = { true, false, false, false, false, true, true, true, false, false};
        

        static bool firstTime = true;
        [STAThread]

        #endregion

        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += OnExit;

            firstTime = GetFirstTimeOpened();
            if(firstTime)
            {
                
                SleepUntil(22);
                SendMailUpdate("Jack care has been opened for the first time.");
                firstTime = false;
                SetFirstTimeOpened();
                MessageBox.Show("Hello Jack. This is JackCare, a program that is unclosable and unremovable. " +
                    "It will monitor your health through a multitude of questions that will be asked daily. " +
                    "This is for the betterment of your own health and as such it is important that you answer truthfully. " +
                    "\n I love you so very much and this is my nerdy way of helping. <3" +
                    "\n Your first set of questions will come tomorrow.");
            }
            else
            {
                SendMailUpdate("Jack care has been opened.");
            }
            List<string> questions = new List<string>();
            List<string> answers = new List<string>();
            while(true)
            {
                SleepUntil(17);
                SystemSounds.Exclamation.Play();
                BoolInputForm BinputForm;
                TextInputForm TinputForm;
                Random rnd = new Random();


                #region Forms

                for (int i = 0; i < question.Length; i++)
                {
                    string q = question[i] [rnd.Next(question[i].Length)];
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
                    questions.Add(question[i][0]);
                }
                #endregion


                WriteFile(questions.ToArray(), answers.ToArray());
                

                MessageBox.Show("I love you <3", "I love you", MessageBoxButtons.OK);
            }
            
            AppDomain.CurrentDomain.ProcessExit -= OnExit;
        }
        
        #region Save and exit functions
        private static void OnExit(object sender, EventArgs e)
        {
            MessageBox.Show("I love you", "I love you", MessageBoxButtons.OK);
        }
        public static bool GetFirstTimeOpened()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderPath = Path.Combine(documentsPath, "JackCare");
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, "Save.txt");

            // Read the bool value from the file
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    bool firstTime;
                    if (bool.TryParse(reader.ReadToEnd(), out firstTime))
                    {
                        return firstTime;
                    }
                }
            }

            // If the file doesn't exist or couldn't be parsed, create it and write a default value to it
            bool defaultValue = true;
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(defaultValue.ToString());
            }
            return defaultValue;
        }
        public static void SetFirstTimeOpened()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderPath = Path.Combine(documentsPath, "JackCare");
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, "Save.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(firstTime.ToString());
            }
        }
        #endregion 


        //Sleep Untill makes the program sleep untill desired time and once that time is reached let the program continue.
        //The function checks every min if the desired time has been reached so it can work despite the computer being in
        //sleep mode.
        public static void SleepUntil(int hour, int min = 0, int second = 0)
        {
            DateTime now = DateTime.Now;

            // Calculate the time until the target time
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, hour, min, second);
            if (now > targetTime)
            {
                targetTime = targetTime.AddDays(1);
            }
            TimeSpan timeUntilTarget = targetTime - now;

            // Ask for the desired time once every minute until the target time is reached
            while (now < targetTime)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
                now = DateTime.Now;
            }
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
            string name = dateTime.ToString("dd-MM-yy");

            // Create a new text file inside the new folder and write the user's input to it
            string filePath = Path.Combine(folderPath, "JackCare-" + name + ".txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(text);
            }
            SendMail(filePath);
        }

        public static void SendMail(string filePath)
        {
            // Configure the SMTP client
            SmtpClient client = new SmtpClient("smtp.Outlook.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("jackcare666@gmail.com", "Superdog123");

            // Compose the email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("jackcare666@gmail.com");
            message.To.Add("jackcare666@gmail.com");
            DateTime dateTime = DateTime.UtcNow.Date;
            string name = dateTime.ToString("dd/MM/yy");
            message.Subject = "JackCare-" + name;
            message.Body = "";
            message.Attachments.Add(new Attachment(filePath));

            // Send the email message
            client.Send(message);
        }

        public static void SendMailUpdate(string Update)
        {
            // Configure the SMTP client
            SmtpClient client = new SmtpClient("smtp.Outlook.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("jackcare666@gmail.com", "Superdog123");

            // Compose the email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("jackcare666@gmail.com");
            message.To.Add("jackcare666@gmail.com");
            message.Subject = "JackCare is now online";
            message.Body = Update;

            // Send the email message
            client.Send(message);
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
            AutoSize = true;
            Width = 7 * question.Length;
            Height = 175;

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
            yesButton.Top = 80;
            yesButton.Click += yesButton_Click;
            yesButton.ForeColor = Color.FromArgb(113, 245, 196);
            yesButton.BackColor = Color.FromArgb(6, 125, 81);
            Controls.Add(yesButton);

            // Add a button
            noButton = new Button();
            noButton.Text = "No";
            noButton.Left = 150;
            noButton.Top = 80;
            noButton.Click += noButton_Click;
            noButton.BackColor = Color.FromArgb(94, 23, 13);
            noButton.ForeColor = Color.FromArgb(245, 200, 193);
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
            AutoSize = true;
            Width = 7 * question.Length;
            Height = 175;

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
            nextButton.Text = "Next Question";
            nextButton.Left = 100;
            nextButton.Top = 80;
            nextButton.AutoSize = true;
            nextButton.ForeColor = Color.FromArgb(156, 255, 239);
            nextButton.BackColor = Color.FromArgb(27, 130, 113);
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
