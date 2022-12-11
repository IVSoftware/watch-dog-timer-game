using System.Drawing.Text;
using System.Reflection;
using System.Linq;

namespace watch_dog_timer_game
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _origPos = buttonDog.Location;
            _origColor= buttonDog.BackColor;
        }
        WatchDogTimer WatchDog { get; } = new WatchDogTimer();

        Color[] _colors { get; } = 
            Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(_=>Color.FromKnownColor(_))
                .ToArray();
        Random _rando = new Random();
        Point _origPos;
        Color _origColor;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            aboutToolStripMenuItem.Click += (sender, e) =>
                MessageBox.Show(
@"IVSoftware, LLC

* No animals were harmed in the making of this game."
                );
            this.textBox1.Text = "The mouse has 1 second to kik the dog (again).";
            // For the sake of simplicity, the TTF is copied to output directory...
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "font", "guidedog.ttf");

            // ... and loaded here.
            privateFontCollection.AddFontFile(path);

            var fontFamily = privateFontCollection.Families[0];
            buttonDog.Font = new Font(fontFamily, 12F);
            buttonDog.UseCompatibleTextRendering = true;
            buttonDog.Text = "\uE803";
            buttonDog.Click += kickTheDog;
        }
        int _count = 0;
        private void kickTheDog(object? sender, EventArgs e)
        {
            var xPos = 
                _rando.Next(2, 
                ClientRectangle.Width - buttonDog.Width);
            var yPos = 
                _rando.Next(100, 
                ClientRectangle.Height - buttonDog.Height);
            BeginInvoke(() => 
            {
                Text = $"KikTheDog {++_count}";
                buttonDog.BackColor = _colors[_rando.Next(0, _colors.Length)];
                buttonDog.Location = new Point(xPos, yPos);
                WatchDog.ThrowBone(()=>showMessage());
            });
        }

        private void showMessage()
        {
            buttonDog.Enabled = false;
            BeginInvoke(() =>
            {
                if(DialogResult.Yes.Equals(MessageBox.Show(
                    this,
$@"You scored {_count}

Play again?",
                    "Game Over",
                    MessageBoxButtons.YesNo
                )))
                {
                    replay();
                }
                else Application.Exit(); 
            });
        }

        private void replay()
        {
            Text = $"KikTheDog";
            _count = 0;
            buttonDog.Location = _origPos;
            buttonDog.BackColor = _origColor;
            buttonDog.Enabled = true;
        }

        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                privateFontCollection.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    class WatchDogTimer
    {
        int _wdtCount = 0;
        public void ThrowBone(Action action)
        {
            _wdtCount++;
            var capturedCount = _wdtCount;
            Task
                .Delay(TimeSpan.FromMilliseconds(1000))
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    // If the 'captured' localCount has not changed after waiting 3 seconds
                    // it indicates that no new selections have been made in that time.        
                    if (capturedCount.Equals(_wdtCount))
                    {
                        action();
                    }
                });
        }
    }
}