namespace ATEMkey.Media
{
    using ATEMkey.Callbacks;
    using ATEMkey.Exceptions;
    using BMDSwitcherAPI;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Upload : LockCallbackBase
    {
        private IBMDSwitcherStills stills;
        private SynchronizationContext context;
        private MediaPool mediaPool;
        private IBMDSwitcherLockCallback lockCallback;
        private IBMDSwitcherFrame frame;
        private uint uploadSlot;
        private string name;
        private Status currentStatus;

        private enum Status
        {
            NotStarted,
            Started,
            Completed,
        }

        public Upload(MediaPool mediaPool)
        {
            context = SynchronizationContext.Current;

            this.mediaPool = mediaPool;
            stills = mediaPool.GetStills();
        }

        public bool Execute(string fileName, uint index)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException($"Unable to find {fileName}");
            name = Path.GetFileNameWithoutExtension(fileName);
            frame = GetFrame(fileName);
            uploadSlot = index;
            currentStatus = Status.Started;
            lockCallback = (IBMDSwitcherLockCallback)new LockCallback(this);
            stills.Lock(lockCallback);
            return true;
        }

        public override void LockCallback()
        {
            context.Post(delegate (object dummy)
            {
                IBMDSwitcherStillsCallback callback = (IBMDSwitcherStillsCallback)new StillsCallback(this);
                stills.AddCallback(callback);
                stills.Upload(uploadSlot, name, frame);
            }, null);
        }

        public override void TransferCompleted()
        {
            context.Post(delegate (object dummy)
            {
                stills.Unlock(this.lockCallback);
                currentStatus = Status.Completed;
            }, null);
        }

        public bool InProgress()
        {
            return currentStatus == Status.Started;
        }

        public int GetProgress()
        {
            if (currentStatus == Status.NotStarted)
            {
                return 0;
            }
            if (currentStatus == Status.Completed)
            {
                return 100;
            }

            double progress;
            stills.GetProgress(out progress);
            return (int)Math.Round(progress * 100.0);
        }

        protected IBMDSwitcherFrame GetFrame(string filename)
        {
            IBMDSwitcherMediaPool switcherMediaPool = mediaPool.switcherMediaPool;
            IBMDSwitcherFrame frame;
            switcherMediaPool.CreateFrame(_BMDSwitcherPixelFormat.bmdSwitcherPixelFormat8BitARGB, mediaPool.VideoWidth(), mediaPool.VideoHeight(), out frame);
            IntPtr buffer;
            frame.GetBytes(out buffer);
            byte[] source = this.ConvertImage(filename);
            Marshal.Copy(source, 0, buffer, source.Length);
            return frame;
        }

        protected byte[] ConvertImage(string filename)
        {
            try
            {
                Bitmap image = new Bitmap(filename);

                if (image.Width != mediaPool.VideoWidth() || image.Height != mediaPool.VideoHeight())
                {
                    image = Rescale(image, mediaPool.VideoWidth(), mediaPool.VideoHeight());
                    //throw new ATEMSwitcherUnsupportedRes(String.Format("Image is {0}x{1} it needs to be the same resolution as the switcher", image.Width.ToString(), image.Height.ToString()));
                }

                byte[] numArray = new byte[image.Width * image.Height * 4];
                for (int index1 = 0; index1 < image.Width * image.Height; index1++)
                {
                    Color pixel = this.GetPixel(image, index1);
                    int index2 = index1 * 4;
                    numArray[index2] = pixel.B;
                    numArray[index2 + 1] = pixel.G;
                    numArray[index2 + 2] = pixel.R;
                    numArray[index2 + 3] = pixel.A;
                }
                return numArray;
            }
            catch (Exception ex)
            {
                throw new ATEMSwitcherImageException(ex.Message);
            }
        }

        private Bitmap Rescale(Bitmap image, uint width, uint height)
        {
            var brush = new SolidBrush(Color.Black);
            float scale = Math.Min((float)width / image.Width, (float)height / image.Height);

            var bmp = new Bitmap((int)width, (int)height);
            var graph = Graphics.FromImage(bmp);

            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(image, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);
            bmp.Save("C:\\Private\\Kirken\\Media Team\\test.jpg", ImageFormat.Jpeg);
            return bmp;
        }

        protected Color GetPixel(Bitmap image, int index)
        {
            int x = index % image.Width;
            int y = (index - x) / image.Width;
            return image.GetPixel(x, y);
        }
    }
}
