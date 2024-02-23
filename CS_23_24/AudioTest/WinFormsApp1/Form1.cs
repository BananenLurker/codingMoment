using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace WinFormsApp1
{
    public class MainForm : Form
    {
        private WaveOut waveOut;
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        public MainForm()
        {
            var buttonPlay = new Button();
            buttonPlay.Text = "Play";
            buttonPlay.Click += OnButtonPlayClick;
            this.Controls.Add(buttonPlay);
        }

        private void OnButtonPlayClick(object o, EventArgs ea)
        {
            if (waveOut == null)
            {
                WaveFileReader reader = new WaveFileReader(@"E:\LocalRepo\CS_23_24\introProject\AudioTest\WinFormsApp1\resources\nope.wav");
                LoopStream loop = new LoopStream(reader);
                waveOut = new WaveOut();
                waveOut.Init(loop);
                waveOut.Play();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }

        private void OnPlaybackStopped(object o, StoppedEventArgs ea)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }
    }

    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        public bool EnableLooping { get; set; }

        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        public override long Length
        {
            get { return sourceStream.Length; }
        }

        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        break;
                    }
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}