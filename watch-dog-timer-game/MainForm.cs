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
            aboutToolStripMenuItem.Click += (sender, e) =>
                MessageBox.Show(
                    "* No animals were harmed in the making of this game."
                );

        }
        Color[] _colors { get; } = 
            Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(_=>Color.FromKnownColor(_))
                .ToArray();
        Random _rando = new Random();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // For the sake of simplicity, the TTF is copied to output directory...
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "font", "guidedog.ttf");

            // ... and loaded here.
            privateFontCollection.AddFontFile(path);

            var fontFamily = privateFontCollection.Families[0];
            buttonDog.Font = new Font(fontFamily, 12F);
            buttonDog.UseCompatibleTextRendering = true;
            buttonDog.Text = "\uE803";
            buttonDog.Click += (sender, e) =>
            {
                buttonDog.BackColor = _colors[_rando.Next(0, _colors.Length)];
            };
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
}