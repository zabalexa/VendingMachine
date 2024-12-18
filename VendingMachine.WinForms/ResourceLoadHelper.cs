using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachine.WinForms.Properties;

namespace VendingMachine.WinForms
{
    public static class ResourceLoadHelper
    {
        private static Dictionary<DrinkType, string> _namePairsCache = new Dictionary<DrinkType, string>();
        private static Dictionary<string, object> _resourcePairsCache = new Dictionary<string, object>();
        private static Dictionary<int, Cursor> _coinCursorCache = new Dictionary<int, Cursor>();

        public static Image GetImage(string name)
        {
            if (!_resourcePairsCache.ContainsKey(name))
            {
                object resourceImage = Resources.ResourceManager.GetObject(name);
                _resourcePairsCache.Add(name, (resourceImage != null) && (resourceImage is Image) ? resourceImage as Image : null);
            }
            return _resourcePairsCache[name] as Image;
        }

        public static Image GetBlankImage(Size size, Color color)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(color.IsEmpty ? Color.Transparent : color);
            }
            using (MemoryStream mem = new MemoryStream())
            {
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                bmp.Dispose();
                return Image.FromStream(mem);
            }
        }

        public static Cursor GetCoinCursor(Size size, Color fillColor, Color drawColor, Font font, int value)
        {
            if (!_coinCursorCache.ContainsKey(value))
            {
                Rectangle r = new Rectangle(0, 0, size.Width, size.Height);
                Bitmap b = new Bitmap(r.Width, r.Height);
                r.Inflate(-1, -1);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.FillEllipse(new SolidBrush(fillColor), r);
                    g.DrawEllipse(new Pen(drawColor), r);
                    string s = value.ToString();
                    r.Inflate(s.Length > 1 ? 0 : -6, -4);
                    Font f = new Font(font.FontFamily, 12, font.Style | FontStyle.Bold);
                    g.DrawString(s, f, new SolidBrush(drawColor), r);
                }
                b.MakeTransparent(Color.White);
                _coinCursorCache[value] = new Cursor(b.GetHicon());
            }
            return _coinCursorCache[value];
        }

        public static Cursor GetPutCoinCursor(Size size, Color fillColor, Color drawColor)
        {
            if (!_coinCursorCache.ContainsKey(0))
            {
                Rectangle r = new Rectangle(0, 0, size.Width, size.Height);
                Bitmap b = new Bitmap(r.Width, r.Height);
                r.Inflate(-1, -1);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.FillRectangle(new SolidBrush(fillColor), r);
                    g.DrawRectangle(new Pen(drawColor), r);
                }
                b.MakeTransparent(Color.White);
                _coinCursorCache[0] = new Cursor(b.GetHicon());
            }
            return _coinCursorCache[0];
        }

        public static string GetImageName(DrinkType kind)
        {
            if (!_namePairsCache.ContainsKey(kind))
            {
                object resourceImage = Resources.ResourceManager.GetObject(kind == DrinkType.Tea ? "TeaImageName" : kind == DrinkType.Coffee ? "CoffeeImageName" : kind == DrinkType.Juice ? "JuiceImageName" : "ProgressImageName");
                _namePairsCache.Add(kind, (resourceImage != null) && (resourceImage is string) ? resourceImage as string : string.Empty);
            }
            return _namePairsCache[kind] as string;
        }

        public static string GetLocalString(string name)
        {
            if (!_resourcePairsCache.ContainsKey(name))
            {
                object resourceImage = Resources.ResourceManager.GetObject(string.Format("_{0}_{1}", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, name));
                _resourcePairsCache.Add(name, (resourceImage != null) && (resourceImage is string) ? resourceImage as string : string.Empty);
            }
            return _resourcePairsCache[name] as string;
        }

        public static string GetLocalName(Dictionary<string, string> locals, string localization = null)
        {
            string local = localization ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            return locals?.ContainsKey(local) ?? false ? locals[local] : null;
        }
    }
}
