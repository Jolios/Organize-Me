using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace Organize_Me
{
    class VoiceRecognition
    {
        private Form2 f;
        SpeechRecognitionEngine RecognitionEngine = new SpeechRecognitionEngine();
        public VoiceRecognition(Form2 f)
        {
            this.f = f;

            if (f.isListening == false)
            {
                Choices commands = new Choices(new string[] { "dashboard", "calendar", "tasks", "family", "profile" });
                GrammarBuilder grammarBuilder = new GrammarBuilder(commands);
                grammarBuilder.Culture = new System.Globalization.CultureInfo("en-GB");
                Grammar grammar = new Grammar(grammarBuilder);
                RecognitionEngine.LoadGrammarAsync(grammar);
                RecognitionEngine.SetInputToDefaultAudioDevice();
                RecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                RecognitionEngine.SpeechRecognized += RecognitionEngine_SpeechRecognized;
            }
            else RecognitionEngine.RecognizeAsyncStop();
        }

        private void RecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "dashboard": 
                    f.btn_Dashboard_Click(new object(), new EventArgs());
                    break;
                case "calendar":
                    f.btn_Calendar_Click(new object(), new EventArgs());
                    break;
                case "tasks":
                    f.btn_Tasks_Click(new object(), new EventArgs());
                    break;
                case "family":
                    f.btn_Family_Click(new object(), new EventArgs());
                    break;
                case "profile":
                    f.btn_Profile_Click(new object(), new EventArgs());
                    break;
            }
        }
    }
}
